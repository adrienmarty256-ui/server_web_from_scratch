using System.Net.Sockets;
using System.Net;
using System;

class PortListener
{
    int port = 5000;
    IPAddress localserver = IPAddress.Loopback;
    TcpListener serverWeb;

    public PortListener()
    {
        serverWeb = new TcpListener(localserver, port);
    }

    public void Start()
    {
        serverWeb.Start();
    }

    public TcpClient AcceptTcpClient()
    {
        return serverWeb.AcceptTcpClient();
    }

    public static void Main()
    {
        PortListener portListener = new PortListener();
        portListener.Start();

        while(true)
        {
            Console.Write("Waiting for a connection... ");
            TcpClient client = portListener.AcceptTcpClient();
            Console.WriteLine("Connected!");
        }
      
    }

}

