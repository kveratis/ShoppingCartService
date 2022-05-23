using ShoppingCartService.BusinessLogic;
using ShoppingCartService.Models;


namespace ShoppingCartService.Test.BusinessLogic
{
    public sealed class ShippingCalculatorTest
    {
        [Fact]
        public void Should_Return_Zero_Cost_With_Empty_Cart()
        {
            var cart = TestHelper.CreateCart();
            var sut = new ShippingCalculator();

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(0d, cost);
        }

        [Theory]
        [InlineData(CustomerType.Standard, ShippingMethod.Standard, 5d)]
        [InlineData(CustomerType.Standard, ShippingMethod.Expedited, 6d)]
        [InlineData(CustomerType.Standard, ShippingMethod.Priority, 10d)]
        [InlineData(CustomerType.Standard, ShippingMethod.Express, 12.5d)]
        [InlineData(CustomerType.Premium, ShippingMethod.Standard, 5d)]
        [InlineData(CustomerType.Premium, ShippingMethod.Expedited, 5d)]
        [InlineData(CustomerType.Premium, ShippingMethod.Priority, 5d)]
        [InlineData(CustomerType.Premium, ShippingMethod.Express, 12.5d)]
        public void Should_Return_Varied_Cost_Depending_On_Customer_Type_And_Shipping_Type(CustomerType customerType, ShippingMethod shippingMethod, double expectedCost)
        {
            var cart = TestHelper.CreateCartWithItems(customerType, shippingMethod);
            var sut = new ShippingCalculator();

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(expectedCost, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", "Dallas", "USA", 5d)]
        [InlineData("Dallas", "USA", "Austin", "USA", 10d)]
        [InlineData("Dallas", "USA", "Mexico City", "Mexico", 75d)]
        [InlineData("Austin", "USA", "Dallas", "USA", 10d)]
        [InlineData("Mexico City", "Mexico", "Dallas", "USA", 75d)]
        public void Should_Return_Varied_Cost_Depending_On_Customer_Location_Relative_To_Warehouse_Locationstring(string customerCity, string customerCountry, string warehouseCity, string warehouseCountry, double expectedCost)
        {
            var cart = TestHelper.CreateCartWithItems();
            cart.ShippingAddress = TestHelper.CreateAddress(customerCity, customerCountry);
            var sut = new ShippingCalculator(TestHelper.CreateAddress(warehouseCity, warehouseCountry));

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(expectedCost, cost);
        }
    }
}
