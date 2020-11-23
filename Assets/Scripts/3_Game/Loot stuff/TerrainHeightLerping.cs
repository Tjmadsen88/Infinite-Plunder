using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace PortTownSpace
{
    public static class TerrainHeightLerpingClass
    {
        private static float getRem(float coordDist, float Pos)
        {
            return (Pos % coordDist)/coordDist;
        }

        private static int[] getCoords(float coordDist, int maxCoord, float Pos)
        {
            float localPos = Pos/coordDist;
            int[] returnInt = new int[2];

            if (localPos <= 0) 
            {
                returnInt[0] = 0;
                returnInt[1] = 1;
                return returnInt;
            }
            if (localPos >= maxCoord-2) 
            {
                returnInt[0] = maxCoord-2;
                returnInt[1] = maxCoord-1;
                return returnInt;
            }

            int truncd = (int)localPos;
            returnInt[0] = truncd;
            returnInt[1] = truncd+1;

            return returnInt;
        }

        private static float linearLerp(float height1, float height2, float decBetween)
        {
            return (height1*(1-decBetween) + (decBetween)*height2);
        }

        public static float getHeight(float[,] terrain2DArray, int maxX, int maxY, float coordDist, float XPos, float YPos)
        {
            // float startX = (-(maxX-1)*coordDist)/2f;
		    // float startZ = (-(maxY-1)*coordDist)/2f;
            float startX = Constants.roomWidthHeight * -2f;
		    float startZ = Constants.roomWidthHeight * -2f;

            float localXPos = -startX + XPos;
            float localYPos = -startZ + YPos;

            int[] xCorners = getCoords(coordDist, maxX, localXPos);
            int[] yCorners = getCoords(coordDist, maxY, localYPos);

            float yRem = getRem(coordDist, localYPos);
            float xRem = getRem(coordDist, localXPos);

            if (xCorners[0] == 0 || xCorners[1] == maxX-1) xRem = 0.5f;
            if (yCorners[0] == 0 || yCorners[1] == maxY-1) yRem = 0.5f;

            float leftHeight = linearLerp(terrain2DArray[xCorners[0], yCorners[0]], 
                                            terrain2DArray[xCorners[0], yCorners[1]], 
                                            yRem);

            float rightHeight = linearLerp(terrain2DArray[xCorners[1], yCorners[0]], 
                                            terrain2DArray[xCorners[1], yCorners[1]], 
                                            yRem);

            return linearLerp(leftHeight, rightHeight, xRem);
        }
    }
}
