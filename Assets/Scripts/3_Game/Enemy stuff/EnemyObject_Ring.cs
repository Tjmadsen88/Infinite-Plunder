using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;


public class EnemyObject_Ring : EnemyObject_Abstract
{
    public GameObject cannon_north;
    public GameObject cannon_east;
    public GameObject cannon_south;
    public GameObject cannon_west;

    private WakeController_Stationary wakeController;

    private byte rotationFrames_current = 0;
    private float rotationAmountPerFrame;

    // Start is called before the first frame update
    void Start()
    {
        enemyID = Constants.enemyID_ring;
        enemyHealth = 60;
        rotationAmountPerFrame = Random.Range(Constants.rotationAmountPerFrame_min, Constants.rotationAmountPerFrame_max);
        
        wakeController = gameObject.GetComponent<WakeController_Stationary>();
    }
    
    public override void advanceEnemy(float playerDistance, Vector3 playerPosition)
    {
        rotateRing(playerDistance);

        wakeController.updateWakes();
    }


    private void rotateRing(float playerDistance)
    {
        // gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, Constants.rotationAmountPerFrame);
        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, rotationAmountPerFrame);

        rotationFrames_current++;
        if (rotationFrames_current >= Constants.rotationFrames_max)
        {
            rotationFrames_current = 0;
            checkForFireCannons(playerDistance);
        }
    }

    private void checkForFireCannons(float playerDistance)
    {
        if (playerDistance < Constants.cannonballEffectiveDistance_enemy)
        {
            fireCannons();
        }
    }

    private void fireCannons()
    {
        cannonballManager.placeCannonball_enemy(cannon_north.transform.position, cannon_north.transform.forward);
        cannonballManager.placeCannonball_enemy(cannon_east.transform.position, cannon_east.transform.forward);
        cannonballManager.placeCannonball_enemy(cannon_south.transform.position, cannon_south.transform.forward);
        cannonballManager.placeCannonball_enemy(cannon_west.transform.position, cannon_west.transform.forward);
    }

    protected override void prepareToBeRemoved()
    {
        wakeController.removeAllWakes();
    }
}