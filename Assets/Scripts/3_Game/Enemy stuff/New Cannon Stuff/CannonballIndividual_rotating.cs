using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class CannonballIndividual_rotating : CannonballIndividual
{
    private Vector3 cannonballCenter;
    private float currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        cannonSmokeManager = gameObject.GetComponent<CannonSmokeManager>();
    }


    public override void advanceAnimation()
    {
        if (!isActive) return;


        if (isMoving)
        {
            gameObject.transform.LookAt(Camera.main.transform);
            // gameObject.transform.position += moveDirection;
            updateCannonballPosition();

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

    // public void resetCannonball_chainShot(Vector3 cannonballPosition, Vector3 cannonballDirection, bool isRight)
    public void resetCannonball_chainShot(Vector3 cannonballPosition, Vector3 cannonballDirection, float startingAngle)
    {
        cannonballCenter = cannonballPosition;

        // if (isRight) currentAngle = 45f;
        // else currentAngle = 225f;
        currentAngle = startingAngle;

        resetCannonball(cannonballPosition, cannonballDirection);
    }

    private void updateCannonballPosition()
    {
        cannonballCenter += moveDirection;
        currentAngle += Constants.cannonChainRotationInc;

        gameObject.transform.position = cannonballCenter + Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.right * Constants.cannonChainRadius;
    }
}
