using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.states;
using cs.project07.pokemon.game.states.list;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.Registry;

namespace cs.project07.pokemon.game.entites
{
    public class Player : ISavable
    {
        const int incrementMouvement = 4;
        const int sizePlayer = 4;

        string[,] spriteNormal =  { { " ", "(", ")", " " },
                                    { "/", "|", "|", "\\" }, 
                                    { " ", "|", "|", " " }, 
                                    { "/", " ", " ", "\\" }  };

        private string [,] spritePlayer;
        public Vector2 playerPosition;
        private ConsoleColor BackgroundColor;
        private ConsoleColor ForegroundColor;
        private int _SprayMovementLeft { get; set; }
        public void SetSprayMovementLeft (int sprayMovementLeft) { _SprayMovementLeft = sprayMovementLeft;  }
        public State _parent { get; set; }

        public Player(Vector2 playerSpawnPoint, State Parent)
        {
            Init(playerSpawnPoint);
            _parent = Parent;
        }

        public void Init(Vector2 playerSpawnPosition)
        {
            playerPosition = playerSpawnPosition;
            BackgroundColor = ConsoleColor.Green;
            ForegroundColor = ConsoleColor.Black;
            spritePlayer = spriteNormal;
        }

        public void zoomPlayer(int zoom)
        {
            if(zoom == 4) 
            {
                BackgroundColor = ConsoleColor.Green;
                ForegroundColor = ConsoleColor.Black;
                playerPosition = new Vector2(playerPosition.X * zoom, playerPosition.Y * zoom);
            }
            else if(zoom == 1) 
            {
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Red;
                playerPosition = new Vector2(playerPosition.X / 4, playerPosition.Y / 4);
            }
        }

        public void Save()
        {
            //save player pos
            SaveManager.PrepareData(
                new Tuple<string, int>( "PlayerPosX" , ((int)playerPosition.X) ),
                new Tuple<string, int>( "PlayerPosY" , ((int)playerPosition.Y) )
                );

            //save captured pokemon
            SaveManager.PrepareData(
                new Tuple<string, int>("CapturedPokemonNumber", PokemonListManager.PokemonCaptured.Count)
                );
            foreach (var pokemon in PokemonListManager.PokemonCaptured)
            {
                pokemon.Save();
            }

            //Save battle team
            var BattleTeam = PokemonListManager.BattleTeam;
            int numberOfPoke = 0;

            foreach (var pokemon in BattleTeam)
            {
                if (pokemon != null) numberOfPoke++;
            }

            SaveManager.PrepareData(
                new Tuple<string, int>("BattleTeamPokemonNumber", numberOfPoke)
                );
            foreach (var pokemon in BattleTeam)
            {
                if (pokemon != null) pokemon.Save();
            }

        }

      public void Load()
        {
            var data = SaveManager.Loaded;
            //load player pos data
            var PlayerPosX = data?["PlayerPosX"];
            var PlayerPosY = data?["PlayerPosY"];
            if (PlayerPosX != null && PlayerPosY != null)
            {
                playerPosition = new Vector2((float)PlayerPosX, (float)PlayerPosY);
            }

            
            //load captured pokemon
            if (data.ContainsKey("CapturedPokemonNumber")) { 
            var NumberOfCapturedPokemon = data?["CapturedPokemonNumber"];
            int? index = data?.Keys.ToList().IndexOf("CapturedPokemonNumber");
            List<Pokemon> CapturedPokemon = new List<Pokemon>();

                if (NumberOfCapturedPokemon != null)
                {
                    index += 1;
                    for (int i = 0; i < NumberOfCapturedPokemon; i++)
                    {
                        int ID, MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed, Level, CurrentHP, Experience, PokemonNumberOfAttacks = 0;
                        List<int> PokemonAttackPP = new List<int>();

                        ID                     = data.ElementAt((int)index + i * 11 + 0).Value;
                        MaxHP                  = data.ElementAt((int)index + i * 11 + 1).Value;
                        Attack                 = data.ElementAt((int)index + i * 11 + 2).Value;
                        Defense                = data.ElementAt((int)index + i * 11 + 3).Value;
                        SPAttack               = data.ElementAt((int)index + i * 11 + 4).Value;
                        SPDeffense             = data.ElementAt((int)index + i * 11 + 5).Value;
                        Speed                  = data.ElementAt((int)index + i * 11 + 6).Value;
                        Level                  = data.ElementAt((int)index + i * 11 + 7).Value;
                        CurrentHP              = data.ElementAt((int)index + i * 11 + 8).Value;
                        Experience             = data.ElementAt((int)index + i * 11 + 9).Value;
                        PokemonNumberOfAttacks = data.ElementAt((int)index + i * 11 + 10).Value;

                        for (int y = 1; y < PokemonNumberOfAttacks + 1; y++)
                        {
                            PokemonAttackPP.Add(data.ElementAt((int)index + i * 11 + 10 + y).Value);
                        }

                        index += PokemonNumberOfAttacks;
                        //to-do fonction de création de poké
                        // CapturedPokemon.add(pokemon);
                    }

                }
            }

            

            //load captured pokemon
            if (data.ContainsKey("BattleTeamPokemonNumber")) { 
                int? BattleTeamPokemonNumber = data?["BattleTeamPokemonNumber"];
                int? index = data?.Keys.ToList().IndexOf("BattleTeamPokemonNumber");
                List<Pokemon> BattleTeam = new List<Pokemon>();

                if (BattleTeamPokemonNumber != null)
                {
                    index += 1;
                    for (int i = 0; i < BattleTeamPokemonNumber; i++)
                    {
                        int ID, MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed, Level, CurrentHP, Experience, PokemonNumberOfAttacks = 0;
                        List<int> PokemonAttackPP = new List<int>();

                        ID                     = data.ElementAt((int)index + i * 11 + 0).Value;
                        MaxHP                  = data.ElementAt((int)index + i * 11 + 1).Value;
                        Attack                 = data.ElementAt((int)index + i * 11 + 2).Value;
                        Defense                = data.ElementAt((int)index + i * 11 + 3).Value;
                        SPAttack               = data.ElementAt((int)index + i * 11 + 4).Value;
                        SPDeffense             = data.ElementAt((int)index + i * 11 + 5).Value;
                        Speed                  = data.ElementAt((int)index + i * 11 + 6).Value;
                        Level                  = data.ElementAt((int)index + i * 11 + 7).Value;
                        CurrentHP              = data.ElementAt((int)index + i * 11 + 8).Value;
                        Experience             = data.ElementAt((int)index + i * 11 + 9).Value;
                        PokemonNumberOfAttacks = data.ElementAt((int)index + i * 11 + 10).Value;

                        for (int y = 1; y < PokemonNumberOfAttacks + 1; y++)
                        {
                            PokemonAttackPP.Add(data.ElementAt((int)index + i * 11 + 10 + y).Value);
                        }

                        index += PokemonNumberOfAttacks;
                        //to-do fonction de création de poké
                        // BattleTeam.add(pokemon);
                    }


                }
            }


        }

