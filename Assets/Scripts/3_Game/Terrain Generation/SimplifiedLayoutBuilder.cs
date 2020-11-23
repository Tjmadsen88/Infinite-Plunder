using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace SimplifiedLayoutBuilderSpace
{
    public static class SimplifiedLayoutBuilder
    {
        private const byte pathLength_shortest = 1;
        // private const byte pathLength_longest = 4;
        private const byte pathLength_longest = 6;

        // private const byte maxWidthHeight_building = 15;
        private const byte maxWidthHeight_path = (byte)(pathLength_longest*2 +1);

        // private const byte playerStartXY = (byte)(maxWidthHeight_building/2);
        private const byte pathStartXY = (byte)(maxWidthHeight_path/2);

        private const byte clearance_null = 200;


        public static SimplifiedLayoutReturnPacket generateLayout_Normal(byte numOfPathsPerKey, byte numOfKeys, System.Random random_thread)
        {
            byte maxWidthHeight_building;

            switch(numOfPathsPerKey)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    maxWidthHeight_building = 15;
                    break;

                default:
                    maxWidthHeight_building = (byte)(5*numOfPathsPerKey);
                    break;
            }

            byte playerStartXY = (byte)(maxWidthHeight_building/2);

            SimplifiedLayoutDataPacket simpleDataPacket = new SimplifiedLayoutDataPacket(maxWidthHeight_building, maxWidthHeight_path, playerStartXY, random_thread);

            bool pathSucceeded;

            // int pathNums_max = Random.Range(0, 5);
            int pathNums_max = numOfPathsPerKey-1;
            int pathNum_current;
            
            bool[] availableClearances = defineWhichClearancesAreAvailable(numOfKeys, random_thread);

            for (int index=0; index<7; index++)
            {
                pathNum_current = pathNums_max;

                while (pathNum_current > 0) {
                    if (buildOnePath(simpleDataPacket, getRandomPathLength(numOfPathsPerKey, random_thread), false, random_thread))
                    {
                        pathNum_current--;
                    }
                }

                // if (availableClearances[index])
                // {
                    do {
                        pathSucceeded = buildOnePath(simpleDataPacket, getRandomPathLength(numOfPathsPerKey, random_thread), true, random_thread);
                    } while (!pathSucceeded);
                    simpleDataPacket.incrementTheClearanceLevel();
                // } else {
                //     do {
                //         pathSucceeded = buildOnePath(simpleDataPacket, getRandomPathLength(random_thread), false, random_thread);
                //     } while (!pathSucceeded);
                // }
            }


            byte[] playerStartingLocation = {playerStartXY, playerStartXY};
                                                    
            return convertBuildingArrayToReturnPacket(simpleDataPacket, playerStartingLocation, numOfKeys, availableClearances, random_thread);

                                                    //simpleDataPacket, byte[] playerStartingLocation, byte numOfKeys
        }

    // ------------------------------------------------------------------------------------------------------
    // ----------- Path-drawing stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private static byte getRandomPathLength(byte numOfPathsPerKey, System.Random random_thread)
    {
        if (numOfPathsPerKey > 0) return (byte)random_thread.Next(pathLength_shortest, pathLength_longest+1);
        return (byte)random_thread.Next(1, 3);
        // return (byte)Random.Range(pathLength_shortest, pathLength_longest+1);
    }

    private static bool buildOnePath(SimplifiedLayoutDataPacket simpleDataPacket, byte lengthOfPath, bool isKeyPath, System.Random random_thread)
    {
        byte[] roomCoords = new byte[2]; // X, Y
        byte[] roomInfo = new byte[2]; // direction, clearance

        // First, find a good spotm and gather information:
        do {
            roomCoords = simpleDataPacket.getRandomCoordInBoundingBox(random_thread);
            roomInfo = gatherInfoAboutRandomAdjacentPlacedRoom(simpleDataPacket.getSimpleArray_building(), roomCoords, random_thread);
        } while (roomInfo[0] == Constants.doorID_null);

        // Reset the path array, update it's position relative to the builderArray, and keep track of the first room:
        SimpleRoom_Building firstRoomInPath = simpleDataPacket.resetArray_Path(roomCoords[0], roomCoords[1]);

        // define the first room, which will always have a door on a keyPath:
        if (isKeyPath)
        {
            roomInfo[1] = simpleDataPacket.getCurrentClearanceLevel();
            if (lengthOfPath > 1) firstRoomInPath.setInformation(roomInfo[1], roomInfo[0], roomInfo[1], 0);
            else firstRoomInPath.setInformation(roomInfo[1], roomInfo[0], roomInfo[1], (byte)(roomInfo[1]+1));
            
        } else {
            byte doorClearance = simpleDataPacket.getRandomClearanceLevel(random_thread);
            roomInfo[1] = (byte)Mathf.Max(doorClearance, roomInfo[1]);
            firstRoomInPath.setInformation(roomInfo[1], roomInfo[0], doorClearance, 0);
        }
        
        // now, attempt to place the next room. If any room fails, this will return false. If they all succeed, this will return true:
        if (addNextRoomToPath(simpleDataPacket, roomCoords, roomInfo, (byte)(lengthOfPath-1), isKeyPath, random_thread))
        {
            simpleDataPacket.joinPathArrayElementsIntoBuildingArray();
            return true;
        }

        return false;
    }


    private static bool addNextRoomToPath(SimplifiedLayoutDataPacket simpleDataPacket, byte[] previousRoomCoords, byte[] previousRoomInfo, byte moreRoomsToPlace, bool isKeyPath, System.Random random_thread)
    {
        // If there are no more rooms to place, send 'true' all the way up:
        if (moreRoomsToPlace == 0) return true;

        // I need to set the previous space to 'tried' right away:
        simpleDataPacket.getPathRoom_usingBuildingCoords(previousRoomCoords[0], previousRoomCoords[1]).setHasntBeenTried(false);

        // We need some info to be defined upfront. The last room of a keyPath needs to have a key:
        byte keyToPlace = 0;
        if (moreRoomsToPlace == 1 && isKeyPath) keyToPlace = (byte)(simpleDataPacket.getCurrentClearanceLevel() +1);

        // We assume the first room has been done already, so any later room could get a random door:
        byte doorToPlace = simpleDataPacket.getRandomClearanceLevel(random_thread);

        // The clearance might update based on the door placed:
        byte[] nextRoomInfo = new byte[2];
        nextRoomInfo[1] = (byte)Mathf.Max(doorToPlace, previousRoomInfo[1]);

        byte[] nextRoomCoords = new byte[2];


        // Now, check each of the surrounding rooms, starting with a random one:
        // byte randomDirection = (byte)Random.Range(0, 4);
        byte randomDirection = (byte)random_thread.Next(0, 4);

        for (int index=0; index<4; index++)
        {
            switch (randomDirection)
            {
                case Constants.doorID_left:
                    nextRoomCoords[0] = (byte)(previousRoomCoords[0]-1);
                    nextRoomCoords[1] = previousRoomCoords[1];
                    nextRoomInfo[0] = Constants.doorID_right;
                    break;
                        
                case Constants.doorID_right:
                    nextRoomCoords[0] = (byte)(previousRoomCoords[0]+1);
                    nextRoomCoords[1] = previousRoomCoords[1];
                    nextRoomInfo[0] = Constants.doorID_left;
                    break;
                        
                case Constants.doorID_up:
                    nextRoomCoords[0] = previousRoomCoords[0];
                    nextRoomCoords[1] = (byte)(previousRoomCoords[1]-1);
                    nextRoomInfo[0] = Constants.doorID_down;
                    break;
                        
                default: //case Constants.doorID_down:
                    nextRoomCoords[0] = previousRoomCoords[0];
                    nextRoomCoords[1] = (byte)(previousRoomCoords[1]+1);
                    nextRoomInfo[0] = Constants.doorID_up;
                    break;
            }

            

            if (simpleDataPacket.checkIfCoordIsAvailable(nextRoomCoords[0], nextRoomCoords[1]))
            {
                if (addNextRoomToPath(simpleDataPacket, nextRoomCoords, nextRoomInfo, (byte)(moreRoomsToPlace-1), isKeyPath, random_thread))
                {
                    simpleDataPacket.getPathRoom_usingBuildingCoords(nextRoomCoords[0], nextRoomCoords[1]).setInformation(nextRoomInfo[1], nextRoomInfo[0], doorToPlace, keyToPlace);
                    return true;
                }
            }

            randomDirection = (byte)((randomDirection + 1) % 4);
        }

        // If we get here, then none of the adjacent spaces had room. We return false, so the room above can try another door:
        return false;
    }


    private static byte[] gatherInfoAboutRandomAdjacentPlacedRoom(SimpleRoom_Building[,] simpleArray_building, byte[] roomCoords, System.Random random_thread)
    {
        int maxWidthHeight_building = simpleArray_building.GetLength(0);
        byte[] roomInfo = new byte[2];

        // If the input space is already occupied, return an error:
        if (simpleArray_building[roomCoords[0], roomCoords[1]].getIsNotEmpty())
        {
            roomInfo[0] = Constants.doorID_null;
            return roomInfo;
        }

        // byte randomDirection = (byte)Random.Range(0, 4);
        byte randomDirection = (byte)random_thread.Next(0, 4);

        for (int index=0; index<4; index++)
        {
            switch (randomDirection)
            {
                case Constants.doorID_left:
                    if (roomCoords[0] > 0 && simpleArray_building[roomCoords[0]-1, roomCoords[1]].getIsNotEmpty())
                    {
                        roomInfo[0] = randomDirection;
                        roomInfo[1] = simpleArray_building[roomCoords[0]-1, roomCoords[1]].getClearanceLevel();
                        return roomInfo;
                    }
                    break;
                        
                case Constants.doorID_right:
                    if (roomCoords[0] < maxWidthHeight_building-1 && simpleArray_building[roomCoords[0]+1, roomCoords[1]].getIsNotEmpty())
                    {
                        roomInfo[0] = randomDirection;
                        roomInfo[1] = simpleArray_building[roomCoords[0]+1, roomCoords[1]].getClearanceLevel();
                        return roomInfo;
                    }
                    break;
                        
                case Constants.doorID_up:
                    if (roomCoords[1] > 0 && simpleArray_building[roomCoords[0], roomCoords[1]-1].getIsNotEmpty())
                    {
                        roomInfo[0] = randomDirection;
                        roomInfo[1] = simpleArray_building[roomCoords[0], roomCoords[1]-1].getClearanceLevel();
                        return roomInfo;
                    }
                    break;
                        
                default: //case Constants.doorID_down:
                    if (roomCoords[1] < maxWidthHeight_building-1 && simpleArray_building[roomCoords[0], roomCoords[1]+1].getIsNotEmpty())
                    {
                        roomInfo[0] = randomDirection;
                        roomInfo[1] = simpleArray_building[roomCoords[0], roomCoords[1]+1].getClearanceLevel();
                        return roomInfo;
                    }
                    break;
            }

            randomDirection = (byte)((randomDirection + 1) % 4);
        }

        roomInfo[0] = Constants.doorID_null;
        return roomInfo;
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Clearance to Key Color stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

        private static byte[] defineClearanceToColorConverter(System.Random random_thread)
        {
            // return new byte[]{0, 1, 2, 3, 4, 5, 6};


            byte[] returnBytes = new byte[7];
            returnBytes[0] = 0;
            // returnBytes[7] = 7;

            byte firstOpenSlot = 1;
            bool[] isAvailable = {false, true, true, true, true, true, true};

            byte tempClearance;

            do {
                // tempClearance = (byte)Random.Range(1, 7);
                tempClearance = (byte)random_thread.Next(1, 7);

                if (isAvailable[tempClearance])
                {
                    returnBytes[firstOpenSlot] = tempClearance;
                    isAvailable[tempClearance] = false;
                    firstOpenSlot++;
                } 
            } while (firstOpenSlot < 7);

            Debug.Log(string.Format("clearanceColors: {0} {1} {2} {3} {4} {5} {6}", returnBytes[0], returnBytes[1], returnBytes[2], returnBytes[3], returnBytes[4], returnBytes[5], returnBytes[6]));

            return returnBytes;
        }

        private static bool[] defineWhichClearancesAreAvailable(byte numOfKeys, System.Random random_thread)
        {
            if (numOfKeys == 0) return new bool[]{false, false, false, false, false, false, true};
            if (numOfKeys == 1) return new bool[]{false, false, false, false, false, true, true};
            if (numOfKeys == 6) return new bool[]{true, true, true, true, true, true, true};

            bool[] returnArray = new bool[]{true, true, true, true, true, true, true};
            byte keysToRemove = (byte)(6-numOfKeys);

            byte tempSlot;

            do {
                // tempSlot = (byte)Random.Range(0, 5);
                tempSlot = (byte)random_thread.Next(0, 5);

                if (returnArray[tempSlot])
                {
                    returnArray[tempSlot] = false;
                    keysToRemove--;
                } 
            } while (keysToRemove > 0);

            return returnArray;
        }

        private static byte[] defineClearanceDemoter(bool[] availableClearances, System.Random random_thread)
        {
            byte[] returnBytes = new byte[8];
            byte currentClearance = 1;

            returnBytes[0] = 0;

            for (int index=0; index<7; index++)
            {
                if (availableClearances[index])
                {
                    returnBytes[index+1] = currentClearance;
                    currentClearance++;
                } else {
                    returnBytes[index+1] = currentClearance;
                }
            }

            returnBytes[7] = 7;

            return returnBytes;
        }


        private static byte convertClearanceLevelToKeyID(byte clearanceLevel, byte[] clearanceToColorConverter, byte[] clearanceDemoter, byte numOfKeys)
        {
            if (clearanceLevel == 0) return Constants.interactableID_none;
            if (clearanceLevel == 7) return Constants.interactableID_treasureFinal;

            if (clearanceDemoter[clearanceLevel-1] == clearance_null) return Constants.interactableID_none;
            // if (clearanceDemoter[clearanceLevel-1]+1 > numOfKeys) return Constants.interactableID_treasureFinal;

            switch(clearanceToColorConverter[clearanceDemoter[clearanceLevel-1]+1])
            {
                case 1: return Constants.interactableID_key1;
                case 2: return Constants.interactableID_key2;
                case 3: return Constants.interactableID_key3;
                case 4: return Constants.interactableID_key4;
                case 5: return Constants.interactableID_key5;
                case 6: return Constants.interactableID_key6;
                case 7: return Constants.interactableID_treasureFinal;
                default: return Constants.interactableID_none;
            }
        }

        private static byte convertClearanceLevelToDoorID(byte clearanceLevel, byte[] clearanceToColorConverter, byte[] clearanceDemoter)
        {
            if (clearanceDemoter[clearanceLevel] == clearance_null) return Constants.interactableID_none;

            switch(clearanceToColorConverter[clearanceDemoter[clearanceLevel]])
            {
                case 1: return Constants.interactableID_door1;
                case 2: return Constants.interactableID_door2;
                case 3: return Constants.interactableID_door3;
                case 4: return Constants.interactableID_door4;
                case 5: return Constants.interactableID_door5;
                case 6: return Constants.interactableID_door6;
                default: return Constants.interactableID_none;
            }
        }

        private static short getTotalNumOfDoors_old(SimplifiedLayoutDataPacket simpleDataPacket, byte numOfKeys)
        {
            short numOfDoors = 0;

            if (numOfKeys > 0) numOfDoors += simpleDataPacket.getNumOfDoors_clearance1();
            if (numOfKeys > 1) numOfDoors += simpleDataPacket.getNumOfDoors_clearance2();
            if (numOfKeys > 2) numOfDoors += simpleDataPacket.getNumOfDoors_clearance3();
            if (numOfKeys > 3) numOfDoors += simpleDataPacket.getNumOfDoors_clearance4();
            if (numOfKeys > 4) numOfDoors += simpleDataPacket.getNumOfDoors_clearance5();
            if (numOfKeys > 5) numOfDoors += simpleDataPacket.getNumOfDoors_clearance6();

            return numOfDoors;
        }

        private static short getTotalNumOfDoors(SimplifiedLayoutDataPacket simpleDataPacket, byte[] clearanceDemoter, byte numOfKeys)
        {
            if (numOfKeys == 0) return 0;

            short numOfDoors = 0;

            if (clearanceDemoter[1] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance1();
            if (clearanceDemoter[2] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance2();
            if (clearanceDemoter[3] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance3();
            if (clearanceDemoter[4] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance4();
            if (clearanceDemoter[5] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance5();
            if (clearanceDemoter[6] != clearance_null) numOfDoors += simpleDataPacket.getNumOfDoors_clearance6();

            return numOfDoors;
        }

        private static bool[] calculateHasKey(byte numOfKeys, byte[] clearanceToColorConverter, byte[] clearanceDemoter)
        {
            bool[] hasKey = new bool[6];

            for(byte index=0; index<6; index++)
            {
                hasKey[clearanceToColorConverter[index+1]-1] = index < numOfKeys;
            }

            return hasKey;
        }




    // ------------------------------------------------------------------------------------------------------
    // ----------- Output Packet stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

        public static SimpleRoom_Output[,] instantiateRooms_Output(byte width, byte height)
        {
            SimpleRoom_Output[,] returnArray = new SimpleRoom_Output[width, height];

            for (int indexX=0; indexX<width; indexX++)
            {
                for (int indexY=0; indexY<height; indexY++)
                {
                    returnArray[indexX, indexY] = new SimpleRoom_Output();
                }
            }

            return returnArray;
        }

        private static SimplifiedLayoutReturnPacket convertBuildingArrayToReturnPacket(SimplifiedLayoutDataPacket simpleDataPacket,
                                                                                    byte[] playerStartingLocation, byte numOfKeys,
                                                                                    bool[] availableClearances, System.Random random_thread)
        {
            int roomBound_L = simpleDataPacket.getRoomBound_L();
            int roomBound_U = simpleDataPacket.getRoomBound_U();

            byte newArrayWidth = (byte)(simpleDataPacket.getRoomBound_R() - roomBound_L);
            byte newArrayHeight = (byte)(simpleDataPacket.getRoomBound_D() - roomBound_U);

            SimpleRoom_Building[,] simpleArray_building = simpleDataPacket.getSimpleArray_building();
            SimpleRoom_Output[,] simpleArray_output = instantiateRooms_Output(newArrayWidth, newArrayHeight);
            int indexX_building;
            int indexY_building;

            byte[] clearanceToColorConverter = defineClearanceToColorConverter(random_thread);
            byte[] clearanceDemoter = defineClearanceDemoter(availableClearances, random_thread);
            Debug.Log(string.Format("availableClearances: {0} {1} {2} {3} {4} {5}", availableClearances[0], availableClearances[1], availableClearances[2], availableClearances[3], availableClearances[4], availableClearances[5]));
            Debug.Log(string.Format("clearanceDemoter: {0} {1} {2} {3} {4} {5}", clearanceDemoter[1], clearanceDemoter[2], clearanceDemoter[3], clearanceDemoter[4], clearanceDemoter[5], clearanceDemoter[6]));

            byte[,] keyLocations = new byte[6, 2];
            byte[] finalTreasureLocation = new byte[2];
            
            bool[] hasKey = calculateHasKey(numOfKeys, clearanceToColorConverter, clearanceDemoter);
            Debug.Log(string.Format("HasKey: {0} {1} {2} {3} {4} {5}", hasKey[0], hasKey[1], hasKey[2], hasKey[3], hasKey[4], hasKey[5]));

            short totalNumOfDoors = getTotalNumOfDoors(simpleDataPacket, clearanceDemoter, numOfKeys);
            byte[,] doorLocations = new byte[totalNumOfDoors, 2];
            byte[] doorSides = new byte[totalNumOfDoors];
            byte[] doorColors = new byte[totalNumOfDoors];
            byte firstOpenDoorSlot = 0;
        
            short numOfRooms = 0;

            byte tempItem;


            for (byte indexX=0; indexX<newArrayWidth; indexX++)
            {
                for (byte indexY=0; indexY<newArrayHeight; indexY++)
                {
                    indexX_building = indexX + roomBound_L;
                    indexY_building = indexY + roomBound_U;

                    if (simpleArray_building[indexX_building, indexY_building].getIsNotEmpty()) 
                    {
                        simpleArray_output[indexX, indexY].setIsNotEmpty(true);

                        // Check the right wall:
                        if (indexX < newArrayWidth-1)
                            fillOutOutputRoomData_checkAdjacentDoors(simpleArray_building[indexX_building, indexY_building], simpleArray_building[indexX_building+1, indexY_building],
                                                                    simpleArray_output[indexX, indexY], simpleArray_output[indexX+1, indexY],
                                                                    Constants.doorID_right, Constants.doorID_left);

                        // Check the bottom wall:                    
                        if (indexY < newArrayHeight-1)
                            fillOutOutputRoomData_checkAdjacentDoors(simpleArray_building[indexX_building, indexY_building], simpleArray_building[indexX_building, indexY_building+1],
                                                                    simpleArray_output[indexX, indexY], simpleArray_output[indexX, indexY+1],
                                                                    Constants.doorID_down, Constants.doorID_up);

                        // Check the UL corner:
                        if (indexX>0 && indexY>0)
                            fillOutOutputRoomData_checkULCorner(simpleArray_output[indexX, indexY], simpleArray_output[indexX-1, indexY],
                                                                simpleArray_output[indexX, indexY-1], simpleArray_output[indexX-1, indexY-1],
                                                                simpleArray_building[indexX_building, indexY_building], simpleArray_building[indexX_building-1, indexY_building],
                                                                simpleArray_building[indexX_building, indexY_building-1], simpleArray_building[indexX_building-1, indexY_building-1]);

                        // Now, gather some stats:
                        numOfRooms++;

                        // Start with the keys/treasure:
                        tempItem = convertClearanceLevelToKeyID(simpleArray_building[indexX_building, indexY_building].getContainsItem(),
                                                                clearanceToColorConverter, clearanceDemoter, numOfKeys);
                        switch(tempItem)
                        {
                            case Constants.interactableID_key1:
                                if (hasKey[0])
                                {
                                    keyLocations[0,0] = indexX;
                                    keyLocations[0,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_key2:
                                if (hasKey[1])
                                {
                                    keyLocations[1,0] = indexX;
                                    keyLocations[1,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_key3:
                                if (hasKey[2])
                                {
                                    keyLocations[2,0] = indexX;
                                    keyLocations[2,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_key4:
                                if (hasKey[3])
                                {
                                    keyLocations[3,0] = indexX;
                                    keyLocations[3,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_key5:
                                if (hasKey[4])
                                {
                                    keyLocations[4,0] = indexX;
                                    keyLocations[4,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_key6:
                                if (hasKey[5])
                                {
                                    keyLocations[5,0] = indexX;
                                    keyLocations[5,1] = indexY;
                                }
                                break;
                                
                            case Constants.interactableID_treasureFinal:
                                finalTreasureLocation[0] = indexX;
                                finalTreasureLocation[1] = indexY;
                                break;

                            default:
                                break;
                        }

                        // Now check for locked doors:
                        tempItem = convertClearanceLevelToDoorID(simpleArray_building[indexX_building, indexY_building].getContainsDoor(),
                                                                clearanceToColorConverter, clearanceDemoter);
                        switch(tempItem)
                        {
                            case Constants.interactableID_door1:
                                if (hasKey[0])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;
                                
                            case Constants.interactableID_door2:
                                if (hasKey[1])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;
                                
                            case Constants.interactableID_door3:
                                if (hasKey[2])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;
                                
                            case Constants.interactableID_door4:
                                if (hasKey[3])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;
                                
                            case Constants.interactableID_door5:
                                if (hasKey[4])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;
                                
                            case Constants.interactableID_door6:
                                if (hasKey[5])
                                {
                                    doorLocations[firstOpenDoorSlot, 0] = indexX;
                                    doorLocations[firstOpenDoorSlot, 1] = indexY;
                                    doorSides[firstOpenDoorSlot] = simpleArray_building[indexX_building, indexY_building].getPreviousRoomDirection();
                                    doorColors[firstOpenDoorSlot] = tempItem;
                                    firstOpenDoorSlot++;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            
            // Also, quickly convert the player's starting location:
            byte[] newPlayerStartingLocation = new byte[2];
            newPlayerStartingLocation[0] = (byte)(playerStartingLocation[0] - roomBound_L);
            newPlayerStartingLocation[1] = (byte)(playerStartingLocation[1] - roomBound_U);

            // Now, put all the info together into one returnPacket:
            return new SimplifiedLayoutReturnPacket(simpleArray_output, newPlayerStartingLocation, 
                                                    keyLocations, hasKey, finalTreasureLocation,
                                                    doorLocations, doorSides, doorColors,
                                                    numOfRooms, totalNumOfDoors);
        }


        private static void fillOutOutputRoomData_checkAdjacentDoors(SimpleRoom_Building thisRoom_building, SimpleRoom_Building adjRoom_building, 
                                                                SimpleRoom_Output thisRoom_output, SimpleRoom_Output adjRoom_output,
                                                                byte thisRoomConnection, byte adjRoomConnection)
        {
            // thisRoomConnection should be either Constants.doorID_right or Constants.doorID_down,
            // while adjRoomConnection should be either Constants.doorID_left or Constants.doorID_up.

            // If this room has no building entry, keep the output empty as well: 
            if (!thisRoom_building.getIsNotEmpty() || !adjRoom_building.getIsNotEmpty()) return;

            // If the rooms share a clearance level, or if they are connected by a door, open their walls:
            if ( thisRoom_building.getClearanceLevel() == adjRoom_building.getClearanceLevel() ||
                (thisRoom_building.getContainsDoor() != Constants.simplified_doorStatus_undefined && thisRoom_building.getPreviousRoomDirection() == thisRoomConnection) ||
                (adjRoom_building.getContainsDoor() != Constants.simplified_doorStatus_undefined && adjRoom_building.getPreviousRoomDirection() == adjRoomConnection) )
            {
                thisRoom_output.setWallAsOpen(thisRoomConnection);
                adjRoom_output.setWallAsOpen(adjRoomConnection);
            }

            // If not, both walls will stay closed.
        }  

        private static void fillOutOutputRoomData_checkULCorner(SimpleRoom_Output thisRoom_output, SimpleRoom_Output leftRoom_output,
                                                                SimpleRoom_Output upRoom_output, SimpleRoom_Output diagonalRoom_output,
                                                                SimpleRoom_Building thisRoom_building, SimpleRoom_Building leftRoom_building,
                                                                SimpleRoom_Building upRoom_building, SimpleRoom_Building diagonalRoom_building)
        {
            // If any of these rooms are empty, keep the corner closed:
            if (!thisRoom_output.getIsNotEmpty() || !leftRoom_output.getIsNotEmpty() ||
                !upRoom_output.getIsNotEmpty() || !diagonalRoom_output.getIsNotEmpty()) 
                return;

            // Check all eight walls connected to this corner. If all of them are open, open up the corner:
            if (thisRoom_output.getWallIsOpen(Constants.doorID_up) && thisRoom_output.getWallIsOpen(Constants.doorID_left) &&
                // leftRoom_output.getWallIsOpen(Constants.doorID_up) && leftRoom_output.getWallIsOpen(Constants.doorID_right) &&
                // upRoom_output.getWallIsOpen(Constants.doorID_down) && upRoom_output.getWallIsOpen(Constants.doorID_left) &&
                diagonalRoom_output.getWallIsOpen(Constants.doorID_down) && diagonalRoom_output.getWallIsOpen(Constants.doorID_right) && 
                (thisRoom_building.getContainsDoor() == Constants.simplified_doorStatus_undefined 
                    || thisRoom_building.getPreviousRoomDirection() == Constants.doorID_right || thisRoom_building.getPreviousRoomDirection() == Constants.doorID_down) &&
                (leftRoom_building.getContainsDoor() == Constants.simplified_doorStatus_undefined 
                    || leftRoom_building.getPreviousRoomDirection() == Constants.doorID_left || leftRoom_building.getPreviousRoomDirection() == Constants.doorID_down) &&
                (upRoom_building.getContainsDoor() == Constants.simplified_doorStatus_undefined 
                    || upRoom_building.getPreviousRoomDirection() == Constants.doorID_up || upRoom_building.getPreviousRoomDirection() == Constants.doorID_right) &&
                (diagonalRoom_building.getContainsDoor() == Constants.simplified_doorStatus_undefined 
                    || diagonalRoom_building.getPreviousRoomDirection() == Constants.doorID_up || diagonalRoom_building.getPreviousRoomDirection() == Constants.doorID_left) )
            {
                thisRoom_output.setCornerAsOpen(Constants.cornerID_ul);
                leftRoom_output.setCornerAsOpen(Constants.cornerID_ur);
                upRoom_output.setCornerAsOpen(Constants.cornerID_dl);
                diagonalRoom_output.setCornerAsOpen(Constants.cornerID_dr);
            }
        }   
    }
}