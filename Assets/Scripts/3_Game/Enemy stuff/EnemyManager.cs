using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using BoolArrayManagerSpace;
using DoorHitboxManagerSpace;
using PortTownSpace;


public class EnemyManager : MonoBehaviour
{
    private GameObject[] enemyArray_object = new GameObject[32];
    private EnemyObject_Abstract[] enemyArray_class = new EnemyObject_Abstract[32];
    int firstOpenSlotInArray = 0;

    public GameView gameView;
    public CannonballManager cannonballManager;
    public PersistentData persistentData;
    public InteractablesManager interactablesManager;
    public EquippedCannonData cannonData;
    public BurnDamageManager burnDamageManager;
    public ExplosionDamageManager explosionDamageManager;

    private bool[,] boolArray;
    private DoorHitboxManager doorHitboxManager;

    private byte areaWidth;
    private byte areaHeight;
    private float spawnRateMultiplier;

    private short spawnTime_minimum = 50;
    private short spawnTime_maximum = 100; 
    private short spawnTime_current = 0;

    private const float spawnRadius_minumum = 1.5f * Constants.roomWidthHeight;
    private const float spawnRadius_maximum = 4f * Constants.roomWidthHeight;
    private const float spawnRadius_portTown = 5f + Constants.cannonballEffectiveDistance_enemy;
    private float spawnRadius_current;
    private float spawnAngle;
    private Vector3 spawnPosition;

    private byte tempEnemyID;
    private bool considerReplacing;

    private Vector3 closestEnemyPosition;
    private Vector3 closestEnemyPosition_secondary;
    // private Vector3 closestEnemyPosition_working;
    private float tempEnemyDistance_smallest;
    private float tempEnemyDistance_smallest_secondary;
    private float tempEnemyDistance_individual;
    // private const float enemyDistance_tooFar = 200000f;

    public GameObject prefab_raft;
    public GameObject prefab_ring;
    public GameObject prefab_anchor;
    // public GameObject prefab_vessel;
    // public GameObject prefab_lootSmall;
    // public GameObject prefab_lootLarge;
    public LootPrefabManager lootPrefabManager;

    private const float tooFarAwayCutoff = Constants.roomWidthHeight * 2f;
    private float tempDistance;
    private Vector3 tempPosition;

    private bool shouldSpawnEnemies = true;

    private float placementRadiusMultiplier = 1f;

    private float frequencyOfEnemy_raft;
    private float frequencyOfEnemy_ring;
    // private float frequencyOfEnemy_anchor;
    private float randomRollID;

    // private byte numOfLootToSpawn = 1;

    private Vector3[] portTownLocations;
    byte numOfPortTowns;

    // private EnemyObject_Abstract burningEnemy;
    // private Vector3 burningLocation;
    // private bool anEnemyIsBurning = false;
    // private byte burnTimer_current = 0;
    // private const byte burnTimer_max = 6;
    // private const byte burnDamage = 3;



    public void placeStartingEnemies(Vector3 boatPos, bool[,] boolArray, DoorHitboxManager doorHitboxManager, byte areaWidth, byte areaHeight, PortTownReturnPacket portTownPacket, float frequencyOfEnemies)
    {
        this.boolArray = boolArray;
        this.doorHitboxManager = doorHitboxManager;
        this.areaWidth = areaWidth;
        this.areaHeight = areaHeight;
        spawnRateMultiplier = 100f / (areaWidth * areaHeight - 7);

        setSpawnFrequency(frequencyOfEnemies);

        
        numOfPortTowns = portTownPacket.getNumofPortTowns_actual();
        portTownLocations = new Vector3[numOfPortTowns];

        for (byte index=0; index<numOfPortTowns; index++)
        {
            portTownLocations[index] = portTownPacket.getPortTownLocation_asVector3(index);
        }


        if (shouldSpawnEnemies)
        {
            int numOfStartingEnemies = (int)(10 / spawnRateMultiplier);

            for (int index=0; index<3; index++)
            {
                attemptToSpawnRandomEnemy_2(boatPos);
            }
        }
    }


