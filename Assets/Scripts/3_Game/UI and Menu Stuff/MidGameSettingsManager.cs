using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

public class MidGameSettingsManager : MonoBehaviour
{
    public GameView gameView;
    public TouchManager touchManager;

    public GameObject settingsMenu_parent;
    public GameObject settingsMenu_menu1;
    public GameObject settingsMenu_menu2;

    public Image toggleButton_normal;
    public Image toggleButton_reversed;
    public Image toggleButton_manual;
    public Image toggleButton_assisted;
    public Image toggleButton_auto;

    public Slider slider_angle;
    public Slider slider_distance;

    public GameObject underAllButton;

    public GameObject returnButton1_image;
    public GameObject returnButton1_hitbox;
    public GameObject returnButton2;

    private byte inactivityTimer = 0;
    private const byte slideInAnimationTime = 6;
    private const byte slideOutAnimationTime = 12;
    private const byte menuChangeTime = 6;

    private bool buttonsCanBePressed = true;


    private readonly Color invisible = new Color(1f, 1f, 1f, 0f);
    private readonly Color visible = new Color(1f, 1f, 1f, 1f);

    private const float slideOffscreenAmount = 100f;

    private bool return1CanBePressed = true;


    // Update is called once per frame
    void Update()
    {
        if (inactivityTimer > 0)
        {
            inactivityTimer--;
            if (inactivityTimer <= 0)
            {
                buttonsCanBePressed = true;
            }
        }
        
    }

    public void setInitialButtonValues(bool normalStickPositions, byte shootingMode, byte camAngleSetting, byte camDistSetting)
    {
        if (normalStickPositions)
        {
            toggleButton_normal.color = visible;
            toggleButton_reversed.color = invisible;
        } else {
            toggleButton_normal.color = invisible;
            toggleButton_reversed.color = visible;
        }

        switch (shootingMode)
        {
            case Constants.shootingMode_manual:
                toggleButton_manual.color = visible;
                toggleButton_assisted.color = invisible;
                toggleButton_auto.color = invisible;
                break;

            case Constants.shootingMode_assisted:
                toggleButton_manual.color = invisible;
                toggleButton_assisted.color = visible;
                toggleButton_auto.color = invisible;
                break;

            default: // auto
                toggleButton_manual.color = invisible;
                toggleButton_assisted.color = invisible;
                toggleButton_auto.color = visible;
                break;
        }

        slider_angle.value = (float)camAngleSetting;

        slider_distance.value = (float)camDistSetting;
    }



    public void buttonPressed_Normal()
    {
        toggleButton_normal.color = visible;
        toggleButton_reversed.color = invisible;

        touchManager.setThumbstickPositions(true);
    }

    public void buttonPressed_Reversed()
    {
        toggleButton_normal.color = invisible;
        toggleButton_reversed.color = visible;

        touchManager.setThumbstickPositions(false);
    }

    public void buttonPressed_Manual()
    {
        toggleButton_manual.color = visible;
        toggleButton_assisted.color = invisible;
        toggleButton_auto.color = invisible;

        touchManager.setShootingMode(Constants.shootingMode_manual);
    }

    public void buttonPressed_Assisted()
    {
        toggleButton_manual.color = invisible;
        toggleButton_assisted.color = visible;
        toggleButton_auto.color = invisible;

        touchManager.setShootingMode(Constants.shootingMode_assisted);
    }

    public void buttonPressed_Auto()
    {
        toggleButton_manual.color = invisible;
        toggleButton_assisted.color = invisible;
        toggleButton_auto.color = visible;

        touchManager.setShootingMode(Constants.shootingMode_auto);
    }

    public void buttonPressed_AdjustCamera()
    {
        settingsMenu_menu1.SetActive(false);
        settingsMenu_menu2.SetActive(true);
    }
    

    public void sliderMoved_CameraAngle(float sliderValue)
    {
        gameView.setCameraAngle((byte)sliderValue);
    }

    public void sliderMoved_CameraDistance(float sliderValue)
    {
        gameView.setCameraDistance((byte)sliderValue);
    }


