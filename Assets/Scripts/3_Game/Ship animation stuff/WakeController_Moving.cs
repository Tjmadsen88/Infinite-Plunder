using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeController_Moving : WakeController_Abstract
{
    private Vector3 previousPosition;

    private bool moving = false;

    private const byte wakeSpawnCooldownMin_moving = 2;
    private const byte wakeSpawnCooldownMax_moving = 4;
    private const byte wakeSpawnCooldownMin_standing = 60;
    private const byte wakeSpawnCooldownMax_standing = 90;

    

    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();
        previousPosition = objectTransform.position;

        // if 'destroyTime' can be a maximum of 50 frames, then...
        numOfChildren = 50 / wakeSpawnCooldownMin_moving + 1;

        allWakeChildren_object = new GameObject[numOfChildren];
        allWakeChildren_class = new WakeIndividual[numOfChildren];

        for (byte index=0; index<numOfChildren; index++)
        {
            allWakeChildren_object[index] = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
            allWakeChildren_class[index] = allWakeChildren_object[index].GetComponent<WakeIndividual>();
        }

        wakeSpawnCooldownCounter = 90;
    }


    public override void updateWakes()
    {
        if (shouldPlaceWakes)
        {
            checkForChangeInMovementState();

            if (--wakeSpawnCooldownCounter <= 0)
            {
                placeNewWake();

                if (moving)
                {
                    setCooldownTime_moving();
                } else {
                    placeNewWake();
                    setCooldownTime_standing();
                }
            }
        }

        advanceAllChildWakes();

        previousPosition = objectTransform.position;
    }

    private void checkForChangeInMovementState()
    {
        if (moving)
        {
            if (!checkIfHasMoved())
            {
                moving = false;
                setCooldownTime_standing();
            }
        } else {
            if (checkIfHasMoved())
            {
                moving = true;
                wakeSpawnCooldownCounter = 1;
            }
        }
    }

    private bool checkIfHasMoved()
    {
        return (objectTransform.position - previousPosition).magnitude > 0.05f;
    }

    private void setCooldownTime_moving()
    {
        wakeSpawnCooldownCounter = (byte)Random.Range(wakeSpawnCooldownMin_moving, wakeSpawnCooldownMax_moving);
    }

    private void setCooldownTime_standing()
    {
        wakeSpawnCooldownCounter = (byte)Random.Range(wakeSpawnCooldownMin_standing, wakeSpawnCooldownMax_standing);
    }
}
