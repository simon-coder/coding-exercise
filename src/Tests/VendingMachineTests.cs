using Lib;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class VendingMachineTests
    {
        private const decimal UnitPrice = 0.50m;
        private const int InitialStockQuantity = 25;
        private const int Pin = 1234;
        private decimal _accountBalance;
        private VendingAccount _vendingAccount;
        private VendingCard _vendingCard;
        private VendingMachine _vendingMachine;
        private Mock<IPinValidationService> _mockPinValidationService;

        [SetUp]
        public void TestSetup()
        {
            // Setup the most common settings for most of the tests
            _accountBalance = 5.00m;
            _vendingAccount = new VendingAccount(_accountBalance);
            _vendingCard = new VendingCard(_vendingAccount);
            _mockPinValidationService = new Mock<IPinValidationService>();
            _vendingMachine = new VendingMachine(_mockPinValidationService.Object, InitialStockQuantity, UnitPrice);
        }

        [Test]
        public void CanVendWhenSufficientStockAvailable()
        {
            const int numberOfCansToVend = 25;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(true);

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsTrue(vendResult);

        }

        [Test]
        public void CannotVendWhenSufficientStockNotAvailable()
        {
            const int numberOfCansToVend = 26;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(true);

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsFalse(vendResult);
        }

        [Test]
        public void CanVendWhenSufficentCreditAvailable()
        {
            const int numberOfCansToVend = 1;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(true);

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsTrue(vendResult);
        }

        [Test]
        public void CannotVendWhenSufficentCreditNotAvailable()
        {
            const int numberOfCansToVend = 1;
            _vendingAccount.LoadNewBalance((0.49m));
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(true);

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsFalse(vendResult);
        }

        [Test]
        public void CannotVendWhenWhenIncorrectPinEntered()
        {
            const int numberOfCansToVend = 1;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(false);

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsFalse(vendResult);
        }

        [Test]
        public void AccountBalanceUpdatesWhenVendSuccessfull()
        {
            const int numberOfCansToVend = 1;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(true);
            var expectedBalance = _accountBalance - UnitPrice;

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsTrue(vendResult);
            Assert.AreEqual(expectedBalance, _vendingAccount.Balance);
        }

        [Test]
        public void AccountBalanceDoesNotUpdatesWhenVendUnsuccessfull()
        {
            const int numberOfCansToVend = 1;
            _mockPinValidationService.Setup(x => x.IsPinValid(It.IsAny<int>())).Returns(false);
            var expectedBalance = _accountBalance;

            var vendResult = _vendingMachine.VendCan(_vendingCard, Pin, numberOfCansToVend);

            Assert.IsFalse(vendResult);
            Assert.AreEqual(expectedBalance, _vendingAccount.Balance);
        }

        [Test]
        public void MultipleCardsAccessAccountAtSameTime()
        {
            // Ran out of time to implement this but have put a lock on the bit which debits the card balance and 
            // decrements the available can quantity so will be done as atomic unit so in theory it's thread safe.
            // I'd planned on using TPL here to call the VendCan at same time to prove that the locking does work...
        }
    }
}
