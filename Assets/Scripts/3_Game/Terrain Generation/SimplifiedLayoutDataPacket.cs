using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace SimplifiedLayoutBuilderSpace
{
    public class SimplifiedLayoutDataPacket
    {
        private SimpleRoom_Building[,] simpleArray_building;
        private SimpleRoom_Building[,] simpleArray_path;

        private byte arraySize_building;
        private byte arraySize_path;

        private byte boundingBox_L;
        private byte boundingBox_R;
        private byte boundingBox_U;
        private byte boundingBox_D;

        private byte roomBound_L;
        private byte roomBound_R;
        private byte roomBound_U;
        private byte roomBound_D;

        private int pathStartXY;

        private int pathToBuildingX;
        private int pathToBuildingY;

        private byte currentClearanceLevel = 0;

        private short numOfDoors_clearance1 = 0;
        private short numOfDoors_clearance2 = 0;
        private short numOfDoors_clearance3 = 0;
        private short numOfDoors_clearance4 = 0;
        private short numOfDoors_clearance5 = 0;
        private short numOfDoors_clearance6 = 0;

        private byte[] newRandomCoord;
        private byte[] dudCoord = new byte[2];


        public SimplifiedLayoutDataPacket(byte arraySize_building, byte arraySize_path, byte playerStartXY, System.Random random_thread)
        {
            simpleArray_building = createInitialArray_Building(arraySize_building, playerStartXY, random_thread);
            simpleArray_path = instantiateRooms_Building(arraySize_path);

            this.arraySize_building = arraySize_building;
            this.arraySize_path = arraySize_path;

            boundingBox_L = (byte)(playerStartXY -2);
            boundingBox_R = (byte)(playerStartXY + 3);
            boundingBox_U = boundingBox_L;
            boundingBox_D = boundingBox_R;
            // boundingBox_L = 0;
            // boundingBox_R = arraySize_building;
            // boundingBox_U = 0;
            // boundingBox_D = arraySize_building;

            roomBound_L = (byte)(playerStartXY -1);
            roomBound_R = (byte)(playerStartXY + 2);
            roomBound_U = roomBound_L;
            roomBound_D = roomBound_R;

            pathStartXY = arraySize_path/2;
        }

        private SimpleRoom_Building[,] instantiateRooms_Building(byte widthHeight)
        {
            SimpleRoom_Building[,] returnArray = new SimpleRoom_Building[widthHeight, widthHeight];

            for (int indexX=0; indexX<widthHeight; indexX++)
            {
                for (int indexY=0; indexY<widthHeight; indexY++)
                {
                    returnArray[indexX, indexY] = new SimpleRoom_Building();
                }
            }

            return returnArray;
        }

        private SimpleRoom_Building[,] createInitialArray_Building(byte widthHeight, byte playerStartXY, System.Random random_thread)
        {
            SimpleRoom_Building[,] simpleArray_building = instantiateRooms_Building(widthHeight);

            // simpleArray_building[playerStartXY, playerStartXY].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[playerStartXY-1, playerStartXY].setInformation(0, Constants.doorID_right, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[playerStartXY+1, playerStartXY].setInformation(0, Constants.doorID_left, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[playerStartXY, playerStartXY-1].setInformation(0, Constants.doorID_down, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[playerStartXY, playerStartXY+1].setInformation(0, Constants.doorID_up, Constants.interactableID_none, Constants.interactableID_none);

            
            simpleArray_building[playerStartXY, playerStartXY].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);

            switch(random_thread.Next(0, 4))
            {
                case 0:
                    dudCoord[0] = (byte)(playerStartXY-1);
                    dudCoord[1] = playerStartXY;

                    // simpleArray_building[playerStartXY-1, playerStartXY].setInformation(0, Constants.doorID_right, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY+1, playerStartXY].setInformation(0, Constants.doorID_left, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY-1].setInformation(0, Constants.doorID_down, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY+1].setInformation(0, Constants.doorID_up, Constants.interactableID_none, Constants.interactableID_none);
                    break;
                    
                case 1:
                    dudCoord[0] = playerStartXY;
                    dudCoord[1] = (byte)(playerStartXY-1);

                    simpleArray_building[playerStartXY-1, playerStartXY].setInformation(0, Constants.doorID_right, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY+1, playerStartXY].setInformation(0, Constants.doorID_left, Constants.interactableID_none, Constants.interactableID_none);
                    // simpleArray_building[playerStartXY, playerStartXY-1].setInformation(0, Constants.doorID_down, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY+1].setInformation(0, Constants.doorID_up, Constants.interactableID_none, Constants.interactableID_none);
                    break;
                    
                case 3:
                    dudCoord[0] = playerStartXY;
                    dudCoord[1] = (byte)(playerStartXY+1);

                    simpleArray_building[playerStartXY-1, playerStartXY].setInformation(0, Constants.doorID_right, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY+1, playerStartXY].setInformation(0, Constants.doorID_left, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY-1].setInformation(0, Constants.doorID_down, Constants.interactableID_none, Constants.interactableID_none);
                    // simpleArray_building[playerStartXY, playerStartXY+1].setInformation(0, Constants.doorID_up, Constants.interactableID_none, Constants.interactableID_none);
                    break;

                default: //case 3:
                    dudCoord[0] = (byte)(playerStartXY+1);
                    dudCoord[1] = playerStartXY;

                    simpleArray_building[playerStartXY-1, playerStartXY].setInformation(0, Constants.doorID_right, Constants.interactableID_none, Constants.interactableID_none);
                    // simpleArray_building[playerStartXY+1, playerStartXY].setInformation(0, Constants.doorID_left, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY-1].setInformation(0, Constants.doorID_down, Constants.interactableID_none, Constants.interactableID_none);
                    simpleArray_building[playerStartXY, playerStartXY+1].setInformation(0, Constants.doorID_up, Constants.interactableID_none, Constants.interactableID_none);
                    break;
            }

            // simpleArray_building[dudCoord[0]-1, dudCoord[1]-1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0], dudCoord[1]-1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0]+1, dudCoord[1]-1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0]+1, dudCoord[1]].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0]+1, dudCoord[1]+1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0], dudCoord[1]+1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0]-1, dudCoord[1]+1].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            // simpleArray_building[dudCoord[0]-1, dudCoord[1]].setInformation(0, Constants.doorID_null, Constants.interactableID_none, Constants.interactableID_none);
            


            return simpleArray_building;
        }

        public SimpleRoom_Building resetArray_Path(byte newRoomX_relativeToBuildingArray, byte newRoomY_relativeToBuildingArray)
        {
            pathToBuildingX = newRoomX_relativeToBuildingArray - pathStartXY;
            pathToBuildingY = newRoomY_relativeToBuildingArray - pathStartXY;

            for (int indexX=0; indexX<arraySize_path; indexX++)
            {
                for (int indexY=0; indexY<arraySize_path; indexY++)
                {
                    simpleArray_path[indexX, indexY].clearRoom();
                    simpleArray_path[indexX, indexY].setHasntBeenTried(true);
                }
            }

            return simpleArray_path[pathStartXY, pathStartXY];
        }


        public void joinPathArrayElementsIntoBuildingArray()
        {
            // For this, we assume the path elements were created correctly and that 
            // all of the 'nonEmpty' elements lie within the builder array:

            int builderX;
            int builderY;

            for (int indexX=0; indexX<arraySize_path; indexX++)
            {
                for (int indexY=0; indexY<arraySize_path; indexY++)
                {
                    if (simpleArray_path[indexX, indexY].getIsNotEmpty())
                    {
                        builderX = indexX + pathToBuildingX;
                        builderY = indexY + pathToBuildingY;

                        simpleArray_building[builderX, builderY].copyDataFromOneToAnother(simpleArray_path[indexX, indexY]);

                        checkForADoor(simpleArray_path[indexX, indexY].getContainsDoor());
                        updateBoundingBox(builderX, builderY);
                    }
                }
            }
        }

        public byte[] getRandomCoordInBoundingBox(System.Random random_thread)
        {
            // return new byte[] {(byte)random_thread.Next(boundingBox_L, boundingBox_R), (byte)random_thread.Next(boundingBox_U, boundingBox_D)};

            do {
                newRandomCoord = new byte[] {(byte)random_thread.Next(boundingBox_L, boundingBox_R), (byte)random_thread.Next(boundingBox_U, boundingBox_D)};
            } while (newRandomCoord[0] == dudCoord[0] && newRandomCoord[1] == dudCoord[1]);
            
            return newRandomCoord;
        }

        public int[] convertPathCoordsToBuilderCoords(byte pathX, byte pathY)
        {
            return new int[] {pathX + pathToBuildingX, pathY + pathToBuildingY};
        }

        private void updateBoundingBox(int roomX_builder, int roomY_builder)
        {
            if (roomX_builder == 0) boundingBox_L = 0;
            else if (roomX_builder == arraySize_building-1) boundingBox_R = arraySize_building;
            else {
                if (roomX_builder <= boundingBox_L) boundingBox_L = (byte)(roomX_builder -1);
                if (roomX_builder >= boundingBox_R-1) boundingBox_R = (byte)(roomX_builder +2);
            }
            
            if (roomY_builder == 0) boundingBox_U = 0;
            else if (roomY_builder == arraySize_building-1) boundingBox_D = arraySize_building;
            else {
                if (roomY_builder <= boundingBox_U) boundingBox_U = (byte)(roomY_builder -1);
                if (roomY_builder >= boundingBox_D-1) boundingBox_D = (byte)(roomY_builder +2);
            }

            if (roomX_builder < roomBound_L) roomBound_L = (byte)roomX_builder;
            if (roomX_builder >= roomBound_R) roomBound_R = (byte)(roomX_builder+1);
            if (roomY_builder < roomBound_U) roomBound_U = (byte)roomY_builder;
            if (roomY_builder >= roomBound_D) roomBound_D = (byte)(roomY_builder+1);
        }

        private void checkForADoor(byte doorClearance)
        {
            switch (doorClearance)
            {
                case 1: 
                    numOfDoors_clearance1++;
                    break;
                    
                case 2: 
                    numOfDoors_clearance2++;
                    break;
                    
                case 3: 
                    numOfDoors_clearance3++;
                    break;
                    
                case 4: 
                    numOfDoors_clearance4++;
                    break;
                    
                case 5: 
                    numOfDoors_clearance5++;
                    break;
                    
                case 6: 
                    numOfDoors_clearance6++;
                    break;

                default:
                    break;
            }
        }


        public void incrementTheClearanceLevel()
        {
            currentClearanceLevel++;
        }

        public bool checkIfCoordIsAvailable(byte roomX, byte roomY)
        {
            if (roomX > 0 && roomX < arraySize_building-1 && roomY > 0 && roomY < arraySize_building-1)
            {
                if (roomX == dudCoord[0] && roomY == dudCoord[1]) return false;

                return !simpleArray_building[roomX, roomY].getIsNotEmpty() && getPathRoom_usingBuildingCoords(roomX, roomY).getCanARoomBePlacedHere();
            }

            return false;
        }



        public SimpleRoom_Building[,] getSimpleArray_building()
        {
            return simpleArray_building;
        }

        public SimpleRoom_Building[,] getSimpleArray_path()
        {
            return simpleArray_path;
        }

        public SimpleRoom_Building getPathRoom_usingBuildingCoords(byte builderX, byte builderY)
        {
            int pathX = builderX - pathToBuildingX;
            int pathY = builderY - pathToBuildingY;

            if (pathX < 0 || pathX >= arraySize_path || pathY < 0 || pathY >= arraySize_path)
                return null;

            return simpleArray_path[pathX, pathY];
        }


        public byte getRoomBound_L()
        {
            return roomBound_L;
        }

        public byte getRoomBound_R()
        {
            return roomBound_R;
        }

        public byte getRoomBound_U()
        {
            return roomBound_U;
        }

        public byte getRoomBound_D()
        {
            return roomBound_D;
        }

        public byte getCurrentClearanceLevel()
        {
            return currentClearanceLevel;
        }

        public byte getRandomClearanceLevel(System.Random random_thread)
        {
            //Temp:
            // return 0;

            // if (Random.Range(0, 5) != 0)
            if (random_thread.Next(0, 5) != 0)
                return 0;

            // byte randomRoom = (byte)Random.Range(1, 7);
            byte randomRoom = (byte)random_thread.Next(1, 7);

            if (randomRoom <= currentClearanceLevel) return randomRoom;
            
            return 0;
        }

        public short getNumOfDoors_clearance1()
        {
            return numOfDoors_clearance1;
        }

        public short getNumOfDoors_clearance2()
        {
            return numOfDoors_clearance2;
        }

        public short getNumOfDoors_clearance3()
        {
            return numOfDoors_clearance3;
        }

        public short getNumOfDoors_clearance4()
        {
            return numOfDoors_clearance4;
        }

        public short getNumOfDoors_clearance5()
        {
            return numOfDoors_clearance5;
        }

        public short getNumOfDoors_clearance6()
        {
            return numOfDoors_clearance6;
        }
    }
}