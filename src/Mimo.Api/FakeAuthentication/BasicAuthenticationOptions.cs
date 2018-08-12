using Microsoft.AspNetCore.Authentication;

namespace Mimo.Api.FakeAuthentication
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
