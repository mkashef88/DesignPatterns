using System;
using static System.Console;

namespace ChainOfResponsibilitySample
{
    public class Creature
    {
        public int Attack, Defense;
        public string Name;
        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public  class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next;

        public CreatureModifier(Creature _creature)
        {
            creature = _creature ?? throw new ArgumentNullException(paramName: nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }
        public virtual void Handle() => next?.Handle();
    }

    
    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature) { }

        public override void Handle()
        {
            WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }
    public class IncreasedDefenseModifier:CreatureModifier
    {
        public IncreasedDefenseModifier(Creature creature) : base(creature) { }

        public override void Handle()
        {
            WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var goblin = new Creature("Goblin", 2, 2);
            var root = new CreatureModifier(goblin);

            WriteLine("Let's double the goblin's attack");
            root.Add(new DoubleAttackModifier(goblin));

            WriteLine("Let's increase goblin's defense");

            root.Add(new IncreasedDefenseModifier(goblin));


            root.Handle();

            WriteLine(goblin);
        }
    }
}
