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
    public class TerrainBuilderReturnPacket
    {
        // Mesh[,] terrainMeshes;
        private bool[,] boolArray;
        private bool[,] boolArray_noNoise;
        private TerrainMeshDataPacket[,] meshData;

        private SimplifiedLayoutReturnPacket simplePacket;
        private PortTownReturnPacket portTownPacket;
        private byte numOfKeys;

        // public TerrainBuilderReturnPacket(SimplifiedLayoutReturnPacket simplePacket, UpscaledLayoutReturnPacket upscaledPacket, Mesh[,] terrainMeshes)
        public TerrainBuilderReturnPacket(SimplifiedLayoutReturnPacket simplePacket, UpscaledLayoutReturnPacket upscaledPacket, TerrainMeshDataPacket[,] meshData, PortTownReturnPacket portTownPacket)
        {
            // this.terrainMeshes = terrainMeshes;
            boolArray = upscaledPacket.getBoolArray();
            boolArray_noNoise = upscaledPacket.getBoolArray_noNoise();
            this.meshData = meshData;
            this.simplePacket = simplePacket;
            this.portTownPacket = portTownPacket;

            numOfKeys = 0;
            for (int index=0; index<6; index++)
            {
                if (simplePacket.getHasKey()[index])
                    numOfKeys++;
            }
        }


        // public Mesh[,] getTerrainMeshes()
        // {
        //     return terrainMeshes;
        // }

        public Mesh[,] getTerrainMeshes()
        {
            int arrayWidth = meshData.GetLength(0);
            int arrayHeight = meshData.GetLength(1);

            Mesh[,] terrainMeshes = new Mesh[arrayWidth, arrayHeight];

            for (int indexX=0; indexX<arrayWidth; indexX++)
            {
                for (int indexY=0; indexY<arrayHeight; indexY++)
                {
                    terrainMeshes[indexX, indexY] = meshData[indexX, indexY].getData_asMesh();
                }
            }

            return terrainMeshes;
        }

        public bool[,] getBoolArray()
        {
            return boolArray;
        }

        public bool[,] getBoolArray_noNoise()
        {
            return boolArray_noNoise;
        }

        public SimplifiedLayoutReturnPacket getSimplePacket()
        {
            return simplePacket;
        }

        public SimpleRoom_Output[,] getSimplifiedRoomArray()
        {
            return simplePacket.getSimplifiedRoomArray();
        }

        public byte getAreaWidth()
        {
            return simplePacket.getAreaWidth();
        }

        public byte getAreaHeight()
        {
            return simplePacket.getAreaHeight();
        }

        public byte[] getPlayerStartingLocation()
        {
            return simplePacket.getPlayerStartingLocation();
        }

        public byte[,] getKeyLocations()
        {
            return simplePacket.getKeyLocations();
        }

        public bool[] getHasKey()
        {
            return simplePacket.getHasKey();
        }

        public byte[] getFinalTreasureLocation()
        {
            return simplePacket.getFinalTreasureLocation();
        }

        public byte[,] getDoorLocations()
        {
            return simplePacket.getDoorLocations();
        }

        public byte[] getDoorSides()
        {
            return simplePacket.getDoorSides();
        }

        public byte[] getDoorColors()
        {
            return simplePacket.getDoorColors();
        }

        public short getNumOfRooms()
        {
            return simplePacket.getNumOfRooms();
        }

        public PortTownReturnPacket getPortTownPacket()
        {
            return portTownPacket;
        }

        public byte getNumOfKeys()
        {
            return numOfKeys;
        }

        public short getNumOfLockedDoors()
        {
            return simplePacket.getNumOfLockedDoors();
        }
    }
}
