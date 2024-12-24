// using System;
// using TreasureHuntGame;

// namespace TreasureHuntGame
// {
//     public class Combat
//     {
//         public static void StartCombat(Hero hero, Monster monster)
//         {
//             Console.WriteLine($"A wild {monster.Name} appears!");
//             monster.DisplayStatus();
//             while (hero.IsAlive() && monster.IsAlive())
//             {
//                 Console.WriteLine("Choose your action: (A)ttack or (R)etreat");
//                 char action = Console.ReadKey(true).KeyChar;

//                 if (action == 'A' || action == 'a')
//                 {
//                     Attack(hero, monster);
//                     if (monster.IsAlive())
//                     {
//                         MonsterAttack(monster, hero);
//                     }
//                 }
//                 else if (action == 'R' || action == 'r')
//                 {
//                     Console.WriteLine("You retreat from the battle.");
//                     break;
//                 }
//                 else
//                 {
//                     Console.WriteLine("Invalid action. Please choose (A)ttack or (R)etreat.");
//                 }
//             }

//             if (!hero.IsAlive())
//             {
//                 Console.WriteLine($"You have been defeated by the {monster.Name}.");
//             }
//             else if (!monster.IsAlive())
//             {
//                 Console.WriteLine($"You have defeated the {monster.Name}!");
//             }
//         }

//         private static void Attack(Hero hero, Monster monster)
//         {
//             int damage = hero.Strength;
//             if (hero.EquippedItem == "Knife")
//             {
//                 damage += 5;
//             }
//             else if (hero.EquippedItem == "Gun")
//             {
//                 damage += 10;
//             }

//             Console.WriteLine($"You attack the {monster.Name} with your {hero.EquippedItem}, dealing {damage} damage!");
//             monster.TakeDamage(damage);
//             monster.DisplayStatus();
//         }

//         private static void MonsterAttack(Monster monster, Hero hero)
//         {
//             int damage = monster.Strength;
//             Console.WriteLine($"The {monster.Name} attacks you, dealing {damage} damage!");
//             hero.TakeDamage(damage);
//             Console.WriteLine($"Your health is now {hero.Health}.");
//         }
//     }
// }
