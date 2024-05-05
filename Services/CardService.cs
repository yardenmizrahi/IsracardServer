using System;
using System.Collections.Generic;
using System.Linq;

namespace server.Services
{
    public class CardService : ICardService
    {
        private readonly List<Card> _cards;
        public List<String> Status = ["EMPLOYED", "SELF_EMPLOYED", "OTHER"];

        public CardService() =>
            // Initialize mock data for cards
            _cards = new List<Card>
            {
                new Card { CardNumber = "1", CardIssueDate = new DateTime (2024, 6, 1, 7, 47, 0), ImageUrl = "images\\card1.jpg", ImageName = "card1",
                     IsBlocked = false, IsDigital = false, CardFrame = 5000, BankCode = "001" }, // Add mock card objects
                new Card { CardNumber = "2", CardIssueDate = new DateTime (2024, 12, 1, 7, 47, 0), ImageUrl = "images\\card2.jpg", ImageName = "card2",
                     IsBlocked = true, IsDigital = false, CardFrame = 10000, BankCode = "002" }, 
                new Card { CardNumber = "3", CardIssueDate = new DateTime (2026, 12, 1, 7, 47, 0), ImageUrl = "images\\card3.jpg", ImageName = "card3",
                     IsBlocked = false, IsDigital = true, CardFrame = 12000, BankCode = "003" }, 
                new Card { CardNumber = "4", CardIssueDate = new DateTime (2026, 8, 1, 7, 47, 0), ImageUrl = "images\\card4.jpg", ImageName = "card4",
                     IsBlocked = false, IsDigital = true, CardFrame = 4500, BankCode = "004" }, 
            };
        public IEnumerable<Card> GetCards(CardFilter filter)
        {
            try
            {
                var query = _cards.AsQueryable();

                if (filter.Blocked.HasValue)
                {
                    query = query.Where(c => c.IsBlocked == filter.Blocked.Value);
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while retrieving cards: {ex}");

                // Throw a custom exception or return an error response
                throw new ServiceException("An error occurred while retrieving cards. Please try again later.", ex);
            }
        }

        public OperationResult IncreaseCreditLimit(IncreaseCreditLimitRequest request)
        {
            try
            {
                // Validate the requested frame amount
                if (request.RequestedFrameAmount <= 0)
                {
                    return new OperationResult(false, "Requested frame amount must be greater than 0.");
                }


                // Validate the average monthly income
                if (request.User.AverageMonthlyIncome <= 0)
                {
                    return new OperationResult(false, "Average monthly income must be greater than 0.");
                }

                // Determine the maximum frame amount based on occupation and income
                int maxFrameAmount;
                if (request.User.Occupation == "EMPLOYED")
                {
                    maxFrameAmount = request.User.AverageMonthlyIncome / 2;
                }
                else if (request.User.Occupation == "SELF_EMPLOYED")
                {
                    maxFrameAmount = request.User.AverageMonthlyIncome / 3;
                }
                else
                {
                    maxFrameAmount = 0; // No increase for other occupations
                }

                // Check if requested frame amount exceeds the maximum allowed
                if (request.RequestedFrameAmount > maxFrameAmount)
                {
                    return new OperationResult(false, $"Requested frame amount exceeds the maximum allowed for {request.User.Occupation} occupation.");
                }

                // Check if the card is blocked
                if (IsCardBlocked(request.CardNumber))
                {
                    return new OperationResult(false, "Cannot increase frame for a blocked card.");
                }

                // Check if the average income is less than NIS 12,000
                if (request.User.AverageMonthlyIncome < 12000)
                {
                    return new OperationResult(false, "Average monthly income is less than NIS 12,000. Frame increase cannot be approved.");
                }

                // Check if the card was issued in the last 3 months
                if (IsCardIssuedRecently(getCardByNumber(request.CardNumber)))
                {
                    return new OperationResult(false, "Card was issued in the last 3 months. Frame increase cannot be approved.");
                }

                // Update card details with the new frame amount
                UpdateCardFrame(request.CardNumber, request.RequestedFrameAmount);

                return new OperationResult(true, "Frame increase approved and card details updated.");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while increasing credit limit: {ex}");

                // Throw a custom exception or return an error response
                throw new ServiceException("An error occurred while increasing credit limit. Please try again later.", ex);
            }
        }

        private void UpdateCardFrame(object cardNumber, int requestedFrameAmount)
        {
            foreach (Card card in _cards)
            {
                if (card.CardNumber == cardNumber)
                {
                    card.CardFrame = requestedFrameAmount;
                }
            }
        }

        private bool IsCardIssuedRecently(object cardNumber)
        {
            DateTime date = DateTime.Now;
            if (cardNumber == null)
            {
                return false; // Handle null parameters gracefully
            }
            foreach (Card card in _cards)
            {
                if (card.CardNumber == cardNumber)
                {
                    if (card.CardIssueDate.Year > date.Year) return false;
                    else if (date.Month - card.CardIssueDate.Month > 3) return false;
                    else return true;
                }
            }
            return false;
        }

        private bool IsCardBlocked(object cardNumber)
        {
            foreach(Card card in _cards)
            {
                if(card.CardNumber == cardNumber)
                {
                    if(card.IsBlocked) return true;
                }
            }
            return false;
        }

        private Card getCardByNumber(object cardNumber)
        {
            foreach (Card card in _cards)
            {
                if (card.CardNumber.Equals(cardNumber))
                {
                    return (Card) card;
                }
            }
            return null;
        }

    }
}
