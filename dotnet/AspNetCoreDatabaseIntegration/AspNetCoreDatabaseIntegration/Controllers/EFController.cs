using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetTotal()
        { 
            var data = unitOfWork.EFBugRepository.GetTotal();
            return Ok(data);
        }

        /// <summary>
        /// Get all items from the database using EF Syntax.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<Bug>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var data = unitOfWork.EFBugRepository.GetAll();
            if (data == null) return Ok();
            return Ok(data.Take(500));
        }

        /// <summary>
        /// Get all items from the database using Raw SQL with EF.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All/RawSQL")]
        [ProducesResponseType(typeof(List<Bug>), 200)]
        public async Task<IActionResult> GetAllRawSQL()
        {
            var data = unitOfWork.EFBugRepository.GetAllRawSQL();
            if (data == null) return Ok();
            return Ok(data.Take(500));
        }

        /// <summary>
        /// Get all items from the database assyncronously.
        /// </summary>
        /// <returns>The total items capped by 500.</returns>
        [HttpGet("All/Parallel")]
        [ProducesResponseType(typeof(List<Bug>), 200)]
        public async Task<IActionResult> GetAllParallel()
        {
            var data = await GetAllParallel(1000);
            if (data == null) return Ok();
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
            var data = await GetAllParallel(-500);
            if (data == null) return Ok();
            return Ok(data);
        }

        private async Task<List<Bug>> GetAllParallel(int ammount)
        {
            var results = new Task<IList<Bug>>[]
            {
                unitOfWork.EFBugRepository.GetAllParallel(ammount, ammount / 4, 0),
                unitOfWork.EFBugRepository.GetAllParallel(ammount / 4, ammount / 4, 1),
                unitOfWork.EFBugRepository.GetAllParallel(ammount - ammount / 4, ammount / 4, 2),
                unitOfWork.EFBugRepository.GetAllParallel(ammount / 2, ammount / 4, 3),
            };
            await Task.WhenAll(results);
            return results.SelectMany(p => p.Result).ToList();
        }
    }
}
