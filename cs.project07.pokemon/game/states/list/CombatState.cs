using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.map;
using cs.project07.pokemon.game.states.gui;

namespace cs.project07.pokemon.game.states.list
{
    public class CombatState : State
    {
        public enum CombatView
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

        private bool _isPlayerTurn;

        private Pokemon _playerPokemon;
        private Pokemon _enemyPokemon;

        private CombatView _currentView;
        private CombatDialogBox _dialogBox;

        private PokemonInfo _playerPokemonUI;
        private PokemonInfo _enemyPokemonUI;

        public Pokemon PlayerPokemon { get => _playerPokemon; }


        public CombatState(Game game, PokedexEntry? playerPokemon = null, PokedexEntry? enemyPokemon = null) : base(game)
        {
            _playerPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon());
            _enemyPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon());
            _dialogBox = new CombatDialogBox(this);
            _playerPokemonUI = new PokemonInfo(this, _playerPokemon, false);
            _enemyPokemonUI = new PokemonInfo(this, _enemyPokemon, true);

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
        
        public void SwitchView(CombatView view)
        {
            _dialogBox.ResetButtons();
            _currentView = view;
            
            switch (view)
            {
                case CombatView.INTRO:
                    _dialogBox.UpdateText("A wild " + _enemyPokemon.Name + " appeared !");
                    break;
                case CombatView.SELECT_ACTION:
                    _dialogBox.InitSelectActionButtons();
                    break;
                case CombatView.SELECT_ATTACK:
                    _dialogBox.InitSelectAttackButtons(_playerPokemon.Attacks);
                    break;
                case CombatView.ACTION_USE:
                    break;
                case CombatView.ACTION_PET:
                    break;
                case CombatView.EFFECTIVE:
                    _dialogBox.ResetButtons();
                    _dialogBox.UpdateText("Blablabal");
                    _enemyPokemonUI.UpdateUI(_enemyPokemon);
                    break;
                case CombatView.ENEMY_ATTACK:
                    //Enemy attack turn
                    break;
                case CombatView.ENEMY_EFFECTIVE:
                    //Depending the element say if it's effective or not on our pokemon
                    break;
                case CombatView.END_COMBAT:
                    //Win text if enemy dead
                    //Loose text if pokemon dead
                    break;
                default:
                    break;
            }
        }

        /*public void SwitchPlayerPokemon(PokemonCaptured pokemon)
        {
            _playerPokemon = pokemon;
        }*/ //TODO

        public void DealEnemyDamage(int amount)
        {
            _enemyPokemon.DealDamage(amount);
            SwitchView(CombatView.EFFECTIVE);
        }
        
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
            _playerPokemonUI.Render();
            _enemyPokemonUI.Render();

            // Render childs
            // ------ Map

        }
    }
}
