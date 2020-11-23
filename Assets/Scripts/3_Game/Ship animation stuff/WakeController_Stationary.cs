using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeController_Stationary : WakeController_Abstract
{
    private const byte wakeSpawnCooldownMin_standing = 60;
    private const byte wakeSpawnCooldownMax_standing = 90;

    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();
        
        // For stationary items, there should only every be two wakes at a time:
        numOfChildren = 2;

        allWakeChildren_object = new GameObject[numOfChildren];
        allWakeChildren_class = new WakeIndividual[numOfChildren];

        for (byte index=0; index<numOfChildren; index++)
        {
            allWakeChildren_object[index] = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
            allWakeChildren_class[index] = allWakeChildren_object[index].GetComponent<WakeIndividual>();
        }
    }


    public override void updateWakes()
    {
        if (shouldPlaceWakes && --wakeSpawnCooldownCounter <= 0)
        {
            placeNewWake();
            placeNewWake();

            setCooldownTime_standing();
        }

        advanceAllChildWakes();
    }

    private void setCooldownTime_standing()
    {
        wakeSpawnCooldownCounter = (byte)Random.Range(wakeSpawnCooldownMin_standing, wakeSpawnCooldownMax_standing);
    }
}
