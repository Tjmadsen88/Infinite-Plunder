using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDamageManager : MonoBehaviour
{
    // public EnemyManager enemyManager;
    public CannonBonkManager cannonBonkManager;

    private EnemyObject_Abstract burningEnemy_class;
    private Transform burningEnemy_transform;
    private bool anEnemyIsBurning = false;

    private byte burnTimer_current = 0;
    private const byte burnTimer_max = 6;
    private const byte burnDamage = 6; // this should make it so an enemy takes 60 damage in 90 frames
    // This assumes that only one enemy can be burning at a time...

    private Color32 color_bonkTint = new Color32(255, 175, 0, 255);
    // private Color32 color_bonkTint = new Color32(255, 128, 0, 255);
    // private const float burnBonkScale = 0.5f;
    private const float burnBonkScale = 1f;


    public void burnEnemy(EnemyObject_Abstract enemyToBurn_class, Transform enemyToBurn_transform)
    {
        burningEnemy_class = enemyToBurn_class;
        burningEnemy_transform = enemyToBurn_transform;

        burnTimer_current = burnTimer_max;
        anEnemyIsBurning = true;
    }


    public void advanceBurningProcess()
    {
        if (anEnemyIsBurning && burnTimer_current > 0)
        {
            burnTimer_current--;

            if (burnTimer_current == 0)
            {
                if (burningEnemy_class.getShouldBeRemoved())
                {
                    burningEnemy_class = null;
                    burningEnemy_transform = null;
                    anEnemyIsBurning = false;
                    return;
                }

                // burn the enemy
                burningEnemy_class.cannonballHitsEnemy(burnDamage);
                cannonBonkManager.placeNewBonk_specificTint(burningEnemy_transform.position, color_bonkTint, burnBonkScale);

                burnTimer_current = burnTimer_max;
            }
        }
    }
}
