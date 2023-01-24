using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    public class Player
    {
        public Vector2 playerPosition;

        public Player(Vector2 playerSpawnPoint)
        {
            Init(playerSpawnPoint);
        }

        public void Init(Vector2 playerSpawnPosition)
        {
            playerPosition = playerSpawnPosition;
        }

        public void mouvPlayer(char dir)
        {
            switch (dir)
            {
                case 'N':
                    playerPosition = new Vector2(playerPosition.X - 1, playerPosition.Y);
                    break;
                case 'S':
                    playerPosition = new Vector2(playerPosition.X + 1, playerPosition.Y);
                    break;
                case 'O':
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y - 1);
                    break;
                case 'E':
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y + 1);
                    break;
            }
        }
    }
}
