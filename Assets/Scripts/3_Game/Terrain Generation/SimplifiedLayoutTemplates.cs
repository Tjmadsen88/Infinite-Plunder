using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace SimplifiedLayoutBuilderSpace
{
    public static class SimplifiedLayoutTemplates
    {
        public static SimplifiedLayoutReturnPacket generateLayout_TitleScreen()
        {
            SimpleRoom_Output[,] simplifiedRoomArray = SimplifiedLayoutBuilder.instantiateRooms_Output(3, 4);
            bool[] doorIsOpen;
            bool[] cornerIsOpen;

            // Doors are in L, U, D, R order:
            // Corners in UL, UR, DL, DR order:

            // farthest row:
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Second row:         
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Third row:         
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Fourth row:         
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            

            byte[] playerPos = {1, 3};
            // byte[,] keyLocations = null;
            bool[] hasKey = { false, false, false, false, false, false };
            // byte[] finalTreasureLocation = null;
            // byte[,] doorLocations = null;
            // byte[] doorSides = null;
            // byte[] doorIDs = null;
            short numOfRooms = 4;
            short numOfLockedDoors = 0;
            return new SimplifiedLayoutReturnPacket(simplifiedRoomArray, playerPos, null, hasKey, null, null, null, null, numOfRooms, numOfLockedDoors);
        }

        
        public static SimplifiedLayoutReturnPacket generateLayout_ShipSelectionScreen()
        {
            SimpleRoom_Output[,] simplifiedRoomArray = SimplifiedLayoutBuilder.instantiateRooms_Output(4, 5);
            bool[] doorIsOpen;
            bool[] cornerIsOpen;

            // Doors are in L, U, D, R order:
            // Corners in UL, UR, DL, DR order:

            // first row:
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[1,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[2,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Second row:         
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Third row:         
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{true, true, false, false};
            // cornerIsOpen = new bool[]{true, false, false, false};
            // simplifiedRoomArray[2,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // fourth row:
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, true, false, true};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[2,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{true, false, false, true};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // fifth row:
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[1,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[2,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            

            byte[] playerPos = {1, 1};
            // byte[,] keyLocations = null;
            bool[] hasKey = { false, false, false, false, false, false };
            // byte[] finalTreasureLocation = null;
            // byte[,] doorLocations = null;
            // byte[] doorSides = null;
            // byte[] doorIDs = null;
            short numOfRooms = 4;
            short numOfLockedDoors = 0;
            return new SimplifiedLayoutReturnPacket(simplifiedRoomArray, playerPos, null, hasKey, null, null, null, null, numOfRooms, numOfLockedDoors);
        }


        public static SimplifiedLayoutReturnPacket generateLayout_Demo()
        {
            SimpleRoom_Output[,] simplifiedRoomArray = SimplifiedLayoutBuilder.instantiateRooms_Output(7, 4);
            bool[] doorIsOpen;
            bool[] cornerIsOpen;

            // Doors are in L, U, D, R order:
            // Corners in UL, UR, DL, DR order:

            // Top row:
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[1,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, true, true};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[2,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key2);
            
            doorIsOpen = new bool[]{true, false, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{true, false, true, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[5,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[6,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Second row:         
            doorIsOpen = new bool[]{false, false, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_treasureFinal);

            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[1,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{true, true, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[5,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[6,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Third row:         
            doorIsOpen = new bool[]{false, true, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{true, false, false, true};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, true};
            simplifiedRoomArray[4,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, true, true};
            simplifiedRoomArray[5,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, false};
            cornerIsOpen = new bool[]{false, false, true, false};
            simplifiedRoomArray[6,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Fourth row:         
            // doorIsOpen = new bool[]{false, true, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[0,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_treasureFinal);
            
            // doorIsOpen = new bool[]{true, false, false, true};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[1,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, true, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, true};
            cornerIsOpen = new bool[]{false, true, false, false};
            simplifiedRoomArray[4,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, true};
            cornerIsOpen = new bool[]{true, true, false, false};
            simplifiedRoomArray[5,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{true, false, false, false};
            simplifiedRoomArray[6,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            

            byte[] playerPos = {5, 3};
            byte[,] keyLocations = { {0, 0}, {3, 0}, {0, 0}, {0, 0}, {0, 0}, {0, 0} };
            bool[] hasKey = {false, true, false, false, false, false};
            byte[] finalTreasureLocation = {0, 1};
            byte[,] doorLocations = { {2, 2} };
            byte[] doorSides = {Constants.doorID_left};
            byte[] doorColors = {Constants.interactableID_door2};
            short numOfRooms = 17;
            short numOfLockedDoors = 1;
            return new SimplifiedLayoutReturnPacket(simplifiedRoomArray, playerPos, keyLocations, hasKey, finalTreasureLocation, doorLocations, doorSides, doorColors, numOfRooms, numOfLockedDoors);
        }


        public static SimplifiedLayoutReturnPacket generateLayout_DebugRoom()
        {
            SimpleRoom_Output[,] simplifiedRoomArray = SimplifiedLayoutBuilder.instantiateRooms_Output(7, 8);
            bool[] doorIsOpen;
            bool[] cornerIsOpen;

            // Doors are in L, U, D, R order:
            // Corners in UL, UR, DL, DR order:

            // First row:
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            /*
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool{false, false, false, false};
            simplifiedRoomArray[3,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            */
            
            doorIsOpen = new bool[]{false, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[5,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,0].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Second row:
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            /*
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            */

            doorIsOpen = new bool[]{false, true, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            /*
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[5,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            */

            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,1].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Third row:
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key1);
            
            doorIsOpen = new bool[]{true, true, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key4);
            
            doorIsOpen = new bool[]{true, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[5,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,2].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Fourth row:
            doorIsOpen = new bool[]{false, true, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key2);
            
            doorIsOpen = new bool[]{true, true, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key5);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[5,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,3].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Fifth row:
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[0,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[1,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[2,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key3);
            
            doorIsOpen = new bool[]{true, true, true, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[4,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_key6);
            
            doorIsOpen = new bool[]{true, false, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[5,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_treasureFinal);
            
            doorIsOpen = new bool[]{false, true, true, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,4].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Sixth row:
            doorIsOpen = new bool[]{false, true, true, true};
            cornerIsOpen = new bool[]{false, false, false, true};
            simplifiedRoomArray[0,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, true, true};
            simplifiedRoomArray[1,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, true, false};
            simplifiedRoomArray[2,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, true};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[3,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, false, true};
            simplifiedRoomArray[4,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, false, true, true};
            cornerIsOpen = new bool[]{false, false, true, false};
            simplifiedRoomArray[5,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{false, false, false, false};
            simplifiedRoomArray[6,5].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Seventh row:
            doorIsOpen = new bool[]{false, true, true, true};
            cornerIsOpen = new bool[]{false, true, false, true};
            simplifiedRoomArray[0,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, true, true};
            cornerIsOpen = new bool[]{true, true, true, true};
            simplifiedRoomArray[1,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, true, false};
            cornerIsOpen = new bool[]{true, false, true, false};
            simplifiedRoomArray[2,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{false, true, false, true};
            cornerIsOpen = new bool[]{false, true, false, false};
            simplifiedRoomArray[4,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{true, false, false, false};
            simplifiedRoomArray[5,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[6,6].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            // Eighth row:
            doorIsOpen = new bool[]{false, true, false, true};
            cornerIsOpen = new bool[]{false, true, false, false};
            simplifiedRoomArray[0,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, true};
            cornerIsOpen = new bool[]{true, true, false, false};
            simplifiedRoomArray[1,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            doorIsOpen = new bool[]{true, true, false, false};
            cornerIsOpen = new bool[]{true, false, false, false};
            simplifiedRoomArray[2,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[3,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[4,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[5,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);
            
            // doorIsOpen = new bool[]{false, false, false, false};
            // cornerIsOpen = new bool[]{false, false, false, false};
            // simplifiedRoomArray[6,7].setInformation(doorIsOpen, cornerIsOpen, Constants.interactableID_none);

            byte[] playerPos = {3, 3};
            byte[,] keyLocations = { {2, 2} , {2, 3}, {2, 4}, {4, 2}, {4, 3}, {4, 4} };
            bool[] hasKey = {true, true, true, true, true, true};
            byte[] finalTreasureLocation = {5, 4};
            byte[,] doorLocations = { {2, 2} , {2, 3}, {2, 4}, {4, 2}, {4, 3}, {4, 4} };
            byte[] doorSides = {Constants.doorID_left, Constants.doorID_left, Constants.doorID_left, Constants.doorID_right, Constants.doorID_right, Constants.doorID_right};
            byte[] doorColors = {Constants.interactableID_door1, Constants.interactableID_door2, Constants.interactableID_door3, Constants.interactableID_door4, Constants.interactableID_door5, Constants.interactableID_door6};
            short numOfRooms = 47;
            short numOfLockedDoors = 6;
            return new SimplifiedLayoutReturnPacket(simplifiedRoomArray, playerPos, keyLocations, hasKey, finalTreasureLocation, doorLocations, doorSides, doorColors, numOfRooms, numOfLockedDoors);
        }
    }
}

