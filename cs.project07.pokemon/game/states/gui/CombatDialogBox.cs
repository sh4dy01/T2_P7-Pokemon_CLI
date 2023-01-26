using cs.project07.pokemon.game.states.gui.managers;
using cs.project07.pokemon.game.states.list;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.states.gui
{
    internal class CombatDialogBox : DialogBox
    {
        public enum CombatButtonState
        {
            SELECT_ACTION,
            SELECT_ATTACK,
            ACTION_USE,
            ACTION_PET,
        };
        
        private CombatButtonState _currentState;
        
        public CombatDialogBox(CombatState state) : base(state)
        {
            InitDefaults();
            InitDefaultButtons();
            _currentState = CombatButtonState.SELECT_ACTION;
        }

        public void SwitchState(CombatButtonState state)
        {
            ResetButtons();
            _currentState = state;
            
            switch (_currentState)
            {
                case CombatButtonState.SELECT_ACTION:
                    InitSelectActionButtons();
                    break;
                case CombatButtonState.SELECT_ATTACK:
                    InitSelectAttackButtons(((CombatState)Parent).PlayerPokemon);
                    break;
                case CombatButtonState.ACTION_USE:
                    break;
                case CombatButtonState.ACTION_PET:
                    break;
                default:
                    break;
            }
        }

        private void ResetButtons()
        {
            _buttons.Clear();
            Text = "";
        }
        
        public void InitSelectAttackButtons(PokedexEntry pokemon) {
            int i = 1;
            foreach (var attack in pokemon.Attacks)
            {
                _buttons[attack.Name] = new Button(this, attack.Name)
                {
                    Offsets = new Vector2(-10, -1),
                    Selected = true,
                    Action = () =>
                    {

                    }
                };
            }
        }

        public void InitSelectActionButtons()
        {
            _buttons["FIGHT"] = new Button(this, "ATTACK")
            {
                Offsets = new Vector2(-10, -1),
                Selected = true,
                Action = () =>
                {
                    SwitchState(CombatButtonState.SELECT_ATTACK);
                }
            };
            _buttons["POKEMON"] = new Button(this, "POKEMON")
            {
                Offsets = new Vector2(10, -1),
                Action = () =>
                {

                }
            };
            _buttons["INVENTORY"] = new Button(this, "INVENTORY")
            {
                Offsets = new Vector2(-10, 2),
                Action = () =>
                {

                }
            };
            _buttons["RUN"] = new Button(this, "RUN")
            {
                Offsets = new Vector2(10, 2),
                Action = () =>
                {

                }
            };
        }

        public override void Render()
        {
            base.Render();

            _buttonManager.Render();
        }
    }
}
