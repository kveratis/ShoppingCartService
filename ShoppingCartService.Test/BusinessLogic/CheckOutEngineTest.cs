using ShoppingCartService.BusinessLogic;

namespace ShoppingCartService.Test.BusinessLogic
{
    public sealed class CheckOutEngineTest
    {
        [Fact]
        public void Should_Calculate_Total_Of_Zero_With_Empty_Cart()
        {
            var shippingCalculator = new ShippingCalculator();
            var mapper = TestHelper.GetMapper();
            var emptyCart = TestHelper.CreateEmptyCart();
            var sut = new CheckOutEngine(shippingCalculator, mapper);

            var dto = sut.CalculateTotals(emptyCart);

            Assert.Equal(0d, dto.Total);
        }

        [Fact]
        public void Should_Calculate_Total_For_StandardCustomer_With_No_Discount()
        {
            var shippingCalculator = new ShippingCalculator();
            var mapper = TestHelper.GetMapper();
            var cart = TestHelper.CreateCartWithItems();
            var sut = new CheckOutEngine(shippingCalculator, mapper);

            var dto = sut.CalculateTotals(cart);

            Assert.Equal(0d, dto.CustomerDiscount);
        }

        [Fact]
        public void Should_Calculate_Total_For_StandardCustomer_Equals_Cost_Plus_Shipping()
        {
            var shippingCalculator = new ShippingCalculator();
            var mapper = TestHelper.GetMapper();
            var cart = TestHelper.CreateCartWithItems();
            var sut = new CheckOutEngine(shippingCalculator, mapper);

            var dto = sut.CalculateTotals(cart);

            Assert.Equal(85d, dto.Total);
        }

        [Fact]
        public void Should_Calculate_Total_For_PremiumCustomer_With_Discount()
        {
            var shippingCalculator = new ShippingCalculator();
            var mapper = TestHelper.GetMapper();
            var cart = TestHelper.CreateCartWithItems(Models.CustomerType.Premium);
            var sut = new CheckOutEngine(shippingCalculator, mapper);

            var dto = sut.CalculateTotals(cart);

            Assert.Equal(10d, dto.CustomerDiscount);
        }
    }
}
