using bcfamilyalbum_db.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task EnsureReadiness()
        {
            using (var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                await dbContext.EnsureDatabaseIsUpgraded();
            }
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
                await dbContext.DeletedFiles.AddAsync(new DeletedFileInfo(relativePath)
                {
                    RemovalTimestamp = DateTime.Now
                });
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveThatFileWasMoved(string from, string to)
        {
            using (var dbContext = new FamilyAlbumDbContext(_albumDbPath))
            {
                await dbContext.MovedFiles.AddAsync(new MovedFileInfo(from, to)
                {
                    MovingTimestamp = DateTime.Now
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
