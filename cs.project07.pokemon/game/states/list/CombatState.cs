using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.states.gui;

namespace cs.project07.pokemon.game.states.list
{
    public class CombatState : State
    {
        enum CombatView
        {
            INTRO,
            SELECT_ACTION,
            SELECT_ATTACK,
            ACTION_USE,
            ACTION_PET,
            EFFECTIVE,
            ENEMY_ATTACK,
            ENEMY_EFFECTIVE,
            END_COMBAT
        };

        private PokedexEntry _playerPokemon;
        private PokedexEntry _enemyPokemon;
        private bool _isPlayerTurn;
        private CombatView _currentView;

        private CombatDialogBox _dialogBox;

        public PokedexEntry PlayerPokemon { get => _playerPokemon; }

        public CombatState(Game game, PokedexEntry? playerPokemon = null, PokedexEntry? enemyPokemon = null) : base(game)
        {
            _playerPokemon = PokemonRegistry.GetRandomPokemon();
            _enemyPokemon = PokemonRegistry.GetRandomPokemon();
            _dialogBox = new CombatDialogBox(this);
            Init();
        }

        protected override void Init()
        {
            Name = "Combat";
            _isPlayerTurn = true;
            _currentView = CombatView.INTRO;
            InitCombat();
        }

        private void InitCombat()
        {
            SwitchView(_currentView);
        }
        
        private void SwitchView(CombatView view)
        {
            _currentView = view;
            
            switch (view)
            {
                case CombatView.INTRO:
                    _dialogBox.UpdateText("A wild " + _enemyPokemon.Name + " appeared !");
                    break;
                case CombatView.SELECT_ACTION:
                    _dialogBox.SwitchState(CombatDialogBox.CombatButtonState.SELECT_ACTION);
                    break;
                case CombatView.SELECT_ATTACK:
                    _dialogBox.SwitchState(CombatDialogBox.CombatButtonState.SELECT_ATTACK);
                    break;
                case CombatView.ACTION_USE:
                    break;
                case CombatView.ACTION_PET:
                    break;
                case CombatView.EFFECTIVE:
                    break;
                case CombatView.ENEMY_ATTACK:
                    break;
                case CombatView.ENEMY_EFFECTIVE:
                    break;
                case CombatView.END_COMBAT:
                    break;
                default:
                    break;
            }
        }

        /*public void SwitchPlayerPokemon(PokemonCaptured pokemon)
        {
            _playerPokemon = pokemon;
        }*/ //TODO
        
        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.Enter:
                    if (_dialogBox.ButtonManager.Buttons.Count <= 0)
                    {
                        var NextState = _currentView + 1;
                        SwitchView(NextState);
                        break;
                    }
                    Button.ExecuteAction(_dialogBox.ButtonManager.Buttons);
                    break;
                case ConsoleKey.UpArrow:
                    Button.SelectPrevious(_dialogBox.ButtonManager.Buttons);
                    break;
                case ConsoleKey.DownArrow:
                    Button.SelectNext(_dialogBox.ButtonManager.Buttons);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            // Update childs
            // ------ Map
        }

        public override void Render()
        {
            base.Render();
            _dialogBox.Render();
            // Render childs
            // ------ Map
            
        }
    }
}
