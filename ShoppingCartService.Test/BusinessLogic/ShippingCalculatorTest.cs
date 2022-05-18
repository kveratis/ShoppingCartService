using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;


namespace ShoppingCartService.Test.BusinessLogic
{
    public sealed class ShippingCalculatorTest
    {
        [Fact]
        public void Should_Return_Zero_Cost_With_Empty_Cart()
        {
            var cart = CreateCart();
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
            var cart = CreateCartWithItems(customerType, shippingMethod);
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
            var cart = CreateCartWithItems();
            cart.ShippingAddress = CreateAddress(customerCity, customerCountry);
            var sut = new ShippingCalculator(CreateAddress(warehouseCity, warehouseCountry));

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(expectedCost, cost);
        }

        private static Address CreateAddress(string city = "Dallas", string country = "USA")
        {
            return new Address
            {
                Street = "123 Jolly Lane",
                City = city,
                Country = country,
            };
        }

        private static Cart CreateCart(CustomerType customerType = CustomerType.Standard, ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            return new Cart
            {
                Items = new List<Item>(),
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                ShippingAddress = CreateAddress()
            };
        }

        private static Cart CreateCartWithItems(CustomerType customerType = CustomerType.Standard, ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            var cart = CreateCart(customerType, shippingMethod);

            cart.Items.Add(new Item
            {
                Quantity = 2,
            });
            cart.Items.Add(new Item
            {
                Quantity = 3,
            });

            return cart;
        }
    }
}
