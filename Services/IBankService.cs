using System.Collections.Generic;

namespace server.Services
{
    public interface IBankService
    {
        IEnumerable<Bank> GetBanks();
    }
}
