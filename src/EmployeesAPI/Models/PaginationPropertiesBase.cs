namespace EmployeesAPI.Models;

public class PaginationPropertiesBase
{
    private readonly int? _pageNumber;

    public int? PageNumber
    {
        get => _pageNumber;
        init => _pageNumber = value ?? 1;
    }

    private readonly int? _pageSize;

    public int? PageSize
    {
        get => _pageSize;
        init => _pageSize = value ?? 10;
    }
}