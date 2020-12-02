using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Blogging.Permissions;
using J3space.Blogging.Posts.Dto;
using J3space.Blogging.Tags;
using J3space.Blogging.Tags.Dto;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging.Posts
{
    [Authorize(BloggingPermissions.Posts.Default)]
    public class PostAppService : BloggingAppService, IPostAppService
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;

        public PostAppService(
            IPostRepository postRepository,
            ITagRepository tagRepository
        )
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
        }

        public async Task<PostWithDetailDto> GetAsync(Guid id)
        {
            var post = await _postRepository.GetAsync(id);
            var postDto = ObjectMapper.Map<Post, PostWithDetailDto>(post);
            postDto.Tags = await GetTagsFromPost(post.Id);

            return postDto;
        }

        public async Task<PagedResultDto<PostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var posts = await _postRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount,
                input.Sorting ?? nameof(Post.LastModificationTime) + " desc");
            var postDtos = new List<PostDto>(ObjectMapper.Map<List<Post>, List<PostDto>>(posts));

            foreach (var postDto in postDtos)
            {
                postDto.Tags = await GetTagsFromPost(postDto.Id);
            }

            var totalCount = await _postRepository.GetCountAsync();
            return new PagedResultDto<PostDto>
            {
                TotalCount = totalCount,
                Items = postDtos
            };
        }

        [Authorize(BloggingPermissions.Posts.Create)]
        public async Task<PostWithDetailDto> CreateAsync(PostCreateDto input)
        {
            // TODO: Title 是否可以重复？
            var post = new Post(GuidGenerator.Create(), input.Title)
            {
                Description = input.Description,
                Content = input.Content
            };

            var tagList = SplitTags(input.Tags);
            await AddNewTags(tagList, post);
            var newPost = await _postRepository.InsertAsync(post);

            // Bug: 返回中不包含 tag 和 审计信息
            return ObjectMapper.Map<Post, PostWithDetailDto>(newPost);
        }

        [Authorize(BloggingPermissions.Posts.Update)]
        public async Task<PostWithDetailDto> UpdateAsync(Guid id, PostUpdateDto input)
        {
            var post = await _postRepository.GetAsync(id);

            post.SetTitle(input.Title);
            post.Content = input.Content;
            post.Description = input.Description;

            var tagList = SplitTags(input.Tags);
            await RemoveOldTags(tagList, post);
            await AddNewTags(tagList, post);
            var newPost = await _postRepository.UpdateAsync(post);

            return ObjectMapper.Map<Post, PostWithDetailDto>(newPost);
        }

        [Authorize(BloggingPermissions.Posts.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var tags = await GetTagsFromPost(id);
            await _tagRepository.DecreaseUsageCountOfTagsAsync(tags.Select(t => t.Id).ToList());
            await _postRepository.DeleteAsync(id);
        }

        public async Task<List<PostDto>> GetPostsByTag(string tagName)
        {
            var posts = await _postRepository.GetListAsync();
            var postDtos = new List<PostDto>(ObjectMapper.Map<List<Post>, List<PostDto>>(posts));

            foreach (var postDto in postDtos)
            {
                postDto.Tags = await GetTagsFromPost(postDto.Id);
            }

            var tag = tagName.IsNullOrWhiteSpace() ? null : await _tagRepository.FindByNameAsync(tagName);
            if (tag != null)
            {
                return postDtos
                    .Where(p => p.Tags.Any(t => t.Id == tag.Id))
                    .ToList();
            }

            return null;
        }

        private async Task<List<TagDto>> GetTagsFromPost(Guid postId)
        {
            var tagIds = (await _postRepository.GetAsync(postId)).Tags;
            var tags = await _tagRepository.GetListAsync(tagIds.Select(t => t.TagId));
            return ObjectMapper.Map<List<Tag>, List<TagDto>>(tags);
        }

        private List<string> SplitTags(string tags)
        {
            if (tags.IsNullOrWhiteSpace())
            {
                return new List<string>();
            }

            return new List<string>(tags.Split(",").Select(t => t.Trim()));
        }

        private async Task AddNewTags(IEnumerable<string> newTags, Post post)
        {
            var tags = await _tagRepository.GetListAsync();

            foreach (var newTag in newTags)
            {
                var tag = tags.FirstOrDefault(t => t.Name == newTag);

                if (tag == null)
                {
                    tag = await _tagRepository.InsertAsync(new Tag(GuidGenerator.Create(), newTag, 1));
                }
                else
                {
                    tag.IncreaseUsageCount();
                    tag = await _tagRepository.UpdateAsync(tag);
                }

                post.AddTag(tag.Id);
            }
        }

        private async Task RemoveOldTags(ICollection<string> newTags, Post post)
        {
            var decreaseUsageCountTagIdList = new List<Guid>();
            foreach (var oldTag in post.Tags.ToList())
            {
                var tag = await _tagRepository.GetAsync(oldTag.TagId);

                var oldTagNameInNewTags = newTags.FirstOrDefault(t => t == tag.Name);

                if (oldTagNameInNewTags == null)
                {
                    post.RemoveTag(oldTag.TagId);
                    decreaseUsageCountTagIdList.Add(oldTag.TagId);
                }
                else
                {
                    newTags.Remove(oldTagNameInNewTags);
                }
            }

            await _tagRepository.DecreaseUsageCountOfTagsAsync(decreaseUsageCountTagIdList);
        }
    }
}