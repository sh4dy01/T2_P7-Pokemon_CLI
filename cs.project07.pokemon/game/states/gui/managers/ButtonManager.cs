#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace cs.project07.pokemon.game.states.gui.managers
{
    public class ButtonManager
    {
        public int _selectedButtonIndex;
        public Dictionary<string, Button>? Buttons;
        
        public ButtonManager()
        {
            Buttons = new Dictionary<string, Button>();
        }

        public Action<ConsoleKey> HandleKeyEvent { get; set; }

        public void InitHandleKeyEvent()
        {
            HandleKeyEvent = (pressedKey) =>
            {
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        Button.SelectPrevious(Buttons);
                        break;
                    case ConsoleKey.DownArrow:
                        Button.SelectNext(Buttons);
                        break;
                    case ConsoleKey.Enter:
                        Button.ExecuteAction(Buttons);
                        break;
                }
            };
        }

        public int GetSelectedButtonIndex()
        {
            return (int)(Buttons?.Values.ToList().FindIndex(button => button.Selected));
        }

        public void Update()
        {
            if (Buttons?.Count == 0) return;
            foreach (Button button in Buttons.Values)
                button.Update();
        }

        public void Render()
        {
            if (Buttons?.Count == 0) return;
            foreach (Button button in Buttons.Values)
                button.Render();
        }
    }
}
