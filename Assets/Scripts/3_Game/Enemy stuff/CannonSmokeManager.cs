using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSmokeManager : MonoBehaviour
{
    public GameObject prefab_smoke;
    private GameObject[] allSmokeChildren_object;
    private CannonSmokeIndividual[] allSmokeChildren_class;
    public byte numOfChildren = 31;
    private byte firstOpenSlot = 0;

    private Transform objectTransform;

    private const float randomMoveMax = 0.05f;

    

    // Start is called before the first frame update
    void Start()
    {
        objectTransform = gameObject.GetComponent<Transform>();

        // if the cannonBall's lifetime is 30 frames, then...
        // numOfChildren = 31;
        // numOfChildren = 31;

        allSmokeChildren_object = new GameObject[numOfChildren];
        allSmokeChildren_class = new CannonSmokeIndividual[numOfChildren];

        for (byte index=0; index<numOfChildren; index++)
        {
            allSmokeChildren_object[index] = Instantiate(prefab_smoke, Vector3.zero, Quaternion.identity);
            allSmokeChildren_class[index] = allSmokeChildren_object[index].GetComponent<CannonSmokeIndividual>();
        }
    }


    public void updateSmokes(bool placeMoreSmoke)
    {
        // if (placeMoreSmoke) placeNewSmoke();
        if (placeMoreSmoke && firstOpenSlot < numOfChildren) placeNewSmoke();
        advanceAllChildSmokes();
    }

    private void advanceAllChildSmokes()
    {
        for (byte index=0; index<firstOpenSlot; index++)
        {
            allSmokeChildren_class[index].advanceSmoke();
        }
    }

    private void placeNewSmoke()
    {
        allSmokeChildren_class[firstOpenSlot].resetSmoke(objectTransform.position, firstOpenSlot, numOfChildren, objectTransform.localScale * 0.9f);

        firstOpenSlot++;
    }

    public void resetSmokeManager()
    {
        firstOpenSlot = 0;
    }
}
