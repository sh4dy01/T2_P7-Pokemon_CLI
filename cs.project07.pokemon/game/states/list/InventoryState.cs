using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.gui;

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

        public Game game;
        private InventoryView _currentView;

        private ButtonManager _buttonManager;
        private Dictionary<string, Button> _buttons;
        private DialogBox _dialogBox;

        public InventoryState(Game gameReceive) : base(gameReceive) 
        { 
            game = gameReceive;
            Init();
        }

        protected override void Init()
        {
            Name = "Inventory";
            _currentView = InventoryView.MENU;
            InitInventory();
        }

        private void InitInventory()
        {
            SwitchView(_currentView);
        }

        private void SwitchView(InventoryView view)
        {
            _currentView = view;

            switch (view)
            {
                case InventoryView.MENU:
                    //_dialogBox.UpdateText("A wild " + _enemyPokemon.Name + " appeared !");
                    break;
                case InventoryView.POKEMON:
                    //_dialogBox.SwitchState(CombatDialogBox.CombatButtonState.SELECT_ACTION);
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
        public override void Update()
        {
            base.Update();
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
