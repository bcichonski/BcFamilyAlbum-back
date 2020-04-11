using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using bcfamilyalbum_back.Interfaces;
using bcfamilyalbum_back.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace bcfamilyalbum_back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumInfoController : ControllerBase
    {
        private readonly ILogger<AlbumInfoController> _logger;

        readonly IAlbumInfoProvider _albumInfoProvider;

        public AlbumInfoController(ILogger<AlbumInfoController> logger, IAlbumInfoProvider albumInfoProvider)
        {
            _logger = logger;
            _albumInfoProvider = albumInfoProvider;
        }

        [HttpGet]
        public async Task<TreeItem> Get()
        {
            return await _albumInfoProvider.GetAlbumInfo();
        }

        static readonly FileExtensionContentTypeProvider _contentTypeProvider = new FileExtensionContentTypeProvider();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _albumInfoProvider.GetItem(int.Parse(id));
            if(item != null)
            {
                if (item is FileTreeItem)
                {
                    string contentType;
                    if (!_contentTypeProvider.TryGetContentType(Path.GetFileName(item.FullPath), out contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    return new PhysicalFileResult(item.FullPath, contentType)
                    {
                        EnableRangeProcessing = IsVideoFile(item.FullPath)
                    };
                }
            }
            throw new Exception($"File {id} not found.");
        }

        static readonly string[] _VideoFormats = new string[]
        {
            ".mp4",
            ".avi",
            ".mkv"
        };

        private bool IsVideoFile(string fullPath)
        {
            var ext = Path.GetExtension(fullPath).ToLowerInvariant();
            return _VideoFormats.Any(v => v == ext);
        }
    }
}
