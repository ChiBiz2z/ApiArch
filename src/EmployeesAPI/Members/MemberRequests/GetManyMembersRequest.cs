using EmployeesAPI.Models;

namespace EmployeesAPI.Members.MemberRequests;

public class GetManyMembersRequest : PaginationPropertiesBase
{
    public string? FullNameSearch { get; init; }

    private readonly int? _ageFilterFrom;

    public int? AgeFilterFrom
    {
        get => _ageFilterFrom;
        init => _ageFilterFrom = value ?? 18;
    }

    private readonly int? _ageFilterTo;

    public int? AgeFilterTo
    {
        get => _ageFilterTo;
        init => _ageFilterTo = value ?? 100;
    }
}