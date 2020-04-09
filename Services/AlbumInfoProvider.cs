using bcfamilyalbum_back.Interfaces;
using bcfamilyalbum_back.Model;
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

namespace bcfamilyalbum_back.Services
{
    public class AlbumInfoProvider : IAlbumInfoProvider
    {
        SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        TreeItem _albumInfoRoot;
        ConcurrentDictionary<int, TreeItem> _cache;
        string _albumRootPath;
        ILogger<AlbumInfoProvider> _logger;

        public AlbumInfoProvider([FromServices] IConfiguration config, [FromServices] ILogger<AlbumInfoProvider> logger)
        {
            _logger = logger;
            _albumRootPath = config.GetValue<string>("AppSettings:AlbumRootPath");
            _cache = new ConcurrentDictionary<int, TreeItem>();

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
            int nextId = 0;

            var tempRoot = new DirectoryTreeItem(nextId++, null, "the album", _albumRootPath);
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
                        var id = nextId++;
                        _cache[id] = new FileTreeItem(
                            id,
                            currentNode,
                            Path.GetFileName(filepath),
                            filepath);
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
                        var childNode = new DirectoryTreeItem(nextId++, currentNode, dir);
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
            }

            _albumInfoRoot = tempRoot;
        }

        private int GetParentNodeId(Dictionary<string, TreeItem> cache, string currentDir)
        {
            var parentNodeId = -1;
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
    }
}
