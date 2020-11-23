using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

public class TitleScreenCustomizationMenuManager : MonoBehaviour
{
    public TitleScreen titleScreen;
    public TitleScreenInputTextManager inputTextManager;

    public GameObject playButton;
    public GameObject customizeButtonParent;

    public GameObject customizeParent;

    public GameObject cancelButton;
    public GameObject saveButton;

    public Text text_buttonMoney;
    public Text text_buttonEnemies;
    public Text text_buttonKeys;
    public Text text_buttonArea;

    public Image button_money_1;
    public Image button_money_2;
    public Image button_money_3;
    public Image button_money_4;
    public Image button_money_random;
    public Image button_money_custom_portrait;
    public Image button_money_custom_landscape;
    public Text text_customMoney;

    public Image button_enemies_1;
    public Image button_enemies_2;
    public Image button_enemies_3;
    public Image button_enemies_4;
    public Image button_enemies_random;
    public Image button_enemies_custom_portrait;
    public Image button_enemies_custom_landscape;
    public Text text_customEnemies;

    public Image button_keys_1;
    public Image button_keys_2;
    public Image button_keys_3;
    public Image button_keys_4;
    public Image button_keys_random;
    public Image button_keys_custom_portrait;
    public Image button_keys_custom_landscape;
    public Text text_customKeys;

    public Image button_area_1;
    public Image button_area_2;
    public Image button_area_3;
    public Image button_area_4;
    public Image button_area_random;
    public Image button_area_custom_portrait;
    public Image button_area_custom_landscape;
    public Text text_customArea;

    public GameObject underAllButton;

    public PersistentData persistentData;
    
    private byte money_selection_temp;
    private byte enemies_selection_temp;
    private byte keys_selection_temp;
    private byte area_selection_temp;    

    private int money_value_temp;
    private float enemies_value_temp;
    private byte keys_value_temp;
    private byte area_value_temp;

    private int money_custom_temp;
    private float enemies_custom_temp;
    private byte keys_custom_temp;
    private byte area_custom_temp;

    private const byte slideInAnimationTime = 12;
    private const byte slideOutAnimationTime = 12;

    private bool buttonsCanBePressed = false;
    private bool menuIsOpen = false;

    // private const float slideOffscreenAmount = -300f;
    private const float slideOffscreenAmount = -100f;


    private Color32 textColor_white = new Color32(255, 255, 255, 255);
    private Color32 textColor_darker = new Color32(138, 118, 90, 255);
    private Color32 buttonColor_visible = new Color32(255, 255, 255, 255);
    private Color32 buttonColor_invisible = new Color32(255, 255, 255, 0);



    // Start is called before the first frame update
    void Start()
    {
        setInitialButtonValues();
        updateButtonTextValues();
    }


    public void setButtonsCanBePressed(bool buttonsCanBePressed)
    {
        this.buttonsCanBePressed = buttonsCanBePressed;
    }




    public void setInitialButtonValues()
    {
        money_selection_temp = persistentData.getStartingMoney_selection();
        enemies_selection_temp = persistentData.getDensityOfEnemies_selection();
        keys_selection_temp = persistentData.getNumberOfKeys_selection();
        area_selection_temp = persistentData.getSizeOfExplorableArea_selection();
        
        money_value_temp = persistentData.getStartingMoney_value();
        enemies_value_temp = persistentData.getDensityOfEnemies_value();
        keys_value_temp = persistentData.getNumberOfKeys_value();
        area_value_temp = persistentData.getSizeOfExplorableArea_value();
        
        money_custom_temp = persistentData.getStartingMoney_custom();
        enemies_custom_temp = persistentData.getDensityOfEnemies_custom();
        keys_custom_temp = persistentData.getNumberOfKeys_custom();
        area_custom_temp = persistentData.getSizeOfExplorableArea_custom();

        updateButtons_money(money_selection_temp);
        updateButtons_enemies(enemies_selection_temp);
        updateButtons_keys(keys_selection_temp);
        updateButtons_area(area_selection_temp);
        
        text_customMoney.text = convertIntToCommaFormat(money_custom_temp);
        text_customEnemies.text = convertFloatToPercentFormat(enemies_custom_temp);
        text_customKeys.text = string.Format("{0}", keys_custom_temp);
        text_customArea.text = string.Format("+{0}", area_custom_temp);

        inputTextManager.setInitialDefaultValues(money_custom_temp, enemies_custom_temp, keys_custom_temp, area_custom_temp);
    }


