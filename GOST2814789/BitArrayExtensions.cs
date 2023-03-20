using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public static class BitArrayExtensions
{
    public static int ParseBits(string bitsString)
    {
        return Convert.ToInt32(bitsString, 2);
    }

    public static int MessageToBin(string messgae)
    {
        return Convert.ToInt32(messgae);
    }

    public static void SetBit(this ref int intValue, int bitPosition, int bit)
    {
        if (bit == 1)
            intValue |= (1 << bitPosition);
        else
            intValue &= ~(1 << bitPosition);
    }

    public static void SetBit(this ref BigInteger intValue, int bitPosition, int bit)
    {
        if (bit == 1)
            intValue |= (1 << bitPosition);
        else
            intValue &= ~(1 << bitPosition);
    }

    public static int GetBit(this int b, int bitNumber)
    {
        return (b >> bitNumber) & 1;
    }

    public static uint GetBitFrame(uint value, int length, int shift)
    {
        uint mask = (1U << length) - 1;
        uint frame = value >> shift & mask;
        return (uint)frame;
    }

    public static uint SetBitFrame(uint value, uint frame, int position)
    {
        return (uint)(value | (frame << position));
    }

    public static void Print(this int source, int seqSize)
    {
        Console.WriteLine(Convert.ToString(source, 2).PadLeft(seqSize, '0'));
    }

    public static BitArray Create(string bits)
    {
        var ba = new BitArray(bits.Length);
        for (int i = 0; i < bits.Length; i++)
            ba[i] = bits[i] == '1' ? true : false;
        return ba;
    }

    public static void Print(this BitArray bitArray)
    {
        foreach (bool bit in bitArray)
            Console.Write("{0}", bit == true ? 1 : 0);
        Console.WriteLine();
    }
}

