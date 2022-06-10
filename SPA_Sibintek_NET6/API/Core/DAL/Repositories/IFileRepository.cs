using SPA_Sibintek_NET6.API.Core.DAL.Entities;
using SPA_Sibintek_NET6.API.Core.Models;

namespace SPA_Sibintek_NET6.API.Core.DAL.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<UserFile> GetFiles(string userToken);
        Task<UserFile?> GetFileContent(int id);
        Task RemoveFile(int id);
        Task<UserFile> UploadFileAsync(UserFileEntity file);
    }
}
