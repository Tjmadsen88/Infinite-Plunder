using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using BoolArrayManagerSpace;
using DoorHitboxManagerSpace;

public class CannonballManager : MonoBehaviour
{
    private byte numOfCannonballs_player;
    private byte numOfCannonballs_enemy;

    private CannonballIndividual[] cannonballArray_player;
    private CannonballIndividual[] cannonballArray_enemies;
    byte oldestCannonball_player = 0;
    byte oldestCannonball_enemies = 0;

    public GameView gameView;
    public WakeController_Cannonball wakeController;
    public EnemyManager enemyManager;
    public CannonBonkManager cannonBonkManager;
    public EquippedCannonData cannonData;
    public ExplosionDamageManager explosionDamageManager;
    // public CameraManager cameraManager;

    private bool[,] boolArray;
    private DoorHitboxManager doorHitboxManager;

    public GameObject prefab_cannonball_player;
    public GameObject prefab_cannonball_player_grapeshot;
    public GameObject prefab_cannonball_player_chainShot;
    public GameObject prefab_cannonball_player_rapid;
    public GameObject prefab_cannonball_player_magical;
    public GameObject prefab_cannonball_player_explosive;
    public GameObject prefab_cannonball_player_hotShot;
    public GameObject prefab_cannonball_player_secret;
    public GameObject prefab_cannonball_enemy;

    private Vector3 tempCannonballPosition;
    private CannonballIndividual tempCannonballClass;

    private Color32 color_cannonballEnemy = new Color32(209, 60, 60, 255);
    private Color32 color_cannonballPlayer = new Color32(80, 80, 80, 255);

    private float tempChainAngle;


    public void initializeCannonballData(bool[,] boolArray, DoorHitboxManager doorHitboxManager, float frequencyOfEnemies, Color32 color_cannonballPlayer)
    {
        this.boolArray = boolArray;
        this.doorHitboxManager = doorHitboxManager;
        this.color_cannonballPlayer = color_cannonballPlayer;

        // numOfCannonballs_player = 2;
        numOfCannonballs_player = cannonData.getStaringNumOfCannonballs();
        if (frequencyOfEnemies <= 1)
        {
            numOfCannonballs_enemy = 25;
        } else {
            numOfCannonballs_enemy = (byte)(15 + Mathf.Round(10*frequencyOfEnemies)); //this number should never be larger than 127...
            Debug.Log("NumOfCannonballs... "+numOfCannonballs_enemy);
        }

        wakeController.initializeData(numOfCannonballs_enemy);

        cannonballArray_player = new CannonballIndividual[numOfCannonballs_player];
        cannonballArray_enemies = new CannonballIndividual[numOfCannonballs_enemy];

        GameObject playerCannonballPrefab = getPlayerCannonballPrefab();
        GameObject tempCannonballObject;

        for (byte index=0; index<numOfCannonballs_player; index++)
        {
            tempCannonballObject = Instantiate(playerCannonballPrefab, Vector3.zero, Quaternion.identity);
            cannonballArray_player[index] = tempCannonballObject.GetComponent<CannonballIndividual>();

            tempCannonballObject.GetComponent<SpriteRenderer>().color = color_cannonballPlayer;
            tempCannonballObject.transform.localScale = Vector3.one * cannonData.getCannonballSize();
        }

        for (byte index=0; index<numOfCannonballs_enemy; index++)
        {
            tempCannonballObject = Instantiate(prefab_cannonball_enemy, Vector3.zero, Quaternion.identity);
            cannonballArray_enemies[index] = tempCannonballObject.GetComponent<CannonballIndividual>();
        }


        if (!cannonData.getExplodes())
            cannonBonkManager.initializeCannnonbonkArray((byte)(numOfCannonballs_player + numOfCannonballs_enemy/2), color_cannonballPlayer, color_cannonballEnemy);
        else cannonBonkManager.initializeCannnonbonkArray((byte)(numOfCannonballs_player * 8 + numOfCannonballs_enemy/2), color_cannonballPlayer, color_cannonballEnemy);
    }


