using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;


// abstract public class InteractableObject_Abstract
public abstract class EnemyObject_Abstract : MonoBehaviour
{
    protected byte enemyID;
    protected CannonballManager cannonballManager;
    protected bool shouldBeRemoved = false;

    protected Vector3 knockbackDirection_normalized;
    protected Vector3 knockbackDirection_current;

    protected byte knockbackDuration_current = 0;
    protected bool isBeingKnockedBack = false;

    protected byte enemyHealth;


    public abstract void advanceEnemy(float playerDistance, Vector3 playerPosition);


    public void setCannonballManager(CannonballManager cannonballManager)
    {
        this.cannonballManager = cannonballManager;
    }

    public void cannonballHitsEnemy(byte cannonballDamage)
    {
        if (enemyHealth > cannonballDamage)
        {
            enemyHealth -= cannonballDamage;
        } else if (enemyHealth > 0) {
            enemyHealth = 0;
            // play death animation.

            prepareToBeRemoved();
            shouldBeRemoved = true;
        }
    }

    protected abstract void prepareToBeRemoved();

    public bool getShouldBeRemoved()
    {
        return shouldBeRemoved;
    }

    public byte getEnemyID()
    {
        return enemyID;
    }

    public void setKnockbackVector(Vector3 knockbackDirection, float knockback_StartingAmount_enemy)
    {
        knockbackDirection_normalized = knockbackDirection.normalized;
        // knockbackDirection_current = knockbackDirection_normalized * Constants.knockback_StartingAmount_enemy;
        knockbackDirection_current = knockbackDirection_normalized * knockback_StartingAmount_enemy;
        knockbackDuration_current = 0;
        isBeingKnockedBack = true;
    }

    public void updateKnockbackVector(float knockback_dragMultiplier)
    {
        if (isBeingKnockedBack)
        {
            // knockbackDirection_current *= Constants.knockback_dragMultiplier_enemy;
            knockbackDirection_current *= knockback_dragMultiplier;

            if (knockbackDuration_current > 0)
            {
                knockbackDuration_current--;
                if (knockbackDuration_current == 0)
                {
                    isBeingKnockedBack = false;
                }
            }
        }
    }

    public bool getIsBeingKnockedBack()
    {
        return isBeingKnockedBack;
    }

    public Vector3 getKnockbackVector()
    {
        if (!isBeingKnockedBack) return Vector3.zero; 
        else return knockbackDirection_current;
    }
}