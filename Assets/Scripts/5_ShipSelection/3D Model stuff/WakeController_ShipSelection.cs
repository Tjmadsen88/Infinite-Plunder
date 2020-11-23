using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeController_ShipSelection : MonoBehaviour
{
    public GameObject prefab_wake;
    // protected GameObject[] allWakeChildren_object;
    // protected WakeIndividual[] allWakeChildren_class;

    private WakeIndividual_ShipSelection wake1;
    private WakeIndividual_ShipSelection wake2;
    // protected byte numOfChildren;
    // protected byte oldestWake = 0;
    // protected bool shouldPlaceWakes = true;

    // protected Transform objectTransform;

    private byte wakeSpawnCooldownCounter=60;
    
    private const byte wakeSpawnCooldownMin_standing = 150;
    private const byte wakeSpawnCooldownMax_standing = 200;






    // Start is called before the first frame update
    void Start()
    {
        // objectTransform = gameObject.GetComponent<Transform>();
        
        // For stationary items, there should only every be two wakes at a time:
        // numOfChildren = 2;

        // allWakeChildren_object = new GameObject[numOfChildren];
        // allWakeChildren_class = new WakeIndividual[numOfChildren];

        // for (byte index=0; index<numOfChildren; index++)
        // {
        //     allWakeChildren_object[index] = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
        //     allWakeChildren_class[index] = allWakeChildren_object[index].GetComponent<WakeIndividual>();
        // }

        GameObject tempWake = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
        wake1 = tempWake.GetComponent<WakeIndividual_ShipSelection>();

        tempWake = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
        wake2 = tempWake.GetComponent<WakeIndividual_ShipSelection>();
    }


    public void updateWakes()
    {
        if (--wakeSpawnCooldownCounter == 0)
        {
            placeNewWake();

            setCooldownTime_standing();
        }

        advanceAllChildWakes();
    }

    private void placeNewWake()
    {
        wake1.resetWake();
        wake2.resetWake();
    }

    private void setCooldownTime_standing()
    {
        wakeSpawnCooldownCounter = (byte)Random.Range(wakeSpawnCooldownMin_standing, wakeSpawnCooldownMax_standing);
    }

    private void advanceAllChildWakes()
    {
        wake1.advanceWake();
        wake2.advanceWake();
    }
}
