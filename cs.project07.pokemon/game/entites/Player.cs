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
        private ConsoleColor BackgroundColor;
        private ConsoleColor ForegroundColor;

        public Player(Vector2 playerSpawnPoint)
        {
            Init(playerSpawnPoint);
        }

        public void Init(Vector2 playerSpawnPosition)
        {
            playerPosition = playerSpawnPosition;
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Black;
        }

        public void mouvPlayer(char dir)
        {
            switch (dir)
            {
                case 'N':
                    playerPosition = new Vector2(playerPosition.X - 1, playerPosition.Y);
                    //drawPlayer();
                    break;
                case 'S':
                    playerPosition = new Vector2(playerPosition.X + 1, playerPosition.Y);
                    //drawPlayer();
                    break;
                case 'O':
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y - 1);
                    //drawPlayer();
                    break;
                case 'E':
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y + 1);
                    //drawPlayer();
                    break;
            }
        }

        public void drawPlayer()
        {
            Console.SetCursorPosition((int)playerPosition.Y, (int)playerPosition.X);
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            Console.Write("P");
        }
    }
}
