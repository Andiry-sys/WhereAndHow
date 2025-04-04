using Core.Domain.Models;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static void Seed(UserContext userContext)
    {
        UserContext _userContext = userContext;
        _userContext.Address.AddRange(
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "4"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "5"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "6"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "7"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "8"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "9"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "11"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 1 Травня",
                NumberHouse = "13"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "1"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "4"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "4A"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "5"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "5A"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "8"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "9"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "11"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "13"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "15"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "17"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Вижниця",
                Street = "вулиця 8 Березня",
                NumberHouse = "23"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця 16-го Липня",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця 16-го Липня",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця 16-го Липня",
                NumberHouse = "6"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця 16-го Липня",
                NumberHouse = "11"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Весняна",
                NumberHouse = "1"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Гоголя",
                NumberHouse = "1"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Гоголя",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Гоголя",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Гоголя",
                NumberHouse = "3A"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Гоголя",
                NumberHouse = "4"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Івасюка Володимира",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Івасюка Володимира",
                NumberHouse = "12"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Івасюка Володимира",
                NumberHouse = "13"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Івасюка Володимира",
                NumberHouse = "19"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "1"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "4"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "5"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "6"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Кіцмань",
                Street = "вулиця Кармелюка",
                NumberHouse = "7"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "1"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "2"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "3"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "4"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "5"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "6"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "7"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "8"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "9"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "10"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "11"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "12"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "13"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "14"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "15"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "16"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "17"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "18"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "19"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "20"
            },
            new Address()
            {
                Id = Guid.NewGuid().ToString(),
                City = "Сторожинець",
                Street = "вулиця Вишнева",
                NumberHouse = "21"
            }
        );
        _userContext.SaveChanges();
    }
}
