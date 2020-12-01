using System.Threading.Tasks;
using J3space.Blogging.Files.Dto;
using Volo.Abp.Application.Services;

namespace J3space.Blogging.Files
{
    public interface IFileAppService : IApplicationService
    {
        Task<RawFileDto> GetAsync(string name);
        Task<FileUploadResultDto> CreateAsync(FileUploadInputDto input);
    }
}