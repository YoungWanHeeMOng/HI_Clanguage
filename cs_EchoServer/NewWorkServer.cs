using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace cs_EchoServer
{
    class NewWorkServer
    {
        static void Main(string[] args)
        {
            Socket sock = null;
            try
            {
                sock = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp); // 소켓 생성

                // 인터페이스와 결합
                IPAddress addr = IPAddress.Parse("192.168.25.6");
                IPEndPoint iep = new IPEndPoint(addr, 10040);
                sock.Bind(iep);

                // 백로그 큐 크기 설정
                sock.Listen(5);
                Socket dosock;

                while(true)
                {
                    dosock = sock.Accept();
                    DoItAsync(dosock);  // DoIt

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sock.Close();
            }
        }

        delegate void DoItdele(Socket dosock);
       
        private static void DoItAsync(Socket dosock)
        {
            DoItdele dele = DoIt;
            dele.BeginInvoke(dosock, null, null);
        }

        private static void DoIt(Socket dosock)
        {
            try
            {
                byte[] packet = new byte[1024];
                IPEndPoint iep = dosock.RemoteEndPoint as IPEndPoint;
                while (true)
                {
                    dosock.Receive(packet);
                    MemoryStream ms = new MemoryStream(packet);
                    BinaryReader br = new BinaryReader(ms);
                    string msg = br.ReadString();
                    br.Close();
                    ms.Close();
                    Console.WriteLine("{0}:{1} >> {2}", iep.Address, iep.Port, msg);
                    if (msg == "exit")
                    {
                        break;
                    }
                    dosock.Send(packet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                dosock.Close();
            }
        }
    }
}
