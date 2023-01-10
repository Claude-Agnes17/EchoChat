using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 4)
            {
                Console.WriteLine($"사용법 : {Process.GetCurrentProcess().ProcessName} <Bind IP> <Bind Port> <Server IP> <Message>");
                return;
            }

            string bindIp = args[0];
            int bindPort = Convert.ToInt32(args[1]);
            string serverIp = args[2];
            const int serverIpPort = 5425;
            string message = args[3];

            try
            {
                IPEndPoint clientAddres = new(IPAddress.Parse(bindIp), bindPort);
                IPEndPoint serverAddres = new(IPAddress.Parse(serverIp), serverIpPort);

                Console.WriteLine($"클라이언트 : {clientAddres}, 서버 : {serverAddres}");

                TcpClient client = new(clientAddres);

                client.Connect(serverAddres);
                byte[] data = Encoding.Default.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                Console.WriteLine($"송신 : {message}");

                data = new byte[256];
                string responseData = "";

                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.Default.GetString(data, 0, bytes);
                Console.WriteLine($"수신 : {responseData}");

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("클라이언트를 종료합니다");
        }
    }
}