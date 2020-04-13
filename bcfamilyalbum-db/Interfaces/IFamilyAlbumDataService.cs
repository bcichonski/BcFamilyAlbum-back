using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bcfamilyalbum_db.Interfaces
{
    public interface IFamilyAlbumDataService
    {
        Task EnsureReadiness();
        Task MarkFileAsDeleted(string relativePath);
        List<string> GetDeletedFiles();

        Task SaveThatFileWasMoved(string from, string to);
        List<string> GetMovedFilesOriginalLocations();
    }
}
