using Angular_SPA_Sibintek.API.Core.DAL.Entities;
using Angular_SPA_Sibintek.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_SPA_Sibintek.API.Core.DAL.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DbContext _dbContext;

        public FileRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UserFileEntity> GetFiles(string userToken)
        {
            return _dbContext.Files.Where(f => f.UserToken == userToken);
        }

        public Task<UserFileEntity> GetFileData(int id)
        {
            return _dbContext.Files.SingleOrDefaultAsync(f => f.Id == id);
        }

        public Task RemoveFile(int id)
        {
            var file = _dbContext.Files.SingleOrDefault(f => f.Id == id);
            if (file == null) throw new FileNotFoundException();

            _dbContext.Files.Remove(file);
            return _dbContext.SaveChangesAsync();
        }

        public async Task UploadFileAsync(UserFile file)
        {
            await _dbContext.Files.AddAsync(new UserFileEntity()
            {
                Name = file.Name,
                Hash = file.Hash,
                UploadDateTime = file.UploadDateTime,
                Data = file.Data,
            });

            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string> GetCheckSum(int id)
        {
            var file = await _dbContext.Files.SingleOrDefaultAsync(f => f.Id == id);
            return file == default ? string.Empty : file.Hash;
        }
    }
}
