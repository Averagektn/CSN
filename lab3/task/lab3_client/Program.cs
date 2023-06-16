using System.Net.Sockets;
using System.Text;

// 52.88.68.92 1234
// india.colorado.edu 13
// freechess.org
// bbs.archaicbinary.net 

var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Thread? receiveThread;
string? path = Console.ReadLine();
string[] parts = Array.Empty<string>();

string host = "localhost";
int port = 23;
if (path != null)
{
    parts = path.Split(' ');
    host = parts[0];
}
if (parts.Length > 1)
{
    port = int.Parse(parts[1]);
}

try
{
    socket.Connect(host, port);

    receiveThread = new Thread(() =>
    {
        //socket.Send(new byte[] { 255, 251, 31, 255, 251, 32 });
        int width = 0, height = 0;
        try
        {
            while (true)
            {
                byte[] bytes = new byte[512];
                int i = socket.Receive(bytes, bytes.Length, SocketFlags.None);
                Mode.CheckRequest(bytes, out width, out height);
                string responseData = Encoding.UTF8.GetString(bytes, 0, i);
                Console.Write(responseData);
            }
        }
        catch
        {
            Console.WriteLine("Disconnected");
        }
    });
    receiveThread.Start();

    while (true)
    {
        if (Mode.lineMode == true)
        {
            string data = Console.ReadLine() + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            socket.Send(bytes, bytes.Length, SocketFlags.None);
        }
        else
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            byte[] bytes = Encoding.UTF8.GetBytes(key.KeyChar.ToString());
            socket.Send(bytes, bytes.Length, SocketFlags.None);
        }

    }
}
catch
{
    Console.WriteLine("Connection error");
}


public static class Mode
{
    public static bool lineMode = true;

    public static void CheckRequest(byte[] bytes, out int width, out int height)
    {
        int count = bytes.Length;
        width = 0; height = 0;
        for (int j = 0; j < count; j++)
        {
            if (bytes[j] == 255)
            {
                bytes[j] = 0;
                j++;
                if (bytes[j] == 250)
                {
                    bytes[j] = 0;
                    j++;
                    if (bytes[j] == 31)
                    {
                        bytes[j] = 0;
                        j++;
                        width = Convert.ToUInt16(bytes[j + 1]);
                        bytes[j] = 0;
                        bytes[j + 1] = 0;
                        j += 2;
                        height = Convert.ToUInt16(bytes[j + 1]);
                        bytes[j] = 0;
                        bytes[j + 1] = 0;
                        j += 2;
                        bytes[j] = 0;
                    }
                }
                else if (bytes[j] == 254)
                {
                    bytes[j] = 0;
                    j++;
                    bytes[j] = 0;
                }
                else if (bytes[j] == 253)
                {
                    bytes[j] = 0;
                    j++;

                    if (bytes[j] == 23)
                    {
                        bytes[j] = 0;
                        Mode.lineMode = false;
                    }
                    if (bytes[j] == 24)
                    {
                        bytes[j] = 0;
                    }
                }
                else if (bytes[j] == 251)
                {
                    bytes[j] = 0;
                    j++;
                    if (bytes[j] == 1 || bytes[j] == 3)
                    {
                        bytes[j] = 0;
                    }
                }
            }

        }
    }
}