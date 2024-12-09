using System;
using System.Collections.Generic;

namespace TreasureHuntGame
{
    public class Tree
    {
        public class RoomNode
        {
            public int RoomNumber { get; set; }
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsReadyToMove { get; set; }
            public RoomNode Left { get; set; }
            public RoomNode Right { get; set; }

            public RoomNode(int roomNumber, string description)
            {
                RoomNumber = roomNumber;
                Description = description;
                IsCompleted = false;
                IsReadyToMove = false;
                Left = null;
                Right = null;
            }
        }

        public RoomNode Root { get; private set; }

        public Tree()
        {
            Root = null;
            InitializeTree();
        }

        
        private void InitializeTree()
        {
           
            Root = new RoomNode(1, "Starting Room");
            Root.Right = new RoomNode(3, "Mid Room");
            Root.Right.Left = new RoomNode(2, "Left Room");
            Root.Right.Right = new RoomNode(4, "Central Room");
            Root.Right.Right.Right = new RoomNode(6, "Right Room");
            Root.Right.Right.Right.Left = new RoomNode(5, "Right-Center Room");
            Root.Right.Right.Right.Right = new RoomNode(7, "Final Room");
        }

        public void AddRoom(int roomNumber, string description)
        {
            Root = AddRoomRecursive(Root, roomNumber, description);
        }

        private RoomNode AddRoomRecursive(RoomNode current, int roomNumber, string description)
        {
            if (current == null)
            {
                return new RoomNode(roomNumber, description);
            }

            if (roomNumber < current.RoomNumber)
            {
                current.Left = AddRoomRecursive(current.Left, roomNumber, description);
            }
            else if (roomNumber > current.RoomNumber)
            {
                current.Right = AddRoomRecursive(current.Right, roomNumber, description);
            }

            return current;
        }

        public RoomNode FindRoom(int roomNumber)
        {
            return FindRoomRecursive(Root, roomNumber);
        }

        private RoomNode FindRoomRecursive(RoomNode current, int roomNumber)
        {
            if (current == null || current.RoomNumber == roomNumber)
            {
                return current;
            }

            return roomNumber < current.RoomNumber
                ? FindRoomRecursive(current.Left, roomNumber)
                : FindRoomRecursive(current.Right, roomNumber);
        }

        public void MarkRoomAsCompleted(int roomNumber)
        {
            RoomNode room = FindRoom(roomNumber);
            if (room != null)
            {
                room.IsCompleted = true;
                Console.WriteLine($"Room {roomNumber} marked as completed.");
            }
        }

        public void SetRoomReadyToMove(int roomNumber)
        {
            RoomNode room = FindRoom(roomNumber);
            if (room != null)
            {
                room.IsReadyToMove = true;
                Console.WriteLine($"Room {roomNumber} is now ready for traversal.");
            }
        }

        public RoomNode GetNextRoomNode(int currentRoomNumber)
        {
            var currentRoom = FindRoom(currentRoomNumber);
            if (currentRoom == null)
            {
                return null;
            }

            // move to the next node in the in-order traversal sequence if the room is ready to move
            if (currentRoom.Right != null && currentRoom.Right.IsReadyToMove && !currentRoom.Right.IsCompleted)
            {
                return currentRoom.Right;
            }
            else if (currentRoom.Left != null && currentRoom.Left.IsReadyToMove && !currentRoom.Left.IsCompleted)
            {
                return currentRoom.Left;
            }

            return null;
        }

        private void DisplayTreeRecursive(RoomNode node)
        {
            if (node == null) return;

            DisplayTreeRecursive(node.Left);
            Console.WriteLine($"Room {node.RoomNumber}: {node.Description}, Completed: {node.IsCompleted}, Ready to Move: {node.IsReadyToMove}");
            DisplayTreeRecursive(node.Right);
        }
    }
}
