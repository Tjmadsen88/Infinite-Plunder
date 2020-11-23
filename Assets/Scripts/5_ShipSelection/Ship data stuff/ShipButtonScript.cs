using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipButtonScript : MonoBehaviour
{
    public Image buttonBG;
    public Text nameText;
    public Text equipText;


    public void setTexts(string nameString, string equipString)
    {
        nameText.text = nameString;
        equipText.text = equipString;
    }

    public void copyTextFromOtherButton(ShipButtonScript otherButton)
    {
        nameText.text = otherButton.getNameText().text;
        equipText.text = otherButton.getEquipText().text;
    }

    public void changeColors(Sprite bgSprite, Color32 nameColor, Color32 equipColor)
    {
        buttonBG.sprite = bgSprite;
        nameText.color = nameColor;
        equipText.color = equipColor;
    }

    public Button getButtonComponent()
    {
        return buttonBG.GetComponent<Button>();
    }

    public Text getNameText()
    {
        return nameText;
    }

    public Text getEquipText()
    {
        return equipText;
    }


    public void prepareToRemoveButton()
    {
        Destroy(buttonBG);
        Destroy(nameText);
        Destroy(equipText);
    }
}
