using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AspNetCoreDatabaseIntegration.Model;
using AspNetCoreDatabaseIntegration.OutputExamples;

namespace AspNetCoreDatabaseIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SQLClient with Dapper")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DapperController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get the amount of stored items on the database.
        /// </summary>
        /// <returns>The database count.</returns>
        [HttpGet("Total")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetTotal()
        {
            var data = await unitOfWork.DapperExceptionTypeRepository.GetTotal();
            return Ok(data);
        }

        /// <summary>
        /// Get all items from the database using EF Syntax.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<ExceptionType>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.DapperExceptionTypeRepository.GetAll();
            if (data == null)
            {
                return Ok();
            }
            return Ok(data.Take(500));
        }

        /// <summary>
        /// Get all items from the database assyncronously.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All/Parallel")]
        [ProducesResponseType(typeof(List<ExceptionType>), 200)]
        public async Task<IActionResult> GetAllParallel()
        {
            var data = await unitOfWork.DapperExceptionTypeRepository.GetAllParallel(1000);
            if (data == null)
            {
                return Ok();
            }
            return Ok(data.Take(500));
        }

        /// <summary>
        /// Get all items from the database assyncronously with an query with error.
        /// </summary>
        /// <returns>An exception.</returns>
        [HttpGet("All/Parallel/Error")]
        [ProducesResponseType(typeof(ExceptionExample), 500)]
        public async Task<IActionResult> GetAllParallelError()
        {
            // This will generate a query with SELECT TOP -250.
            // and that is not allowed, generating an error.
            var data = await unitOfWork.DapperExceptionTypeRepository.GetAllParallel(-500);
            if (data == null)
            {
                return Ok();
            }
            return Ok(data);
        }
    }
}
