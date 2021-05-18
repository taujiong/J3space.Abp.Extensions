using System;
using System.Threading.Tasks;
using J3space.Blogging.Posts.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace J3space.Blogging.Posts
{
    public interface IPostAppService : ICrudAppService<
        PostWithDetailDto,
        PostDto,
        Guid,
        PagedAndSortedResultRequestDto,
        PostCreateDto,
        PostUpdateDto
    >
    {
        public Task<ListResultDto<PostDto>> GetPostsByTag(string tagName);
    }
}