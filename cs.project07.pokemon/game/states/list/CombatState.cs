using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.Registry;
using cs.project07.pokemon.game.states.gui;
using System.Numerics;
using System.Text;

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
            SELECT_ITEM,
            SELECT_ATTACK,
            EFFECTIVE,
            ACTION_USE,
            ENEMY_ATTACK,
            END_TURN,
            END_COMBAT
        };

        private bool _isPlayerTurn = true;
        private string _effectivenessMessage = "";
        
        private float _runChance = 33;

        private CombatView _currentView;
        private readonly CombatDialogBox _dialogBox;
        private readonly Pokemon _enemyPokemon;
        private readonly PokemonInfoBox _enemyPokemonUi;

        private readonly PokemonSprite _playerSprite;
        private PokemonSprite _enemySprite;

        public Pokemon? _playerPokemon;
        private readonly AttackInfoBox? _attackInfoUi;
        public PokemonInfoBox? _playerPokemonUi;

        public CombatState(Game game, bool isBoss) : base(game)
        {
            Console.Clear();

            BackgroundColor = ConsoleColor.White;

            _enemyPokemon = isBoss ? new BossPokemon(PokemonRegistry.GetPokemonByPokedexId(493)) : // FIGHT ARCEUS
                new Pokemon(PokemonRegistry.GetRandomPokemon(), 2); // Fight pokemon
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
            if (_enemyPokemon.GetType() != typeof(BossPokemon))
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
                case CombatView.SELECT_ITEM:
                    _dialogBox.ResetText();
                    _dialogBox.InitSelectItemsButtons();
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

        public void UsedAnItem(string message)
        {
            _isPlayerTurn = false;
            SwitchView(CombatView.ACTION_USE);
            _dialogBox.UpdateText(message);
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
                _runChance = 25;
            else if (_enemyPokemon.Level < _playerPokemon.Level)
                _runChance = 70;

            SwitchView(_isPlayerTurn ? CombatView.SELECT_ACTION : CombatView.ENEMY_ATTACK);
        }

        private void CheckIfCombatEnd()
        {
            if (_playerPokemon.IsDead)
            {
                _playerPokemon = null;
                _playerPokemonUi?.Clear();
                _playerSprite.Clear();

                if (!PokemonListManager.IsAllPokemonDead())
                    SwitchPokemonFromAction();
                else
                {
                    _dialogBox.UpdateText("You have no more pokemon ! You lost !");
                    Game.StatesList.Pop(); //Return to map
                    Game.StatesList.Pop(); //Return to main menu
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
            if (_currentView != CombatView.SELECT_ATTACK) return;
            if (_dialogBox.ButtonManager.GetSelectedButtonIndex() < _playerPokemon.Attacks.Length)
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
                            case CombatView.ACTION_USE:
                                SwitchView(CombatView.END_TURN);
                                break;
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
                        case CombatView.SELECT_ATTACK or CombatView.SELECT_ITEM:
                            _attackInfoUi?.Hide();
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

            if (!_isInit)
            {
                PaintBackground();
                _isInit = true;
            }
            
            _enemySprite?.Render();
            _enemyPokemonUi.Render();

            if (_playerPokemon is null) return;
            _playerPokemonUi?.Render();
            _attackInfoUi?.Render();
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

        public void TryToCatch(float multiplicator)
        {
            Random rnd = new();
            
            int n = 0;
            int ballUsed = 8; // All pokeball except Hyperball

            switch (multiplicator)
            {
                case 1 :
                    n = 255;
                    break;
                case 1.5f:
                    n = 200;
                    break;
                case 2:
                    n = 150;
                    ballUsed = 12;
                    break;
                case 255:
                    CatchPokemon();
                    return;
                default:
                    n = 255;
                    break;
            }

            n = rnd.Next(0, n);

            if (n > _enemyPokemon.Dex.CatchRate*0.75f) 
            {
                //Second try
                rnd = new();
                int m = rnd.Next(0, 255);

                float f = _enemyPokemon.Stat.MaxHP * 255 * 4 / (_enemyPokemon.Currenthealth * ballUsed); //Official algorithm
                if (f < 1) f = 1;
                else if (f > 255) f = 255;

                if (f >= m) 
                    CatchPokemon();
                else
                {
                    _dialogBox.UpdateText("You failed to catch the pokemon !");
                    _isPlayerTurn = false;
                    SwitchView(CombatView.ENEMY_ATTACK);
                }
            }
            else
            {
                CatchPokemon();
            }
        }

        private void CatchPokemon()
        {
            _dialogBox.UpdateText("You caught the pokemon !");
            PokemonListManager.AddPokemon(_enemyPokemon);
            SwitchView(CombatView.END_COMBAT);
        }
    }
}