    public bool requestOpenMenu()
    {
        if (buttonsCanBePressed)
        {
            inactivityTimer = slideInAnimationTime;
            buttonsCanBePressed = false;

            settingsMenu_parent.SetActive(true);
            settingsMenu_menu1.SetActive(true);
            settingsMenu_menu2.SetActive(false);

            underAllButton.SetActive(true);

            returnButton1_hitbox.SetActive(true);
            returnButton1_image.SetActive(true);

            // And make the menu slide in.
            StartCoroutine("playSlideInAnimation");

            return true;
        }
        
        return false;
    }

    public bool requestCloseMenu()
    {
        if (buttonsCanBePressed)
        {
            inactivityTimer = slideOutAnimationTime;
            buttonsCanBePressed = false;

            underAllButton.SetActive(false);

            returnButton1_hitbox.SetActive(false);

            // And make the menu slide out.
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }

    IEnumerator playSlideInAnimation()
    {
        Vector2 amountToMove = new Vector2(slideOffscreenAmount / (float)slideInAnimationTime, 0f);
        float amountToFade = 1f / (float)slideInAnimationTime;

        RectTransform parentRect = settingsMenu_parent.GetComponent<RectTransform>();
        CanvasGroup parentGroup = settingsMenu_parent.GetComponent<CanvasGroup>();
        Image returnButtonImage = returnButton1_image.GetComponent<Image>();

        parentRect.anchoredPosition = new Vector2(slideOffscreenAmount, 0f);
        parentGroup.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            parentRect.anchoredPosition -= amountToMove;
            parentGroup.alpha += amountToFade;
            returnButtonImage.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideInAnimationTime));
            yield return null;
        }

        parentRect.anchoredPosition = new Vector2(0f, 0f);
        parentGroup.alpha = 1f;
            returnButtonImage.color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator playSlideOutAnimation()
    {
        Vector2 amountToMove = new Vector2(slideOffscreenAmount / (float)slideOutAnimationTime, 0f);
        float amountToFade = 1f / (float)slideOutAnimationTime;

        RectTransform parentRect = settingsMenu_parent.GetComponent<RectTransform>();
        CanvasGroup parentGroup = settingsMenu_parent.GetComponent<CanvasGroup>();
        Image returnButtonImage = returnButton1_image.GetComponent<Image>();

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            parentRect.anchoredPosition += amountToMove;
            parentGroup.alpha -= amountToFade;
            returnButtonImage.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideOutAnimationTime));
            yield return null;
        }

        settingsMenu_parent.SetActive(false);
        returnButton1_image.SetActive(false);

        gameView.theMenuIsNowClosed();
    }


    public void absorbTouch()
    {
        //Debug.Log("Touch Absorbed!");
    }



    public void buttonPressed_return1()
    {
        if (return1CanBePressed)
        {
            // make the 'return1' hitbox disappear,
            return1CanBePressed = false;

            // make the 'return2' button slide in
            StartCoroutine("slideTheReturn2ButtonInAndOut");
        }
    }

    public void buttonPressed_return2()
    {
        // tell the GameView to return to the Title Screen.
        gameView.buttonPressed_returnToTitle();
    }

    IEnumerator slideTheReturn2ButtonInAndOut()
    {
        returnButton2.SetActive(true);

        // move the 'return2' button into place...
        float slideAmount = 50f;
        Vector2 amountToMove = new Vector2(slideAmount / (float)slideInAnimationTime, 0f);
        //float amountToFade = 1f / (float)slideInAnimationTime;

        RectTransform buttonRect = returnButton2.GetComponent<RectTransform>();
        Image buttonImage = returnButton2.GetComponent<Image>();

        buttonRect.anchoredPosition = new Vector2(-slideAmount + 15f, -15f);
        buttonImage.color = new Color(1f, 1f, 1f, 0f);

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            buttonRect.anchoredPosition += amountToMove;
            buttonImage.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideInAnimationTime));
            yield return null;
        }

        buttonRect.anchoredPosition = new Vector2(15f, -15f);
        buttonImage.color = new Color(1f, 1f, 1f, 1f);


        // have it stay there for a little bit,
        for (int index = 0; index<120; index++) 
        {
            yield return null;
        }


        // then have it slide back out.
        amountToMove = new Vector2(slideAmount / (float)slideOutAnimationTime, 0f);

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            buttonRect.anchoredPosition -= amountToMove;
            buttonImage.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideOutAnimationTime));
            yield return null;
        }

        // when it's all over, make the 'return1' hitbox appear again.
        return1CanBePressed = true;
        
        returnButton2.SetActive(false);
    }
}
