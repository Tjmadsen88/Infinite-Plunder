using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioManager_Title : MonoBehaviour
{
    private const float normalAspect_wDivH_portrait = 0.5625f;
    private const float normalAspect_wDivH_landscape = 1.7777777777777777777778f;

    public CanvasScaler canvasScaler;

    private Camera mainCam;

    private float aspectRatio_wDivH;

    private const float normalLongerFOV = 45f;
    private float normalShorterFOV;

    private bool finishedStarting = false;

    public GameObject customButton_portrait;
    public GameObject customButton_landscape;
    public GameObject customBG_portrait;
    public GameObject customBG_landscape;

    private RectTransform customButton_portrait_transform;
    private RectTransform customButton_landscape_transform;

    public RectTransform buttonText_money;
    public RectTransform buttonText_enemies;
    public RectTransform buttonText_keys;
    public RectTransform buttonText_area;

    public RectTransform buttonParent_money;
    public RectTransform buttonParent_enemies;
    public RectTransform buttonParent_keys;
    public RectTransform buttonParent_area;
    
    public RectTransform button1_money;
    public RectTransform button2_money;
    public RectTransform button3_money;
    public RectTransform button4_money;
    public RectTransform buttonRandom_money;
    public GameObject buttonCustom_portrait_money;
    public GameObject buttonCustom_landscape_money;
    public RectTransform buttonCustomText_money;

    public RectTransform button1_enemies;
    public RectTransform button2_enemies;
    public RectTransform button3_enemies;
    public RectTransform button4_enemies;
    public RectTransform buttonRandom_enemies;
    public GameObject buttonCustom_portrait_enemies;
    public GameObject buttonCustom_landscape_enemies;
    public RectTransform buttonCustomText_enemies;
    
    public RectTransform button1_keys;
    public RectTransform button2_keys;
    public RectTransform button3_keys;
    public RectTransform button4_keys;
    public RectTransform buttonRandom_keys;
    public GameObject buttonCustom_portrait_keys;
    public GameObject buttonCustom_landscape_keys;
    public RectTransform buttonCustomText_keys;    

    public RectTransform button1_area;
    public RectTransform button2_area;
    public RectTransform button3_area;
    public RectTransform button4_area;
    public RectTransform buttonRandom_area;
    public GameObject buttonCustom_portrait_area;
    public GameObject buttonCustom_landscape_area;
    public RectTransform buttonCustomText_area;

    private Vector2 posPortrait_buttonMoney = new Vector2(-438, -13);
    private Vector2 posPortrait_buttonEnemies = new Vector2(-213, -13);
    private Vector2 posPortrait_buttonKeys = new Vector2(13, -13);
    private Vector2 posPortrait_buttonArea = new Vector2(238, -13);
    private Vector2 posLandscape_buttonMoney = new Vector2(-11.5f, 289);
    private Vector2 posLandscape_buttonEnemies = new Vector2(-11.5f, 189);
    private Vector2 posLandscape_buttonKeys = new Vector2(-11.5f, 89);
    private Vector2 posLandscape_buttonArea = new Vector2(-11.5f, -11);

    private Vector2 posPortrait_money = new Vector2(0, 1387);
    private Vector2 posPortrait_enemies = new Vector2(0, 1087);
    private Vector2 posPortrait_keys = new Vector2(0, 787);
    private Vector2 posPortrait_area = new Vector2(0, 487);
    private Vector2 posLandscape_money = new Vector2(-560, 818);
    private Vector2 posLandscape_enemies = new Vector2(-260, 818);
    private Vector2 posLandscape_keys = new Vector2(40, 818);
    private Vector2 posLandscape_area = new Vector2(340, 818);

    private Vector2 posPortrait_button1 = new Vector2(-440, 0);
    private Vector2 posPortrait_button2 = new Vector2(-215, 0);
    private Vector2 posPortrait_button3 = new Vector2(10, 0);
    private Vector2 posPortrait_button4 = new Vector2(235, 0);
    private Vector2 posPortrait_buttonRandom = new Vector2(-440, -100);
    private Vector2 posPortrait_buttonCustomText = new Vector2(327, -101.5f);
    private Vector2 posLandscape_button1 = new Vector2(0, -100);
    private Vector2 posLandscape_button2 = new Vector2(0, -200);
    private Vector2 posLandscape_button3 = new Vector2(0, -300);
    private Vector2 posLandscape_button4 = new Vector2(0, -400);
    private Vector2 posLandscape_buttonRandom = new Vector2(0, 20);
    private Vector2 posLandscape_buttonCustomText = new Vector2(102, -596.5f);

    // private Vector2 posPortrait_buttonKeyRandom = new Vector2(-440, 0);
    // private Vector2 posPortrait_buttonKeyAuto = new Vector2(-215, 0);
    // private Vector2 posPortrait_buttonKeyCustomText = new Vector2(327, -1.5f);
    // private Vector2 posLandscape_buttonKeyRandom = new Vector2(0, 20);
    // private Vector2 posLandscape_buttonKeyAuto = new Vector2(0, -100);
    // private Vector2 posLandscape_buttonKeyCustomText = new Vector2(102, -296.5f);


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        normalShorterFOV = Camera.VerticalToHorizontalFieldOfView(normalLongerFOV, 9f/16f);

        customButton_portrait_transform = customButton_portrait.GetComponent<RectTransform>();
        customButton_landscape_transform = customButton_landscape.GetComponent<RectTransform>();

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

        changeCustomizationMenu_portrait();
    }

    private void changeUI_portrait_forTablets()
    {
        // match the canvas to height, and set the height to 1920.
        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);

        changeCustomizationMenu_portrait();
    }

    private void changeUI_landscape_forTablets()
    {
        // match the canvas width, and set the width to 1920.
        canvasScaler.matchWidthOrHeight = 0f;
        canvasScaler.referenceResolution = new Vector2(1920, 1920);

        changeCustomizationMenu_landscape();
    }

    private void changeUI_landscape_forTallerPhones()
    {
        // match the canvas height, and set the height to 1080.
        canvasScaler.matchWidthOrHeight = 1f;
        canvasScaler.referenceResolution = new Vector2(1080, 1080);

        changeCustomizationMenu_landscape();
    }


    private void changeCustomizationMenu_portrait()
    {
        customButton_portrait.SetActive(true);
        customButton_landscape.SetActive(false);

        buttonText_money.SetParent(customButton_portrait_transform, false);
        buttonText_enemies.SetParent(customButton_portrait_transform, false);
        buttonText_keys.SetParent(customButton_portrait_transform, false);
        buttonText_area.SetParent(customButton_portrait_transform, false);

        buttonText_money.anchoredPosition = posPortrait_buttonMoney;
        buttonText_enemies.anchoredPosition = posPortrait_buttonEnemies;
        buttonText_keys.anchoredPosition = posPortrait_buttonKeys;
        buttonText_area.anchoredPosition = posPortrait_buttonArea;


        customBG_portrait.SetActive(true);
        customBG_landscape.SetActive(false);

        buttonParent_money.anchoredPosition = posPortrait_money;
        buttonParent_enemies.anchoredPosition = posPortrait_enemies;
        buttonParent_keys.anchoredPosition = posPortrait_keys;
        buttonParent_area.anchoredPosition = posPortrait_area;

        changeButtonPositions_portrait();
    }

    private void changeCustomizationMenu_landscape()
    {
        customButton_portrait.SetActive(false);
        customButton_landscape.SetActive(true);

        buttonText_money.SetParent(customButton_landscape_transform, false);
        buttonText_enemies.SetParent(customButton_landscape_transform, false);
        buttonText_keys.SetParent(customButton_landscape_transform, false);
        buttonText_area.SetParent(customButton_landscape_transform, false);

        buttonText_money.anchoredPosition = posLandscape_buttonMoney;
        buttonText_enemies.anchoredPosition = posLandscape_buttonEnemies;
        buttonText_keys.anchoredPosition = posLandscape_buttonKeys;
        buttonText_area.anchoredPosition = posLandscape_buttonArea;


        customBG_portrait.SetActive(false);
        customBG_landscape.SetActive(true);

        buttonParent_money.anchoredPosition = posLandscape_money;
        buttonParent_enemies.anchoredPosition = posLandscape_enemies;
        buttonParent_keys.anchoredPosition = posLandscape_keys;
        buttonParent_area.anchoredPosition = posLandscape_area;

        changeButtonPositions_landscape();
    }


    private void changeButtonPositions_portrait()
    {
        button1_money.anchoredPosition = posPortrait_button1;
        button1_enemies.anchoredPosition = posPortrait_button1;
        button1_keys.anchoredPosition = posPortrait_button1;
        button1_area.anchoredPosition = posPortrait_button1;
        
        button2_money.anchoredPosition = posPortrait_button2;
        button2_enemies.anchoredPosition = posPortrait_button2;
        button2_keys.anchoredPosition = posPortrait_button2;
        button2_area.anchoredPosition = posPortrait_button2;
        
        button3_money.anchoredPosition = posPortrait_button3;
        button3_enemies.anchoredPosition = posPortrait_button3;
        button3_keys.anchoredPosition = posPortrait_button3;
        button3_area.anchoredPosition = posPortrait_button3;
        
        button4_money.anchoredPosition = posPortrait_button4;
        button4_enemies.anchoredPosition = posPortrait_button4;
        button4_keys.anchoredPosition = posPortrait_button4;
        button4_area.anchoredPosition = posPortrait_button4;
        
        buttonRandom_money.anchoredPosition = posPortrait_buttonRandom;
        buttonRandom_enemies.anchoredPosition = posPortrait_buttonRandom;
        buttonRandom_keys.anchoredPosition = posPortrait_buttonRandom;
        buttonRandom_area.anchoredPosition = posPortrait_buttonRandom;
        
        buttonCustomText_money.anchoredPosition = posPortrait_buttonCustomText;
        buttonCustomText_enemies.anchoredPosition = posPortrait_buttonCustomText;
        buttonCustomText_keys.anchoredPosition = posPortrait_buttonCustomText;
        buttonCustomText_area.anchoredPosition = posPortrait_buttonCustomText;

        // buttonRandom_keys.anchoredPosition = posPortrait_buttonKeyRandom;
        // button1_keys.anchoredPosition = posPortrait_buttonKeyAuto;
        // buttonCustomText_keys.anchoredPosition = posPortrait_buttonKeyCustomText;

        buttonCustom_portrait_money.SetActive(true);
        buttonCustom_portrait_enemies.SetActive(true);
        buttonCustom_portrait_keys.SetActive(true);
        buttonCustom_portrait_area.SetActive(true);

        buttonCustom_landscape_money.SetActive(false);
        buttonCustom_landscape_enemies.SetActive(false);
        buttonCustom_landscape_keys.SetActive(false);
        buttonCustom_landscape_area.SetActive(false);
    }

    private void changeButtonPositions_landscape()
    {
        button1_money.anchoredPosition = posLandscape_button1;
        button1_enemies.anchoredPosition = posLandscape_button1;
        button1_keys.anchoredPosition = posLandscape_button1;
        button1_area.anchoredPosition = posLandscape_button1;
        
        button2_money.anchoredPosition = posLandscape_button2;
        button2_enemies.anchoredPosition = posLandscape_button2;
        button2_keys.anchoredPosition = posLandscape_button2;
        button2_area.anchoredPosition = posLandscape_button2;
        
        button3_money.anchoredPosition = posLandscape_button3;
        button3_enemies.anchoredPosition = posLandscape_button3;
        button3_keys.anchoredPosition = posLandscape_button3;
        button3_area.anchoredPosition = posLandscape_button3;
        
        button4_money.anchoredPosition = posLandscape_button4;
        button4_enemies.anchoredPosition = posLandscape_button4;
        button4_keys.anchoredPosition = posLandscape_button4;
        button4_area.anchoredPosition = posLandscape_button4;
        
        buttonRandom_money.anchoredPosition = posLandscape_buttonRandom;
        buttonRandom_enemies.anchoredPosition = posLandscape_buttonRandom;
        buttonRandom_keys.anchoredPosition = posLandscape_buttonRandom;
        buttonRandom_area.anchoredPosition = posLandscape_buttonRandom;
        
        buttonCustomText_money.anchoredPosition = posLandscape_buttonCustomText;
        buttonCustomText_enemies.anchoredPosition = posLandscape_buttonCustomText;
        buttonCustomText_keys.anchoredPosition = posLandscape_buttonCustomText;
        buttonCustomText_area.anchoredPosition = posLandscape_buttonCustomText;

        // buttonRandom_keys.anchoredPosition = posLandscape_buttonKeyRandom;
        // button1_keys.anchoredPosition = posLandscape_buttonKeyAuto;
        // buttonCustomText_keys.anchoredPosition = posLandscape_buttonKeyCustomText;

        buttonCustom_portrait_money.SetActive(false);
        buttonCustom_portrait_enemies.SetActive(false);
        buttonCustom_portrait_keys.SetActive(false);
        buttonCustom_portrait_area.SetActive(false);

        buttonCustom_landscape_money.SetActive(true);
        buttonCustom_landscape_enemies.SetActive(true);
        buttonCustom_landscape_keys.SetActive(true);
        buttonCustom_landscape_area.SetActive(true);
    }


}
