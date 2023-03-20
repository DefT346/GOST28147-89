using System.Text;


namespace GOST2814789
{
    internal class GOST
    {
        public static bool debug = false;

        enum Operation { Encode, Decode }

        public static byte[] Encode(string message, string key)
        {
            Console.WriteLine($"\n - Шифрование сообщения -\n");
            Console.WriteLine($" Кодирование текстового сообщения в байты...\n");
            return Encode(
                Encoding.UTF8.GetBytes(message),
                Encoding.UTF8.GetBytes(key));
        }

        public static string Decode(byte[] data, string key)
        {
            Console.WriteLine($"\n - Расшифровка сообщения -\n");
            return Encoding.UTF8.GetString(Decode(
                data,
                Encoding.UTF8.GetBytes(key)));
        }

        public static byte[] Encode(byte[] data, byte[] key)
        {
            return Operate(data, key, Operation.Encode);
        }

        public static byte[] Decode(byte[] data, byte[] key)
        {
            return Operate(data, key, Operation.Decode);
        }

        private static byte[] Operate(byte[] data, byte[] key, Operation op)
        {
            var keys = new Keys(key);

            Console.WriteLine("Ключевое запоминающее устройство ------");
            Console.WriteLine(keys);
            Console.WriteLine("-------------------------------- ------\n");

            var rsize = ((int)Math.Ceiling(data.Length / 8f));

            var result = new byte[rsize * 8];
            
            for (int i = 0; i < rsize; i++)
            {
                var block = new byte[8];
                var size = data.Length - 8 * i;
                Array.Copy(data, 8 * i, block, 0, size < 8 ? size : 8);

                var pairBlock = new Pair(block); Console.WriteLine($"Блок данных [{i}]\n{pairBlock}\n");

                var operBlock = Block(pairBlock, keys, op).ToByteArray();
                Array.Copy(operBlock, 0, result, 8 * i, 8);

                Console.WriteLine($"Блок {(op == Operation.Encode ? "зашифрованных" : "расшифрованных")} данных [{i}]\n{new Pair(operBlock)}\n");
            }
            return result;
        }

        private static uint Sub(uint N)
        {
            uint[,] H = {
                { 1, 13, 4, 6, 7, 5, 14, 4 },
                { 15, 11, 11, 12, 13, 8, 11 ,10},
                { 13, 4, 10, 7, 10 ,1, 4, 9 },
                { 0, 1, 0, 1, 1, 13, 12, 2 },
                { 5, 3, 7, 5, 0, 10, 6, 13 },
                { 7, 15, 2, 15, 8, 3, 13, 8 },
                { 10, 5, 1, 13, 9, 4, 15, 0 },
                { 4, 9, 13, 8, 15, 2, 10, 14 },
                { 9, 0, 3, 4, 14,14, 2, 6 },
                { 2, 10, 6, 10, 4, 15, 3, 11 },
                { 3, 14, 8, 9, 6, 12, 8, 1 },
                { 14, 7, 5, 14, 12, 7 ,1, 12 },
                { 6, 6, 9, 0, 11, 6, 0, 7 },
                { 11, 8, 12, 3, 2, 0, 7, 15 },
                { 8, 2, 15, 11, 5, 9, 5, 5 },
                { 12, 12, 14, 2 ,3, 11, 9, 3 }
            };

            uint result = 0;
            for (int i = 8; i > 0; i--)
            {
                uint frame = BitArrayExtensions.GetBitFrame(N, 4, (i - 1) * 4);
                uint num = H[frame, 8 - i];
                result = BitArrayExtensions.SetBitFrame(result, num, (i - 1) * 4);
            }

            return result;
        }

        private static Pair Block(Pair block, Keys keys, Operation op)
        {
            var res = block.Copy();
            for (int n = 0; n < 32; n++)
            {
                int keyIndex = n < (op == Operation.Encode ? 24 : 8) ? (n % 8) : (7 - n % 8);

                // Основной шаг криптопреобразования

                var s = (res.N1 + keys[keyIndex]) % uint.MaxValue;

                s = Sub(s);

                s = (s << 11) | (s >> 21);

                s ^= res.N2;

                if (n < 31)
                {  
                    res.N2 = res.N1;
                    res.N1 = s;
                }
                else
                    res.N2 = s;

                Console.WriteLine($"Раунд [{n}]\n");
                Console.WriteLine(res);
                Console.WriteLine();
                // В конце цикла старшая и младшая часть блока результата меняются местами
                // что необходимо для взаимной обратимости циклов 32-З и 32-Р

                // --------------------------------
            }

            return res;
        }

        static string BitStr(uint value)
        {
            return Convert.ToString(value, 2);
        }
    }
}
