using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        //Способ того какой метод использовать для обработки пакета 
        //в зависимости от полученного id пакета обработчиком пакета
        public delegate void PacketHandler(int _fromClient, Packet _packet);

        //Словарь для отслеживания наших обработчиков пакетов 
        public static Dictionary<int, PacketHandler> packetHandlers;


        public static TcpListener tcpListener;


        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Запускаем сервер...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Сервер запущен на порту {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Исходящее соединение от {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients.ContainsKey(i) && clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} безуспешное соединение: Сервер полон!");
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
{
    { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
    { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
    { (int)ClientPackets.letterPressed, ServerHandle.LetterPressed },
    { (int)ClientPackets.drumSpinRequest, ServerHandle.DrumSpinRequest },
    { (int)ClientPackets.drumSpinResult, ServerHandle.DrumSpinResult }
};

            // ОТДЕЛЬНО добавьте ServerPackets ТОЛЬКО в клиенте
            if (!packetHandlers.ContainsKey((int)ServerPackets.playerPosition))
            {
                packetHandlers.Add((int)ServerPackets.playerPosition, ServerHandle.PlayerPosition);
            }

            if (!packetHandlers.ContainsKey((int)ServerPackets.playerRotation))
            {
                packetHandlers.Add((int)ServerPackets.playerRotation, ServerHandle.PlayerRotation);
            }
    
            Console.WriteLine("Initialized packets...");
        }


        public static void HandleDrumSpinRequest(int fromClient, Packet packet)
        {
            int playerId = packet.ReadInt();
            int sectorNumber = packet.ReadInt();
            int points = packet.ReadInt();

            Console.WriteLine($"[Server] Игрок {playerId} запустил барабан, сектор: {sectorNumber}, очки: {points}");

            if (Server.clients.TryGetValue(playerId, out Client client))
            {
                if (client.player == null)
                {
                    Console.WriteLine($"[Server] Ошибка: client.player == null у игрока {playerId}!");
                }
                else
                {
                    Console.WriteLine($"[Server] Найден игрок {playerId}, обновляем результат барабана.");
                    client.player.SetDrumResult(sectorNumber);
                    client.player.AddScore(points);
                    ServerSend.RatingUpdate(playerId, client.player.score);
                }
            }
            else
            {
                Console.WriteLine($"[Server] Ошибка: Игрок {playerId} не найден в Server.clients! Все ID игроков: {string.Join(", ", Server.clients.Keys)}");
            }
        }
    }
}

