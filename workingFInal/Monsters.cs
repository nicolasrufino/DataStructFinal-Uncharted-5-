// using System;

// namespace TreasureHuntGame
// {
//     public class Monster
//     {
//         public string Name { get; set; }
//         public int Health { get; set; }
//         public int Strength { get; set; }
//         public string Type { get; set; } 

//         public Monster(string name, int health, int strength, string type)
//         {
//             Name = name;
//             Health = health;
//             Strength = strength;
//             Type = type;
//         }

//         public bool IsAlive() => Health > 0;

//         public void TakeDamage(int damage)
//         {
//             Health -= damage;
//             if (Health < 0) Health = 0;
//         }

//         public void DisplayStatus()
//         {
//             Console.WriteLine($"{Name} ({Type}) - Health: {Health}, Strength: {Strength}");
//         }
//     }
// }