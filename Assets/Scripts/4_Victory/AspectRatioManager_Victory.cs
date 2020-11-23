using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioManager_Victory : MonoBehaviour
{
    private const float normalAspect_wDivH_portrait = 0.5625f;
    private const float normalAspect_wDivH_landscape = 1.7777777777777777777778f;

    public CanvasScaler canvasScaler;
    public RectTransform textParentRect;
    public RectTransform statsParentRect;

    private Camera mainCam;
    public Camera victoryCamera;
    public Camera defeatCamera;

    private float aspectRatio_wDivH;

    private const float normalLongerFOV = 45f;
    private float normalShorterFOV;

    private bool finishedStarting = false;
    private bool playerHasWon = true;

    public PersistentData persistentData;

    private readonly Vector2 noOffset = new Vector2(0f, 0f);
    private readonly Vector2 textParentOffset_landscape = new Vector2(-480f, 0f);
    private readonly Vector2 statsParentOffset_portrait = new Vector2(0f, -310f);

    private readonly Vector3 cameraAng_victory_portrait = new Vector3(16.6f, 212.9f, 0f);
    private readonly Vector3 cameraAng_victory_landscape = new Vector3(22.9f, 219.6f, 0f);

    // private readonly Vector3 cameraAng_defeat_portrait = new Vector3(8.42f, 241.2f, 0f);
    // private readonly Vector3 cameraAng_defeat_landscape = new Vector3(14.72f, 247.9f, 0f);

    private readonly Vector3 cameraAng_defeat_portrait = new Vector3(5.58f, 241.2f, 0f);
    private readonly Vector3 cameraAng_defeat_landscape = new Vector3(11.88f, 247.9f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        // mainCam = Camera.main;
        normalShorterFOV = Camera.VerticalToHorizontalFieldOfView(normalLongerFOV, 9f/16f);

        if (persistentData.getPlayerHasLost()) 
        {
            playerHasWon = false;
            mainCam = defeatCamera;
        } else {
            mainCam = victoryCamera;
        }
        // playerHasWon = false;

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
        textParentRect.anchoredPosition = noOffset;
        statsParentRect.anchoredPosition = statsParentOffset_portrait;

        canvasScaler.matchWidthOrHeight = 0f;
        canvasScaler.referenceResolution = new Vector2(1080, 1080);

        if (playerHasWon) mainCam.GetComponent<Transform>().eulerAngles = cameraAng_victory_portrait;
        else mainCam.GetComponent<Transform>().eulerAngles = cameraAng_defeat_portrait;
    }

    private void changeUI_portrait_forTablets()
    {
        // match the canvas to height, and set the height to 1920.
        textParentRect.anchoredPosition = noOffset;
        statsParentRect.anchoredPosition = statsParentOffset_portrait;

        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);

        if (playerHasWon) mainCam.GetComponent<Transform>().eulerAngles = cameraAng_victory_portrait;
        else mainCam.GetComponent<Transform>().eulerAngles = cameraAng_defeat_portrait;
    }

    private void changeUI_landscape_forTablets()
    {
        // match the canvas width, and set the width to 1920.
        textParentRect.anchoredPosition = textParentOffset_landscape;
        statsParentRect.anchoredPosition = noOffset;

        canvasScaler.matchWidthOrHeight = 0f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);

        if (playerHasWon) mainCam.GetComponent<Transform>().eulerAngles = cameraAng_victory_landscape;
        else mainCam.GetComponent<Transform>().eulerAngles = cameraAng_defeat_landscape;
    }

    private void changeUI_landscape_forTallerPhones()
    {
        // match the canvas height, and set the height to 1080.
        textParentRect.anchoredPosition = textParentOffset_landscape;
        statsParentRect.anchoredPosition = noOffset;

        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1080, 1080);

        if (playerHasWon) mainCam.GetComponent<Transform>().eulerAngles = cameraAng_victory_landscape;
        else mainCam.GetComponent<Transform>().eulerAngles = cameraAng_defeat_landscape;
    }
}
