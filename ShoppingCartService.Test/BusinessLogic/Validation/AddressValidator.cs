using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;

namespace ShoppingCartService.Test.BusinessLogic.Validation
{
    public sealed  class AddressValidatorShould
    {
        [Fact]
        public void Require_Address()
        {
            var sut = new AddressValidator();

            Assert.False(sut.IsValid(null));
        }

        [Fact]
        public void Require_Address_Have_Country()
        {
            var sut = new AddressValidator();
            var address = CreateAddress();
            var nullAddress = address with { Country = null };
            var emptyAddress = address with { Country = String.Empty };
            
            Assert.False(sut.IsValid(nullAddress));
            Assert.False(sut.IsValid(emptyAddress));
        }

        [Fact]
        public void Require_Address_Have_City()
        {
            var sut = new AddressValidator();
            var address = CreateAddress();
            var nullAddress = address with { City = null };
            var emptyAddress = address with { City = String.Empty };

            Assert.False(sut.IsValid(nullAddress));
            Assert.False(sut.IsValid(emptyAddress));
        }

        [Fact]
        public void Require_Address_Have_Street()
        {
            var sut = new AddressValidator();
            var address = CreateAddress();
            var nullAddress = address with { Street = null };
            var emptyAddress = address with { Street = String.Empty };

            Assert.False(sut.IsValid(nullAddress));
            Assert.False(sut.IsValid(emptyAddress));
        }

        [Fact]
        public void Approve_Valid_Address()
        {
            var sut = new AddressValidator();
            var address = CreateAddress();

            Assert.True(sut.IsValid(address));
        }

        private static Address CreateAddress()
        {
            return new Address
            {
                Street = "123 Jolly Ln.",
                City = "Somewhere",
                Country = "United States"
            };
        }
    }
}
