using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonnementController : ControllerBase
    {

        private readonly CarAndAllContext _context;



        public AbonnementController(CarAndAllContext context)
        {
            _context = context;
        }


        [HttpGet("Abonnement")]
        public async Task<ActionResult<IEnumerable<Abonnement>>> GetAbonnementen()
        {
            return await _context.Abonnementen.ToListAsync();
        }

        //// GET: Abonnement/actief
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Abonnement>>> GetActieveAbonnementen()
        //{
        //    return await _context.Abonnementen.ToListAsync();
        //}
    }
}
