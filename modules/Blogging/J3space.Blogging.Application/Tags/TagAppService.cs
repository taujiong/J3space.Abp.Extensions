using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Blogging.Tags.Dto;

namespace J3space.Blogging.Tags
{
    public class TagAppService : BloggingAppService, ITagAppService
    {
        private readonly ITagRepository _tagRepository;

        public TagAppService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<List<TagListDto>> GetListAsync()
        {
            var tags = await _tagRepository.GetListAsync();
            return ObjectMapper.Map<List<Tag>, List<TagListDto>>(tags);
        }
    }
}