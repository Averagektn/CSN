using System.Net.Sockets;
using System.Text;

namespace lab4
{
    internal static class Client
    {
        public static string HttpGet(string url)
        {
            try
            {
                var uri = new Uri(url);
                var client = new TcpClient(uri.Host, uri.Port);

                string request = "GET " + uri.PathAndQuery + " HTTP/1.0\r\n";
                request += "Host: " + uri.Host + "\r\n";
                request += "User-Agent: Mozilla/4.05 (WinNT; 1)\r\n";
                request += "Accept: image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*\r\n";
                request += "Connection: Keep-Alive\r\n";
                request += "\r\n";

                byte[] requestBytes = Encoding.UTF8.GetBytes(request);

                Stream stream = client.GetStream();
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string HttpPost(string url, string data)
        {
            try
            {
                var uri = new Uri(url);

                var client = new TcpClient(uri.Host, uri.Port);

                string request = "POST " + uri.PathAndQuery + " HTTP/1.0\r\n";
                request += "Host: " + uri.Host + "\r\n";
                request += "Content-Type: application/x-www-form-urlencoded\r\n";
                request += "Content-Length: " + data.Length + "\r\n";
                request += "Connection: Keep-Alive\r\n";
                request += "\r\n";
                request += data;

                byte[] requestBytes = Encoding.UTF8.GetBytes(request);

                Stream stream = client.GetStream();

                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string HttpHead(string url)
        {
            try
            {
                var uri = new Uri(url);

                var client = new TcpClient(uri.Host, uri.Port);

                string request = "HEAD " + uri.PathAndQuery + " HTTP/1.0\r\n";
                request += "Host: " + uri.Host + "\r\n";
                request += "Connection: Keep-Alive\r\n";
                request += "\r\n";

                byte[] requestBytes = Encoding.UTF8.GetBytes(request);

                Stream stream = client.GetStream();

                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] responseBytes = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
                string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);

                client.Close();

                return response;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
