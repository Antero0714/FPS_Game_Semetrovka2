using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace GameServer
{
    class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public Player player;
        public TCP tcp;

        public int rating = 0; // Добавляем рейтинг


        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;
            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                ServerSend.Welcome(id, "Добро пожаловать на сервер");
                
            }

            public void SendData(Packet _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    }
                }
                catch (Exception _ex)
                {

                    Console.WriteLine($"Ошибка отправки данных игроку {id} via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        Server.clients[id].Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    receivedData.Reset(HandleData(_data));
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch(Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data {_ex}");
                    Server.clients[id].Disconnect();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLenght = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLenght = receivedData.ReadInt();
                    if (_packetLenght <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLenght > 0 && _packetLenght <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLenght);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            Server.packetHandlers[_packetId](id, _packet);
                        }
                    });

                    _packetLenght = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLenght = receivedData.ReadInt();
                        if (_packetLenght <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLenght <= 1)
                {
                    return true;
                }

                return false;
            }

            public void Disconnect()
            {
                socket.Close();

                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public void SendIntoGame(string _playerName)
        {
            // Создаем игрока (например, new Player(id, _playerName, Vector2.zero))
            player = new Player(id, _playerName, new Vector2(0, 0));

            // Опционально: загрузка рейтинга
            Server.clients[id].rating = LoadPlayerRatingFromDatabase(_playerName);
            ServerSend.SendPlayerData(id, id, Server.clients[id].rating);

            // Рассылаем данные о новом игроке всем клиентам
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    ServerSend.SpawnPlayer(_client.id, player);
                }
            }
        }



        private int LoadPlayerRatingFromDatabase(string username)
        {
            // Тут должен быть код для запроса к БД, пока заглушка
            return 100; // Например, у всех игроков начальный рейтинг 100
        }

        public void Disconnect()
        {
            Console.WriteLine($"Игрок {id} отключился.");

            ServerSend.PlayerDisconnected(id); // Отправляем клиентам инфу об отключении

            player = null;
            tcp.Disconnect();

            // Очищаем только сокет, но оставляем ID в clients
            Server.clients[id] = new Client(id); // Пересоздаём объект клиента, но с тем же ID
        }






    }
}
