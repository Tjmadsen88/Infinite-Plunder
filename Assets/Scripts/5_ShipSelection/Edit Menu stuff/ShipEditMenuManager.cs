using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

using NameGeneratorSpace;

public class ShipEditMenuManager : MonoBehaviour
{
    public ShipSelection shipSelection;
    public ShipTextureManager shipTextureManager;
    public ShipSelectionPopupManager popupManager;

    public TabButtonManager tabButtonManager;
    public SailPatternButtonManager sailPatternButtonManager;
    public CannonballPreviewManager cannonballPreviewManager;
    public CannonButtonManager cannonButtonManager;

    public GameObject editMenu_parent;
    private CanvasGroup editMenu_group;

    public Image cancelButtonGrey_image;
    public Image saveButtonGrey_image;

    public CanvasGroup tabButton_group;

    public GameObject window_SailPatterns;
    public GameObject window_Colors;
    public GameObject window_Cannons;
    public GameObject window_Name;
    public Text windowTitle;

    public Slider slider_red;
    public Slider slider_green;
    public Slider slider_blue;
    public Text text_redValue;
    public Text text_greenValue;
    public Text text_blueValue;

    public InputField text_shipName;

    public Sprite sprite_CancelGrey;
    public Sprite sprite_Next;
    public Sprite sprite_Back;
    public Sprite sprite_Save;

    private static readonly Color32 color_visible = Color.white;
    private static readonly Color32 color_invisible = new Color32(255, 255, 255, 0);

    private bool buttonsCanBePressed = false;
    private bool tabsCanBePressed = true;

    private Color32[] tempShipColor;
    private byte tempSailPatternSelection;
    private bool tempSailIsMirrored_horizontal;
    private bool tempSailIsMirrored_vertical;
    private byte tempCannonSelection;
    private int previousShipButtonIndex;

    private const byte slideInAnimationTime = 12;
    private const byte slideOutAnimationTime = 12;
    private const byte animationTime_tabs = 6;

    private bool menuIsOpen = false;

    private int openTab;

    private bool notChaningTabs;
    private bool somethingHasBeenChanged;
    private bool nextHasBeenPressed;

    private const byte windowID_pattern = 0;
    private const byte windowID_color = 1;
    private const byte windowID_cannon = 2;
    private const byte windowID_name = 3;

    private byte numOfTimesPressedOnRandomCannon_current = 0;
    private const byte numOfTimesPressedOnRandomCannon_target = 9;




    // Start is called before the first frame update
    void Start()
    {
        editMenu_group = editMenu_parent.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }




    public bool requestOpenMenu_editMenu(Color32[] shipColors, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical,
                                        byte cannonSelection, string shipName, int shipButtonIndex)
    {
        if (!menuIsOpen)
        {
            menuIsOpen = true;
            notChaningTabs = false;
            somethingHasBeenChanged = false;
            nextHasBeenPressed = false;
            
            tempShipColor = shipColors;
            tempSailPatternSelection = sailPatternSelection;
            tempSailIsMirrored_horizontal = sailIsMirrored_horizontal;
            tempSailIsMirrored_vertical = sailIsMirrored_vertical;
            tempCannonSelection = cannonSelection;
            previousShipButtonIndex = shipButtonIndex;
            if (tempCannonSelection == 10) numOfTimesPressedOnRandomCannon_current = numOfTimesPressedOnRandomCannon_target;

            text_shipName.text = shipName;
            window_Name.SetActive(false);

            sailPatternButtonManager.changeSelectedButton(tempSailPatternSelection);
            cannonButtonManager.changeSelectedButton(tempCannonSelection);
            cannonballPreviewManager.changeImageColor(tempShipColor[5]);
            // changeCannonballSizeBasedOnSelection(tempCannonSelection);

            cancelButtonGrey_image.sprite = sprite_CancelGrey;
            saveButtonGrey_image.sprite = sprite_Next;

            buttonPressed_tab_effect(0);

            editMenu_parent.SetActive(true);

            tabsCanBePressed = true;
            tabButton_group.alpha = 1;

            notChaningTabs = true;

            StartCoroutine("playSlideInAnimation");
            return true;
        }
        
        return false;
    }

