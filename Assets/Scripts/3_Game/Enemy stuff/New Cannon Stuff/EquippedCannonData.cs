using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class EquippedCannonData : MonoBehaviour
{
    public PersistentData persistentData;

    private byte shotType;
    private float cannonballSize;
    private float bonkSize;
    private byte cannonballDamage;
    private byte cannonballDamage_secondary = 0;
    
    private bool passesThroughWalls = false;
    private bool autoTargets = false;
    private bool appliesBurn = false;
    private bool explodes = false;
    private bool healsWhenShoots = false;

    private float knockback_StartingAmount;
    private float knockback_dragMultiplier;
    // private byte startingNumOfCannonballs;

    private byte reloadTime_manual;
    private byte reloadTime_auto;
    private float reloadWheelReductionAmount;

    private byte cannonballPrefabID = Constants.cannonballPrefabID_normal;

    private float autoTargetingDistance_auto = 10f;
    private float autoTargetingDistance_assisted = 15f;

    private byte knockback_Duration = Constants.knockback_Duration;

    // Start is called before the first frame update
    void Awake()
    {
        fillOutCannonballData(persistentData.getCannonSelection());
    }
    

    public byte getShotType()
    {
        return shotType;
    }

    public float getCannonballSize()
    {
        return cannonballSize;
    }

    public float getBonkSize()
    {
        return bonkSize;
    }

    public byte getCannonballDamage()
    {
        return cannonballDamage;
    }

    public byte getCannonballDamage_secondary()
    {
        return cannonballDamage_secondary;
    }
    

    public bool getPassesThroughWalls()
    {
        return passesThroughWalls;
    }

    public bool getAutoTargets()
    {
        return autoTargets;
    }

    public bool getAppliesBurn()
    {
        return appliesBurn;
    }

    public bool getExplodes()
    {
        return explodes;
    }

    public bool getHealsWhenShoots()
    {
        return healsWhenShoots;
    }


    public float getKnockback_StartingAmount()
    {
        return knockback_StartingAmount;
    }

    public float getKnockback_dragMultiplier()
    {
        return knockback_dragMultiplier;
    }

    public byte getReloadTime_manual()
    {
        return reloadTime_manual;
    }

    public byte getReloadTime_auto()
    {
        return reloadTime_auto;
    }

    public float getReloadWheelReductionAmount()
    {
        return reloadWheelReductionAmount;
    }

    public byte getCannonballPrefabID()
    {
        return cannonballPrefabID;
    }

    public float getAutoTargetingDistance_auto()
    {
        return autoTargetingDistance_auto;
    }

    public float getAutoTargetingDistance_assisted()
    {
        return autoTargetingDistance_assisted;
    }


    public byte getStaringNumOfCannonballs()
    {
        switch(persistentData.getCannonSelection())
        {
            case 0: // reliable cannon:
                return 2;

            case 1: // grapeshot cannon:
                return 12;

            case 2: // chain-shot cannon:
                return 6;

            case 3: // rapid-fire cannon:
                return 21;

            case 4: // mystical cannon:
                return 10;

            case 5: // explosive cannon:
                return 2;

            case 6: // hot-shot cannon:
                return 2;

            default: // secret weapon:
                return 61;
        }
    }


    private void fillOutCannonballData(byte cannonSelection)
    {
        byte tempCannonSelection = cannonSelection;

        if (cannonSelection == 11) {
            do {
                tempCannonSelection = (byte)Random.Range(0, 7);
            } while (tempCannonSelection == persistentData.getPreviousCannonSelection1() || tempCannonSelection == persistentData.getPreviousCannonSelection2());
        }

        persistentData.setPreviousCannonSelection(tempCannonSelection);

        switch(tempCannonSelection)
        {
            case 0: // reliable cannon:
                shotType = Constants.cannonShotType_pointAndRelease;
                cannonballSize = 5f;
                bonkSize = 1f;
                cannonballDamage = 30;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy;
                reloadTime_manual = 55;
                reloadTime_auto = 75;
                reloadWheelReductionAmount = 1f / 60f;
                break;

            case 1: // grapeshot cannon:
                shotType = Constants.cannonShotType_pointAndRelease_spread;
                cannonballSize = 3.5f;
                bonkSize = 0.7f;
                cannonballDamage = 6;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy;
                reloadTime_manual = 55;
                reloadTime_auto = 75;
                reloadWheelReductionAmount = 1f / 60f;
                
                cannonballPrefabID = Constants.cannonballPrefabID_grapeshot;
                break;

            case 2: // chain shot cannon:
                shotType = Constants.cannonShotType_pointAndRelease_chain;
                cannonballSize = 4f;
                bonkSize = 0.8f;
                cannonballDamage = 15;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy;
                reloadTime_manual = 55;
                reloadTime_auto = 75;
                reloadWheelReductionAmount = 1f / 60f;
                
                cannonballPrefabID = Constants.cannonballPrefabID_chainShot;
                break;

            case 3: // rapid-fire cannon:
                shotType = Constants.cannonShotType_fireInDirection;
                cannonballSize = 2.5f;
                bonkSize = 0.5f;
                cannonballDamage = 2;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy /6f;
                reloadTime_manual = 3;
                reloadTime_auto = 3;
                reloadWheelReductionAmount = 1f;
                
                cannonballPrefabID = Constants.cannonballPrefabID_rapid;
                autoTargetingDistance_auto = 12.5f;
                // autoTargetingDistance_assisted = 18.75f;
                break;

            case 4: // mystical cannon:
                shotType = Constants.cannonShotType_pointAndRelease_mystical;
                cannonballSize = 5f;
                bonkSize = 1f;
                cannonballDamage = 6;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy /2f;
                reloadTime_manual = 55;
                reloadTime_auto = 75;
                reloadWheelReductionAmount = 1f / 60f;

                autoTargets = true;
                passesThroughWalls = true;
                cannonballPrefabID = Constants.cannonballPrefabID_mystical;
                break;

            case 5: // explosive cannon:
                shotType = Constants.cannonShotType_pointAndRelease;
                // cannonballSize = 7f;
                cannonballSize = 3.5f;
                bonkSize = 1.4f;
                cannonballDamage = 0;
                cannonballDamage_secondary = 60;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy *5f;
                reloadTime_manual = 110;
                reloadTime_auto = 150;
                reloadWheelReductionAmount = 1f / 120f;

                explodes = true;
                knockback_Duration *= 2;
                cannonballPrefabID = Constants.cannonballPrefabID_explosive;
                break;

            case 6: // hot-shot cannon:
                shotType = Constants.cannonShotType_pointAndRelease;
                cannonballSize = 5f;
                bonkSize = 1f;
                cannonballDamage = 0;
                // cannonballDamage = 90;
                cannonballDamage_secondary = 3;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy *2f;
                reloadTime_manual = 110;
                reloadTime_auto = 150;
                reloadWheelReductionAmount = 1f / 120f;

                appliesBurn = true;
                cannonballPrefabID = Constants.cannonballPrefabID_hotShot;
                autoTargetingDistance_auto = 15f;
                autoTargetingDistance_assisted = 22.5f;
                break;

            default: // secret weapon:
                // I should think about this some more...
                shotType = Constants.cannonShotType_secret;
                // cannonballSize = 10f;
                cannonballSize = 5f;
                bonkSize = 2f;
                cannonballDamage = 10;
                knockback_StartingAmount = Constants.knockback_StartingAmount_enemy;
                reloadTime_manual = 1;
                reloadTime_auto = 1;
                reloadWheelReductionAmount = 1f;

                passesThroughWalls = true;
                healsWhenShoots = true;
                cannonballPrefabID = Constants.cannonballPrefabID_secret;
                autoTargetingDistance_auto = 15f;
                autoTargetingDistance_assisted = 22.5f;
                break;
        }

        knockback_dragMultiplier = Mathf.Pow(Constants.knockback_EndingAmount / knockback_StartingAmount, 1f / knockback_Duration);
    }
}
