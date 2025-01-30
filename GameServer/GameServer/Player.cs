using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GameServer
{
    internal class Player
    {
        public int id;
        public string username;

        public Vector2 position;
        //МОЖНО БУДЕТ УДАЛИТЬ
        public Quaternion rotation;
        
        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            //МОЖНО БУДЕТ УДАЛИТЬ
            rotation = Quaternion.Identity;
        }
    }
}
