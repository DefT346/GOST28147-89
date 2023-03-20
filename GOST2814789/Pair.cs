using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOST2814789
{
    public class Pair
    {
        public uint N1, N2;
        public Pair(uint L, uint R)
        {
            this.N1 = L;
            this.N2 = R;
        }

        public Pair Copy()
        {
            return new Pair(N1, N2);
        }

        public Pair(string L, string R)
        {
            this.N1 = Convert.ToUInt32(L, 2);
            this.N2 = Convert.ToUInt32(R, 2);
        }

        public Pair(byte[] bytes)
        {
            N1 = BitConverter.ToUInt32(bytes, 0);
            N2 = BitConverter.ToUInt32(bytes, 4);
        }

        public byte[] ToByteArray()
        {
            var resultBuffer = new byte[8];
            var N1buff = BitConverter.GetBytes(N1);
            var N2buff = BitConverter.GetBytes(N2);

            for (int i = 0; i < 4; i++)
            {
                resultBuffer[i] = N1buff[i];
                resultBuffer[4 + i] = N2buff[i];
            }

            return resultBuffer;
        }

        public override string ToString()
        {
            var row = "";

            row += "L = " + Convert.ToString(N1, 2) + " \n";
            row += "R = " + Convert.ToString(N2, 2);

            return row;
        }
    }
}
