using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace CarAndAll.Server.Data
{
    public class DbSeeding
    {
        public static async Task SeedDatabase(CarAndAllContext context, UserManager<Gebruiker> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            if(await roleManager.FindByNameAsync("Particulier") == null) {
                var rollen = new List<IdentityRole>
                {
                    new IdentityRole { Id = "1", Name = "Particulier", NormalizedName = "PARTICULIER" },
                    new IdentityRole { Id = "2", Name = "Zakelijk", NormalizedName = "ZAKELIJK" },
                    new IdentityRole { Id = "3", Name = "Wagenparkbeheerder", NormalizedName = "WAGENPARKBEHEERDER" },
                    new IdentityRole { Id = "4", Name = "BackofficeMedewerker", NormalizedName = "BACKOFFICEMEDEWERKER" },
                    new IdentityRole { Id = "5", Name = "FrontofficeMedewerker", NormalizedName = "FRONTOFFICEMEDEWERKER" }
                };

                foreach(var rol in rollen) {
                    await roleManager.CreateAsync(rol);
                }
            }

            if(await userManager.FindByEmailAsync("scott@caa.nl") == null) {
                var medewerker = new Medewerker { PersoneelsNummer = 001, Naam = "Scott", Email = "scott@caa.nl", UserName = "scott@caa.nl", WachtwoordBijgewerktDatum = DateTime.UtcNow.Date};

                var result = await userManager.CreateAsync(medewerker, "Test123!");
                if(result.Succeeded) {
                    await userManager.AddToRolesAsync(medewerker, new List<string> { "BackofficeMedewerker", "FrontofficeMedewerker" });
                }

                if (!result.Succeeded)
                {
                    Console.WriteLine("Error creating user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
                    }
                }
            }

            if(!context.Voertuigen.Any()) {
                var random = new Random();
                List<Voertuig> data;

                data = new List<Voertuig>{
                    new Voertuig { Kenteken = "AB-123-CD", Soort = "auto", Merk = "Toyota", Type = "Corolla", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "EF-456-GH", Soort = "auto", Merk = "Ford", Type = "Focus", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "IJ-789-KL", Soort = "auto", Merk = "Volkswagen", Type = "Golf", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "MN-012-OP", Soort = "auto", Merk = "Honda", Type = "Civic", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "QR-345-ST", Soort = "auto", Merk = "BMW", Type = "3 Serie", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "UV-678-WX", Soort = "auto", Merk = "Audi", Type = "A4", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-901-AB", Soort = "auto", Merk = "Mercedes", Type = "C-Klasse", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-234-EF", Soort = "auto", Merk = "Nissan", Type = "Qashqai", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-567-IJ", Soort = "auto", Merk = "Peugeot", Type = "208", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "KL-890-MN", Soort = "auto", Merk = "Renault", Type = "Clio", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "OP-123-QR", Soort = "auto", Merk = "Hobby", Type = "De Luxe", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "ST-456-UV", Soort = "auto", Merk = "Fendt", Type = "Bianco", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-789-YZ", Soort = "auto", Merk = "Knaus", Type = "Sport", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "AB-012-CD", Soort = "auto", Merk = "Dethleffs", Type = "Camper", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "EF-345-GH", Soort = "auto", Merk = "Adria", Type = "Altea", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "IJ-678-KL", Soort = "auto", Merk = "Eriba", Type = "Touring", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "MN-901-OP", Soort = "auto", Merk = "Tabbert", Type = "Puccini", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "QR-234-ST", Soort = "auto", Merk = "Burstner", Type = "Premio", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "UV-567-WX", Soort = "auto", Merk = "LMC", Type = "Musica", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "auto", Merk = "Sprite", Type = "Cruzer", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "auto", Merk = "Volkswagen", Type = "California", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "auto", Merk = "Mercedes", Type = "Marco Polo", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "auto", Merk = "Ford", Type = "Nugget", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "OP-012-QR", Soort = "auto", Merk = "Fiat", Type = "Ducato", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "ST-345-UV", Soort = "auto", Merk = "Citroen", Type = "Jumper", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "auto", Merk = "Peugeot", Type = "Boxer", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "AB-901-CD", Soort = "auto", Merk = "Renault", Type = "Master", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "EF-234-GH", Soort = "auto", Merk = "Iveco", Type = "Daily", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "IJ-567-KL", Soort = "auto", Merk = "Opel", Type = "Movano", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "MN-890-OP", Soort = "auto", Merk = "Nissan", Type = "NV400", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "QR-123-ST", Soort = "auto", Merk = "Kia", Type = "Sportage", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "UV-456-WX", Soort = "auto", Merk = "Hyundai", Type = "Tucson", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-789-AB", Soort = "auto", Merk = "Skoda", Type = "Octavia", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-012-EF", Soort = "auto", Merk = "Mazda", Type = "3", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-345-IJ", Soort = "auto", Merk = "Subaru", Type = "Impreza", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "KL-678-MN", Soort = "auto", Merk = "Suzuki", Type = "Vitara", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "OP-901-QR", Soort = "auto", Merk = "Volvo", Type = "XC60", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "ST-234-UV", Soort = "auto", Merk = "Mitsubishi", Type = "Outlander", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-567-YZ", Soort = "auto", Merk = "Jeep", Type = "Wrangler", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "auto", Merk = "Land Rover", Type = "Defender", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "auto", Merk = "Bailey", Type = "Unicorn", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "auto", Merk = "Lunar", Type = "Clubman", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "auto", Merk = "Swi", Type = "Conqueror", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "OP-012-QR", Soort = "auto", Merk = "Elddis", Type = "Avante", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "ST-345-UV", Soort = "auto", Merk = "Compass", Type = "Casita", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "auto", Merk = "Coachman", Type = "VIP", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "auto", Merk = "Coachman", Type = "VIP", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "AB-901-CD", Soort = "auto", Merk = "Buccaneer", Type = "Commodore", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "EF-234-GH", Soort = "auto", Merk = "Caravelair", Type = "Allegra", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "IJ-567-KL", Soort = "auto", Merk = "Sterckeman", Type = "Starlett", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "MN-890-OP", Soort = "auto", Merk = "Tab", Type = "320", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "QR-123-ST", Soort = "auto", Merk = "Volkswagen", Type = "Grand California", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "UV-456-WX", Soort = "auto", Merk = "Mercedes", Type = "Sprinter", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-789-AB", Soort = "auto", Merk = "Ford", Type = "Transit Custom", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-012-EF", Soort = "auto", Merk = "Fiat", Type = "Scudo", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-345-IJ", Soort = "auto", Merk = "Citroen", Type = "Berlingo", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "auto", Merk = "Peugeot", Type = "Partner", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "AB-901-CD", Soort = "auto", Merk = "Renault", Type = "Kangoo", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "EF-234-GH", Soort = "auto", Merk = "Iveco", Type = "Eurocargo", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "IJ-567-KL", Soort = "auto", Merk = "Opel", Type = "Combo", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "MN-890-OP", Soort = "auto", Merk = "Nissan", Type = "NV200", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "QR-123-ST", Soort = "auto", Merk = "Kia", Type = "Picanto", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "UV-456-WX", Soort = "auto", Merk = "Hyundai", Type = "i30", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-789-AB", Soort = "auto", Merk = "Skoda", Type = "Superb", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "CD-012-EF", Soort = "auto", Merk = "Mazda", Type = "6", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "GH-345-IJ", Soort = "auto", Merk = "Subaru", Type = "Forester", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "KL-678-MN", Soort = "auto", Merk = "Suzuki", Type = "Swift", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "OP-901-QR", Soort = "auto", Merk = "Volvo", Type = "XC90", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "ST-234-UV", Soort = "auto", Merk = "Mitsubishi", Type = "Eclipse Cross", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "WX-567-YZ", Soort = "auto", Merk = "Jeep", Type = "Renegade", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "auto", Merk = "Land Rover", Type = "Discovery", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 59.99, 29.99) },
                    new Voertuig { Kenteken = "AB-123-CD", Soort = "camper", Merk = "Volkswagen", Type = "California", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "EF-456-GH", Soort = "camper", Merk = "Mercedes", Type = "Marco Polo", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "IJ-789-KL", Soort = "camper", Merk = "Ford", Type = "Transit Custom", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "MN-012-OP", Soort = "camper", Merk = "Fiat", Type = "Ducato", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "QR-345-ST", Soort = "camper", Merk = "Citroën", Type = "Jumper", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "UV-678-WX", Soort = "camper", Merk = "Peugeot", Type = "Boxer", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "YZ-901-AB", Soort = "camper", Merk = "Renault", Type = "Master", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "CD-234-EF", Soort = "camper", Merk = "Nissan", Type = "NV400", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "GH-567-IJ", Soort = "camper", Merk = "Opel", Type = "Movano", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "KL-890-MN", Soort = "camper", Merk = "Iveco", Type = "Daily", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "OP-123-QR", Soort = "camper", Merk = "Volkswagen", Type = "Grand California", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "ST-456-UV", Soort = "camper", Merk = "Mercedes", Type = "Sprinter", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "WX-789-YZ", Soort = "camper", Merk = "Ford", Type = "Nugget", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "AB-012-CD", Soort = "camper", Merk = "Fiat", Type = "Talento", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "EF-345-GH", Soort = "camper", Merk = "Citroën", Type = "SpaceTourer", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "IJ-678-KL", Soort = "camper", Merk = "Peugeot", Type = "Traveller", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "MN-901-OP", Soort = "camper", Merk = "Renault", Type = "Trafic", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "QR-234-ST", Soort = "camper", Merk = "Nissan", Type = "Primastar", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "UV-567-WX", Soort = "camper", Merk = "Opel", Type = "Vivaro", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "camper", Merk = "Iveco", Type = "Eurocargo", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "camper", Merk = "Volkswagen", Type = "Mul van", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "camper", Merk = "Mercedes", Type = "Vito", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "camper", Merk = "Ford", Type = "Kuga Camper", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "OP-012-QR", Soort = "camper", Merk = "Fiat", Type = "Scudo", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "ST-345-UV", Soort = "camper", Merk = "Citroën", Type = "Berlingo", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "camper", Merk = "Peugeot", Type = "Expert Camper", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "AB-901-CD", Soort = "camper", Merk = "Renault", Type = "Kangoo Camper", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "EF-234-GH", Soort = "camper", Merk = "Nissan", Type = "Juke Camper", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "GH-567-IJ", Soort = "camper", Merk = "Opel", Type = "Zafira Camper", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "KL-890-MN", Soort = "camper", Merk = "Iveco", Type = "Camper 2000", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "OP-123-QR", Soort = "camper", Merk = "Volkswagen", Type = "Kombi", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "ST-456-UV", Soort = "camper", Merk = "Mercedes", Type = "Sprinter XXL", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "WX-789-YZ", Soort = "camper", Merk = "Ford", Type = "Custom Camper", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "AB-012-CD", Soort = "camper", Merk = "Fiat", Type = "Ducato Maxi", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "EF-345-GH", Soort = "camper", Merk = "Citroën", Type = "Jumper Camper", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "IJ-678-KL", Soort = "camper", Merk = "Peugeot", Type = "Boxer XL", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "MN-901-OP", Soort = "camper", Merk = "Renault", Type = "Master Pro", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "QR-234-ST", Soort = "camper", Merk = "Nissan", Type = "NV300 Camper", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "UV-567-WX", Soort = "camper", Merk = "Opel", Type = "Vivaro XL", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "camper", Merk = "Iveco", Type = "Daily Pro", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "camper", Merk = "Volkswagen", Type = "T6 California", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "camper", Merk = "Mercedes", Type = "V-Class Camper", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "camper", Merk = "Ford", Type = "Transit Nugget Plus", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 109.99, 69.99) },
                    new Voertuig { Kenteken = "AB-123-CD", Soort = "caravan", Merk = "Hobby", Type = "De Luxe", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "EF-456-GH", Soort = "caravan", Merk = "Fendt", Type = "Bianco", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "IJ-789-KL", Soort = "caravan", Merk = "Knaus", Type = "Sport", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "MN-012-OP", Soort = "caravan", Merk = "Adria", Type = "Altea", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "QR-345-ST", Soort = "caravan", Merk = "Dethleffs", Type = "Camper", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "UV-678-WX", Soort = "caravan", Merk = "Tabbert", Type = "Puccini", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "YZ-901-AB", Soort = "caravan", Merk = "Burstner", Type = "Premio", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "CD-234-EF", Soort = "caravan", Merk = "LMC", Type = "Musica", Aanschafjaar = 2015, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "GH-567-IJ", Soort = "caravan", Merk = "Sprite", Type = "Cruzer", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "KL-890-MN", Soort = "caravan", Merk = "Bailey", Type = "Unicorn", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "OP-123-QR", Soort = "caravan", Merk = "Lunar", Type = "Clubman", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "ST-456-UV", Soort = "caravan", Merk = "Swi", Type = "Conqueror", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "WX-789-YZ", Soort = "caravan", Merk = "Compass", Type = "Casita", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "AB-012-CD", Soort = "caravan", Merk = "Coachman", Type = "VIP", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "EF-345-GH", Soort = "caravan", Merk = "Buccaneer", Type = "Commodore", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "IJ-678-KL", Soort = "caravan", Merk = "Caravelair", Type = "Allegra", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "MN-901-OP", Soort = "caravan", Merk = "Sterckeman", Type = "Starle", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "QR-234-ST", Soort = "caravan", Merk = "Tab", Type = "320", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "UV-567-WX", Soort = "caravan", Merk = "Eriba", Type = "Touring", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "caravan", Merk = "Adria", Type = "Ac on", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "caravan", Merk = "Fendt", Type = "Tendenza", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "caravan", Merk = "Knaus", Type = "Sudwind", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "caravan", Merk = "Hobby", Type = "Excellent", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "OP-012-QR", Soort = "caravan", Merk = "Dethleffs", Type = "Beduin", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "ST-345-UV", Soort = "caravan", Merk = "Burstner", Type = "Averso", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "WX-678-YZ", Soort = "caravan", Merk = "LMC", Type = "Vivo", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "AB-901-CD", Soort = "caravan", Merk = "Sprite", Type = "Major", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "EF-234-GH", Soort = "caravan", Merk = "Bailey", Type = "Phoenix", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "GH-567-IJ", Soort = "caravan", Merk = "Lunar", Type = "Delta", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "KL-890-MN", Soort = "caravan", Merk = "Swi", Type = "Elegance", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "OP-123-QR", Soort = "caravan", Merk = "Compass", Type = "Corona", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "ST-456-UV", Soort = "caravan", Merk = "Coachman", Type = "Acadia", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "WX-789-YZ", Soort = "caravan", Merk = "Buccaneer", Type = "Barracuda", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "AB-012-CD", Soort = "caravan", Merk = "Caravelair", Type = "Antares", Aanschafjaar = 2016, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "EF-345-GH", Soort = "caravan", Merk = "Sterckeman", Type = "Evolu on", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "IJ-678-KL", Soort = "caravan", Merk = "Tab", Type = "400", Aanschafjaar = 2021, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "MN-901-OP", Soort = "caravan", Merk = "Eriba", Type = "Nova", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "QR-234-ST", Soort = "caravan", Merk = "Adria", Type = "Adora", Aanschafjaar = 2019, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "UV-567-WX", Soort = "caravan", Merk = "Fendt", Type = "Opal", Aanschafjaar = 2022, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "YZ-890-AB", Soort = "caravan", Merk = "Knaus", Type = "Sky Traveller", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "CD-123-EF", Soort = "caravan", Merk = "Hobby", Type = "Pres ge", Aanschafjaar = 2018, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "GH-456-IJ", Soort = "caravan", Merk = "Dethleffs", Type = "C'go", Aanschafjaar = 2020, Prijs = GenereerPrijs(random, 79.99, 54.99) },
                    new Voertuig { Kenteken = "KL-789-MN", Soort = "caravan", Merk = "Burstner", Type = "Premio Life", Aanschafjaar = 2017, Prijs = GenereerPrijs(random, 79.99, 54.99) }
                };

                int i = -1;
                foreach (var item in data)
                {
                    item.VoertuigId = i--;
                }

                context.Voertuigen.AddRange(data);
                context.SaveChanges();
            }
        }

        private static decimal GenereerPrijs(Random random, double beginRange, double eindRange)
        {
            return Math.Round((decimal)(random.NextDouble() * (beginRange - eindRange) + eindRange), 2);
        }
    }
}