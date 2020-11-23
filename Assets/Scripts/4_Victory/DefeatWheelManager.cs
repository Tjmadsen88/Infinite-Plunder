using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeightArrayTo3DMeshConverterSpace;

public class DefeatWheelManager : MonoBehaviour
{
    public VictoryScreen victoryScreen;

    private bool shouldPlayAnimation = false;
    private bool animationIsOver = true;
    private byte animationState = 0;

    private const byte animFrames_falling = 46; // This should be as long as the chest-opening animation?

    private const float wheelRotation_total = 60f;
    private Vector3 wheelRotation_perFrame = new Vector3(0f, 0f, wheelRotation_total / animFrames_falling);

    private const float wheelHeightOffset_total = -5f;
    private Vector3 wheelHeightOffset_perFrame = new Vector3(0f, wheelHeightOffset_total / animFrames_falling, 0f);

    private const byte animFrames_slowRotation = 10;
    private const float slowRotation_total = 5f;
    private Vector3 slowRotation_perFrame = new Vector3(0f, 0f, slowRotation_total / animFrames_slowRotation);

    private const byte animFrames_after = 15;  // delay before stats appear


    public void playAnimation(Color32 waterColor)
    {
        // I assume this is attached to the wheel object...
        gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(waterColor, Color.white, 0.75f);

        shouldPlayAnimation = true;
    }


    void Update()
    {
        if (shouldPlayAnimation && animationIsOver)
        {
            switch (animationState)
            {
                case 0:
                    StartCoroutine("wheelAnimation_1_dropWheel");
                    break;
                    
                case 1:
                    StartCoroutine("wheelAnimation_2_slowRotation");
                    break;
                    
                case 2:
                    StartCoroutine("wheelAnimation_3_delayBeforeStats");
                    break;

                default:
                    victoryScreen.beginShowingStats();
                    break;
            }
            
            animationIsOver = false;
            animationState++;
        }
    }


    IEnumerator wheelAnimation_1_dropWheel()
    {
        gameObject.transform.eulerAngles -= wheelRotation_perFrame * animFrames_falling + slowRotation_perFrame * animFrames_slowRotation;
        gameObject.transform.position -= wheelHeightOffset_perFrame * animFrames_falling;

        for (int index = 0; index<animFrames_falling; index++) 
        {
            gameObject.transform.eulerAngles += wheelRotation_perFrame;
            gameObject.transform.position += wheelHeightOffset_perFrame;
            yield return null;
        }

        animationIsOver = true;
    }

    IEnumerator wheelAnimation_2_slowRotation()
    {
        Vector3 targetRotation = gameObject.transform.eulerAngles + slowRotation_perFrame * animFrames_slowRotation;

        for (int index = 0; index<animFrames_slowRotation; index++) 
        {
            // gameObject.transform.eulerAngles += slowRotation_perFrame;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, targetRotation, 0.15f);
            yield return null;
        }

        animationIsOver = true;
    }

    IEnumerator wheelAnimation_3_delayBeforeStats()
    {
        for (int index = 0; index<animFrames_after; index++) 
        {
            yield return null;
        }

        animationIsOver = true;
    }
}
