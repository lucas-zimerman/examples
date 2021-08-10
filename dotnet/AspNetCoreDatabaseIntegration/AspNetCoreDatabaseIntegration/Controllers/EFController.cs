using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using AspNetCoreDatabaseIntegration.OutputExamples;

namespace AspNetCoreDatabaseIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Entity Framework Core")]
    [ApiController]
    public class EFController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public EFController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get the amount of stored items on the database.
        /// </summary>
        /// <returns>The database count.</returns>
        [HttpGet("Total")]
        [ProducesResponseType(typeof(int), 200)]
        public IActionResult GetTotal()
        {
            var data = unitOfWork.EFExceptionTypeRepository.GetTotal();
            return Ok(data);
        }

        /// <summary>
        /// Get all items from the database using EF Syntax.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<ExceptionType>), 200)]
        public IActionResult GetAll()
        {
            var data = unitOfWork.EFExceptionTypeRepository.GetAll();
            if (data == null)
            {
                return Ok();
            }
            return Ok(data.Take(500));
        }

        /// <summary>
        /// Get all items from the database using Raw SQL with EF.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All/RawSQL")]
        [ProducesResponseType(typeof(List<ExceptionType>), 200)]
        public IActionResult GetAllRawSQL()
        {
            var data = unitOfWork.EFExceptionTypeRepository.GetAllRawSQL();
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
        [HttpGet("All/Error")]
        [ProducesResponseType(typeof(ExceptionExample), 500)]
        public IActionResult GetAllError()
        {
            // This will generate a query with SELECT TOP -500.
            var data = unitOfWork.EFExceptionTypeRepository.GetAll(-500);
            if (data == null)
            {
                return Ok();
            }
            return Ok(data);
        }
    }
}
