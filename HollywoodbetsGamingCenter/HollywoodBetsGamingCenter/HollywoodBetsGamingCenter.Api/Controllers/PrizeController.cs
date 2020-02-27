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
    public class PrizeController : Controller
    {
        private IRepository<Prize> _repo;
        private IRepository<Log> _logger;

        public PrizeController(IRepository<Prize> repo, IRepository<Log> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
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
                data.MethodName = "PrizeConfigController|Get";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("ID not found in request.");

                var result = await _repo.GetBy(id);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (System.Exception ex)
            {
                var data = new Log();
                data.MethodName = "PrizeConfigController|Get{id}";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Prize data)
        {
            try
            {
                if (data == null)
                    return BadRequest("Request body not found.");

                var result = await _repo.Upsert(data);
                if (!string.IsNullOrEmpty(result))
                    return Ok(result);
                else
                    return BadRequest("ID not found in request.");
            }
            catch (System.Exception ex)
            {
                var obj = new Log();
                obj.MethodName = "PrizeConfigController|Post";
                obj.Message = ex.ToString();
                await _logger.Upsert(obj);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("/GetByParent/{id}", Name = "GetByParent")]
        public async Task<IActionResult> GetByParent(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("ID not found in request.");

                var result = await _repo.GetByParent(id);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (System.Exception ex)
            {
                var obj = new Log();
                obj.MethodName = "PrizeConfigController|GetByParent";
                obj.Message = ex.ToString();
                await _logger.Upsert(obj);
                return BadRequest(ex.Message);
            }
        }
    }
}
