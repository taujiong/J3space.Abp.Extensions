using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace J3space.Blogging.Tags.Dto
{
    public class TagCreateDto
    {
        [Required]
        [DynamicStringLength(typeof(TagConstant), nameof(TagConstant.MaxNameLength))]
        public string Name { get; set; }

        [Required]
        [DynamicStringLength(typeof(TagConstant), nameof(TagConstant.MaxDescriptionLength))]
        public string Description { get; set; }
    }
}