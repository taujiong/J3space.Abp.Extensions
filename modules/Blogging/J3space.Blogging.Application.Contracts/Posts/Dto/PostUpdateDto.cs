using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace J3space.Blogging.Posts.Dto
{
    public class PostUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(PostConstant), nameof(PostConstant.MaxTitleLength))]
        public string Title { get; set; }

        [Required]
        [DynamicStringLength(typeof(PostConstant), nameof(PostConstant.MaxContentLength))]
        public string Content { get; set; }

        public string Tags { get; set; }

        [DynamicStringLength(typeof(PostConstant), nameof(PostConstant.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}