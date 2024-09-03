using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Brute_Force_Lab3
{
    public class BruteForceAttackSimulator
    {
        private readonly string[] _alphabet = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "å", "ä", "ö" };
        private int[] _positions = { 0, 0, 0, 0, 0, 0 };

        public int FailedAttempts { get; private set; }
        public DateTime? LockoutEndTime { get; private set; }

        public bool AttemptBruteForce(string hashedPassword, string salt, int maxAttempts, TimeSpan lockoutDuration)
        {
            string current = "";
            FailedAttempts = 0;
            LockoutEndTime = null;

            while (!VerifyPassword(current, hashedPassword, salt))
            {
                for (int i = 0; i < _positions.Length; i++)
                {
                    if (_positions[i] == _alphabet.Length)
                    {
                        _positions[i] = 0;
                        if (i + 1 < _positions.Length)
                        {
                            _positions[i + 1]++;
                        }
                    }
                }

                current = (_alphabet[_positions[5]] + _alphabet[_positions[4]] + _alphabet[_positions[3]] + _alphabet[_positions[2]] + _alphabet[_positions[1]] + _alphabet[_positions[0]]).ToString();
                _positions[0]++;

                FailedAttempts++;
                if (FailedAttempts >= maxAttempts)
                {
                    LockoutEndTime = DateTime.Now.Add(lockoutDuration);
                    return false;
                }
            }

            return true;
        }
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string attemptedPassword, string hashedPassword, string salt)
        {
            string hashedAttemptedPassword = HashPassword(attemptedPassword, salt);
            return hashedAttemptedPassword == hashedPassword;
        }
        public static bool ShowCaptcha()
        {
            Random rand = new Random();
            int num1 = rand.Next(1, 10);
            int num2 = rand.Next(1, 10);

            Console.WriteLine($"CAPTCHA: What is {num1} + {num2}?");
            string response = Console.ReadLine();

            int expectedAnswer = num1 + num2;
            if (int.TryParse(response, out int userAnswer) && userAnswer == expectedAnswer)
            {
                return true;
            }

            return false;
        }
    }
}
