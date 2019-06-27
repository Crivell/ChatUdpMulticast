using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatUdpMulticast
{
    class Program
    {

        
        static void Main(string[] args)
        {
            Threads thre = new Threads();
            Thread t1 = new Thread(new ThreadStart(thre.Sender));
            Thread t2 = new Thread(new ThreadStart(thre.Lisiner));
            t1.Start();
            t2.Start();
           
        }

      
            
    }
}
