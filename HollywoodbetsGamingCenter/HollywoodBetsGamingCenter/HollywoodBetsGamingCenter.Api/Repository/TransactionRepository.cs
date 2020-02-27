using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using HollywoodBetsGamingCenter.Api.Interfaces;
using HollywoodBetsGamingCenter.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Repository
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly IBucket _bucket;

        public TransactionRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("gamedb");
        }

        #region Query Methods

        public async Task<List<Transaction>> Get()
        {
            var query = "select * from gamedb where type='transaction'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<Transaction>();
            foreach (var row in result.Rows)
            {
                var obj = new Transaction();
                obj.Created = row.gamedb.created;
                obj.IsMonetary = row.gamedb.isMonetary;
                obj.TransactionID = row.gamedb.transactionID;
                obj.User.GameID = row.gamedb.user.gameID;
                obj.User.IDNo = row.gamedb.user.idNo;
                obj.User.Name = row.gamedb.user.name;
                obj.User.PhoneNo = row.gamedb.user.phoneNo;
                obj.Prize.PrizeID = row.gamedb.prize.prizeID;
                obj.Prize.GameID = row.gamedb.prize.gameID;
                obj.Prize.PrizeName = row.gamedb.prize.prizeName;
                obj.Prize.PrizeValue = row.gamedb.prize.prizeValue;
                obj.Prize.IsMonetary = row.gamedb.prize.isMonetary;
                obj.Prize.Active = row.gamedb.prize.active;
                results.Add(obj);
            }

            return results;
        }

        public async Task<List<Transaction>> GetByParent(string ID)
        {
            var query = $"select * from gamedb where type='transaction' and `user`.gameID='{ID}'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<Transaction>();
            foreach (var row in result.Rows)
            {
                var obj = new Transaction();
                obj.Created = row.gamedb.created;
                obj.IsMonetary = row.gamedb.isMonetary;
                obj.TransactionID = row.gamedb.transactionID;
                obj.User.GameID = row.gamedb.user.gameID;
                obj.User.IDNo = row.gamedb.user.idNo;
                obj.User.Name = row.gamedb.user.name;
                obj.User.PhoneNo = row.gamedb.user.phoneNo;
                obj.Prize.PrizeID = row.gamedb.prize.prizeID;
                obj.Prize.GameID = row.gamedb.prize.gameID;
                obj.Prize.PrizeName = row.gamedb.prize.prizeName;
                obj.Prize.PrizeValue = row.gamedb.prize.prizeValue;
                obj.Prize.IsMonetary = row.gamedb.prize.isMonetary;
                obj.Prize.Active = row.gamedb.prize.active;
                results.Add(obj);
            }

            return results;
        }

        public Task<Transaction> GetBy(string ID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Persist Methods

        public async Task<string> Upsert(Transaction data)
        {
            var id = Guid.NewGuid().ToString();
            data.TransactionID = id;

            var doc = new Document<Transaction> { Id = id, Content = data };

            var result = await _bucket.InsertAsync<Transaction>(doc);

            if (result.Status == Couchbase.IO.ResponseStatus.Success)
                return id;
            else
                return string.Empty;
        }

        #endregion
    }
}
