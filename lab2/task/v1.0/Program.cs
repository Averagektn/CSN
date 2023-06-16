using System.Net;
using System.Net.Sockets;
using System.Text;

// add UDP

namespace TcpServer
{
    class Program
    {
        const string FOLDER_CONTENTS = "contents_server";
        const string ipAddress = "127.0.0.1";
        const int port = 5555;
        static void Main()
        {
            IPEndPoint ipPoint = new(IPAddress.Parse(ipAddress), port);
            try
            {
                Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(ipPoint);
                socket.Listen(10);
                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    Socket clientSocket = socket.Accept();

                    Thread clientThread = new(() => CommunicateWithClient(clientSocket));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void CommunicateWithClient(Socket clientSocket)
        {
            byte[] mode = new byte[4];
            int md;

            try
            {
                clientSocket.Receive(mode);
                md = BitConverter.ToInt32(mode);
                Console.WriteLine(md);

                switch (md)
                {
                    // Receiver
                    case 1:
                        {
                            Get(clientSocket);
                            break;
                        }
                    // Sender
                    case 2:
                        {
                            ShowFiles();
                            SendFileNames(clientSocket);
                            Post(clientSocket);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect mode received");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        static void Get(Socket socket)
        {
            int outputMessage;

            var fileNameBytes = new byte[1024];
            var buffer = new byte[1024];

            int bytesReceived;
            string msg, fileName, filePath;
            byte[] outputData;
            string content = string.Empty;

            bytesReceived = socket.Receive(fileNameBytes, fileNameBytes.Length, SocketFlags.None);
            msg = Encoding.UTF8.GetString(fileNameBytes, 0, bytesReceived);

            string[] parts = msg.Split('\n');
            fileName = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                content += parts[i];
            }

            filePath = Path.Combine(Directory.GetCurrentDirectory(), FOLDER_CONTENTS, fileName);

            using (var destinationStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                if (content != string.Empty)
                {
                    buffer = Encoding.UTF8.GetBytes(content, 0, content.Length);
                    destinationStream.Write(buffer, 0, buffer.Length);
                }

                while ((bytesReceived = socket.Receive(buffer, buffer.Length, SocketFlags.None)) > 0)
                {
                    destinationStream.Write(buffer, 0, bytesReceived);
                }
            }

            outputMessage = 0;

            outputData = BitConverter.GetBytes(outputMessage);
            socket.Send(outputData);
        }

        static void SendFileNames(Socket socket)
        {
            string directoryPath = Path.Combine(Environment.CurrentDirectory, FOLDER_CONTENTS);
            string[] fileNames = Directory.GetFiles(directoryPath);
            var sb = new StringBuilder();

            foreach (string fileName in fileNames)
            {
                sb.Append(Path.GetFileName(fileName));
                sb.Append('\n');
            }

            byte[] sizeBytes = BitConverter.GetBytes(sb.Length);
            socket.Send(sizeBytes);

            byte[] fileNameBytes = Encoding.UTF8.GetBytes(sb.ToString());
            socket.Send(fileNameBytes);
        }

        static string[] ShowFiles()
        {
            string directoryPath = Path.Combine(Environment.CurrentDirectory, FOLDER_CONTENTS);
            string[] fileNames = Directory.GetFiles(directoryPath);

            foreach (string fileName in fileNames)
            {
                Console.WriteLine(Path.GetFileName(fileName));
            }

            return fileNames;
        }

        static void Post(Socket socket)
        {
            var fileNameBytes = new byte[1024];

            int bytesReceived = socket.Receive(fileNameBytes, fileNameBytes.Length, SocketFlags.None);

            var fileName = Encoding.UTF8.GetString(fileNameBytes, 0, bytesReceived);
            var filePath = Path.Combine(Environment.CurrentDirectory, FOLDER_CONTENTS, fileName);

            var buffer = new byte[1024];
            int bytesRead;

            if (File.Exists(filePath))
            {
                using var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    socket.Send(buffer, bytesRead, SocketFlags.None);
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }
    }
}