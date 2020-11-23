using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ShipDataSpace;

public class ShipButtonManager : MonoBehaviour
{
    public ShipSelection shipSelection;

    public GameObject selectionMenu_parent;
    private CanvasGroup selectionMenu_group;

    public Image shipButton_new_image;
    public Text shipButton_new_text;
    public RectTransform shipButton_new_transform;

    public GameObject shipButtonsParent;
    private RectTransform shipButtonsParent_transform;
    public RectTransform shipButtons_ScrollAreaTransform;
    public Scrollbar shipButtons_scrollbar;

    private ShipButtonScript[] shipButtonArray;
    private int numOfShipButtons = 0;
    private int currentlySelectedButton = 0;
    
    public GameObject prefab_shipButton;

    public GameObject selectRandomButton;
    public GameObject editSelectedButton;
    public GameObject deleteSelectedButton;
    public GameObject reorderUpButton;
    public GameObject reorderDownButton;

    private Image selectRandomButton_Image;
    private Image editSelectedButton_Image;

    public Sprite sprite_selectRandomButton_selected;
    public Sprite sprite_selectRandomButton_greyed;
    public Sprite sprite_editSelected_normal;
    public Sprite sprite_editSelected_faded;
    public Sprite sprite_createNew;

    public Sprite sprite_shipButton_normal_selected;
    public Sprite sprite_shipButton_normal_greyed;
    public Sprite sprite_shipButton_new_selected;
    public Sprite sprite_shipButton_new_greyed;

    private Color32 color_textBrown = new Color32(120, 71, 31, 255);
    private Color32 color_textWhite = Color.white;
    private Color32 color_textBlack = new Color32(80, 80, 80, 255);

    private const float buttonTopMargin = -41;

    private bool firstButtonSelected = false;

    private const byte slideInAnimationTime = 12;
    private const byte slideOutAnimationTime = 12;

    private bool menuIsOpen = true;
    private bool buttonsCanBePressed = true;


    // Start is called before the first frame update
    void Awake()
    {
        selectionMenu_group = selectionMenu_parent.GetComponent<CanvasGroup>();

        selectRandomButton_Image = selectRandomButton.GetComponent<Image>();
        editSelectedButton_Image = editSelectedButton.GetComponent<Image>();

        shipButtonsParent_transform = shipButtonsParent.GetComponent<RectTransform>();
    }

    // ------------------------------------------------------------------------------------------------------
    // ----------- Button selection stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------
    
    public void selectButton(int shipButtonIndex)
    {
        if (buttonsCanBePressed)
        {
            selectButton_effect(shipButtonIndex);
        }
    }
    
    public void selectButton_effect(int shipButtonIndex)
    {
        deselectPreviousShipButton();
        currentlySelectedButton = shipButtonIndex;

        switch (shipButtonIndex)
        {
            case -2: // 'New' is selected.
                selectButton_createNew();
                ensureSelectedButtonIsOnScreen(-2);
                break;

            case -1: // 'Random' is selected.
                selectButton_selectRandom();
                break;

            default: // A ship button is selected.
                selectButton_shipButton(shipButtonIndex);
                ensureSelectedButtonIsOnScreen(shipButtonIndex);
                break;
        }
    }

    private void selectButton_createNew()
    {
        // shipButton_new_image.sprite = sprite_shipButton_new_selected;
        shipButton_new_image.sprite = sprite_shipButton_normal_selected;
        shipButton_new_text.color = color_textWhite;

        editSelectedButton_Image.sprite = sprite_createNew;
        deleteSelectedButton.SetActive(false);
        reorderUpButton.SetActive(false);
        reorderDownButton.SetActive(false);
    }

    private void selectButton_selectRandom()
    {
        selectRandomButton_Image.sprite = sprite_selectRandomButton_selected;
        
        editSelectedButton_Image.sprite = sprite_editSelected_faded;
        deleteSelectedButton.SetActive(false);
        reorderUpButton.SetActive(false);
        reorderDownButton.SetActive(false);
    }

    private void selectButton_shipButton(int shipButtonIndex)
    {
        shipButtonArray[shipButtonIndex].changeColors(sprite_shipButton_normal_selected, color_textWhite, color_textBlack);
        
        editSelectedButton_Image.sprite = sprite_editSelected_normal;
        deleteSelectedButton.SetActive(true);

        if (shipButtonIndex > 0) reorderUpButton.SetActive(true);
        else reorderUpButton.SetActive(false);
        
        if (shipButtonIndex < numOfShipButtons-1) reorderDownButton.SetActive(true);
        else reorderDownButton.SetActive(false);
    }

