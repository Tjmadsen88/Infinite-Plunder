using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using SimplifiedLayoutBuilderSpace;
using UpscaledRoomTemplatesSpace;

namespace UpscaledLayoutBuilderSpace
{
    public static class UpscaledLayoutBuilder
    {
        private const float maxHeight = 3f;
        private const float temp_heightMultiplier = 1.5f;

        // private const float maxPerlin_1 = 0.5f;     // Be careful when changing these!
        // private const float perlinWidth_1 = 0.05f;
        // private const float maxPerlin_2 = 6f;
        // private const float perlinWidth_2 = 0.01f;
        private const float maxPerlin_1 = 0.5f;     // Be careful when changing these!
        private const float perlinWidth_1 = 0.045f;
        private const float maxPerlin_2 = 6f;
        private const float perlinWidth_2 = 0.01f;



        public static UpscaledLayoutReturnPacket upscaleLayout_Game(SimplifiedLayoutReturnPacket simplifiedLayout, float roomWidthHeight, int numOfVertsPerEdge, System.Random random_thread)
        {
            UpscaledLayoutReturnPacket returnPacket = new UpscaledLayoutReturnPacket();

            float[,] heightArray = convertSimpleLayoutToHeightArray_preNoise(simplifiedLayout, numOfVertsPerEdge);
            returnPacket.setBoolArray_noNoise(convertHeightArrayToBoolArray(heightArray, numOfVertsPerEdge, 0.5f));
            addNoiseToHeightArray(heightArray, random_thread);

            returnPacket.setLandVertexHeights(heightArray);
            returnPacket.setBoolArray(convertHeightArrayToBoolArray(heightArray, numOfVertsPerEdge, 0));

            // TEMP:
            // bool[,] boolArray = convertHeightArrayToBoolArray(heightArray, numOfVertsPerEdge, 0);
            // returnPacket.setLandVertexHeights(temp_convertBoolArrayToHeightArray(boolArray, numOfVertsPerEdge));

            return returnPacket;
        }

        public static UpscaledLayoutReturnPacket upscaleLayout_Title(SimplifiedLayoutReturnPacket simplifiedLayout, float roomWidthHeight, int numOfVertsPerEdge, System.Random random_thread)
        {
            UpscaledLayoutReturnPacket returnPacket = new UpscaledLayoutReturnPacket();

            float[,] heightArray = convertSimpleLayoutToHeightArray_preNoise_noBorders(simplifiedLayout, numOfVertsPerEdge);
            addNoiseToHeightArray(heightArray, random_thread);

            returnPacket.setLandVertexHeights(heightArray);

            // If I want to print out more room templetes, I do it here:
            // float[,] tempHeightArray = buildHeightRoom_LUDR_ooxx(numOfVertsPerEdge);
            // UpscaledRoomTemplates.temp_printRoomTemplateToConsole(tempHeightArray);

            return returnPacket;
        }

        public static UpscaledLayoutReturnPacket upscaleLayout_Demo(SimplifiedLayoutReturnPacket simplifiedLayout, float roomWidthHeight, int numOfVertsPerEdge)
        {
            UpscaledLayoutReturnPacket returnPacket = new UpscaledLayoutReturnPacket();

            float[,] heightArray = convertSimpleLayoutToHeightArray_preNoise(simplifiedLayout, numOfVertsPerEdge);
            returnPacket.setBoolArray_noNoise(convertHeightArrayToBoolArray(heightArray, numOfVertsPerEdge, 0.5f));
            addNoiseToHeightArray_predefinedOffsets(heightArray, 0.13456f, 0.36548f);

            returnPacket.setLandVertexHeights(heightArray);
            returnPacket.setBoolArray(convertHeightArrayToBoolArray(heightArray, numOfVertsPerEdge, 0));

            return returnPacket;
        }





        private static bool[,] convertHeightArrayToBoolArray(float[,] heightArray, int numOfVertsPerEdge, float thresholdVal)
        {
            int horizSize = heightArray.GetLength(0) -3*numOfVertsPerEdge;
            int vertiSize = heightArray.GetLength(1) -3*numOfVertsPerEdge;

            bool[,] returnArray = new bool[horizSize, vertiSize];

            for (int indexX=0; indexX<horizSize; indexX++)
            {
                for (int indexY=0; indexY<vertiSize; indexY++)
                {
                    returnArray[indexX, indexY] = heightArray[indexX + 2*numOfVertsPerEdge, indexY + 2*numOfVertsPerEdge] < thresholdVal;
                }
            }

            return returnArray;
        }

