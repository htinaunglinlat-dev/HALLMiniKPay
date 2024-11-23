namespace HALLMiniKPay.RestApi.ViewModels
{
    public class WalletViewModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? Pin { get; set; }
        public long? Amount { get; set; }
    }
    public class WithdrawAndDepositViewModel
    {
        public string Phone { get; set; }
        public string Pin { get; set; }
        public long Amount { get; set; }
    }
    public class TransferViewModel
    {
        public string FromPhone{ get; set; }
        public string Pin { get; set; }
        public string ToPhone { get; set; }
        public long Amount { get; set; }
    }
    public class CheckBalanceViewModel
    {
        public string Phone { get; set; }
        public string Pin { get; set; }
    }
}
