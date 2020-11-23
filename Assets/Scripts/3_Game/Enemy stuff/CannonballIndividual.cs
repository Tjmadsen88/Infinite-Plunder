using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class CannonballIndividual : MonoBehaviour
{
    protected Vector3 moveDirection = new Vector3(0, 0, 0); 

    protected bool isMoving = false;
    protected bool isActive = false;

    public byte lifetime_max = 30;
    protected byte lifetime_current = 0;
    public float cannonballMoveSpeed = 0.5f;

    // public GameObject cannonballImage;
    protected CannonSmokeManager cannonSmokeManager;
    

    // Start is called before the first frame update
    void Start()
    {
        cannonSmokeManager = gameObject.GetComponent<CannonSmokeManager>();
    }


    public virtual void advanceAnimation()
    {
        // if is inactive, return
        if (!isActive) return;


        if (isMoving)
        {
            gameObject.transform.LookAt(Camera.main.transform);
            gameObject.transform.position += moveDirection;

            cannonSmokeManager.updateSmokes(true);

            if (lifetime_current > 0)
            {
                lifetime_current--;
            }
        } else {
            cannonSmokeManager.updateSmokes(false);

            if (lifetime_current > 0)
            {
                lifetime_current--;
            } else {
                isActive = false;
            }
        }
    }

    public void resetCannonball(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        cannonSmokeManager.resetSmokeManager();
        
        isActive = true;
        isMoving = true;

        gameObject.transform.position = cannonballPosition;
        moveDirection = cannonballDirection.normalized * cannonballMoveSpeed;

        gameObject.SetActive(true);
        lifetime_current = lifetime_max;
    }

    public void adjustMoveMagnitude(float adjustment)
    {
        moveDirection *= adjustment;
    }

    public bool getIsActive()
    {
        return isActive;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public bool checkForTimeout()
    {
        return isMoving && lifetime_current == 0;
    }

    public void stopCannonball()
    {
        isMoving = false;

        gameObject.SetActive(false);
        lifetime_current = 60;
    }

    public Vector3 getCannonballPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 getCannonballDirection()
    {
        return moveDirection;
    }

    public void rotateDirectionToTarget(Vector3 target, float rotationAngle)
    {
        if (isMoving && target != Constants.closestEnemyPosition_null) 
            moveDirection = Vector3.RotateTowards(moveDirection, target-gameObject.transform.position, rotationAngle * (lifetime_max - lifetime_current), 0.0f);
    }
}
