using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplifiedLayoutBuilderSpace
{
    public class SimplifiedLayoutReturnPacket
    {
        private SimpleRoom_Output[,] simplifiedRoomArray;
        private byte[] playerStartingLocation = new byte[2];

        private byte areaWidth;
        private byte areaHeight;

        private byte[,] keyLocations;
        private bool[] hasKey;
        private byte[] finalTreasureLocation;

        private byte[,] doorLocations;
        private byte[] doorSides;
        private byte[] doorColors;

        private short numOfRooms;
        private short numOfLockedDoors;


        public SimplifiedLayoutReturnPacket(SimpleRoom_Output[,] simplifiedRoomArray, byte[] playerStartingLocation, 
                                            byte[,] keyLocations, bool[] hasKey, byte[] finalTreasureLocation,
                                            byte[,] doorLocations, byte[] doorSides, byte[] doorColors,
                                            short numOfRooms, short numOfLockedDoors)
        {
            this.simplifiedRoomArray = simplifiedRoomArray;
            areaWidth = (byte)simplifiedRoomArray.GetLength(0);
            areaHeight = (byte)simplifiedRoomArray.GetLength(1);

            this.playerStartingLocation = playerStartingLocation;
            this.keyLocations = keyLocations;
            this.hasKey = hasKey;
            this.finalTreasureLocation = finalTreasureLocation;
            this.doorLocations = doorLocations;
            this.doorSides = doorSides;
            this.doorColors = doorColors;
            this.numOfRooms = numOfRooms;
            this.numOfLockedDoors = numOfLockedDoors;
        }


        public SimpleRoom_Output[,] getSimplifiedRoomArray()
        {
            return simplifiedRoomArray;
        }

        public byte getAreaWidth()
        {
            return areaWidth;
        }

        public byte getAreaHeight()
        {
            return areaHeight;
        }

        public byte[] getPlayerStartingLocation()
        {
            return playerStartingLocation;
        }

        public byte[,] getKeyLocations()
        {
            return keyLocations;
        }

        public bool[] getHasKey()
        {
            return hasKey;
        }

        public byte[] getFinalTreasureLocation()
        {
            return finalTreasureLocation;
        }

        public byte[,] getDoorLocations()
        {
            return doorLocations;
        }

        public byte[] getDoorSides()
        {
            return doorSides;
        }

        public byte[] getDoorColors()
        {
            return doorColors;
        }

        public short getNumOfRooms()
        {
            return numOfRooms;
        }

        public short getNumOfLockedDoors()
        {
            return numOfLockedDoors;
        }

    }
}
