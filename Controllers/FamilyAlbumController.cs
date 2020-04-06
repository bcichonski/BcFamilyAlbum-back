using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bcfamilyalbum_back.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bcfamilyalbum_back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FamilyAlbumController : ControllerBase
    {
        private static readonly TreeItem[] Tree = new TreeItem[]
        {
            new TreeItem(1,0,"2011"),
            new TreeItem(2,1,"may"),
            new TreeItem(3,2,"pictures"),
            new TreeItem(4,0,"2012"),
            new TreeItem(5,0,"2013"),
        };

        private readonly ILogger<FamilyAlbumController> _logger;

        public FamilyAlbumController(ILogger<FamilyAlbumController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<TreeItem> Get()
        {
            return Tree;
        }
    }
}
