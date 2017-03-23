namespace Lib
{
    public class VendingCard
    {
        public VendingAccount VendingAccount { get; }

        public VendingCard(VendingAccount vendingAccount)
        {
            VendingAccount = vendingAccount;
        }
    }

}
