using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;


namespace ShoppingCartService.Test.BusinessLogic
{
    public sealed class ShippingCalculatorTest
    {
        private const double BaseShippingRate = 1.0;
        private const double ExpeditedShippingRate = 1.2;
        private const double PriorityShippingRate = 2.0;
        private const double ExpressShippingRate = 2.5;

        [Theory]
        [InlineData("Dallas", "USA")]
        [InlineData("Austin", "USA")]
        [InlineData("Mexico City", "Mexico")]
        public void Should_Return_Zero_Cost_With_Empty_Cart(string city, string country)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart();
            
            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(0d, cost);
        }


        [Theory]
        [InlineData("Dallas", "USA", 1, ShippingCalculator.SameCityRate)]
        [InlineData("Dallas", "USA", 2, ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", 1, ShippingCalculator.SameCountryRate)]
        [InlineData("Austin", "USA", 2, ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", 1, ShippingCalculator.InternationalShippingRate)]
        [InlineData("Mexico City", "Mexico", 2, ShippingCalculator.InternationalShippingRate)]
        public void Should_Return_Quantity_Times_Rate_When_Shipping_Standard(string city, string country, uint quantity, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart();
            cart.Items.Add(
                new() { Quantity = quantity }
            );

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(quantity * locationShippingRate, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", 1, ShippingCalculator.SameCityRate)]
        [InlineData("Dallas", "USA", 2, ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", 1, ShippingCalculator.SameCountryRate)]
        [InlineData("Austin", "USA", 2, ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", 1, ShippingCalculator.InternationalShippingRate)]
        [InlineData("Mexico City", "Mexico", 2, ShippingCalculator.InternationalShippingRate)]
        public void Should_Return_Quantity_Times_Rate_When_Shipping_Expedited(string city, string country, uint quantity, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart(shippingMethod: ShippingMethod.Expedited);
            cart.Items.Add(
                new() { Quantity = quantity }
            );

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(quantity * locationShippingRate * ExpeditedShippingRate, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", 1, ShippingCalculator.SameCityRate)]
        [InlineData("Dallas", "USA", 2, ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", 1, ShippingCalculator.SameCountryRate)]
        [InlineData("Austin", "USA", 2, ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", 1, ShippingCalculator.InternationalShippingRate)]
        [InlineData("Mexico City", "Mexico", 2, ShippingCalculator.InternationalShippingRate)]
        public void Should_Return_Quantity_Times_Rate_When_Shipping_Same_City_And_Priority_Shipping(string city, string country, uint quantity, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart(shippingMethod: ShippingMethod.Priority);
            cart.Items.Add(
                new() { Quantity = quantity }
            );

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(quantity * locationShippingRate * PriorityShippingRate, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", 1, ShippingCalculator.SameCityRate)]
        [InlineData("Dallas", "USA", 2, ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", 1, ShippingCalculator.SameCountryRate)]
        [InlineData("Austin", "USA", 2, ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", 1, ShippingCalculator.InternationalShippingRate)]
        [InlineData("Mexico City", "Mexico", 2, ShippingCalculator.InternationalShippingRate)]
        public void Should_Return_Quantity_Times_Rate_When_Shipping_Same_City_And_Express_Shipping(string city, string country, uint quantity, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart(shippingMethod: ShippingMethod.Express);
            cart.Items.Add(
                new() { Quantity = quantity }
            );

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(quantity * locationShippingRate * ExpressShippingRate, cost);
        }

        
        [Theory]
        [InlineData("Dallas", "USA", ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", ShippingCalculator.InternationalShippingRate)]
        public void Should_Return_SumOfItemsQuantity_Times_Rate_When_Shipping_Multiple_Item_Types(string city, string country, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress(city, country);
            var sut = new ShippingCalculator(address);
            var cart = TestHelper.CreateEmptyCart();
            cart.Items.Add(
                new() { Quantity = 2 }
            );
            cart.Items.Add(
                new() { Quantity = 3 }
            );

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(5 * locationShippingRate, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", ShippingCalculator.InternationalShippingRate)]
        public void Should_Not_Pay_Shipping_Rate_When_Premium_Customer_Shipping_Priority(string city, string country, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress();
            var sut = new ShippingCalculator(address);
            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Priority,
                ShippingAddress = TestHelper.CreateAddress(city, country),
                Items = new List<Item>()
                {
                    new Item() { Quantity = 1 }
                },
            };

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(BaseShippingRate * locationShippingRate, cost);
        }

        [Theory]
        [InlineData("Dallas", "USA", ShippingCalculator.SameCityRate)]
        [InlineData("Austin", "USA", ShippingCalculator.SameCountryRate)]
        [InlineData("Mexico City", "Mexico", ShippingCalculator.InternationalShippingRate)]
        public void Should_Not_Pay_Shipping_Rate_When_Premium_Customer_Shipping_Expedited(string city, string country, double locationShippingRate)
        {
            var address = TestHelper.CreateAddress();
            var sut = new ShippingCalculator(address);
            var cart = new Cart
            {
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Expedited,
                ShippingAddress = TestHelper.CreateAddress(city, country),
                Items = new List<Item>()
                {
                    new Item() { Quantity = 1 }
                },
            };

            var cost = sut.CalculateShippingCost(cart);

            Assert.Equal(BaseShippingRate * locationShippingRate, cost);
        }
    }
}
