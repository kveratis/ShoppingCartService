using AutoMapper;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;

namespace ShoppingCartService.Test
{
    internal static class TestHelper
    {
        public static MapperConfiguration GetMapperConfiguration()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });
        }

        public static IMapper GetMapper()
        {
            return new Mapper(GetMapperConfiguration());
        }

        public static Cart CreateEmptyCart(CustomerType customerType = CustomerType.Standard, ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            return new Cart
            {
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                ShippingAddress = CreateAddress(),
                Items = new List<Item>(),
            };
        }

        public static Address CreateAddress(string city = "Dallas", string country = "USA")
        {
            return new Address
            {
                Street = "123 Jolly Lane",
                City = city,
                Country = country,
            };
        }

        public static Cart CreateCartWithItems(CustomerType customerType = CustomerType.Standard, ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            var cart = CreateEmptyCart(customerType, shippingMethod);

            cart.Items.Add(new Item
            {
                Quantity = 2,
                Price = 10,
            });
            cart.Items.Add(new Item
            {
                Quantity = 3,
                Price = 20,
            });

            return cart;
        }
    }
}
