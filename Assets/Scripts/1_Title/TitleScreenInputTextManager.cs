using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenInputTextManager : MonoBehaviour
{
    public TitleScreenCustomizationMenuManager customizationMenuManager;

    public GameObject textInput_parent;
    public Image textInput_BGImage;
    public InputField textInput_inputField;
    // public Text textInput_visibleText;

    private bool buttonsCanBePressed = false;
    private bool menuIsOpen = false;
    private byte openTextCategory;

    private const byte slideInAnimationTime = 6;
    private const byte slideOutAnimationTime = 12;
    // private const float slideOffscreenAmount = -100f;

    private int defaultVal_money = 375000;
    private int defaultVal_enemies = 150;
    private int defaultVal_keys = 3;
    private int defaultVal_area = 1;

    private const int maxVal_money = 999999;
    private const int maxVal_enemies = 999;
    private const int maxVal_keys = 6;
    private const int maxVal_area = 9;

    public Sprite menuBG_money;
    public Sprite menuBG_enemies;
    public Sprite menuBG_keys;
    public Sprite menuBG_area;




    public void setInitialDefaultValues(int customMoney, float customEnemies, byte customKeys, byte customArea)
    {
        defaultVal_money = customMoney;
        defaultVal_enemies = (int)Mathf.Round(customEnemies * 100);
        defaultVal_keys = customKeys;
        defaultVal_area = customArea;
    }

    public bool requestOpenTextMenu(int categoryNum)
    {
        if (!menuIsOpen)
        {
            menuIsOpen = true;

            openTextCategory = (byte)categoryNum;

            textInput_parent.SetActive(true);

            updateMenuInfo(categoryNum);

            textInput_inputField.Select();

            // // And make the menu slide in.
            StartCoroutine("playSlideInAnimation");

            return true;
        }
        
        return false;
    }

    private bool requestCloseTextMenu()
    {
        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;

            // // And make the menu slide out.
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }


    public void buttonPressed_cancel()
    {
        requestCloseTextMenu();
    }

    public void buttonPressed_apply()
    {
        if (requestCloseTextMenu())
        {
            convertInputStringToValue();
            tellCustomizationMenuSomethingHasChanged();
        }
    }

    public void buttonPressed_textButton()
    {
        textInput_inputField.Select();
    }

    // public void updateVisibleText()
    // {
    //     switch(openTextCategory)
    //     {
    //         case 1:
    //             textInput_visibleText.text = int.Parse(textInput_inputField.text).ToString("n0");
    //             break;
                
    //         case 2:
    //             textInput_visibleText.text = string.Format("{0}%", int.Parse(textInput_inputField.text));
    //             break;
                
    //         case 3:
    //             textInput_visibleText.text = string.Format("{0}", int.Parse(textInput_inputField.text));
    //             break;

    //         case 4:
    //             textInput_visibleText.text = string.Format("+{0}", int.Parse(textInput_inputField.text));
    //             break;
            
    //         default:
    //             break;
    //     }
    // }



    private void updateMenuInfo(int categoryNum)
    {
        switch (categoryNum)
        {
            case 1:
                textInput_inputField.text = string.Format("{0}", defaultVal_money);
                textInput_BGImage.sprite = menuBG_money;
                break;
                
            case 2:
                textInput_inputField.text = string.Format("{0}", defaultVal_enemies);
                textInput_BGImage.sprite = menuBG_enemies;
                break;
                
            case 3:
                textInput_inputField.text = string.Format("{0}", defaultVal_keys);
                textInput_BGImage.sprite = menuBG_keys;
                break;
                
            case 4:
                textInput_inputField.text = string.Format("{0}", defaultVal_area);
                textInput_BGImage.sprite = menuBG_area;
                break;

            default:
                break;
        }
    }

    private void convertInputStringToValue()
    {
        switch(openTextCategory)
        {
            case 1:
                defaultVal_money = Mathf.Min(int.Parse(textInput_inputField.text), maxVal_money);
                break;
                
            case 2:
                defaultVal_enemies = Mathf.Min(int.Parse(textInput_inputField.text), maxVal_enemies);
                break;
                
            case 3:
                defaultVal_keys = Mathf.Min(int.Parse(textInput_inputField.text), maxVal_keys);
                break;

            case 4:
                defaultVal_area = Mathf.Min(int.Parse(textInput_inputField.text), maxVal_area);
                break;
            
            default:
                break;
        }
    }

    private void tellCustomizationMenuSomethingHasChanged()
    {
        switch(openTextCategory)
        {
            case 1:
                customizationMenuManager.updateCustom_money(defaultVal_money);
                break;
                
            case 2:
                customizationMenuManager.updateCustom_enemies(defaultVal_enemies);
                break;
                
            case 3:
                customizationMenuManager.updateCustom_keys(defaultVal_keys);
                break;

            case 4:
                customizationMenuManager.updateCustom_area(defaultVal_area);
                break;
            
            default:
                break;
        }
    }



    

    IEnumerator playSlideInAnimation()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        CanvasGroup textInputGroup = textInput_parent.GetComponent<CanvasGroup>();
        textInputGroup.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            textInputGroup.alpha += amountToFade;
            yield return null;
        }

        textInputGroup.alpha = 1f;

        buttonsCanBePressed = true;
    }

    IEnumerator playSlideOutAnimation()
    {
        float amountToFade = 1f / (float)slideOutAnimationTime;

        CanvasGroup textInputGroup = textInput_parent.GetComponent<CanvasGroup>();

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            textInputGroup.alpha -= amountToFade;
            yield return null;
        }

        customizationMenuManager.setButtonsCanBePressed(true);

        textInput_parent.SetActive(false);
        menuIsOpen = false;
    }
}
