using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace server.Services
{
    public class BankService : IBankService
    {
        private readonly IMemoryCache _cache;

        public BankService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<Bank> GetBanks()
        {
            // Try to get the list of banks from the cache
            if (!_cache.TryGetValue("Banks", out IEnumerable<Bank> banks))
            {
                // Cache entry not found or expired, fetch the data from the source
                banks = LoadBanksFromSource();

                // Store the data in the cache with a 5-minute expiration
                _cache.Set("Banks", banks, TimeSpan.FromMinutes(5));
            }

            return banks;
        }

        private IEnumerable<Bank> LoadBanksFromSource()
        {
            return new List<Bank>
            {
                new Bank { Code = "001", Name = "Bank A", Description = "Description of Bank A" },
                new Bank { Code = "002", Name = "Bank B", Description = "Description of Bank B" },
                new Bank { Code = "003", Name = "Bank C", Description = "Description of Bank C" },
                new Bank { Code = "004", Name = "Bank D", Description = "Description of Bank D" },
            };
        }
    }
}
