using Microsoft.AspNetCore.Authorization;

namespace Services.Authentication
{
    public class HasApiKeyAttribute : AuthorizeAttribute
    {
        public HasApiKeyAttribute()
        {
            AuthenticationSchemes = ApiKeyAuthSchemeOptions.Scheme;
        }
    }
}
