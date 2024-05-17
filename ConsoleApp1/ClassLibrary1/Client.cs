using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Client
    {
        public async Task TimeToShine()
        {
            await Task.Run(() => Open());
        }
        public void Open()
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            try
            {
                Communicate("localhost", 8888);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
        static void Communicate(string hostname, int port)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];
            // Соединяемся с удаленным сервером
            // Устанавливаем удаленную точку(сервер) для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(hostname);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            Socket sock = new Socket(ipAddr.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
            // Подключаемся к серверу
            sock.Connect(ipEndPoint);
            Console.Write("Введите сообщение: ");
            string message = Console.ReadLine();
            Console.WriteLine("Подключаемся к порту {0} ",
            sock.RemoteEndPoint.ToString());
            byte[] data = Encoding.UTF8.GetBytes(message);
            // Получаем к - во отправленных байтов
            int bytesSent = sock.Send(data);
            // Получаем ответ от сервера, bytesRec - к - во принятых байтов
            int bytesRec = sock.Receive(bytes);
            Console.WriteLine("\n Ответ сервера: ( 0 ) \n \n ", Encoding.UTF8.GetString(bytes, 0, bytesRec));
            //Вызываем Communicate() еще
            if (message.IndexOf("<The End>") == -1)
                Communicate(hostname, port);
            // Освобождаем сокет
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}
