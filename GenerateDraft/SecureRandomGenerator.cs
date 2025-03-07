using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (minValue >= maxValue)
                return 1;

                // throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than minValue");

            int range = maxValue - minValue;
            if (range <= 0)
                throw new ArgumentOutOfRangeException(nameof(range), "The range must be greater than 0");

            // Create a mask for generating numbers that won't introduce bias
            uint mask = uint.MaxValue;
            if (range < int.MaxValue)
            {
                // Create the smallest mask that can contain the range
                mask = (uint)range;
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
            } while (randomUint > range);

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
