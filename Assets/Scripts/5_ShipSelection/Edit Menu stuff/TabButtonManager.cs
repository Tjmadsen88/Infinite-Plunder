using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButtonManager : MonoBehaviour
{
    public Image tabButton1_image;
    public Image tabButton2_image;
    public Image tabButton3_image;
    public Image tabButton4_image;
    public Image tabButton5_image;
    public Image tabButton6_image;
    public Image tabButton7_image;
    public Image tabButton8_image;

    private int currentlySelectedButtonNum = 0;

    public Sprite sprite_tabButton1_greyed;
    public Sprite sprite_tabButton2_greyed;
    public Sprite sprite_tabButton3_greyed;
    public Sprite sprite_tabButton4_greyed;
    public Sprite sprite_tabButton5_greyed;
    public Sprite sprite_tabButton6_greyed;
    public Sprite sprite_tabButton7_greyed;
    public Sprite sprite_tabButton8_greyed;
    public Sprite sprite_tabButton1_selected;
    public Sprite sprite_tabButton2_selected;
    public Sprite sprite_tabButton3_selected;
    public Sprite sprite_tabButton4_selected;
    public Sprite sprite_tabButton5_selected;
    public Sprite sprite_tabButton6_selected;
    public Sprite sprite_tabButton7_selected;
    public Sprite sprite_tabButton8_selected;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeSelectedButton(int selectionNum)
    {
        deselectCurrentButton();

        currentlySelectedButtonNum = selectionNum;

        selectCurrentButton();
    }



    private void deselectCurrentButton()
    {
        switch(currentlySelectedButtonNum)
        {
            case 0:
                tabButton1_image.sprite = sprite_tabButton1_greyed;
                return;
                
            case 1:
                tabButton2_image.sprite = sprite_tabButton2_greyed;
                return;
                
            case 2:
                tabButton3_image.sprite = sprite_tabButton3_greyed;
                return;
                
            case 3:
                tabButton4_image.sprite = sprite_tabButton4_greyed;
                return;
                
            case 4:
                tabButton5_image.sprite = sprite_tabButton5_greyed;
                return;
                
            case 5:
                tabButton6_image.sprite = sprite_tabButton6_greyed;
                return;
                
            case 6:
                tabButton7_image.sprite = sprite_tabButton7_greyed;
                return;
                
            default:
                tabButton8_image.sprite = sprite_tabButton8_greyed;
                return;
        }
    }


    private void selectCurrentButton()
    {
        switch(currentlySelectedButtonNum)
        {
            case 0:
                tabButton1_image.sprite = sprite_tabButton1_selected;
                return;
                
            case 1:
                tabButton2_image.sprite = sprite_tabButton2_selected;
                return;
                
            case 2:
                tabButton3_image.sprite = sprite_tabButton3_selected;
                return;
                
            case 3:
                tabButton4_image.sprite = sprite_tabButton4_selected;
                return;
                
            case 4:
                tabButton5_image.sprite = sprite_tabButton5_selected;
                return;
                
            case 5:
                tabButton6_image.sprite = sprite_tabButton6_selected;
                return;
                
            case 6:
                tabButton7_image.sprite = sprite_tabButton7_selected;
                return;
                
            default:
                tabButton8_image.sprite = sprite_tabButton8_selected;
                return;
        }
    }

}
