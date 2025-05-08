namespace Core.Domain.DTOs;
public class ApartamentResponseDTO
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? TypeRoom { get; set; }
    public float? Price { get; set; }
    public string? Description { get; set; }

    public AddressDTO? Address { get; set; }
    public List<PhotoDTO> Photos { get; set; }
}
