﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Pamagiti();
            ClientApp.Client client = new ClientApp.Client();
            client.Open();
        }
    }
}
