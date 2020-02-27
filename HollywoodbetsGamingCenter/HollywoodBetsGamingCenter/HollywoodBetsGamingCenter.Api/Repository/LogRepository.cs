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
    public class LogRepository : IRepository<Log>
    {
        private readonly IBucket _bucket;

        public LogRepository(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("gamedb");
        }

        #region Query Methods

        public Task<List<Log>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Log> GetBy(string ID)
        {
            throw new NotImplementedException();
        }

        public Task<List<Log>> GetByParent(string ID)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Persist Methods

        public async Task<string> Upsert(Log data)
        {
            string id = Guid.NewGuid().ToString();
            data.LogID = id;

            var doc = new Document<Log> { Id = id, Content = data };

            var result = await _bucket.UpsertAsync<Log>(doc);

            if (result.Status == Couchbase.IO.ResponseStatus.Success)
                return id;
            else
                return string.Empty;
        }

        #endregion
    }
}
