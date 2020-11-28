using AutoMapper;
using J3space.Blogging.Posts;
using J3space.Blogging.Posts.Dto;
using J3space.Blogging.Tags;
using J3space.Blogging.Tags.Dto;
using Volo.Abp.AutoMapper;

namespace J3space.Blogging
{
    public class BloggingApplicationAutoMapperProfile : Profile
    {
        public BloggingApplicationAutoMapperProfile()
        {
            CreateMap<Post, PostDto>()
                .Ignore(p => p.Tags);
            CreateMap<Post, PostWithDetailDto>()
                .Ignore(p => p.Tags);
            CreateMap<Tag, TagDto>();
        }
    }
}