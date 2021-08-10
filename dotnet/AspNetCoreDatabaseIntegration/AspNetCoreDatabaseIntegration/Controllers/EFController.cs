﻿using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace AspNetCoreDatabaseIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public EFController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("Total")]
        public async Task<IActionResult> GetTotal()
        { 
            var data = unitOfWork.EFBugRepository.GetTotal();
            return Ok(data);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = unitOfWork.EFBugRepository.GetAll();
            if (data == null) return Ok();
            return Ok(data.Take(500));
        }

        [HttpGet("GetAllRawSQL")]
        public async Task<IActionResult> GetAllRawSQL()
        {
            var data = unitOfWork.EFBugRepository.GetAllRawSQL();
            if (data == null) return Ok();
            return Ok(data.Take(500));
        }

        [HttpGet("GetAllParallel")]
        public async Task<IActionResult> GetAllParallel()
        {
            var data = await unitOfWork.EFBugRepository.GetAllParallel(1000);
            if (data == null) return Ok();
            return Ok(data.Take(500));
        }

        [HttpGet("GetAllParallel/Error")]
        public async Task<IActionResult> GetAllParallelError()
        {
            // This will generate a query with SELECT TOP -250.
            // and that is not allowed, generating an error.
            var data = await unitOfWork.EFBugRepository.GetAllParallel(-500);
            if (data == null) return Ok();
            return Ok(data);
        }
    }
}
