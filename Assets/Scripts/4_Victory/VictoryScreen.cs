using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public GameObject victoryText;
    public GameObject defeatText;

    public Text text_startingMoney;
    public Text text_moneyCollected;
    public Text text_moneyLost;
    public Text text_finalTreasure;
    public Text text_totalMoney;

    public Text text_foesSunk;
    public Text text_shipsLost;

    public Text text_areaExplored_white;
    public Text text_areaExplored_blue;
    public Text text_keysFound_white;
    public Text text_keysFound_blue;
    public Text text_gatesOpened_white;
    public Text text_gatesOpened_blue;

    public Image returnButton;

    public CanvasGroup canvasGroup_victoryParent;
    public CanvasGroup canvasGroup_yellowStats;
    public CanvasGroup canvasGroup_redStats;
    public CanvasGroup canvasGroup_blueStats;

    public PersistentData persistentData;

    public VictoryChestManager chestManager;
    public DefeatWheelManager wheelManager;

    public GameObject victoryVisualsParent;
    public GameObject defeatVisualsParent;

    public Camera victoryCamera;
    public Camera defeatCamera;

    // public Light directionalLight;
    public Transform directionalLightTransform;



    private bool buttonCanBePressed = false;

    private const byte animationTime_victoryTextAppears = 15;
    private const byte animationTime_pauseAfterVictoryText = 30;
    private const byte animationTime_statBlockFadesIn = 15;
    private const byte animationTime_pauseBeforeNumbersRoll = 15;
    private const byte animationTime_numberRoll_consistant = 30;
    private const byte animationTime_waitAfterRollNumber = 20;
    // private const byte animationTime_numberRoll_final = 60;
    private byte animationTime_numberRoll_final = 60;
    private const byte animationTime_pauseBeforeBlockAppears = 30;
    private const byte animationTime_returnButtonAppears = 40;

    private int number_startingMoney = 375000;
    private int number_moneyCollected = 225500;
    private int number_moneyLost = 125000;
    private int number_finalTreasure = 300000;
    private int number_totalMoney = 775500;
    
    private int number_foesSunk = 64;
    private int number_shipsLost = 1;

    private int number_areaExplored_actual = 136;
    private int number_areaExplored_total = 164;
    private int number_keysFound_actual = 5;
    private int number_keysFound_total = 5;
    private int number_gatesOpened_actual = 23;
    private int number_gatesOpened_total = 24;






    // Start is called before the first frame update
    void Start()
    {
        // Make the game run at 60fps:
        Application.targetFrameRate = 60;
        
        // Change the imagery if the player has lost:
        if (persistentData.getPlayerHasLost()) setVisuals_Defeat();
        else setVisuals_Victory();
        // setVisuals_Defeat();

        // fill out the stats with data from the persistentDataPacket:
        number_startingMoney = persistentData.getStartingMoney_value();
        number_moneyCollected = persistentData.getMoneyCollected();
        number_moneyLost = persistentData.getMoneyLost();
        number_finalTreasure = persistentData.getFinalTreasure();
        number_totalMoney = number_startingMoney + number_moneyCollected - number_moneyLost + number_finalTreasure;

        number_foesSunk = persistentData.getFoesSunk();
        number_shipsLost = persistentData.getShipsLost();

        number_areaExplored_actual = persistentData.getAreaExplored();
        number_areaExplored_total = persistentData.getAreaTotal();
        number_keysFound_actual = persistentData.getKeysFound();
        number_keysFound_total = persistentData.getKeysTotal();
        number_gatesOpened_actual = persistentData.getDoorsFound();
        number_gatesOpened_total = persistentData.getDoorsTotal();
    }


    private void setVisuals_Victory()
    {
        chestManager.playAnimation();

        StartCoroutine("playIntroCameraAnimation_Victory");
    }


    private void setVisuals_Defeat()
    {
        victoryText.SetActive(false);
        defeatText.SetActive(true);

        victoryVisualsParent.SetActive(false);
        defeatVisualsParent.SetActive(true);

        victoryCamera.enabled = false;
        defeatCamera.enabled = true;

        // Color32 waterColor = persistentData.getTerrainColors()[0];
        Color32 waterColor = Color.Lerp(persistentData.getTerrainColors()[0], Color.black, 0.1f);

        RenderSettings.fogColor = waterColor;
        defeatCamera.backgroundColor = waterColor;

        directionalLightTransform.eulerAngles = new Vector3(28.5f, 152.42f, 0f);

        wheelManager.playAnimation(waterColor);

        animationTime_numberRoll_final = animationTime_numberRoll_consistant;

        StartCoroutine("playIntroCameraAnimation_Defeat");
    }



    public void returnButtonPressed()
    {
        if (buttonCanBePressed)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }




    private string convertNumberToCommaForm(int number)
    {
        return number.ToString("n0");
    }

    private byte getRollTimeBasedOnNumberOfDigits(int number)
    {
        // We assume that no number can be more than 8 digits long, and that they're always positive:
        if (number < 10) return 3;
        if (number < 100) return 4;
        if (number < 1000) return 5;
        if (number < 10000) return 6;
        if (number < 100000) return 7;
        if (number < 1000000) return 8;
        if (number < 10000000) return 9;
        
        return 8;
    }



    private void changeImageryForDefeat()
    {
        // Will swap out the text, the 3D models, and... possibly change the lighting as well.
        // This can probably be a simple deactivate/activate of parents, however?
    }


    public void beginShowingStats()
    {
        StartCoroutine("playVictoryScreenAnimations");
    }


    


    IEnumerator playVictoryScreenAnimations()
    {
        int currentNumber;
        Text currentText;
        int numberRollTime;

        // ------------------------- VICTORY TEXT ---------------------------------------------------------
        // First, we make the Victory Text appear:
        for (int index = 1; index<=animationTime_victoryTextAppears; index++) 
        {
            // For now... it simply fades in.
            canvasGroup_victoryParent.alpha = ((float)index) / ((float)animationTime_victoryTextAppears);
            yield return null;
        }

        // We wait for a little bit:
        for (int index = 0; index<animationTime_pauseAfterVictoryText; index++) 
        {
            yield return null;
        }


        // ------------------------- YELLOW STATS ---------------------------------------------------------
        // We make the yellow BG fade in:
        for (int index = 1; index<=animationTime_statBlockFadesIn; index++) 
        {
            canvasGroup_yellowStats.alpha = ((float)index) / ((float)animationTime_statBlockFadesIn);
            yield return null;
        }

        // We wait for a little bit:
        for (int index = 0; index<animationTime_pauseBeforeNumbersRoll; index++) 
        {
            yield return null;
        }

        // The first stat begins to roll up:
        currentNumber = number_startingMoney;
        currentText = text_startingMoney;
        numberRollTime = animationTime_numberRoll_consistant;
        for (int index = 1; index<=numberRollTime; index++) 
        {
            currentText.text = convertNumberToCommaForm((int)((currentNumber * (float)index) / ((float)numberRollTime)));
            yield return null;
        }

        // Now pause before we show the next one:
        for (int index = 1; index<=animationTime_waitAfterRollNumber; index++) 
        {
            yield return null;
        }

        // The second stat begins to roll up:
        currentNumber = number_moneyCollected;
        currentText = text_moneyCollected;
        numberRollTime = animationTime_numberRoll_consistant;
        for (int index = 1; index<=numberRollTime; index++) 
        {
            currentText.text = "+ "+convertNumberToCommaForm((int)((currentNumber * (float)index) / ((float)numberRollTime)));
            yield return null;
        }

        // Now pause before we show the next one:
        for (int index = 1; index<=animationTime_waitAfterRollNumber; index++) 
        {
            yield return null;
        }

        // The third stat begins to roll up:
        currentNumber = number_moneyLost;
        currentText = text_moneyLost;
        numberRollTime = animationTime_numberRoll_consistant;
        for (int index = 1; index<=numberRollTime; index++) 
        {
            currentText.text = "‒ "+convertNumberToCommaForm((int)((currentNumber * (float)index) / ((float)numberRollTime)));
            yield return null;
        }

        // Now pause before we show the next one:
        for (int index = 1; index<=animationTime_waitAfterRollNumber; index++) 
        {
            yield return null;
        }

        // The fourth stat begins to roll up:
        currentNumber = number_finalTreasure;
        currentText = text_finalTreasure;
        numberRollTime = animationTime_numberRoll_consistant;
        for (int index = 1; index<=numberRollTime; index++) 
        {
            currentText.text = "+ "+convertNumberToCommaForm((int)((currentNumber * (float)index) / ((float)numberRollTime)));
            yield return null;
        }

        // Now pause before we show the next one:
        for (int index = 1; index<animationTime_waitAfterRollNumber; index++) 
        {
            yield return null;
        }

        // The fifth and final stat begins to roll up:
        currentNumber = number_totalMoney;
        currentText = text_totalMoney;
        numberRollTime = animationTime_numberRoll_final;
        for (int index = 1; index<=numberRollTime; index++) 
        {
            currentText.text = convertNumberToCommaForm((int)((currentNumber * (float)index) / ((float)numberRollTime)));
            yield return null;
        }

        // We wait for a little bit:
        for (int index = 0; index<animationTime_pauseBeforeBlockAppears; index++) 
        {
            yield return null;
        }

        // ------------------------- Everything else fades in -----------------------------------------
        // Set all the values:
        text_foesSunk.text = convertNumberToCommaForm(number_foesSunk);
        text_shipsLost.text = convertNumberToCommaForm(number_shipsLost);
        
        text_areaExplored_white.text = convertNumberToCommaForm(number_areaExplored_actual);
        text_areaExplored_blue.text = convertNumberToCommaForm(number_areaExplored_total);
        text_keysFound_white.text = convertNumberToCommaForm(number_keysFound_actual);
        text_keysFound_blue.text = convertNumberToCommaForm(number_keysFound_total);
        text_gatesOpened_white.text = convertNumberToCommaForm(number_gatesOpened_actual);
        text_gatesOpened_blue.text = convertNumberToCommaForm(number_gatesOpened_total);

        // Make them fade in:
        for (int index = 1; index<=animationTime_returnButtonAppears * 2; index++) 
        {
            canvasGroup_redStats.alpha = ((float)index) / ((float)animationTime_returnButtonAppears);
            canvasGroup_blueStats.alpha = ((float)index) / ((float)animationTime_returnButtonAppears) - 0.5f;

            returnButton.color = new Color(1f, 1f, 1f, ((float)index) / ((float)animationTime_returnButtonAppears) -1f);
            yield return null;
        }

        buttonCanBePressed = true;
    }

    

    IEnumerator playIntroCameraAnimation_Victory()
    {
        byte animFrames = 90;

        Transform camTransform = Camera.main.GetComponent<Transform>();

        Vector3 endingPos = camTransform.position;
        camTransform.localPosition += new Vector3(.75f, -.75f, .25f);


        for (int index = 0; index<animFrames; index++) 
        {
            camTransform.position = Vector3.Lerp(camTransform.position, endingPos, 0.03f);
            yield return null;
        }
    }


    IEnumerator playIntroCameraAnimation_Defeat()
    {
        byte animFrames = 90;

        Transform camTransform = Camera.main.GetComponent<Transform>();

        Vector3 endingPos = camTransform.position;
        camTransform.localPosition += new Vector3(-0.5f, 1.5f, -0.5f);
        // camTransform.localPosition += new Vector3(-.75f, .75f, -.25f);

        float lerpVal = 0.03f;
        float lerpInc = 0.001f;


        for (int index = 0; index<animFrames; index++) 
        {
            camTransform.position = Vector3.Lerp(camTransform.position, endingPos, lerpVal);
            lerpVal += lerpInc;
            yield return null;
        }

        camTransform.position = endingPos;
    }
}
