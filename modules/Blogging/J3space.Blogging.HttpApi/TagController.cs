using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Blogging.Tags;
using J3space.Blogging.Tags.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace J3space.Blogging
{
    [RemoteService(Name = BloggingRemoteServiceConstants.RemoteServiceName)]
    [Area("blogging")]
    [Route("api/blogging/tags")]
    public class TagController : BloggingController, ITagAppService
    {
        private readonly ITagAppService _tagAppService;

        public TagController(ITagAppService tagAppService)
        {
            _tagAppService = tagAppService;
        }

        [HttpGet]
        public Task<List<TagListDto>> GetListAsync()
        {
            return _tagAppService.GetListAsync();
        }
    }
}