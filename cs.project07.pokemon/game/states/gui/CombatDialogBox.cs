using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.Registry;
using cs.project07.pokemon.game.states.list;
using System.Numerics;
using System.Text;
using cs.project07.pokemon.game.items.list;

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
                SetOffset(ref offsetX, ref offsetY, ref selected, i, 20);

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
                    ForegroundColor = TypeChart.TypeColor[attack.Element],
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
                    ((CombatState)Parent).SwitchView(CombatState.CombatView.SELECT_ITEM);
                }
            };
            _buttons["POKEMON"] = new Button(this, "POKEMON")
            {
                Offsets = new Vector2(10, -1),
                Action = () =>
                {
                    ((CombatState)Parent).SwitchPokemonFromAction();
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
            
            int offsetX = -60;
            int offsetY = -1;
            bool selectedDone = false;
            bool selected = false;

            for (int i = 0; i < pokemons.Length; i++)
            {
                if (pokemons[i] == null) continue;
                
                SetOffset(ref offsetX, ref offsetY, ref selected, i, 25);

                var pokemon = pokemons[i];
                var aFgColor = TypeChart.TypeColor[pokemon.Element];
                var fgColor = TypeChart.TypeColor[pokemon.Element];

                switch (pokemon.IsDead)
                {
                    case true:
                        aFgColor = ConsoleColor.DarkGray;
                        fgColor = aFgColor;
                        break;
                    case false when !selectedDone:
                        selected = true;
                        selectedDone = true;
                        break;
                }

                if (!pokemon.IsDead && PokemonListManager.ActivePokemonIndex >= 0 && pokemon == PokemonListManager.ActivePokemon)
                {
                    fgColor = ConsoleColor.Yellow;
                    aFgColor = ConsoleColor.Yellow;
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("Lv:").Append(pokemon.Level).Append(' ').Append(pokemon.Name).Append(' ').Append(Math.Round((int)pokemon.Currenthealth / pokemon.Stat.MaxHP * 100)).Append('%');

                _buttons[pokemon.Name + i] = new Button(this, sb.ToString())
                {
                    Offsets = new Vector2(offsetX, offsetY),
                    Selected = selected,
                    Action = () =>
                    {
                        if (!pokemon.IsDead && (PokemonListManager.ActivePokemonIndex == -1 || pokemon != PokemonListManager.ActivePokemon))
                        {
                            ((CombatState)Parent).SwapPlayerPokemon(pokemon);
                        }
                    },
                    ForegroundColor = fgColor,
                    ActiveForegroundColor = aFgColor
                };
                
                selected = false;
            }
        }

        internal void InitSelectItemsButtons()
        {
            int offsetX = -25;
            int offsetY = -2;
            bool selected = true;

            int count = 0;
            foreach (var item in InventoryManager.Inventory.Where(item => item.Name != "Spray"))
            {
                _buttons[item.Name] = new Button(this, item.Name + " x" + item.GetQuantity())
                {
                    Offsets = new Vector2(offsetX, offsetY),
                    Selected = selected,
                    Action = () =>
                    {
                        if (item.GetQuantity() > 0)
                        {
                            if (item.GetType() == typeof(Potion))
                            {
                                if (PokemonListManager.ActivePokemon.Currenthealth < PokemonListManager.ActivePokemon.Stat.MaxHP)
                                {
                                    item.Use(PokemonListManager.ActivePokemon);
                                    ((CombatState)Parent)._playerPokemonUi?.UpdateHealth(PokemonListManager.ActivePokemon);
                                }
                            }
                            
                            else if (item.GetType() == typeof(Pokeball))
                            {
                                item.Use();
                                ((CombatState)Parent).TryToCatch(((Pokeball)item).GetMultiplicator());
                                return;
                            }
                            
                            StringBuilder sb = new();
                            sb.Append("You used a ").Append(item.Name).Append(" !");
                            ((CombatState)Parent).UsedAnItem(sb.ToString());
                        }
                        else
                            UpdateText("No more uses left");
                    },
                    ForegroundColor = ConsoleColor.White,
                    ActiveForegroundColor = ConsoleColor.White
                };

                selected = false;
                if (count == 3)
                {
                    offsetY = -3;
                    offsetX = 15;
                }
                offsetY ++;
                count++;
            }
        }

        private static void SetOffset(ref int offsetX, ref int offsetY, ref bool selected, int i, int offsetXIncrement)
        {
            if (i > 0) selected = false;
            if (i %2 == 0)
            {
                offsetX += offsetXIncrement;
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
