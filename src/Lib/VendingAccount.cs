namespace Lib
{
    public class VendingAccount
    {
        public decimal Balance { get; set; }

        public VendingAccount(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public void LoadNewBalance(decimal newBalance)
        {
            Balance = newBalance;
        }
    }
}
