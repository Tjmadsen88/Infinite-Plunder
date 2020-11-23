using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;


public class EnemyObject_Anchor : EnemyObject_Abstract
{
    public GameObject anchorBase;
    public GameObject cannon;

    private WakeController_Stationary wakeController;

    private byte cannonCooldown_main_current = Constants.cannonCooldown_anchor_main_max;
    private byte cannonCooldown_sub_current = 0;
    private bool canFireCannon = false;
    private bool isFiringCannon = false;

    private byte shotCannonballNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        enemyID = Constants.enemyID_anchor;
        enemyHealth = 90;
        wakeController = gameObject.GetComponent<WakeController_Stationary>();

        cannon.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
    }
    
    public override void advanceEnemy(float playerDistance, Vector3 playerPosition)
    {
        cannon.transform.forward = Vector3.RotateTowards(cannon.transform.forward, playerPosition - gameObject.transform.position, Constants.cannonTrackSpeed_anchor, 0.0f);

        if (!canFireCannon)
        {
            handleCannonCooldown();
        } else if (isFiringCannon)
        {
            handleCannonBurstFire();
        } else if (playerDistance < Constants.cannonballEffectiveDistance_enemy)
        {
            beginFiringCannon();
        }

        wakeController.updateWakes();
    }


    private void handleCannonCooldown()
    {
        cannonCooldown_main_current--;
        if (cannonCooldown_main_current == 0)
        {
            canFireCannon = true;
        }
    }

    private void handleCannonBurstFire()
    {
        if (--cannonCooldown_sub_current == 0)
        {
            fireCannon();
            if (shotCannonballNum == 3) 
            {
                isFiringCannon = false;
                canFireCannon = false;
                cannonCooldown_main_current = Constants.cannonCooldown_anchor_main_max;
            }
        }
    }

    private void fireCannon()
    {
        cannonballManager.placeCannonball_enemy(cannon.transform.position, cannon.transform.forward);
        cannonCooldown_sub_current = Constants.cannonCooldown_anchor_sub_max;
        shotCannonballNum++;
    }

    private void beginFiringCannon()
    {
        shotCannonballNum = 0;
        isFiringCannon = true;
        fireCannon();
    }

    protected override void prepareToBeRemoved()
    {
        wakeController.removeAllWakes();
    }
}