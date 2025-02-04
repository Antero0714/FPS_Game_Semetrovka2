using System;
using System.Numerics;
using System.Text;

namespace GameServer
{
    // Статический класс для генерации случайных чисел
    public static class RandomGenerator
    {
        private static readonly Random _rnd = new Random();
        public static int Next(int min, int max)
        {
            lock (_rnd)
            {
                return _rnd.Next(min, max);
            }
        }
    }

    // Делегат для обработки пакетов
    public delegate void PacketHandler(int _fromClient, Packet _packet);

    class ServerHandle
    {
        // Обработка пакета welcomeReceived (клиент посылает свой ID и имя)
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}. ID: {_clientIdCheck}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"[Warning] Player \"{_username}\" (ID: {_fromClient}) assumed wrong client ID ({_clientIdCheck})!");
            }
            // Создаем объект игрока на сервере
            Server.clients[_fromClient].player = new Player(_fromClient, _username, new Vector2(0, 0));
            // После создания игрока вызываем метод, который рассылает данные о новом игроке всем клиентам
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        // Обработка запроса на вращение барабана (drumSpinRequest)
        public static void DrumSpinRequest(int _fromClient, Packet _packet)
        {
            // Читаем ID игрока из пакета (либо можно использовать _fromClient)
            int playerId = _packet.ReadInt();
            Console.WriteLine($"[Server] DrumSpinRequest received from player {playerId}");

            // Генерируем результат вращения барабана.
            // Предположим, что барабан имеет 12 секторов (0-11)
            int sectorNumber = RandomGenerator.Next(0, 12);
            int points = sectorNumber * 100; // Очки зависят от сектора

            Console.WriteLine($"[Server] DrumSpinResult: player {playerId}, sector {sectorNumber}, points {points}");

            // Отправляем всем клиентам результат спина
            ServerSend.DrumSpinResult(playerId, sectorNumber, points);
        }

        // Обработка пакета drumSpinResult (если клиент посылает результат, хотя обычно сервер генерирует результат)
        public static void DrumSpinResult(int _fromClient, Packet _packet)
        {
            int playerId = _packet.ReadInt();
            int sectorNumber = _packet.ReadInt();
            int points = _packet.ReadInt();
            Console.WriteLine($"[Server] DrumSpinResult received from player {playerId}: sector {sectorNumber}, points {points}");

            if (Server.clients.TryGetValue(playerId, out Client client) && client.player != null)
            {
                client.player.SetDrumResult(sectorNumber);
                client.player.AddScore(points);
                ServerSend.RatingUpdate(playerId, client.player.score);
            }
            else
            {
                Console.WriteLine($"[Server] Error: Player {playerId} not found.");
            }
        }

        // Обработчик для LetterPressed (клиент отправляет выбранную букву)
        public static void LetterPressed(int _fromClient, Packet _packet)
        {
            int playerId = _fromClient;  // Или _packet.ReadInt() если ID передается явно
            // Если Packet не содержит метода ReadChar, можно прочитать int и привести:
            int ascii = _packet.ReadInt();
            char letter = (char)ascii;

            Console.WriteLine($"[Server] LetterPressed: player {playerId} pressed letter '{letter}'");

            // Проверка правильности (например, слово "СКИФЫ")
            bool isCorrect = "СКИФЫ".Contains(letter.ToString());
            int pointsAwarded = isCorrect ? 500 : 0;

            if (isCorrect && Server.clients.TryGetValue(playerId, out Client client) && client.player != null)
            {
                client.player.AddScore(pointsAwarded);
                ServerSend.RatingUpdate(playerId, client.player.score);
            }

            // Рассылаем всем клиентам результат нажатия буквы
            using (Packet packet = new Packet((int)ServerPackets.letterResult))
            {
                packet.Write(playerId);
                packet.Write(pointsAwarded);
                ServerSend.SendTCPDataAll(packet);
            }

            // Если условие победы выполнено (например, если игрок набрал определённое количество очков)
            if (Server.clients[playerId].player.score >= 2500)
            {
                ServerSend.WinAnnouncement(playerId, Server.clients[playerId].player.username);
            }
        }

        // Обработчик для playerPosition
        public static void PlayerPosition(int _fromClient, Packet _packet)
        {
            int playerId = _packet.ReadInt();
            // Читаем позицию как два float для Vector2
            Vector2 pos = new Vector2(_packet.ReadFloat(), _packet.ReadFloat());
            Console.WriteLine($"[Server] PlayerPosition: player {playerId} moved to {pos}");

            if (Server.clients.TryGetValue(playerId, out Client client) && client.player != null)
            {
                client.player.position = pos;
            }
        }

        // Обработчик для playerRotation (для 2D игры — читаем один float, угол)
        public static void PlayerRotation(int _fromClient, Packet _packet)
        {
            int playerId = _packet.ReadInt();
            float angle = _packet.ReadFloat();  // Угол Z
            Console.WriteLine($"[Server] PlayerRotation: player {playerId} rotated to {angle} degrees");
            // Если требуется, можно сохранить угол в объекте игрока:
            // client.player.rotation = Quaternion.Euler(0, 0, angle);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            // Предположим, что клиент отправляет позицию как два float значения (Vector2)
            int playerId = _fromClient;  // или можно явно читать ID, если он передается
            float posX = _packet.ReadFloat();
            float posY = _packet.ReadFloat();
            Vector2 position = new Vector2(posX, posY);

            Console.WriteLine($"[Server] PlayerMovement: player {playerId} moved to {position}");

            if (Server.clients.ContainsKey(playerId) && Server.clients[playerId].player != null)
            {
                Server.clients[playerId].player.position = position;
            }
        }

    }
}
