using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    public GameObject waterPlane;


    public void setWaterSize(byte numOfRooms_horizontal, byte numOfRooms_vertical, float roomWidthHeight)
    {
        Transform waterTransform = waterPlane.GetComponent<Transform>();
        waterTransform.position = new Vector3((numOfRooms_horizontal/2f-2f)*roomWidthHeight, 0f, (numOfRooms_vertical/2f-2f)*-roomWidthHeight);
        waterTransform.localScale = new Vector3(numOfRooms_horizontal/10f*roomWidthHeight, 1f, numOfRooms_vertical/10f*roomWidthHeight);
    }

    public void setWaterTint(Color32 waterTint)
    {
        waterPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color32(waterTint.r, waterTint.g, waterTint.b, 64));
    }
}