        // private static float[,] temp_convertBoolArrayToHeightArray(bool[,] boolArray, int numOfVertsPerEdge)
        // {
        //     int horizSize = boolArray.GetLength(0) -3*numOfVertsPerEdge;
        //     int vertiSize = boolArray.GetLength(1) -3*numOfVertsPerEdge;

        //     float[,] returnArray = new float[horizSize, vertiSize];

        //     for (int indexX=0; indexX<horizSize; indexX++)
        //     {
        //         for (int indexY=0; indexY<vertiSize; indexY++)
        //         {
        //             if (boolArray[indexX, indexY])
        //                 returnArray[indexX, indexY] = -5f;
        //             else returnArray[indexX, indexY] = 5f;
        //         }
        //     }

        //     return returnArray;
        // }






        private static void addSmallerHeightRoomToTotalArray(float[,] biggerArray, float[,] smallerArray, int roomStartX, int roomStartY)
        {
            for (int indexX=0; indexX<smallerArray.GetLength(0); indexX++)
            {
                for (int indexY=0; indexY<smallerArray.GetLength(0); indexY++)
                {
                    biggerArray[roomStartX + indexX, roomStartY + indexY] = smallerArray[indexX, indexY];
                }
            }
        }

        private static float[,] convertSimpleLayoutToHeightArray_preNoise(SimplifiedLayoutReturnPacket simplifiedLayout, int numOfVertsPerEdge)
        {
            int returnArrayWidth = (simplifiedLayout.getAreaWidth() +3) * numOfVertsPerEdge;
            int returnArrayHeight = (simplifiedLayout.getAreaHeight() +3) * numOfVertsPerEdge;

            float[,] totalArea = new float[returnArrayWidth, returnArrayHeight];
            float[,] smallerRoom;

            SimpleRoom_Output currentRoom;

            for (int indexX=0; indexX<simplifiedLayout.getAreaWidth(); indexX++)
            {
                for (int indexY=0; indexY<simplifiedLayout.getAreaHeight(); indexY++)
                {
                    currentRoom = simplifiedLayout.getSimplifiedRoomArray()[indexX, indexY];
                    smallerRoom = buildHeightRoom_general_roomTemplates(numOfVertsPerEdge, currentRoom);
                    
                    addSmallerHeightRoomToTotalArray(totalArea, smallerRoom, (indexX+2)*numOfVertsPerEdge, (indexY+2)*numOfVertsPerEdge);
                }
            }

            // Fill the edges with maxValues:
            // Horizontal first:
            for (int indexX=0; indexX<returnArrayWidth; indexX++)
            {
                for (int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    totalArea[indexX, indexY] = maxHeight;
                    totalArea[indexX, indexY+numOfVertsPerEdge] = maxHeight;
                    totalArea[indexX, returnArrayHeight-numOfVertsPerEdge+indexY] = maxHeight;
                }
            }
            
            // Then vertical:
            for (int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for (int indexY=numOfVertsPerEdge; indexY<returnArrayHeight-numOfVertsPerEdge; indexY++)
                {
                    totalArea[indexX, indexY] = maxHeight;
                    totalArea[indexX+numOfVertsPerEdge, indexY] = maxHeight;
                    totalArea[returnArrayWidth-numOfVertsPerEdge+indexX, indexY] = maxHeight;
                }
            }

            return totalArea;
        }


