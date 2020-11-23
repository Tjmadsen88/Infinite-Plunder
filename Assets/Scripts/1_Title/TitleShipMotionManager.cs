using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleShipMotionManager : MonoBehaviour
{
    private float sinVal_wheel;
    private float sinInc_wheel = 0.0125f;

    private float sinVal_x;
    private float sinVal_y;
    private float sinVal_z;
    private float sinInc_x;
    private float sinInc_y;
    private float sinInc_z;
    private const float minSinInc = 0.0075f;
    private const float maxSinInc = 0.02f;

    private const float maxTumbleAngle = 0.75f;
    private const float maxCameraTumble = 0.1f;
    private const float maxWheelRotation = 5f;

    private Transform shipParentTransform;
    public Transform wheelTransform;
    public Transform cameraTransform;

    private Vector3 initialCamRotation;


    // Start is called before the first frame update
    void Start()
    {
        shipParentTransform = gameObject.GetComponent<Transform>();

        sinVal_x = Random.Range(0f, 2*Mathf.PI);
        sinVal_y = Random.Range(0f, 2*Mathf.PI);
        sinVal_z = Random.Range(0f, 2*Mathf.PI);
        
        sinInc_x = Random.Range(minSinInc, maxSinInc);
        sinInc_y = Random.Range(minSinInc, maxSinInc);
        sinInc_z = Random.Range(minSinInc, maxSinInc);

        initialCamRotation = cameraTransform.eulerAngles;

        //StartCoroutine("moveTheCameraInAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        updateShipBobbing();
    }

    private void updateShipBobbing()
    {
        sinVal_wheel = updateSinVal(sinVal_wheel, sinInc_wheel);
        sinVal_x = updateSinVal(sinVal_x, sinInc_x);
        sinVal_y = updateSinVal(sinVal_y, sinInc_y);
        sinVal_z = updateSinVal(sinVal_z, sinInc_z);

        shipParentTransform.eulerAngles = new Vector3(maxTumbleAngle * Mathf.Sin(sinVal_x), 
                                                    maxTumbleAngle * Mathf.Sin(sinVal_y), 
                                                    maxTumbleAngle * Mathf.Sin(sinVal_z));

        cameraTransform.localEulerAngles = initialCamRotation + new Vector3(maxCameraTumble * Mathf.Sin(sinVal_x), 
                                                                    maxCameraTumble * Mathf.Sin(sinVal_y), 
                                                                    maxCameraTumble * Mathf.Sin(sinVal_z));
                                                                    
        wheelTransform.localEulerAngles = new Vector3(0f, 0f, maxWheelRotation * Mathf.Sin(sinVal_wheel - Mathf.PI/2f));
    }

    private float updateSinVal(float sinVal, float sinInc)
    {
        if (sinVal <= 2 * Mathf.PI) return sinVal + sinInc;

        return sinVal + sinInc - 2 * Mathf.PI;
    }


    IEnumerator moveTheCameraInAnimation()
    {
        // CUurrently unused... should I remove this?
        Vector3 cameraPosition = cameraTransform.localPosition;
        
        cameraTransform.localPosition -= new Vector3(0f, 0f, -1.5f);

        for (int index = 0; index<120; index++) 
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraPosition, 0.05f);
            yield return null;
        }

        cameraTransform.localPosition = cameraPosition;
    }
}
