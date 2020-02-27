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
    public class GameRepository : IRepository<Game>
    {
        private readonly IBucket _bucket;

        public GameRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("gamedb");
        }

        public async Task<List<Game>> Get()
        {
            var query = "select * from gamedb where type='game'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<Game>();
            foreach (var row in result.Rows)
            {
                var obj = new Game();
                obj.DailyBudget = row.gamedb.dailyBudget;
                obj.EndDate = row.gamedb.endDate;
                obj.GameID = row.gamedb.gameID;
                obj.GameName = row.gamedb.gameName;
                obj.StartDate = row.gamedb.startDate;
                results.Add(obj);
            }

            return results;
        }

        public async Task<Game> GetBy(string ID)
        {
            var result = await _bucket.GetDocumentAsync<Game>(ID);
            return result.Document.Content;
        }

        public async Task<string> Upsert(Game data)
        {
            string id = "";
            if (string.IsNullOrEmpty(data.GameID.ToString()))
            {
                id = Guid.NewGuid().ToString();
                data.GameID = id;
            }
            else
                id = data.GameID.ToString();

            var doc = new Document<Game> { Id = id, Content = data };

            var result = await _bucket.UpsertAsync<Game>(doc);

            if (result.Status == Couchbase.IO.ResponseStatus.Success)
                return id;
            else
                return string.Empty;
        }

        public Task<List<Game>> GetByParent(string ID)
        {
            throw new NotImplementedException();
        }
    }
}