    public bool requestCloseMenu_editMenu()
    {
        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;
            cannonballPreviewManager.requestMakeDisappear();
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Save/Cancel stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_cancel()
    {
        if (buttonsCanBePressed)
        {
            if (nextHasBeenPressed)
            {
                window_Name.SetActive(false);
                buttonPressed_tab_effect(openTab);

                cancelButtonGrey_image.sprite = sprite_CancelGrey;
                saveButtonGrey_image.sprite = sprite_Next;

                nextHasBeenPressed = false;
                StartCoroutine("playSlideInAnimation_tabs");
                tabsCanBePressed = true;
            } else if (somethingHasBeenChanged) {
                popupManager.requestOpenMenu_popup(Constants.popupID_cancel);
            } else {
                shipSelection.closeEditMenu();
            }
        }
    }


    public void buttonPressed_save()
    {
        if (buttonsCanBePressed)
        {
            if (nextHasBeenPressed)
            {
                if (previousShipButtonIndex >= 0)
                {
                    shipSelection.editShip(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempCannonSelection, text_shipName.text);
                } else {
                    shipSelection.createNewShip(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempCannonSelection, text_shipName.text);
                }
                shipSelection.closeEditMenu();
            } else {
                switchWindow(windowID_name);
                window_Name.SetActive(true);
                windowTitle.text = "Give this ship a fitting name:";

                cancelButtonGrey_image.sprite = sprite_Back;
                saveButtonGrey_image.sprite = sprite_Save;

                nextHasBeenPressed = true;
                StartCoroutine("playSlideOutAnimation_tabs");
                tabsCanBePressed = false;
            }
        }
    }

    private void userHasChangedSomething()
    {
        if (notChaningTabs)
            somethingHasBeenChanged = true;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Tab related stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_tab(int tabNum)
    {
        if (buttonsCanBePressed && tabsCanBePressed)
        {
            notChaningTabs = false;
            buttonPressed_tab_effect(tabNum);
            notChaningTabs = true;
        }
    }


    private void buttonPressed_tab_effect(int tabNum)
    {
        openTab = tabNum;
        tabButtonManager.changeSelectedButton(tabNum);

        switch(tabNum)
        {
            case 0: // Sail pattern selection:
                switchWindow(windowID_pattern);
                windowTitle.text = "Select a sail pattern:";
                // sailPatternButtonManager.changeSelectedButton(tempSailPatternSelection);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 1: // Sail color primary:
                switchWindow(windowID_color);
                windowTitle.text = "Primary sail color:";
                setAllSliderPositions(tempShipColor[0]);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 2: // Sail pattern color:
                switchWindow(windowID_color);
                windowTitle.text = "Sail pattern color:";
                setAllSliderPositions(tempShipColor[1]);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 3: // Mast and deck color:
                switchWindow(windowID_color);
                windowTitle.text = "Mast and deck color:";
                setAllSliderPositions(tempShipColor[2]);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 4: // Railing color:
                switchWindow(windowID_color);
                windowTitle.text = "Railing color:";
                setAllSliderPositions(tempShipColor[3]);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 5: // Hull color:
                switchWindow(windowID_color);
                windowTitle.text = "Hull color:";
                setAllSliderPositions(tempShipColor[4]);
                cannonballPreviewManager.requestMakeDisappear();
                break;

            case 6: // Cannon selection:
                switchWindow(windowID_cannon);
                windowTitle.text = "Select a cannon:";
                cannonballPreviewManager.requestMakeAppear();
                /// Other cannon stuff
                break;

            default: // Cannonball color:
                switchWindow(windowID_color);
                windowTitle.text = "Cannonball color:";
                setAllSliderPositions(tempShipColor[5]);
                cannonballPreviewManager.requestMakeAppear();
                break;
        }
    }


    private void switchWindow(byte windowID)
    {
        switch(windowID)
        {
            case windowID_pattern:
                window_SailPatterns.SetActive(true);
                window_Colors.SetActive(false);
                window_Cannons.SetActive(false);
                break;
                
            case windowID_color:
                window_SailPatterns.SetActive(false);
                window_Colors.SetActive(true);
                window_Cannons.SetActive(false);
                break;
                
            case windowID_cannon:
                window_SailPatterns.SetActive(false);
                window_Colors.SetActive(false);
                window_Cannons.SetActive(true);
                break;
                
            default: // Name selection window:
                window_SailPatterns.SetActive(false);
                window_Colors.SetActive(false);
                window_Cannons.SetActive(false);
                break;
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Sail Selection stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_sailPatternSelection(int selectionNum)
    {
        if (buttonsCanBePressed && tempSailPatternSelection != selectionNum)
        {
            tempSailPatternSelection = (byte)selectionNum;
            tempSailIsMirrored_horizontal = false;
            tempSailIsMirrored_vertical = false;
            shipTextureManager.adjustSailPattern(tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempShipColor[0], tempShipColor[1]);

            sailPatternButtonManager.changeSelectedButton(tempSailPatternSelection);
            
            userHasChangedSomething();
        }
    }

    public void buttonPressed_mirrorSail_horizontal()
    {
        if (buttonsCanBePressed)
        {
            tempSailIsMirrored_horizontal = !tempSailIsMirrored_horizontal;

            shipTextureManager.adjustSailPattern(tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempShipColor[0], tempShipColor[1]);
            
            userHasChangedSomething();
        }
    }

    public void buttonPressed_mirrorSail_vertical()
    {
        if (buttonsCanBePressed)
        {
            tempSailIsMirrored_vertical = !tempSailIsMirrored_vertical;

            shipTextureManager.adjustSailPattern(tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempShipColor[0], tempShipColor[1]);
            
            userHasChangedSomething();
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Color related stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_inputRed()
    {
        if (buttonsCanBePressed)
        {
            switch(openTab)
            {
                case 1: // Sail color primary:
                    popupManager.setTextFieldValue(tempShipColor[0].r);
                    break;

                case 2: // Sail pattern color:
                    popupManager.setTextFieldValue(tempShipColor[1].r);
                    break;

                case 3: // Mast and deck color:
                    popupManager.setTextFieldValue(tempShipColor[2].r);
                    break;

                case 4: // Railing color:
                    popupManager.setTextFieldValue(tempShipColor[3].r);
                    break;

                case 5: // Hull color:
                    popupManager.setTextFieldValue(tempShipColor[4].r);
                    break;

                default: // Cannonball color:
                    popupManager.setTextFieldValue(tempShipColor[5].r);
                    break;
            }

            popupManager.requestOpenMenu_popup(Constants.popupID_value_red);
        }
    }


    public void buttonPressed_inputGreen()
    {
        if (buttonsCanBePressed)
        {
            switch(openTab)
            {
                case 1: // Sail color primary:
                    popupManager.setTextFieldValue(tempShipColor[0].g);
                    break;

                case 2: // Sail pattern color:
                    popupManager.setTextFieldValue(tempShipColor[1].g);
                    break;

                case 3: // Mast and deck color:
                    popupManager.setTextFieldValue(tempShipColor[2].g);
                    break;

                case 4: // Railing color:
                    popupManager.setTextFieldValue(tempShipColor[3].g);
                    break;

                case 5: // Hull color:
                    popupManager.setTextFieldValue(tempShipColor[4].g);
                    break;

                default: // Cannonball color:
                    popupManager.setTextFieldValue(tempShipColor[5].g);
                    break;
            }

            popupManager.requestOpenMenu_popup(Constants.popupID_value_green);
        }
    }


    public void buttonPressed_inputBlue()
    {
        if (buttonsCanBePressed)
        {
            switch(openTab)
            {
                case 1: // Sail color primary:
                    popupManager.setTextFieldValue(tempShipColor[0].b);
                    break;

                case 2: // Sail pattern color:
                    popupManager.setTextFieldValue(tempShipColor[1].b);
                    break;

                case 3: // Mast and deck color:
                    popupManager.setTextFieldValue(tempShipColor[2].b);
                    break;

                case 4: // Railing color:
                    popupManager.setTextFieldValue(tempShipColor[3].b);
                    break;

                case 5: // Hull color:
                    popupManager.setTextFieldValue(tempShipColor[4].b);
                    break;

                default: // Cannonball color:
                    popupManager.setTextFieldValue(tempShipColor[5].b);
                    break;
            }

            popupManager.requestOpenMenu_popup(Constants.popupID_value_blue);
        }
    }


    private void setAllSliderPositions(Color32 colorsToSet)
    {
        slider_red.value = colorsToSet.r;
        slider_green.value = colorsToSet.g;
        slider_blue.value = colorsToSet.b;
    }


    public void moveSliderInCode(byte colorChannel, int colorValue)
    {
        switch(colorChannel)
        {
            case Constants.popupID_value_red:
                slider_red.value = colorValue;
                break;
                
            case Constants.popupID_value_green:
                slider_green.value = colorValue;
                break;
                
            default: //case Constants.popupID_value_blue:
                slider_blue.value = colorValue;
                break;
        }
    }


    public void sliderMoved_red(float sliderValue)
    {
        if (buttonsCanBePressed)
        {
            text_redValue.text = sliderValue.ToString();
            userHasChangedSomething();

            switch(openTab)
            {
                case 1: // Sail color primary:
                    tempShipColor[0].r = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 2: // Sail pattern color:
                    tempShipColor[1].r = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 3: // Mast and deck color:
                    tempShipColor[2].r = (byte)sliderValue;
                    shipTextureManager.adjustColor_MastAndDeck(tempShipColor[2]);
                    break;

                case 4: // Railing color:
                    tempShipColor[3].r = (byte)sliderValue;
                    shipTextureManager.adjustColor_Rails(tempShipColor[3]);
                    break;

                case 5: // Hull color:
                    tempShipColor[4].r = (byte)sliderValue;
                    shipTextureManager.adjustColor_Hull(tempShipColor[4]);
                    break;

                default: // Cannonball color:
                    tempShipColor[5].r = (byte)sliderValue;
                    cannonballPreviewManager.changeImageColor(tempShipColor[5]);
                    break;
            }
        }
    }


    public void sliderMoved_green(float sliderValue)
    {
        if (buttonsCanBePressed)
        {
            text_greenValue.text = sliderValue.ToString();
            userHasChangedSomething();

            switch(openTab)
            {
                case 1: // Sail color primary:
                    tempShipColor[0].g = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 2: // Sail pattern color:
                    tempShipColor[1].g = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 3: // Mast and deck color:
                    tempShipColor[2].g = (byte)sliderValue;
                    shipTextureManager.adjustColor_MastAndDeck(tempShipColor[2]);
                    break;

                case 4: // Railing color:
                    tempShipColor[3].g = (byte)sliderValue;
                    shipTextureManager.adjustColor_Rails(tempShipColor[3]);
                    break;

                case 5: // Hull color:
                    tempShipColor[4].g = (byte)sliderValue;
                    shipTextureManager.adjustColor_Hull(tempShipColor[4]);
                    break;

                default: // Cannonball color:
                    tempShipColor[5].g = (byte)sliderValue;
                    cannonballPreviewManager.changeImageColor(tempShipColor[5]);
                    break;
            }
        }
    }


    public void sliderMoved_blue(float sliderValue)
    {
        if (buttonsCanBePressed)
        {
            text_blueValue.text = sliderValue.ToString();
            userHasChangedSomething();

            switch(openTab)
            {
                case 1: // Sail color primary:
                    tempShipColor[0].b = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 2: // Sail pattern color:
                    tempShipColor[1].b = (byte)sliderValue;
                    shipTextureManager.adjustColor_Sails(tempShipColor[0], tempShipColor[1], tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
                    break;

                case 3: // Mast and deck color:
                    tempShipColor[2].b = (byte)sliderValue;
                    shipTextureManager.adjustColor_MastAndDeck(tempShipColor[2]);
                    break;

                case 4: // Railing color:
                    tempShipColor[3].b = (byte)sliderValue;
                    shipTextureManager.adjustColor_Rails(tempShipColor[3]);
                    break;

                case 5: // Hull color:
                    tempShipColor[4].b = (byte)sliderValue;
                    shipTextureManager.adjustColor_Hull(tempShipColor[4]);
                    break;

                default: // Cannonball color:
                    tempShipColor[5].b = (byte)sliderValue;
                    cannonballPreviewManager.changeImageColor(tempShipColor[5]);
                    break;
            }
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Cannon Selection stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_cannonSelection(int selectionNum)
    {
        if (buttonsCanBePressed)
        {
            if (selectionNum >= 0)
            {
                tempCannonSelection = (byte)selectionNum;
                numOfTimesPressedOnRandomCannon_current = 0;
            } else if (numOfTimesPressedOnRandomCannon_current == numOfTimesPressedOnRandomCannon_target)
            {
                tempCannonSelection = 10;
            } else {
                tempCannonSelection = 11;
                numOfTimesPressedOnRandomCannon_current++;
            }

            cannonButtonManager.changeSelectedButton(tempCannonSelection);
            // changeCannonballSizeBasedOnSelection(tempCannonSelection);

            userHasChangedSomething();
        }
    }

    private void changeCannonballSizeBasedOnSelection(byte selectionNum)
    {
        switch (selectionNum)
        {
            case 1: // tri-burst
                cannonballPreviewManager.changeImageScale(0.23094f);
                break;

            case 2: // grapeshot
                cannonballPreviewManager.changeImageScale(0.178885f);
                break;

            case 3: // rapid-fire
                cannonballPreviewManager.changeImageScale(0.126491f);
                break;

            case 5: // explosive
                cannonballPreviewManager.changeImageScale(0.565685f);
                break;

            // case 0: // reliable
            // case 4: // hot-shot
            // case 10: // secret!
            default: // random
                cannonballPreviewManager.changeImageScale(0.4f);
                break;
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Name related stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_randomizeName()
    {
        if (buttonsCanBePressed)
        {
            text_shipName.text = RandomNameGenerator.generateRandomName();
            userHasChangedSomething();
        }
    }


    public void buttonPressed_nameText()
    {
        if (buttonsCanBePressed)
        {
            text_shipName.Select();
        }
    }


    public void nameHasChanged()
    {
        userHasChangedSomething();
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Animation stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    IEnumerator playSlideInAnimation()
    {
        float amountToFade = 1f / (float)slideInAnimationTime;

        editMenu_group.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            editMenu_group.alpha += amountToFade;
            yield return null;
        }

        editMenu_group.alpha = 1f;

        buttonsCanBePressed = true;
    }


    IEnumerator playSlideOutAnimation()
    {
        float amountToFade = 1f / (float)slideOutAnimationTime;

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            editMenu_group.alpha -= amountToFade;
            yield return null;
        }

        editMenu_parent.SetActive(false);
        menuIsOpen = false;
    }
    

    IEnumerator playSlideInAnimation_tabs()
    {
        float amountToFade = 1f / (float)animationTime_tabs;

        tabButton_group.alpha = 0f;

        for (int index = 0; index<animationTime_tabs; index++) 
        {
            tabButton_group.alpha += amountToFade;
            yield return null;
        }

        tabButton_group.alpha = 1f;
    }


    IEnumerator playSlideOutAnimation_tabs()
    {
        float amountToFade = 1f / (float)animationTime_tabs;

        for (int index = 0; index<animationTime_tabs; index++) 
        {
            tabButton_group.alpha -= amountToFade;
            yield return null;
        }

        tabButton_group.alpha = 0f;
    }
}
