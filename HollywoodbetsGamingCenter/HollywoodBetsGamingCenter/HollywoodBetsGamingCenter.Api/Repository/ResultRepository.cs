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
    public class ResultRepository : IRepository<ResultModel>
    {
        private readonly IBucket _bucket;

        public ResultRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("gamedb");
        }

        #region Query Methods

        public async Task<List<ResultModel>> Get()
        {
            var query = $"select * from gamedb where type='result' ";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<ResultModel>();
            foreach (var row in result.Rows)
            {
                var obj = new ResultModel();
                obj.Created = row.gamedb.created;
                obj.ResultID = row.gamedb.resultID;
                obj.User.GameID = row.gamedb.user.gameID;
                obj.User.IDNo = row.gamedb.user.idNo;
                obj.User.Name = row.gamedb.user.name;
                obj.User.PhoneNo = row.gamedb.user.phoneNo;
                results.Add(obj);
            }

            return results;
        }

        public async Task<List<ResultModel>> GetByParent(string ID)
        {
            var query = $"select * from gamedb where type='result' and gameID='{ID}'";
            var result = await _bucket.QueryAsync<dynamic>(query);
            var results = new List<ResultModel>();
            foreach (var row in result.Rows)
            {
                var obj = new ResultModel();
                obj.Created = row.gamedb.created;
                obj.ResultID = row.gamedb.resultID;
                obj.User.GameID = row.gamedb.user.gameID;
                obj.User.IDNo = row.gamedb.user.idNo;
                obj.User.Name = row.gamedb.user.name;
                obj.User.PhoneNo = row.gamedb.user.phoneNo;
                results.Add(obj);
            }

            return results;
        }

        public Task<ResultModel> GetBy(string ID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Persist Methods

        public async Task<string> Upsert(ResultModel data)
        {
            var id = Guid.NewGuid().ToString();
            data.ResultID = id;

            var doc = new Document<ResultModel> { Id = id, Content = data };

            var result = await _bucket.InsertAsync<ResultModel>(doc);

            if (result.Status == Couchbase.IO.ResponseStatus.Success)
                return id;
            else
                return string.Empty;
        }

        #endregion
    }
}
