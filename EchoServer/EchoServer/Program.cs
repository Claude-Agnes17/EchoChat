using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine($"사용법 : {Process.GetCurrentProcess().ProcessName} <Bind IP>");
                return;
            }

            string bindip = args[0];
            const int bindport = 5425;
            TcpListener server = null;
            try
            {
                IPEndPoint localAddress = new(IPAddress.Parse(bindip), bindport);

                server = new(localAddress);
                server.Start();
                Console.WriteLine("메아리 서버시작...");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"클라이언트 접속 : {(IPEndPoint)client.Client.RemoteEndPoint}");
                    NetworkStream stream = client.GetStream();

                    int length;
                    string data = null;
                    byte[] bytes = new byte[256];

                    while((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.Default.GetString(bytes, 0, length);
                        Console.WriteLine(string.Format($"수신 : {data}"));
                        byte[] msg = Encoding.Default.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine(string.Format($"송신 : {data}"));
                    }

                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("서버를 종료합니다.");
        }
    }
}