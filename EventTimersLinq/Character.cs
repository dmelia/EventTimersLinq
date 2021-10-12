using System;
using System.Collections.Generic;
using System.Linq;

namespace EventTimersLinq
{
    internal class Character
    {
        private int _currentTick;
        
        public bool IsPresident
        {
            get;
            set;
        }
        
        public string Name
        {
            get;
            set;
        }

        public List<Character> Friends
        {
            get;
            set;
        }

        public List<Character> Enemies
        {
            get;
            set;
        }

        public int LifeDuration
        {
            get;
            set;
        }

        public Character(string name)
        {
            Name = name;
            var rand = new Random(NameToInt() + (int)DateTime.Now.Millisecond);
            LifeDuration = rand.Next(1, 100);
            IsPresident = false;
            _currentTick = 0;
            Enemies = new List<Character>();
            Friends = new List<Character>();
        }

        public void IncrementTick()
        {
            _currentTick++;
        }

        public bool IsDead()
        {
            return _currentTick > LifeDuration;
        }

        public int NameToInt()
        {
            return Name.ToCharArray().Sum(c => (int) Char.GetNumericValue(c));
        }

        public void ReactToDeath(Character characterThatDied)
        {
            {
                if (Friends.Exists(x => x.Name == characterThatDied.Name))
                {
                    Friends.Remove(characterThatDied);
                    if (characterThatDied.IsPresident)
                    {
                        Console.WriteLine(Name + " honours the death of " + characterThatDied.Name + ", who was the last President");
                    }
                    else
                    {
                        Console.WriteLine(Name + " mourns the death of " + characterThatDied.Name);
                    }
                }

                if (Enemies.Exists(x => x.Name == characterThatDied.Name))
                {
                    Enemies.Remove(characterThatDied);
                    if (characterThatDied.IsPresident)
                    {
                        Console.WriteLine(Name + " thinks " + characterThatDied.Name + " was a bad President");
                    }
                    else
                    {
                        Console.WriteLine(Name + " rejoices for the death of " + characterThatDied.Name);
                    }
                }
            }
        }

        protected bool Equals(Character other)
        {
            return Name == other.Name && LifeDuration == other.LifeDuration;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Character) obj);
        }
    }
}