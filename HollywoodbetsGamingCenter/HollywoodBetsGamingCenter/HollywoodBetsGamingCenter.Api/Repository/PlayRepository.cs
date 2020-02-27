using HollywoodBetsGamingCenter.Api.Interfaces;
using HollywoodBetsGamingCenter.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Repository
{
    public class PlayRepository
    {
        private IRepository<Game> _gameRepo;
        private IRepository<Prize> _prizeRepo;
        private IRepository<Transaction> _transactionRepo;

        private Game _game;
        private List<Prize> _Prizes;
        private User _user;
        private List<Transaction> _Transactions;

        public PlayRepository(IRepository<Transaction> transactionRepo,
            IRepository<Game> gameRepo,
            IRepository<Prize> prizeRepo,
            User user)
        {
            _transactionRepo = transactionRepo;
            _gameRepo = gameRepo;
            _prizeRepo = prizeRepo;
            _user = user;

            _game = _gameRepo.GetBy(user.GameID.ToString()).Result;
            _Prizes = _prizeRepo.Get().Result;
            _Transactions = transactionRepo.GetByParent(user.GameID.ToString()).Result;
        }

        public ResponseMessage<Prize> Start()
        {
            var response = new ResponseMessage<Prize>();
            var prize = new Prize();
            int transactionCount = 1;

            if (UserHasPlayedToday() == true)
            {
                response.Data = null;
                response.Message = $"Unfortunately you can only play once per day. This competition is valid till {_game.EndDate.ToString("dd-MMM-yyyy")}";
                return response;
            }

            try
            {
                if (DailyLimitBeenIsReached() == true)
                {
                    prize = GetPrize(false);
                    transactionCount = prize.PrizeValue;
                }
                else
                {
                    if (WastheLastTransactionsWin() == true)
                    {
                        prize = GetPrize(false);
                        transactionCount = prize.PrizeValue;
                    }
                    else
                        prize = GetPrize(true);
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = "An error has occured. Please try again later :(";
            }

            for (int i = 0; i < transactionCount; i++)
            {
                CreateTransaction(_user, prize);
            }

            response.Data = prize;
            response.Message = "success";
            return response;
        }

        #region Helper Methods

        private async void CreateTransaction(User user, Prize prize)
        {
            var transaction = new Transaction();
            transaction.Created = DateTime.Now;
            transaction.IsMonetary = prize.IsMonetary;
            transaction.Prize = prize;
            transaction.User = user;

            await _transactionRepo.Upsert(transaction);
        }

        private bool DailyLimitBeenIsReached()
        {
            var moneyTransactions = _Transactions.Where(r => r.IsMonetary == true && r.Created.Date == DateTime.Today.Date).ToList();
            int value = moneyTransactions.Sum(r => r.Prize.PrizeValue);

            if (value < _game.DailyBudget)
                return false;
            else
                return true;
        }

        private Prize GetPrize(bool monetary)
        {
            var moneyPrizes = _Prizes.Where(r => r.Active == true && r.IsMonetary == monetary).ToList();
            var randy = new Random();
            var index = randy.Next(0, moneyPrizes.Count);
            return moneyPrizes[index];
        }

        private bool WastheLastTransactionsWin()
        {
            var transactions = _Transactions
                .Where(r => r.Created.Date == DateTime.Today.Date)
                .OrderByDescending(r => r.Created)
                .ToList();

            if (transactions.Count < 4)
                return false;

            var randy = new Random();
            var index = randy.Next(4);
            var result = transactions[index];
            return result.IsMonetary;
        }

        private bool UserHasPlayedToday()
        {
            var transactions = _Transactions.Where(r => r.Created.Date == DateTime.Today.Date).ToList();
            var result = transactions.Any(r => r.User.IDNo == _user.IDNo || r.User.PhoneNo == _user.PhoneNo);

            return result;
        }

        private bool DidUserPlayYesterday()
        {
            var transactions = _Transactions.Where(r => r.Created == DateTime.Today.AddDays(-1)).ToList();
            return transactions.Any(r => r.User.IDNo == _user.IDNo);
        }

        #endregion

    }
}
