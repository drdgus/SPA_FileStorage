using Microsoft.EntityFrameworkCore;
using SPA_Sibintek_NET6.API.Core.DAL.Entities;
using SPA_Sibintek_NET6.API.Core.Models;

namespace SPA_Sibintek_NET6.API.Core.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DbContext _dbContext;

        public FileRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UserFile> GetFiles(string userToken)
        {
            return _dbContext.Files.Where(f => f.UserToken == userToken).Select(f => new UserFile
            {
                Id = f.Id,
                Name = f.Name,
                Hash = f.Hash,
                UploadDateTime = f.UploadDateTime,
                Data = Array.Empty<byte>()
            });
        }

        public async Task<UserFile?> GetFileContent(int id)
        {
            var file = await _dbContext.Files.SingleOrDefaultAsync(f => f.Id == id);
            if (file == null) return default;
            return new UserFile
            {
                Id = file.Id,
                Name = file.Name,
                Hash = file.Hash,
                ContentType = file.ContentType,
                UploadDateTime = file.UploadDateTime,
                Data = file.Data
            };
        }

        public Task RemoveFile(int id)
        {
            var file = _dbContext.Files.SingleOrDefault(f => f.Id == id);
            if (file == null) throw new FileNotFoundException();

            _dbContext.Files.Remove(file);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<UserFile> UploadFileAsync(UserFileEntity file)
        {
            var newFile = (await _dbContext.Files.AddAsync(file)).Entity;

            await _dbContext.SaveChangesAsync();

            return new UserFile
            {
                Id = newFile.Id,
                Name = newFile.Name,
                Hash = newFile.Hash,
                UploadDateTime = newFile.UploadDateTime,
                Data = Array.Empty<byte>()
            };
        }
    }
}
