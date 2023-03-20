
using GOST2814789;

string key = "ABDJRKDPELSUTRIELDNRIEJFRLDRPSRP";

while (true)
{
    var enc = GOST.Encode(Console.ReadLine(), key);
    var mes = GOST.Decode(enc, key);

    Console.WriteLine(mes);
}










