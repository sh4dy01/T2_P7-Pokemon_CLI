

namespace cs.project07.pokemon
{
    public class Attack
    {
        static ElementType[] _specialTypes = new[]
        {   
            ElementType.DARK,
            ElementType.ELECTRIC,
            ElementType.FIRE,
            ElementType.WATER,
            ElementType.GRASS,
            ElementType.PSYCHIC,
        };
        
        
        private readonly string _name;
        private int _power;
        private readonly ElementType _type;
        private int _maxUsage;
        private int _currentUsage;

        public string Name => _name;
        public int Power { get => _power; set => _power = value; }
        public ElementType Element { get => _type; }
        public int Usage { get => _currentUsage; }

        public Attack(string name, int damage, ElementType type, int maxUsage = 20)
        {
            _name = name;
            _power = damage;
            _type = type;
            _maxUsage = maxUsage;
            _currentUsage = _maxUsage;
        }

        public void SetPP(int pp) => _currentUsage = pp;
        public void SetPPToMax() => _currentUsage = _maxUsage;


        public void Use()
        {
            if (_currentUsage > 0)
            {
                _currentUsage--;
            }
        }

        public bool IsSpecialMove() => _specialTypes.Contains(Element);
        public bool IsPhysicalMove() => !IsSpecialMove();
        
    }
}
