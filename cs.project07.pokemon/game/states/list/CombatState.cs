using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.gui;
using cs.project07.pokemon.game.combat;
using System.Text;
using System.Numerics;
using cs.project07.pokemon.game.Registry;

namespace cs.project07.pokemon.game.states.list
{
    public class CombatState : State
    {
        private bool _isInit = false;
        
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

        private bool _isPlayerTurn = true;
        private string _effectivenessMessage = "";
        private float _runChance = 50;
        
        private CombatView _currentView;
        private readonly CombatDialogBox _dialogBox;
        private readonly Pokemon _enemyPokemon;
        private readonly PokemonInfoBox _enemyPokemonUi;

        private PokemonSprite _playerSprite;
        private PokemonSprite _enemySprite;

        private Pokemon? _playerPokemon;
        private AttackInfoBox? _attackInfoUi;
        private PokemonInfoBox? _playerPokemonUi;

        public CombatState(Game game) : base(game)
        {
            BackgroundColor = ConsoleColor.White;

            _enemyPokemon = new Pokemon(PokemonRegistry.GetRandomPokemon(), 2);
            _dialogBox = new CombatDialogBox(this);
            _enemyPokemonUi = new PokemonInfoBox(this, _enemyPokemon, true);
            _attackInfoUi = new AttackInfoBox(this);
            _playerSprite = new PokemonSprite(false, new Vector2(25, 27), ForegroundColor, BackgroundColor);
            _enemySprite = new PokemonSprite(true, new Vector2(130, 5), ForegroundColor, BackgroundColor);
            

            Init();
        }

        protected override void Init()
        {
            Name = "Combat";
            _currentView = CombatView.INTRO;
            _enemyPokemon.InitEnemyStats();
            _enemySprite.LoadSprite(_enemyPokemon.Name);
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
                    PokemonListManager.EndCombat();
                    Game.StatesList.Pop();
                    break;
            }
        }

        public void SwitchPokemonFromAction()
        {
            SwitchView(CombatView.SELECT_POKEMON);
            _isPlayerTurn = false;
        }

        public void SwapPlayerPokemon(Pokemon pokemon)
        {
            if (_playerPokemon is not null)
            {
                PokemonListManager.UpdatePokemon(_playerPokemon);
            }

            PokemonListManager.SetActivePokemon(pokemon);
            
            _playerPokemon = pokemon;
            _playerSprite.LoadSprite(pokemon.Name);
            
            _playerPokemonUi = new PokemonInfoBox(this, _playerPokemon, false);
            _playerPokemonUi.Render();
            
            if (_enemyPokemon.Level > _playerPokemon.Level)
                _runChance *= 0.5f;
            else if (_enemyPokemon.Level < _playerPokemon.Level)
                _runChance *= 1.5f;

            SwitchView(_isPlayerTurn ? CombatView.SELECT_ACTION : CombatView.ENEMY_ATTACK);
        }

        private void CheckIfCombatEnd()
        {
            if (_playerPokemon.IsDead)
            {
                _playerPokemon = null;
                _playerPokemonUi.Clear();
                _playerSprite.Clear();

                if (!PokemonListManager.IsAllPokemonDead())
                {
                    SwitchPokemonFromAction();
                }
                else
                {
                    _dialogBox.UpdateText("You have no more pokemon ! You lost !");
                }
            }
            else if (_enemyPokemon.IsDead)
            {
                int oldLevel = _playerPokemon.Level;
                int experience = Pokemon.LEVEL_UP_GAINED * _enemyPokemon.Level;

                _enemySprite.Clear();
                _enemySprite = null;

                _playerPokemonUi.UpdateExperience(experience);
                _playerPokemon.GainExperience(experience);
                PokemonListManager.UpdatePokemon(_playerPokemon);

                if (oldLevel >= _playerPokemon.Level) return;

                StringBuilder sb = new();
                sb.Append("Your ").Append(_playerPokemon.Name).Append(" gained ").Append(_playerPokemon.Level - oldLevel).Append(" level !");
                _dialogBox.UpdateText(sb.ToString());
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
            _enemyPokemon.TakeDamage(GetDamageWithMultiplier(attack, _playerPokemon, _enemyPokemon));
            _enemyPokemonUi.UpdateHealth(_enemyPokemon);
            _isPlayerTurn = false;
            _dialogBox.ResetButtons();
        }
        
        private void DealPlayerDamage()
        {
            Attack attack = _enemyPokemon.ChooseBestAttack(_playerPokemon.Element);
            _dialogBox.UpdateText("The enemy " + _enemyPokemon.Name + " used " + attack.Name + " !");
            _playerPokemon.TakeDamage(GetDamageWithMultiplier(attack, _enemyPokemon, _playerPokemon));
            _playerPokemonUi.UpdateHealth(_playerPokemon);
            _isPlayerTurn = true;
        }

        private int GetDamageWithMultiplier(Attack attack, Pokemon attacker, Pokemon defender)
        {
            float damage = DamageCalculator.DamageWithMultiplier(attack, attacker, defender, out float damageMultiplier, out int critical);

            UpdateEffectivenessMessage(damageMultiplier, critical > 1);

            return (int)damage;
        }


        private void UpdateEffectivenessMessage(float damageMultiplier, bool isCritical)
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

            if (isCritical)
            {
                _effectivenessMessage += " Critical Hit !";
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
                            case CombatView.END_TURN:
                                SwitchView(CombatView.END_COMBAT);
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
                        case CombatView.SELECT_POKEMON:
                            if (_playerPokemon is not null)
                            {
                                SwitchView(CombatView.SELECT_ACTION);
                            }
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
            //Me: Print me all the chars in the _playerSprite



            if (!_isInit)
            {
                PaintBackground();
                _isInit = true;
            }
            
            if (_playerPokemon is not null)
            {
                _playerPokemonUi.Render();
                _attackInfoUi.Render();

                if (!_playerSprite.IsEmpty())
                {
                    //_playerSprite.Render();
                }
            }

            if (_enemySprite is not null)
            {
                _enemySprite.Render();
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
