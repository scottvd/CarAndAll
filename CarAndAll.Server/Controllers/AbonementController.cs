using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbbonementController : ControllerBase
    {

        private readonly CarAndAllContext _context;



        public AbbonementController(CarAndAllContext context)
        {
            _context = context;
        }


        // GET: api/Abonnement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Abonnement>>> GetAbonnementen()
        {
            return await _context.Abonnementen.ToListAsync();
        }


        //// GET: api/Abonnement/5
        //[HttpGet("{id}")]

    }
}
