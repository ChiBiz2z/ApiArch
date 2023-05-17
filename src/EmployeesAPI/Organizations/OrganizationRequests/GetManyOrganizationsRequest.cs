using EmployeesAPI.Models;

namespace EmployeesAPI.Organizations.OrganizationRequests;

public class GetManyOrganizationsRequest : PaginationPropertiesBase
{
    private readonly string? _search;

    public string? Search
    {
        get => _search;
        init => _search = value ?? string.Empty;
    }
}