    public void buttonPressed_money(int buttonNum)
    {
        if (buttonsCanBePressed)
        {
            buttonPressed_money_effect(buttonNum);
        }
    }

    private void buttonPressed_money_effect(int buttonNum)
    {
        money_selection_temp = (byte)buttonNum;

        updateValues_money(buttonNum);
        updateButtons_money(buttonNum);
    }


    public void buttonPressed_enemies(int buttonNum)
    {
        if (buttonsCanBePressed)
        {
            buttonPressed_enemies_effect(buttonNum);
        }
    }

    private void buttonPressed_enemies_effect(int buttonNum)
    {
        enemies_selection_temp = (byte)buttonNum;

        updateValues_enemies(buttonNum);
        updateButtons_enemies(buttonNum);
    }


    public void buttonPressed_keys(int buttonNum)
    {
        if (buttonsCanBePressed)
        {
            buttonPressed_keys_effect(buttonNum);
        }
    }

    private void buttonPressed_keys_effect(int buttonNum)
    {
        keys_selection_temp = (byte)buttonNum;

        updateValues_keys(buttonNum);
        updateButtons_keys(buttonNum);
    }


    public void buttonPressed_area(int buttonNum)
    {
        if (buttonsCanBePressed)
        {
            buttonPressed_area_effect(buttonNum);
        }
    }

    private void buttonPressed_area_effect(int buttonNum)
    {
        area_selection_temp = (byte)buttonNum;

        updateValues_area(buttonNum);
        updateButtons_area(buttonNum);
    }


    public void buttonPressed_text(int categoryNum)
    {
        if (buttonsCanBePressed && inputTextManager.requestOpenTextMenu(categoryNum))
        {
            buttonsCanBePressed = false;
        }
    }


    public void updateCustom_money(int newValue)
    {
        money_custom_temp = newValue;
        text_customMoney.text = convertIntToCommaFormat(money_custom_temp);
        buttonPressed_money_effect(5);
    }

    public void updateCustom_enemies(int newValue)
    {
        enemies_custom_temp = (float)(newValue)/100f;
        text_customEnemies.text = convertFloatToPercentFormat(enemies_custom_temp);
        buttonPressed_enemies_effect(5);
    }

    public void updateCustom_keys(int newValue)
    {
        keys_custom_temp = (byte)newValue;
        text_customKeys.text = string.Format("{0}", keys_custom_temp);
        buttonPressed_keys_effect(5);
    }

    public void updateCustom_area(int newValue)
    {
        area_custom_temp = (byte)newValue;
        text_customArea.text = string.Format("+{0}", area_custom_temp);
        buttonPressed_area_effect(5);
    }





    private string convertIntToCommaFormat(int number)
    {
        return number.ToString("n0");
    }

    private string convertFloatToPercentFormat(float floatToConvert)
    {
        return string.Format("{0}%", Mathf.Round(floatToConvert * 100f));
    }


    public void buttonPressed_Save()
    {
        if (requestCloseMenu())
        {
            persistentData.setAndSaveCustomizationData(money_selection_temp, enemies_selection_temp, keys_selection_temp, area_selection_temp,
                                                        money_value_temp, enemies_value_temp, keys_value_temp, area_value_temp,
                                                        money_custom_temp, enemies_custom_temp, keys_custom_temp, area_custom_temp);

            updateButtonTextValues();
        }
    }

    public void buttonPressed_Cancel()
    {
        requestCloseMenu();
    }


    public bool requestOpenMenu()
    {
        if (!menuIsOpen)
        {
            menuIsOpen = true;

            playButton.SetActive(false);

            customizeParent.SetActive(true);
            saveButton.SetActive(true);
            cancelButton.SetActive(true);

            underAllButton.SetActive(true);

            // And make the menu slide in.
            StartCoroutine("playSlideInAnimation");

            return true;
        }
        
        return false;
    }

    private bool requestCloseMenu()
    {
        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;

            underAllButton.SetActive(false);

            customizeButtonParent.SetActive(true);

            // And make the menu slide out.
            StartCoroutine("playSlideOutAnimation");

            return true;
        }
        
