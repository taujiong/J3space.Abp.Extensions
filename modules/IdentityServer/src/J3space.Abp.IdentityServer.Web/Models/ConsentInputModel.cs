using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace J3space.Abp.IdentityServer.Web.Models
{
    public class ConsentInputModel
    {
        public List<ScopeViewModel> IdentityScopes { get; set; }

        public List<ScopeViewModel> ApiScopes { get; set; }

        [Required] public string UserDecision { get; set; }

        public string UserName { get; set; }

        public bool RememberConsent { get; set; }

        public List<string> GetAllowedScopeNames()
        {
            var identityScopes = IdentityScopes ?? new List<ScopeViewModel>();
            var apiScopes = ApiScopes ?? new List<ScopeViewModel>();
            return identityScopes
                .Union(apiScopes)
                .Where(s => s.Checked)
                .Select(s => s.Name)
                .ToList();
        }
    }
}