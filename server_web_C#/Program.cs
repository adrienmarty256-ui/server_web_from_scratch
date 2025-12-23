using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

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
        Console.WriteLine($"Server started on {localserver}:{port}");
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
            Console.WriteLine("Waiting for a connection...");
            TcpClient client = portListener.AcceptTcpClient();
            Console.WriteLine("Connected!");

            // Lecture de la requête HTTP
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            string requestLine = reader.ReadLine();
            Console.WriteLine($"Request: {requestLine}");

            // Extraire le chemin demandé
            string path = "/";
            if (!string.IsNullOrEmpty(requestLine))
            {
                string[] tokens = requestLine.Split(' ');
                if (tokens.Length >= 2)
                    path = tokens[1];
            }

            // Préparer la réponse selon le chemin
            string responseText = path switch
            {
                "/" => "Bienvenue sur la page d'accueil !",
                "/hello" => "Hello world depuis le serveur C#!",
                _ => "404 - Page non trouvée"
            };

            // Envoyer la réponse HTTP
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("HTTP/1.1 200 OK");
            writer.WriteLine("Content-Type: text/plain");
            writer.WriteLine();
            writer.WriteLine(responseText);
            writer.Flush();

            client.Close();
        }
    }
}

