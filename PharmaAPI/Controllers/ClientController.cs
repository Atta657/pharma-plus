using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaAPI.Data;
using PharmaAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Client/company/5
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsByCompany(int companyId)
        {
            return await _context.Clients.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        // POST: api/Client
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientsByCompany), new { companyId = client.CompanyId }, client);
        }
    }
}
