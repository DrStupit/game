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
    public class ResultController : Controller
    {
        private IRepository<ResultModel> _resultRepo;
        private IRepository<Transaction> _transactionRepo;
        private IRepository<Game> _gameRepo;
        private IRepository<Log> _logger;

        public ResultController(IRepository<ResultModel> resultRepo,
            IRepository<Transaction> transactionRepo,
            IRepository<Game> gameRepo,
            IRepository<Log> logger)
        {
            _resultRepo = resultRepo;
            _transactionRepo = transactionRepo;
            _gameRepo = gameRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _resultRepo.Get();
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                var data = new Log();
                data.MethodName = "ResultController|Get";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string gameID)
        {
            try
            {
                if (string.IsNullOrEmpty(gameID))
                    return BadRequest("ID not found in request.");

                var result = await _resultRepo.GetBy(gameID);
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                var data = new Log();
                data.MethodName = "ResultController|Get{id}";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string gameID)
        {
            try
            {
                var results = new List<ResultModel>();

                var transactionsForGame = await _transactionRepo.GetByParent(gameID);

                if (transactionsForGame.Count == 0)
                    return NotFound("No transactions for this game.");

                var transactions = transactionsForGame
                    .Where(r => r.IsMonetary == false)
                    .OrderByDescending(r => r.Created)
                    .ToList();

                var userNumbers = new List<int>();

                for (int i = 0; i < 3; i++)
                {
                    var obj = new ResultModel();

                    var randy = new Random();
                    int index;

                    index = randy.Next(transactions.Count);

                    if (userNumbers.Contains(index) == true)
                        index = randy.Next(transactions.Count);

                    userNumbers.Add(index);

                    var winner = transactions[index];

                    obj.Created = DateTime.Now;
                    obj.GameID = gameID;
                    obj.User = winner.User;

                    await _resultRepo.Upsert(obj);

                    results.Add(obj);
                }

                var game = await _gameRepo.GetBy(gameID);
                game.IsResulted = true;

                await _gameRepo.Upsert(game);

                return Ok(results);
            }
            catch (Exception ex)
            {
                var data = new Log();
                data.MethodName = "ResultController|Post";
                data.Message = ex.ToString();
                await _logger.Upsert(data);
                return BadRequest(ex.Message);
            }
        }
    }
}
