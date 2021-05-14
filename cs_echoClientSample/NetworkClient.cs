
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace cs_echoClientSample
{
    class NetworkClient
    {
        static void Main(string[] args)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork,
                                                SocketType.Stream,
                                                ProtocolType.Tcp);

            IPAddress addr = IPAddress.Parse("192.168.25.6");
            IPEndPoint iep = new IPEndPoint(addr, 10040);
            sock.Connect(iep);
            string str;
            string str2;
            byte[] packet = new byte[1024];
            byte[] packet2 = new byte[1024];
            while (true)
            {
                Console.Write("전송할 메시지 : ");
                str = Console.ReadLine();
                MemoryStream ms = new MemoryStream(packet);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(str);
                bw.Close();
                ms.Close();
                sock.Send(packet);
                if(str=="exit")
                {
                    break;
                }
                sock.Receive(packet2);
                MemoryStream ms2 = new MemoryStream(packet2);
                BinaryReader br = new BinaryReader(ms2);
                str2 = br.ReadString();
                Console.WriteLine("수신한 메세지: {0}", str2);
                br.Close();
                ms2.Close();
            }
            sock.Close();
        }
    }
}
