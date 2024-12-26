using System;
using System.Collections.Generic;

namespace TreasureHuntGame
{
    public class Hero
    {
        // attributes
        public int Health { get; set; }
        public int Food { get; set; }
        public int Water { get; set; }
        public int Strength { get; private set; }
        public int Agility { get; private set; }
        public int Intelligence { get; private set; }
        public int PositionX { get; set; }
        public int CurrentRoomNumber { get; set; }
        public int LastTraversalDirection { get; set; }
        public int PositionY { get; set; }
        public List<string> Inventory { get; private set; } = new List<string>();
        public string EquippedItem { get; set; } 
        public bool IsReadyToMove { get; set; } 
        public List<string> Jewels { get; private set; } = new List<string>(); 
        private int jewelsCollected;
        private Timer resourceTimer;

        public Hero()
        {
            
            Health = 100;
            Food = 75;
            Water = 75;


            PositionX = 9;
            PositionY = 5;


            Strength = 5;
            Agility = 5;
            Intelligence = 5;

            
            Inventory.Add("Water");
            Inventory.Add("Knife");
            
            
            EquippedItem = "Knife";

            
            LastTraversalDirection = 1;
            jewelsCollected = 0;
        }
        public void CollectJewel()
        {
            jewelsCollected++;
        }

        public int GetJewelsCount()
        {
            return jewelsCollected;
        }

        public void SetJewelsCount(int count)
    {
        jewelsCollected = count;
    }
       
        private void ResourceDepletion(object state)
        {
            Water = Math.Max(Water - 5, 0);
            Food = Math.Max(Food - 2, 0);

            if (Water <= 10 && Food <= 10)
            {
                TakeDamage(5); 
                Console.WriteLine("You are dehydrated and starving! Your health is diminishing.");
            }
        }


        public bool IsAlive() => Health > 0;


        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }


        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, 100);
        }

       
        public void ConsumeResource(string resource)
        {
            switch (resource)
            {
                case "Food":
                    Food = Math.Min(Food + 20, 70);
                    Console.WriteLine("You consumed food. Food level: " + Food);
                    RemoveItem(resource);
                    break;
                case "Water":
                    Water = Math.Min(Water + 20, 70);
                    Console.WriteLine("You drank water. Water level: " + Water);
                    RemoveItem(resource);
                    break;
                default:
                    Console.WriteLine($"You can't use {resource} directly.");
                    break;
            }
        }

        
        public void UseItem(string item)
        {
            if (Inventory.Contains(item))
            {
                switch (item)
                {
                    case "Health Potion":
                        Heal(20);
                        Console.WriteLine("You used a Health Potion. Health: " + Health);
                        RemoveItem(item);
                        break;
                    case "Food":
                    case "Water":
                        ConsumeResource(item);
                        break;
                    case "Key":
                        Console.WriteLine("You used a Key. It may unlock something.");
                      
                        RemoveItem(item);
                        break;
                    default:
                        Console.WriteLine($"You can't use {item} directly.");
                        break;
                }
            }
            else
            {
                Console.WriteLine($"{item} is not in your inventory.");
            }
        }

   
        public void AddItem(string item)
        {
            if (Inventory.Count >= 5)
            {
                Console.WriteLine("Inventory is full! Removing the oldest item.");
                Inventory.RemoveAt(0);
            }

            Inventory.Add(item);
            Console.WriteLine($"{item} has been added to your inventory.");
        }

       
        public void RemoveItem(string item)
        {
            if (Inventory.Contains(item))
            {
                Inventory.Remove(item);
                Console.WriteLine($"{item} has been removed from your inventory.");
            }
            else
            {
                Console.WriteLine($"{item} is not in your inventory.");
            }
        }

        public void CollectJewel(string jewel)
        {
            Jewels.Add(jewel);
            Console.WriteLine($"You collected a jewel: {jewel}.");
        }
    }
}