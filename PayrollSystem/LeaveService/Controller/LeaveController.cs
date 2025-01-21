using LeaveService.Data;
using LeaveService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveDbContext _context;

        public LeaveController(LeaveDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddLeave(Leave leave)
        {
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return Ok(leave);
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            return Ok(await _context.Leaves.ToListAsync());
        }
    }

}
