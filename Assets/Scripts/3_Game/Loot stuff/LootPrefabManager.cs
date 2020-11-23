using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class LootPrefabManager : MonoBehaviour
{
    public GameObject prefab_small_gold;
    public GameObject prefab_large_gold;
    public GameObject prefab_large_cannon1;
    public GameObject prefab_shipGold;

    private GameObject tempObject;


    public GameObject getLootObject_small(Vector3 dropPosition)
    {
        tempObject = Instantiate(prefab_small_gold, dropPosition, Quaternion.identity);
        tempObject.transform.localScale = getRandomPrefabScale();

        return tempObject;
    }

    public GameObject getLootObject_large(Vector3 dropPosition)
    {
        tempObject = Instantiate(prefab_large_gold, dropPosition, Quaternion.identity);
        tempObject.transform.localScale = getRandomPrefabScale();

        return tempObject;
    }

    public GameObject getLootObject_cannon(Vector3 dropPosition)
    {
        tempObject = Instantiate(prefab_large_cannon1, dropPosition, Quaternion.identity);
        tempObject.transform.localScale = getRandomPrefabScale();

        return tempObject;
    }

    public GameObject getLootObject_ship(Vector3 dropPosition)
    {
        tempObject = Instantiate(prefab_shipGold, dropPosition, Quaternion.identity);
        tempObject.transform.localScale = getRandomPrefabScale();

        return tempObject;
    }


    private Vector3 getRandomPrefabScale()
    {
        switch(Random.Range(0, 4))
        {
            case 0:
                return new Vector3(1f, 1f, 1f);

            case 1:
                return new Vector3(-1f, 1f, 1f);
                
            case 2:
                return new Vector3(1f, 1f, -1f);
                
            default:
                return new Vector3(-1f, 1f, -1f);
        }
    }

}
