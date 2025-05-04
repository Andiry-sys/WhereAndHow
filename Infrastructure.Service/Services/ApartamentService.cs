using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Infrastructure.Persistence.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Service.Services;
internal class ApartamentService (IApartamentRepository repository): IApartamentService
{
    private readonly IApartamentRepository _repository = repository;

    public async Task<bool> AddAsync(ApartamentDTO dto, string uploadRootPath)
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

        user.ApartamentId = roomId;
        user.Apartaments.Add(apartament);

        await _repository.AddAsync(apartament);
        return true;
    }

    public async Task<List<object>> GetAllAsync(HttpRequest request)
    {
        var list = await _repository.GetAllAsync();
        string baseUrl = $"{request.Scheme}://{request.Host}";

        return list.Select(a => new
        {
            a.Id,
            ApartamentName = a.Name,
            ApartamentTypeRoom = a.TypeRoom,
            ApartamentPrice = a.Price,
            AddressStreet = a.Address?.Street,
            AddressCity = a.Address?.City,
            AddressNumberHouse = a.Address?.NumberHouse,
            a.Description,
            Photos = a.Photos.Select(p => new
            {
                PhotoImagePath = $"{baseUrl}/{p.ImagePath.Replace("\\", "/")}"
            })
        }).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(string id, HttpRequest request)
    {
        var room = await _repository.GetByIdAsync(id);
        if(room == null)
            return null;

        string baseUrl = $"{request.Scheme}://{request.Host}";

        return new
        {
            room.Id,
            ApartamentName = room.Name,
            ApartamentTypeRoom = room.TypeRoom,
            ApartamentPrice = room.Price,
            AddressStreet = room.Address?.Street,
            AddressCity = room.Address?.City,
            AddressNumberHouse = room.Address?.NumberHouse,
            room.Description,
            Photos = room.Photos.Select(p => new
            {
                PhotoImagePath = $"{baseUrl}/{p.ImagePath.Replace("\\", "/")}"
            })
        };
    }

    public async Task<List<Apartament>> SearchAsync(SearchApartamentDTO dto)
    {
        return await _repository.SearchAsync(dto);
    }
}
