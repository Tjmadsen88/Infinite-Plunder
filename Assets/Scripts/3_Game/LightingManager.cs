using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public GameObject sunLight;

    private float minXRotation = 20f;
    private float maxXRotation = 60f;

    private float minYRotation = 0f;
    private float maxYRotation = 360f;


    // Start is called before the first frame update
    void Start()
    {
        // float xRotation = Random.Range(20f, 60f);
        //float ambientAmount = 0.5f - (((xRotation - 20f) / 40f) * 0.25f);

        //RenderSettings.ambientLight = new Color(ambientAmount, ambientAmount, ambientAmount, 1f);

        //light.GetComponent<Transform>().eulerAngles = new Vector3(Random.Range(minXRotation, maxXRotation), Random.Range(minYRotation, maxYRotation), 0f);
        sunLight.GetComponent<Transform>().eulerAngles = new Vector3(Random.Range(minXRotation, maxXRotation), Random.Range(minYRotation, maxYRotation), 0f);

        // makeItNightTime();

        /*
        switch(Random.Range(0, 11))
        {
            case 0:
            case 1:
                makeItSunset();
                break;

            case 2:
            case 3:
            case 4:
                makeItNightTime();
                break;

            default:
                makeItDayTime();
                break;
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }


    // private void makeItDayTime()
    // {
    //     light.GetComponent<Transform>().eulerAngles = new Vector3(Random.Range(minXRotation, maxXRotation), Random.Range(minYRotation, maxYRotation), 0f);
    // }

    // private void makeItNightTime()
    // {
    //     light.GetComponent<Light>().color = new Color32(195, 220, 255, 255);
    //     light.GetComponent<Light>().intensity = 0.85f;

    //     light.GetComponent<Transform>().eulerAngles = new Vector3(Random.Range(minXRotation, maxXRotation), Random.Range(minYRotation, maxYRotation), 0f);
    // }

    // private void makeItSunset()
    // {
    //     light.GetComponent<Light>().color = new Color32(255, 190, 135, 255);
    //     light.GetComponent<Light>().intensity = 1.5f;
        
    //     light.GetComponent<Transform>().eulerAngles = new Vector3(25f, Random.Range(minYRotation, maxYRotation), 0f);
    // }

    private void makeItNightTime()
    {
        sunLight.GetComponent<Light>().color = new Color32(255, 235, 179, 255);

        RenderSettings.ambientLight = new Color32(85, 107, 221, 255);
    }
}
