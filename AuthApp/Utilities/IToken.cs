using System.Collections.Generic;
using System.Security.Claims;

namespace AuthApp.Utilities
{
    public interface IToken
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}
