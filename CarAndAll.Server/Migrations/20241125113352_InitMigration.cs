using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarAndAll.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Prijs = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Medewerkers",
                columns: table => new
                {
                    PersoneelsNummer = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.PersoneelsNummer);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    Kenteken = table.Column<string>(type: "TEXT", nullable: false),
                    Soort = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Merk = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Aanschafjaar = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.Kenteken);
                });

            migrationBuilder.CreateTable(
                name: "Bedrijven",
                columns: table => new
                {
                    KvkNummer = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    AbonnementType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.KvkNummer);
                    table.ForeignKey(
                        name: "FK_Bedrijven_Abonnementen_AbonnementType",
                        column: x => x.AbonnementType,
                        principalTable: "Abonnementen",
                        principalColumn: "Type");
                });

            migrationBuilder.CreateTable(
                name: "Klanten",
                columns: table => new
                {
                    KlantID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Adres = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    BedrijfKvkNummer = table.Column<int>(type: "INTEGER", nullable: false),
                    AbonnementType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klanten", x => x.KlantID);
                    table.ForeignKey(
                        name: "FK_Klanten_Abonnementen_AbonnementType",
                        column: x => x.AbonnementType,
                        principalTable: "Abonnementen",
                        principalColumn: "Type");
                    table.ForeignKey(
                        name: "FK_Klanten_Bedrijven_BedrijfKvkNummer",
                        column: x => x.BedrijfKvkNummer,
                        principalTable: "Bedrijven",
                        principalColumn: "KvkNummer",
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
                    Status = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    KlantID = table.Column<int>(type: "INTEGER", nullable: false),
                    VoertuigKenteken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verhuuraanvragen", x => x.AanvraagID);
                    table.ForeignKey(
                        name: "FK_Verhuuraanvragen_Klanten_KlantID",
                        column: x => x.KlantID,
                        principalTable: "Klanten",
                        principalColumn: "KlantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Verhuuraanvragen_Voertuigen_VoertuigKenteken",
                        column: x => x.VoertuigKenteken,
                        principalTable: "Voertuigen",
                        principalColumn: "Kenteken",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schademeldingen",
                columns: table => new
                {
                    SchademeldingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Beschrijving = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    VoertuigKenteken = table.Column<string>(type: "TEXT", nullable: false),
                    VerhuuraanvraagAanvraagID = table.Column<int>(type: "INTEGER", nullable: false),
                    MedewerkerPersoneelsNummer = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schademeldingen", x => x.SchademeldingID);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Medewerkers_MedewerkerPersoneelsNummer",
                        column: x => x.MedewerkerPersoneelsNummer,
                        principalTable: "Medewerkers",
                        principalColumn: "PersoneelsNummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Verhuuraanvragen_VerhuuraanvraagAanvraagID",
                        column: x => x.VerhuuraanvraagAanvraagID,
                        principalTable: "Verhuuraanvragen",
                        principalColumn: "AanvraagID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Voertuigen_VoertuigKenteken",
                        column: x => x.VoertuigKenteken,
                        principalTable: "Voertuigen",
                        principalColumn: "Kenteken",
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
                    Inhoud = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    SchademeldingID = table.Column<int>(type: "INTEGER", nullable: false),
                    MedewerkerPersoneelsNummer = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notities", x => x.NotitieID);
                    table.ForeignKey(
                        name: "FK_Notities_Medewerkers_MedewerkerPersoneelsNummer",
                        column: x => x.MedewerkerPersoneelsNummer,
                        principalTable: "Medewerkers",
                        principalColumn: "PersoneelsNummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notities_Schademeldingen_SchademeldingID",
                        column: x => x.SchademeldingID,
                        principalTable: "Schademeldingen",
                        principalColumn: "SchademeldingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bedrijven_AbonnementType",
                table: "Bedrijven",
                column: "AbonnementType");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_MedewerkerPersoneelsNummer",
                table: "Fotos",
                column: "MedewerkerPersoneelsNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_SchademeldingID",
                table: "Fotos",
                column: "SchademeldingID");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_AbonnementType",
                table: "Klanten",
                column: "AbonnementType");

            migrationBuilder.CreateIndex(
                name: "IX_Klanten_BedrijfKvkNummer",
                table: "Klanten",
                column: "BedrijfKvkNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Notities_MedewerkerPersoneelsNummer",
                table: "Notities",
                column: "MedewerkerPersoneelsNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Notities_SchademeldingID",
                table: "Notities",
                column: "SchademeldingID");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_MedewerkerPersoneelsNummer",
                table: "Schademeldingen",
                column: "MedewerkerPersoneelsNummer");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VerhuuraanvraagAanvraagID",
                table: "Schademeldingen",
                column: "VerhuuraanvraagAanvraagID");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VoertuigKenteken",
                table: "Schademeldingen",
                column: "VoertuigKenteken");

            migrationBuilder.CreateIndex(
                name: "IX_Verhuuraanvragen_KlantID",
                table: "Verhuuraanvragen",
                column: "KlantID");

            migrationBuilder.CreateIndex(
                name: "IX_Verhuuraanvragen_VoertuigKenteken",
                table: "Verhuuraanvragen",
                column: "VoertuigKenteken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "Notities");

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
                name: "Abonnementen");
        }
    }
}
