using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelectionDropdownManager : MonoBehaviour
{
    public GameObject dropDownMenu;
    private CanvasGroup dropDownMenu_group;
    private RectTransform dropDownMenu_transform;
    public Image dropDownShadow;

    public GameObject dropDownButton_button;

    private bool dropDownMenuIsOpen = false;
    private bool buttonsCanBePressed = false;
    
    private const byte slideInAnimationTime_dropDownMenu = 6;
    private const byte slideOutAnimationTime_dropDownMenu = 12;
    private const float slideOffscreenAmount_dropDownMenu = 100f;

    private Color32 color_invisible = new Color32(0, 0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        dropDownMenu_group = dropDownMenu.GetComponent<CanvasGroup>();
        dropDownMenu_transform = dropDownMenu.GetComponent<RectTransform>();
    }


    public bool getButtonsCanBePressed()
    {
        return buttonsCanBePressed;
    }


    
    public bool changeMenuState()
	{
        if (dropDownMenuIsOpen) requestCloseMenu_dropDownMenu();
        else requestOpenMenu_dropDownMenu();

        return dropDownMenuIsOpen;
	}


    public bool requestOpenMenu_dropDownMenu()
    {
        if (!dropDownMenuIsOpen)
        {
            dropDownMenuIsOpen = true;

            dropDownButton_button.SetActive(false);
            dropDownMenu.SetActive(true);

            // And make the menu slide in.
            StartCoroutine("playSlideInAnimation");

            return true;
        }
        
        return false;
    }

    private bool requestCloseMenu_dropDownMenu()
    {
        if (dropDownMenuIsOpen)
        {
            dropDownMenuIsOpen = false;
            buttonsCanBePressed = false;

            dropDownButton_button.SetActive(false);

            // And make the menu slide out.
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }

    IEnumerator playSlideInAnimation()
    {
        Vector2 amountToMove = new Vector2(0f, slideOffscreenAmount_dropDownMenu / (float)slideInAnimationTime_dropDownMenu);
        float amountToFade = 1f / (float)slideInAnimationTime_dropDownMenu;

        dropDownMenu_transform.anchoredPosition = new Vector2(0f, slideOffscreenAmount_dropDownMenu);
        dropDownMenu_group.alpha = 0f;
        dropDownShadow.color = color_invisible;

        for (int index = 0; index<slideInAnimationTime_dropDownMenu; index++) 
        {
            dropDownMenu_transform.anchoredPosition -= amountToMove;
            dropDownMenu_group.alpha += amountToFade;
            dropDownShadow.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideInAnimationTime_dropDownMenu));
            yield return null;
        }

        dropDownMenu_transform.anchoredPosition = new Vector2(0f, 0f);
        dropDownMenu_group.alpha = 1f;
        dropDownShadow.color = new Color(1f, 1f, 1f, 1f);

        dropDownButton_button.SetActive(true);
        buttonsCanBePressed = true;
    }

    IEnumerator playSlideOutAnimation()
    {
        Vector2 amountToMove = new Vector2(0f, slideOffscreenAmount_dropDownMenu / (float)slideOutAnimationTime_dropDownMenu);
        float amountToFade = 1f / (float)slideOutAnimationTime_dropDownMenu;

        for (int index = 0; index<slideOutAnimationTime_dropDownMenu; index++) 
        {
            dropDownMenu_transform.anchoredPosition += amountToMove;
            dropDownMenu_group.alpha -= amountToFade;
            dropDownShadow.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideInAnimationTime_dropDownMenu));
            yield return null;
        }

        dropDownMenu.SetActive(false);
        
        dropDownShadow.color = color_invisible;

        dropDownButton_button.SetActive(true);
    }
}
