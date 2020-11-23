using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class CameraManager : MonoBehaviour
{
    public GameObject playerBoat;
    private Transform playerBoatTransform;

    private Camera mainCam;
    private Transform mainCamTransform;

    private float cameraDistance;

    private const float angleLerpSpeed = 0.1f;
    // private const float distanceLerpSpeed = 0.1f;
    private const float distanceLerpSpeed = 0.075f;
    // private const float distanceAheadOfBoat = 5f;
    private const float distanceAheadOfBoat = 10f;

    private Vector3 targetForward;
    private Vector3 currentOffset = new Vector3(0f, 0f, 0f);
    private Vector3 targetOffset;


    private bool cameraIsShaking = false;
    private const byte cameraShakeDuration_max = 35;
    private byte cameraShakeDuration_current = 0;
    private float cameraShakeIntensity_max = 0.2f;
    private float cameraShakeIntensity_current = 0f;
    private Vector3 cameraShakeOffset = new Vector3(0f, 0f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        mainCamTransform = mainCam.GetComponent<Transform>();

        playerBoatTransform = playerBoat.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        moveCam();

        if (cameraIsShaking)
        {
            mainCamTransform.position += cameraShakeOffset;
            mainCamTransform.eulerAngles += cameraShakeOffset * 4f;

            playDamageShakeAnimation();
        }
    }


    private void moveCam()
    {
        targetOffset = playerBoatTransform.forward * distanceAheadOfBoat - targetForward * cameraDistance;
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, distanceLerpSpeed);

        mainCamTransform.position = playerBoatTransform.position + currentOffset;

        
        mainCamTransform.forward = Vector3.Lerp(mainCamTransform.forward, targetForward, angleLerpSpeed);
    }



    public void setInitialCameraSettings(byte camAngleSetting, byte camDistSetting)
    {
        adjustCameraAngle(camAngleSetting);
        adjustCameraDistance(camDistSetting);
        mainCamTransform.forward = targetForward;

        currentOffset = playerBoatTransform.forward * distanceAheadOfBoat - targetForward * cameraDistance;
    }


    public void adjustCameraAngle(byte camAngleSetting)
    {
        float cameraAngle_vert = Constants.getSetting_CamAngle(camAngleSetting);
        float cameraAngle_horiz = -Mathf.PI/4f;
        
        targetForward = new Vector3(-Mathf.Sin(cameraAngle_vert) * Mathf.Cos(cameraAngle_horiz), 
                                    -Mathf.Cos(cameraAngle_vert), 
                                    -Mathf.Sin(cameraAngle_vert) * Mathf.Sin(cameraAngle_horiz));
    }

    public void adjustCameraDistance(byte camDistSetting)
    {
        cameraDistance = Constants.getSetting_CamDist(camDistSetting);
    }


    public void playDamageShakeAnimation()
    {
        if (!cameraIsShaking)
        {
            cameraIsShaking = true;
            cameraShakeDuration_current = cameraShakeDuration_max;
            cameraShakeIntensity_current = cameraShakeIntensity_max;
        }
        
        cameraShakeOffset = new Vector3(Random.Range(-cameraShakeIntensity_current, cameraShakeIntensity_current), 
                                        Random.Range(-cameraShakeIntensity_current, cameraShakeIntensity_current), 
                                        Random.Range(-cameraShakeIntensity_current, cameraShakeIntensity_current));

        cameraShakeIntensity_current *= 0.95f;

        if (--cameraShakeDuration_current <= 5)
        {
            cameraIsShaking = false;
        }
    }
}