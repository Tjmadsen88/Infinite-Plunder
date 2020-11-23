using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageManager : MonoBehaviour
{
    public EquippedCannonData cannonData;
    public CannonballManager cannonballManager;
    public CannonBonkManager cannonBonkManager;
    public PersistentData persistentData;

    public GameObject prefab_explosionSmoke;

    private EnemyObject_Abstract[] enemyArray_class = new EnemyObject_Abstract[8];
    int firstOpenSlotInArray = 0;


    private byte animationTimer_current = 0;
    private const byte animationTimer_end = 110;
    private const byte animationTimer_damage = 11;

    private bool animationIsPlaying = false;

    // Vector3 offsetVector = Vector3.zero;

    // bool needsToUpdateExplosionColor = true;

    Color32 explosionColor;

    private byte numOfExplosionSmokes = 120;
    ExplosionSmokeIndividual[] smokeArray_class;

    private const float smokeSpeed_initial = 0.75f;


    private Vector3 explosionCenter;
    private float explosionScale;


    // Start is called before the first frame update
    void Start()
    {
        if (cannonData.getExplodes())
        {
            smokeArray_class = new ExplosionSmokeIndividual[numOfExplosionSmokes];
            GameObject tempSmoke;
            Vector3 tempDirection;

            for (int index=0; index<numOfExplosionSmokes; index++)
            {
                tempSmoke = Instantiate(prefab_explosionSmoke, Vector3.zero, Quaternion.identity);
                smokeArray_class[index] = tempSmoke.GetComponent<ExplosionSmokeIndividual>();

                tempDirection = Quaternion.AngleAxis((float)index / (float)numOfExplosionSmokes * 360f, Vector3.up) * Vector3.forward * smokeSpeed_initial;
                smokeArray_class[index].setInitialSmokeData(tempDirection);
            }

            explosionColor = persistentData.getShipColors()[5];
        }
    }

    public void advanceAnimation()
    {
        if (animationIsPlaying)
        {
            animationTimer_current++;

            for (int index=0; index<numOfExplosionSmokes; index++)
            {
                smokeArray_class[index].advanceSmoke(explosionColor);
            }


            if (animationTimer_current == animationTimer_damage)
            {
                damageAllEnemiesInArray();
                cannonballManager.destroyAllEnemyCannonballsWithinRadius(explosionCenter, explosionScale);
            } else if (animationTimer_current == animationTimer_end)
            {
                animationIsPlaying = false;
            }
        }
    }

    public void startExplosion(Vector3 explosionCenter, float explosionScale, Color32 cannonballColor)
    {
        this.explosionCenter = explosionCenter;
        this.explosionScale = explosionScale;

        cannonballManager.destroyAllEnemyCannonballsWithinRadius(explosionCenter, explosionScale);

        for (int index=0; index<numOfExplosionSmokes; index++)
        {
            smokeArray_class[index].resetSmoke(explosionCenter, explosionColor);
        }

        animationTimer_current = 0;
        animationIsPlaying = true;
    }


    public void addEnemyToExplosionArray(EnemyObject_Abstract enemyToDamage, Vector3 enemyCenter, Vector3 explosionCenter)
    {
        // add hte enemy to the array, and possibly expand the array if needed.
        if (firstOpenSlotInArray >= enemyArray_class.Length) expandArray();

        enemyArray_class[firstOpenSlotInArray] = enemyToDamage;
        enemyArray_class[firstOpenSlotInArray].setKnockbackVector(enemyCenter - explosionCenter, cannonData.getKnockback_StartingAmount());

        firstOpenSlotInArray++;

        // Play a bonk animation,
        cannonBonkManager.placeNewBonk(enemyCenter, cannonData.getBonkSize(), true);
    }


    private void expandArray()
    {
        EnemyObject_Abstract[] newArray_class = new EnemyObject_Abstract[enemyArray_class.Length + 4];

        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            newArray_class[index] = enemyArray_class[index];
        }

        enemyArray_class = newArray_class;
    }


    private void damageAllEnemiesInArray()
    {
        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            enemyArray_class[index].cannonballHitsEnemy(cannonData.getCannonballDamage_secondary());
            enemyArray_class[index] = null;
        }

        firstOpenSlotInArray = 0;
    }

}
