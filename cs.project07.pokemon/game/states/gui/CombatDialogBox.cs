using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.entites;
using cs.project07.pokemon.game.states.list;
using System.Numerics;

namespace cs.project07.pokemon.game.states.gui
{
    public class CombatDialogBox : DialogBox
    {
               
        public CombatDialogBox(CombatState state) : base(state)
        {
            InitDefaults();
            InitDefaultButtons();
        }

        public void ResetButtons()
        {
            _buttons.Clear();
        }

        public void ResetText()
        {
            Text = "";
        }

        public void InitSelectAttackButtons(Attack[] attacks) 
        {
            int offsetX = -25;
            int offsetY = -1;
            bool selected = true;

            for (int i = 0; i < attacks.Length; i++)
            {
                SetOffset(ref offsetX, ref offsetY, ref selected, i);

                var attack = attacks[i];
                _buttons[attack.Name] = new Button(this, attack.Name)
                {
                    Offsets = new Vector2(offsetX, offsetY),
                    Selected = selected,
                    Action = () =>
                    {
                        if (attack.Usage > 0)
                        {
                            ((CombatState)Parent).DealEnemyDamage(attack);
                            attack.Use();
                        }
                        else
                            UpdateText("No more uses left");
                    },
                    ActiveForegroundColor = TypeChart.TypeColor[attack.Element]
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
                    ((CombatState)Parent).SwitchView(CombatState.CombatView.SELECT_ATTACK);
                }
            };
            _buttons["INVENTORY"] = new Button(this, "INVENTORY")
            {
                Offsets = new Vector2(-10, 1),
                Action = () =>
                {

                }
            };
            _buttons["POKEMON"] = new Button(this, "POKEMON")
            {
                Offsets = new Vector2(10, -1),
                Action = () =>
                {
                    //Switch to select pokemon state
                }
            };
            _buttons["RUN"] = new Button(this, "RUN")
            {
                Offsets = new Vector2(10, 1),
                Action = () =>
                {
                    ((CombatState)Parent).TryToRun();
                }
            };
        }
        public void InitSelectPokemonsButtons()
        {
            var pokemons = PokemonListManager.BattleTeam;
            
            int offsetX = -45;
            int offsetY = -1;
            bool selected = true;

            for (int i = 0; i < pokemons.Length; i++)
            {
                if (pokemons[i] == null || pokemons[i].IsDead) continue;

                SetOffset(ref offsetX, ref offsetY, ref selected, i);

                var pokemon = pokemons[i];
                _buttons[pokemon.Name + i] = new Button(this, "Lv:" + pokemon.Level + " " + pokemon.Name)
                {
                    Offsets = new Vector2(offsetX, offsetY),
                    Selected = selected,
                    Action = () =>
                    {
                        ((CombatState)Parent).SwapPlayerPokemon(pokemon);
                    }
                };
            }
        }

        private static void SetOffset(ref int offsetX, ref int offsetY, ref bool selected, int i)
        {
            if (i > 0) selected = false;
            if (i %2 == 0)
            {
                offsetX += 20;
                offsetY = -1;
            }

            if (i % 2 == 1) { offsetY = -offsetY; }
        }

        public override void Render()
        {
            base.Render();

            _buttonManager.Render();
        }
    }
}
