using FinancialProject.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FinancialProject.Infrastructure.Security;

internal sealed class HttpCurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            var claimValue = httpContext.User.FindFirst("sub")?.Value
                ?? httpContext.User.FindFirst("user_id")?.Value;

            return Guid.TryParse(claimValue, out var userId) ? userId : null;
        }
    }
}
