using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cs_EchoServerClass
{
    public delegate void AcceptedEventHandler(object sender, AcceptedEventArgs e);


    public class AcceptedEventArgs:EventArgs
    {
        public IPEndPoint RemoteEP
        {
            get;
            private set;
        }
        public string IPStr
        {
            get
            {
                return RemoteEP.Address.ToString();
            }
        }
        
        public int Port
        {
            get
            {
                return RemoteEP.Port;
            }
        }
        public AcceptedEventArgs(IPEndPoint remote_ep)
        {
            RemoteEP = remote_ep;
        }

    }
}