    private void deselectPreviousShipButton()
    {
        switch (currentlySelectedButton)
        {
            case -2: // 'New' is pressed.
                shipButton_new_image.sprite = sprite_shipButton_new_greyed;
                shipButton_new_text.color = color_textBrown;
                break;

            case -1: // 'Random' is pressed.
                selectRandomButton_Image.sprite = sprite_selectRandomButton_greyed;
                break;

            default: // A ship button is pressed.
                shipButtonArray[currentlySelectedButton].changeColors(sprite_shipButton_normal_greyed, color_textBrown, color_textBrown);
                break;
        }
    }

    // ------------------------------------------------------------------------------------------------------
    // ----------- Array management stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void loadShipButtons(ShipDataPacket shipDataPacket)
    {
        ShipDataIndividual[] shipData = shipDataPacket.getShipData();
        int numOfSavedShips = shipDataPacket.getNumOfSavedShips();
        
        shipButtonArray = new ShipButtonScript[numOfSavedShips];

        for (int index=0; index<numOfSavedShips; index++)
        {
            createNewShipButton(index, shipData[index].getShipName(), shipData[index].getCannonSelection());
        }

        moveShipButtonNewToTheEndOfTheList();

        currentlySelectedButton = shipDataPacket.getPreviouslySelectedShip();

        if (currentlySelectedButton >= 0)
            shipSelection.buttonPressed_ShipButton(currentlySelectedButton);
        else 
        {
            shipSelection.buttonPressed_SelectRandom();
            firstButtonSelected = true;
        }
    }


    private void createNewShipButton(int shipButtonIndex, string shipName, byte cannonSelection)
    {
        if (shipButtonIndex >= shipButtonArray.Length)
            expandShipButtonArray();

        GameObject newButton = Instantiate(prefab_shipButton);

        shipButtonArray[shipButtonIndex] = newButton.GetComponent<ShipButtonScript>();
        shipButtonArray[shipButtonIndex].setTexts(shipName, getEquipStringFromCannonSelection(cannonSelection));

        RectTransform newButton_transform = newButton.GetComponent<RectTransform>();

        newButton_transform.SetParent(shipButtonsParent_transform);
        newButton_transform.anchoredPosition = new Vector3(0f, -46 - 143*shipButtonIndex);
        newButton_transform.localScale = Vector3.one;

        shipSelection.attachButtonListenerToButton(shipButtonArray[shipButtonIndex], shipButtonIndex);

        numOfShipButtons++;
    }


    public void createNewShipButtonAtEnd_andSelectIt(string shipName, byte cannonSelection)
    {
        createNewShipButton(numOfShipButtons, shipName, cannonSelection);
        moveShipButtonNewToTheEndOfTheList();

        shipSelection.buttonPressed_ShipButton_effect(numOfShipButtons-1);
    }

    private void expandShipButtonArray()
    {
        int arrayLength = shipButtonArray.Length;
        ShipButtonScript[] newArray = new ShipButtonScript[arrayLength +4];

        for (int index=0; index<arrayLength; index++)
        {
            newArray[index] = shipButtonArray[index];
        }

        shipButtonArray = newArray;
    }

    public void removeShipButton(int shipButtonIndex)
    {
        for (int index=shipButtonIndex; index<numOfShipButtons-1; index++)
        {
            shipButtonArray[index].copyTextFromOtherButton(shipButtonArray[index+1]);
        }

        shipButtonArray[numOfShipButtons-1].prepareToRemoveButton();
        numOfShipButtons--;
        moveShipButtonNewToTheEndOfTheList();


        if (currentlySelectedButton >= numOfShipButtons)
            currentlySelectedButton--;

        if (currentlySelectedButton >= 0)
            shipSelection.buttonPressed_ShipButton(currentlySelectedButton);
        else shipSelection.buttonPressed_SelectNew();
    }