    public void manageEnemies(Vector3 boatPos)
    {
        if (!shouldSpawnEnemies) return;


        if (cannonData.getAppliesBurn())
        {
            burnDamageManager.advanceBurningProcess();
        }


        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            if (Mathf.Abs(enemyArray_object[index].transform.position.x - boatPos.x) < tooFarAwayCutoff && Mathf.Abs(enemyArray_object[index].transform.position.z - boatPos.z) < tooFarAwayCutoff)
            {
                if (!enemyArray_class[index].getShouldBeRemoved())
                {
                    tempDistance = (enemyArray_object[index].transform.position - boatPos).magnitude;
                    enemyArray_class[index].advanceEnemy(tempDistance, boatPos);

                    checkEnemyKnockback(index);
                } else {
                    persistentData.sunkAFoe();
                    // gameView.earnedMoney(Random.Range(25, 100) *10);    //Until I can sell the loot...

                    spawnRandomLootAtPosition(enemyArray_object[index].transform.position);

                    removeObjectFromArray(index);
                }
            }
        }


        if (spawnTime_current-- <= 0)
        {
            // spawnTime_current = (short)Random.Range(spawnTime_minimum, spawnTime_maximum);
            // attemptToSpawnRandomEnemy(boatPos, boolArray, doorHitboxManager);

            spawnTime_current = getNewSpawnTimer();
            attemptToSpawnRandomEnemy_2(boatPos);
        }
    }


    private void checkEnemyKnockback(int index)
    {
        if (enemyArray_class[index].getIsBeingKnockedBack())
        {
            enemyArray_class[index].updateKnockbackVector(cannonData.getKnockback_dragMultiplier());

            if (BoolArrayManager.checkForConflict_V3(boolArray, doorHitboxManager, enemyArray_object[index].transform.position + enemyArray_class[index].getKnockbackVector()))
            {
                enemyArray_object[index].transform.position += enemyArray_class[index].getKnockbackVector();
            }
        } 
    }


    public void advanceAllPrefabAnimations()
    {
        // for (byte index=0; index<firstOpenSlotInArray; index++)
        // {
        //     objectArray[index].advanceAnimation();
        // }
    }

    private void setSpawnFrequency(float frequencyOfEnemies)
    {   
        if (frequencyOfEnemies == 0f)
        {
            shouldSpawnEnemies = false;
            return;
        }

        spawnTime_minimum = (short)Mathf.Round(50 / frequencyOfEnemies);
        spawnTime_maximum = (short)Mathf.Round(100 / frequencyOfEnemies);

        if (frequencyOfEnemies > 0.5f)
        {
            placementRadiusMultiplier = 1f / frequencyOfEnemies;
        } else {
            placementRadiusMultiplier = 2f;
        }


        frequencyOfEnemy_raft = Random.Range(0.000001f, 1f);
        frequencyOfEnemy_ring = Random.Range(0.000001f, 1f);
        float frequencyOfEnemy_anchor = Random.Range(0.000001f, 1f);
        float totalSum = frequencyOfEnemy_raft + frequencyOfEnemy_ring + frequencyOfEnemy_anchor;

        frequencyOfEnemy_raft /= totalSum;

        frequencyOfEnemy_ring /= totalSum;
        frequencyOfEnemy_ring += frequencyOfEnemy_raft;
        // frequencyOfEnemy_anchor /= totalSum;

        // frequencyOfEnemy_raft = 1f/3f;
        // frequencyOfEnemy_ring = 2f/3f;

        Debug.Log("Spawn thresholds: "+frequencyOfEnemy_raft+", "+frequencyOfEnemy_ring+", "+1f);
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Cannonball-related stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public bool checkIfCannonballHasHitEnemy(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        for (int index=0; index < firstOpenSlotInArray; index++)
        {
            if (Constants.checkIfObjectIsCloseEnough(cannonballPosition, 
                                                    enemyArray_object[index].transform.position, 
                                                    getEnemyHitboxRadiusFromID(enemyArray_class[index].getEnemyID())))
            {
                enemyArray_class[index].cannonballHitsEnemy(cannonData.getCannonballDamage());
                enemyArray_class[index].setKnockbackVector(cannonballDirection, cannonData.getKnockback_StartingAmount());

                if (cannonData.getAppliesBurn())
                {
                    burnDamageManager.burnEnemy(enemyArray_class[index], enemyArray_object[index].transform);
                }

                return true;
            }
        }

        return false;
    }

    public void addEnemiesToExplosionManager(Vector3 explosionCenter, float explosionRadius, Color32 cannonballColor)
    {
        for (int index=0; index < firstOpenSlotInArray; index++)
        {
            if (Constants.checkIfObjectIsCloseEnough(explosionCenter, 
                                                    enemyArray_object[index].transform.position, 
                                                    explosionRadius))
            {
                explosionDamageManager.addEnemyToExplosionArray(enemyArray_class[index], enemyArray_object[index].transform.position, explosionCenter);
            }
        }
    }

    public Vector3 findClosestEnemy_noRestriction(Vector3 shipPosition, float attackRange)
    {
        closestEnemyPosition = Constants.closestEnemyPosition_null;
        tempEnemyDistance_smallest = attackRange;

        for (int index=0; index < firstOpenSlotInArray; index++)
        {
            tempPosition = enemyArray_object[index].transform.position;

            if (Mathf.Abs(shipPosition.x - tempPosition.x) < attackRange && Mathf.Abs(shipPosition.z - tempPosition.z) < attackRange)
            {
                tempEnemyDistance_individual = Mathf.Sqrt(Mathf.Pow(shipPosition.x - tempPosition.x, 2) + Mathf.Pow(shipPosition.z - tempPosition.z, 2));

                if (tempEnemyDistance_individual < tempEnemyDistance_smallest)
                {
                    closestEnemyPosition = tempPosition;
                    tempEnemyDistance_smallest = tempEnemyDistance_individual;
                }
            }
        }

        return closestEnemyPosition;
    }

    public Vector3 findClosestEnemy_withinWedge(Vector3 shipPosition, float attackRange_wedge, float attackRange_ring, Vector3 attackDirection, float halfWedgeAngle)
    {
        closestEnemyPosition = Constants.closestEnemyPosition_null;
        closestEnemyPosition_secondary = Constants.closestEnemyPosition_null;
        tempEnemyDistance_smallest = attackRange_wedge;
        tempEnemyDistance_smallest_secondary = attackRange_ring;

        for (int index=0; index < firstOpenSlotInArray; index++)
        {
            tempPosition = enemyArray_object[index].transform.position;

            if (Mathf.Abs(shipPosition.x - tempPosition.x) < attackRange_wedge && Mathf.Abs(shipPosition.z - tempPosition.z) < attackRange_wedge)
            {
                tempEnemyDistance_individual = Mathf.Sqrt(Mathf.Pow(shipPosition.x - tempPosition.x, 2) + Mathf.Pow(shipPosition.z - tempPosition.z, 2));

                if (Vector3.Angle(tempPosition - shipPosition, attackDirection) <= halfWedgeAngle)
                {
                    if (tempEnemyDistance_individual < tempEnemyDistance_smallest)
                    {
                        closestEnemyPosition = tempPosition;
                        tempEnemyDistance_smallest = tempEnemyDistance_individual;
                    }
                } else if (tempEnemyDistance_individual < attackRange_ring)
                {
                    if (tempEnemyDistance_individual < tempEnemyDistance_smallest_secondary)
                    {
                        closestEnemyPosition_secondary = tempPosition;
                        tempEnemyDistance_smallest_secondary = tempEnemyDistance_individual;
                    }
                }

                
            }
        }

        if (closestEnemyPosition == Constants.closestEnemyPosition_null) return closestEnemyPosition_secondary;
        return closestEnemyPosition;
    }


    private void spawnRandomLootAtPosition(Vector3 spawnPosition)
    {
        switch(Random.Range(0, 9))
        {
            case 0:
                interactablesManager.addObjectToArray(lootPrefabManager.getLootObject_cannon(spawnPosition), Constants.interactableID_treasureCannon, 0);
                break;

            case 1:
            case 2:
                interactablesManager.addObjectToArray(lootPrefabManager.getLootObject_large(spawnPosition), Constants.interactableID_treasureLarge, 0);
                break;

            default:
                interactablesManager.addObjectToArray(lootPrefabManager.getLootObject_small(spawnPosition), Constants.interactableID_treasureSmall, 0);
                break;
        }
    }




    // ------------------------------------------------------------------------------------------------------
    // ----------- Spawning new enemies stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private short getNewSpawnTimer()
    {
        return (short)(Random.Range(spawnTime_minimum, spawnTime_maximum) * spawnRateMultiplier);
    }

    private byte getRandomEnemyID()
    {
        // return (byte)Random.Range(0, 3);

        randomRollID = Random.Range(0f, 1f);

        if (randomRollID <= frequencyOfEnemy_raft)
        {
            return Constants.enemyID_raft;
        } else if (randomRollID <= frequencyOfEnemy_ring)
        {
            return Constants.enemyID_ring;
        } else {
            return Constants.enemyID_anchor;
        }

    }

    private GameObject instantiateNewEnemy(Vector3 enemyPosition, byte enemyID)
    {
        switch(enemyID)
        {
            case Constants.enemyID_raft:
                return Instantiate(prefab_raft, enemyPosition, Quaternion.identity);
                
            case Constants.enemyID_ring:
                return Instantiate(prefab_ring, enemyPosition, Quaternion.identity);
                
            default: //case Constants.enemyID_anchor:
                return Instantiate(prefab_anchor, enemyPosition, Quaternion.identity);
                
            // default: //case Constants.enemyID_vessel:
            //     return Instantiate(prefab_vessel, enemyPosition, Quaternion.identity);
        }
    }


    private void attemptToSpawnRandomEnemy(Vector3 boatPos)
    {
        // Create a random enemy ID:
        tempEnemyID = getRandomEnemyID();

        // Create a random placement for the enemy:
        do {
            spawnRadius_current = Random.Range(spawnRadius_minumum, spawnRadius_maximum);
            spawnAngle = Random.Range(0, 2f * Mathf.PI);

            spawnPosition = boatPos + new Vector3(Mathf.Cos(spawnAngle) * spawnRadius_current,
                                                0f,
                                                Mathf.Sin(spawnAngle) * spawnRadius_current);

            // If placement is outside the map, abort placing enemy.
            if (checkIfPositionIsOutsideTheMap(spawnPosition))
            {
                if (Random.Range(0, 2) == 0)
                {
                    Debug.Log("Enemy placement aborted, outside map. "+spawnPosition);
                    return;
                } else {
                    considerReplacing = true;
                }
            } else {
                considerReplacing = !checkIfPositionIsInWater(spawnPosition, boolArray, doorHitboxManager);
            }
        } while (considerReplacing);
        
        // If too close to another enemy, abort placing enemy.
        if (checkIfPostionIsTooCloseToAnotherEnemy(spawnPosition, getEnemyPlacementRadiusFromID(tempEnemyID)))
        {
            Debug.Log("Enemy placement aborted, too close to other enemy.");
            return;
        }

        // Now, place enemy:
        addObjectToArray(spawnPosition, tempEnemyID);
        Debug.Log("Enemy placed at: "+spawnPosition);
    }


    private void attemptToSpawnRandomEnemy_2(Vector3 boatPos)
    {
        // Create a random enemy ID:
        tempEnemyID = getRandomEnemyID();

        byte breaker = 210;

        // Create a random placement for the enemy:
        do {
            spawnPosition = new Vector3(Random.Range(2f*Constants.vertDistances, areaWidth * Constants.roomWidthHeight - 2f*Constants.vertDistances),
                                        0f,
                                        Random.Range(areaHeight * -Constants.roomWidthHeight +2f*Constants.vertDistances, -2f*Constants.vertDistances));

            if (breaker-- <= 10) return;
        } while (!checkIfPositionIsInWater(spawnPosition, boolArray, doorHitboxManager) || checkIfPostionIsTooCloseToPlayerShip(spawnPosition, boatPos, spawnRadius_minumum) ||
                    checkIfPostionIsTooCloseToAPortTown(spawnPosition, spawnRadius_portTown));
        
        // If too close to another enemy, abort placing enemy.
        if (checkIfPostionIsTooCloseToAnotherEnemy(spawnPosition, getEnemyPlacementRadiusFromID(tempEnemyID)))
        {
            // Debug.Log("Enemy placement aborted, too close to other enemy.");
            return;
        }

        // Now, place enemy:
        addObjectToArray(spawnPosition, tempEnemyID);
        // Debug.Log("Enemy placed at: "+spawnPosition);
    }


    private bool checkIfPositionIsOutsideTheMap(Vector3 position)
    {
        return (position.x < 0 || position.z > 0 || position.x > areaWidth * Constants.roomWidthHeight || position.z < areaHeight * -Constants.roomWidthHeight);
    }


    private bool checkIfPositionIsInWater(Vector3 position, bool[,] boolArray, DoorHitboxManager doorHitboxManager)
    {
        return BoolArrayManager.checkForConflict_V3(boolArray, doorHitboxManager, position);
    }


    private bool checkIfPostionIsTooCloseToAnotherEnemy(Vector3 position, float enemyPlacementRadius)
    {
        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            if (Constants.checkIfObjectIsCloseEnough(position, 
                                                    enemyArray_object[index].transform.position, 
                                                    Mathf.Min(enemyPlacementRadius, getEnemyPlacementRadiusFromID(enemyArray_class[index].getEnemyID()))))
                return true;
        }

        return false;
    }


    private bool checkIfPostionIsTooCloseToPlayerShip(Vector3 position, Vector3 boatPos, float playerShipRadius)
    {
        return Constants.checkIfObjectIsCloseEnough(position, boatPos, playerShipRadius);
    }



    private bool checkIfPostionIsTooCloseToAPortTown(Vector3 position, float noSpawnRadius)
    {
        for (int index = 0; index < numOfPortTowns; index++)
        {
            if (Constants.checkIfObjectIsCloseEnough(position, portTownLocations[index], noSpawnRadius))
                return true;
        }

        return false;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Array management stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void addObjectToArray(Vector3 enemyPosition, byte enemyID)
    {
        if (firstOpenSlotInArray >= enemyArray_class.Length) expandArray();

        switch(enemyID)
        {
            case Constants.enemyID_raft:
                enemyArray_object[firstOpenSlotInArray] = instantiateNewEnemy(enemyPosition, enemyID);
                enemyArray_class[firstOpenSlotInArray] = enemyArray_object[firstOpenSlotInArray].GetComponent<EnemyObject_Raft>();
                ((EnemyObject_Raft)enemyArray_class[firstOpenSlotInArray]).setEnemyManager(this);
                break;

            case Constants.enemyID_ring:
                enemyArray_object[firstOpenSlotInArray] = instantiateNewEnemy(enemyPosition, enemyID);
                enemyArray_class[firstOpenSlotInArray] = enemyArray_object[firstOpenSlotInArray].GetComponent<EnemyObject_Ring>();
                break;

            default: //case Constants.enemyID_anchor:
                enemyArray_object[firstOpenSlotInArray] = instantiateNewEnemy(enemyPosition, enemyID);
                enemyArray_class[firstOpenSlotInArray] = enemyArray_object[firstOpenSlotInArray].GetComponent<EnemyObject_Anchor>();
                break;
        }

        enemyArray_class[firstOpenSlotInArray].setCannonballManager(cannonballManager);
        firstOpenSlotInArray++;
    }


    private void expandArray()
    {
        GameObject[] newArray_object = new GameObject[enemyArray_object.Length * 2];
        EnemyObject_Abstract[] newArray_class = new EnemyObject_Abstract[enemyArray_object.Length * 2];

        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            newArray_object[index] = enemyArray_object[index];
            newArray_class[index] = enemyArray_class[index];
        }

        enemyArray_object = newArray_object;
        enemyArray_class = newArray_class;
    }


    private void removeObjectFromArray(int indexOfEnemy)
    {
        //Destroy the original enemy's 3D model:
        Destroy(enemyArray_object[indexOfEnemy]);

        for (int index=indexOfEnemy; index<firstOpenSlotInArray-1; index++)
        {
            enemyArray_object[index] = enemyArray_object[index+1];
            enemyArray_class[index] = enemyArray_class[index+1];
        }

        firstOpenSlotInArray--;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Misc stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------
    
    private float getEnemyPlacementRadiusFromID(byte enemyID)
    {
        return 50f * placementRadiusMultiplier;
        // return 3f;
    }

    private float getEnemyHitboxRadiusFromID(byte enemyID)
    {
        return 3f;
    }

    public void slideRaftBackwards(GameObject raftEnemy, Vector3 slideAmount)
    {
        if (BoolArrayManager.checkForConflict_V3(boolArray, doorHitboxManager, raftEnemy.transform.position + slideAmount))
        {
            raftEnemy.transform.position += slideAmount;
        }

        // raftEnemy.transform.position += BoolArrayManager.adjustMovementBasedOnWalls(boolArray, raftEnemy.transform.position, slideAmount, doorHitboxManager);
    }
}
