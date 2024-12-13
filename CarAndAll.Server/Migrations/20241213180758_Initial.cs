using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarAndAll.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Prijs = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gebruikers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruikers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medewerkers",
                columns: table => new
                {
                    PersoneelsNummer = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.PersoneelsNummer);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    VoertuigID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Kenteken = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Soort = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Merk = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Aanschafjaar = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.VoertuigID);
                });

            migrationBuilder.CreateTable(
                name: "Bedrijven",
                columns: table => new
                {
                    KvkNummer = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    AbonnementId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.KvkNummer);
                    table.ForeignKey(
                        name: "FK_Bedrijven_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Gebruikers_UserId",
                        column: x => x.UserId,
                        principalTable: "Gebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Gebruikers_UserId",
                        column: x => x.UserId,
                        principalTable: "Gebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Gebruikers_UserId",
                        column: x => x.UserId,
                        principalTable: "Gebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Gebruikers_UserId",
                        column: x => x.UserId,
                        principalTable: "Gebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    BedrijfId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Klanten_Bedrijven_BedrijfId",
                        column: x => x.BedrijfId,
                        principalTable: "Bedrijven",
                        principalColumn: "KvkNummer");
                    table.ForeignKey(
                        name: "FK_Klanten_Gebruikers_Id",
                        column: x => x.Id,
                        principalTable: "Gebruikers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Verhuuraanvragen",
                columns: table => new
                {
                    AanvraagID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    KlantId = table.Column<string>(type: "TEXT", nullable: false),
                    VoertuigId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verhuuraanvragen", x => x.AanvraagID);
                    table.ForeignKey(
                        name: "FK_Verhuuraanvragen_Klanten_KlantId",
                        column: x => x.KlantId,
                        principalTable: "Klanten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Verhuuraanvragen_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schademeldingen",
                columns: table => new
                {
                    SchademeldingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Beschrijving = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    VoertuigId = table.Column<int>(type: "INTEGER", nullable: false),
                    VerhuuraanvraagId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedewerkerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schademeldingen", x => x.SchademeldingID);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Medewerkers_MedewerkerId",
                        column: x => x.MedewerkerId,
                        principalTable: "Medewerkers",
                        principalColumn: "PersoneelsNummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Verhuuraanvragen_VerhuuraanvraagId",
                        column: x => x.VerhuuraanvraagId,
                        principalTable: "Verhuuraanvragen",
                        principalColumn: "AanvraagID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "VoertuigID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    FotoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SchademeldingID = table.Column<int>(type: "INTEGER", nullable: false),
                    MedewerkerPersoneelsNummer = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.FotoID);
                    table.ForeignKey(
                        name: "FK_Fotos_Medewerkers_MedewerkerPersoneelsNummer",
                        column: x => x.MedewerkerPersoneelsNummer,
                        principalTable: "Medewerkers",
                        principalColumn: "PersoneelsNummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fotos_Schademeldingen_SchademeldingID",
                        column: x => x.SchademeldingID,
                        principalTable: "Schademeldingen",
                        principalColumn: "SchademeldingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notities",
                columns: table => new
                {
                    NotitieID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Inhoud = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SchademeldingId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedewerkerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notities", x => x.NotitieID);
                    table.ForeignKey(
                        name: "FK_Notities_Medewerkers_MedewerkerId",
                        column: x => x.MedewerkerId,
                        principalTable: "Medewerkers",
                        principalColumn: "PersoneelsNummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notities_Schademeldingen_SchademeldingId",
                        column: x => x.SchademeldingId,
                        principalTable: "Schademeldingen",
                        principalColumn: "SchademeldingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Voertuigen",
                columns: new[] { "VoertuigID", "Aanschafjaar", "Kenteken", "Merk", "Soort", "Type" },
                values: new object[,]
                {
                    { -157, 2017, "KL-789-MN", "Burstner", "caravan", "Premio Life" },
                    { -156, 2020, "GH-456-IJ", "Dethleffs", "caravan", "C'go" },
                    { -155, 2018, "CD-123-EF", "Hobby", "caravan", "Pres ge" },
                    { -154, 2017, "YZ-890-AB", "Knaus", "caravan", "Sky Traveller" },
                    { -153, 2022, "UV-567-WX", "Fendt", "caravan", "Opal" },
                    { -152, 2019, "QR-234-ST", "Adria", "caravan", "Adora" },
                    { -151, 2020, "MN-901-OP", "Eriba", "caravan", "Nova" },
                    { -150, 2021, "IJ-678-KL", "Tab", "caravan", "400" },
                    { -149, 2018, "EF-345-GH", "Sterckeman", "caravan", "Evolu on" },
                    { -148, 2016, "AB-012-CD", "Caravelair", "caravan", "Antares" },
                    { -147, 2020, "WX-789-YZ", "Buccaneer", "caravan", "Barracuda" },
                    { -146, 2019, "ST-456-UV", "Coachman", "caravan", "Acadia" },
                    { -145, 2021, "OP-123-QR", "Compass", "caravan", "Corona" },
                    { -144, 2018, "KL-890-MN", "Swi", "caravan", "Elegance" },
                    { -143, 2017, "GH-567-IJ", "Lunar", "caravan", "Delta" },
                    { -142, 2022, "EF-234-GH", "Bailey", "caravan", "Phoenix" },
                    { -141, 2019, "AB-901-CD", "Sprite", "caravan", "Major" },
                    { -140, 2020, "WX-678-YZ", "LMC", "caravan", "Vivo" },
                    { -139, 2021, "ST-345-UV", "Burstner", "caravan", "Averso" },
                    { -138, 2019, "OP-012-QR", "Dethleffs", "caravan", "Beduin" },
                    { -137, 2017, "KL-789-MN", "Hobby", "caravan", "Excellent" },
                    { -136, 2020, "GH-456-IJ", "Knaus", "caravan", "Sudwind" },
                    { -135, 2018, "CD-123-EF", "Fendt", "caravan", "Tendenza" },
                    { -134, 2017, "YZ-890-AB", "Adria", "caravan", "Ac on" },
                    { -133, 2022, "UV-567-WX", "Eriba", "caravan", "Touring" },
                    { -132, 2019, "QR-234-ST", "Tab", "caravan", "320" },
                    { -131, 2020, "MN-901-OP", "Sterckeman", "caravan", "Starle" },
                    { -130, 2021, "IJ-678-KL", "Caravelair", "caravan", "Allegra" },
                    { -129, 2018, "EF-345-GH", "Buccaneer", "caravan", "Commodore" },
                    { -128, 2016, "AB-012-CD", "Coachman", "caravan", "VIP" },
                    { -127, 2020, "WX-789-YZ", "Compass", "caravan", "Casita" },
                    { -126, 2019, "ST-456-UV", "Swi", "caravan", "Conqueror" },
                    { -125, 2017, "OP-123-QR", "Lunar", "caravan", "Clubman" },
                    { -124, 2018, "KL-890-MN", "Bailey", "caravan", "Unicorn" },
                    { -123, 2021, "GH-567-IJ", "Sprite", "caravan", "Cruzer" },
                    { -122, 2015, "CD-234-EF", "LMC", "caravan", "Musica" },
                    { -121, 2022, "YZ-901-AB", "Burstner", "caravan", "Premio" },
                    { -120, 2016, "UV-678-WX", "Tabbert", "caravan", "Puccini" },
                    { -119, 2021, "QR-345-ST", "Dethleffs", "caravan", "Camper" },
                    { -118, 2017, "MN-012-OP", "Adria", "caravan", "Altea" },
                    { -117, 2020, "IJ-789-KL", "Knaus", "caravan", "Sport" },
                    { -116, 2019, "EF-456-GH", "Fendt", "caravan", "Bianco" },
                    { -115, 2018, "AB-123-CD", "Hobby", "caravan", "De Luxe" },
                    { -114, 2022, "KL-789-MN", "Ford", "camper", "Transit Nugget Plus" },
                    { -113, 2019, "GH-456-IJ", "Mercedes", "camper", "V-Class Camper" },
                    { -112, 2020, "CD-123-EF", "Volkswagen", "camper", "T6 California" },
                    { -111, 2018, "YZ-890-AB", "Iveco", "camper", "Daily Pro" },
                    { -110, 2017, "UV-567-WX", "Opel", "camper", "Vivaro XL" },
                    { -109, 2022, "QR-234-ST", "Nissan", "camper", "NV300 Camper" },
                    { -108, 2019, "MN-901-OP", "Renault", "camper", "Master Pro" },
                    { -107, 2021, "IJ-678-KL", "Peugeot", "camper", "Boxer XL" },
                    { -106, 2018, "EF-345-GH", "Citroën", "camper", "Jumper Camper" },
                    { -105, 2016, "AB-012-CD", "Fiat", "camper", "Ducato Maxi" },
                    { -104, 2020, "WX-789-YZ", "Ford", "camper", "Custom Camper" },
                    { -103, 2021, "ST-456-UV", "Mercedes", "camper", "Sprinter XXL" },
                    { -102, 2017, "OP-123-QR", "Volkswagen", "camper", "Kombi" },
                    { -101, 2018, "KL-890-MN", "Iveco", "camper", "Camper 2000" },
                    { -100, 2021, "GH-567-IJ", "Opel", "camper", "Zafira Camper" },
                    { -99, 2015, "EF-234-GH", "Nissan", "camper", "Juke Camper" },
                    { -98, 2022, "AB-901-CD", "Renault", "camper", "Kangoo Camper" },
                    { -97, 2016, "WX-678-YZ", "Peugeot", "camper", "Expert Camper" },
                    { -96, 2019, "ST-345-UV", "Citroën", "camper", "Berlingo" },
                    { -95, 2018, "OP-012-QR", "Fiat", "camper", "Scudo" },
                    { -94, 2017, "KL-789-MN", "Ford", "camper", "Kuga Camper" },
                    { -93, 2020, "GH-456-IJ", "Mercedes", "camper", "Vito" },
                    { -92, 2018, "CD-123-EF", "Volkswagen", "camper", "Mul van" },
                    { -91, 2017, "YZ-890-AB", "Iveco", "camper", "Eurocargo" },
                    { -90, 2022, "UV-567-WX", "Opel", "camper", "Vivaro" },
                    { -89, 2019, "QR-234-ST", "Nissan", "camper", "Primastar" },
                    { -88, 2020, "MN-901-OP", "Renault", "camper", "Trafic" },
                    { -87, 2021, "IJ-678-KL", "Peugeot", "camper", "Traveller" },
                    { -86, 2018, "EF-345-GH", "Citroën", "camper", "SpaceTourer" },
                    { -85, 2016, "AB-012-CD", "Fiat", "camper", "Talento" },
                    { -84, 2020, "WX-789-YZ", "Ford", "camper", "Nugget" },
                    { -83, 2019, "ST-456-UV", "Mercedes", "camper", "Sprinter" },
                    { -82, 2017, "OP-123-QR", "Volkswagen", "camper", "Grand California" },
                    { -81, 2018, "KL-890-MN", "Iveco", "camper", "Daily" },
                    { -80, 2021, "GH-567-IJ", "Opel", "camper", "Movano" },
                    { -79, 2015, "CD-234-EF", "Nissan", "camper", "NV400" },
                    { -78, 2022, "YZ-901-AB", "Renault", "camper", "Master" },
                    { -77, 2016, "UV-678-WX", "Peugeot", "camper", "Boxer" },
                    { -76, 2021, "QR-345-ST", "Citroën", "camper", "Jumper" },
                    { -75, 2017, "MN-012-OP", "Fiat", "camper", "Ducato" },
                    { -74, 2020, "IJ-789-KL", "Ford", "camper", "Transit Custom" },
                    { -73, 2019, "EF-456-GH", "Mercedes", "camper", "Marco Polo" },
                    { -72, 2018, "AB-123-CD", "Volkswagen", "camper", "California" },
                    { -71, 2021, "YZ-890-AB", "Land Rover", "auto", "Discovery" },
                    { -70, 2022, "WX-567-YZ", "Jeep", "auto", "Renegade" },
                    { -69, 2017, "ST-234-UV", "Mitsubishi", "auto", "Eclipse Cross" },
                    { -68, 2020, "OP-901-QR", "Volvo", "auto", "XC90" },
                    { -67, 2019, "KL-678-MN", "Suzuki", "auto", "Swift" },
                    { -66, 2021, "GH-345-IJ", "Subaru", "auto", "Forester" },
                    { -65, 2017, "CD-012-EF", "Mazda", "auto", "6" },
                    { -64, 2020, "YZ-789-AB", "Skoda", "auto", "Superb" },
                    { -63, 2019, "UV-456-WX", "Hyundai", "auto", "i30" },
                    { -62, 2018, "QR-123-ST", "Kia", "auto", "Picanto" },
                    { -61, 2021, "MN-890-OP", "Nissan", "auto", "NV200" },
                    { -60, 2022, "IJ-567-KL", "Opel", "auto", "Combo" },
                    { -59, 2017, "EF-234-GH", "Iveco", "auto", "Eurocargo" },
                    { -58, 2020, "AB-901-CD", "Renault", "auto", "Kangoo" },
                    { -57, 2019, "WX-678-YZ", "Peugeot", "auto", "Partner" },
                    { -56, 2021, "GH-345-IJ", "Citroen", "auto", "Berlingo" },
                    { -55, 2018, "CD-012-EF", "Fiat", "auto", "Scudo" },
                    { -54, 2017, "YZ-789-AB", "Ford", "auto", "Transit Custom" },
                    { -53, 2020, "UV-456-WX", "Mercedes", "auto", "Sprinter" },
                    { -52, 2019, "QR-123-ST", "Volkswagen", "auto", "Grand California" },
                    { -51, 2018, "MN-890-OP", "Tab", "auto", "320" },
                    { -50, 2021, "IJ-567-KL", "Sterckeman", "auto", "Starlett" },
                    { -49, 2015, "EF-234-GH", "Caravelair", "auto", "Allegra" },
                    { -48, 2022, "AB-901-CD", "Buccaneer", "auto", "Commodore" },
                    { -47, 2016, "WX-678-YZ", "Coachman", "auto", "VIP" },
                    { -46, 2016, "WX-678-YZ", "Coachman", "auto", "VIP" },
                    { -45, 2021, "ST-345-UV", "Compass", "auto", "Casita" },
                    { -44, 2017, "OP-012-QR", "Elddis", "auto", "Avante" },
                    { -43, 2020, "KL-789-MN", "Swi", "auto", "Conqueror" },
                    { -42, 2019, "GH-456-IJ", "Lunar", "auto", "Clubman" },
                    { -41, 2018, "CD-123-EF", "Bailey", "auto", "Unicorn" },
                    { -40, 2021, "YZ-890-AB", "Land Rover", "auto", "Defender" },
                    { -39, 2022, "WX-567-YZ", "Jeep", "auto", "Wrangler" },
                    { -38, 2017, "ST-234-UV", "Mitsubishi", "auto", "Outlander" },
                    { -37, 2020, "OP-901-QR", "Volvo", "auto", "XC60" },
                    { -36, 2019, "KL-678-MN", "Suzuki", "auto", "Vitara" },
                    { -35, 2021, "GH-345-IJ", "Subaru", "auto", "Impreza" },
                    { -34, 2018, "CD-012-EF", "Mazda", "auto", "3" },
                    { -33, 2017, "YZ-789-AB", "Skoda", "auto", "Octavia" },
                    { -32, 2020, "UV-456-WX", "Hyundai", "auto", "Tucson" },
                    { -31, 2019, "QR-123-ST", "Kia", "auto", "Sportage" },
                    { -30, 2018, "MN-890-OP", "Nissan", "auto", "NV400" },
                    { -29, 2021, "IJ-567-KL", "Opel", "auto", "Movano" },
                    { -28, 2015, "EF-234-GH", "Iveco", "auto", "Daily" },
                    { -27, 2022, "AB-901-CD", "Renault", "auto", "Master" },
                    { -26, 2016, "WX-678-YZ", "Peugeot", "auto", "Boxer" },
                    { -25, 2021, "ST-345-UV", "Citroen", "auto", "Jumper" },
                    { -24, 2017, "OP-012-QR", "Fiat", "auto", "Ducato" },
                    { -23, 2020, "KL-789-MN", "Ford", "auto", "Nugget" },
                    { -22, 2019, "GH-456-IJ", "Mercedes", "auto", "Marco Polo" },
                    { -21, 2018, "CD-123-EF", "Volkswagen", "auto", "California" },
                    { -20, 2022, "YZ-890-AB", "Sprite", "auto", "Cruzer" },
                    { -19, 2018, "UV-567-WX", "LMC", "auto", "Musica" },
                    { -18, 2019, "QR-234-ST", "Burstner", "auto", "Premio" },
                    { -17, 2021, "MN-901-OP", "Tabbert", "auto", "Puccini" },
                    { -16, 2015, "IJ-678-KL", "Eriba", "auto", "Touring" },
                    { -15, 2020, "EF-345-GH", "Adria", "auto", "Altea" },
                    { -14, 2016, "AB-012-CD", "Dethleffs", "auto", "Camper" },
                    { -13, 2019, "WX-789-YZ", "Knaus", "auto", "Sport" },
                    { -12, 2018, "ST-456-UV", "Fendt", "auto", "Bianco" },
                    { -11, 2017, "OP-123-QR", "Hobby", "auto", "De Luxe" },
                    { -10, 2018, "KL-890-MN", "Renault", "auto", "Clio" },
                    { -9, 2021, "GH-567-IJ", "Peugeot", "auto", "208" },
                    { -8, 2015, "CD-234-EF", "Nissan", "auto", "Qashqai" },
                    { -7, 2022, "YZ-901-AB", "Mercedes", "auto", "C-Klasse" },
                    { -6, 2016, "UV-678-WX", "Audi", "auto", "A4" },
                    { -5, 2021, "QR-345-ST", "BMW", "auto", "3 Serie" },
                    { -4, 2017, "MN-012-OP", "Honda", "auto", "Civic" },
                    { -3, 2020, "IJ-789-KL", "Volkswagen", "auto", "Golf" },
                    { -2, 2019, "EF-456-GH", "Ford", "auto", "Focus" },
                    { -1, 2018, "AB-123-CD", "Toyota", "auto", "Corolla" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Bedrijven_AbonnementId",
                table: "Bedrijven",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_MedewerkerPersoneelsNummer",
                table: "Fotos",
                column: "MedewerkerPersoneelsNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_SchademeldingID",
                table: "Fotos",
                column: "SchademeldingID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Gebruikers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Gebruikers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_BedrijfId",
                table: "Klanten",
                column: "BedrijfId");

            migrationBuilder.CreateIndex(
                name: "IX_Medewerkers_PersoneelsNummer",
                table: "Medewerkers",
                column: "PersoneelsNummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notities_MedewerkerId",
                table: "Notities",
                column: "MedewerkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notities_SchademeldingId",
                table: "Notities",
                column: "SchademeldingId");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_MedewerkerId",
                table: "Schademeldingen",
                column: "MedewerkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VerhuuraanvraagId",
                table: "Schademeldingen",
                column: "VerhuuraanvraagId");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VoertuigId",
                table: "Schademeldingen",
                column: "VoertuigId");

            migrationBuilder.CreateIndex(
                name: "IX_Verhuuraanvragen_KlantId",
                table: "Verhuuraanvragen",
                column: "KlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Verhuuraanvragen_VoertuigId",
                table: "Verhuuraanvragen",
                column: "VoertuigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "Notities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Schademeldingen");

            migrationBuilder.DropTable(
                name: "Medewerkers");

            migrationBuilder.DropTable(
                name: "Verhuuraanvragen");

            migrationBuilder.DropTable(
                name: "Klanten");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Bedrijven");

            migrationBuilder.DropTable(
                name: "Gebruikers");

            migrationBuilder.DropTable(
                name: "Abonnementen");
        }
    }
}
