﻿using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.states;
using cs.project07.pokemon.game.states.list;
using System.Numerics;
using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.Registry;
using cs.project07.pokemon.game.states.gui;

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
        private DialogBox? dialog;
        
        public static int _SprayMovementLeft { get; set; }
        public static void SetSprayMovementLeft (int sprayMovementLeft) { _SprayMovementLeft = sprayMovementLeft;  }
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


            //Save Items
            foreach (var item in InventoryManager.Inventory)
            {
                SaveManager.PrepareData(
                new Tuple<string, int>(Convert.ToString(item.ID), item.GetQuantity())
                );
            }
        }

      public void Load()
        {
            var data = SaveManager.Loaded;
            //load player pos data
            if (data != null) if (data.ContainsKey("PlayerPosX") && data.ContainsKey("PlayerPosY")) {
                var PlayerPosX = data?["PlayerPosX"];
                var PlayerPosY = data?["PlayerPosY"];
                if (PlayerPosX != null && PlayerPosY != null)
                {
                    playerPosition = new Vector2((float)PlayerPosX, (float)PlayerPosY);
                }
            }




            int id = 1;

            //load battleTeam pokemon
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
                        BattleTeam.Add(LoadPokemon(MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed, ID, Level, Experience, CurrentHP, PokemonAttackPP,id));
                        id++;
                    }
                    index = 0;
                    Pokemon[] result = new Pokemon[6];
                    foreach (Pokemon p in BattleTeam) 
                    {
                        result[(int)index] = p;
                        index++;
                    }
                    PokemonListManager.SetBattleTeam(result);
                }

                //load captured pokemon
                if (data.ContainsKey("CapturedPokemonNumber"))
                {
                    var NumberOfCapturedPokemon = data?["CapturedPokemonNumber"];
                    index = data?.Keys.ToList().IndexOf("CapturedPokemonNumber");
                    List<Pokemon> CapturedPokemon = new List<Pokemon>();

                    if (NumberOfCapturedPokemon != null)
                    {
                        index += 1;
                        for (int i = 0; i < NumberOfCapturedPokemon; i++)
                        {
                            int ID, MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed, Level, CurrentHP, Experience, PokemonNumberOfAttacks = 0;
                            List<int> PokemonAttackPP = new List<int>();

                            ID = data.ElementAt((int)index + i * 11 + 0).Value;
                            MaxHP = data.ElementAt((int)index + i * 11 + 1).Value;
                            Attack = data.ElementAt((int)index + i * 11 + 2).Value;
                            Defense = data.ElementAt((int)index + i * 11 + 3).Value;
                            SPAttack = data.ElementAt((int)index + i * 11 + 4).Value;
                            SPDeffense = data.ElementAt((int)index + i * 11 + 5).Value;
                            Speed = data.ElementAt((int)index + i * 11 + 6).Value;
                            Level = data.ElementAt((int)index + i * 11 + 7).Value;
                            CurrentHP = data.ElementAt((int)index + i * 11 + 8).Value;
                            Experience = data.ElementAt((int)index + i * 11 + 9).Value;
                            PokemonNumberOfAttacks = data.ElementAt((int)index + i * 11 + 10).Value;

                            for (int y = 1; y < PokemonNumberOfAttacks + 1; y++)
                            {
                                PokemonAttackPP.Add(data.ElementAt((int)index + i * 11 + 10 + y).Value);
                            }

                            index += PokemonNumberOfAttacks;
                            CapturedPokemon.Add(LoadPokemon(MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed, ID, Level, Experience, CurrentHP, PokemonAttackPP,id));
                            id++;
                        }
                        PokemonListManager.SetPokemonCaptured(CapturedPokemon);
                    }
                }

                // load items
                char[] ItemsPossibilities = InventoryManager.ItemsPossibilities;
                foreach (var ID in ItemsPossibilities)
                {
                    string strID = ID.ToString();
                    foreach (var item in InventoryManager.Inventory)
                    {
                        if (ID == item.ID) if (data.ContainsKey(strID)) item.SetQuantity(data[strID]);
                    }
                }

            }


        }

      private static Pokemon LoadPokemon(int MaxHP, int Attack, int Defense, int SPAttack, int SPDeffense, int Speed, int ID,
          int Level, int Experience, int CurrentHP, List<int> PokemonAttackPP,int id)
      {
          var stat = new Stat((MaxHP, Attack, Defense, SPAttack, SPDeffense, Speed));
          var pokemon = new Pokemon(PokemonRegistry.GetPokemonByPokedexId(ID), Level, stat, Experience, CurrentHP);
            pokemon.SetId(id);

          int count = 0;
          foreach (var attack in pokemon.Attacks)
          {
              attack.SetPP(PokemonAttackPP[count]);
              count++;
          }

          return pokemon;
      }

      public void Render()
        {
            if (dialog != null) dialog.Render();
        }

        public void StopDialog()
        {
            dialog= null;
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
                    Game.StatesList?.Push(new CombatState(game, false));
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

        public void collisionItems(char[,] grid, Map map)
        {
            char[] ItemsPossibilities = InventoryManager.ItemsPossibilities;
            foreach (var element in ItemsPossibilities)
            {
                if (grid[(int)playerPosition.X, (int)playerPosition.Y] == element)
                {
                    foreach (var item in InventoryManager.Inventory)
                    {
                        if (item.ID == element)
                        {
                            item.Add();
                            map.ModifyMap((int)playerPosition.X, (int)playerPosition.Y, "ITEMS", '\0');
                            map.ModifyMap((int)playerPosition.X, (int)playerPosition.Y, "GROUND", ' ');
                            int id = map.ItemsId;
                            SaveManager.PrepareData(
                                new Tuple<string, int>("item" + id + "x", (int)playerPosition.X),
                                new Tuple<string, int>("item" + id + "y", (int)playerPosition.Y)
                                );
                            dialog = new DialogBox(_parent);
                            dialog.UpdateText("vous avez récupéré x1 "+ item.Name);
                            dialog.Render();
                            
                        }

                    }
                    
                }
            }
        }

        public void collisionBoss(char[,] grid, Game game)
        {
            if (grid[(int)playerPosition.X, (int)playerPosition.Y] == 'A')
            {
                Game.StatesList?.Push(new CombatState(game, true));
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
