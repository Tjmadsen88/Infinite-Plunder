using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using SimplifiedLayoutBuilderSpace;
using UpscaledLayoutBuilderSpace;
using BoolArrayManagerSpace;

namespace PortTownSpace
{
    public static class PortTownPlacementGenerator
    {
        // private const float housePlacementDiameter = 40f;
        // private const byte maxNumberOfHouses = 30;
        // private const byte maxNumberOfHouses = 30;
        
        private const float houseSize_boundBox_max = 3f;
        // private const float houseSize_boundBox_max = 4f;
        private const float houseSize_front_min = 1f;
        private const float houseSize_front_max_add = 0.5f;
        private const float houseSize_side_min = 1.5f;
        private const float houseSize_side_max_add = 1f;
        // private const float houseSize_top_min = 0.5f;
        // private const float houseSize_top_max_add = 1f;
        private const float houseSize_top_min = 0.5f;
        private const float houseSize_top_max_add = 0.5f;

        private const float houseFootprintRadius = 3f;
        private const float houseMinHeight = 1.5f;

        // private const byte colorVal_min = 40;
        // private const byte colorVal_max = 150;
        private const byte colorVal_min = 40;
        private const byte colorVal_max = 200;


        public static PortTownReturnPacket generatePortTownInformation(SimplifiedLayoutReturnPacket simplifiedLayout, UpscaledLayoutReturnPacket upscaledLayout, 
                                                                        float roomWidthHeight, int numOfVertsPerEdge, byte sizeOfExplorableArea, System.Random random_forThread)
        {
            byte targetNumberOfPortTowns = getTargetNumberOfPortTownsByAreaSize(sizeOfExplorableArea, random_forThread);

            SimpleRoom_Output[,] simplifiedRoomArray = simplifiedLayout.getSimplifiedRoomArray();
            PortTownReturnPacket portTownPacket = new PortTownReturnPacket(targetNumberOfPortTowns);

            byte simpleLayoutWidth = (byte)simplifiedRoomArray.GetLength(0);
            byte simpleLayoutHeight = (byte)simplifiedRoomArray.GetLength(1);

            byte roomX;
            byte roomY;
            byte breaker;

            // First, place a port town at the starting space:
            roomX = simplifiedLayout.getPlayerStartingLocation()[0];
            roomY = simplifiedLayout.getPlayerStartingLocation()[1];
            portTownPacket.addNewPortTownData(createNewPortTownAtLocation(simplifiedLayout, upscaledLayout, roomX, roomY, roomWidthHeight, numOfVertsPerEdge, random_forThread));

            // Then, build more as neccessary:
            for (byte index=0; index<targetNumberOfPortTowns-1; index++)
            {
                breaker = 50;

                do {
                    roomX = (byte)random_forThread.Next(0, simpleLayoutWidth);
                    roomY = (byte)random_forThread.Next(0, simpleLayoutHeight);

                    // If the town can't be placed in 50 attempts, chances are none of the later ones will either.
                    // So we give up and return the port towns before the target number is reached.
                    if (--breaker == 0 && index > 0) return portTownPacket; 
                } while (!checkIfRoomCanHavePortTown(simplifiedRoomArray[roomX, roomY], portTownPacket, roomX, roomY, simplifiedLayout.getFinalTreasureLocation()));


                portTownPacket.addNewPortTownData(createNewPortTownAtLocation(simplifiedLayout, upscaledLayout, roomX, roomY, roomWidthHeight, numOfVertsPerEdge, random_forThread));
            }

            return portTownPacket;
        }

        public static PortTownReturnPacket generatePortTownInformation_DemoVer(SimplifiedLayoutReturnPacket simplifiedLayout, UpscaledLayoutReturnPacket upscaledLayout, 
                                                                                float roomWidthHeight, int numOfVertsPerEdge)
        {
            PortTownReturnPacket portTownPacket = new PortTownReturnPacket(1);
            
            PortTownIndividualPacket portTownIndividual = new PortTownIndividualPacket();
            portTownIndividual.setAllData_forDemo();
            
            portTownPacket.addNewPortTownData(portTownIndividual);

            return portTownPacket;
        }


        private static byte getTargetNumberOfPortTownsByAreaSize(byte sizeOfExplorableArea, System.Random random_forThread)
        {
            if (sizeOfExplorableArea > 0)
            {
                return (byte)random_forThread.Next(sizeOfExplorableArea, sizeOfExplorableArea+2);
            } else return 1;
        }


