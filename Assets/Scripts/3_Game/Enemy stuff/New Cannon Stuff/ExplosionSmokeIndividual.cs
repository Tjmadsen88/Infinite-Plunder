using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSmokeIndividual : MonoBehaviour
{
    private byte lifetimeTimer = 0;
    private byte destroyTime = 0;
    private float lifetimePercent;
    
    private Transform smokeTransform;
    public SpriteRenderer smokeRenderer;

    // private float startingOpacity = 0.4f;
    // private float startingOpacity = 0.8f;
    private float startingOpacity = 2f;
    private float moveScalar;

    private Vector3 startingScale;
    private Vector3 growAmount;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        smokeTransform = gameObject.GetComponent<Transform>();
        gameObject.SetActive(false);
        // smokeRenderer.color = new Color(1f, 1f, 1f, 0f);

        // startingScale = Vector3.one * 4.5f;
        // startingScale = Vector3.one * 7.5f;
    }


    public void setInitialSmokeData(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
    }


    public void advanceSmoke(Color smokeColor)
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

                lifetimePercent = 1f - ((float)lifetimeTimer)/((float)destroyTime);

                smokeRenderer.color = new Color(smokeColor.r, smokeColor.g, smokeColor.b, startingOpacity * lifetimePercent);
                smokeTransform.position += moveDirection * moveScalar * Mathf.Pow(lifetimePercent, 3);
            }
        }
    }

    public void resetSmoke(Vector3 newPos, Color smokeColor)
    {
        gameObject.SetActive(true);

        smokeTransform.position = newPos;

        lifetimeTimer = 0;
        destroyTime = (byte)Random.Range(30, 90);

        // startingScale = Vector3.one * Random.Range(2.5f, 7.5f);
        startingScale = Vector3.one * Random.Range(1.25f, 3.75f);
        moveScalar = 60f / destroyTime;

        smokeTransform.localScale = startingScale;
        // growAmount = Vector3.one * Random.Range(0.05f, 0.15f);
        growAmount = Vector3.one * Random.Range(0.025f, 0.075f);

        smokeRenderer.color = new Color(smokeColor.r, smokeColor.g, smokeColor.b, startingOpacity);
    }
}
