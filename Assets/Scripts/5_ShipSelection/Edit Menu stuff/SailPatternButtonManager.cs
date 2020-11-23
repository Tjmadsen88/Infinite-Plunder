using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SailPatternButtonManager : MonoBehaviour
{
    public Image sailPatternSelectionButton_01;
    public Image sailPatternSelectionButton_02;
    public Image sailPatternSelectionButton_03;
    public Image sailPatternSelectionButton_04;
    public Image sailPatternSelectionButton_05;
    public Image sailPatternSelectionButton_06;
    public Image sailPatternSelectionButton_07;
    public Image sailPatternSelectionButton_08;
    public Image sailPatternSelectionButton_09;
    public Image sailPatternSelectionButton_10;
    public Image sailPatternSelectionButton_11;
    public Image sailPatternSelectionButton_12;
    public Image sailPatternSelectionButton_13;
    public Image sailPatternSelectionButton_14;
    public Image sailPatternSelectionButton_15;
    public Image sailPatternSelectionButton_16;
    public Image sailPatternSelectionButton_17;
    public Image sailPatternSelectionButton_18;
    public Image sailPatternSelectionButton_19;
    public Image sailPatternSelectionButton_20;
    public Image sailPatternSelectionButton_21;
    public Image sailPatternSelectionButton_none;

    private Image currentlySelectedButton;

    private static readonly Color32 color_visible = Color.white;
    private static readonly Color32 color_invisible = new Color32(255, 255, 255, 0);


    // Start is called before the first frame update
    void Start()
    {
        currentlySelectedButton = sailPatternSelectionButton_01;
    }

    public void changeSelectedButton(int selectionNum)
    {
        currentlySelectedButton.color = color_invisible;

        switch (selectionNum)
        {
            case 0:
                currentlySelectedButton = sailPatternSelectionButton_01;
                break;
                
            case 1:
                currentlySelectedButton = sailPatternSelectionButton_02;
                break;
                
            case 2:
                currentlySelectedButton = sailPatternSelectionButton_03;
                break;
                
            case 3:
                currentlySelectedButton = sailPatternSelectionButton_04;
                break;
                
            case 4:
                currentlySelectedButton = sailPatternSelectionButton_05;
                break;
                
            case 5:
                currentlySelectedButton = sailPatternSelectionButton_06;
                break;
                
            case 6:
                currentlySelectedButton = sailPatternSelectionButton_07;
                break;
                
            case 7:
                currentlySelectedButton = sailPatternSelectionButton_08;
                break;
                
            case 8:
                currentlySelectedButton = sailPatternSelectionButton_09;
                break;
                
            case 9:
                currentlySelectedButton = sailPatternSelectionButton_10;
                break;
                
            case 10:
                currentlySelectedButton = sailPatternSelectionButton_11;
                break;
                
            case 11:
                currentlySelectedButton = sailPatternSelectionButton_12;
                break;
                
            case 12:
                currentlySelectedButton = sailPatternSelectionButton_13;
                break;
                
            case 13:
                currentlySelectedButton = sailPatternSelectionButton_14;
                break;
                
            case 14:
                currentlySelectedButton = sailPatternSelectionButton_15;
                break;
                
            case 15:
                currentlySelectedButton = sailPatternSelectionButton_16;
                break;
                
            case 16:
                currentlySelectedButton = sailPatternSelectionButton_17;
                break;
                
            case 17:
                currentlySelectedButton = sailPatternSelectionButton_18;
                break;
                
            case 18:
                currentlySelectedButton = sailPatternSelectionButton_19;
                break;
                
            case 19:
                currentlySelectedButton = sailPatternSelectionButton_20;
                break;
                
            case 20:
                currentlySelectedButton = sailPatternSelectionButton_21;
                break;
            
            default: // none:
                currentlySelectedButton = sailPatternSelectionButton_none;
                break;
        }

        currentlySelectedButton.color = color_visible;
    }
}
