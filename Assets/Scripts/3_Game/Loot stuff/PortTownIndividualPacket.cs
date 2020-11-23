using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PortTownSpace
{
    public class PortTownIndividualPacket
    {
        private byte[] portTownPierCoords_simple = new byte[2];
        private float[] portTownPierCoords_upscaled = new float[2];
        private float portTownYRotation;

        private Vector3[] houseLocations;
        private Vector3[] houseScales;
        private float[] houseYRotations;
        private Color32[] houseRoofColors;
        private Color32[] houseWallColors;

        // private Color32 baseRoofColor;

        public void setPortTownPierCoords_simple(byte xCoord, byte yCoord)
        {
            portTownPierCoords_simple[0] = xCoord;
            portTownPierCoords_simple[1] = yCoord;
        }

        public void setPortTownPierCoords_upscaled(float xCoord, float zCoord)
        {
            portTownPierCoords_upscaled[0] = xCoord;
            portTownPierCoords_upscaled[1] = zCoord;
        }

        public void setPortTownYRotation(float portTownYRotation)
        {
            this.portTownYRotation = portTownYRotation;
        }

        // public void setHouseData(Vector3[] houseLocations, Vector3[] houseScales, float[] houseYRotations, Color32 baseRoofColor)
        public void setHouseData(Vector3[] houseLocations, Vector3[] houseScales, float[] houseYRotations, Color32[] houseRoofColors, Color32[] houseWallColors)
        {
            this.houseLocations = houseLocations;
            this.houseScales = houseScales;
            this.houseYRotations = houseYRotations;
            // this.baseRoofColor = baseRoofColor;
            this.houseRoofColors = houseRoofColors;
            this.houseWallColors = houseWallColors;
        }

        public void setAllData_forDemo()
        {
            portTownPierCoords_simple[0] = 5;
            portTownPierCoords_simple[1] = 3;

            portTownPierCoords_upscaled[0] = 165f;
            portTownPierCoords_upscaled[1] = -110.8001f;

            portTownYRotation = 0f;

            houseLocations = new Vector3[5];
            houseLocations[0] = new Vector3(156.0945f, 1.884948f, -120.3454f);
            houseLocations[1] = new Vector3(165.787f, 1.729026f, -118.2171f);
            houseLocations[2] = new Vector3(170.0854f, 2.34975f, -124.4419f);
            houseLocations[3] = new Vector3(174.0533f, 2.325243f, -118.0143f);
            houseLocations[4] = new Vector3(177.3852f, 3.032691f, -118.1349f);

            houseScales = new Vector3[5];
            houseScales[0] = new Vector3(1.049013f, 0.6055747f, 2.236849f);
            houseScales[1] = new Vector3(1.314497f, 0.8609981f, 1.66843f);
            houseScales[2] = new Vector3(1.044066f, 0.6740726f, 1.706836f);
            houseScales[3] = new Vector3(1.690497f, 0.8665862f, 1.476954f);
            houseScales[4] = new Vector3(1.096496f, 0.5187829f, 2.375456f);

            houseYRotations = new float[5];
            houseYRotations[0] = 82.78101f;
            houseYRotations[1] = 88.36401f;
            houseYRotations[2] = 80.397f;
            houseYRotations[3] = 170.354f;
            houseYRotations[4] = 171.748f;

            houseRoofColors = new Color32[5];
            houseRoofColors[0] = new Color32(172, 144, 63, 255);
            houseRoofColors[1] = new Color32(132, 98, 21, 255);
            houseRoofColors[2] = new Color32(168, 143, 71, 255);
            houseRoofColors[3] = new Color32(149, 119, 40, 255);
            houseRoofColors[4] = new Color32(124, 97, 17, 255);

            houseWallColors = new Color32[5];
            houseWallColors[0] = new Color32(169, 157, 142, 255);
            houseWallColors[1] = new Color32(161, 145, 123, 255);
            houseWallColors[2] = new Color32(177, 174, 169, 255);
            houseWallColors[3] = new Color32(149, 126, 104, 255);
            houseWallColors[4] = new Color32(142, 122, 110, 255);
        }

        public byte[] getPortTownCoords_simple()
        {
            return portTownPierCoords_simple;
        }

        public float[] getPortTownCoords_upscaled()
        {
            return portTownPierCoords_upscaled;
        }

        public float getPortTownYRotation()
        {
            return portTownYRotation;
        }

        public Vector3[] getHouseLocations()
        {
            return houseLocations;
        }

        public Vector3[] getHouseScales()
        {
            return houseScales;
        }

        public float[] getHouseRotations()
        {
            return houseYRotations;
        }

        public Vector3 getHouseRotation_asVector3(byte houseIndex)
        {
            return new Vector3(0f, houseYRotations[houseIndex], 0f);
        }

        // public Color32 getBaseRoofColor()
        // {
        //     return baseRoofColor;
        // }

        public Color32[] getHouseRoofColors()
        {
            return houseRoofColors;
        }

        public Color32[] getHouseWallColors()
        {
            return houseWallColors;
        }
    }
}
