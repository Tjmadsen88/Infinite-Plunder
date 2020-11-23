using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpscaledLayoutBuilderSpace
{
    public class UpscaledLayoutReturnPacket
    {
        private bool[,] boolArray;
        private bool[,] boolArray_noNoise;
        private float[,] landVertexHeights;
        //private byte[,] itemLocations;
        // private float[,] portTownLocations;

        public void setBoolArray(bool[,] boolArray)
        {
            this.boolArray = boolArray;
        }

        public void setBoolArray_noNoise(bool[,] boolArray_noNoise)
        {
            this.boolArray_noNoise = boolArray_noNoise;
        }

        public void setLandVertexHeights(float[,] landVertexHeights)
        {
            this.landVertexHeights = landVertexHeights;
        }

        /*public void setItemLocations(byte[,] itemLocations)
        {
            this.itemLocations = itemLocations;
        }

        public void setPortTownLocations(float[,] portTownLocations)
        {
            this.portTownLocations = portTownLocations;
        }*/


        public bool[,] getBoolArray()
        {
            return boolArray;
        }

        public bool[,] getBoolArray_noNoise()
        {
            return boolArray_noNoise;
        }

        public float[,] getLandVertexHeights()
        {
            return landVertexHeights;
        }

        /*public byte[,] getItemLocations()
        {
            return itemLocations;
        }

        public float[,] getPortTownLocations()
        {
            return portTownLocations;
        }*/

    }
}
