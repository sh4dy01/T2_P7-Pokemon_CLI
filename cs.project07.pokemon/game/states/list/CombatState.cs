using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.combat;

namespace cs.project07.pokemon.game.states.list
{
    public class CombatState : State
    {
        const string EFFECTIVE_MSG = "It's super effective!";
        const string INEFFECTIVE_MSG = "It's not very effective...";
        
        public enum CombatView
        {
            INTRO,
            SELECT_ACTION,
            SELECT_ATTACK,
            EFFECTIVE,
            ACTION_USE,
            ACTION_PET,
            ENEMY_ATTACK,
            ENEMY_EFFECTIVE,
            END_COMBAT
        };

        private bool _isPlayerTurn;
        private string _effectivenessMessage = "";

        private CombatView _currentView;
        private readonly CombatDialogBox _dialogBox;
        private readonly Pokemon _playerPokemon;
        private readonly Pokemon _enemyPokemon;
        private readonly PokemonInfoBox _playerPokemonUi;
        private readonly PokemonInfoBox _enemyPokemonUi;

        public CombatState(Game game, PokedexEntry? playerPokemon = null, PokedexEntry? enemyPokemon = null) : base(game)
        {
            _playerPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon()); //TODO : Get selected Pokemon
            _enemyPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon()); //TODO : Get the random pokemon

            _dialogBox = new CombatDialogBox(this);
            _playerPokemonUi = new PokemonInfoBox(this, _playerPokemon, false);
            _enemyPokemonUi = new PokemonInfoBox(this, _enemyPokemon, true);

            Init();
        }

        protected override void Init()
        {
            Name = "Combat";
            _isPlayerTurn = true;
            _currentView = CombatView.INTRO;
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
                    _dialogBox.ResetText();
                    _dialogBox.InitSelectActionButtons();
                    break;
                case CombatView.SELECT_ATTACK:
                    _currentView = CombatView.SELECT_ATTACK;
                    _dialogBox.InitSelectAttackButtons(_playerPokemon.Attacks);
                    break;
                case CombatView.EFFECTIVE:
                    _dialogBox.UpdateText(_effectivenessMessage);
                    CheckIfCombatEnd();
                    break;
                case CombatView.ACTION_USE:
                    //TODO
                    break;
                case CombatView.ACTION_PET:
                    //TODO
                    break;
                case CombatView.ENEMY_ATTACK:
                    DealPlayerDamage();
                    break;
                case CombatView.ENEMY_EFFECTIVE:
                    _dialogBox.UpdateText(_effectivenessMessage);
                    CheckIfCombatEnd();
                    break;
                case CombatView.END_COMBAT:
                    Game.StatesList.Pop();
                    break;
            }
        }

        /*public void SwitchPlayerPokemon(PokemonCaptured pokemon)
        {
            _playerPokemon = pokemon;
        }*/ //TODO

        private void CheckIfCombatEnd()
        {
            if (_playerPokemon.IsDead)
            {
                _dialogBox.UpdateText("You lost !");
                //Switch Pokemon
                SwitchView(CombatView.END_COMBAT);
            }
            else if (_enemyPokemon.IsDead)
            {
                _dialogBox.UpdateText("You won, GG !");
                SwitchView(CombatView.END_COMBAT);
            }
            else switch (_isPlayerTurn)
            {
                case true:
                    SwitchView(CombatView.SELECT_ACTION);
                    break;
                case false:
                    SwitchView(CombatView.ENEMY_ATTACK);
                    break;
            }
        }

        public void DealEnemyDamage(Attack attack)
        {
            _dialogBox.UpdateText("You'r " + _playerPokemon.Name + " used " + attack.Name + " !");
            _enemyPokemon.TakeDamage(DamageWithMultiplier(attack, _playerPokemon, _enemyPokemon));
            _enemyPokemonUi.UpdateUI(_enemyPokemon);
            _isPlayerTurn = false;
            _dialogBox.ResetButtons();
        }
        
        private void DealPlayerDamage()
        {
            Attack attack = _enemyPokemon.ChooseRandomAttack();
            _dialogBox.UpdateText("The enemy " + _enemyPokemon.Name + " used " + attack.Name + " !");
            _playerPokemon.TakeDamage(DamageWithMultiplier(attack, _enemyPokemon, _playerPokemon));
            _playerPokemonUi.UpdateUI(_playerPokemon);
            _isPlayerTurn = true;
        }

        private float DamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender)
        {
            float damageMultiplier = TypeChart.GetDamageMultiplier(attacker.Type, defender.Type);
            UpdateEffectivenessMessage(damageMultiplier);
            
            return attack.Damage * damageMultiplier;
        }

        private void UpdateEffectivenessMessage(float damageMultiplier)
        {
            switch (damageMultiplier)
            {
                case 2:
                    _effectivenessMessage = EFFECTIVE_MSG;
                    break;
                case 0.5f:
                    _effectivenessMessage = INEFFECTIVE_MSG;
                    break;
                default:
                    _effectivenessMessage = "Ok...";
                    break;
            }
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
                case ConsoleKey.Backspace:
                    switch (_currentView)
                    {
                        case CombatView.SELECT_ATTACK:
                            SwitchView(CombatView.SELECT_ACTION);
                            break;
                    }
                    break;
            }
        }

        public override void Update()
        {
            base.Update();

            // Update childs
            // ------ Map
            _dialogBox?.Update();
        }

        public override void Render()
        {
            base.Render();
            
            _dialogBox.Render();
            _playerPokemonUi.Render();
            _enemyPokemonUi.Render();
            // Render childs
            // ------ Map

        }
    }
}
