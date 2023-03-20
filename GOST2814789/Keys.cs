namespace GOST2814789
{
    public class Keys
    {
        private uint[] keys = new uint[8];

        public Keys(uint[] keys)
        {
            this.keys = keys;
        }

        public Keys(byte[] key)
        {
            Generate(key);
        }

        public Keys Inverse()
        {
            var newkeys = new uint[8];
            for (int i = 0; i < 8; i++)
            {
                newkeys[i] = keys[7 - i];
            }
            return new Keys(newkeys);
        }

        public void Generate(byte[] key)
        {
            if (key.Length != 32)
            {
                throw new Exception("Wrong key.");
            }

            for (int i = 0; i < 8; i++)
            {
                keys[i] = BitConverter.ToUInt32(key, 4 * i);
            }
        }

        public uint this[int v]
        {
            get => keys[v];
            set { keys[v] = value; }
        }

        public override string ToString()
        {
            string row = "";
            for (int i = 0; i < keys.Length; i++)
            {
                row += Convert.ToString(keys[i], 2) + "\n";
            }
            return row;
        }
    }
}
