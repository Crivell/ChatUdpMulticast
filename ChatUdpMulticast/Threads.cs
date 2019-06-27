using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatUdpMulticast
{
    class Threads
    {


        public string myNick { get; private set; } = "";

        
        public void Sender()
        {
            string nick;
            UdpClient udpclient = new UdpClient();
            udpclient.Client.ReceiveTimeout = 1000;
            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            udpclient.JoinMulticastGroup(multicastaddress);
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, 2222);

            byte[] buffer = Encoding.ASCII.GetBytes("");
            while (true)
            {
                Console.WriteLine("By zacząć podaj swoj nick:");
                nick = Console.ReadLine();
                MakeSend(udpclient, remoteep, "NICK " + nick);
                try
                {
                    Byte[] data = udpclient.Receive(ref remoteep);
                    string strData = Encoding.Unicode.GetString(data);

                    Console.WriteLine("Nick " + myNick + " zajety");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Udana rezerwacja nicku " + nick );
                    myNick = nick;
                    break;
                }



            }

            string st;
            while (true)
            {
                st = Console.ReadLine();
                MakeSend(udpclient, remoteep, "MSG " + this.myNick + " " + st);
                if (st.Equals("end"))
                {
                    break;
                }
            }

            Console.WriteLine("All Done! Press ENTER to quit.");
            Console.ReadLine();
        }

        private static void makeSend()
        {
            throw new NotImplementedException();
        }

        public void Lisiner()
        {

            UdpClient client = new UdpClient();

            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            client.JoinMulticastGroup(multicastaddress);

            while (true)
            {
                Byte[] data = client.Receive(ref localEp);
                string strData = Encoding.Unicode.GetString(data);


                string[] nick = strData.Split(' ');

                if (nick[0].Equals("NICK"))
                {
                    
                    if (myNick.Equals(nick[1]))
                    {
                        MakeSend(client, localEp, "NICK " + nick[1] + " BUSY");
                    }
                }
                
                if (!myNick.Equals(nick[1]) && !myNick.Equals("") &&!nick[0].Equals("NICK"))
                {
                    Console.WriteLine(strData + " " + myNick);
                }

            }
        }

        static void MakeSend(UdpClient udpClient, IPEndPoint remoteep, String stringToSend)
        {
            byte[] buffer;
            buffer = Encoding.Unicode.GetBytes(stringToSend);
            udpClient.Send(buffer, buffer.Length, remoteep);
        }
    }
}
