using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeightArrayTo3DMeshConverterSpace
{
    public static class HeightArrayTo3DMeshConverter
    {
        public static Mesh create_total(float[,] heightArray, float coordDist)
        {
            Mesh returnMesh = new Mesh();
            int maxX = heightArray.GetLength(0);
            int maxY = heightArray.GetLength(1);

            Vector3[] vertices = createVertices(heightArray, maxX, maxY, coordDist);
            int[] triangles = createTriangles(maxX, maxY);
            Vector3[] normals = createNormals(vertices, maxX, maxY);
            // Vector3[] normals = createNormals_approx(heightArray, maxX, maxY, coordDist);
            Vector2[] uv = createUVs(maxX, maxY);

            returnMesh.vertices = vertices;
            returnMesh.triangles = triangles;
            returnMesh.normals = normals;
            returnMesh.uv = uv;

            return returnMesh;
        }


        public static Vector3[] createVertices(float[,] heightArray, int maxX, int maxY, float coordDist)
        {
            Vector3[] returnArray = new Vector3[maxX * maxY];

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    returnArray[indexX + maxX*indexZ] =
                        new Vector3(indexX*coordDist, heightArray[indexX, indexZ], indexZ * -coordDist);
                }
            }

            return returnArray;
        }

        public static int[] createTriangles(int maxX, int maxY)
        {
            int[] returnArray = new int[(maxX-1)*(maxY-1)*6];
            int startingIndex;
            int startingIndex2;

            for (int indexZ = 0; indexZ < maxY-1; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX-1; indexX++) 
                {
                    startingIndex = (indexX + (maxX-1)*indexZ);
                    startingIndex2 = (indexX + (maxX)*indexZ);

                    // Upper right triangle:
                    returnArray[startingIndex*6] = startingIndex2; // the upper left
                    returnArray[startingIndex*6 + 1] = startingIndex2 + 1; // the upper right
                    returnArray[startingIndex*6 + 2] = startingIndex2 + 1 + maxX; // the bottom right
                    // Bottom left triangle:
                    returnArray[startingIndex*6 + 3] = startingIndex2; // the upper left
                    returnArray[startingIndex*6 + 4] = startingIndex2 + 1 + maxX; // the bottom right
                    returnArray[startingIndex*6 + 5] = startingIndex2 + maxX; // the bottom left
                }
		    }

            return returnArray;
        }

        public static Vector3[] createNormals(Vector3[] vertices, int maxX, int maxY)
        {
            Vector3 tempVec1;
            Vector3 tempVec2;
            Vector3 tempVec3;
            Vector3 totalVec;
            
            Vector3[] returnArray = new Vector3[maxX * maxY];

            bool notOnTop;
            bool notOnleft;
            bool notOnRight;
            bool notOnBottom;

            int startingIndex;

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    startingIndex = (indexX + maxX*indexZ);

                    notOnTop = indexZ != 0;
                    notOnBottom = indexZ != maxY-1;
                    notOnleft = indexX != 0;
                    notOnRight = indexX != maxX-1;

                    totalVec = new Vector3(0f, 0.0001f, 0f);

                    if (notOnTop && notOnleft) // The upper left vertices:
                    {
                        tempVec1 = vertices[startingIndex -maxX] - vertices[startingIndex]; // The vertex up above
                        tempVec2 = vertices[startingIndex -1 -maxX] - vertices[startingIndex]; // The vertex to the upper-left
                        tempVec3 = vertices[startingIndex -1] - vertices[startingIndex]; // The vertex to the left

                        totalVec += Vector3.Cross(tempVec1, tempVec2);
                        totalVec += Vector3.Cross(tempVec2, tempVec3);
                    }
                    
                    if (notOnleft && notOnBottom) // The bottom left vertices:
                    {
                        tempVec1 = vertices[startingIndex -1] - vertices[startingIndex]; // The vertex to the left
                        tempVec2 = vertices[startingIndex +maxX] - vertices[startingIndex]; // The vertex on the bottom

                        totalVec += Vector3.Cross(tempVec1, tempVec2);
                    }
                    
                    if (notOnBottom && notOnRight) // The bottom right vertices:
                    {
                        tempVec1 = vertices[startingIndex +maxX] - vertices[startingIndex]; // The vertex on the bottom
                        tempVec2 = vertices[startingIndex +1 +maxX] - vertices[startingIndex]; // The vertex in the bottom right
                        tempVec3 = vertices[startingIndex +1] - vertices[startingIndex]; // The vertex on the right

                        totalVec += Vector3.Cross(tempVec1, tempVec2);
                        totalVec += Vector3.Cross(tempVec2, tempVec3);
                    }
                    
                    if (notOnRight && notOnTop) // The upper right vertices:
                    {
                        tempVec1 = vertices[startingIndex +1] - vertices[startingIndex]; // The vertex to the right
                        tempVec2 = vertices[startingIndex -maxX] - vertices[startingIndex]; // The vertex up above

                        totalVec += Vector3.Cross(tempVec1, tempVec2);
                    }

                    returnArray[indexX + maxX*indexZ] = -totalVec.normalized;
                }
            }

            return returnArray;
        }

        public static Vector3[] createNormals_approx(float[,] heightArray, int maxX, int maxY, float coordDist)
        {
            float origHeight;
            float tempVert;
            float xVal = 0;
            float yVal = 0;
            float zVal = 0;

            Vector3 totalVec;
            
            Vector3[] returnArray = new Vector3[maxX * maxY];

            bool notOnTop;
            bool notOnLeft;
            bool notOnRight;
            bool notOnBottom;

            int startingIndex;

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    startingIndex = (indexX + maxX*indexZ);

                    notOnTop = indexZ != 0;
                    notOnBottom = indexZ != maxY-1;
                    notOnLeft = indexX != 0;
                    notOnRight = indexX != maxX-1;

                    origHeight = heightArray[indexX, indexZ];

                    if (notOnTop) // The top vertex
                    {
                        tempVert = heightArray[indexX, indexZ-1]; // The vertex up above

                        zVal += origHeight - tempVert;
                        yVal += coordDist * 0.58333f;
                    }
                    
                    if (notOnBottom) // The bottom vertex
                    {
                        tempVert = heightArray[indexX, indexZ+1]; // The vertex on the bottom

                        zVal += tempVert - origHeight;
                        yVal += coordDist * 0.58333f;
                    }
                    
                    if (notOnLeft) // The left vertex
                    {
                        tempVert = heightArray[indexX-1, indexZ]; // The vertex to the left

                        xVal += tempVert - origHeight;
                        yVal += coordDist * 0.58333f;
                    }
                    
                    if (notOnRight) // The right vertex
                    {
                        tempVert = heightArray[indexX+1, indexZ]; // The vertex on the right

                        xVal += origHeight - tempVert;
                        yVal += coordDist * 0.58333f;
                    }

                    totalVec = new Vector3(xVal, yVal, zVal);
                    returnArray[indexX + maxX*indexZ] = totalVec.normalized;
                }
            }

            return returnArray;
        }

        public static Vector3[] createNormals_2(Vector3[] vertices, int maxX, int maxY)
        {
            Vector3 vec_u;
            Vector3 vec_ul;
            Vector3 vec_l;
            Vector3 vec_d;
            Vector3 vec_dr;
            Vector3 vec_r;
            Vector3 totalVec;
            
            Vector3[] returnArray = new Vector3[maxX * maxY];

            int startingIndex;

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    startingIndex = ((indexX+1) + (maxX+2)*(indexZ+1));

                    vec_u = vertices[startingIndex -(maxX+2)] - vertices[startingIndex]; // The vertex up above
                    vec_ul = vertices[startingIndex -1 -(maxX+2)] - vertices[startingIndex]; // The vertex to the upper-left
                    vec_l = vertices[startingIndex -1] - vertices[startingIndex]; // The vertex to the left
                    vec_d = vertices[startingIndex +(maxX+2)] - vertices[startingIndex]; // The vertex on the bottom
                    vec_dr = vertices[startingIndex +1 +(maxX+2)] - vertices[startingIndex]; // The vertex in the bottom right
                    vec_r = vertices[startingIndex +1] - vertices[startingIndex]; // The vertex on the right

                    totalVec = new Vector3(0f, 0.0001f, 0f);
                    
                    totalVec += Vector3.Cross(vec_u, vec_ul);
                    totalVec += Vector3.Cross(vec_ul, vec_l);
                    totalVec += Vector3.Cross(vec_l, vec_d);
                    totalVec += Vector3.Cross(vec_d, vec_dr);
                    totalVec += Vector3.Cross(vec_dr, vec_r);
                    totalVec += Vector3.Cross(vec_r, vec_u);

                    returnArray[indexX + maxX*indexZ] = -totalVec.normalized;
                }
            }

            return returnArray;
        }

        public static Vector3[] createNormals_2_approx(float[,] heightArray_forNormals, int maxX, int maxY, float coordDist)
        {
            float vert_u;
            float vert_l;
            float vert_d;
            float vert_r;
            Vector3 totalVec;
            
            Vector3[] returnArray = new Vector3[maxX * maxY];

            // int startingIndex;

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    // startingIndex = ((indexX+1) + (maxX+2)*(indexZ+1));

                    vert_u = heightArray_forNormals[indexX+1, indexZ]; // The vertex up above
                    vert_l = heightArray_forNormals[indexX, indexZ+1]; // The vertex to the left
                    vert_d = heightArray_forNormals[indexX+1, indexZ+2]; // The vertex on the bottom
                    vert_r = heightArray_forNormals[indexX+2, indexZ+1]; // The vertex on the right

                    totalVec = new Vector3(vert_l - vert_r, coordDist*2.333f, vert_d - vert_u);
                    // totalVec = new Vector3(vert_l - vert_r, coordDist*4f, vert_d - vert_u);

                    returnArray[indexX + maxX*indexZ] = totalVec.normalized;
                }
            }

            return returnArray;
        }

        public static Vector2[] createUVs(int maxX, int maxY)
        {
            Vector2[] returnArray = new Vector2[maxX * maxY];

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    returnArray[indexX + maxX*indexZ] = new Vector2(((float)indexX) / ((float)maxX-1), ((float)indexZ) / ((float)maxY-1));
                }
            }

            return returnArray;
        }

        public static Vector2[] createUVs_2(float[,] heightArray, int maxX, int maxY)
        {
            Vector2[] returnArray = new Vector2[maxX * maxY];

            float minHeight = -0.5f;
            float maxHeight = 3.5f;

            for (int indexZ = 0; indexZ < maxY; indexZ++) 
            {
                for (int indexX = 0; indexX < maxX; indexX++) 
                {
                    if (heightArray[indexX, indexZ] <= minHeight) returnArray[indexX + maxX*indexZ] = new Vector2(0f, 0f);
                    else if (heightArray[indexX, indexZ] >= maxHeight) returnArray[indexX + maxX*indexZ] = new Vector2(1f, 0f);
                    else {
                        returnArray[indexX + maxX*indexZ]= new Vector2((heightArray[indexX, indexZ]-minHeight) / (maxHeight-minHeight), 0f);
                    }
                }
            }

            return returnArray;
        }

    }
}