        // Check to see if the simple room has one open door and one closed door.
        private static bool checkIfRoomCanHavePortTown(SimpleRoom_Output simpleRoom, PortTownReturnPacket portTownPacket, byte xCoord, byte yCoord, byte[] finalTreasureCoords)
        {
            // First, check if the space has water, and is not too close to another port town:
            if (simpleRoom.getIsNotEmpty() && !portTownPacket.checkIfCoordinateIsTooCloseToAnotherPortTown(xCoord, yCoord) &&
                Mathf.Abs(xCoord - finalTreasureCoords[0]) > 1 && Mathf.Abs(yCoord - finalTreasureCoords[1]) > 1)
            {
                for (byte index=0; index<4; index++)
                {
                    // If the room hasSomething (water), and has a wall that is closed...
                    // then there is space to place a port town:
                    if (!simpleRoom.getWallIsOpen(index)) return true;
                }
            }

            return false;
        }


        private static PortTownIndividualPacket createNewPortTownAtLocation(SimplifiedLayoutReturnPacket simplifiedLayout, UpscaledLayoutReturnPacket upscaledLayout, 
                                                                        byte roomX, byte roomY, float roomWidthHeight, int numOfVertsPerEdge, System.Random random_forThread)
        {
            PortTownIndividualPacket portTownIndividual = new PortTownIndividualPacket();
            portTownIndividual.setPortTownPierCoords_simple(roomX, roomY);

            Vector2 waterPosition = new Vector2((roomX + 0.5f) * roomWidthHeight, (roomY + 0.5f) * -roomWidthHeight);
            Vector2 landPosition = findPortTownPierLandPoint(simplifiedLayout.getSimplifiedRoomArray()[roomX, roomY], waterPosition, roomWidthHeight, random_forThread);

            portTownIndividual.setPortTownYRotation(Vector2.SignedAngle(waterPosition - landPosition, Vector2.up));

            Vector2 stepAmount = (landPosition - waterPosition).normalized * Constants.shipMoveSpeed;

            do {
                waterPosition += stepAmount;
            } while (BoolArrayManager.checkForConflict_V2_noDoors(upscaledLayout.getBoolArray(), waterPosition));

            stepAmount = stepAmount / Constants.shipMoveSpeed * Constants.portTownDistanceFromLand;
            waterPosition -= stepAmount;

            portTownIndividual.setPortTownPierCoords_upscaled(waterPosition.x, waterPosition.y);

            addHousesAroundPier(portTownIndividual, upscaledLayout.getLandVertexHeights(), random_forThread);

            return portTownIndividual;
        }


        private static Vector2 findPortTownPierLandPoint(SimpleRoom_Output simpleRoom, Vector2 centerPoint, float roomWidthHeight, System.Random random_forThread)
        {
            byte switchByte = 0;

            if (simpleRoom.getWallIsOpen(Constants.doorID_left)) switchByte += 1;
            if (simpleRoom.getWallIsOpen(Constants.doorID_up)) switchByte += 2;
            if (simpleRoom.getWallIsOpen(Constants.doorID_down)) switchByte += 4;
            if (simpleRoom.getWallIsOpen(Constants.doorID_right)) switchByte += 8;

            switch (switchByte)
            {
                case 1: // l open
                    switch(random_forThread.Next(0, 5))
                    {
                        case 0: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                        case 1: return centerPoint + new Vector2(0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); // ur
                        case 2: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                        case 3: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0.5f * -roomWidthHeight);  // dr
                        default: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                    }

                case 2: // u open
                    switch(random_forThread.Next(0, 5))
                    {
                        case 0: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                        case 1: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0.5f * -roomWidthHeight);  // dr
                        case 2: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                        case 3: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0.5f * -roomWidthHeight); // dl
                        default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    }

                case 3: // lu open
                    return centerPoint + new Vector2(0.5f * roomWidthHeight, 0.5f * -roomWidthHeight);  // dr
                    // switch(random_forThread.Next(0, 3))
                    // {
                    //     case 0: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                    //     case 1: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0.5f * -roomWidthHeight);  // dr
                    //     default: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                    // }

                case 4: // d open
                    switch(random_forThread.Next(0, 5))
                    {
                        case 0: return centerPoint + new Vector2(-0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); //ul
                        case 1: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                        case 2: return centerPoint + new Vector2(0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); // ur
                        case 3: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                        default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    }

                case 5: // ld open
                    return centerPoint + new Vector2(0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); // ur
                    // switch(random_forThread.Next(0, 3))
                    // {
                    //     case 0: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                    //     case 1: return centerPoint + new Vector2(0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); // ur
                    //     default: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                    // }

                case 6: // ud open
                    switch(random_forThread.Next(0, 2))
                    {
                        case 0: return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r
                        default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    }

                case 7: // lud open
                    return centerPoint + new Vector2(0.5f * roomWidthHeight, 0f);                       // r

