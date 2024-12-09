using System;
using System.Collections.Generic;

namespace TreasureHuntGame
{
    public class Items
    {
        private Stack<string> itemsStack;
        private List<string> jewelsCollected; 

        public Items()
        {
            itemsStack = new Stack<string>();
            jewelsCollected = new List<string>();
        }

        public void AddItem(string item)
        {
            if (itemsStack.Count >= 5)
            {
                string removedItem = itemsStack.Pop();
                Console.WriteLine($"Inventory is full! Dropping the oldest item: {removedItem}");
            }
            itemsStack.Push(item);
            Console.WriteLine($"Item added: {item}");
        }

        public string UseItem()
        {
            if (itemsStack.Count == 0)
            {
                Console.WriteLine("No items to use!");
                return null;
            }
            string usedItem = itemsStack.Pop();
            Console.WriteLine($"Used item: {usedItem}");
            return usedItem;
        }

        public void DisplayItemsStack()
        {
            if (itemsStack.Count == 0)
            {
                Console.WriteLine("No items in the stack.");
                return;
            }

            Console.WriteLine("Current items in stack:");
            foreach (var item in itemsStack)
            {
                Console.WriteLine($"- {item}");
            }
        }

        public int ItemsCount()
        {
            return itemsStack.Count;
        }

       
        public void CollectJewel(string jewel)
        {
            jewelsCollected.Add(jewel);
            Console.WriteLine($"You have collected the jewel: {jewel}");
        }
        public static void ManageInventory(Hero hero)
        {
            Console.WriteLine("Inventory:");
            for (int i = 0; i < hero.Inventory.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] - {hero.Inventory[i]}");
            }
            for (int i = hero.Inventory.Count; i < 5; i++)
            {
                Console.WriteLine($"[{i + 1}] - Empty");
            }

            Console.WriteLine("Select the item number you want to use:");
            if (int.TryParse(Console.ReadLine(), out int slot) && slot > 0 && slot <= hero.Inventory.Count)
            {
                string itemToUse = hero.Inventory[slot - 1];
                hero.ConsumeResource(itemToUse); 
            }
            else
            {
                Console.WriteLine("Invalid slot selected.");
            }
        }

        public static void UseEquippedItem(Hero hero)
        {
            if (!string.IsNullOrEmpty(hero.EquippedItem))
            {
                Console.WriteLine($"You used {hero.EquippedItem}.");
                hero.ConsumeResource(hero.EquippedItem); 
                hero.RemoveItem(hero.EquippedItem); // remove item
                hero.EquippedItem = null; // clear
            }
            else
            {
                Console.WriteLine("You have no item equipped.");
            }
        }

        public static void AddItemToInventory(Hero hero, string newItem)
        {
            if (hero.Inventory.Count >= 5)
            {
                string removedItem = hero.Inventory[0];
                hero.Inventory.RemoveAt(0);
                Console.WriteLine($"Inventory is full! Dropping the oldest item: {removedItem}");
            }

            hero.Inventory.Add(newItem);
            Console.WriteLine($"{newItem} has been added to your inventory.");
        }
    }
}
