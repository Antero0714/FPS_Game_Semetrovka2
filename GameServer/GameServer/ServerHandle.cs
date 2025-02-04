using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}. \n ID: {_clientIdCheck}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has asumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }
        public static void DrumSpinResult(int playerId, int sectorNumber, int points)
        {
            using (Packet packet = new Packet((int)ServerPackets.drumSpinResult))
            {
                packet.Write(playerId);
                packet.Write(sectorNumber);
                packet.Write(points);

                Console.WriteLine($"[Server] Отправка drumSpinResult: playerId={playerId}, sector={sectorNumber}, points={points}, пакет размером {packet.Length()} байт");

                ServerSend.SendTCPDataAll(packet);
            }
        }


        public static void LetterPressed(int _fromClient, Packet _packet)
        {
            int playerId = _fromClient;
            char letter = (char)_packet.ReadInt();

            Console.WriteLine($"[Server] Игрок {playerId} нажал букву {letter}");

            bool isCorrect = ("СКИФЫ".Contains(letter.ToString()));

            int pointsAwarded = isCorrect ? 500 : 0;

            if (isCorrect && Server.clients.ContainsKey(playerId) && Server.clients[playerId].player != null)
            {
                Server.clients[playerId].rating += pointsAwarded;
                ServerSend.RatingUpdate(playerId, Server.clients[playerId].rating);
            }

            using (Packet packet = new Packet((int)ServerPackets.letterResult))
            {
                packet.Write(playerId);
                packet.Write(pointsAwarded);
                ServerSend.SendTCPDataAll(packet);
            }

            // Проверка победы
            if (Server.clients[playerId].rating >= 2500) // Пример: 5 правильных букв по 500 очков
            {
                ServerSend.WinAnnouncement(playerId, Server.clients[playerId].player.username);
            }
        }


        public static void HandleDrumSpin(int fromClient, Packet packet)
        {
            int playerId = packet.ReadInt();
            int sectorNumber = packet.ReadInt();
            int points = packet.ReadInt();

            Console.WriteLine($"[Server] Игрок {playerId} запустил барабан, сектор: {sectorNumber}, очки: {points}");

            // Обновляем состояние игрока
            if (Server.clients.TryGetValue(playerId, out Client client))
            {
                client.player.SetDrumResult(sectorNumber);
                client.player.AddScore(points);

                // Отправляем обновленные данные всем игрокам
                ServerSend.RatingUpdate(playerId, client.player.score);
            }
        }
        public static void HandleDrumSpinResult(int fromClient, Packet packet)
        {
            int playerId = packet.ReadInt();
            int sectorNumber = packet.ReadInt();
            int points = packet.ReadInt();

            Console.WriteLine($"[Server] Игрок {playerId} выбил сектор {sectorNumber} и получил {points} очков!");

            if (Server.clients.TryGetValue(playerId, out Client client))
            {
                client.player.SetDrumResult(sectorNumber);
                client.player.AddScore(points);

                // Отправляем обновленные данные всем игрокам
                ServerSend.RatingUpdate(playerId, client.player.score);
            }
        }
        public static void HandleLetterPressed(int fromClient, Packet packet)
        {
            int playerId = packet.ReadInt();
            char letter = packet.ReadChar();

            if (Server.clients.TryGetValue(playerId, out Client client))
            {
                // Проверяем, правильная ли буква
                bool isCorrect = "СКИФЫ".Contains(letter.ToString());

                if (isCorrect)
                {
                    client.player.AddOpenedLetter(letter);
                    client.player.IncrementCorrectLetters();
                    client.player.AddScore(500); // Начисляем очки

                    // Отправляем обновленные данные всем игрокам
                    ServerSend.RatingUpdate(playerId, client.player.score);

                    // Проверяем, выиграл ли игрок
                    if (client.player.correctLettersCount >= 5)
                    {
                        ServerSend.WinAnnouncement(playerId, client.player.username);
                    }
                }
            }
        }


    }
}
