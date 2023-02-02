using cs.project07.pokemon.game.combat;
using cs.project07.pokemon.game.save;
using cs.project07.pokemon.game.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    public class Pokemon : ISavable
    {
        public const int LEVEL_UP_STEP = 25;
        public const int LEVEL_UP_GAINED = 50;

        protected PokedexEntry _dex;
        protected Stat _stat;

        private int _id;
        private bool _isDead;
        protected float _currentHealth;
        protected int _level;
        private float _experience;
        protected float _requiredExperience;

        public PokedexEntry Dex => _dex;
        public Stat Stat => _stat;
        public int Id { get => _id; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public string Name { get => _dex.Name; }

        public int Level
        {
            get => _level;
            protected set => _level = value;
        }

        public float Currenthealth { get => _currentHealth; }
        public float Experience { get => _experience; }
        public float RequiredExp { get => _requiredExperience; }

        public virtual ElementType Element { get => _dex.Element; }
        public Attack[] Attacks { get => _dex.Attacks; }

        protected Pokemon()
        {
            
        }
        
        public Pokemon(PokedexEntry dex, int level)
        {
            _dex = dex;
            _stat = new Stat((dex.Stat.MaxHP, dex.Stat.Attack, dex.Stat.Defense, dex.Stat.SPAttack, dex.Stat.SPDefense, dex.Stat.Speed));
            _currentHealth = _dex.Stat.MaxHP;
            _level = level;
            _experience = 0;
            _requiredExperience = LEVEL_UP_STEP;
            _isDead = false;
            InitStat();
        }

        public void Save() {
            SaveManager.PrepareData(
                new Tuple<string, int>("PokemonID"              + Id, _dex.PokedexID),
                new Tuple<string, int>("PokemonMaxHP"           + Id, ((int)Stat.MaxHP)),
                new Tuple<string, int>("PokemonAttack"          + Id, ((int)Stat.Attack)),
                new Tuple<string, int>("PokemonDefense"         + Id, ((int)Stat.Defense)),
                new Tuple<string, int>("PokemonSPAttack"        + Id, ((int)Stat.SPAttack)),
                new Tuple<string, int>("PokemonSPDefense"       + Id, ((int)Stat.SPDefense)),
                new Tuple<string, int>("PokemonSpeed"           + Id, ((int)Stat.Speed)),
                new Tuple<string, int>("PokemonLevel"           + Id, Level),
                new Tuple<string, int>("PokemonCurrentHP"       + Id, ((int)Currenthealth)),
                new Tuple<string, int>("PokemonExperience"      + Id, ((int)Experience)),
                new Tuple<string, int>("PokemonNumberOfAttacks" + Id, Attacks.Length)
                );

            int index = 0;
            foreach (var attack in Attacks)
            {
                SaveManager.PrepareData(
                    new Tuple<string, int>("AttackPP" + Id + index, attack.Usage)
                    );
                index++;
            }
        }
        public void Load() { }
        public void SetId()
        {
            _id = PokemonListManager.GetNextId();
        }

        protected void InitStat()
        {
            for (int i = 1; i < _level; i++)
            {
                LevelUpStat();
            }

            _stat.RoundToInt();
            _currentHealth = _stat.MaxHP;
        }

        public virtual void InitEnemyStats()
        {
            float threshold;
            int avgLevel = PokemonListManager.GetAverageLevel();
            int pkmCount = PokemonListManager.GetPokemonInBattleCount();

            if (pkmCount < 2)
            {
                threshold = avgLevel / 1.5f;
            }
            else threshold = avgLevel;
            
            Random rnd = new Random();
            _level = rnd.Next(1, (int)threshold);
            
            InitStat();
        }

        protected void LevelUpStat()
        {
            _currentHealth += (int)Math.Round(_stat.MaxHP * Stat.LEVEL_UP_STEP);
            _stat.LevelUpStat(_dex.Stat);
            _requiredExperience += LEVEL_UP_STEP;
        }

        public void TakeDamage(float damage)
        {
            if (damage < 0) return;
            
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                _isDead = true;
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0) return;
            if (_currentHealth <= 0) return;

            _currentHealth += amount;
            if (_currentHealth >= _stat.MaxHP) _currentHealth = _stat.MaxHP;
        }

        public void HealMax()
        {
            _currentHealth = _stat.MaxHP;
        }

        public void GainExperience(int experience)
        {
            if (experience <= 0) return;
            
            _experience += experience;

            while (_experience >= _requiredExperience)
            {
                _experience -= _requiredExperience;
                _level++;
                _requiredExperience += LEVEL_UP_STEP;
                _stat.LevelUpStat(_dex.Stat);
            }
        }

        public Attack ChooseRandomAttack()
        {
            Random random = new Random();
            int index = random.Next(0, Attacks.Length);
            return Attacks[index];
        }
        
        public Attack ChooseBestAttack(ElementType defenderType)
        {
            Attack? attack = null;
            float maxDamage = 0;
            bool isAttackSuperEffective = false;

            foreach (Attack a in Attacks)
            {
                if (TypeChart.IsSuperEffective(a.Element, defenderType))
                {
                    if (a.Power > maxDamage)
                    {
                        isAttackSuperEffective = true;
                        attack = a;
                        maxDamage = a.Power;
                    }
                }
                else if (TypeChart.IsEffective(a.Element, defenderType) && !isAttackSuperEffective)
                {
                    if (a.Power > maxDamage)
                    {
                        attack = a;
                        maxDamage = a.Power;
                    }
                }
                else if (TypeChart.IsNotEffective(a.Element, defenderType))
                {
                    continue;
                }
            }
            
            attack ??= ChooseRandomAttack();
            
            return attack;
        }

    }
}