        private static float[,] convertSimpleLayoutToHeightArray_preNoise_noBorders(SimplifiedLayoutReturnPacket simplifiedLayout, int numOfVertsPerEdge)
        {
            int returnArrayWidth = (simplifiedLayout.getAreaWidth()) * numOfVertsPerEdge;
            int returnArrayHeight = (simplifiedLayout.getAreaHeight()) * numOfVertsPerEdge;

            float[,] totalArea = new float[returnArrayWidth, returnArrayHeight];
            float[,] smallerRoom;

            SimpleRoom_Output currentRoom;

            for (int indexX=0; indexX<simplifiedLayout.getAreaWidth(); indexX++)
            {
                for (int indexY=0; indexY<simplifiedLayout.getAreaHeight(); indexY++)
                {
                    currentRoom = simplifiedLayout.getSimplifiedRoomArray()[indexX, indexY];
                    smallerRoom = buildHeightRoom_general_roomTemplates(numOfVertsPerEdge, currentRoom);
                    
                    addSmallerHeightRoomToTotalArray(totalArea, smallerRoom, (indexX)*numOfVertsPerEdge, (indexY)*numOfVertsPerEdge);
                }
            }

            return totalArea;
        }
        

        private static void addNoiseToHeightArray(float[,] heightArray, System.Random random_thread)
        {
            // float perlinOffsetX = Random.Range(0f, 200f);
            // float perlinOffsetY = Random.Range(0f, 200f);
            float perlinOffsetX = (float)(200f * random_thread.NextDouble());
            float perlinOffsetY = (float)(200f * random_thread.NextDouble());

            addNoiseToHeightArray_predefinedOffsets(heightArray, perlinOffsetX, perlinOffsetY);
        }

