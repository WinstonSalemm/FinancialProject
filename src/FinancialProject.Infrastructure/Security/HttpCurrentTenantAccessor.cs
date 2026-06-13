using FinancialProject.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FinancialProject.Infrastructure.Security;

internal sealed class HttpCurrentTenantAccessor : ICurrentTenantAccessor
{
    private const string OrganizationIdClaim = "org_id";
    private const string OrganizationIdHeader = "X-Organization-Id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentTenantAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? OrganizationId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return null;
            }

            var claimValue = httpContext.User.FindFirst(OrganizationIdClaim)?.Value;
            if (Guid.TryParse(claimValue, out var claimGuid))
            {
                return claimGuid;
            }

            if (httpContext.Request.Headers.TryGetValue(OrganizationIdHeader, out var headerValues)
                && Guid.TryParse(headerValues.FirstOrDefault(), out var headerGuid))
            {
                return headerGuid;
            }

            return null;
        }
    }
}
