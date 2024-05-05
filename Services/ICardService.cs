using System.Collections.Generic;

namespace server.Services
{
    public interface ICardService
    {
        IEnumerable<Card> GetCards(CardFilter filter);
        OperationResult IncreaseCreditLimit(IncreaseCreditLimitRequest request);
    }
}
