namespace WarehouseManagement.API.Application.Dtos;

public class WarehouseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public CompanyRecord Company { get; set; }

    public record struct CompanyRecord(int Id, string Name);
}
