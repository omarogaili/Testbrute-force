using Brute_Force_Lab3;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestBruteForce
{
    public class UnitTest1
    {
        [Fact]
        public void TestCorrectLogin()
        {
            // Arrange
            string password = "test";
            string salt = BruteForceAttackSimulator.GenerateSalt();
            string hashedPassword = BruteForceAttackSimulator.HashPassword(password, salt);

            // Act
            bool result = BruteForceAttackSimulator.VerifyPassword(password, hashedPassword, salt);
            // Assert
            Assert.True(result, "Password verification failed.");
        }
        [Fact]
        public void TestBruteForceAttack()
        {
            // Arrange
            string password = "test";
            string salt = BruteForceAttackSimulator.GenerateSalt();
            string hashedPassword = BruteForceAttackSimulator.HashPassword(password, salt);
            BruteForceAttackSimulator bruteForceSimulator = new BruteForceAttackSimulator();
            int maxAttempts = BruteForceAttack.maxAttempts;
            TimeSpan lockoutDuration = BruteForceAttack.lockoutDuration;
            // Act
            bool isPasswordFound = bruteForceSimulator.AttemptBruteForce(hashedPassword, salt, maxAttempts, lockoutDuration);
            // Assert
            Assert.False(isPasswordFound, "Password should not be found.");
            Assert.True(bruteForceSimulator.FailedAttempts >= maxAttempts, "Failed attempts should be equal or greater than max attempts.");
            Assert.True(bruteForceSimulator.LockoutEndTime.HasValue, "Account should be locked out after max attempts.");
        }

    }
}