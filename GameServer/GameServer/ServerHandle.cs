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

                ServerSend.SendTCPDataAll(packet); // Исправленный вызов метода
            }
        }




        public static void LetterPressed(int _fromClient, Packet _packet)
        {
            int playerId = _fromClient; // или _packet.ReadInt(), если отправляется явно
            char letter = (char)_packet.ReadInt(); // можно передавать как int (код символа) или как string и брать первый символ

            Console.WriteLine($"[Server] Игрок {playerId} нажал букву {letter}");

            // Допустим, у нас правильный ответ – буква 'С' (пример, можно задать набор)
            bool isCorrect = ("СКИФЫ".Contains(letter.ToString()));

            int pointsAwarded = isCorrect ? 500 : 0;

            // Обновляем рейтинг для игрока, если буква правильная
            if (isCorrect && Server.clients.ContainsKey(playerId) && Server.clients[playerId].player != null)
            {
                Server.clients[playerId].rating += pointsAwarded;
                // Отправляем обновление рейтинга всем клиентам
                ServerSend.RatingUpdate(playerId, Server.clients[playerId].rating);
            }

            // Рассылаем результат нажатия буквы (letterResult) всем клиентам
            using (Packet packet = new Packet((int)ServerPackets.letterResult))
            {
                packet.Write(playerId);
                packet.Write(pointsAwarded);
                ServerSend.SendTCPDataAll(packet);
            }

            // Если общее число правильных (например, глобальный счёт) достигло 5, объявляем победителя.
            // Здесь можно реализовать глобальный счет или проверять рейтинг.
            // Например, если рейтинг игрока превысил порог, считаем его победителем:
            // if (Server.clients[playerId].rating >= threshold) { ... }
        }





    }
}
