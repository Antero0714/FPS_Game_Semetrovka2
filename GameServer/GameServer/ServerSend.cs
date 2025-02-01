using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);

            Console.WriteLine($"Отправлен пакет клиенту {_toClient}");
        }


        public static void SendPlayerData(int toClient, int playerId, int rating)
        {
            using (Packet packet = new Packet((int)ServerPackets.playerData))
            {
                packet.Write(playerId);
                packet.Write(rating);

                Console.WriteLine($"[Server] Отправка рейтинга {rating} для игрока ID {playerId}");

                SendTCPData(toClient, packet);
            }
        }



        private static void SendTCPDataAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);
                SendTCPData(_toClient, _packet);
            }

            // Отправляем новому клиенту информацию обо всех игроках, кроме его самого
            foreach (var client in Server.clients.Values)
            {
                if (client.player != null && client.player.id != _toClient)
                {
                    SpawnPlayer(_toClient, client.player);
                }
            }

            Console.WriteLine($"[Server] Отправлен ID клиенту: {_toClient}");
        }




        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);

                Console.WriteLine($"[Server] Отправка SpawnPlayer: игрок ID {_player.id}, Username: {_player.username} клиенту {_toClient}");
                SendTCPData(_toClient, _packet);
            }
        }


        public static void PlayerDisconnected(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
            {
                _packet.Write(_playerId);
                SendTCPDataAll(_packet);
            }
        }

    }
}
