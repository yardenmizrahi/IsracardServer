namespace server.Services
{
    public class IncreaseCreditLimitRequest
    {
        public string CardNumber { get; set; }
        public int RequestedFrameAmount { get; set; }
        public User User { get; set; }
    }
}