    private GameObject getPlayerCannonballPrefab()
    {
        switch(cannonData.getCannonballPrefabID())
        {
            case Constants.cannonballPrefabID_grapeshot:
                return prefab_cannonball_player_grapeshot;
                
            case Constants.cannonballPrefabID_chainShot:
                return prefab_cannonball_player_chainShot;
                
            case Constants.cannonballPrefabID_rapid:
                return prefab_cannonball_player_rapid;
                
            case Constants.cannonballPrefabID_mystical:
                return prefab_cannonball_player_magical;

            case Constants.cannonballPrefabID_explosive:
                return prefab_cannonball_player_explosive;

            case Constants.cannonballPrefabID_hotShot:
                return prefab_cannonball_player_hotShot;

            case Constants.cannonballPrefabID_secret:
                return prefab_cannonball_player_secret;

            default:
                return prefab_cannonball_player;
        }
    }


    public void moveAllActiveCannonballs()
    {
        for (byte index=0; index<numOfCannonballs_player; index++)
        {
            tempCannonballClass = cannonballArray_player[index];
            tempCannonballClass.advanceAnimation();

            if (cannonData.getAutoTargets())
            {
                tempCannonballClass.rotateDirectionToTarget(enemyManager.findClosestEnemy_noRestriction(tempCannonballClass.getCannonballPosition(), 
                                                                                                        Constants.roomWidthHeight), 
                                                            Constants.cannonAutoTargetingSpeed);
            }

            checkForDeactivateCannonball(tempCannonballClass, false);

            // if (tempCannonballClass.getIsMoving() && enemyManager.checkIfCannonballHasHitEnemy(tempCannonballClass.getCannonballPosition(), tempCannonballClass.getCannonballDirection()))
            // {
            //     deactivateCannonball(tempCannonballClass);
            //     cannonBonkManager.placeNewBonk(tempCannonballClass.getCannonballPosition() + tempCannonballClass.getCannonballDirection() * 2f, color_cannonballPlayer, cannonData.getCannonballSize()/5f);
            // }
        }

        for (byte index=0; index<numOfCannonballs_enemy; index++)
        {
            tempCannonballClass = cannonballArray_enemies[index];
            tempCannonballClass.advanceAnimation();

            checkForDeactivateCannonball(tempCannonballClass, true);

            // if (tempCannonballClass.getIsMoving() && gameView.checkIfCannonballHasHitPlayer(tempCannonballClass.getCannonballPosition(), tempCannonballClass.getCannonballDirection()))
            // {
            //     deactivateCannonball(tempCannonballClass);
            //     cannonBonkManager.placeNewBonk(tempCannonballClass.getCannonballPosition(), color_cannonballEnemy, 1f);
            //     // cameraManager.playDamageShakeAnimation();
            // }
        }

        wakeController.updateWakes();
        cannonBonkManager.advanceAllCannonbonkAnimations();
        explosionDamageManager.advanceAnimation();
    }


    private void checkForDeactivateCannonball(CannonballIndividual cannonballClass, bool isEnemyCannonball)
    {
        // if cannonball is currently inactive, return
        if (!cannonballClass.getIsMoving())
            return;

        // if cannonball's lifetime is 0, or if it hits a wall/door (enemy collision will happen someplace else)
        //      call a 'makeInactive' funcion on the cannonball, and place two new wakes there.
        // if (cannonballClass.checkForTimeout() || !BoolArrayManager.checkForConflict_V3(boolArray, doorHitboxManager, cannonballClass.getCannonballPosition()))
        // if (cannonballClass.checkForTimeout() || !BoolArrayManager.checkForConflict_V3_noDoors(boolArray, cannonballClass.getCannonballPosition()))
        // if (cannonballClass.checkForTimeout() || 
        //         ((isEnemyCannonball || !cannonData.getPassesThroughWalls()) && !BoolArrayManager.checkForConflict_V3_noDoors(boolArray, cannonballClass.getCannonballPosition())))
        // {
        //     deactivateCannonball(cannonballClass);
        // }

        // Check to see if the cannonball has timed out:
        if (cannonballClass.checkForTimeout())
        {
            deactivateCannonball(cannonballClass);
            return;
        }

        // Then check to see if it has hit a target:
        if (isEnemyCannonball)
        {
            // The enemy's cannonballs:
            if (gameView.checkIfCannonballHasHitPlayer(cannonballClass.getCannonballPosition(), cannonballClass.getCannonballDirection()))
            {
                deactivateCannonball(cannonballClass);
                cannonBonkManager.placeNewBonk(cannonballClass.getCannonballPosition(), 1f, false);
                return;
            }

            if (!BoolArrayManager.checkForConflict_V3_noDoors(boolArray, cannonballClass.getCannonballPosition()))
            {
                deactivateCannonball(cannonballClass);
                return;
            }
        } else {
            // The player's cannonballs:
            if (enemyManager.checkIfCannonballHasHitEnemy(cannonballClass.getCannonballPosition(), cannonballClass.getCannonballDirection()))
            {
                deactivateCannonball(cannonballClass);
                // cannonBonkManager.placeNewBonk(cannonballClass.getCannonballPosition() + cannonballClass.getCannonballDirection() * 2f, cannonData.getBonkSize(), true);
                
                if (cannonData.getExplodes())
                    makeCannonballExplode(cannonballClass.getCannonballPosition());
                else 
                    cannonBonkManager.placeNewBonk(cannonballClass.getCannonballPosition() + cannonballClass.getCannonballDirection() * 2f, cannonData.getBonkSize(), true);

                return;
            }
            
            if (!cannonData.getPassesThroughWalls() && !BoolArrayManager.checkForConflict_V3_noDoors(boolArray, cannonballClass.getCannonballPosition()))
            {
                deactivateCannonball(cannonballClass);
                
                if (cannonData.getExplodes())
                    makeCannonballExplode(cannonballClass.getCannonballPosition());

                return;
            }
        }
    }


