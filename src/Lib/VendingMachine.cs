namespace Lib
{
    public class VendingMachine
    {
        private int _stockQuantity;
        private readonly decimal _unitPrice;

        private readonly IPinValidationService _pinValidationService;
        private static readonly object Locker = new object();

        public VendingMachine(IPinValidationService pinValidationService, int initialStockQuantity, decimal unitPrice)
        {
            _pinValidationService = pinValidationService;
            _stockQuantity = initialStockQuantity;
            _unitPrice = unitPrice;
        }

        public bool VendCan(VendingCard vendingCard, int pin, int quantityToVend)
        {
            var vendingAccount = vendingCard.VendingAccount;
            if (!_pinValidationService.IsPinValid((pin))) return false;
            if (quantityToVend > _stockQuantity) return false;
            if (vendingAccount.Balance < _unitPrice) return false;

            lock (Locker)
            {
                _stockQuantity = _stockQuantity - quantityToVend;
                vendingAccount.Balance = vendingAccount.Balance - _unitPrice;
            }
            return true;
        }
    }

}
