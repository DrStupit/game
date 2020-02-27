using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HollywoodBetsGamingCenter.Api.Interfaces;
using HollywoodBetsGamingCenter.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HollywoodBetsGamingCenter.Api.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private IRepository<Transaction> _repo;
        private IRepository<Log> _logger;

        public TransactionController(IRepository<Transaction> repo, IRepository<Log> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = await _repo.Get();
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (System.Exception ex)
            {
                var data = new Log();
                data.MethodName = "TransactionController|Get";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string gameID)
        {
            try
            {
                var result = await _repo.GetByParent(gameID);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (System.Exception ex)
            {
                var data = new Log();
                data.MethodName = "TransactionController|GetByParent";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }
    }
}
