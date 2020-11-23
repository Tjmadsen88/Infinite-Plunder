using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeIndividual : MonoBehaviour
{
    private byte lifetimeTimer = 0;
    private byte destroyTime = 0;
    
    private Transform wakeTransform;
    public SpriteRenderer wakeRenderer;

    // private float startingOpacity = 0.1f;
    private float startingOpacity = 0.2f;

    private Vector3 startingScale;
    private Vector3 growAmount;

    // Start is called before the first frame update
    void Start()
    {
        wakeTransform = gameObject.GetComponent<Transform>();
        wakeRenderer.color = new Color(1f, 1f, 1f, 0f);

        startingScale = wakeTransform.localScale;
    }


    public void advanceWake()
    {
        if (lifetimeTimer < destroyTime)
        {
            lifetimeTimer++;
            wakeTransform.localScale += growAmount;

            wakeRenderer.color = new Color(0.95f, 0.95f, 0.95f, startingOpacity*(1f - ((float)lifetimeTimer)/((float)destroyTime)));
        }
    }

    public void resetWake(Vector3 newPos)
    {
        wakeTransform.position = newPos;

        lifetimeTimer = 0;
        destroyTime = (byte)Random.Range(40, 50);

        wakeTransform.localScale = startingScale;
        growAmount = new Vector3(1, 1, 1) * Random.Range(0.05f, 0.075f);

        wakeTransform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        wakeRenderer.color = new Color(0.95f, 0.95f, 0.95f, startingOpacity);
    }

    public void hideWake()
    {
        lifetimeTimer = destroyTime;
        wakeRenderer.color = new Color(0.95f, 0.95f, 0.95f, 0f);
    }
}
