using System;
using System.Security.Cryptography;

namespace EHMAssistant
{
    public class SecureRandomGenerator : IDisposable
    {
        private readonly RNGCryptoServiceProvider _rng;
        private bool _disposed;

        public SecureRandomGenerator()
        {
            _rng = new RNGCryptoServiceProvider();
        }

        public int GetRandomValue(int minValue, int maxValue)
        {
            // Handle edge cases
            if (minValue == maxValue)
                return minValue; // Only one possible value

            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than or equal to minValue");

            // Calculate the range (maxValue is exclusive)
            uint range = (uint)(maxValue - minValue);

            // Create a mask for generating numbers that won't introduce bias
            uint mask = uint.MaxValue;
            if (range < uint.MaxValue)
            {
                // Create the smallest mask that can contain the range
                mask = range;
                mask |= mask >> 1;
                mask |= mask >> 2;
                mask |= mask >> 4;
                mask |= mask >> 8;
                mask |= mask >> 16;
            }

            byte[] randomBytes = new byte[4];
            uint randomUint;

            // Keep generating until we get a number within our range
            do
            {
                _rng.GetBytes(randomBytes);
                randomUint = BitConverter.ToUInt32(randomBytes, 0) & mask;
            } while (randomUint >= range); // Ensure the value is strictly less than range

            return minValue + (int)randomUint;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _rng.Dispose();
                _disposed = true;
            }
        }
    }
}