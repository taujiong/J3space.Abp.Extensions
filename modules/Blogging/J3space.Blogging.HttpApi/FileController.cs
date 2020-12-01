using System.IO;
using System.Threading.Tasks;
using J3space.Blogging.Files;
using J3space.Blogging.Files.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Http;

namespace J3space.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConstants.RemoteServiceName)]
    [Area("blogging")]
    [Route("api/blogging/files")]
    public class FileController : BloggingController
    {
        private readonly IFileAppService _fileAppService;

        public FileController(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpGet]
        [Route("{name}")]
        public Task<RawFileDto> GetAsync(string name)
        {
            return _fileAppService.GetAsync(name);
        }

        [HttpGet]
        [Route("www/{name}")]
        public async Task<FileResult> GetForWebAsync(string name)
        {
            var file = await _fileAppService.GetAsync(name);
            return File(
                file.Bytes,
                MimeTypes.GetByExtension(Path.GetExtension(name))
            );
        }

        [HttpPost]
        public async Task<FileUploadResultDto> CreateAsync(IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var result = await _fileAppService.CreateAsync(new FileUploadInputDto
            {
                Name = file.FileName,
                Bytes = ms.ToArray()
            });

            return result;
        }
    }
}