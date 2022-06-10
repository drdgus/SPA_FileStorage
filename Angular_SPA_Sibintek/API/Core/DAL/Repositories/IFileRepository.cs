using Angular_SPA_Sibintek.API.Core.DAL.Entities;
using Angular_SPA_Sibintek.API.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Angular_SPA_Sibintek.API.Core.DAL.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<UserFileEntity> GetFiles(string userToken);
        Task<UserFileEntity> GetFileData(int id);
        Task RemoveFile(int id);
        Task UploadFileAsync(UserFile file);
        Task<string> GetCheckSum(int id);
    }
}
