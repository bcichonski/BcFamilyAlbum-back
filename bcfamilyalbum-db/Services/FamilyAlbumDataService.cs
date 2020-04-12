using bcfamilyalbum_db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bcfamilyalbum_db.Services
{
    public class FamilyAlbumDataService : IFamilyAlbumDataService
    {
        protected string _albumDbPath;

        public FamilyAlbumDataService(string dbPath)
        {
            _albumDbPath = dbPath;
        }

        public List<string> GetDeletedFiles()
        {
            using (var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                return dbContext.DeletedFiles.Select(f => f.RelativePath).ToList();
            }
        }

        public List<string> GetMovedFilesOriginalLocations()
        {
            using (var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                return dbContext.MovedFiles.Select(f => f.OriginalRelativePath).ToList();
            }
        }

        public async Task MarkFileAsDeleted(string relativePath)
        {
            using(var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                await dbContext.AddAsync(new DeletedFileInfo(relativePath));
            }
        }

        public async Task SaveThatFileWasMoved(string from, string to)
        {
            using (var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                await dbContext.AddAsync(new MovedFileInfo(from, to));
            }
        }
    }
}
