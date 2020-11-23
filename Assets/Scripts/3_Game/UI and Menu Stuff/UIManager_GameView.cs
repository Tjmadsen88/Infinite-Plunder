using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

using TouchManagerSpace;

public class UIManager_GameView : MonoBehaviour
{
    public RectTransform canvasRect;

    public Text moneyText;
    public Text livesText;

    public GameObject key_red;
    public GameObject key_yellow;
    public GameObject key_green;
    public GameObject key_cyan;
    public GameObject key_blue;
    public GameObject key_magenta;

    public GameObject stickBase_left;
    public GameObject stickBase_right;
    public GameObject stickKnob_left;
    public GameObject stickKnob_right;

    public RectTransform stickBase_left_transform;
    public RectTransform stickBase_right_transform;
    public RectTransform stickKnob_left_transform;
    public RectTransform stickKnob_right_transform;

    private bool[] thumbstickWasActive = {false, false};

    private float screenWidth;
    private float screenHeight;

    private const float stickMaxMag = 77f;
    private float[] realStickMag = new float[2];

    private const byte numberRollTimer_max = 45;
    private byte numberRollTimer_current = 0;

    private float moneyNumber_current;
    private float moneyNumber_target;

    private float moneyNumber_increment;

    private float finalTreasureValue = 0f;




    // Start is called before the first frame update
    void Start()
    {
        stickBase_left_transform = stickBase_left.GetComponent<RectTransform>();
        stickBase_right_transform = stickBase_right.GetComponent<RectTransform>();
        stickKnob_left_transform = stickKnob_left.GetComponent<RectTransform>();
        stickKnob_right_transform = stickKnob_right.GetComponent<RectTransform>();
    }

    public void advanceMoneyRollAnimation()
    {
        if (numberRollTimer_current != 0)
        {
            numberRollTimer_current--;
            
            if (numberRollTimer_current != 0)
            {
                moneyNumber_current += moneyNumber_increment;
            } else {
                moneyNumber_current = moneyNumber_target;
            }
            
            displayCurrentMoney();
        }
    }


    public void collectedAKey(byte keyID)
    {
        switch (keyID)
        {
            case Constants.interactableID_key1:
                key_red.SetActive(true);
                break;
                
            case Constants.interactableID_key2:
                key_yellow.SetActive(true);
                break;
                
            case Constants.interactableID_key3:
                key_green.SetActive(true);
                break;
                
            case Constants.interactableID_key4:
                key_cyan.SetActive(true);
                break;
                
            case Constants.interactableID_key5:
                key_blue.SetActive(true);
                break;
            
            default: // key6
                key_magenta.SetActive(true);
                break;
        }
    }


    public void setMoneyAndLivesText_initial(int startingMoney)
    {
        moneyNumber_current = (float)startingMoney;

        moneyText.text = startingMoney.ToString("n0");
        livesText.text = string.Format("x{0}", (startingMoney / 100000).ToString("n0"));
    }

    public void updateMoneyAndLivesText(int newMoneyTotal)
    {
        moneyNumber_target = (float)newMoneyTotal + finalTreasureValue;
        moneyNumber_increment = (moneyNumber_target - moneyNumber_current) / numberRollTimer_max;

        numberRollTimer_current = numberRollTimer_max;
    }

    private void displayCurrentMoney()
    {
        moneyText.text = ((int)moneyNumber_current).ToString("n0");
        livesText.text = string.Format("x{0}", (((int)moneyNumber_current) / 100000).ToString("n0"));
    }

    public void setFinalTreasureValue(int finalTreasureValue)
    {
        this.finalTreasureValue = (float)finalTreasureValue;
    }


    public void setThumbsticks(TouchReturnPacket touchPacket)
    {
        Vector2 screenPoint;
        Vector2 newPlacement;

        // First the left thumbstick:
        if (touchPacket.getHasTouch()[0])
        {
            if (!thumbstickWasActive[0])
            {
                thumbstickWasActive[0] = true;
                stickBase_left.SetActive(true);
                stickKnob_left.SetActive(true);

                screenPoint = new Vector2(touchPacket.getStickCenterX()[0], touchPacket.getStickCenterY()[0]);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out newPlacement);
                stickBase_left_transform.anchoredPosition = newPlacement;
            }

            screenPoint = new Vector2(touchPacket.getTouchX()[0], touchPacket.getTouchY()[0]);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out newPlacement);

            newPlacement -= stickBase_left_transform.anchoredPosition;
            if (newPlacement.magnitude > stickMaxMag)
                newPlacement = newPlacement.normalized * stickMaxMag;

            realStickMag[0] = newPlacement.magnitude / stickMaxMag;

            newPlacement += stickBase_left_transform.anchoredPosition;

            stickKnob_left_transform.anchoredPosition = newPlacement;

        } else {
            if (thumbstickWasActive[0])
            {
                thumbstickWasActive[0] = false;
                stickBase_left.SetActive(false);
                stickKnob_left.SetActive(false);
                realStickMag[0] = 0f;
            }
        }


        // Then the right thumbstick:
        if (touchPacket.getHasTouch()[1])
        {
            if (!thumbstickWasActive[1])
            {
                thumbstickWasActive[1] = true;
                stickBase_right.SetActive(true);
                stickKnob_right.SetActive(true);

                screenPoint = new Vector2(touchPacket.getStickCenterX()[1], touchPacket.getStickCenterY()[1]);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out newPlacement);
                stickBase_right_transform.anchoredPosition = newPlacement;
            }

            screenPoint = new Vector2(touchPacket.getTouchX()[1], touchPacket.getTouchY()[1]);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out newPlacement);

            newPlacement -= stickBase_right_transform.anchoredPosition;
            if (newPlacement.magnitude > stickMaxMag)
                newPlacement = newPlacement.normalized * stickMaxMag;

            realStickMag[1] = newPlacement.magnitude / stickMaxMag;

            newPlacement += stickBase_right_transform.anchoredPosition;

            stickKnob_right_transform.anchoredPosition = newPlacement;

        } else {
            if (thumbstickWasActive[1])
            {
                thumbstickWasActive[1] = false;
                stickBase_right.SetActive(false);
                stickKnob_right.SetActive(false);
                realStickMag[1] = 0f;
            }
        }
    }

    public void removeBothThumbsticks()
    {
        thumbstickWasActive[0] = false;
        stickBase_left.SetActive(false);
        stickKnob_left.SetActive(false);
        realStickMag[0] = 0f;
        
        thumbstickWasActive[1] = false;
        stickBase_right.SetActive(false);
        stickKnob_right.SetActive(false);
        realStickMag[1] = 0f;
    }

    public float[] getRealStickMagnitude()
    {
        return realStickMag;
    }



}