        public void mouvPlayer(char dir)
        {
            float Xposition = 0;
            float Yposition = 0;
            switch (dir)
            {
                case 'N':
                    Xposition = playerPosition.X - incrementMouvement;
                    Yposition = playerPosition.Y;
                    break;
                case 'S':
                    Xposition = playerPosition.X + incrementMouvement;
                    Yposition = playerPosition.Y;
                    break;
                case 'O':
                    Xposition = playerPosition.X;
                    Yposition = playerPosition.Y - incrementMouvement;                   
                    break;
                case 'E':
                    Xposition = playerPosition.X;
                    Yposition = playerPosition.Y + incrementMouvement;
                    break;
            }
            if (_SprayMovementLeft > 0)
                _SprayMovementLeft--;
            playerPosition = new Vector2(Xposition, Yposition);
        }
        public bool collisionWall(char[,] grid, char dir)
        {
            switch (dir)
            {
                case 'N':
                    if (grid[(int)playerPosition.X - incrementMouvement, (int)playerPosition.Y] == '#')
                    {
                        return false;
                    }
                    break;
                case 'S':
                    if (grid[(int)playerPosition.X + incrementMouvement + 3, (int)playerPosition.Y] == '#')
                    {
                        return false;
                    }
                    break;
                case 'O':
                    if (grid[(int)playerPosition.X, (int)playerPosition.Y - incrementMouvement] == '#')
                    {
                        return false;
                    }
                    break;
                case 'E':
                    if (grid[(int)playerPosition.X, (int)playerPosition.Y + incrementMouvement + 3] == '#')
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        public void collisionGrass(char[,] grid, Game game)
        {
            if (grid[(int)playerPosition.X, (int)playerPosition.Y] == '*' && _SprayMovementLeft <= 0)
            {
                const int maxPercentage = 15;
                int percentage = new Random().Next(1, 100);
                if (percentage is > 0 and <= maxPercentage)
                {
                    Game.StatesList?.Push(new CombatState(game));
                }
            }
        }

        public void collisionTeleporter( List<Tuple<string,int,int,string, int, int>> teleporter)
        {
            foreach (var element in teleporter)
            {
                if (playerPosition.Y >= (element.Item3 * 4 - 4) && playerPosition.Y <= (element.Item3 * 4 - 1))
                {
                    if (playerPosition.X >= (element.Item2 * 4 - 4) && playerPosition.X <= (element.Item2 * 4 - 1))
                    {
                        playerPosition = new Vector2(element.Item5*4, element.Item6*4);
                        ((GameState)_parent).ChangeMap(element.Item4);
                    }
                }
            }
        }

        public void collisionItems(char[,] grid)
        {
            char[] ItemsPossibilities = { 'p', 'P', 'h', 'H', 'b', 'B', 'c', 'C', 'S' };
            foreach (var element in ItemsPossibilities)
            {
                if (grid[(int)playerPosition.X, (int)playerPosition.Y] == element)
                {
                    int i = 0;
                }
            }
        }

        public void drawPlayer(int zoom, Tuple<int, int> cameraOffset)
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            if(zoom == 1)
            {
                Console.SetCursorPosition((int)playerPosition.Y, (int)playerPosition.X);
                Console.Write("P");
            }
            else if(zoom == 4)
            {
                Console.SetCursorPosition((int)Game.ConsoleSize.X / 2, (int)Game.ConsoleSize.Y / 2);
                if (cameraOffset.Item2 == 0)
                    Console.SetCursorPosition((int)playerPosition.Y, (int)Game.ConsoleSize.Y / 2);
                else if (cameraOffset.Item1 == 0)
                    Console.SetCursorPosition((int)Game.ConsoleSize.X / 2, (int)playerPosition.X);

                DrawZoomedPlayer();
            }
        }

        private void DrawZoomedPlayer()
        {
            int tempCursorL = Console.GetCursorPosition().Left;
            int tempCursorT = Console.GetCursorPosition().Top;
            for (int i = 0; i < sizePlayer; i++)
            {
                int Ydraw = tempCursorL + i;
                for (int j = 0; j < sizePlayer; j++)
                {
                    int Xdraw = tempCursorT + j;
                    Console.SetCursorPosition(Ydraw, Xdraw);
                    string caraToPrint = spritePlayer[j,i];
                    Console.Write(caraToPrint);
                }
            }
        }
    }
}
