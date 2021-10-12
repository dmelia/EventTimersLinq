using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace EventTimersLinq
{
    class Program
    {
        private static Timer _timer;
        private static readonly string[] Names = {
            "bob 1", "bob 2", "bob 3", "bob 4", "bob 5", "bob 6", "bob 7", "bob 8", "bob 9", "bob 10", "bob 11",
            "bob 12", "bob 13", "bob 14", "bob 15"
        };
        private static List<Character> _characters = new List<Character>();
        static void Main(string[] args)
        {
            Initialise();

            ElectPresident(_characters);
            SetTimer();
            Console.Read();
        }

        private static void Initialise()
        {
            Console.WriteLine("###############");
            Console.WriteLine("Initialising...");
            Console.WriteLine("###############");
            foreach (var name in Names)
            {
                _characters.Add(new Character(name));
            }

            
            foreach (var character in _characters)
            {
                List<Character> charactersAlreadySorted = new List<Character>();
                charactersAlreadySorted.AddRange(character.Enemies);
                charactersAlreadySorted.AddRange(character.Friends);
                foreach (Character secondCharacter in _characters
                    .Where(secondCharacter => character.Name != secondCharacter.Name).ToList())
                {
                    if (!character.Friends.Contains(secondCharacter) && !character.Enemies.Contains(secondCharacter))
                    {
                        if (RandomFriendly(secondCharacter))
                        {
                            character.Friends.Add(secondCharacter);
                            secondCharacter.Friends.Add(character);
                        }
                        else
                        {
                            character.Enemies.Add(secondCharacter);
                            secondCharacter.Enemies.Add(character);
                        }
                    }
                }
            }

            foreach (var character in _characters)
            {
                Console.WriteLine(" name : " + character.Name);
                string enemies = "";
                foreach (var enemy in character.Enemies)
                {
                    enemies += " " + enemy.Name;
                }
                Console.WriteLine("enemies : " + enemies);
                
                string friends = "";
                foreach (var enemy in character.Friends)
                {
                    friends += " " + enemy.Name;
                }
                Console.WriteLine("friends : " + friends);
            }
            Console.WriteLine("###############");
            Console.WriteLine("Init done");
            Console.WriteLine("###############");
        }

        private static void SetTimer()
        {
            // Create a timer with a quarter second tick interval.
            _timer = new Timer(250);
            _timer.Elapsed += IncrementTimers;
            _timer.Elapsed += CharacterProcess;
            _timer.Enabled = true;
        }
        private static void CharacterProcess(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Players remaining : " + _characters.Count);
            if (_characters.Count == 0)
            {
                Console.WriteLine("Game finished");
                _timer.Stop();
            }

            foreach (Character character in _characters)
            {
                if (character.IsDead())
                {
                    KillCharacter(character, _characters);
                    foreach (Character otherCharacters in _characters)
                    {
                        otherCharacters.ReactToDeath(character);
                    }

                    if (character.IsPresident)
                    {
                        ElectPresident(_characters);
                    }
                }
            }
        }

        private static void IncrementTimers(Object source, ElapsedEventArgs e)
        {
            foreach (Character character in _characters)
            {
                character.IncrementTick();
            }
        }

        private static bool RandomFriendly(Character character)
        {
            Random random = new Random(character.NameToInt() + (int)DateTime.Now.Ticks);
            int number = random.Next(0, 2);
            return number > 0;
        }

        private static void ElectPresident(List<Character> characters)
        {
            if (characters.Count >= 3)
            {
                Character currentPresident = null;
                int voteCount = 0;
                int enemiesCount = 0;
                foreach (var candidate in characters)
                {
                    if (currentPresident == null)
                    {
                        currentPresident = candidate;
                        voteCount = candidate.Friends.Count;
                        enemiesCount = candidate.Enemies.Count;
                    }
                    else
                    {
                        if (candidate.Friends.Count > voteCount)
                        {
                            currentPresident = candidate;
                            voteCount = candidate.Friends.Count;
                            enemiesCount = candidate.Enemies.Count;
                        }
                        if (candidate.Friends.Count == voteCount && candidate.Enemies.Count < enemiesCount)
                        {
                            currentPresident = candidate;
                            voteCount = candidate.Friends.Count;
                            enemiesCount = candidate.Enemies.Count;
                        }
                    }
                }

                if (currentPresident != null)
                {
                    Console.WriteLine(currentPresident.Name + " was elected as President");
                    currentPresident.IsPresident = true;
                }
                else
                {
                    Console.WriteLine("No president was elected - Not enough participants");
                }
            }
        }

        private static void KillCharacter(Character characterToKill, List<Character> characters)
        {
            Console.WriteLine(characterToKill.Name + " died");
            characters.Remove(characterToKill);
            _characters.Remove(characterToKill);

            foreach (Character character in characters)
            {
                character.ReactToDeath(characterToKill);
                character.Enemies.Remove(characterToKill);
                character.Friends.Remove(characterToKill);
            }
        }
    }
}