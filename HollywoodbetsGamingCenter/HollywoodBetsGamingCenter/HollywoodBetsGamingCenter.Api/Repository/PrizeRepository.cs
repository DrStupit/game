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
    public class PrizeRepository : IRepository<Prize>
    {

        private readonly IBucket _bucket;

        public PrizeRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("gamedb");
        }

        #region Query Methods

        public async Task<List<Prize>> Get()
        {
            const string query = "SELECT * FROM gamedb WHERE type = 'prize'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<Prize>();
            foreach (var row in result.Rows)
            {
                var obj = new Prize();
                obj.Active = Convert.ToBoolean(row.gamedb.active);
                obj.GameID = row.gamedb.gameID;
                obj.IsMonetary = Convert.ToBoolean(row.gamedb.isMonetary);
                obj.PrizeID = row.gamedb.prizeID;
                obj.PrizeName = row.gamedb.prizeName;
                obj.PrizeValue = Convert.ToInt32(row.gamedb.prizeValue);
                results.Add(obj);
            }
            return results;
        }

        public async Task<Prize> GetBy(string ID)
        {
            var result = await _bucket.GetDocumentAsync<Prize>(ID);
            return result.Document.Content;
        }

        public async Task<List<Prize>> GetByParent(string ID)
        {
            string query = $"select * from gamedb where type='prize' and gameID= '{ID}'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<Prize>();
            foreach (var row in result.Rows)
            {
                var obj = new Prize();
                obj.Active = Convert.ToBoolean(row.gamedb.active);
                obj.GameID = row.gamedb.gameID;
                obj.IsMonetary = Convert.ToBoolean(row.gamedb.isMonetary);
                obj.PrizeID = row.gamedb.prizeID;
                obj.PrizeName = row.gamedb.prizeName;
                obj.PrizeValue = Convert.ToInt32(row.gamedb.prizeValue);
                results.Add(obj);
            }
            return results;
        }

        #endregion

        #region Persist Methods

        public async Task<string> Upsert(Prize data)
        {
            string id = "";
            if (string.IsNullOrEmpty(data.PrizeID.ToString()))
            {
                id = Guid.NewGuid().ToString();
                data.PrizeID = id;
            }
            else
                id = data.PrizeID.ToString();

            var doc = new Document<Prize> { Id = id, Content = data };

            var result = await _bucket.UpsertAsync<Prize>(doc);

            if (result.Status == Couchbase.IO.ResponseStatus.Success)
                return id;
            else
                return string.Empty;
        }

        #endregion
    }
}
