using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonButtonManager : MonoBehaviour
{
    public Image cannonSelectionButton_01;
    public Image cannonSelectionButton_02;
    public Image cannonSelectionButton_03;
    public Image cannonSelectionButton_04;
    public Image cannonSelectionButton_05;
    public Image cannonSelectionButton_06;
    public Image cannonSelectionButton_07;
    public Image cannonSelectionButton_random;

    private Image currentlySelectedButton;

    public Text text_cannonName;
    public Text text_cannonDescription;

    private bool secretButtonIsVisible = false;

    private static readonly Color32 color_visible = Color.white;
    private static readonly Color32 color_invisible = new Color32(255, 255, 255, 0);

    public Sprite sprite_randomButton;
    public Sprite sprite_secretButton;


    // Start is called before the first frame update
    void Start()
    {
        currentlySelectedButton = cannonSelectionButton_01;
    }

    public void changeSelectedButton(byte selectionNum)
    {
        currentlySelectedButton.color = color_invisible;
        if (secretButtonIsVisible) 
        {
            secretButtonIsVisible = false;
            cannonSelectionButton_random.sprite = sprite_randomButton;
        }

        switch (selectionNum)
        {
            case 0:
                currentlySelectedButton = cannonSelectionButton_01;
                setTexts_reliable();
                break;
                
            case 1:
                currentlySelectedButton = cannonSelectionButton_02;
                setTexts_grapeshot();
                break;
                
            case 2:
                currentlySelectedButton = cannonSelectionButton_03;
                setTexts_chainShot();
                break;
                
            case 3:
                currentlySelectedButton = cannonSelectionButton_04;
                setTexts_rapidFire();
                break;
                
            case 4:
                currentlySelectedButton = cannonSelectionButton_05;
                setTexts_mystical();
                break;
                
            case 5:
                currentlySelectedButton = cannonSelectionButton_06;
                setTexts_explosive();
                break;
                
            case 6:
                currentlySelectedButton = cannonSelectionButton_07;
                setTexts_hotShot();
                break;
                
            case 10:
                currentlySelectedButton = cannonSelectionButton_random;
                setTexts_secret();
                secretButtonIsVisible = true;
                cannonSelectionButton_random.sprite = sprite_secretButton;
                break;
            
            default:
                currentlySelectedButton = cannonSelectionButton_random;
                setTexts_random();
                break;
        }

        currentlySelectedButton.color = color_visible;
    }


    private void setTexts_reliable()
    {
        text_cannonName.text = "Reliable Cannon";
        text_cannonDescription.text = "Fires a single large cannonball that packs a serious punch.";
    }

    private void setTexts_grapeshot()
    {
        text_cannonName.text = "Grapeshot Cannon";
        text_cannonDescription.text = "Fires a cluster of cannonballs that spread out while in the air.";
    }

    private void setTexts_chainShot()
    {
        text_cannonName.text = "Chain-shot Cannon";
        text_cannonDescription.text = "Fires two cannonballs connected by a chain, which swirl around each other in flight.";
    }

    private void setTexts_rapidFire()
    {
        text_cannonName.text = "Rapid-fire Cannon";
        text_cannonDescription.text = "Rapidly fires tiny cannonballs for as long as the stick is held down.";
    }

    private void setTexts_mystical()
    {
        text_cannonName.text = "Mystical Cannon";
        text_cannonDescription.text = "Fires a slew of slower-moving cannonballs that pass through walls and seek out targets.";
    }

    private void setTexts_explosive()
    {
        text_cannonName.text = "Exploding Cannon";
        text_cannonDescription.text = "Fires a giant cannonball that explodes on impact, dealing heavy damage to a wide area.\nRequires additional time to reload.";
    }

    private void setTexts_hotShot()
    {
        text_cannonName.text = "Hot-shot Cannon";
        text_cannonDescription.text = "Fires super-heated cannonballs that travel much further and set targets on fire.\nRequires additional time to reload.";
    }

    private void setTexts_random()
    {
        text_cannonName.text = "Random Cannon";
        text_cannonDescription.text = "Becomes a different cannon every time a game is played!";
    }

    private void setTexts_secret()
    {
        text_cannonName.text = "The Secret Weapon";
        text_cannonDescription.text = "An extremely awesome, intentionally overpowered superweapon.\nNot intended to be taken seriously.";
    }




}
