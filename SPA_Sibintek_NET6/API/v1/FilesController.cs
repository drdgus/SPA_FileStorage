using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SPA_Sibintek_NET6.API.Core.DAL.Entities;
using SPA_Sibintek_NET6.API.Core.DAL.Repositories;
using SPA_Sibintek_NET6.API.Core.Models;

namespace SPA_Sibintek_NET6.API.v1
{
#if !DEBUG
    [Authorize]
#endif
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilesController : ControllerBase
    {
        private const int MaxFileLength = 1024 * 1024 * 1024;

        private readonly IFileRepository _fileRepository;

        public FilesController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpGet]
        public IActionResult GetFiles()
        {
            var token = User.Identity.Name;
#if DEBUG
            token = "DEBUG";
#endif
            return Ok(_fileRepository.GetFiles(token));
        }

        [HttpGet("Download/{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = await _fileRepository.GetFileContent(id);
            if (file == default) return BadRequest("File not found.");

            return File(file.Data, file.ContentType);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                await _fileRepository.RemoveFile(id);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return BadRequest("File not found.");
            }
        }

        [HttpPost("Add/")]
        public async Task<IActionResult> AddNewFile([FromForm] IFormFile file, [FromForm] string md5, [FromForm] string contentType)
        {
            if (file.Length == 0) return BadRequest("File is empty.");
            if (file.Length > int.MaxValue) return BadRequest("File too large.");

            await using var stream = new MemoryStream((int)file.Length);
            await file.CopyToAsync(stream);

            var userFile = new UserFileEntity
            {
                UserToken = User.Identity.Name,
                Name = file.FileName,
                Hash = md5,
                ContentType = contentType,
                UploadDateTime = DateTimeOffset.Now,
                Data = stream.ToArray()
            };
#if DEBUG
            userFile.UserToken = "DEBUG";
#endif

            var createdFile = await _fileRepository.UploadFileAsync(userFile);
            return Ok(createdFile);
        }
    }
}
