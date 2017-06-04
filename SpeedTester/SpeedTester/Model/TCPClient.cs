﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTester
{
    public delegate void OnBrokenConnection();
    class TCPClient
    {
        public OnBrokenConnection OnBrokenConnection;
        bool isRunning = false;
        IPAddress ipAddress;
        int port;
        Socket clientSocket;
        int bufferSize;

        public TCPClient(IPAddress ipAddress, int port, int bufferSize)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.bufferSize = bufferSize;
            clientSocket = InitConnectionSocket();
        }

        public Socket InitConnectionSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipAddress, port);
            return s;
        }

        public void Run()
        {
            try
            {
                WorkWithServer(bufferSize);
            }
            catch (SocketException e)
            {
                OnBrokenConnection();
            }
            clientSocket.Close();
        }

        public void WorkWithServer(int bufferSize)
        {
            isRunning = true;
            String ts = "SIZE:"+bufferSize;
            byte[] toServer = Encoding.UTF8.GetBytes(ts);
            clientSocket.Send(toServer);
            byte[] bufferContent = Encoding.UTF8.GetBytes(GenerateContent(bufferSize));
            do
            {
                clientSocket.Send(bufferContent);
            } while (isRunning);
        }
        private static string GenerateContent(int bufferSize)
        {
            string content="";
            for(int i = 0; i < bufferSize; i++)
            {
                content += "X";
            }
            return content;
        }

        public void RequestStop()
        {
            isRunning = false;
        }
    }
}
