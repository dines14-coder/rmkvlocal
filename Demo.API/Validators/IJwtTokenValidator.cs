using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Validators
{
    public interface IJwtTokenValidator
    {
        JwtSecurityToken ValidateToken(string token);
    }
}
