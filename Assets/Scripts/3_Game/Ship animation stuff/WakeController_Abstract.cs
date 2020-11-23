using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WakeController_Abstract : MonoBehaviour
{
    public GameObject prefab_wake;
    protected GameObject[] allWakeChildren_object;
    protected WakeIndividual[] allWakeChildren_class;
    protected byte numOfChildren;
    protected byte oldestWake = 0;
    protected bool shouldPlaceWakes = true;

    protected Transform objectTransform;

    protected byte wakeSpawnCooldownCounter=30;


    public abstract void updateWakes();

    protected void advanceAllChildWakes()
    {
        for (byte index=0; index<numOfChildren; index++)
        {
            allWakeChildren_class[index].advanceWake();
        }
    }

    public void removeAllWakes()
    {
        for (byte index=0; index<numOfChildren; index++)
        {
            Destroy(allWakeChildren_object[index]);
        }
    }

    public void hideAllWakes()
    {
        for (byte index=0; index<numOfChildren; index++)
        {
            allWakeChildren_class[index].hideWake();
        }
    }

    public void setShouldPlaceWakes(bool shouldPlaceWakes)
    {
        this.shouldPlaceWakes = shouldPlaceWakes;
    }

    protected void placeNewWake()
    {
        placeNewWake(objectTransform.position);
    }

    protected void placeNewWake(Vector3 newPosition)
    {
        allWakeChildren_class[oldestWake].resetWake(newPosition);

        oldestWake++;
        if (oldestWake >= numOfChildren) oldestWake = 0;
    }
}
