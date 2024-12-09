using System;
using System.IO;
using System.Collections.Generic;

namespace TreasureHuntGame
{
    public class Map
    {
        public int[,] MapGrid { get; private set; }
        private bool[,] VisitedRooms;
        private Dictionary<string, string> ItemsInRooms; // stores items 
        private Tree roomTree;
        private string[] levels; // array of level files

        public Map(Tree tree, string[] levels)
        {
            roomTree = tree;
            this.levels = levels;
        }

        public void LoadMapFromFile(int roomNumber)
        {
            if (roomNumber < 1 || roomNumber > levels.Length)
            {
                Console.WriteLine($"Error: Invalid room number {roomNumber}.");
                return;
            }

            string filePath = levels[roomNumber - 1];
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: Map file not found.");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            int rows = lines.Length;
            int cols = lines[0].Length;

            MapGrid = new int[rows, cols];
            VisitedRooms = new bool[rows, cols];
            ItemsInRooms = new Dictionary<string, string>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char symbol = lines[i][j];
                    MapGrid[i, j] = symbol switch
                    {
                        '0' => 0,   // Empty space
                        'D' => 2,   // Door
                        'K' => 3,   // Key
                        'W' => 25,   // Water
                        'T' => 5,   // Trap (invisible to player)
                        '#' => -1,  // Wall
                        'J' => 6,   // Jewel
                        'F' => 7,   // Food
                        '1' => 8,   // Transition to level 1
                        '2' => 9,   // Transition to level 2
                        '3' => 10,  // Transition to level 3
                        '4' => 11,  // Transition to level 4
                        '5' => 12,  // Transition to level 5
                        '6' => 13,  // Transition to level 6
                        '7' => 14,  // Transition to level 7
                        _ => -1     // Unknown symbols default to wall
                    };

                    // Track items in specific rooms (keys, food, jewels, etc.)
                    if (symbol == 'K')
                    {
                        ItemsInRooms[$"{i},{j}"] = "Key";
                    }
                    else if (symbol == 'F')
                    {
                        ItemsInRooms[$"{i},{j}"] = "Food";
                    }
                    else if (symbol == 'J')
                    {
                        ItemsInRooms[$"{i},{j}"] = "Jewel";
                    }
                }
            }
        }

        public void DisplayMap(int heroX, int heroY)
        {
            if (MapGrid == null)
            {
                Console.WriteLine("Error: Map is not loaded.");
                return;
            }

            Console.Clear();
            for (int i = 0; i < MapGrid.GetLength(0); i++)
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++)
                {
                    if (i == heroX && j == heroY)
                    {
                        Console.Write("👤 "); // Hero 
                    }
                    else
                    {
                        Console.Write(MapGrid[i, j] switch
                        {
                            0 => "⬛ ", // empty space
                            -1 => "🟩 ", // wall
                            2 => "🚪 ", // door
                            3 => "🗝️  ", //key
                            25 => "💧 ", // water
                            5 => "⬛ ", // trap invisble ofc
                            6 => "💎 ", // jewel
                            7 => "🍲 ", // food
                            8 => "1️⃣  ", // transition to level 1
                            9 => "2️⃣  ", // transition to level 2
                            10 => "3️⃣  ", // transition to level 3
                            11 => "4️⃣  ", // transition to level 4
                            12 => "5️⃣  ", // transition to level 5
                            13 => "6️⃣  ", // transition to level 6
                            14 => "7️⃣  ", // transition to level 7
                            _ => "⬜ "  // idk
                        });
                    }
                }
                Console.WriteLine();
            }
        }

        public bool IsRoomVisited(int x, int y)
        {
            if (VisitedRooms == null)
            {
                return false;
            }
            return VisitedRooms[x, y];
        }

        public void MarkRoomAsVisited(int x, int y)
        {
            if (VisitedRooms != null)
            {
                VisitedRooms[x, y] = true;
            }
        }

        public void HandleRoomInteraction(int x, int y, Hero hero)
        {
            if (MapGrid == null)
            {
                Console.WriteLine("Error: Map is not loaded.");
                return;
            }

            string roomKey = $"{x},{y}";

            if (ItemsInRooms.ContainsKey(roomKey))
            {
                string item = ItemsInRooms[roomKey];
                if (item == "Jewel")
                {
                    Console.WriteLine("You found a Jewel!");
                    hero.CollectJewel(item);
                }
                else
                {
                    Console.WriteLine($"You found a {item}!");
                    hero.AddItem(item);
                }
                ItemsInRooms.Remove(roomKey); // remove item from room after it's picked up
            }

            if (MapGrid[x, y] == 2) // door
            {
                if (hero.Inventory.Contains("Key"))
                {
                    Console.WriteLine("You used a key to unlock the door.");
                    hero.RemoveItem("Key");
                    hero.IsReadyToMove = true;
                }
                else
                {
                    Console.WriteLine("You need a key to unlock this door.");
                    return;
                }
            }

            if (MapGrid[x, y] >= 8 && MapGrid[x, y] <= 14 && hero.IsReadyToMove) // Room transition marker
            {
                int nextRoom = MapGrid[x, y] - 7; // calculate room number from marker
                Console.WriteLine($"You proceed to room {nextRoom}.");
                hero.CurrentRoomNumber = nextRoom; // update hero's current room number
                LoadMapFromFile(nextRoom); // load new level map
                hero.IsReadyToMove = false; // reset readiness after moving
            }

            if (MapGrid[x, y] == 5) // Trap (invisible)
            {
                Console.WriteLine("You stepped on a trap! You take 10 damage.");
                hero.TakeDamage(10);
            }
        }
    }
}
