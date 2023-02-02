using cs.project07.pokemon.game.combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.project07.pokemon.game.entites
{
    public class Pokemon
    {
        public const int LEVEL_UP_STEP = 25;
        public const int LEVEL_UP_GAINED = 50;

        private readonly PokedexEntry _dex;
        private readonly Stat _stat;

        private bool _isDead;
        private float _currentHealth;
        private int _level;
        private float _experience;
        private float _requiredExperience;

        public PokedexEntry Dex => _dex;
        public Stat Stat => _stat;
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
        
        public Pokemon(PokedexEntry dex)
        {
            _dex = dex;
            _stat = new Stat((dex.Stat.MaxHP, dex.Stat.Attack, dex.Stat.Defense, dex.Stat.SPAttack, dex.Stat.SPDefense, dex.Stat.Speed));
            _currentHealth = _dex.Stat.MaxHP;
            _level = 1;
            _experience = 0;
            _requiredExperience = LEVEL_UP_STEP;
            _isDead = false;
        }

        public virtual void InitEnemyStats()
        {
            _level = PokemonListManager.GetAverageLevel();
            LevelUpStat();
        }

        protected void LevelUpStat()
        {
            for (int i = 1; i < _level; i++)
            {
                _stat.LevelUpStat(_dex.Stat);
                _requiredExperience += LEVEL_UP_STEP;
                _currentHealth += (int)_stat.MaxHP * Stat.LEVEL_UP_STEP;
            }
        }

        public void InitHealth(int amount)
        {
            if (amount < 0) amount = 0;
            
            _currentHealth = amount;
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
            _experience += experience;

            while (_experience >= _requiredExperience)
            {
                _experience -= _requiredExperience;
                _level++;
                _requiredExperience += LEVEL_UP_STEP;
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
