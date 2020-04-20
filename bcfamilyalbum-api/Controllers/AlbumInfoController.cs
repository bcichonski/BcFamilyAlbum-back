using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using bcfamilyalbum_api.Interfaces;
using bcfamilyalbum_api.Model;
using bcfamilyalbum_db.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace bcfamilyalbum_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumInfoController : ControllerBase
    {
        private readonly ILogger<AlbumInfoController> _logger;

        readonly IAlbumInfoProvider _albumInfoProvider;

        readonly IFamilyAlbumDataService _dbService;

        public AlbumInfoController(
            ILogger<AlbumInfoController> logger, 
            IAlbumInfoProvider albumInfoProvider,
            IFamilyAlbumDataService dbService)
        {
            _logger = logger;
            _albumInfoProvider = albumInfoProvider;
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<ActionResult<TreeItem>> Get()
        {
            return await _albumInfoProvider.GetAlbumInfo();
        }

        static readonly FileExtensionContentTypeProvider _contentTypeProvider = new FileExtensionContentTypeProvider();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _albumInfoProvider.GetItem(id);
            if (item != null)
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
                        EnableRangeProcessing = VideoTreeItem.IsAnInstance(item.FullPath)
                    };
                }
            }
            throw new Exception($"File {id} not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            var item = await _albumInfoProvider.DeleteItem(id);
            if (item != null)
            {
                await _dbService.EnsureReadiness();
                await _dbService.MarkFileAsDeleted(_albumInfoProvider.GetRelativePath(item.FullPath));
                return Ok();
            }
            throw new Exception($"File {id} not found.");
        }

        [HttpPost]
        public async Task<IActionResult> RotateItem(AlbumInfoControllerFrontendPostCommand command)
        {
            await _albumInfoProvider.RotateItem(command.Id);
            return Ok();
        }
    }
}
