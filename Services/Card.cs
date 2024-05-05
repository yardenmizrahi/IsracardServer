namespace server.Services
{
    public class Card
    {
        public string CardNumber { get; set; }
        public DateTime CardIssueDate { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDigital { get; set; }
        public int CardFrame { get; set; }
        public string BankCode { get; set; }

    }
}
