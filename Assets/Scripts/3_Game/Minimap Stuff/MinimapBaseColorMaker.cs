using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimapBaseColorMakerSpace
{
    public static class MinimapBaseColorMaker
    {
        public static Color32[] convertBoolArrayToMinimapColorArray(bool[,] boolArray, int boolArrayWidth, int boolArrayHeight)
        {
            Color32 color_invisible = new Color32(0, 0, 0, 0); // ID 0
            Color32 color_unexplored = new Color32(170, 138, 90, 255); // ID 1
            Color32 color_walls = new Color32(108, 94, 75, 255); // ID 2

            Color32[] minimapColorArray = new Color32[boolArrayWidth * boolArrayHeight];


            bool[,] wallFilter = createWallFilter(findBorderThickness(boolArrayWidth, boolArrayHeight, 5));

            for (int indexX=0; indexX<boolArrayWidth; indexX++)
            {
                for (int indexY=0; indexY<boolArrayHeight; indexY++)
                {
                    switch (getPixelColorID(boolArray, boolArrayWidth, boolArrayHeight, wallFilter, indexX, indexY))
                    {
                        case 0:
                            minimapColorArray[indexX + boolArrayWidth*indexY] = color_invisible;
                            break;

                        case 1:
                            minimapColorArray[indexX + boolArrayWidth*indexY] = color_unexplored;
                            break;

                        default:
                            minimapColorArray[indexX + boolArrayWidth*indexY] = color_walls;
                            break;
                    }
                }
            }

            return minimapColorArray;
        }


        private static float findBorderThickness(int imageWidth, int imageHeight, float targetThickness)
        {
            return targetThickness * Mathf.Max(imageWidth, imageHeight) / 343.65f;
        }


        private static bool[,] createWallFilter(float radius)
        {
            int intRadius = (int)Mathf.Round(radius);
            int returnArrayWidthHeight = 2*intRadius+1;
            bool[,] returnArray = new bool[returnArrayWidthHeight, returnArrayWidthHeight];

            for (int indexX=0; indexX<returnArrayWidthHeight; indexX++)
            {
                for (int indexY=0; indexY<returnArrayWidthHeight; indexY++)
                {
                    returnArray[indexX, indexY] = Mathf.Sqrt(Mathf.Pow(indexX-intRadius, 2) + Mathf.Pow(indexY-intRadius, 2)) <= intRadius;
                }
            }

            return returnArray;
        }


        private static byte getPixelColorID(bool[,] boolArray, int boolArrayWidth, int boolArrayHeight, bool[,] wallFilter, int pixelX, int pixelY)
        {
            if (boolArray[pixelX, pixelY]) return 1;

            int wallFilterSize = wallFilter.GetLength(0);

            int filterCenter = (wallFilterSize - 1)/2;
            int entryX;
            int entryY;

            for (int indexX=0; indexX<wallFilterSize; indexX++)
            {
                for (int indexY=0; indexY<wallFilterSize; indexY++)
                {
                    entryX = pixelX + filterCenter - indexX;
                    entryY = pixelY + filterCenter - indexY;

                    if (entryX >= 0 && entryY >= 0 && entryX < boolArrayWidth && entryY < boolArrayHeight)
                    {
                        if (boolArray[entryX, entryY] && wallFilter[indexX, indexY]) return 2;
                    }
                }
            }

            return 0;
        }
    }
}