    public void updateShipButton(int shipButtonIndex, string newShipName, byte cannonSelection)
    {
        shipButtonArray[shipButtonIndex].setTexts(newShipName, getEquipStringFromCannonSelection(cannonSelection));
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Scroll area management stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void moveShipButtonNewToTheEndOfTheList()
    {
        shipButton_new_transform.anchoredPosition = new Vector3(0f, -46 - 143*numOfShipButtons);

        shipButtons_ScrollAreaTransform.sizeDelta = new Vector2(0f, 34 + 143*(numOfShipButtons+3));
    }

    private void ensureSelectedButtonIsOnScreen(int shipButtonIndex)
    {
        if (!firstButtonSelected)
        {
            firstButtonSelected = true;
            centerMenuOnButton(shipButtonIndex);
            return;
        }

        float shipButtonTop;
        float shipButtonBottom;

        if (shipButtonIndex >= 0)
        {
            shipButtonTop = -36 -143 * shipButtonIndex;
            shipButtonBottom = -56 -143*(shipButtonIndex+1);
        } else {
            shipButtonTop = -36 -143 * numOfShipButtons;
            shipButtonBottom = -56 -143*(numOfShipButtons+1);
        }

        float totalHeight = 34 + 143*(numOfShipButtons+3);
        float bottomMargin = 235;
        float windowOffset_top = (shipButtons_scrollbar.value -1) * totalHeight * (1-shipButtons_scrollbar.size);
        float windowOffset_bottom = windowOffset_top - (totalHeight * shipButtons_scrollbar.size) +bottomMargin;

        if (shipButtonTop > windowOffset_top)
        {
            // shipButtons_scrollbar.value = (shipButtonTop+36) / (totalHeight * (1-shipButtons_scrollbar.size)) +1;
            shipButtons_scrollbar.value = Mathf.Min((shipButtonTop+94) / (totalHeight * (1-shipButtons_scrollbar.size)) +1, 1);
            // Debug.Log("Scrollbar moved down to value: "+shipButtons_scrollbar.value);
        } else if (shipButtonBottom < windowOffset_bottom)
        {
            // shipButtons_scrollbar.value = ((shipButtonBottom-29) +(windowOffset_top - windowOffset_bottom)) / (totalHeight * (1-shipButtons_scrollbar.size)) +1;
            shipButtons_scrollbar.value = Mathf.Max( ((shipButtonBottom-94) +(windowOffset_top - windowOffset_bottom)) / (totalHeight * (1-shipButtons_scrollbar.size)) +1, 0f);
            // Debug.Log("Scrollbar moved up to value: "+shipButtons_scrollbar.value);
        }
    }

    private void centerMenuOnButton(int shipButton)
    {
        // shipButtonsParent_transform.anchoredPosition = new Vector3(0, Mathf.Max(0f, -201 +143*shipButton), 0f);
        shipButtonsParent_transform.anchoredPosition = new Vector3(0, Mathf.Max(0f, -225 +143*shipButton), 0f);
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Fading in and out stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public bool requestOpenMenu_selectionMenu()
    {
        if (!menuIsOpen)
        {
            selectionMenu_parent.SetActive(true);
            StartCoroutine("playSlideInAnimation");
            return true;
        }
        return false;
    }

    public bool requestCloseMenu_selectionMenu()
    {
        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;
            StartCoroutine("playSlideOutAnimation");
            return true;
        }
        return false;
    }

    IEnumerator playSlideInAnimation()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        selectionMenu_group.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            selectionMenu_group.alpha += amountToFade;
            yield return null;
        }

        selectionMenu_group.alpha = 1f;

        buttonsCanBePressed = true;
    }


    IEnumerator playSlideOutAnimation()
    {
        float amountToFade = 1f / (float)slideOutAnimationTime;

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            selectionMenu_group.alpha -= amountToFade;
            yield return null;
        }

        selectionMenu_parent.SetActive(false);
        menuIsOpen = false;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Other stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void swapShipButtons(int selectedButton, int otherButton)
    {
        string tempName = shipButtonArray[selectedButton].getNameText().text;
        string tempEquip = shipButtonArray[selectedButton].getEquipText().text;

        shipButtonArray[selectedButton].copyTextFromOtherButton(shipButtonArray[otherButton]);
        shipButtonArray[otherButton].setTexts(tempName, tempEquip);

        shipButtonArray[selectedButton].changeColors(sprite_shipButton_normal_greyed, color_textBrown, color_textBrown);
        // shipButtonArray[otherButton].changeColors(sprite_shipButton_normal_selected, color_textWhite, color_textBlack);

        currentlySelectedButton = otherButton;

        shipSelection.buttonPressed_ShipButton(currentlySelectedButton);
    }

    private string getEquipStringFromCannonSelection(byte cannonSelection)
    {
        switch (cannonSelection)
        {  
            case 0:
                return "equipped with a reliable cannon";

            case 1:
                return "equipped with a grapeshot cannon";

            case 2:
                return "equipped with a chain-shot cannon";

            case 3:
                return "equipped with a rapid-fire cannon";

            case 4:
                return "equipped with a mystical cannon";

            case 5:
                return "equipped with an explosive cannon";

            case 6:
                return "equipped with a hot-shot cannon";

            case 10:
                return "equipped with the secret weapon";
            
            default:
                return "equipped with a random cannon";
        }
    }
}
