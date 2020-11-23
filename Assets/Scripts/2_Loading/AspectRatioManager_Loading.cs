using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioManager_Loading : MonoBehaviour
{
    private const float normalAspect_wDivH_portrait = 0.5625f;
    private const float normalAspect_wDivH_landscape = 1.7777777777777777777778f;

    public CanvasScaler canvasScaler;

    private Camera mainCam;

    private float aspectRatio_wDivH;

    private const float normalLongerFOV = 45f;
    private float normalShorterFOV;

    private bool finishedStarting = false;


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
                // set the FOV such that the horizontal FOV is the same as normalShorterFOV
                mainCam.fieldOfView = Camera.HorizontalToVerticalFieldOfView(normalShorterFOV, aspectRatio_wDivH);
            } else if (aspectRatio_wDivH <= 1f)
            {
                changeUI_portrait_forTablets();
                // set the FOV to 90
                mainCam.fieldOfView = normalLongerFOV;
            } else if (aspectRatio_wDivH <= normalAspect_wDivH_landscape)
            {
                changeUI_landscape_forTablets();
                // set the FOV such that the horizontal is 90.
                mainCam.fieldOfView = Camera.HorizontalToVerticalFieldOfView(normalLongerFOV, aspectRatio_wDivH);
            } else {
                changeUI_landscape_forTallerPhones();
                // set the FOV to normalShorterFOV
                mainCam.fieldOfView = normalShorterFOV;
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
}
