using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using SimplifiedLayoutBuilderSpace;
using UpscaledLayoutBuilderSpace;
using HeightArrayTo3DMeshConverterSpace;
using PortTownSpace;


namespace TerrainBuilderSpace
{
    public static class TerrainBuilder
    {
        public static TerrainBuilderReturnPacket generateTerrain(float roomWidthHeight, int numOfVertsPerEdge, byte sizeOfExplorableArea, byte numberOfKeys)
        {
            System.Random random_forThread = new System.Random();

            // SimplifiedLayoutReturnPacket simplifiedLayout = SimplifiedLayoutTemplates.generateLayout_DebugRoom();
            Debug.Log("Size of area: "+sizeOfExplorableArea);
            SimplifiedLayoutReturnPacket simplifiedLayout = SimplifiedLayoutBuilder.generateLayout_Normal(sizeOfExplorableArea, numberOfKeys, random_forThread);

            UpscaledLayoutReturnPacket upscaledLayout = UpscaledLayoutBuilder.upscaleLayout_Game(simplifiedLayout, roomWidthHeight, numOfVertsPerEdge, random_forThread);

            // Mesh[,] returnMeshes = sliceHeightArrayIntoMultipleMeshes(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);
            TerrainMeshDataPacket[,] meshData = sliceHeightArrayIntoMultipleMeshes_2(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);

            PortTownReturnPacket portTownPacket = PortTownPlacementGenerator.generatePortTownInformation(simplifiedLayout, upscaledLayout, roomWidthHeight, numOfVertsPerEdge, sizeOfExplorableArea, random_forThread);

            // return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, returnMeshes);
            return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, meshData, portTownPacket);
        }
        


