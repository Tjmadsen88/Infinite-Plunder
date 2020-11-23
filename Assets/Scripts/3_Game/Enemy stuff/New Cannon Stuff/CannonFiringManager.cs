using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class CannonFiringManager : MonoBehaviour
{
    public EquippedCannonData cannonData;
    public CannonballManager cannonballManager;
    public GameView gameView;

    public PersistentData persistentData;

    private byte reloadTimer_current = 0;
    // private byte cooldownTimer_max;

    // private byte reloadTimer_secondary_current = 0;
    // private byte reloadTimer_secondary_max = 10;

    // private byte numOfShots_secondary_current = 3;
    // private const byte numOfShots_secondary_max = 3;

    private Vector3 previousDirection;
    private Vector3 randomizedDirection;
    private const float maxSpreadAngle_grapeshot = 10f;
    private const float spreadInc_mystical = 72f;
    private const float maxSpreadAngle_rapid = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void checkCannonReload(Vector3 boatPosition)
    {
        if (reloadTimer_current > 0)
        {
            reloadTimer_current--;
        }

        // if (reloadTimer_secondary_current > 0)
        // {
        //     reloadTimer_secondary_current--;

        //     if (reloadTimer_secondary_current == 0 && numOfShots_secondary_current < numOfShots_secondary_max)
        //     {
        //         fireCannon_spread_next(boatPosition);
        //     }
        // }
    }

    public bool requestFireCannon(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        if (reloadTimer_current == 0)
        {
            fireCannon(boatPosition, cannonballDirection);
            setReloadTime();

            return true;
        }

        return false;
    }



    private void fireCannon(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        switch(cannonData.getShotType())
        {
            case Constants.cannonShotType_pointAndRelease_spread:
                fireCannon_spread(boatPosition, cannonballDirection);
                break;
                
            case Constants.cannonShotType_pointAndRelease_chain:
                fireCannon_chain(boatPosition, cannonballDirection);
                break;
                
            case Constants.cannonShotType_fireInDirection:
                fireCannon_rapid(boatPosition, cannonballDirection);
                break;

            case Constants.cannonShotType_pointAndRelease_mystical:
                fireCannon_mystical(boatPosition, cannonballDirection);
                break;

            case Constants.cannonShotType_secret:
                fireCannon_secret(boatPosition, cannonballDirection);
                break;

            // case Constants.cannonShotType_pointAndRelease:
            default:
                fireCannon_normal(boatPosition, cannonballDirection);
                break;
        }

        if (cannonData.getHealsWhenShoots())
        {
            gameView.secretCannonLifeDrain();
        }
    }


    private void fireCannon_normal(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        cannonballManager.placeCannonball_player(boatPosition, cannonballDirection);
    }

    private void fireCannon_spread(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        cannonballManager.placeCannonball_player_grapeshot(boatPosition, cannonballDirection);

        randomizedDirection = Quaternion.AngleAxis(-maxSpreadAngle_grapeshot/2f, Vector3.up) * cannonballDirection;
        cannonballManager.placeCannonball_player_grapeshot(boatPosition, randomizedDirection);

        randomizedDirection = Quaternion.AngleAxis(maxSpreadAngle_grapeshot/2f, Vector3.up) * cannonballDirection;
        cannonballManager.placeCannonball_player_grapeshot(boatPosition, randomizedDirection);

        for (int index=0; index < 3; index++)
        {
            randomizedDirection = Quaternion.AngleAxis(Random.Range(-maxSpreadAngle_grapeshot, maxSpreadAngle_grapeshot), Vector3.up) * cannonballDirection;
            cannonballManager.placeCannonball_player_grapeshot(boatPosition, randomizedDirection);
        }
    }

    private void fireCannon_chain(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        cannonballManager.placeCannonball_player_chainShot(boatPosition, cannonballDirection);
    }

    private void fireCannon_rapid(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        randomizedDirection = Quaternion.AngleAxis(Random.Range(-maxSpreadAngle_rapid, maxSpreadAngle_rapid), Vector3.up) * cannonballDirection;
        cannonballManager.placeCannonball_player(boatPosition, randomizedDirection);
    }

    private void fireCannon_mystical(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        for (int index=0; index<5; index++)
        {
            randomizedDirection = Quaternion.AngleAxis(index * spreadInc_mystical, Vector3.up) * cannonballDirection;
            cannonballManager.placeCannonball_player(boatPosition, randomizedDirection);
        }
    }

    private void fireCannon_secret(Vector3 boatPosition, Vector3 cannonballDirection)
    {
        // TODO: do this for real
        randomizedDirection = Quaternion.AngleAxis(Random.Range(-maxSpreadAngle_grapeshot, maxSpreadAngle_grapeshot), Vector3.up) * cannonballDirection;
        cannonballManager.placeCannonball_player(boatPosition, randomizedDirection);
    }



    private void setReloadTime()
    {
        if (persistentData.getShootingMode() != Constants.shootingMode_auto) 
            reloadTimer_current = cannonData.getReloadTime_manual();
        else reloadTimer_current = cannonData.getReloadTime_auto();
    }
}
