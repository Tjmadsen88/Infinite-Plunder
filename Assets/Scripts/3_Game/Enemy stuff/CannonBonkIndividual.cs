using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBonkIndividual : MonoBehaviour
{
    public GameObject cannonBonk1;
    public GameObject cannonBonk2;
    private SpriteRenderer cannonBonk1_renderer;
    private SpriteRenderer cannonBonk2_renderer;

    private byte currentAnimFrame = 0;
    private bool isActive = false;

    // private const int targetBonkColor = 230;


    // Start is called before the first frame update
    void Start()
    {
        cannonBonk1_renderer = cannonBonk1.GetComponent<SpriteRenderer>();
        cannonBonk2_renderer = cannonBonk2.GetComponent<SpriteRenderer>();
    }


    public byte getCurrentAnimFrame()
    {
        return currentAnimFrame;
    }

    public bool getIsActive()
    {
        return isActive;
    }

    public void updateImages(Sprite newSprite)
    {
        cannonBonk1_renderer.sprite = newSprite;
        cannonBonk2_renderer.sprite = newSprite;
        currentAnimFrame++;

        if (currentAnimFrame > 20)
        {
            isActive = false;
            gameObject.SetActive(false);
        }

        // gameObject.transform.LookAt(Camera.main.transform);
    }

    public void resetAnimation(Vector3 newPosition, Sprite firstSprite, Color32 tintColor, float newScale)
    {
        // Color32 tintColor = getCannonbonkTint(cannonballColor);
        
        isActive = true;

        currentAnimFrame = 0;
        updateImages(firstSprite);
        randomizeBonkRotations();

        cannonBonk1_renderer.color = tintColor;
        cannonBonk2_renderer.color = tintColor;

        gameObject.transform.position = newPosition + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        gameObject.transform.localScale = Vector3.one * newScale;
        gameObject.SetActive(true);

        gameObject.transform.LookAt(Camera.main.transform);
    }

    // public void resetAnimation_specificTint(Vector3 newPosition, Sprite firstSprite, Color32 tintColor, float newScale)
    // {
    //     isActive = true;

    //     currentAnimFrame = 0;
    //     updateImages(firstSprite);
    //     randomizeBonkRotations();

    //     cannonBonk1_renderer.color = tintColor;
    //     cannonBonk2_renderer.color = tintColor;

    //     gameObject.transform.position = newPosition + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    //     gameObject.transform.localScale = Vector3.one * newScale;
    //     gameObject.SetActive(true);

    //     gameObject.transform.LookAt(Camera.main.transform);
    // }

    private void randomizeBonkRotations()
    {
        cannonBonk1.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        cannonBonk2.transform.localEulerAngles = cannonBonk1.transform.localEulerAngles + new Vector3(0f, 0f, Random.Range(45f, 90f));
    }

    // private Color32 getCannonbonkTint(Color32 cannonballColor)
    // {
    //     float colorAverage = (cannonballColor.r + cannonballColor.g + cannonballColor.b)/3f;

    //     float[] newValues = new float[3];
    //     newValues[0] = (cannonballColor.r - colorAverage)/2f + targetBonkColor; 
    //     newValues[1] = (cannonballColor.g - colorAverage)/2f + targetBonkColor; 
    //     newValues[2] = (cannonballColor.b - colorAverage)/2f + targetBonkColor; 

    //     float newMax = Mathf.Max(newValues[0], newValues[1], newValues[2]);

    //     if (newMax > 255)
    //     {
    //         newValues[0] = newValues[0] / newMax * 255;
    //         newValues[1] = newValues[1] / newMax * 255;
    //         newValues[2] = newValues[2] / newMax * 255;
    //     }

    //     return new Color32( (byte)newValues[0], (byte)newValues[1], (byte)newValues[2], 255);
    // }
}
