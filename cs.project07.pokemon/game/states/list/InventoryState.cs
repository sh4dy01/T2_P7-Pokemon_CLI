using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.entites;
using System.Numerics;
using cs.project07.pokemon.game.items.list;
using cs.project07.pokemon.game.items;

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

        private PokemonListManager _pokemonListManager;
        public Pokemon[] _pokemonInInventory { get; private set; }
        bool drawInventory = false;

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
            _pokemonListManager = new PokemonListManager();
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
            _pokemonListManager.AddPokemon(pokemonToADD);
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
                        //TO DO: SHOW Pokemon interface
                    }
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

        private void drawStatPokemon()
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
                    for(int j = 0; j <= 40; j++)
                    {
                        Console.SetCursorPosition((int)Xpos - increX - 1 + j, (int)Ypos - 1 + 1 * i);
                        Console.WriteLine(" ");
                    }
                }
                //HP
                Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY);
                Console.WriteLine("HP : " + pokemon.Currenthealth + "/" + pokemon.MaxHealth);
                //Lvl
                Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY);
                Console.WriteLine("Lvl : " + pokemon.Level);
                //Element
                Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY*2);
                Console.WriteLine("Type : " + pokemon.Element);
                //Xp
                Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY*2);
                Console.WriteLine("XP : " + pokemon.Experience + "/" + pokemon.RequiredExp);
                ////STAT//
                //Console.SetCursorPosition((int)Xpos + 2, (int)Ypos + increY*4);
                //Console.WriteLine("STATS");
                ////Attack
                //Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY*5);
                //Console.WriteLine("Attack : " + pokemon.Attack);
                ////Defense
                //Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY * 5);
                //Console.WriteLine("Defense : " + pokemon.Defense);
                ////SPAttack
                //Console.SetCursorPosition((int)Xpos - increX, (int)Ypos + increY * 6);
                //Console.WriteLine("SPAttack : " + pokemon.SPAttack);
                ////SPDefense
                //Console.SetCursorPosition((int)Xpos + increX, (int)Ypos + increY * 6);
                //Console.WriteLine("SPDefense : " + pokemon.SPDefense);
                ////Speed
                //Console.SetCursorPosition((int)Xpos, (int)Ypos + increY * 7);
                //Console.WriteLine("Speed : " + pokemon.Speed);
            }
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
                drawStatPokemon();
            }
            _buttonManager.Render();
        }
    }
}
