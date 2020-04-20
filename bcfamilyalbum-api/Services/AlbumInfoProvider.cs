using bcfamilyalbum_api.Interfaces;
using bcfamilyalbum_api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bcfamilyalbum_api.Services
{
    public class AlbumInfoProvider : IAlbumInfoProvider
    {
        const string AlbumDbFileName = "album.db";
        const string RemovedFilesDirectory = "trash";

        SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        TreeItem _albumInfoRoot;
        ConcurrentDictionary<string, TreeItem> _cache;
        string _albumRootPath;
        ILogger<AlbumInfoProvider> _logger;

        public string AlbumDbPath => Path.Combine(_albumRootPath, AlbumDbFileName);

        public AlbumInfoProvider([FromServices] IConfiguration config, [FromServices] ILogger<AlbumInfoProvider> logger)
        {
            _logger = logger;
            _albumRootPath = config.GetValue<string>("AppSettings:AlbumRootPath");
            _cache = new ConcurrentDictionary<string, TreeItem>();

            if (string.IsNullOrWhiteSpace(_albumRootPath))
            {
                throw new Exception("Album directory setting is empty");
            }

            if (!Directory.Exists(_albumRootPath))
            {
                throw new Exception($"Cannot find album directory {_albumRootPath}");
            }
        }

        public async Task<TreeItem> GetAlbumInfo()
        {
            if (_albumInfoRoot == null)
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (_albumInfoRoot == null)
                    {
                        await Task.Run(() => GetAlbumInfoInternal());
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return _albumInfoRoot;
        }

        private void GetAlbumInfoInternal()
        {
            _logger.LogInformation("Scanning album {_albumRootPath} structure", _albumRootPath);
            _cache.Clear();
            Queue<TreeItem> directoryQueue = new Queue<TreeItem>();

            var tempRoot = new DirectoryTreeItem("\\", null, "the album", _albumRootPath);
            directoryQueue.Enqueue(tempRoot);
            _cache[tempRoot.Id] = tempRoot;

            while (directoryQueue.Count > 0)
            {
                var currentNode = directoryQueue.Dequeue();
                bool errorLogged = false;
                try
                {
                    var files = Directory.GetFiles(currentNode.FullPath, "*", SearchOption.TopDirectoryOnly);
                    foreach (var filepath in files)
                    {                        
                        if(MediaFileTreeItem.IsAnInstance(filepath))
                        {
                            var relpath = Path.GetRelativePath(_albumRootPath, filepath);
                            var item = MediaFileTreeItem.GetNew(
                                relpath,
                                currentNode,
                                Path.GetFileName(filepath),
                                filepath);
                            _cache[item.Id] = item;
                        }                  
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error enumerating files in {currentNode.FullPath}: {ex.Message}");
                    errorLogged = true;
                }

                try
                {
                    var directories = Directory.EnumerateDirectories(currentNode.FullPath, "*", SearchOption.TopDirectoryOnly);
                    foreach (var dir in directories)
                    {
                        var relpath = Path.GetRelativePath(_albumRootPath, dir);
                        var childNode = new DirectoryTreeItem(relpath, currentNode, dir);
                        _cache[childNode.Id] = childNode;
                        directoryQueue.Enqueue(childNode);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorLogged)
                    {
                        _logger.LogError(ex, $"Error enumerating subdirectories in {currentNode.FullPath}: {ex.Message}");
                    }
                }

                currentNode.SortChildren();
            }

            CutTheTree(tempRoot);

            _albumInfoRoot = tempRoot;
        }

        private void CutTheTree(TreeItem node)
        {
            var childrenCount = node.Children?.Count ?? 0;
            if (childrenCount > 0)
            {
                foreach (var child in node.Children.ToList())
                {
                    if (child is DirectoryTreeItem)
                    {
                        CutTheTree(child);
                    }
                }

                childrenCount = node.Children?.Count ?? 0;
            }

            if (childrenCount == 0 && node is DirectoryTreeItem)
            {
                node.Parent?.RemoveChild(node);
            }
        }

        private string GetParentNodeId(Dictionary<string, TreeItem> cache, string currentDir)
        {
            var parentNodeId = "";
            if (currentDir != _albumRootPath)
            {
                var parentDir = Path.GetFullPath(Path.Combine(currentDir, ".."));
                if (cache.TryGetValue(parentDir, out TreeItem parentNode))
                {
                    parentNodeId = parentNode.Id;
                }
                else
                {
                    _logger.LogWarning("Cannot find parent node for {currentDir}", currentDir);
                }
            }

            return parentNodeId;
        }



        public void Invalidate()
        {
            _albumInfoRoot = null;
            _cache = null;
        }

        public async Task<TreeItem> GetItem(string id)
        {
            await GetAlbumInfo();

            if (_cache.TryGetValue(id, out TreeItem node))
            {
                return node;
            }
            return null;
        }

        public string GetRelativePath(string path)
        {
            return Path.GetRelativePath(_albumRootPath, path);
        }

        public async Task<TreeItem> DeleteItem(string id)
        {
            var node = await GetItem(id);

            if (node != null)
            {
                node.MoveTo(Path.Combine(_albumRootPath, RemovedFilesDirectory));
                return node;
            }

            return null;
        }

        public async Task RotateItem(string id)
        {
            var node = await GetItem(id);

            if (node != null)
            {
                await node.Rotate();
            } else
            {
                throw new Exception($"Item not found. Id: {id}");
            }
        }
    }
}
