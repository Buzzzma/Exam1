using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Server
    {
        public async Task Pamagiti()
        {
            await Task.Run(() => Load());
        }
        public void Load()
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            // Подготавливаем конечную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8888);
            Socket sock = new Socket(ipAddr.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с конечной точкой
                sock.Bind(ipEndPoint);
                // начинаем прослушку сокета
                sock.Listen(10);
                // Начинаем слушать соединения
                while (true)
                {
                    // Программа приостанавливается,
                    // ожидая входящее соединение
                    // сокет для обмена данными с клиентом
                    Socket s = sock.Accept();
                    // сюда будем записывать полученные от клиента данные
                    string data = null;
                    // Клиент есть, начинаем читать от него запрос
                    // массив полученных данных
                    byte[] bytes = new byte[1024];
                    // длина полученных данных
                    int bytesCount = s.Receive(bytes);
                    // Декодируем строку
                    data += Encoding.UTF8.GetString(bytes, 0, bytesCount);
                    // Показываем данные на консоли
                    StreamWriter writer = new StreamWriter("data.txt", true);
                    writer.WriteLine("Данные от клиента: " + data + "\n Текущее время: " + DateTime.Now.Hour + " " + DateTime.Now.Minute);
                    // Отправляем ответ клиенту
                    string reply = "Query size: " + data.Length.ToString()
                    + " chars";
                    // кодируем ответ сервера
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    // отправляем ответ сервера
                    s.Send(msg);
                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        break;
                    }
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

    }
}
