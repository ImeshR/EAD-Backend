/*
 * File: Master Data Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Master Data  management.

*/

using EAD_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly MasterDataService _masterDataService;

        public MasterDataController(MasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        //! ======================================================== Define API Endpoints ============================================================>
        //! GET: api/MasterData/GetRoles
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(_masterDataService.GetRoles());
        }
    }
}