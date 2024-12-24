using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace TreasureHuntGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("=====================================================");
            Console.WriteLine("     UNCHARTED: BLACKBEARD'S FINAL DESTINATION");
            Console.WriteLine("=====================================================");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Continue Game");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();
      //!!
            // int parsedChoice = int.TryParse(choice());

            Hero hero = new Hero();
            Tree roomTree = new Tree();
            InitializeTree(roomTree);
            string levelsFolder = Path.Combine(Directory.GetCurrentDirectory(), "levels");
            string[] levels = {
                Path.Combine(levelsFolder, "level1.txt"),
                Path.Combine(levelsFolder, "level2.txt"),
                Path.Combine(levelsFolder, "level3.txt"),
                Path.Combine(levelsFolder, "level4.txt"),
                Path.Combine(levelsFolder, "level5.txt"),
                Path.Combine(levelsFolder, "level6.txt"),
                Path.Combine(levelsFolder, "level7.txt")
            };

            Map map = new Map(roomTree, levels);
            Items items = new Items();

            if (!Directory.Exists(levelsFolder) || !ValidateLevelFiles(levels))
            {
                Console.WriteLine("Error: Missing 'levels' folder or level files.");
                return;
            }

            int currentLevel = 1;
            if (choice == "2" && File.Exists("game_save.txt"))
            {
                (hero, map, items, currentLevel) = LoadGame(levels);
                Console.WriteLine("Game loaded successfully!");
                Thread.Sleep(2000);
            }
            else if (choice == "1" && File.Exists("game_save.txt"))
            {
                map.LoadMapFromFile(currentLevel);
                Console.WriteLine("Starting a new game...");
                Thread.Sleep(2000);
                ShowIntro();
            }
            else
            {
                map.LoadMapFromFile(currentLevel);
                Console.WriteLine("It seems that your input was incorrect. A new game will begin.");
                Thread.Sleep(2000);
                ShowIntro();
            }

            bool gameRunning = true;
            int hungerTimer = 0;

            do
            {
                Console.Clear();
                map.DisplayMap(hero.PositionX, hero.PositionY);
                Console.WriteLine($"Health: {hero.Health}, Food: {hero.Food}, Water: {hero.Water}");
                Console.WriteLine($"The hero's coordinates are: {hero.PositionX}, {hero.PositionY}");
                Console.WriteLine("Inventory:");
                for (int i = 0; i < hero.Inventory.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] - {hero.Inventory[i]}");
                }
                for (int i = hero.Inventory.Count; i < 5; i++)
                {
                    Console.WriteLine($"[{i + 1}] - Empty");
                }

                int currentRoom = map.MapGrid[hero.PositionX, hero.PositionY];
                map.HandleRoomInteraction(hero.PositionX, hero.PositionY, hero);

                if (hero.IsReadyToMove)
                {
                    var nextRoomNode = roomTree.GetNextRoomNode(hero.CurrentRoomNumber);
                    if (nextRoomNode != null)
                    {
                        Console.WriteLine($"You proceed to room {nextRoomNode.RoomNumber}.");
                        hero.CurrentRoomNumber = nextRoomNode.RoomNumber;
                        currentLevel = hero.CurrentRoomNumber; // update current level
                        map.LoadMapFromFile(currentLevel);
                        hero.IsReadyToMove = false; // reset move readiness after moving to the next room
                        Thread.Sleep(2000);
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("There are no available paths to proceed. Complete the current tasks to proceed further.");
                    }
                }

                hungerTimer++;
                if (hungerTimer >= 20) // every 20 ticks, decrease food and water
                {
                    hero.Food -= 2;
                    hero.Water -= 5;
                    if (hero.Food <= 0 && hero.Water <= 0)
                    {
                        hero.TakeDamage(5); // lose health if starving or dehydrated
                        Console.WriteLine("You are starving or dehydrated! Find food and water quickly.");
                    }
                    hungerTimer = 0;
                }

                Console.WriteLine("Move using W (up), A (left), S (down), D (right). Press Q to quit, or press TAB to use an item from the inventory.");
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Q)
                {
                    SaveGame(hero, map, items, currentLevel);
                    Console.WriteLine("Game saved! Goodbye!");
                    break;
                }

                if (key == ConsoleKey.Tab)
                {
                    Console.WriteLine("Select the item number you want to use:");
                    if (int.TryParse(Console.ReadLine(), out int slot) && slot > 0 && slot <= hero.Inventory.Count)
                    {
                        string itemToUse = hero.Inventory[slot - 1];
                        hero.UseItem(itemToUse);
                    }
                    else
                    {
                        Console.WriteLine("Invalid slot selected.");
                    }
                    Thread.Sleep(2000);
                    continue;
                }

                int newX = hero.PositionX;
                int newY = hero.PositionY;

                switch (key)
                {
                    case ConsoleKey.W: newX--; hero.LastTraversalDirection = 1; break;
                    case ConsoleKey.A: newY--; hero.LastTraversalDirection = 1; break;
                    case ConsoleKey.S: newX++; hero.LastTraversalDirection = 2; break;
                    case ConsoleKey.D: newY++; hero.LastTraversalDirection = 2; break;
                    default: Console.WriteLine("Invalid input! Use W, A, S, or D to move."); continue;
                }

                if (!IsValidMove(map, newX, newY))
                {
                    Console.WriteLine("You can't move there. It's either a wall or out of bounds!");
                }
                else
                {
                    hero.PositionX = newX;
                    hero.PositionY = newY;
                }

                if (!hero.IsAlive())
                {
                    Console.WriteLine("You have perished. Game Over.");
                    gameRunning = false;
                }

            } while (gameRunning);

            Console.WriteLine("Thanks for playing!");
        }

        static void InitializeTree(Tree roomTree)
        {
            roomTree.AddRoom(1, "Starting Room");
            roomTree.AddRoom(3, "Left Branch Room");
            roomTree.AddRoom(2, "Right Branch Room");
            roomTree.AddRoom(4, "Mid-Left Room");
            roomTree.AddRoom(6, "Far Right Room");
            roomTree.AddRoom(5, "Mid-Right Room");
            roomTree.AddRoom(7, "Final Room");
        }

        static bool IsValidMove(Map map, int x, int y)
        {
            return x >= 0 && x < map.MapGrid.GetLength(0) &&
                   y >= 0 && y < map.MapGrid.GetLength(1) &&
                   map.MapGrid[x, y] != -1; // make sure not a wall
        }

        static void ShowIntro()
        {
            Console.Clear();
            TypewriterEffect("You are Nathan Drake, a legendary treasure hunter.", 50);
            Thread.Sleep(1000);
            TypewriterEffect("Your mission: Discover Blackbeard’s legendary treasure.", 50);
            Thread.Sleep(1000);
            TypewriterEffect("But beware: traps, puzzles, and rival treasure hunters stand in your way.", 50);
            Thread.Sleep(1000);
            TypewriterEffect("Use your skills: Strength for combat, Agility for traps, and Intelligence for puzzles.", 50);
            Thread.Sleep(1000);
            TypewriterEffect("Press 'S' to continue...", 50);

            if (Console.ReadKey(true).Key == ConsoleKey.S) return;
        }

        static void SaveGame(Hero hero, Map map, Items items, int level)
        {
            using (StreamWriter writer = new StreamWriter("game_save.txt"))
            {
                writer.WriteLine(level);
                writer.WriteLine(hero.PositionX);
                writer.WriteLine(hero.PositionY);
                writer.WriteLine(hero.Health);
                writer.WriteLine(hero.Food);
                writer.WriteLine(hero.Water);
                writer.WriteLine(hero.CurrentRoomNumber);
                writer.WriteLine(hero.GetJewelsCount());
                writer.WriteLine(string.Join(",", hero.Inventory.ToArray()));
            }
        }

        static (Hero, Map, Items, int) LoadGame(string[] levels)
        {
            string[] saveData = File.ReadAllLines("game_save.txt");
            int level = int.Parse(saveData[0]);
            int row = int.Parse(saveData[1]);
            int col = int.Parse(saveData[2]);
            int health = int.Parse(saveData[3]);
            int food = int.Parse(saveData[4]);
            int water = int.Parse(saveData[5]);
            int currentRoomNumber = int.Parse(saveData[6]);
            int jewelsCount = int.Parse(saveData[7]);
            string[] inventory = saveData[8].Split(',');

            Hero hero = new Hero
            {
                PositionX = row,
                PositionY = col,
                Health = health,
                Food = food,
                Water = water,
                CurrentRoomNumber = currentRoomNumber
            };
            hero.SetJewelsCount(jewelsCount);
            foreach (var item in inventory)
            {
                hero.AddItem(item);
            }

            Tree roomTree = new Tree();
            InitializeTree(roomTree);
            Map map = new Map(roomTree, levels);
            map.LoadMapFromFile(level);

            Items items = new Items();

            return (hero, map, items, level);
        }

        static bool ValidateLevelFiles(string[] levels)
        {
            foreach (string levelPath in levels)
            {
                if (!File.Exists(levelPath)) return false;
            }
            return true;
        }

        static void TypewriterEffect(string text, int delay = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}