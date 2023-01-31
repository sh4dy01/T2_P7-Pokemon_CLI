using cs.project07.pokemon.game.states.list;
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
        private PokemonListManager _pokemonListManager;

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

        public void zoomPlayer(int zoom)
        {
            if(zoom == 4) 
            {
                playerPosition = new Vector2(playerPosition.X * zoom, playerPosition.Y * zoom);
            }
            else if(zoom == 1) 
            {
                playerPosition = new Vector2(playerPosition.X / 4, playerPosition.Y / 4);
            }
        }

        public void mouvPlayer(char dir, int zoom)
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
        public bool collisionWall(char[,] grid, char dir)
        {
            switch (dir)
            {
                case 'N':
                    if (grid[(int)playerPosition.X - 1, (int)playerPosition.Y] == '#')
                    {
                        return false;
                    }
                    break;
                case 'S':
                    if (grid[(int)playerPosition.X + 1, (int)playerPosition.Y] == '#')
                    {
                        return false;
                    }
                    break;
                case 'O':
                    if (grid[(int)playerPosition.X, (int)playerPosition.Y - 1] == '#')
                    {
                        return false;
                    }
                    break;
                case 'E':
                    if (grid[(int)playerPosition.X, (int)playerPosition.Y + 1] == '#')
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        public void collisionGrass(char[,] grid, Game game)
        {
            if (grid[(int)playerPosition.X, (int)playerPosition.Y] == '*')
            {
                const int maxPercentage = 15;
                int percentage = new Random().Next(1, 100);
                if (percentage > 0 && percentage <= maxPercentage)
                {
                    Game.StatesList?.Push(new CombatState(game));
                }
            }
        }
        public void drawPlayer()
        {
            Console.SetCursorPosition((int)playerPosition.Y, (int)playerPosition.X);
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            Console.Write("P");
            Console.SetCursorPosition(0,0);
        }
    }
}
