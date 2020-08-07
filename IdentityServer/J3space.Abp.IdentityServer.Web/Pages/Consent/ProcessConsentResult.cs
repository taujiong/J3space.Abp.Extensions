namespace J3space.Abp.IdentityServer.Web.Pages.Consent
{
    public class ProcessConsentResult
    {
        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri { get; set; }

        public bool HasValidationError => ValidationError != null;
        public string ValidationError { get; set; }
    }
}