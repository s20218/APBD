using Assignment7.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment7.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IDatabaseService _dbService;

        public ClientsController(IDatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClientAsync(int idClient)
        {
            bool response = await _dbService.DeleteClientAsync(idClient);

            if (!response)
            {
                return StatusCode(400, "Client with the given id has tours");
            }

            return StatusCode(204);
        }
    }
}
