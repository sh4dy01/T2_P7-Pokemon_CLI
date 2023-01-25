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
        
        public void InitSelectActionButtons() {
            _buttons["FIGHT"] = new Button(this, "Play")
            {
                Offsets = new Vector2(0, 2),
                Selected = true,
                Action = () =>
                {
                    
                }
            };
            _buttons["POKEMON"] = new Button(this, "Pokemon")
            {
                Offsets = new Vector2(0, 2),
                Action = () =>
                {

                }
            };
            _buttons["INVENTORY"] = new Button(this, "Inventory")
            {
                Offsets = new Vector2(3, 0),
                Action = () =>
                {
                    Game.StatesList?.Pop();
                }
            };
            _buttons["RUN"] = new Button(this, "Run")
            {
                Offsets = new Vector2(3, 2),
                Action = () =>
                {
                    Game.StatesList?.Pop();
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