    private void deactivateCannonball(CannonballIndividual cannonballClass)
    {
        cannonballClass.stopCannonball();
        wakeController.cannonballSplashesAtLocation(cannonballClass.getCannonballPosition());
    }


    private void makeCannonballExplode(Vector3 cannonballPosition)
    {
        explosionDamageManager.startExplosion(cannonballPosition, Constants.cannonExplosionRadius, color_cannonballPlayer);
        enemyManager.addEnemiesToExplosionManager(cannonballPosition, Constants.cannonExplosionRadius, color_cannonballPlayer);
    }




    public void placeCannonball_player(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        cannonballArray_player[oldestCannonball_player].resetCannonball(cannonballPosition, cannonballDirection);

        oldestCannonball_player++;
        if (oldestCannonball_player >= numOfCannonballs_player) oldestCannonball_player = 0;
    }

    public void placeCannonball_player_grapeshot(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        cannonballArray_player[oldestCannonball_player].resetCannonball(cannonballPosition, cannonballDirection);
        cannonballArray_player[oldestCannonball_player].adjustMoveMagnitude(Random.Range(0.9f, 1.1f));

        oldestCannonball_player++;
        if (oldestCannonball_player >= numOfCannonballs_player) oldestCannonball_player = 0;
    }

    public void placeCannonball_player_chainShot(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        tempChainAngle = Random.Range(0f, 360f);

        ((CannonballIndividual_rotating)cannonballArray_player[oldestCannonball_player]).resetCannonball_chainShot(cannonballPosition, cannonballDirection, tempChainAngle);

        oldestCannonball_player++;
        if (oldestCannonball_player >= numOfCannonballs_player) oldestCannonball_player = 0;
        

        ((CannonballIndividual_rotating)cannonballArray_player[oldestCannonball_player]).resetCannonball_chainShot(cannonballPosition, cannonballDirection, tempChainAngle + 180f);

        oldestCannonball_player++;
        if (oldestCannonball_player >= numOfCannonballs_player) oldestCannonball_player = 0;
    }

    public void placeCannonball_enemy(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        cannonballArray_enemies[oldestCannonball_enemies].resetCannonball(cannonballPosition, cannonballDirection);

        oldestCannonball_enemies++;
        if (oldestCannonball_enemies >= numOfCannonballs_enemy) oldestCannonball_enemies = 0;
    }


    public void destroyAllEnemyCannonballsWithinRadius(Vector3 explosionCenter, float explosionRadius)
    {
        for (int index=0; index < numOfCannonballs_enemy; index++)
        {
            if ( cannonballArray_enemies[index].getIsMoving() && Constants.checkIfObjectIsCloseEnough(explosionCenter, 
                                                                cannonballArray_enemies[index].getCannonballPosition(), 
                                                                explosionRadius))
            {
                deactivateCannonball(cannonballArray_enemies[index]);
                // cannonBonkManager.placeNewBonk(cannonballArray_enemies[index].getCannonballPosition(), cannonData.getCannonballSize()/5f, true);
                cannonBonkManager.placeNewBonk(cannonballArray_enemies[index].getCannonballPosition(), 1f, true);
            }
        }
    }
}
