using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBonkManager : MonoBehaviour
{
    public EquippedCannonData cannonData;

    public Sprite sprite_bonk1;
    public Sprite sprite_bonk2;
    public Sprite sprite_bonk3;
    public Sprite sprite_bonk4;
    public Sprite sprite_bonk5;
    public Sprite sprite_bonk6;
    public Sprite sprite_bonk7;
    public Sprite sprite_bonk8;
    public Sprite sprite_bonk9;
    public Sprite sprite_bonk10;
    public Sprite sprite_bonk11;
    public Sprite sprite_bonk12;
    public Sprite sprite_bonk13;
    public Sprite sprite_bonk14;
    public Sprite sprite_bonk15;
    public Sprite sprite_bonk16;
    public Sprite sprite_bonk17;
    public Sprite sprite_bonk18;
    public Sprite sprite_bonk19;
    public Sprite sprite_bonk20;

    public GameObject prefab_cannonBonk;
    private CannonBonkIndividual[] bonkArray_class;
    
    // private CannonBonkIndividual tempBonkClass;
    // private Sprite tempSprite;

    private byte numOfCannonbonks;
    private byte oldestCannonbonk = 0;

    private Color32 bonkColor_player;
    private Color32 bonkColor_enemy;

    private const int targetBonkColor = 230;
    

    public void initializeCannnonbonkArray(byte numOfCannonbonks, Color32 cannonballColor_player, Color32 cannonballColor_enemy)
    {
        this.numOfCannonbonks = numOfCannonbonks;
        bonkArray_class = new CannonBonkIndividual[numOfCannonbonks];
        GameObject tempBonk;

        for (int index=0; index<numOfCannonbonks; index++)
        {
            tempBonk = Instantiate(prefab_cannonBonk, Vector3.zero, Quaternion.identity);

            bonkArray_class[index] = tempBonk.GetComponent<CannonBonkIndividual>();
        }

        bonkColor_player = generateCannonbonkTint(cannonballColor_player);
        bonkColor_enemy = generateCannonbonkTint(cannonballColor_enemy);
    }


    public void advanceAllCannonbonkAnimations()
    {
        for (byte index=0; index<numOfCannonbonks; index++)
        {
            if (bonkArray_class[index].getIsActive())
            {
                bonkArray_class[index].updateImages(getSpriteFromAnimFrame(bonkArray_class[index].getCurrentAnimFrame()));
            }
        }
    }


    public void placeNewBonk(Vector3 newPosition, float bonkScale, bool isPlayerCannonball)
    {
        if (isPlayerCannonball) bonkArray_class[oldestCannonbonk].resetAnimation(newPosition, sprite_bonk2, bonkColor_player, bonkScale);
        else bonkArray_class[oldestCannonbonk].resetAnimation(newPosition, sprite_bonk2, bonkColor_enemy, bonkScale);

        oldestCannonbonk++;
        if (oldestCannonbonk >= numOfCannonbonks) oldestCannonbonk = 0;
    }


    public void placeNewBonk_specificTint(Vector3 newPosition, Color32 bonkTint, float bonkScale)
    {
        bonkArray_class[oldestCannonbonk].resetAnimation(newPosition, sprite_bonk2, bonkTint, bonkScale);

        oldestCannonbonk++;
        if (oldestCannonbonk >= numOfCannonbonks) oldestCannonbonk = 0;
    }


    private Sprite getSpriteFromAnimFrame(byte animFrame)
    {
        switch(animFrame)
        {
            case 1: return sprite_bonk2;
            case 2: return sprite_bonk2;
            case 3: return sprite_bonk3;
            case 4: return sprite_bonk4;
            case 5: return sprite_bonk5;
            case 6: return sprite_bonk6;
            case 7: return sprite_bonk7;
            case 8: return sprite_bonk8;
            case 9: return sprite_bonk9;
            case 10: return sprite_bonk10;
            case 11: return sprite_bonk11;
            case 12: return sprite_bonk12;
            case 13: return sprite_bonk13;
            case 14: return sprite_bonk14;
            case 15: return sprite_bonk15;
            case 16: return sprite_bonk16;
            case 17: return sprite_bonk17;
            case 18: return sprite_bonk18;
            case 19: return sprite_bonk19;
            default: return sprite_bonk20;
        }
    }

    private Color32 generateCannonbonkTint(Color32 cannonballColor)
    {
        float colorAverage = (cannonballColor.r + cannonballColor.g + cannonballColor.b)/3f;

        float[] newValues = new float[3];
        newValues[0] = (cannonballColor.r - colorAverage)/2f + targetBonkColor; 
        newValues[1] = (cannonballColor.g - colorAverage)/2f + targetBonkColor; 
        newValues[2] = (cannonballColor.b - colorAverage)/2f + targetBonkColor; 

        float newMax = Mathf.Max(newValues[0], newValues[1], newValues[2]);

        if (newMax > 255)
        {
            newValues[0] = newValues[0] / newMax * 255;
            newValues[1] = newValues[1] / newMax * 255;
            newValues[2] = newValues[2] / newMax * 255;
        }

        return new Color32( (byte)newValues[0], (byte)newValues[1], (byte)newValues[2], 255);
    }

    public Color32 getBonkColor_player()
    {
        return bonkColor_player;
    }
}
