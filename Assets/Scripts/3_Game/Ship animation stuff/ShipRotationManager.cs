using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotationManager : MonoBehaviour
{
    public Transform shipTransform_delayed;
    private Transform shipTransform_real;

    private float sinVal_x;
    private float sinVal_y;
    private float sinVal_z;
    private float sinInc_x;
    private float sinInc_y;
    private float sinInc_z;
    private const float minSinInc = 0.03f;
    private const float maxSinInc = 0.05f;
    private const float maxTumbleAngle = 3f;

    private Vector3 previousForward;
    private float angleToTarget;
    private float turnAngle;

    private float leanAngle_target;
    private float leanAngle_actual;
    private const float leanAngle_inc = 0.5f;

    private const float maxTurnCutoff = 7.5f;
    private const float angleToLeanConverter = -6f;


    // Start is called before the first frame update
    void Start()
    {
        shipTransform_real = gameObject.GetComponent<Transform>();

        sinVal_x = Random.Range(0f, 2*Mathf.PI);
        sinVal_y = Random.Range(0f, 2*Mathf.PI);
        sinVal_z = Random.Range(0f, 2*Mathf.PI);
        
        sinInc_x = Random.Range(minSinInc, maxSinInc);
        sinInc_y = Random.Range(minSinInc, maxSinInc);
        sinInc_z = Random.Range(minSinInc, maxSinInc);

        previousForward = shipTransform_delayed.forward;
    }

    
    public void updateShipRotation()
    {
        shipTransform_real.position = shipTransform_delayed.position;

        angleToTarget = findAngleToVec3(Vector3.forward, shipTransform_delayed.forward);
        leanAngle_target = findLeanAngle_target();
        leanAngle_actual = findLeanAngle_actual(leanAngle_target, leanAngle_actual);

        sinVal_x = updateSinVal(sinVal_x, sinInc_x);
        sinVal_y = updateSinVal(sinVal_y, sinInc_y);
        sinVal_z = updateSinVal(sinVal_z, sinInc_z);

        shipTransform_real.eulerAngles = joinAllEulerAngles();

        previousForward = shipTransform_delayed.forward;
    }


    private float findAngleToVec3(Vector3 vector1, Vector3 vector2)
    {
        if (Vector3.Cross(vector1, vector2).y >= 0)
            return Vector3.Angle(vector1, vector2);
            
        return -Vector3.Angle(vector1, vector2);
    }

    private float findLeanAngle_target()
    {
        turnAngle = findAngleToVec3(previousForward, shipTransform_delayed.forward);

        if (turnAngle >= maxTurnCutoff) return maxTurnCutoff * angleToLeanConverter;
        if (turnAngle <= -maxTurnCutoff) return -maxTurnCutoff * angleToLeanConverter;

        return turnAngle * angleToLeanConverter;
    }

    private float findLeanAngle_actual(float leanAngle_target, float leanAngle_actual)
    {
        if (Mathf.Abs(leanAngle_target - leanAngle_actual) <= leanAngle_inc)
            return leanAngle_target;

        if (leanAngle_actual < leanAngle_target) return leanAngle_actual + leanAngle_inc;

        return leanAngle_actual - leanAngle_inc;
    }

    private float updateSinVal(float sinVal, float sinInc)
    {
        if (sinVal + sinInc <= 2*Mathf.PI) return sinVal + sinInc;

        return sinVal + sinInc - 2*Mathf.PI;
    }

    private Vector3 joinAllEulerAngles()
    {
        return new Vector3(Mathf.Sin(sinVal_x) * maxTumbleAngle, 
                            angleToTarget + Mathf.Sin(sinVal_y) * maxTumbleAngle,
                            leanAngle_actual + Mathf.Sin(sinVal_z) * maxTumbleAngle);
    }



}
