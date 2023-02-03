using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.entites;
using System.Numerics;
using cs.project07.pokemon.game.items.list;
using cs.project07.pokemon.game.items;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using cs.project07.pokemon.game.Registry;

namespace cs.project07.pokemon.game.states.list
{
    public class InventoryState : State
    {
        private enum InventoryView
        {
            MENU,
            POKEMON,
            POKEDEX,
            ITEMS
        }

        private enum AllItemList
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

        public Game game;
        private InventoryView _currentView;

        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;

        public InventoryState(Game gameReceive) : base(gameReceive) 
        {
            Console.Clear();

            game = gameReceive;
            Init();
        }

        protected override void Init()
        {
            Name = "Inventory";
            _currentView = InventoryView.MENU;
            _dialogBox = new DialogBox(this);
            InitMenu();
        }

        //####### INVENTORY MENU #######//
        private void InitMenu()
        {
            _buttonManager = new ButtonManager();
            _buttons = _buttonManager.Buttons;

            ShowMenu();

            _buttonManager.InitHandleKeyEvent();
        }

        private void ShowMenu()
        {
            _dialogBox.Left = 0;
            _dialogBox.Top = 0;
            _buttons["POKEMON_INV"] = new Button(_dialogBox, "Pokemon Inventory")
            {
                Selected = true,
                //Offsets = new Vector2(3, 1),
                Action = () =>
                {
                    SwitchView(InventoryView.POKEMON);
                },
                BackgroundColor = ConsoleColor.Gray,
                ForegroundColor = ConsoleColor.Black,
                ActiveBackgroundColor = ConsoleColor.DarkGray,
                ActiveForegroundColor = ConsoleColor.Black
            };
            _buttons["ITEMS"] = new Button(_dialogBox, "Items")
            {
                Selected = false,
                //Offsets = new Vector2(3, 2),
                Action = () =>
                {
                    SwitchView(InventoryView.ITEMS);
                },
                BackgroundColor = ConsoleColor.Gray,
                ForegroundColor = ConsoleColor.Black,
                ActiveBackgroundColor = ConsoleColor.DarkGray,
                ActiveForegroundColor = ConsoleColor.Black
            };
            _buttons["EXIT"] = new Button(_dialogBox, "Exit")
            {
                Selected = false,
                //Offsets = new Vector2(3, 3),
                Action = () =>
                {
                    Game.StatesList?.Pop();
                },
                BackgroundColor = ConsoleColor.Gray,
                ForegroundColor = ConsoleColor.Black,
                ActiveBackgroundColor = ConsoleColor.DarkGray,
                ActiveForegroundColor = ConsoleColor.Black
            };
            for (int i = 0; i < _buttons.Count; i++)
            {
                _buttons.ElementAt(i).Value.Offsets += new Vector2(3, 1 + i);
            }
        }

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            //BATTLE TEAM
            _buttonManager.HandleKeyEvent(pressedKey);
            if (_currentView == InventoryView.POKEMON && pressedKey == ConsoleKey.Backspace && !moreStatPoke)
            {
                SwitchView(InventoryView.MENU);
            }
            if (_currentView == InventoryView.POKEMON && pressedKey == ConsoleKey.Backspace && moreStatPoke)
            {
                SwitchView(InventoryView.POKEMON);
            }

            //ITEMS
            if (_currentView == InventoryView.ITEMS && pressedKey == ConsoleKey.Backspace && renderPokeball)
            {
                renderPokeball = false;

                rendTitleItem = false;
                showItems = true;
                ShowItems();
            }
            else if (_currentView == InventoryView.ITEMS && pressedKey == ConsoleKey.Backspace && showPotion)
            {
                showPotion = false;

                _buttons.Clear();
                rendTitleItem = false;
                showItems = true;
                ShowItems();
            }
            else if (_currentView == InventoryView.ITEMS && pressedKey == ConsoleKey.Backspace && showSpray)
            {
                showSpray = false;

                _buttons.Clear();
                rendTitleItem = false;
                showItems = true;
                ShowItems();
            }
            else if (_currentView == InventoryView.ITEMS && pressedKey == ConsoleKey.Backspace && !rendTitleItem)
            {
                SwitchView(InventoryView.MENU);
            }
        }

        //private void HandleKeyEventButtons(ConsoleKey pressedKey)
        //{
        //    _buttonManager.HandleKeyEvent(pressedKey);
        //}

