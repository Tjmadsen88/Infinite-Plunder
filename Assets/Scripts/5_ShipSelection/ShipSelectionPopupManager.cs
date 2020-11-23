using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

public class ShipSelectionPopupManager : MonoBehaviour
{
    public ShipSelection shipSelection;
    public ShipEditMenuManager editMenuManager;

    public GameObject popupParent;

    public GameObject deleteBG;
    public GameObject cancelBG;
    public GameObject valueParent;
    public GameObject yesButton;

    public Image valueBG_image;

    public Sprite sprite_valueBG_red;
    public Sprite sprite_valueBG_green;
    public Sprite sprite_valueBG_blue;

    public InputField textInputField;

    private bool buttonsCanBePressed = false;
    private bool buttonsCanBePressed_confirm = false;
    private bool menuIsOpen = false;
    private byte openPopupID;

    private const byte slideInAnimationTime = 6;
    private const byte slideOutAnimationTime = 12;

    private const byte delayBeforeConfirm_max = 30;
    private byte delayBeforeConfirm_current = 0;



    // Update is called once per frame
    void Update()
    {
        if (buttonsCanBePressed && delayBeforeConfirm_current > 0)
        {
            delayBeforeConfirm_current--;
            if (delayBeforeConfirm_current == 0)
            {
                buttonsCanBePressed_confirm = true;

                if (openPopupID == Constants.popupID_delete)
                    yesButton.SetActive(true);
            }
        }
    }


    public bool requestOpenMenu_popup(byte popupID)
    {
        if (!menuIsOpen)
        {
            menuIsOpen = true;
            openPopupID = popupID;
            popupParent.SetActive(true);

            switch(popupID)
            {
                case Constants.popupID_delete:
                    deleteBG.SetActive(true);
                    delayBeforeConfirm_current = delayBeforeConfirm_max;
                    break;

                case Constants.popupID_cancel:
                    cancelBG.SetActive(true);
                    delayBeforeConfirm_current = 1;
                    break;

                case Constants.popupID_value_red:
                    valueParent.SetActive(true);
                    valueBG_image.sprite = sprite_valueBG_red;
                    delayBeforeConfirm_current = 1;
                    //update menu info to previous value
                    textInputField.Select();
                    break;

                case Constants.popupID_value_green:
                    valueParent.SetActive(true);
                    valueBG_image.sprite = sprite_valueBG_green;
                    delayBeforeConfirm_current = 1;
                    //update menu info to previous value
                    textInputField.Select();
                    break;

                default: //Constants.popupID_value_blue:
                    valueParent.SetActive(true);
                    valueBG_image.sprite = sprite_valueBG_blue;
                    delayBeforeConfirm_current = 1;
                    //update menu info to previous value
                    textInputField.Select();
                    break;
            }

            StartCoroutine("playSlideInAnimation");
            return true;
        }
        
        return false;
    }

    private bool requestCloseMenu_popup()
    {
        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;
            buttonsCanBePressed_confirm = false;
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }


    public void buttonPressed_cancel()
    {
        requestCloseMenu_popup();
    }

    public void buttonPressed_apply()
    {
        if (buttonsCanBePressed_confirm && requestCloseMenu_popup())
        {
            switch(openPopupID)
            {
                case Constants.popupID_delete:
                    shipSelection.deleteSelectedShip();
                    break;

                case Constants.popupID_cancel:
                    shipSelection.closeEditMenu();
                    break;

                // case Constants.popupID_value_red:
                // case Constants.popupID_value_green:
                default: //Constants.popupID_value_blue:
                    editMenuManager.moveSliderInCode(openPopupID, (byte)Mathf.Min(int.Parse(textInputField.text), 255));
                    break;
            }
        }
    }


    public void buttonPressed_textButton()
    {
        textInputField.Select();
    }



    public void setTextFieldValue(byte newValue)
    {
        textInputField.text = newValue.ToString();
    }


    

    IEnumerator playSlideInAnimation()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        CanvasGroup popupGroup = popupParent.GetComponent<CanvasGroup>();
        popupGroup.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            popupGroup.alpha += amountToFade;
            yield return null;
        }

        popupGroup.alpha = 1f;

        buttonsCanBePressed = true;
    }

    IEnumerator playSlideOutAnimation()
    {
        float amountToFade = 1f / (float)slideOutAnimationTime;

        CanvasGroup popupGroup = popupParent.GetComponent<CanvasGroup>();

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            popupGroup.alpha -= amountToFade;
            yield return null;
        }

        switch(openPopupID)
        {
            case Constants.popupID_delete:
                shipSelection.setButtonsCanBePressed(true);
                break;

            // case Constants.popupID_cancel:
            // case Constants.popupID_value_red:
            // case Constants.popupID_value_green:
            // case Constants.popupID_value_blue:
            default:
                // editMenu.setButtonsCanBePressed(true); ...or something.
                break;
        }

        deleteBG.SetActive(false);
        cancelBG.SetActive(false);
        valueParent.SetActive(false);
        yesButton.SetActive(false);

        popupParent.SetActive(false);
        menuIsOpen = false;
    }
}
