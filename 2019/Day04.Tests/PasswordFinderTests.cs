using System;
using System.Linq;
using Xunit;

namespace Day04.Tests
{
    public class PasswordFinderTests
    {
        [Fact]
        public void Range_111111_111111_Returns_111111()
        {
            var password = 111111;
            var passwordFinder = new PasswordFinder(password, password);
            Assert.True(passwordFinder.FoundPasswords);
            Assert.Equal(password, passwordFinder.Passwords.First());
        }

        [Fact]
        public void Range_122345_122345_Returns_122345()
        {
            var password = 122345;
            var passwordFinder = new PasswordFinder(password, password);
            Assert.True(passwordFinder.FoundPasswords);
            Assert.Equal(password, passwordFinder.Passwords.First());
        }

        [Fact]
        public void Range_112233_112233_Returns_112233()
        {
            var password = 112233;
            var passwordFinder = new PasswordFinder(password, password);
            Assert.True(passwordFinder.FoundPasswords);
            Assert.Equal(password, passwordFinder.Passwords.First());
        }

        [Fact]
        public void Range_111122_111122_Returns_111122()
        {
            var password = 111122;
            var passwordFinder = new PasswordFinder(password, password);
            Assert.True(passwordFinder.FoundPasswords);
            Assert.Equal(password, passwordFinder.Passwords.First());
        }

        [Fact]
        public void Range_223450_223450_Does_Not_Find_Passwords()
        {
            var password = 223450;
            var passwordFinder = new PasswordFinder(password, password);

            Assert.False(passwordFinder.FoundPasswords);
        }

        [Fact]
        public void Range_123789_123789_Does_Not_Find_Passwords()
        {
            var password = 123789;
            var passwordFinder = new PasswordFinder(password, password);

            Assert.False(passwordFinder.FoundPasswords);
        }


        [Fact]
        public void Range_123444_123444_Does_Not_Find_Passwords()
        {
            var password = 123444;
            var passwordFinder = new PasswordFinder(password, password);

            Assert.False(passwordFinder.FoundPasswords);
        }

    }
}
