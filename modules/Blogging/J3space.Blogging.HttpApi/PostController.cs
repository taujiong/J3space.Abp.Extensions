using System;
using System.Threading.Tasks;
using J3space.Blogging.Posts;
using J3space.Blogging.Posts.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConstants.RemoteServiceName)]
    [Area("blogging")]
    [Route("api/blogging/posts")]
    public class PostController : BloggingController, IPostAppService
    {
        private readonly IPostAppService _postAppService;

        public PostController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<PostWithDetailDto> GetAsync(Guid id)
        {
            return _postAppService.GetAsync(id);
        }

        [HttpGet]
        public Task<PagedResultDto<PostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _postAppService.GetListAsync(input);
        }

        [HttpPost]
        public Task<PostWithDetailDto> CreateAsync(PostCreateDto input)
        {
            return _postAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<PostWithDetailDto> UpdateAsync(Guid id, PostUpdateDto input)
        {
            return _postAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _postAppService.DeleteAsync(id);
        }
    }
}