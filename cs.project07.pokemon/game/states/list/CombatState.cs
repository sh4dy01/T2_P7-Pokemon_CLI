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
            SELECT_POKEMON,
            SELECT_ACTION,
            SELECT_ATTACK,
            EFFECTIVE,
            ACTION_USE,
            ENEMY_ATTACK,
            END_TURN,
            END_COMBAT
        };

        private bool _isPlayerTurn;
        private string _effectivenessMessage = "";
        private float _runChance = 50;
        
        private CombatView _currentView;
        
        private readonly CombatDialogBox _dialogBox;
        private readonly Pokemon _enemyPokemon;
        private readonly PokemonInfoBox _enemyPokemonUi;
        
        private Pokemon? _playerPokemon;
        private AttackInfoBox? _attackInfoUi;
        private PokemonInfoBox? _playerPokemonUi;

        private readonly PokemonListManager _pokemonListManager; //TODO : REMOVE

        public CombatState(Game game) : base(game)
        {
            _enemyPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon()); //TODO : Get the random pokemon
            _pokemonListManager = new PokemonListManager(); //TODO : REMOVE
            _dialogBox = new CombatDialogBox(this);
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
                case CombatView.SELECT_POKEMON:
                    _dialogBox.ResetText();
                    _dialogBox.InitSelectPokemonsButtons();
                    break;
                case CombatView.SELECT_ACTION:
                    _dialogBox.ResetText();
                    _dialogBox.InitSelectActionButtons();
                    break;
                case CombatView.SELECT_ATTACK:
                    _currentView = CombatView.SELECT_ATTACK;
                    _dialogBox.InitSelectAttackButtons(_playerPokemon.Attacks);
                    _attackInfoUi.Show(_playerPokemon.Attacks[0]);
                    break;
                case CombatView.EFFECTIVE:
                    if (_effectivenessMessage == "")
                    {
                        SwitchView(CombatView.END_TURN);
                        break;
                    }
                    _dialogBox.UpdateText(_effectivenessMessage);
                    break;
                case CombatView.ACTION_USE:
                    //TODO
                    break;
                case CombatView.ENEMY_ATTACK:
                    DealPlayerDamage();
                    break;
                case CombatView.END_TURN:
                    CheckIfCombatEnd();
                    break;
                case CombatView.END_COMBAT:
                    Game.StatesList.Pop();
                    break;
            }
        }

        public void SwapPlayerPokemon(Pokemon pokemon)
        {
            _playerPokemon = pokemon;
            _playerPokemonUi = new PokemonInfoBox(this, _playerPokemon, false);
            _attackInfoUi = new AttackInfoBox(this);
            if (_enemyPokemon.Level > _playerPokemon.Level)
                _runChance *= 0.5f;
            else if (_enemyPokemon.Level < _playerPokemon.Level)
                _runChance *= 1.5f;
            SwitchView(CombatView.SELECT_ACTION);
        }

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
                int experience = 50 * _enemyPokemon.Level;
                _playerPokemon.GainExperience(experience);
                //_playerPokemonUi.UpdateExperience(experience);
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
            _attackInfoUi.Hide();
            _dialogBox.UpdateText("Your " + _playerPokemon.Name + " used " + attack.Name + " !");
            _enemyPokemon.TakeDamage(DamageWithMultiplier(attack, _playerPokemon, _enemyPokemon));
            _enemyPokemonUi.UpdateHealth(_enemyPokemon);
            _isPlayerTurn = false;
            _dialogBox.ResetButtons();
        }
        
        private void DealPlayerDamage()
        {
            Attack attack = _enemyPokemon.ChooseBestAttack(_playerPokemon.Type);
            _dialogBox.UpdateText("The enemy " + _enemyPokemon.Name + " used " + attack.Name + " !");
            _playerPokemon.TakeDamage(DamageWithMultiplier(attack, _enemyPokemon, _playerPokemon));
            _playerPokemonUi.UpdateHealth(_playerPokemon);
            _isPlayerTurn = true;
        }

        private float DamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender)
        {
            float damageMultiplier = TypeChart.GetDamageMultiplier(attack.Type, defender.Type);
            UpdateEffectivenessMessage(damageMultiplier);

            float A, D;
            if (attack.IsPhysicalMove())
            {
                A = attacker.Attack;
                D = defender.Defense;
            } else
            {
                A = attacker.SPAttack;
                D = defender.SPDefense;
            }

            float STAB = 1;
            if (attack.Type == attacker.Type)
                STAB = 1.5f;

            Random rnd = new Random();
            float random = rnd.Next(217, 256) / 255.0f;

            float damage = ((2 * attacker.Level / 5 + 2) * attack.Power * A/D / 50 + 2) * STAB * damageMultiplier * random;
            
            return damage;
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
                    _effectivenessMessage = "";
                    break;
            }
        }

        private void UpdateAttackInfoUi()
        {
            if (_currentView == CombatView.SELECT_ATTACK)
            {
                _attackInfoUi.Show(_playerPokemon.Attacks[_dialogBox.ButtonManager.GetSelectedButtonIndex()]);
            }
        }

        public override void HandleKeyEvent(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.Enter:
                    if (_dialogBox.ButtonManager.Buttons.Count <= 0)
                    {
                        switch (_currentView)
                        {
                            case CombatView.ENEMY_ATTACK:
                                SwitchView(CombatView.EFFECTIVE);
                                break;
                            case CombatView.EFFECTIVE:
                                SwitchView(CombatView.END_TURN);
                                break;
                            default:
                                var NextState = _currentView + 1;
                                SwitchView(NextState);
                                break;
                        }
                    }
                    else Button.ExecuteAction(_dialogBox.ButtonManager.Buttons);
                    break;
                case ConsoleKey.UpArrow:
                    Button.SelectPrevious(_dialogBox.ButtonManager.Buttons);
                    UpdateAttackInfoUi();
                    break;
                case ConsoleKey.DownArrow:
                    Button.SelectNext(_dialogBox.ButtonManager.Buttons);
                    UpdateAttackInfoUi();
                    break;
                case ConsoleKey.Backspace:
                    switch (_currentView)
                    {
                        case CombatView.SELECT_ATTACK:
                            _attackInfoUi.Hide();
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
            if (_playerPokemon is not null)
            {
                _playerPokemonUi.Render();
                _attackInfoUi.Render();
            }

            _enemyPokemonUi.Render();
            // Render childs
            // ------ Map

        }

        public void TryToRun()
        {
            Random rnd = new();
            if (rnd.Next(0, 100) <= _runChance)
            {
                _dialogBox.UpdateText("You ran away !");
                SwitchView(CombatView.END_COMBAT);
            }
            else
            {
                _dialogBox.UpdateText("You failed to run away !");
                _isPlayerTurn = false;
                SwitchView(CombatView.ENEMY_ATTACK);
            }
        }
    }
}
