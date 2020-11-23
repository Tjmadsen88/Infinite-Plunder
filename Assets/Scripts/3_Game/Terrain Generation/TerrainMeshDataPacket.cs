using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeightArrayTo3DMeshConverterSpace
{
    public class TerrainMeshDataPacket
    {
        Vector3[] vertices;
        int[] triangles;
        Vector3[] normals;
        Vector2[] uv;

        public void setData_section(float[,] heightArray, float[,] heightArray_forNormals, float coordDist)
        {
            int maxX = heightArray.GetLength(0);
            int maxY = heightArray.GetLength(1);

            vertices = HeightArrayTo3DMeshConverter.createVertices(heightArray, maxX, maxY, coordDist);
            triangles = HeightArrayTo3DMeshConverter.createTriangles(maxX, maxY);
            // normals = HeightArrayTo3DMeshConverter.createNormals_2(HeightArrayTo3DMeshConverter.createVertices(heightArray_forNormals, maxX+2, maxY+2, coordDist), maxX, maxY);
            normals = HeightArrayTo3DMeshConverter.createNormals_2_approx(heightArray_forNormals, maxX, maxY, coordDist);
            uv = HeightArrayTo3DMeshConverter.createUVs_2(heightArray, maxX, maxY);
        }

        public Mesh getData_asMesh()
        {
            Mesh returnMesh = new Mesh();

            returnMesh.vertices = vertices;
            returnMesh.triangles = triangles;
            returnMesh.normals = normals;
            returnMesh.uv = uv;

            return returnMesh;
        }
    }
}