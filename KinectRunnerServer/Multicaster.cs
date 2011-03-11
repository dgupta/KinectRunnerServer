using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace KinectRunnerServer
{
	public class Multicaster
	{
		
		
		UdpClient publisher;
		public Multicaster ()
		{
			publisher = new UdpClient("localhost",8899);
			Console.WriteLine("Publisher Built on localhost");
		}
		
		public SendData(string toSend){
			publisher.Send(toSend,toSend.Length);
		}

	}
}

