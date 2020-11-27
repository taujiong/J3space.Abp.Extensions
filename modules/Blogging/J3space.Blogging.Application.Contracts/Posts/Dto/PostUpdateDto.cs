using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace J3space.Blogging.Posts.Dto
{
    public class PostUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxContentLength))]
        public string Content { get; set; }

        public string Tags { get; set; }

        [DynamicStringLength(typeof(PostConsts), nameof(PostConsts.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}