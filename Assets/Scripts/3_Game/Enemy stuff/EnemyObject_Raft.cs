using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;


public class EnemyObject_Raft : EnemyObject_Abstract
{
    public GameObject raftBase;
    public GameObject cannon;
    private EnemyManager enemyManager;

    private WakeController_Moving wakeController;

    private byte cannonCooldown_current = Constants.cannonCooldown_raft_max;
    private bool canFireCannon = false;

    private float currentMoveSpeed = 0f;
    private Vector3 cannonDirection;

    
    // Start is called before the first frame update
    void Start()
    {
        enemyID = Constants.enemyID_raft;
        enemyHealth = 30;
        wakeController = gameObject.GetComponent<WakeController_Moving>();

        raftBase.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
        cannon.transform.eulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
    }

    public void setEnemyManager(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }
    
    public override void advanceEnemy(float playerDistance, Vector3 playerPosition)
    {
        cannon.transform.forward = Vector3.RotateTowards(cannon.transform.forward, playerPosition - gameObject.transform.position, Constants.cannonTrackSpeed_raft, 0.0f);

        if (!canFireCannon)
        {
            handleCannonCooldown();
            slideRaftBackwards();
        } else if (playerDistance < Constants.cannonballEffectiveDistance_player)
        {
            fireCannon();
        }

        wakeController.updateWakes();
    }


    private void handleCannonCooldown()
    {
        cannonCooldown_current--;
        if (cannonCooldown_current == 0)
        {
            canFireCannon = true;
        }
    }

    private void fireCannon()
    {
        // gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, Random.Range(-Constants.maxRotationAfterFiring_raft, Constants.maxRotationAfterFiring_raft));
        cannonDirection = cannon.transform.forward;

        cannonballManager.placeCannonball_enemy(cannon.transform.position, cannonDirection);
        cannonCooldown_current = Constants.cannonCooldown_raft_max;

        canFireCannon = false;
        cannonCooldown_current = Constants.cannonCooldown_raft_max;

        currentMoveSpeed = Constants.shipMoveSpeed;
    }

    private void slideRaftBackwards()
    {
        currentMoveSpeed *= 0.97f;

        enemyManager.slideRaftBackwards(gameObject, cannonDirection * -currentMoveSpeed);
    }

    protected override void prepareToBeRemoved()
    {
        wakeController.removeAllWakes();
    }
}
