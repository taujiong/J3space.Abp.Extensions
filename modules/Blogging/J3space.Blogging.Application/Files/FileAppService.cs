using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using J3space.Blogging.Files.Dto;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Validation;

namespace J3space.Blogging.Files
{
    public class FileAppService : BloggingAppService, IFileAppService
    {
        private readonly IBlobContainer<BloggingFileBlobContainer> _blobContainer;

        public FileAppService(IBlobContainer<BloggingFileBlobContainer> blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public async Task<RawFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return new RawFileDto
            {
                Bytes = await _blobContainer.GetAllBytesAsync(name)
            };
        }

        public async Task<FileUploadResultDto> CreateAsync(FileUploadInputDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                ThrowValidationException("Bytes of file can not be null or empty!", "Bytes");
            }

            if (input.Bytes.Length > BloggingFileConstant.MaxFileSize)
            {
                throw new UserFriendlyException(
                    $"File exceeds the maximum upload size ({BloggingFileConstant.MaxFileSizeAsMegabytes} MB)!");
            }

            var uniqueFileName = Path.GetFileNameWithoutExtension(input.Name)
                                 + "_"
                                 + GuidGenerator.Create().ToString("N")
                                 + Path.GetExtension(input.Name);

            await _blobContainer.SaveAsync(uniqueFileName, input.Bytes);

            return new FileUploadResultDto
            {
                Name = uniqueFileName,
                WebUrl = "/api/blogging/files/www/" + uniqueFileName
            };
        }

        private static void ThrowValidationException(string message, string memberName)
        {
            throw new AbpValidationException(message,
                new List<ValidationResult>
                {
                    new ValidationResult(message, new[] {memberName})
                });
        }
    }
}