                case 8: // r open
                    switch(random_forThread.Next(0, 5))
                    {
                        case 0: return centerPoint + new Vector2(-0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); //ul
                        case 1: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                        case 2: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                        case 3: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0.5f * -roomWidthHeight); // dl
                        default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    }

                case 9: // lr open
                    switch(random_forThread.Next(0, 2))
                    {
                        case 0: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                        default: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                    }

                case 10: // ur open
                    return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0.5f * -roomWidthHeight); // dl
                    // switch(random_forThread.Next(0, 3))
                    // {
                    //     case 0: return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d
                    //     case 1: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0.5f * -roomWidthHeight); // dl
                    //     default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    // }

                case 11: // lur open
                    return centerPoint + new Vector2(0f, 0.5f * -roomWidthHeight);                      // d

                case 12: // dr open
                    return centerPoint + new Vector2(-0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); //ul
                    // switch(random_forThread.Next(0, 3))
                    // {
                    //     case 0: return centerPoint + new Vector2(-0.5f * roomWidthHeight, -0.5f * -roomWidthHeight); //ul
                    //     case 1: return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
                    //     default: return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l
                    // }

                case 13: // ldr open
                    return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u

                case 14: // udr open
                    return centerPoint + new Vector2(-0.5f * roomWidthHeight, 0f);                     // l

                        // case 0: // all doors closed... should never happen
                default: //case 15: // all doors open... should never happen
                    return centerPoint + new Vector2(0f, -0.5f * -roomWidthHeight);                     // u
            }
        }


        private static void addHousesAroundPier(PortTownIndividualPacket portTownIndividual, float[,] heightArray, System.Random random_forThread)
        {
            float townSize = 0.5f + (float)(random_forThread.NextDouble()) * 1.5f;
            float housePlacementDiameter = 40f * townSize;
            byte maxNumberOfHouses = (byte)(30 * townSize * townSize * (0.5f + random_forThread.NextDouble()));

            Vector3[] housePositions = new Vector3[maxNumberOfHouses];
            Vector3[] houseScales = new Vector3[maxNumberOfHouses];
            float[] houseRotations = new float[maxNumberOfHouses];
            byte firstOpenSlotInArray = 0;

            Vector2 houseCenter = Vector2.zero;
            float houseHeight;
            float houseScale_front;
            float houseScale_side;
            float houseScale_top;
            
            byte cornerIndexOffset = getCornerIndexOffset(portTownIndividual);


            for (byte index=0; index<maxNumberOfHouses; index++)
            {
                do {
                    houseCenter.x = (float)(random_forThread.NextDouble() -0.5);
                    houseCenter.y = (float)(random_forThread.NextDouble() -0.5);
                } while ((houseCenter.x * houseCenter.x) + (houseCenter.y * houseCenter.y) > 0.25f);

                houseCenter.x = houseCenter.x * housePlacementDiameter + portTownIndividual.getPortTownCoords_upscaled()[0];
                houseCenter.y = houseCenter.y * housePlacementDiameter + portTownIndividual.getPortTownCoords_upscaled()[1];

                houseHeight = getLandHeightAtCoord(houseCenter, heightArray);

                if (checkIfHouseCanBePlacedHere(houseCenter, houseHeight, housePositions, firstOpenSlotInArray))
                {
                    houseHeight = Mathf.Max(getLandHeightAtCoord(houseCenter + Constants.walls_hittestVectors[0+cornerIndexOffset] * houseSize_boundBox_max, heightArray), houseHeight);
                    houseHeight = Mathf.Max(getLandHeightAtCoord(houseCenter + Constants.walls_hittestVectors[4+cornerIndexOffset] * houseSize_boundBox_max, heightArray), houseHeight);
                    houseHeight = Mathf.Max(getLandHeightAtCoord(houseCenter + Constants.walls_hittestVectors[8+cornerIndexOffset] * houseSize_boundBox_max, heightArray), houseHeight);
                    houseHeight = Mathf.Max(getLandHeightAtCoord(houseCenter + Constants.walls_hittestVectors[12+cornerIndexOffset] * houseSize_boundBox_max, heightArray), houseHeight);

                    housePositions[firstOpenSlotInArray] = new Vector3(houseCenter.x, houseHeight-0.5f, houseCenter.y);

                    houseScale_front = houseSize_front_min + (float)(random_forThread.NextDouble()) * houseSize_front_max_add;
                    houseScale_side = houseSize_side_min + (float)(random_forThread.NextDouble()) * houseSize_side_max_add;
                    houseScale_top = houseSize_top_min + (float)(random_forThread.NextDouble()) * houseSize_top_max_add;

                    if (random_forThread.Next(0, 2) == 0)
                    {
                        houseScales[firstOpenSlotInArray] = new Vector3(houseScale_front, houseScale_top, houseScale_side);
                    } else {
                        houseScales[firstOpenSlotInArray] = new Vector3(houseScale_side, houseScale_top, houseScale_front);
                    }
                    

                    // houseRotations[firstOpenSlotInArray] = portTownIndividual.getPortTownYRotation() + random_forThread.Next(0, 4) * 90f;
                    // houseRotations[firstOpenSlotInArray] = Mathf.Atan((houseCenter.y - portTownIndividual.getPortTownCoords_upscaled()[1]) / 
                    //                                                     (houseCenter.x - portTownIndividual.getPortTownCoords_upscaled()[0]))
                    //                                                      * -180f / Mathf.PI + 90f
                    //                                                      + (float)((random_forThread.NextDouble() -0.5)) * 15f;
                    houseRotations[firstOpenSlotInArray] = portTownIndividual.getPortTownYRotation() + random_forThread.Next(0, 4) * 90f
                                                                         + (float)((random_forThread.NextDouble() -0.5)) * 20f;

                    firstOpenSlotInArray++;
                }
            }

            Vector3[] housePositions_actual = new Vector3[firstOpenSlotInArray];
            Vector3[] houseScales_actual = new Vector3[firstOpenSlotInArray];
            float[] houseRotations_actual = new float[firstOpenSlotInArray];

            Color32[] roofColors = new Color32[firstOpenSlotInArray];
            Color32[] wallColors = new Color32[firstOpenSlotInArray];
            Color32 roofColor_base = generateRandomRoofColor_base(random_forThread);

            for (byte index=0; index<firstOpenSlotInArray; index++)
            {
                housePositions_actual[index] = housePositions[index];
                houseScales_actual[index] = houseScales[index];
                houseRotations_actual[index] = houseRotations[index];

                roofColors[index] = generateRandomRoofColor_offset(random_forThread, roofColor_base);
                wallColors[index] = generateRandomWallColor(random_forThread);
            }

            // portTownIndividual.setHouseData(housePositions_actual, houseScales_actual, houseRotations_actual, generateRandomRoofColor(random_forThread));
            portTownIndividual.setHouseData(housePositions_actual, houseScales_actual, houseRotations_actual, roofColors, wallColors);
        }


        private static byte getCornerIndexOffset(PortTownIndividualPacket portTownIndividual)
        {
            switch ((short)Mathf.Abs(portTownIndividual.getPortTownYRotation()))
            {
                case 0:
                case 1:
                case 89:
                case 90:
                case 91:
                case 269:
                case 270:
                case 271:
                case 359:
                case 360:
                    return 2;

                default:
                    return 0;
            }
        }


        private static bool checkIfHouseCanBePlacedHere(Vector2 houseCenter, float centerHeight, Vector3[] housePositions, byte firstOpenSlotInArray)
        {
            if (centerHeight < houseMinHeight)
                return false;
                
            for (byte index=0; index<firstOpenSlotInArray; index++)
            {
                if (Mathf.Abs(houseCenter.x - housePositions[index].x) < houseSize_boundBox_max &&
                    Mathf.Abs(houseCenter.y - housePositions[index].z) < houseSize_boundBox_max)
                    return false;
            }

            return true;
        }


        private static float getLandHeightAtCoord(Vector2 coord, float[,] heightArray)
        {
            return TerrainHeightLerpingClass.getHeight(heightArray, heightArray.GetLength(0), heightArray.GetLength(1), Constants.vertDistances, coord.x, -coord.y);
        }


        private static Color32 generateRandomRoofColor_base(System.Random random_forThread)
        {
            return new Color32((byte)random_forThread.Next(colorVal_min, colorVal_max), (byte)random_forThread.Next(colorVal_min, colorVal_max), (byte)random_forThread.Next(colorVal_min, colorVal_max), 255);
        }


        private static Color32 generateRandomRoofColor_offset(System.Random random_forThread, Color32 baseColor)
        {
            int brightnessOffset = random_forThread.Next(-25, 26);

            byte redColor = (byte)(baseColor.r + brightnessOffset + random_forThread.Next(-5, 6));
            byte greenColor = (byte)(baseColor.g + brightnessOffset + random_forThread.Next(-5, 6));
            byte blueColor = (byte)(baseColor.b + brightnessOffset + random_forThread.Next(-5, 6));

            return new Color32(redColor, greenColor, blueColor, 255);
        }


        private static Color32 generateRandomWallColor(System.Random random_forThread)
        {
            float colorIntensity = (float)random_forThread.NextDouble();
            int greyIntensity = random_forThread.Next(100, 200);

            byte redColor = (byte)(random_forThread.Next(150, 190) * colorIntensity + (1f-colorIntensity) * greyIntensity);
            byte greenColor = (byte)(random_forThread.Next(110, 150) * colorIntensity + (1f-colorIntensity) * greyIntensity);
            byte blueColor = (byte)(random_forThread.Next(60, 100) * colorIntensity + (1f-colorIntensity) * greyIntensity);

            return new Color32(redColor, greenColor, blueColor, 255);
        }

    }
}