using Microsoft.AspNetCore.Mvc;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class RegistreerController : ControllerBase
{
    private readonly UserManager<Gebruiker> _userManager;
    private readonly CarAndAllContext _context;

    public RegistreerController(UserManager<Gebruiker> userManager, CarAndAllContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpPost("RegistreerGebruiker")]
    public async Task<IActionResult> Register([FromBody] RegistreerDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var nieuweHuurder = new Huurder
            {
                Email = dto.Email,
                UserName = dto.Email,
                Naam = dto.Naam,
                Adres = dto.Adres,
                Type = dto.Zakelijk ? HuurderType.Zakelijk : HuurderType.Particulier
            };

            var result = await _userManager.CreateAsync(nieuweHuurder, dto.Wachtwoord);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errors });
            }

            if (dto.Zakelijk)
            {
                var nieuwBedrijf = new Bedrijf
                {
                    KvkNummer = dto.Kvk.Value,
                    Naam = dto.BedrijfNaam,
                    Adres = dto.BedrijfAdres,
                    Huurders = new List<Huurder> { nieuweHuurder }
                };

                _context.Bedrijven.Add(nieuwBedrijf);
                await _context.SaveChangesAsync();

                nieuweHuurder.BedrijfId = nieuwBedrijf.KvkNummer;
                _context.Huurders.Update(nieuweHuurder);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return StatusCode(500, new
            {
                Message = "Er is iets fout gegaan.",
                Error = ex.Message,
            });
        }
    }
}
