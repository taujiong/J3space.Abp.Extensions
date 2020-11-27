using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace J3space.Blogging.Tags.Dto
{
    public class TagUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(TagConsts), nameof(TagConsts.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(TagConsts), nameof(TagConsts.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}