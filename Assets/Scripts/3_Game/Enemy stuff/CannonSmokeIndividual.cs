using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSmokeIndividual : MonoBehaviour
{
    private byte lifetimeTimer = 0;
    private byte destroyTime = 0;
    
    private Transform smokeTransform;
    public SpriteRenderer smokeRenderer;

    // private float startingOpacity = 0.1f;
    private float startingOpacity = 0.4f;

    private Vector3 startingScale;
    private Vector3 growAmount;
    private Vector3 moveDirection;

    private const float moveMin = -0.05f;
    private const float moveMax = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        smokeTransform = gameObject.GetComponent<Transform>();
        // smokeRenderer.color = new Color(1f, 1f, 1f, 0f);
        gameObject.SetActive(false);

        startingScale = Vector3.one * 4.5f;
    }


    public void advanceSmoke()
    {
        if (lifetimeTimer < destroyTime)
        {
            smokeTransform.LookAt(Camera.main.transform);

            lifetimeTimer++;

            if (lifetimeTimer == destroyTime)
            {
                gameObject.SetActive(false);
            } else {
                smokeTransform.localScale += growAmount;
                smokeTransform.position += moveDirection;

                smokeRenderer.color = new Color(1f, 1f, 1f, startingOpacity*(1f - ((float)lifetimeTimer)/((float)destroyTime)));
                // smokeRenderer.color = new Color(0.6f, 0.6f, 0.6f, startingOpacity*(1f - ((float)lifetimeTimer)/((float)destroyTime)));
            }
        }
    }

    public void resetSmoke(Vector3 newPos, byte childNum, byte numOfChildren)
    {
        resetSmoke(newPos, childNum, numOfChildren, startingScale);
    }

    public void resetSmoke(Vector3 newPos, byte childNum, byte numOfChildren, Vector3 specificStartingScale)
    {
        gameObject.SetActive(true);

        // startingOpacity = 0.4f - 0.012f * childNum;
        startingOpacity = 0.4f * (1f - (float)childNum / ((float)numOfChildren + 1f));

        smokeTransform.position = newPos;

        lifetimeTimer = 0;
        destroyTime = (byte)Random.Range(20, 40);

        smokeTransform.localScale = specificStartingScale;
        growAmount = specificStartingScale * Random.Range(0.0675f, 0.09f);

        // smokeTransform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        smokeRenderer.color = new Color(1f, 1f, 1f, startingOpacity);

        moveDirection = new Vector3(Random.Range(moveMin, moveMax), Random.Range(moveMin, moveMax), Random.Range(moveMin, moveMax));
    }
}
