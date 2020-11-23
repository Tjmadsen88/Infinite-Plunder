using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioManager_ShipSelection : MonoBehaviour
{
    private const float normalAspect_wDivH_portrait = 0.5625f;
    private const float normalAspect_wDivH_landscape = 1.7777777777777777777778f;

    public CanvasScaler canvasScaler;
    // public RectTransform canvasTransform;
    public RectTransform buttonsAreaTransform;
    public RectTransform cameraAreaTransform;
    public CameraRotationManager cameraRotationManager;

    private Camera mainCam;

    private float aspectRatio_wDivH;

    private const float normalLongerFOV = 45f;
    private float normalShorterFOV;

    private bool finishedStarting = false;

    // private Vector3 cameraAngle_portrait = new Vector3(32.25f, 0, 0);
    // private Vector3 cameraAngle_landscape = new Vector3(21.5f, 14.5f, 0);
    private Vector3 cameraAngle_portrait = new Vector3(13.5f, 0, 0);
    private Vector3 cameraAngle_landscape = new Vector3(0, 13.5f, 0);


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        normalShorterFOV = Camera.VerticalToHorizontalFieldOfView(normalLongerFOV, 9f/16f);

        finishedStarting = true;
        OnRectTransformDimensionsChange();
    }


    void OnRectTransformDimensionsChange()
    {
        float newAspect = ((float)Screen.width)/((float)Screen.height);

        if (aspectRatio_wDivH != newAspect && finishedStarting)
        {
            aspectRatio_wDivH = newAspect;



            if (aspectRatio_wDivH <= normalAspect_wDivH_portrait)
            {
                changeUI_portrait_forTallerPhones();
                moveScreenAreas_portrait_forTallerPhones(newAspect);
                // set the FOV such that the horizontal FOV is the same as normalShorterFOV
                mainCam.fieldOfView = Camera.HorizontalToVerticalFieldOfView(normalShorterFOV, aspectRatio_wDivH);

                cameraRotationManager.setIsPortrait(true);
            } else if (aspectRatio_wDivH <= 1f)
            {
                changeUI_portrait_forTablets();
                moveScreenAreas_portrait_forTablets(newAspect);
                // set the FOV to 90
                mainCam.fieldOfView = normalLongerFOV;

                cameraRotationManager.setIsPortrait(true);
            } else if (aspectRatio_wDivH <= normalAspect_wDivH_landscape)
            {
                changeUI_landscape_forTablets();
                moveScreenAreas_landscape_forTablets(newAspect);
                // set the FOV such that the horizontal is 90.
                mainCam.fieldOfView = Camera.HorizontalToVerticalFieldOfView(normalLongerFOV, aspectRatio_wDivH);

                cameraRotationManager.setIsPortrait(false);
            } else {
                changeUI_landscape_forTallerPhones();
                moveScreenAreas_landscape_forTallerPhones(newAspect);
                // set the FOV to normalShorterFOV
                mainCam.fieldOfView = normalShorterFOV;

                cameraRotationManager.setIsPortrait(false);
            }
        }
    }



    private void changeUI_portrait_forTallerPhones()
    {
        // match the canvas to the width, and set the width to 1080.
        canvasScaler.matchWidthOrHeight = 0f;
        canvasScaler.referenceResolution = new Vector2(1080, 1080);
    }

    private void changeUI_portrait_forTablets()
    {
        // match the canvas to height, and set the height to 1920.
        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);
    }

    private void changeUI_landscape_forTablets()
    {
        // match the canvas width, and set the width to 1920.
        canvasScaler.matchWidthOrHeight = 0f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);
    }

    private void changeUI_landscape_forTallerPhones()
    {
        // match the canvas height, and set the height to 1080.
        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1080, 1080);
    }


    private void moveScreenAreas_portrait_forTallerPhones(float aspectRatio)
    {
        float screenWidth = 1080;
        float screenHeight = screenWidth / aspectRatio;

        buttonsAreaTransform.sizeDelta = new Vector2(screenWidth, 1080);
        cameraAreaTransform.sizeDelta = new Vector2(screenWidth, screenHeight - 1080);

        // float camHeight = (screenHeight-1080)/screenHeight;
        // mainCam.rect = new Rect(0, 1-camHeight, 1, camHeight);
        changeCameraAngle_portrait();
    }

    private void moveScreenAreas_portrait_forTablets(float aspectRatio)
    {
        float screenHeight = 1920;
        float screenWidth = aspectRatio * screenHeight;

        buttonsAreaTransform.sizeDelta = new Vector2(screenWidth, 1080);
        cameraAreaTransform.sizeDelta = new Vector2(screenWidth, screenHeight - 1080);

        // float camHeight = (screenHeight-1080)/screenHeight;
        // mainCam.rect = new Rect(0, 1-camHeight, 1, camHeight);
        changeCameraAngle_portrait();
    }

    private void moveScreenAreas_landscape_forTallerPhones(float aspectRatio)
    {
        float screenHeight = 1080;
        float screenWidth = aspectRatio * screenHeight;

        buttonsAreaTransform.sizeDelta = new Vector2(1080, screenHeight);
        cameraAreaTransform.sizeDelta = new Vector2(screenWidth - 1080, screenHeight);

        // float camWidth = (screenWidth-1080)/screenWidth;
        // mainCam.rect = new Rect(0, 0, camWidth, 1);
        changeCameraAngle_landscape();
    }

    private void moveScreenAreas_landscape_forTablets(float aspectRatio)
    {
        float screenWidth = 1920;
        float screenHeight = screenWidth / aspectRatio;

        buttonsAreaTransform.sizeDelta = new Vector2(1080, screenHeight);
        cameraAreaTransform.sizeDelta = new Vector2(screenWidth - 1080, screenHeight);

        // float camWidth = (screenWidth-1080)/screenWidth;
        // mainCam.rect = new Rect(0, 0, camWidth, 1);
        changeCameraAngle_landscape();
    }


    private void changeCameraAngle_portrait()
    {
        mainCam.transform.localEulerAngles = cameraAngle_portrait; 
    }

    private void changeCameraAngle_landscape()
    {
        mainCam.transform.localEulerAngles = cameraAngle_landscape;
    }
}
