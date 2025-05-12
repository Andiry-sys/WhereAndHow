using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Interfaces;

namespace Infrastructure.Service.Services;
internal class ApartamentService (IApartamentRepository repository): IApartamentService
{
    private readonly IApartamentRepository _repository = repository;

    public async Task<bool> AddAsync(ApartamentRequestDTO dto, string uploadRootPath)
    {
        var user = await _repository.GetUserByIdAsync(dto.OwnerId);
        var address = await _repository.GetAddressByIdAsync(dto.AddressId);
        if(user == null || address == null)
            return false;

        string roomId = Guid.NewGuid().ToString();

        if(!Directory.Exists(uploadRootPath))
            Directory.CreateDirectory(uploadRootPath);

        var photos = new List<Photo>();
        foreach(var file in dto.Images)
        {
            string safeFileName = Path.GetFileName(file.FileName);
            string fullPath = Path.Combine(uploadRootPath, safeFileName);

            using(var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            photos.Add(new Photo
            {
                Id = Guid.NewGuid().ToString(),
                ImagePath = Path.Combine("uploads", safeFileName).Replace("\\", "/"),                
                RoomId = roomId
            });
        }

        var apartament = new Apartament
        {
            Id = roomId,
            Name = dto.Name,
            Price = dto.Price,
            TypeRoom = dto.TypeRoom,
            AddressId = dto.AddressId,
            OwnerId = dto.OwnerId,
            Description = dto.Description,
            Photos = photos,
            Address = address,
            Owner = user
        };

        user.Apartaments.Add(apartament);

        await _repository.AddAsync(apartament);
        return true;
    }

    public async Task<List<ApartamentResponseDTO>> GetAllApartamentsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ApartamentResponseDTO> GetByIdAsync(string id)
    {
        var room = await _repository.GetByIdAsync(id);
        if(room == null)
            return new();

        return room;
    }

    public async Task<List<ApartamentResponseDTO>> SearchAsync(SearchApartamentDTO dto)
    {
        return await _repository.SearchAsync(dto);
    }
}