        return false;
    }

    IEnumerator playSlideInAnimation()
    {
        Vector2 amountToMove = new Vector2(0f, slideOffscreenAmount / (float)slideInAnimationTime);
        float amountToFade = 1f / (float)slideInAnimationTime;

        RectTransform customizeRect = customizeParent.GetComponent<RectTransform>();
        CanvasGroup customizeGroup = customizeParent.GetComponent<CanvasGroup>();
        CanvasGroup buttonGroup = customizeButtonParent.GetComponent<CanvasGroup>();
        Image saveButtonImage = saveButton.GetComponent<Image>();
        Image cancelButtonImage = cancelButton.GetComponent<Image>();
        // Image customizeButtonImage = customizeButton.GetComponent<Image>();

        customizeRect.anchoredPosition = new Vector2(0f, slideOffscreenAmount);
        customizeGroup.alpha = 0f;

        for (int index = 0; index<slideInAnimationTime; index++) 
        {
            customizeRect.anchoredPosition -= amountToMove;
            customizeGroup.alpha += amountToFade;
            buttonGroup.alpha -= amountToFade;
            saveButtonImage.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideInAnimationTime));
            cancelButtonImage.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideInAnimationTime));
            // customizeButtonImage.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideInAnimationTime));
            yield return null;
        }

        customizeRect.anchoredPosition = new Vector2(0f, 0f);
        customizeGroup.alpha = 1f;
        buttonGroup.alpha = 0f;
        saveButtonImage.color = new Color(1f, 1f, 1f, 1f);
        cancelButtonImage.color = new Color(1f, 1f, 1f, 1f);

        customizeButtonParent.SetActive(false);

        buttonsCanBePressed = true;
    }

    IEnumerator playSlideOutAnimation()
    {
        Vector2 amountToMove = new Vector2(0f, slideOffscreenAmount / (float)slideOutAnimationTime);
        float amountToFade = 1f / (float)slideOutAnimationTime;

        RectTransform customizeRect = customizeParent.GetComponent<RectTransform>();
        CanvasGroup customizeGroup = customizeParent.GetComponent<CanvasGroup>();
        CanvasGroup buttonGroup = customizeButtonParent.GetComponent<CanvasGroup>();
        Image saveButtonImage = saveButton.GetComponent<Image>();
        Image cancelButtonImage = cancelButton.GetComponent<Image>();
        // Image customizeButtonImage = customizeButton.GetComponent<Image>();

        for (int index = 0; index<slideOutAnimationTime; index++) 
        {
            customizeRect.anchoredPosition += amountToMove;
            customizeGroup.alpha -= amountToFade;
            buttonGroup.alpha += amountToFade;
            saveButtonImage.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideOutAnimationTime));
            cancelButtonImage.color = new Color(1f, 1f, 1f, 1f - ((float)index)/((float)slideOutAnimationTime));
            // customizeButtonImage.color = new Color(1f, 1f, 1f, ((float)index)/((float)slideOutAnimationTime));
            yield return null;
        }

        customizeParent.SetActive(false);
        saveButton.SetActive(false);
        cancelButton.SetActive(false);
        
        // customizeButtonImage.color = new Color(1f, 1f, 1f, 1f);

        playButton.SetActive(true);
        titleScreen.setButtonsCanBePressed(true);

        menuIsOpen = false;
    }


    private void updateButtonTextValues()
    {
        // Money:
        switch(money_selection_temp)
        {
            case 0:
                text_buttonMoney.text = "Random";
                break;

            default:
                text_buttonMoney.text = convertIntToCommaFormat(money_value_temp);
                break;
        }
        
        // Enemies:
        switch(enemies_selection_temp)
        {
            case 0:
                text_buttonEnemies.text = "Random";
                break;

            case 1:
                text_buttonEnemies.text = "None";
                break;
                
            case 2:
                text_buttonEnemies.text = "Fewer";
                break;
                
            case 3:
                text_buttonEnemies.text = "Normal";
                break;
                
            case 4:
                text_buttonEnemies.text = "Many";
                break;

            default:
                text_buttonEnemies.text = convertFloatToPercentFormat(enemies_value_temp);
                break;
        }
        
        // Keys:
        switch(keys_selection_temp)
        {
            case 0:
                text_buttonKeys.text = "Random";
                break;
                
            case 1:
                text_buttonKeys.text = "Zero";
                break;
                
            case 2:
                text_buttonKeys.text = "Lower";
                break;
                
            case 3:
                text_buttonKeys.text = "Higher";
                break;
                
            case 4:
                text_buttonKeys.text = "All";
                break;

            default:
                text_buttonKeys.text = string.Format("{0}", keys_value_temp);
                break;
        }
        
        // Area:
        switch(area_selection_temp)
        {
            case 0:
                text_buttonArea.text = "Random";
                break;

            case 1:
                text_buttonArea.text = "Tiny";
                break;
                
            case 2:
                text_buttonArea.text = "Small";
                break;
                
            case 3:
                text_buttonArea.text = "Normal";
                break;
                
            case 4:
                text_buttonArea.text = "Large";
                break;

            default:
                text_buttonArea.text = string.Format("+{0}", area_custom_temp);
                break;
        }
    }



    private void updateValues_money(int buttonNum)
    {
        switch (buttonNum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                money_value_temp = Constants.getCustomization_StartingMoney((byte)buttonNum);
                break;

            case 5:
                money_value_temp = money_custom_temp;
                break;

            default:
                break;
        }
    }

    private void updateValues_enemies(int buttonNum)
    {
        switch (buttonNum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                enemies_value_temp = Constants.getCustomization_DensityOfEnemeies((byte)buttonNum);
                break;

            case 5:
                enemies_value_temp = enemies_custom_temp;
                break;

            default:
                break;
        }
    }

    private void updateValues_keys(int buttonNum)
    {
        switch (buttonNum)
        {
            case 1:
                keys_value_temp = 0;
                break;
                
            case 4:
                keys_value_temp = 6;
                break;
                
            case 5:
                keys_value_temp = keys_custom_temp;
                break;

            default:
                break;
        }
    }

    private void updateValues_area(int buttonNum)
    {
        switch (buttonNum)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                area_value_temp = Constants.getCustomization_SizeOfArea((byte)buttonNum);
                break;

            case 5:
                area_value_temp = (byte)(3 + area_custom_temp);
                break;

            default:
                break;
        }
    }




    private void updateButtons_money(int buttonNum)
    {
        switch(buttonNum)
        {
            case 0:
                button_money_random.color = buttonColor_visible;
                button_money_1.color = buttonColor_invisible;
                button_money_2.color = buttonColor_invisible;
                button_money_3.color = buttonColor_invisible;
                button_money_4.color = buttonColor_invisible;
                button_money_custom_portrait.color = buttonColor_invisible;
                button_money_custom_landscape.color = buttonColor_invisible;
                text_customMoney.color = textColor_darker;
                break;

            case 1:
                button_money_random.color = buttonColor_invisible;
                button_money_1.color = buttonColor_visible;
                button_money_2.color = buttonColor_invisible;
                button_money_3.color = buttonColor_invisible;
                button_money_4.color = buttonColor_invisible;
                button_money_custom_portrait.color = buttonColor_invisible;
                button_money_custom_landscape.color = buttonColor_invisible;
                text_customMoney.color = textColor_darker;
                break;
                
            case 2:
                button_money_random.color = buttonColor_invisible;
                button_money_1.color = buttonColor_invisible;
                button_money_2.color = buttonColor_visible;
                button_money_3.color = buttonColor_invisible;
                button_money_4.color = buttonColor_invisible;
                button_money_custom_portrait.color = buttonColor_invisible;
                button_money_custom_landscape.color = buttonColor_invisible;
                text_customMoney.color = textColor_darker;
                break;
                
            case 3:
                button_money_random.color = buttonColor_invisible;
                button_money_1.color = buttonColor_invisible;
                button_money_2.color = buttonColor_invisible;
                button_money_3.color = buttonColor_visible;
                button_money_4.color = buttonColor_invisible;
                button_money_custom_portrait.color = buttonColor_invisible;
                button_money_custom_landscape.color = buttonColor_invisible;
                text_customMoney.color = textColor_darker;
                break;
                
            case 4:
                button_money_random.color = buttonColor_invisible;
                button_money_1.color = buttonColor_invisible;
                button_money_2.color = buttonColor_invisible;
                button_money_3.color = buttonColor_invisible;
                button_money_4.color = buttonColor_visible;
                button_money_custom_portrait.color = buttonColor_invisible;
                button_money_custom_landscape.color = buttonColor_invisible;
                text_customMoney.color = textColor_darker;
                break;
                
            case 5:
                button_money_random.color = buttonColor_invisible;
                button_money_1.color = buttonColor_invisible;
                button_money_2.color = buttonColor_invisible;
                button_money_3.color = buttonColor_invisible;
                button_money_4.color = buttonColor_invisible;
                button_money_custom_portrait.color = buttonColor_visible;
                button_money_custom_landscape.color = buttonColor_visible;
                text_customMoney.color = textColor_white;
                break;

            default:
                break;
        }
    }


    private void updateButtons_enemies(int buttonNum)
    {
        switch(buttonNum)
        {
            case 0:
                button_enemies_random.color = buttonColor_visible;
                button_enemies_1.color = buttonColor_invisible;
                button_enemies_2.color = buttonColor_invisible;
                button_enemies_3.color = buttonColor_invisible;
                button_enemies_4.color = buttonColor_invisible;
                button_enemies_custom_portrait.color = buttonColor_invisible;
                button_enemies_custom_landscape.color = buttonColor_invisible;
                text_customEnemies.color = textColor_darker;
                break;

            case 1:
                button_enemies_random.color = buttonColor_invisible;
                button_enemies_1.color = buttonColor_visible;
                button_enemies_2.color = buttonColor_invisible;
                button_enemies_3.color = buttonColor_invisible;
                button_enemies_4.color = buttonColor_invisible;
                button_enemies_custom_portrait.color = buttonColor_invisible;
                button_enemies_custom_landscape.color = buttonColor_invisible;
                text_customEnemies.color = textColor_darker;
                break;
                
            case 2:
                button_enemies_random.color = buttonColor_invisible;
                button_enemies_1.color = buttonColor_invisible;
                button_enemies_2.color = buttonColor_visible;
                button_enemies_3.color = buttonColor_invisible;
                button_enemies_4.color = buttonColor_invisible;
                button_enemies_custom_portrait.color = buttonColor_invisible;
                button_enemies_custom_landscape.color = buttonColor_invisible;
                text_customEnemies.color = textColor_darker;
                break;
                
            case 3:
                button_enemies_random.color = buttonColor_invisible;
                button_enemies_1.color = buttonColor_invisible;
                button_enemies_2.color = buttonColor_invisible;
                button_enemies_3.color = buttonColor_visible;
                button_enemies_4.color = buttonColor_invisible;
                button_enemies_custom_portrait.color = buttonColor_invisible;
                button_enemies_custom_landscape.color = buttonColor_invisible;
                text_customEnemies.color = textColor_darker;
                break;
                
            case 4:
                button_enemies_random.color = buttonColor_invisible;
                button_enemies_1.color = buttonColor_invisible;
                button_enemies_2.color = buttonColor_invisible;
                button_enemies_3.color = buttonColor_invisible;
                button_enemies_4.color = buttonColor_visible;
                button_enemies_custom_portrait.color = buttonColor_invisible;
                button_enemies_custom_landscape.color = buttonColor_invisible;
                text_customEnemies.color = textColor_darker;
                break;
                
            case 5:
                button_enemies_random.color = buttonColor_invisible;
                button_enemies_1.color = buttonColor_invisible;
                button_enemies_2.color = buttonColor_invisible;
                button_enemies_3.color = buttonColor_invisible;
                button_enemies_4.color = buttonColor_invisible;
                button_enemies_custom_portrait.color = buttonColor_visible;
                button_enemies_custom_landscape.color = buttonColor_visible;
                text_customEnemies.color = textColor_white;
                break;

            default:
                break;
        }
    }


    private void updateButtons_keys(int buttonNum)
    {
        switch(buttonNum)
        {
            case 0:
                button_keys_random.color = buttonColor_visible;
                button_keys_1.color = buttonColor_invisible;
                button_keys_2.color = buttonColor_invisible;
                button_keys_3.color = buttonColor_invisible;
                button_keys_4.color = buttonColor_invisible;
                button_keys_custom_portrait.color = buttonColor_invisible;
                button_keys_custom_landscape.color = buttonColor_invisible;
                text_customKeys.color = textColor_darker;
                break;
                
            case 1:
                button_keys_random.color = buttonColor_invisible;
                button_keys_1.color = buttonColor_visible;
                button_keys_2.color = buttonColor_invisible;
                button_keys_3.color = buttonColor_invisible;
                button_keys_4.color = buttonColor_invisible;
                button_keys_custom_portrait.color = buttonColor_invisible;
                button_keys_custom_landscape.color = buttonColor_invisible;
                text_customKeys.color = textColor_darker;
                break;
                
            case 2:
                button_keys_random.color = buttonColor_invisible;
                button_keys_1.color = buttonColor_invisible;
                button_keys_2.color = buttonColor_visible;
                button_keys_3.color = buttonColor_invisible;
                button_keys_4.color = buttonColor_invisible;
                button_keys_custom_portrait.color = buttonColor_invisible;
                button_keys_custom_landscape.color = buttonColor_invisible;
                text_customKeys.color = textColor_darker;
                break;
                
            case 3:
                button_keys_random.color = buttonColor_invisible;
                button_keys_1.color = buttonColor_invisible;
                button_keys_2.color = buttonColor_invisible;
                button_keys_3.color = buttonColor_visible;
                button_keys_4.color = buttonColor_invisible;
                button_keys_custom_portrait.color = buttonColor_invisible;
                button_keys_custom_landscape.color = buttonColor_invisible;
                text_customKeys.color = textColor_darker;
                break;
                
            case 4:
                button_keys_random.color = buttonColor_invisible;
                button_keys_1.color = buttonColor_invisible;
                button_keys_2.color = buttonColor_invisible;
                button_keys_3.color = buttonColor_invisible;
                button_keys_4.color = buttonColor_visible;
                button_keys_custom_portrait.color = buttonColor_invisible;
                button_keys_custom_landscape.color = buttonColor_invisible;
                text_customKeys.color = textColor_darker;
                break;
                
            case 5:
                button_keys_random.color = buttonColor_invisible;
                button_keys_1.color = buttonColor_invisible;
                button_keys_2.color = buttonColor_invisible;
                button_keys_3.color = buttonColor_invisible;
                button_keys_4.color = buttonColor_invisible;
                button_keys_custom_portrait.color = buttonColor_visible;
                button_keys_custom_landscape.color = buttonColor_visible;
                text_customKeys.color = textColor_white;
                break;

            default:
                break;
        }
    }


    private void updateButtons_area(int buttonNum)
    {
        switch(buttonNum)
        {
            case 0:
                button_area_random.color = buttonColor_visible;
                button_area_1.color = buttonColor_invisible;
                button_area_2.color = buttonColor_invisible;
                button_area_3.color = buttonColor_invisible;
                button_area_4.color = buttonColor_invisible;
                button_area_custom_portrait.color = buttonColor_invisible;
                button_area_custom_landscape.color = buttonColor_invisible;
                text_customArea.color = textColor_darker;
                break;

            case 1:
                button_area_random.color = buttonColor_invisible;
                button_area_1.color = buttonColor_visible;
                button_area_2.color = buttonColor_invisible;
                button_area_3.color = buttonColor_invisible;
                button_area_4.color = buttonColor_invisible;
                button_area_custom_portrait.color = buttonColor_invisible;
                button_area_custom_landscape.color = buttonColor_invisible;
                text_customArea.color = textColor_darker;
                break;
                
            case 2:
                button_area_random.color = buttonColor_invisible;
                button_area_1.color = buttonColor_invisible;
                button_area_2.color = buttonColor_visible;
                button_area_3.color = buttonColor_invisible;
                button_area_4.color = buttonColor_invisible;
                button_area_custom_portrait.color = buttonColor_invisible;
                button_area_custom_landscape.color = buttonColor_invisible;
                text_customArea.color = textColor_darker;
                break;
                
            case 3:
                button_area_random.color = buttonColor_invisible;
                button_area_1.color = buttonColor_invisible;
                button_area_2.color = buttonColor_invisible;
                button_area_3.color = buttonColor_visible;
                button_area_4.color = buttonColor_invisible;
                button_area_custom_portrait.color = buttonColor_invisible;
                button_area_custom_landscape.color = buttonColor_invisible;
                text_customArea.color = textColor_darker;
                break;
                
            case 4:
                button_area_random.color = buttonColor_invisible;
                button_area_1.color = buttonColor_invisible;
                button_area_2.color = buttonColor_invisible;
                button_area_3.color = buttonColor_invisible;
                button_area_4.color = buttonColor_visible;
                button_area_custom_portrait.color = buttonColor_invisible;
                button_area_custom_landscape.color = buttonColor_invisible;
                text_customArea.color = textColor_darker;
                break;
                
            case 5:
                button_area_random.color = buttonColor_invisible;
                button_area_1.color = buttonColor_invisible;
                button_area_2.color = buttonColor_invisible;
                button_area_3.color = buttonColor_invisible;
                button_area_4.color = buttonColor_invisible;
                button_area_custom_portrait.color = buttonColor_visible;
                button_area_custom_landscape.color = buttonColor_visible;
                text_customArea.color = textColor_white;
                break;

            default:
                break;
        }
    }
}