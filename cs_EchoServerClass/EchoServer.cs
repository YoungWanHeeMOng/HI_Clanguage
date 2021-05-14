using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace cs_EchoServerClass
{
    public class EchoServer
    {
        public event AcceptedEventHandler AcceptedEventHandler = null;
        public event ClosedEventHandler ClosedEventHandler = null;
        public event ReceivedMsgEventHandler ReceivedMsgEventHandler = null;
        public string IPStr
        {
            get;
            private set;
        }
        public int Port
        {
            get;
            private set;
        }
        public EchoServer(string ipstr, int port)
        {
            IPStr = ipstr;
            Port = port;
        }
        Socket sock = null;
        public bool Start()
        {
            try
            {
                sock = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);

                IPAddress addr = IPAddress.Parse(IPStr);
                IPEndPoint iep = new IPEndPoint(addr, Port);
                sock.Bind(iep);

                sock.Listen(5);
                AcceptLoopAsyn();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void Close()
        {
            if (sock             != null)
            {
                try
                {
                    sock.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
              
        }

        delegate void AcceptDele();
        private void AcceptLoopAsyn()
        {
            AcceptDele dele = AcceptLoop;
            dele.BeginInvoke(null, null);
        }

        private void AcceptLoop()
        {
            Socket dosock = null;
            while(true)
            {
                dosock = sock.Accept();
                DoItAsync(dosock);
            }
        }

        delegate void DoItDele(Socket dosock);

        private void DoItAsync(Socket dosock)
        {
            IPEndPoint remote_ep = dosock.RemoteEndPoint as IPEndPoint;
            if(AcceptedEventHandler != null)
            {
                AcceptedEventHandler(this, new AcceptedEventArgs(remote_ep));
            }
            try
            {
                byte[] packet = new byte[1024];
                while(true)
                {
                    dosock.Receive(packet);
                    MemoryStream ms = new MemoryStream(packet);
                    BinaryReader br = new BinaryReader(ms);
                    string msg = br.ReadString();
                    br.Close();
                    ms.Close();
                    if(ReceivedMsgEventHandler != null)
                    {
                        ReceivedMsgEventHandler(this, new ReceivedMsgEventArgs(remote_ep,msg));

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
                if (ClosedEventHandler != null)
                {
                    ClosedEventHandler(this, new ClosedEventArgs(remote_ep));
                }
            }
        }
    }
}
