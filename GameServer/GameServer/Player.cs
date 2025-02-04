using System.Numerics;

namespace GameServer
{
    internal class Player
    {
        public int id;
        public string username;
        public Vector2 position;
        public Quaternion rotation;

        // Дополнительные поля
        public int score; // Текущие очки игрока
        public int drumResult; // Результат спина барабана
        public List<char> openedLetters; // Открытые буквы
        public int correctLettersCount; // Количество правильно угаданных букв

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            rotation = Quaternion.Identity;

            // Инициализация дополнительных полей
            score = 0;
            drumResult = 0;
            openedLetters = new List<char>();
            correctLettersCount = 0;
        }

        // Метод для обновления очков
        public void AddScore(int points)
        {
            score += points;
        }

        // Метод для добавления открытой буквы
        public void AddOpenedLetter(char letter)
        {
            openedLetters.Add(letter);
        }

        // Метод для обновления результата барабана
        public void SetDrumResult(int result)
        {
            drumResult = result;
        }

        // Метод для увеличения счетчика правильных букв
        public void IncrementCorrectLetters()
        {
            correctLettersCount++;
        }
    }
}