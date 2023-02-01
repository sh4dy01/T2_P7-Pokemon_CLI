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

        private const int IncrementX = 30;

        public Game game;
        private InventoryView _currentView;

        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;

        private PokemonListManager _pokemonListManager;
        public Pokemon[] _pokemonInInventory { get; private set; }

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
            InitMenu();
            Pokemon rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            addPokemon(rdpoke);
            //rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            //addPokemon(rdpoke);
            //rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            //addPokemon(rdpoke);
            //rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            //addPokemon(rdpoke);
            //rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            //addPokemon(rdpoke);
            //rdpoke = new Pokemon(PokemonRegistry.GetRandomPokemon());
            //addPokemon(rdpoke);
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
            HandleKeyEventButtons(pressedKey);
        }

        private void HandleKeyEventButtons(ConsoleKey pressedKey)
        {
            _buttonManager.HandleKeyEvent(pressedKey);
        }

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
            bool first = false;
            _pokemonInInventory = PokemonListManager.BattleTeam;
            foreach (Pokemon pokemon in _pokemonInInventory) 
            {
                count++;
                if(count == 1)
                    first = true;
                else
                    first = false;
                _buttons[pokemon.Name + count] = new Button(_dialogBox, pokemon.Name)
                {
                    Offsets = new Vector2(IncrementX, 1*count),
                    Selected = first,
                    Action = () =>
                    {
                        //TO DO: SHOW Pokemon interface
                    }
                };
            }
            count++;
            _buttons["RETURN"] = new Button(_dialogBox, "Return")
            {
                Selected = false,
                Offsets = new Vector2(IncrementX, 1 * count),
                Action = () =>
                {
                    SwitchView(InventoryView.MENU);
                }
            };
        }


        public override void Update()
        {
            _buttonManager.Update();
        }

        public override void Render()
        {
            _buttonManager.Render();
        }
    }
}
