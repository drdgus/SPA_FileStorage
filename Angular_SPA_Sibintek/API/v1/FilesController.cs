using Angular_SPA_Sibintek.API.Core.DAL.Repositories;
using Angular_SPA_Sibintek.API.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Angular_SPA_Sibintek.API.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        public FilesController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var token = User.Identity.Name;

            return Ok(_fileRepository.GetFiles(token).Select(f => new UserFile
            {

                Name = f.Name,
                Hash = f.Hash,
                UploadDateTime = f.UploadDateTime,
                Data = Array.Empty<byte>()
            }));
        }

        [HttpGet("File/{id}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var file = await _fileRepository.GetFileData(id);
            if (file == default) return BadRequest("File not found.");

            var result = File(file.Data, "application/octet-stream");
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Remove(int id)
        {
            try
            {
                _fileRepository.RemoveFile(id);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return BadRequest("File not found.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(IFormFile file, string md5)
        {
            if (file.Length == 0) return BadRequest("File is empty.");
            if (file.Length > 1024 * 1024 * 1024) return BadRequest("File too large.");

            await using var stream = new MemoryStream((int)file.Length);
            await file.CopyToAsync(stream);

            var userFile = new UserFile
            {
                Name = file.FileName,
                Hash = md5,
                UploadDateTime = DateTimeOffset.Now,
                Data = stream.ToArray()
            };

            //userFile.Hash = GetMD5Hash(userFile.Data);

            await _fileRepository.UploadFileAsync(userFile).ConfigureAwait(false);
            return Ok();
        }

        private string GetMD5Hash(byte[] bytes)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            var hashBytes = md5.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
