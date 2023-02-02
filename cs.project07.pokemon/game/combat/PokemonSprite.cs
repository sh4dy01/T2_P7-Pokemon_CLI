using cs.project07.pokemon.game.states;
using cs.project07.pokemon.game.states.gui;
using System.Numerics;

namespace cs.project07.pokemon.game.combat
{
    internal class PokemonSprite : Sprite
    {
        private bool _isEnemy;

        public PokemonSprite(bool isEnemy, Vector2 position, ConsoleColor fgColor, ConsoleColor bgColor) : base(position, fgColor, bgColor)
        {
            _isEnemy = isEnemy;
        }

        public override void LoadSprite(string name) 
        {
            if (!IsEmpty()) Clear();

            switch (_isEnemy)
            {
                case false:
                    _sprite = AsciiArtLoader.GetPlayerSpriteByName(name);
                    Width = 40;
                    break;
                case true:
                    _sprite = AsciiArtLoader.GetEnemySpriteByName(name);
                    Width = 35;
                    break;

            }

            Render();
        }
       

        public void InitDefaults()
        {
            
        }
    }
}