        public static TerrainBuilderReturnPacket generateTerrain_titleVer(float roomWidthHeight, int numOfVertsPerEdge)
        {
            System.Random random_forThread = new System.Random();

            SimplifiedLayoutReturnPacket simplifiedLayout = SimplifiedLayoutTemplates.generateLayout_TitleScreen();

            UpscaledLayoutReturnPacket upscaledLayout = UpscaledLayoutBuilder.upscaleLayout_Title(simplifiedLayout, roomWidthHeight, numOfVertsPerEdge, random_forThread);

            // Mesh[,] returnMeshes = sliceHeightArrayIntoMultipleMeshes(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);
            TerrainMeshDataPacket[,] meshData = sliceHeightArrayIntoMultipleMeshes_2(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);

            // return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, returnMeshes);
            return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, meshData, null);
        }
        


        public static TerrainBuilderReturnPacket generateTerrain_shipSelectionVer(float roomWidthHeight, int numOfVertsPerEdge)
        {
            System.Random random_forThread = new System.Random();

            SimplifiedLayoutReturnPacket simplifiedLayout = SimplifiedLayoutTemplates.generateLayout_ShipSelectionScreen();

            UpscaledLayoutReturnPacket upscaledLayout = UpscaledLayoutBuilder.upscaleLayout_Title(simplifiedLayout, roomWidthHeight, numOfVertsPerEdge, random_forThread);

            // Mesh[,] returnMeshes = sliceHeightArrayIntoMultipleMeshes(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);
            TerrainMeshDataPacket[,] meshData = sliceHeightArrayIntoMultipleMeshes_2(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);

            // return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, returnMeshes);
            return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, meshData, null);
        }
        


        public static TerrainBuilderReturnPacket generateTerrain_DemoVer(float roomWidthHeight, int numOfVertsPerEdge)
        {
            // System.Random random_forThread = new System.Random();

            SimplifiedLayoutReturnPacket simplifiedLayout = SimplifiedLayoutTemplates.generateLayout_Demo();

            UpscaledLayoutReturnPacket upscaledLayout = UpscaledLayoutBuilder.upscaleLayout_Demo(simplifiedLayout, roomWidthHeight, numOfVertsPerEdge);

            // Mesh[,] returnMeshes = sliceHeightArrayIntoMultipleMeshes(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);
            TerrainMeshDataPacket[,] meshData = sliceHeightArrayIntoMultipleMeshes_2(upscaledLayout.getLandVertexHeights(), roomWidthHeight, numOfVertsPerEdge);

            PortTownReturnPacket portTownPacket = PortTownPlacementGenerator.generatePortTownInformation_DemoVer(simplifiedLayout, upscaledLayout, roomWidthHeight, numOfVertsPerEdge);

            // return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, returnMeshes);
            return new TerrainBuilderReturnPacket(simplifiedLayout, upscaledLayout, meshData, portTownPacket);
        }


        


        // private static Mesh[,] sliceHeightArrayIntoMultipleMeshes(float[,] heightArray, float roomWidthHeight, int numOfVertsPerEdge)
        // {
        //     int heightArrayLength = heightArray.GetLength(0);
        //     int heightArrayWidth = heightArray.GetLength(1);
        //     int numOfRooms_horizontal = heightArrayLength / numOfVertsPerEdge;
        //     int numOfRooms_vertical = heightArrayWidth / numOfVertsPerEdge;

        //     Mesh[,] returnMeshes = new Mesh[numOfRooms_horizontal, numOfRooms_vertical];
        //     float[,] smallerRoom = new float[numOfVertsPerEdge+1, numOfVertsPerEdge+1];
        //     float[,] smallerRoom_forNormals = new float[numOfVertsPerEdge+3, numOfVertsPerEdge+3];


        //     for (int indexX=0; indexX<numOfRooms_horizontal; indexX++)
        //     {
        //         for (int indexY=0; indexY<numOfRooms_vertical; indexY++)
        //         {
        //             fillSmallerRoom(smallerRoom, heightArray, heightArrayLength, heightArrayWidth, indexX*numOfVertsPerEdge, indexY*numOfVertsPerEdge, numOfVertsPerEdge+1, numOfVertsPerEdge+1);
        //             fillSmallerRoom(smallerRoom_forNormals, heightArray, heightArrayLength, heightArrayWidth, indexX*numOfVertsPerEdge-1, indexY*numOfVertsPerEdge-1, numOfVertsPerEdge+3, numOfVertsPerEdge+3);
                    
        //             returnMeshes[indexX, indexY] = HeightArrayTo3DMeshConverter.create_section(smallerRoom, smallerRoom_forNormals, roomWidthHeight/((float)numOfVertsPerEdge));
        //         }
        //     }

        //     return returnMeshes;
        // }


        private static TerrainMeshDataPacket[,] sliceHeightArrayIntoMultipleMeshes_2(float[,] heightArray, float roomWidthHeight, int numOfVertsPerEdge)
        {
            int heightArrayLength = heightArray.GetLength(0);
            int heightArrayWidth = heightArray.GetLength(1);
            int numOfRooms_horizontal = heightArrayLength / numOfVertsPerEdge;
            int numOfRooms_vertical = heightArrayWidth / numOfVertsPerEdge;

            TerrainMeshDataPacket[,] meshData = new TerrainMeshDataPacket[numOfRooms_horizontal, numOfRooms_vertical];
            float[,] smallerRoom = new float[numOfVertsPerEdge+1, numOfVertsPerEdge+1];
            float[,] smallerRoom_forNormals = new float[numOfVertsPerEdge+3, numOfVertsPerEdge+3];


            for (int indexX=0; indexX<numOfRooms_horizontal; indexX++)
            {
                for (int indexY=0; indexY<numOfRooms_vertical; indexY++)
                {
                    fillSmallerRoom(smallerRoom, heightArray, heightArrayLength, heightArrayWidth, indexX*numOfVertsPerEdge, indexY*numOfVertsPerEdge, numOfVertsPerEdge+1, numOfVertsPerEdge+1);
                    fillSmallerRoom(smallerRoom_forNormals, heightArray, heightArrayLength, heightArrayWidth, indexX*numOfVertsPerEdge-1, indexY*numOfVertsPerEdge-1, numOfVertsPerEdge+3, numOfVertsPerEdge+3);
                    
                    // meshData[indexX, indexY] = HeightArrayTo3DMeshConverter.create_section(smallerRoom, smallerRoom_forNormals, roomWidthHeight/((float)numOfVertsPerEdge));
                    meshData[indexX, indexY] = new TerrainMeshDataPacket();
                    meshData[indexX, indexY].setData_section(smallerRoom, smallerRoom_forNormals, roomWidthHeight/((float)numOfVertsPerEdge));
                }
            }

            return meshData;
        }



        private static void fillSmallerRoom(float[,] smallerRoom, float[,] totalArea, int totalAreaWidth, int totalAreaHeight, 
                                            int roomStartX, int roomStartY, int numOfVertsPerEdge_horiz, int numOfVertsPerEdge_verti)
        {
            for (int indexX=0; indexX<numOfVertsPerEdge_horiz; indexX++)
            {
                for (int indexY=0; indexY<numOfVertsPerEdge_verti; indexY++)
                {
                    if (indexX+roomStartX < 0 || indexY+roomStartY < 0 || indexX+roomStartX >= totalAreaWidth || indexY+roomStartY >= totalAreaHeight)
                    {
                        smallerRoom[indexX, indexY] = 0f;
                    } else {
                        smallerRoom[indexX, indexY] = totalArea[indexX+roomStartX, indexY+roomStartY];
                    }
                }
            }
        }


    }
}