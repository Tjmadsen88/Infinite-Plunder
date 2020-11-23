using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeController_Cannonball : WakeController_Abstract
{
    private Vector3 splashPosition_actual;

    public void initializeData(byte numOfCannonballs)
    {
        numOfChildren = (byte)(numOfCannonballs * 2);

        allWakeChildren_object = new GameObject[numOfChildren];
        allWakeChildren_class = new WakeIndividual[numOfChildren];

        for (byte index=0; index<numOfChildren; index++)
        {
            allWakeChildren_object[index] = Instantiate(prefab_wake, Vector3.zero, Quaternion.identity);
            allWakeChildren_class[index] = allWakeChildren_object[index].GetComponent<WakeIndividual>();
        }

        splashPosition_actual = new Vector3(0f, 0f, 0f);
    }


    public override void updateWakes()
    {
        advanceAllChildWakes();
    }

    public void cannonballSplashesAtLocation(Vector3 splashPosition)
    {
        splashPosition_actual.x = splashPosition.x;
        splashPosition_actual.z = splashPosition.z;

        placeNewWake(splashPosition_actual);
        placeNewWake(splashPosition_actual);
    }
}
