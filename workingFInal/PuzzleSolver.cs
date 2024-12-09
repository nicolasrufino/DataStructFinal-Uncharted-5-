// using System;
// using System.Collections.Generic;

// namespace TreasureHuntGame
// {
//     public class PuzzleSolver
//     {
//         public static bool SolvePuzzle(string requiredItem, Hero hero)
//         {
//             if (hero.Inventory.Contains(requiredItem))
//             {
//                 Console.WriteLine($"You used {requiredItem} to solve the puzzle.");
//                 hero.RemoveItem(requiredItem);
//                 return true;
//             }
//             else
//             {
//                 Console.WriteLine($"You need {requiredItem} to solve this puzzle, but it's not in your inventory.");
//                 return false;
//             }
//         }
//     }
// }
