using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bcfamilyalbum_back.Interfaces;
using bcfamilyalbum_back.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bcfamilyalbum_back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyAlbumController : ControllerBase
    {
        private readonly ILogger<FamilyAlbumController> _logger;

        readonly IAlbumInfoProvider _albumInfoProvider;

        public FamilyAlbumController(ILogger<FamilyAlbumController> logger, IAlbumInfoProvider albumInfoProvider)
        {
            _logger = logger;
            _albumInfoProvider = albumInfoProvider;
        }

        [HttpGet]
        public async Task<TreeItem> Get()
        {
            return await _albumInfoProvider.GetAlbumInfo();
        }
    }
}
