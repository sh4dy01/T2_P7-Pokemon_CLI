using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.entites;
using System.Numerics;
using cs.project07.pokemon.game.items.list;
using cs.project07.pokemon.game.items;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace cs.project07.pokemon.game.states.list
{
    public class InventoryState : State
    {
        enum InventoryView
        {
            MENU,
            POKEMON,
            POKEDEX,
            POTION,
            POKEBALL,
            SPRAY
        }

        enum AllItemList
        {
            POKEBALL,
            SUPERBALL,
            HYPERBALL,
            MASTERBALL,
            POTION,
            SUPERPOTION,
            HYPERPOTION,
            POTIONMAX,
            SPRAY
        }

        private const int IncrementX = 3;

        public Game game;
        private InventoryView _currentView;

        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;
        public Pokemon[] _pokemonInInventory { get; private set; }
        bool drawInventory = false;
        int showMoreStatPoke = -1;
        bool moreStatPoke = false;

        public List<Item> _itemList { get; private set; }

        public InventoryState(Game gameReceive) : base(gameReceive) 
        { 
            game = gameReceive;
            Init();
        }

        protected override void Init()
        {
            Name = "Inventory";
            _currentView = InventoryView.MENU;
            _dialogBox = new DialogBox(this);
            InitItem();
            InitMenu();
        }

        private void InitItem()
        {
            _itemList = new List<Item>();
            for(int i = 0; i < 4; i++)
            {
                _itemList.Add(new Pokeball(i));
            }
            for (int i = 0; i < 4; i++)
            {
                _itemList.Add(new Potion(i));
            }
            _itemList.Add(new Spray());
        }

        private void InitMenu()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;

            ShowMenu();

            _buttonManager.InitHandleKeyEvent();


            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons.ElementAt(i).Value.Offsets += new Vector2(3, 1 + i);
            }
        }

        private void ShowMenu()
        {
            _dialogBox.Left = 0;
            _dialogBox.Top = 0;
            _buttons["POKEMON_INV"] = new Button(_dialogBox, "Pokemon Inventory")
            {
                Selected = true,
                //Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    SwitchView(InventoryView.POKEMON);
                }
            };
            _buttons["SAVE"] = new Button(_dialogBox, "Save")
            {
                Selected = false,
                //Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    //TO DO SAVE THE GAME
                }
            };
            _buttons["EXIT"] = new Button(_dialogBox, "Exit")
            {
                Selected = false,
                //Offsets = new Vector2(150, 0),
                Action = () =>
                {
                    Game.StatesList?.Pop();
                }
            };
        }

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            _buttonManager.HandleKeyEvent(pressedKey);
        }

        //private void HandleKeyEventButtons(ConsoleKey pressedKey)
        //{
        //    _buttonManager.HandleKeyEvent(pressedKey);
        //}

        private void SwitchView(InventoryView view)
        {
            _currentView = view;
            foreach (var button in _buttons)
            {
                _buttons.Remove(button.Key);
            }
            switch (view)
            {
                case InventoryView.MENU:
                    ShowMenu();
                    break;
                case InventoryView.POKEMON:
                    showInventoryPokemon();
                    break;
                case InventoryView.POKEDEX:
                    //_dialogBox.SwitchState(CombatDialogBox.CombatButtonState.SELECT_ATTACK);
                    break;
                case InventoryView.POTION:
                    break;
                case InventoryView.POKEBALL:
                    break;
                case InventoryView.SPRAY:
                    break;
                default:
                    break;
            }
        }



        public void addPokemon(Pokemon pokemonToADD)
        {
            //if(_pokemonInInventory.Count <= 6)
            //{
            //    _pokemonInInventory.Add(pokemonToADD);
            //}
            PokemonListManager.AddPokemon(pokemonToADD);
        }

        public void showInventoryPokemon()
        {
            int count = 0;
            int inlineX = 0;
            int inlineY = 1;
            bool first = false;
            _dialogBox.Left = 0;
            _dialogBox.Top = 0;
            _pokemonInInventory = PokemonListManager.BattleTeam;
            foreach (Pokemon pokemon in _pokemonInInventory) 
            {
                count++;
                int numb = count;
                inlineX++;
                if(count == 1) //Why first show is 3 ????
                    first = true;
                else
                    first = false;
                if(count == 4)
                {
                    inlineX=1;
                    inlineY = 3;
                }
                float Xpos = Console.WindowWidth / 6 * inlineX * 1.5f;
                float Ypos = Console.WindowHeight / 5 * inlineY;
                _buttons[pokemon.Name + count] = new Button(_dialogBox, pokemon.Name)
                {
                    Offsets = new Vector2(Xpos, Ypos),
                    Selected = first,
                    Action = () =>
                    {
                        //showPokemonStats(pokemon, Xpos, Ypos);
                        showMoreStatPoke = numb;
                        //TO DO: SHOW Pokemon interface
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor= ConsoleColor.Black
                 
                };


            }
            count++;
            drawInventory = true;
            //_buttons["RETURN"] = new Button(_dialogBox, "Return")
            //{
            //    Selected = false,
            //    Offsets = new Vector2(0, 0),
            //    Action = () =>
            //    {
            //        SwitchView(InventoryView.MENU);
            //    }
            //};
            //for (int i = 0; i < _buttons.Count; i++)
            //{
            //    _buttons.ElementAt(i).Value.Offsets += new Vector2(3, 1 + i);
            //}
        }

        private void drawBattelTeam()
        {
            int count = 0;
            int inlineX = 0;
            int inlineY = 1;
            foreach (Pokemon pokemon in _pokemonInInventory)
            {

                count++;
                inlineX++;
                if (count == 4)
                {
                    inlineX = 1;
                    inlineY = 3;
                }
                float Xpos = Console.WindowWidth / 6 * inlineX * 1.5f;
                float Ypos = Console.WindowHeight / 5 * inlineY;
                const int increX = 15;
                const int increY = 2;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                for(int i = 0; i <= 6;i++)
                {
                    for(int j = 0; j <= 47; j++)
                    {
                        Console.SetCursorPosition((int)Xpos - increX - 3 + j, (int)Ypos - 1 + 1 * i);
                        Console.WriteLine(" ");
                    }
                }
                //HP
                Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY);
                Console.WriteLine("HP : " + pokemon.Currenthealth + "/" + pokemon.Stat.MaxHP);
                //Lvl
                Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY);
                Console.WriteLine("Lvl : " + pokemon.Level);
                //Element
                Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY*2);
                Console.WriteLine("Type : " + pokemon.Element);
                //Xp
                Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY*2);
                Console.WriteLine("XP : " + pokemon.Experience + "/" + pokemon.RequiredExp);

                if(count == showMoreStatPoke)
                {
                    //for (int i = 0; i <= 9; i++)
                    //{
                    //    for (int j = 0; j <= 47; j++)
                    //    {
                    //        Console.SetCursorPosition((int)Xpos - increX - 3 + j, (int)Ypos + 6 + + 1 * i);
                    //        Console.WriteLine(" ");
                    //    }
                    //}
                    //STAT//
                    Console.SetCursorPosition((int)Xpos + 2, (int)Ypos + increY * 4);
                    Console.WriteLine("STATS");
                    //Attack
                    Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY * 5);
                    Console.WriteLine("Attack : " + pokemon.Stat.Attack);
                    //Defense
                    Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY * 5);
                    Console.WriteLine("Defense : " + pokemon.Stat.Defense);
                    //SPAttack
                    Console.SetCursorPosition((int)Xpos - increX - 2, (int)Ypos + increY * 6);
                    Console.WriteLine("SPAttack : " + pokemon.Stat.SPAttack);
                    //SPDefense
                    Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY * 6);
                    Console.WriteLine("SPDefense : " + pokemon.Stat.SPDefense);
                    //Speed
                    Console.SetCursorPosition((int)Xpos, (int)Ypos + increY * 7);
                    Console.WriteLine("Speed : " + pokemon.Stat.Speed);

                    if (!moreStatPoke)
                    {
                        moreStatPoke = true;
                        foreach (var button in _buttons)
                        {
                            _buttons.Remove(button.Key);
                        }
                    }
                    navButtonPokeStat((int)Xpos - increX, (int)Ypos + increY * 9);
                }
                int tempX = inlineX;
                int tempY = inlineY;
                int tempCount = count;
                count = 0;
                inlineX = 0;
                inlineY = 1;
                //_dialogBox.Left = 0;
                //_dialogBox.Top = 0;
                foreach (Pokemon pok in _pokemonInInventory)
                {
                    BackgroundColor = ConsoleColor.Gray;
                    ForegroundColor = ConsoleColor.Black;
                    count++;
                    inlineX++;
                    if (count == 4)
                    {
                        inlineX = 1;
                        inlineY = 3;
                    }
                    float Xpos2 = Console.WindowWidth / 6 * inlineX * 1.5f;
                    float Ypos2 = Console.WindowHeight / 5 * inlineY;
                    Console.SetCursorPosition((int)Xpos2, (int)Ypos2);
                    Console.WriteLine(pok.Name);
                }
                count = tempCount;
                inlineX = tempX;
                inlineY = tempY;
            }
        }

        private void navButtonPokeStat(int Xpos, int Ypos)
        {
            _dialogBox.Left = 0;
            _dialogBox.Top = 0;
            _buttons["USE_ITEM"] = new Button(_dialogBox, "Items")
            {
                Selected = true,
                Offsets = new Vector2(Xpos, Ypos),
                Action = () =>
                {
                    //TO DO SAVE THE GAME
                },
                BackgroundColor = ConsoleColor.Gray,
                ForegroundColor = ConsoleColor.Black,
                ActiveBackgroundColor = ConsoleColor.DarkGray,
                ActiveForegroundColor = ConsoleColor.Black
            };
            _buttons["SWAP_TEAM"] = new Button(_dialogBox, "Swap pokemon")
            {
                Selected = false,
                Offsets = new Vector2(Xpos+30, Ypos),
                Action = () =>
                {
                    SwitchView(InventoryView.POKEMON);
                },
                BackgroundColor = ConsoleColor.Gray,
                ForegroundColor = ConsoleColor.Black,
                ActiveBackgroundColor = ConsoleColor.DarkGray,
                ActiveForegroundColor = ConsoleColor.Black
            };
        }

        public override void Update()
        {
            _buttonManager.Update();
        }

        public override void Render()
        {
            PaintBackground();
            if (drawInventory)
            {
                drawBattelTeam();
            }
            _buttonManager.Render();
        }
    }
}
