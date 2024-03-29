using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace J3space.Abp.Account.Web.Models
{
    public class ExternalProviderHelper
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        public IEnumerable<ExternalProviderModel> VisibleExternalProviders { get; set; }

        public ExternalProviderHelper(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }

        public async Task GetVisibleExternalProviders()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            VisibleExternalProviders = schemes
                .Where(x => !string.IsNullOrEmpty(x.DisplayName))
                .Select(x => new ExternalProviderModel
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();
        }
    }
}