using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tutorial5.Models;
using Tutorial5.Services;

namespace Tutorial5.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public WarehouseController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPurchaseAsync(Purchase newPurchase)
        {
            
            bool productWarehouseExist = await _dbService.ProductAndWarehouseExist(newPurchase.IdProduct, newPurchase.IdWarehouse);

            if (!productWarehouseExist)
            {
                return StatusCode(404, "product or warehouse with the given id doesn't exist");
            }

            int idOrder = await _dbService.GetOrderId(newPurchase.IdProduct, newPurchase.CreatedAt, newPurchase.Amount);

            if (idOrder == -1)
            {
                return StatusCode(404, "there is no suitable order");
            }

            bool productPurchaseExist = await _dbService.ProductPurchaseExist(idOrder);

            if (productPurchaseExist)
            {
                return StatusCode(400, "Order has already been purchased");
            }

            return Ok(await _dbService.AddWarehouseAsync(newPurchase, idOrder));
        }
    }
}