        private static void addNoiseToHeightArray_predefinedOffsets(float[,] heightArray, float perlinOffsetX, float perlinOffsetY)
        {
            for (int indexX=0; indexX<heightArray.GetLength(0); indexX++)
            {
                for (int indexY=0; indexY<heightArray.GetLength(1); indexY++)
                {
                    heightArray[indexX, indexY] += (maxPerlin_1 * (Mathf.PerlinNoise((indexX + perlinOffsetX) * 
                                                    perlinWidth_1, (indexY + perlinOffsetY) * perlinWidth_1 ) - 0.5f) * 2f)
                                                    * maxPerlin_2 * (Mathf.PerlinNoise((indexX + perlinOffsetX) * 
                                                    perlinWidth_2, (indexY + perlinOffsetY) * perlinWidth_2 ) + 1.0f) / 2f;

                    if(heightArray[indexX, indexY] < 0)
                    {
                        heightArray[indexX, indexY] *= 0.75f * maxPerlin_2 * Mathf.PerlinNoise((indexX + perlinOffsetX) * 
                                                    perlinWidth_2, (indexY + perlinOffsetY) * perlinWidth_2 );
                    }
                    
                    //heightArray[indexX, indexY] -= 0.5f;
                    
                }
            }
        }




        
        private static float[,] buildHeightRoom_general_roomTemplates(int numOfVertsPerEdge, SimpleRoom_Output currentRoom)
        {
            byte switchByte_doors = 0;
            byte switchByte_corners = 0;

            if (currentRoom.getWallIsOpen(Constants.doorID_left))
                switchByte_doors += 1;
            if (currentRoom.getWallIsOpen(Constants.doorID_up))
                switchByte_doors += 2;
            if (currentRoom.getWallIsOpen(Constants.doorID_down))
                switchByte_doors += 4;
            if (currentRoom.getWallIsOpen(Constants.doorID_right))
                switchByte_doors += 8;

            if (currentRoom.getCornerIsOpen(Constants.cornerID_ul)) switchByte_corners += 1;
            if (currentRoom.getCornerIsOpen(Constants.cornerID_ur)) switchByte_corners += 2;
            if (currentRoom.getCornerIsOpen(Constants.cornerID_dl)) switchByte_corners += 4;
            if (currentRoom.getCornerIsOpen(Constants.cornerID_dr)) switchByte_corners += 8;

            switch (switchByte_doors)
            {
                case 0: // no doors are connected:
                    return UpscaledRoomTemplates.roomTemplate_noDoorsOpen;
                case 1: // Upper left
                    return UpscaledRoomTemplates.roomTemplate_L;
                case 2: // Upper right
                    return UpscaledRoomTemplates.roomTemplate_U;
                case 3: // Upper left and Upper right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 2: // Upper right
                        case 4: // Lower left
                        case 6: // Upper right and lower left
                        case 8: // Lower right
                        case 10: // Upper right and lower right
                        case 12: // Lower left and lower right
                        case 14: // Upper right, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LU_c;
                        case 1: // Upper left
                        case 3: // Upper left and Upper right
                        case 5: // Upper left and lower left
                        case 7: // Upper left, upper right, and lower left
                        case 9: // Upper left and lower right
                        case 11: // Upper left, upper right and lower right
                        case 13: // Upper left, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LU_o;
                    }
                case 4: // Lower left
                    return UpscaledRoomTemplates.roomTemplate_D;
                case 5: // Upper left and lower left
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 1: // Upper left
                        case 2: // Upper right
                        case 3: // Upper left and Upper right
                        case 8: // Lower right
                        case 9: // Upper left and lower right
                        case 10: // Upper right and lower right
                        case 11: // Upper left, upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LD_c;
                        case 4: // Lower left
                        case 5: // Upper left and lower left
                        case 6: // Upper right and lower left
                        case 7: // Upper left, upper right, and lower left
                        case 12: // Lower left and lower right
                        case 13: // Upper left, lower left and lower right
                        case 14: // Upper right, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LD_o;
                    }
                case 6: // Upper right and lower left
                    return UpscaledRoomTemplates.roomTemplate_UD;
                case 7: // Upper left, upper right, and lower left
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 2: // Upper right
                        case 8: // Lower right
                        case 10: // Upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUD_cxcx;
                        case 1: // Upper left
                        case 3: // Upper left and Upper right
                        case 9: // Upper left and lower right
                        case 11: // Upper left, upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUD_oxcx;
                        case 4: // Lower left
                        case 6: // Upper right and lower left
                        case 12: // Lower left and lower right
                        case 14: // Upper right, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUD_cxox;
                        // case 5: // Upper left and lower left
                        // case 7: // Upper left, upper right, and lower left
                        // case 13: // Upper left, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LUD_oxox;
                    }
                case 8: // Lower right
                    return UpscaledRoomTemplates.roomTemplate_R;
                case 9: // Upper left and lower right
                    return UpscaledRoomTemplates.roomTemplate_LR;
                case 10: // Upper right and lower right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 1: // Upper left
                        case 4: // Lower left
                        case 5: // Upper left and lower left
                        case 8: // Lower right
                        case 9: // Upper left and lower right
                        case 12: // Lower left and lower right
                        case 13: // Upper left, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_UR_c;
                        case 2: // Upper right
                        case 3: // Upper left and Upper right
                        case 6: // Upper right and lower left
                        case 7: // Upper left, upper right, and lower left
                        case 10: // Upper right and lower right
                        case 11: // Upper left, upper right and lower right
                        case 14: // Upper right, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_UR_o;
                    }
                case 11: // Upper left, upper right and lower right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 4: // Lower left
                        case 8: // Lower right
                        case 12: // Lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUR_ccxx;
                        case 1: // Upper left
                        case 5: // Upper left and lower left
                        case 9: // Upper left and lower right
                        case 13: // Upper left, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUR_ocxx;
                        case 2: // Upper right
                        case 6: // Upper right and lower left
                        case 10: // Upper right and lower right
                        case 14: // Upper right, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUR_coxx;
                        // case 3: // Upper left and Upper right
                        // case 7: // Upper left, upper right, and lower left
                        // case 11: // Upper left, upper right and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LUR_ooxx;
                    }
                case 12: // Lower left and lower right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 1: // Upper left
                        case 2: // Upper right
                        case 3: // Upper left and Upper right
                        case 4: // Lower left
                        case 5: // Upper left and lower left
                        case 6: // Upper right and lower left
                        case 7: // Upper left, upper right, and lower left
                            return UpscaledRoomTemplates.roomTemplate_DR_c;
                        // case 8: // Lower right
                        // case 9: // Upper left and lower right
                        // case 10: // Upper right and lower right
                        // case 11: // Upper left, upper right and lower right
                        // case 12: // Lower left and lower right
                        // case 13: // Upper left, lower left and lower right
                        // case 14: // Upper right, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_DR_o;
                    }
                case 13: // Upper left, lower left and lower right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 1: // Upper left
                        case 2: // Upper right
                        case 3: // Upper left and Upper right
                            return UpscaledRoomTemplates.roomTemplate_LDR_xxcc;
                        case 4: // Lower left
                        case 5: // Upper left and lower left
                        case 6: // Upper right and lower left
                        case 7: // Upper left, upper right, and lower left
                            return UpscaledRoomTemplates.roomTemplate_LDR_xxoc;
                        case 8: // Lower right
                        case 9: // Upper left and lower right
                        case 10: // Upper right and lower right
                        case 11: // Upper left, upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LDR_xxco;
                        // case 12: // Lower left and lower right
                        // case 13: // Upper left, lower left and lower right
                        // case 14: // Upper right, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LDR_xxoo;
                    }
                case 14: // Upper right, lower left and lower right
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                        case 1: // Upper left
                        case 4: // Lower left
                        case 5: // Upper left and lower left
                            return UpscaledRoomTemplates.roomTemplate_UDR_xcxc;
                        case 2: // Upper right
                        case 3: // Upper left and Upper right
                        case 6: // Upper right and lower left
                        case 7: // Upper left, upper right, and lower left
                            return UpscaledRoomTemplates.roomTemplate_UDR_xoxc;
                        case 8: // Lower right
                        case 9: // Upper left and lower right
                        case 12: // Lower left and lower right
                        case 13: // Upper left, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_UDR_xcxo;
                        //case 10: // Upper right and lower right
                        //case 11: // Upper left, upper right and lower right
                        //case 14: // Upper right, lower left and lower right
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_UDR_xoxo;
                    }
                default: // All four open
                    switch (switchByte_corners)
                    {
                        case 0: // all corners closed:
                            return UpscaledRoomTemplates.roomTemplate_LUDR_cccc;
                        case 1: // Upper left
                            return UpscaledRoomTemplates.roomTemplate_LUDR_occc;
                        case 2: // Upper right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_cocc;
                        case 3: // Upper left and Upper right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_oocc;
                        case 4: // Lower left
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ccoc;
                        case 5: // Upper left and lower left
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ococ;
                        case 6: // Upper right and lower left
                            return UpscaledRoomTemplates.roomTemplate_LUDR_cooc;
                        case 7: // Upper left, upper right, and lower left
                            return UpscaledRoomTemplates.roomTemplate_LUDR_oooc;
                        case 8: // Lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ccco;
                        case 9: // Upper left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_occo;
                        case 10: // Upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_coco;
                        case 11: // Upper left, upper right and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ooco;
                        case 12: // Lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ccoo;
                        case 13: // Upper left, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_ocoo;
                        case 14: // Upper right, lower left and lower right
                            return UpscaledRoomTemplates.roomTemplate_LUDR_cooo;
                        default: // All four open
                            return UpscaledRoomTemplates.roomTemplate_LUDR_oooo;
                    }
            }
        }


        // These are the room template builders. I should keep them around in case I want to modify the templates...

        // -----------------------------------------------------------------------------------------------------------
        // ---------------- Template Builders begin: --------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------
        /*

        // No doors open:
        private static float[,] buildHeightRoom_noDoorsOpen(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = maxHeight;
                }
            }

            return standaloneRoom;
        }

        // One door open:
        private static float[,] buildHeightRoom_L(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialCenter(indexX, indexY, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_U(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialCenter(indexX, indexY, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_D(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY >= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialCenter(indexX, indexY, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }

        private static float[,] buildHeightRoom_R(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX >= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialCenter(indexX, indexY, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }

        // Two doors open
        // Opposite sides:
        private static float[,] buildHeightRoom_LR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_UD(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }

        // Touching sides:
        private static float[,] buildHeightRoom_LU(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc(indexX, indexY, 0, 0, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_UR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_DR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LD(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }

        // Three open doors:
        private static float[,] buildHeightRoom_LUD(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX >= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LUR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY >= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_UDR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LDR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }

        // Every door open:
        private static float[,] buildHeightRoom_LUDR(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        if (indexY <= numOfVertsPerEdge/2)
                        {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                        } else {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        }
                    } else {
                        if (indexY <= numOfVertsPerEdge/2)
                        {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                        } else {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        }
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LUDR_occc(int numOfVertsPerEdge) //One corner open
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        if (indexY <= numOfVertsPerEdge/2)
                        {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                            //standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge)/2f -4.5f;
                        } else {
                            //standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                            standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                        }
                    } else {
                        if (indexY <= numOfVertsPerEdge/2)
                        {
                            //standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                            standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge)/2f -4.5f;
                        } else {
                            standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                            //standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                        }
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LUDR_ccoo(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY >= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexY, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge)/2f -4.5f;
                    } else {
                        // standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                        standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge)/2f -4.5f;
                    }
                }
            }

            return standaloneRoom;
        }

        private static float[,] buildHeightRoom_LUDR_oocc(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY <= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexY, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else if (indexX <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                    } else {
                        // standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                    }
                }
            }

            return standaloneRoom;
        }

        private static float[,] buildHeightRoom_LUDR_coco(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX >= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexX, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, 0, numOfVertsPerEdge)/2f -4.5f;
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                    }
                }
            }

            return standaloneRoom;
        }
        
        private static float[,] buildHeightRoom_LUDR_ococ(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexX, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else if (indexY <= numOfVertsPerEdge/2)
                    {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge)/2f -4.5f;
                    } else {
                        standaloneRoom[indexX, indexY] = findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                        // standaloneRoom[indexX, indexY] = -findVertHeight_radialJunctionCorner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge)/2f -4.5f;
                    }
                }
            }

            return standaloneRoom;
        }

        private static float[,] buildHeightRoom_LUDR_cooo(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeOuter(indexX, indexY, 0, 0, numOfVertsPerEdge);
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeOuter(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeOuter(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeOuter(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }

        private static float[,] buildHeightRoom_LU_o(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeInner(indexX, indexY, 0, 0, numOfVertsPerEdge);
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeInner(indexX, indexY, 0, numOfVertsPerEdge-1, numOfVertsPerEdge);
                    // standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeInner(indexX, indexY, numOfVertsPerEdge-1, 0, numOfVertsPerEdge);
                    standaloneRoom[indexX, indexY] = findVertHeight_radialArc_negativeInner(indexX, indexY, numOfVertsPerEdge-1, numOfVertsPerEdge-1, numOfVertsPerEdge);
                }
            }

            return standaloneRoom;
        }

        
        
        private static float[,] buildHeightRoom_LUDR_oxox(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexX <= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexX, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexX, numOfVertsPerEdge)/2f -4.5f;
                        // standaloneRoom[indexX, indexY] = findVertHeight_linear(indexX, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }

        
        private static float[,] buildHeightRoom_LUDR_ooxx(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    if (indexY <= numOfVertsPerEdge/2)
                    {
                        // standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexY, numOfVertsPerEdge)/2f -4.5f;
                        standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    } else {
                        standaloneRoom[indexX, indexY] = -findVertHeight_linear(indexY, numOfVertsPerEdge)/2f -4.5f;
                        // standaloneRoom[indexX, indexY] = findVertHeight_linear(indexY, numOfVertsPerEdge);
                    }
                }
            }

            return standaloneRoom;
        }







        private static float[,] buildHeightRoom_LUDR_oooo(int numOfVertsPerEdge)
        {
            float[,] standaloneRoom = createEmptyRoomArray(numOfVertsPerEdge);

            for(int indexX=0; indexX<numOfVertsPerEdge; indexX++)
            {
                for(int indexY=0; indexY<numOfVertsPerEdge; indexY++)
                {
                    standaloneRoom[indexX, indexY] = -maxHeight*2f;
                }
            }

            return standaloneRoom;
        }







        // Base template builders:
        private static float[,] createEmptyRoomArray(int numOfVertsPerEdge)
        {
            return new float[numOfVertsPerEdge, numOfVertsPerEdge];
        }

        private static float findVertHeight_linear(int relevantCoord, int numOfVertsPerEdge)
        {
            //return (Mathf.Abs(((float)relevantCoord) / ((float)numOfVertsPerEdge-1) - 0.5f) - 0.25f) * 4f * maxHeight;
            //return (Mathf.Pow(Mathf.Abs((((float)relevantCoord) / ((float)numOfVertsPerEdge-1) - 0.5f) * 2f), 2f) - 0.5f) * 2f * maxHeight;

            // -One to one:
            float returnFloat = (Mathf.Abs(((float)relevantCoord) / ((float)numOfVertsPerEdge-1) - 0.5f) -0.25f) * 4f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            return returnFloat * maxHeight;
        }

        private static float findVertHeight_radialArc(int vertX, int vertY, int cornerX, int cornerY, int numOfVertsPerEdge)
        {
            float vertRadius = Mathf.Sqrt(Mathf.Pow(vertX - cornerX, 2) + Mathf.Pow(vertY - cornerY, 2));

            if (vertRadius >= numOfVertsPerEdge-1) return maxHeight;

            //return (Mathf.Abs(vertRadius / ((float)numOfVertsPerEdge-1) - 0.5f) - 0.25f) * 4f * maxHeight;
            //return (Mathf.Pow(Mathf.Abs((vertRadius / ((float)numOfVertsPerEdge-1) - 0.5f) * 2f), 2f) - 0.5f) * 2f * maxHeight;

            // -One to one:
            float returnFloat = (Mathf.Abs(vertRadius / ((float)numOfVertsPerEdge-1) - 0.5f) -0.25f) * 4f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            return returnFloat * maxHeight;
        }

        private static float findVertHeight_radialCenter(int vertX, int vertY, int numOfVertsPerEdge)
        {
            float distanceToCenter = (((float)numOfVertsPerEdge)-1f)/2f;
            float vertRadius = Mathf.Sqrt(Mathf.Pow(((float)vertX) - distanceToCenter, 2) + Mathf.Pow(((float)vertY) - distanceToCenter, 2));

            if (vertRadius >= distanceToCenter) return maxHeight;

            //return (vertRadius / ((float)((numOfVertsPerEdge-1)/2)) - 0.5f) * 2f * maxHeight;
            //return (Mathf.Pow(vertRadius / ((float)((numOfVertsPerEdge-1)/2)), 2f) - 0.5f) * 2f * maxHeight;

            // -One to one:
            float returnFloat = (vertRadius / distanceToCenter - 0.5f) * 2f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            return returnFloat * maxHeight;
        }

        private static float findVertHeight_radialJunctionCorner(int vertX, int vertY, int cornerX, int cornerY, int numOfVertsPerEdge)
        {
            float distanceToCenter = (((float)numOfVertsPerEdge)-1f)/2f;
            float vertRadius = Mathf.Sqrt(Mathf.Pow(vertX - cornerX, 2) + Mathf.Pow(vertY - cornerY, 2));

            if (vertRadius >= distanceToCenter) return -maxHeight;

            // -One to one:
            float returnFloat = (vertRadius / distanceToCenter - 0.5f) * -2f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            return returnFloat * maxHeight;
        }

        

        private static float findVertHeight_radialArc_negativeOuter(int vertX, int vertY, int cornerX, int cornerY, int numOfVertsPerEdge)
        {
            float vertRadius = Mathf.Sqrt(Mathf.Pow(vertX - cornerX, 2) + Mathf.Pow(vertY - cornerY, 2));

            if (vertRadius >= numOfVertsPerEdge-1) return -maxHeight*2f;

            // -One to one:
            float returnFloat = (Mathf.Abs(vertRadius / ((float)numOfVertsPerEdge-1) - 0.5f) -0.25f) * 4f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            if (vertRadius <= (numOfVertsPerEdge-1)/2f) return returnFloat * maxHeight;

            return -(returnFloat * maxHeight)/2f -4.5f;;
        }

        private static float findVertHeight_radialArc_negativeInner(int vertX, int vertY, int cornerX, int cornerY, int numOfVertsPerEdge)
        {
            float vertRadius = Mathf.Sqrt(Mathf.Pow(vertX - cornerX, 2) + Mathf.Pow(vertY - cornerY, 2));

            if (vertRadius >= numOfVertsPerEdge-1) return maxHeight;

            // -One to one:
            float returnFloat = (Mathf.Abs(vertRadius / ((float)numOfVertsPerEdge-1) - 0.5f) -0.25f) * 4f;
            // Sin'd:
            returnFloat = Mathf.Sin(returnFloat * Mathf.PI/2f);

            if (vertRadius >= (numOfVertsPerEdge-1)/2f) return returnFloat * maxHeight;

            return -(returnFloat * maxHeight)/2f -4.5f;;
        }


        // -----------------------------------------------------------------------------------------------------------
        // ---------------- Template Builders end: --------------------------------------------------------------------
        // -----------------------------------------------------------------------------------------------------------
        */









    }
}