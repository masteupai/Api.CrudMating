using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Threading.Tasks;
using API.Configurations.Factories;
using API.Domains.Models;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace API.Domains.Services
{
    public interface IAuthenticatedService
    {
        JwtSecurityToken Token();
    }
    
    public class AuthenticatedService : IAuthenticatedService
    {
        private readonly JwtSecurityToken _token;
        public AuthenticatedService(IHttpContextAccessor acessor)
        {
#if (DEBUG)
            var content = acessor.HttpContext.Request.Headers["X-JWT-Assertion"];

            if (string.IsNullOrWhiteSpace(content)) 
            {
                acessor.HttpContext.Request.Headers.Add("X-JWT-Assertion", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBUEkgQm9vdHN0cmFwIElkZW50aXR5IFNlcnZlciIsImlhdCI6MTU0ODM3MzM4NywiZXhwIjoxNTc5OTA5Mzg3LCJhdWQiOiJ3d3cuZXhhbXBsZS5jb20iLCJzdWIiOiJtYXRoZXVzcGFlczE5QGdtYWlsLmNvbSIsIk5hbWUiOiJNYXRoZXVzIFBhZXMiLCJQcm9maWxlIjoiTWFuYWdlciJ9.-2osLjvSuOa1J1DkV8jQZ9mBhWUiEWy4cyPIKEbUtc8");
            }
#endif      

            var header = acessor.HttpContext.Request.Headers["X-JWT-Assertion"];

            _token = new JwtSecurityTokenHandler().ReadJwtToken(header);
        }

        public JwtSecurityToken Token()
        {
            return _token;
        }
    }
}
