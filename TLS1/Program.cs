using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;


public class Server
{
    public static void Main()
    {
        int i = 0;
        string certFile = "C:/Program Files/OpenSSL-Win64/bin/server.pfx"; // Caminho para o arquivo PFX
        string certPassword = "password"; // Senha do arquivo PFX

        X509Certificate2 serverCertificate = new X509Certificate2(certFile, certPassword);

        TcpListener listener = new TcpListener(IPAddress.Any, 443); // Porta do servidor
        listener.Start();

        Console.WriteLine("Servidor TLS iniciado. Aguardando conexões...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            SslStream sslStream = new SslStream(client.GetStream(), false);
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate);

                // Ler dados do cliente
                byte[] buffer = new byte[2048];
                int bytes = sslStream.Read(buffer, 0, buffer.Length);
                Console.WriteLine("Recebido " + i + ": " + Encoding.UTF8.GetString(buffer, 0, bytes));
                i++;
                // Enviar dados para o cliente
                byte[] message = Encoding.UTF8.GetBytes("Hello from server.");
                sslStream.Write(message);
                sslStream.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //sslStream.Close();
                //client.Close();
            }
        }
    }
}
