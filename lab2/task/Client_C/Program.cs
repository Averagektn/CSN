using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpClient
{
    class Program
    {
        const string FOLDER_CONTENTS = "contents_client";
        const string ipAddress = "127.0.0.1";
        const int port = 5555;
        static void Main()
        {
            IPEndPoint ipPoint = new(IPAddress.Parse(ipAddress), port);

            while (true)
            {
                try
                {
                    Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    socket.Connect(ipPoint);

                    Communicate(socket);

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static string GetString(Socket socket)
        {
            long size, received = 0;
            byte[] sizeBytes = new byte[sizeof(long)];
            socket.Receive(sizeBytes);
            size = BitConverter.ToInt64(sizeBytes, 0);

            byte[] buffer = new byte[1024];
            StringBuilder sb = new();

            while (received < size)
            {
                int bytesReceived = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));
                received += bytesReceived;
            }

            return sb.ToString();
        }

        static void Communicate(Socket socket)
        {
            Console.WriteLine("Choose mode. 1 - send, 2 - load");
            int mode = Convert.ToInt32(Console.ReadLine());
            byte[] bytes = BitConverter.GetBytes(mode);

            socket.Send(bytes);

            switch (mode)
            {
                // Sender
                case 1:
                    {
                        Post(socket);
                        break;
                    }
                // Receiver
                case 2:
                    {
                        // receive long size then generate buffer and receive string
                        Console.WriteLine(GetString(socket));
                        Get(socket);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Incorrect mode sent");
                        break;
                    }
            }
        }

        static void Send(string sourceFilePath, Socket socket)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            var fileNameBytes = Encoding.UTF8.GetBytes(fileName + '\n');
            var buffer = new byte[1024];
            int bytesRead;

            socket.Send(fileNameBytes);

            using var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                socket.Send(buffer, 0, bytesRead, SocketFlags.None);
            }
        }

        //SOLVED
        static void Post(Socket socket)
        {
            Console.WriteLine("Enter the following file names: ");

            string directoryPath = Path.Combine(Environment.CurrentDirectory, FOLDER_CONTENTS);
            string[] fileNames = Directory.GetFiles(directoryPath);
            foreach (string fileName in fileNames)
            {
                Console.WriteLine(Path.GetFileName(fileName));
            }
            string ref_path = string.Empty;
            string? path = Console.ReadLine();
            if (path != null)
            {
                ref_path = Path.Combine(FOLDER_CONTENTS, path);
            }

            byte[] bytes = new byte[1024];
            int res;

            if (path != null && File.Exists(ref_path))
            {
                Send(ref_path, socket);

                socket.Shutdown(SocketShutdown.Send);

                socket.Receive(bytes);
                res = BitConverter.ToInt32(bytes, 0);
                if (res < 0)
                {
                    Console.WriteLine("Error. File not sent");
                }
                else
                {
                    Console.WriteLine("File was sent successfully");
                }
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        // SOLVED
        static void Receive(string fileName, Socket socket)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, FOLDER_CONTENTS, fileName);
            var fileNameBytes = Encoding.UTF8.GetBytes(fileName);
            int bytesReceived;
            var buffer = new byte[1024];

            socket.Send(fileNameBytes);

            bytesReceived = socket.Receive(buffer, buffer.Length, SocketFlags.None);
            if (bytesReceived > 0)
            {
                using var destinationStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                {
                    destinationStream.Write(buffer, 0, bytesReceived);
                    while ((bytesReceived = socket.Receive(buffer, buffer.Length, SocketFlags.None)) > 0)
                    {
                        destinationStream.Write(buffer, 0, bytesReceived);
                    }
                    Console.WriteLine("File was created");
                }
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }

        static void Get(Socket socket)
        {
            Console.WriteLine("Enter file name");
            string? fileName = Console.ReadLine();

            if (fileName != null)
            {
                Receive(fileName, socket);
            }
        }
    }
}