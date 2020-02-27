using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HollywoodBetsGamingCenter.Api.Interfaces;
using HollywoodBetsGamingCenter.Api.Models;
using HollywoodBetsGamingCenter.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HollywoodBetsGamingCenter.Api.Controllers
{
    [Route("api/[controller]")]
    public class PlayController : Controller
    {
        private IRepository<Game> _gameRepo;
        private IRepository<Prize> _prizeRepo;
        private IRepository<Transaction> _transactionRepo;
        private IRepository<Log> _logger;

        public PlayController(IRepository<Transaction> transactionRepo,
            IRepository<Game> gameRepo,
            IRepository<Prize> prizeRepo,
            IRepository<Log> logger)
        {
            _transactionRepo = transactionRepo;
            _gameRepo = gameRepo;
            _prizeRepo = prizeRepo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]User user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.Name)
               || string.IsNullOrEmpty(user.GameID))
                    return BadRequest("User not supplied correctly.");

                if (user.PhoneNo.ToString().Length != 10)
                    return BadRequest("Cell Number seems to be in the wrong format.");

                if (user.IDNo.ToString().Length != 13)
                    return BadRequest("ID Number seems to be in the wrong format.");

                var subYear = user.IDNo.ToString().Substring(0, 2);
                int year = Convert.ToInt32($"19{subYear}");

                if (ValidationRepository.EighteenYearsOrOlder(year) == false)
                    return BadRequest("User must be 18 years or older.");

                var play = new PlayRepository(_transactionRepo, _gameRepo, _prizeRepo, user);
                var msg = play.Start();

                return Ok(msg);
            }
            catch (Exception ex)
            {
                var obj = new Log();
                obj.MethodName = "PlayController|Post";
                obj.Message = ex.ToString();
                await _logger.Upsert(obj);
                return BadRequest(ex.Message);
            }
        }

    }
}
