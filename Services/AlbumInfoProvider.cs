using bcfamilyalbum_back.Interfaces;
using bcfamilyalbum_back.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_back.Services
{
    public class AlbumInfoProvider : IAlbumInfoProvider
    {
        List<TreeItem> _albumInfo;
        string _albumRootPath;
        ILogger<AlbumInfoProvider> _logger;

        public AlbumInfoProvider([FromServices] IConfiguration config, [FromServices] ILogger<AlbumInfoProvider> logger)
        {
            _logger = logger;
            _albumRootPath = config.GetValue<string>("AppSettings:AlbumRootPath");

            if (string.IsNullOrWhiteSpace(_albumRootPath))
            {
                throw new Exception("Album directory setting is empty");
            }

            if (!Directory.Exists(_albumRootPath))
            {
                throw new Exception($"Cannot find album directory {_albumRootPath}");
            }
        }

        public async Task<List<TreeItem>> GetAlbumInfo()
        {
            if (_albumInfo == null)
            {
                await Task.Run(() => GetAlbumInfoInternal());
            }
            return _albumInfo;
        }

        private void GetAlbumInfoInternal()
        {
            _logger.LogInformation("Scanning album {_albumRootPath} structure", _albumRootPath);

            Queue<string> directoryQueue = new Queue<string>();
            directoryQueue.Enqueue(_albumRootPath);
            Dictionary<string, TreeItem> cache = new Dictionary<string, TreeItem>();
            int nextId = 0;

            while (directoryQueue.Count > 0)
            {
                var currentDir = directoryQueue.Dequeue();
                string itemName = GetDirectoryName(currentDir);
                int parentNodeId = GetParentNodeId(cache, currentDir);

                var currentNode = cache[currentDir] = new TreeItem(nextId++, parentNodeId, itemName, currentDir);
                bool errorLogged = false;

                try
                {
                    var files = Directory.GetFiles(currentDir, "*", SearchOption.TopDirectoryOnly)
                        .Select(filepath =>
                            cache[filepath] = new TreeItem(
                                nextId++,
                                currentNode.Id,
                                Path.GetFileName(filepath),
                                filepath))
                        .ToList();//force errors here                     
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error enumerating files in {currentDir}: {ex.Message}");
                    errorLogged = true;
                }

                try
                {
                    var directories = Directory.EnumerateDirectories(currentDir, "*", SearchOption.TopDirectoryOnly);
                    foreach (var dir in directories)
                    {
                        directoryQueue.Enqueue(dir);
                    }
                }
                catch (Exception ex)
                {
                    if (!errorLogged)
                    {
                        _logger.LogError(ex, $"Error enumerating subdirectories in {currentDir}: {ex.Message}");
                    }
                }
            }

            _albumInfo = cache.Values.OrderBy(v => v.Id).ToList();
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

        private string GetDirectoryName(string currentDir)
        {
            string itemName;
            if (currentDir == _albumRootPath)
            {
                itemName = "album";
            }
            else
            {
                itemName = Path.GetFileNameWithoutExtension(Path.TrimEndingDirectorySeparator(currentDir));
            }

            return itemName;
        }

        public void Invalidate()
        {
            _albumInfo = null;
        }
    }
}