        private void SwitchView(InventoryView view)
        {
            _currentView = view;
            _buttons.Clear();
            switch (view)
            {
                case InventoryView.MENU:
                    renderBattleTeam = false;
                    ShowMenu();
                    break;
                case InventoryView.POKEMON:
                    showMoreStatPoke = -1;
                    moreStatPoke = false;
                    renderBattleTeam= true;
                    initUseButton = true;
                    showInventoryPokemon();
                    break;
                case InventoryView.POKEDEX:
                    //_dialogBox.SwitchState(CombatDialogBox.CombatButtonState.SELECT_ATTACK);
                    break;
                case InventoryView.ITEMS:
                    renderBattleTeam = false;
                    showItems = true;
                    renderItem = true;
                    ShowItems();
                    break;
                default:
                    break;
            }
        }


        //####### BATTLETEAM #######//
        public Pokemon[] _pokemonInInventory { get; private set; }
        bool renderBattleTeam = false;
        int showMoreStatPoke = -1;
        bool moreStatPoke = false;
        private bool initUseButton = false;
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
                if (pokemon is null) continue;
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
                    Offsets = new Vector2(Xpos + 2, Ypos),
                    Selected = first,
                    Action = () =>
                    {
                        //showPokemonStats(pokemon, Xpos, Ypos);
                        showMoreStatPoke = numb;
                        //TO DO: SHOW Pokemon interface
                    },
                    BackgroundColor = ConsoleColor.DarkGray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor= ConsoleColor.White
                 
                };


            }
            count++;
            renderBattleTeam = true;
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
                if (pokemon is null) continue;
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
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.Black;
                for (int i = 0; i <= 6; i++)
                {
                    for (int j = 0; j <= 47; j++)
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
                Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY * 2);
                Console.WriteLine("Type : " + pokemon.Element);
                //Xp
                Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY * 2);
                Console.WriteLine("XP : " + pokemon.Experience + "/" + pokemon.RequiredExp);

                if (count == showMoreStatPoke)
                {
                    BackgroundColor = ConsoleColor.DarkGray;

                    for (int i = 0; i <= 9; i++)
                    {
                        for (int j = 0; j <= 47; j++)
                        {
                            Console.SetCursorPosition((int)Xpos - increX - 3 + j, (int)Ypos + 6 + +1 * i);
                            Console.WriteLine(" ");
                        }
                    }
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
                        _buttons.Clear();
                    }
                    int calcX = (int)Xpos - increX;
                    int calcY = (int)Ypos + increY * 9;
                    _dialogBox.Left = 0;
                    _dialogBox.Top = 0;
                    navButtonPokeStat(calcX, calcY);
                    _buttonManager.Update();

                }

                int tempX = inlineX;
                int tempY = inlineY;
                int tempCount = count;
                count = 0;
                inlineX = 0;
                inlineY = 1;

                foreach (Pokemon pok in _pokemonInInventory)
                {
                    if (pok is null) continue;
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
                    Console.SetCursorPosition((int)Xpos2 + 2, (int)Ypos2);
                    Console.WriteLine(pok.Name);
                }
                count = tempCount;
                inlineX = tempX;
                inlineY = tempY;
            }
        }

        private void navButtonPokeStat(int Xpos, int Ypos)
        {
            if(initUseButton) 
            {
                _buttons["POTIONS"] = new Button(_dialogBox, "Potions")
                {
                    Selected = true,
                    Offsets = new Vector2(Xpos, Ypos),
                    Action = () =>
                    {
                        SwitchView(InventoryView.ITEMS);
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
                initUseButton = false;
            }
        }


        //####### ITEM MENU #######//
        bool showItems = false;
        private bool renderItem = false;
        bool renderPokeball = false;
        bool showPotion = false;
        bool showSpray = false;
        bool rendTitleItem = false;


        private void ShowItems()
        {
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            if(showItems)
            {
                _buttons["POKEBALLS"] = new Button(_dialogBox, "Pokeballs")
                {
                    Offsets = new Vector2(Xpos, Ypos),
                    Selected = true,
                    Action = () =>
                    {
                        _buttons.Clear();
                        rendTitleItem = true;
                        renderPokeball = true;
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor = ConsoleColor.Black
                };
                _buttons["POTIONS"] = new Button(_dialogBox, "Potions")
                {
                    Selected = false,
                    Offsets = new Vector2(Xpos*2, Ypos),
                    Action = () =>
                    {
                        _buttons.Clear();
                        showPotion = true;
                        rendTitleItem = true;
                        ShowPotionList();
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor = ConsoleColor.Black
                };
                _buttons["SPRAYS"] = new Button(_dialogBox, "Sprays")
                {
                    Selected = false,
                    Offsets = new Vector2(Xpos*3, Ypos),
                    Action = () =>
                    {
                        _buttons.Clear();
                        showSpray = true;
                        rendTitleItem = true;
                        ShowSprayList();
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor = ConsoleColor.Black
                };
                showItems = false;
            }
        }


        private static void RenderTitleItems()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            Console.SetCursorPosition(Xpos, Ypos);
            Console.WriteLine("Pokeballs");
            Console.SetCursorPosition(Xpos * 2, Ypos);
            Console.WriteLine("Potions");
            Console.SetCursorPosition(Xpos * 3, Ypos);
            Console.WriteLine("Sprays");
        }

        private void showPokeballList()
        {
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            int count = 0;

            foreach (var pokeball in InventoryManager.Inventory.Where(Item => Item.GetType() == typeof(Pokeball)))
            {
                count++;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(Xpos, Ypos + 2 * count + 1);
                Console.WriteLine(pokeball.Name + " " + pokeball.GetQuantity());
            }
        }


        private void ShowPotionList()
        {
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            int count = 0;
            bool first = false;
            foreach (var potion in InventoryManager.Inventory.Where(Item=>Item.GetType() == typeof(Potion)))
            {
                count++;
                if (count == 1) //Why first show is 3 ????
                    first = true;
                else
                    first = false;
                string temp = potion.Name + " " + potion.GetQuantity();
                _buttons[potion.Name] = new Button(_dialogBox, temp)
                {
                    Selected = first,
                    Offsets = new Vector2(Xpos * 2, Ypos + 2 * count + 1),
                    Action = () =>
                    {
                        _buttons.Clear();
                        selectPokemonToUse((Potion)potion);
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor = ConsoleColor.Black
                };
            }
        }

        private void selectPokemonToUse(Potion potion)
        {
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            int count = 0;
            bool first = false;
            foreach (var pokemon in PokemonListManager.BattleTeam)
            {
                if(pokemon is null) continue;
                count++;
                if (count == 1) //Why first show is 3 ????
                    first = true;
                else
                    first = false;
                string temp = pokemon.Name + " HP :" + pokemon.Currenthealth + "/" + pokemon.Stat.MaxHP;
                _buttons[pokemon.Name + count] = new Button(_dialogBox, temp)
                {
                    Selected = first,
                    Offsets = new Vector2(Xpos * 2, Ypos + 2 * count + 1),
                    Action = () =>
                    {
                        potion.Use(pokemon);
                        _buttons.Clear();
                        ShowPotionList();
                    },
                    BackgroundColor = ConsoleColor.Gray,
                    ForegroundColor = ConsoleColor.Black,
                    ActiveBackgroundColor = ConsoleColor.DarkGray,
                    ActiveForegroundColor = ConsoleColor.Black
                };
            }
        }

        private void ShowSprayList()
        {
            int Xpos = Console.WindowWidth / 4;
            int Ypos = 5;
            int count = 0;
            bool first = false;
            foreach (var Spray in InventoryManager.Inventory.Where(Item => Item.Name == "Spray"))
            {
                if (Spray.Name == "Spray")
                {
                    count++;
                    if (count == 1) //Why first show is 3 ????
                        first = true;
                    else
                        first = false;
                    string temp = Spray.Name + " " + Spray.GetQuantity();
                    _buttons[Spray.Name] = new Button(_dialogBox, temp)
                    {
                        Selected = first,
                        Offsets = new Vector2(Xpos * 3, Ypos + 2 * count + 1),
                        Action = () =>
                        {
                            Spray.Use();
                            _buttons.Clear();
                            showItems = true;
                            ShowItems();
                        },
                        BackgroundColor = ConsoleColor.Gray,
                        ForegroundColor = ConsoleColor.Black,
                        ActiveBackgroundColor = ConsoleColor.DarkGray,
                        ActiveForegroundColor = ConsoleColor.Black
                    };
                }
            }
        }


        public override void Update()
        {
            if (renderItem)
                ShowItems();
            _buttonManager.Update();
        }

        public override void Render()
        {
            PaintBackground();
            if (renderBattleTeam)
                drawBattelTeam();
            if (renderPokeball)
                showPokeballList();
            if(rendTitleItem)
                RenderTitleItems();
            _buttonManager.Render();
        }
    }
}
