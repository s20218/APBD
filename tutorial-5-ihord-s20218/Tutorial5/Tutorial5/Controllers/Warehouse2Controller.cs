using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tutorial5.Models;
using Tutorial5.Services;

namespace Tutorial5.Controllers
{
    [Route("api/warehouses2")]
    [ApiController]
    public class Warehouse2Controller : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public Warehouse2Controller(IDatabaseService dbService)
        {
            _dbService = dbService;
        }


        [HttpPost]
        public async Task<IActionResult> PostPurchaseAsync(Purchase newPurchase)
        {
            await _dbService.ExecuteProcudere(newPurchase);
            return Ok();
        }
    }
}
