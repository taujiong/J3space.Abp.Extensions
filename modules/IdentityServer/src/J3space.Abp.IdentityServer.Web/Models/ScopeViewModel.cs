using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace J3space.Abp.IdentityServer.Web.Models
{
    public class ScopeViewModel
    {
        [Required]
        [HiddenInput]
        public string Name { get; set; }

        public bool Checked { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Emphasize { get; set; }

        public bool Required { get; set; }
    }

}