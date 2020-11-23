using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

public class DoorAnimationHandler : MonoBehaviour
{
    public GameObject fullDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    private bool animationIsOver = false;
    private bool openDoorNormal;

    private Vector3 currentRotation = new Vector3(0f, 0f, 0f);

    private float fadeAlpha = 0f;

    private const float rotateInc = 0.03f;
    private float rotateVal = -Mathf.PI / 2f;

    private Vector3 targetPosition_left = new Vector3(15f, 0f, -1f);
    private Vector3 targetPosition_right = new Vector3(-15f, 0f, -1f);


    public void updateAnimation()
    {
        if (!animationIsOver)
        {
            // if (openDoorNormal)
            // {
            //     updateAnimation_2(90f, rightDoor, leftDoor);
            // } else {
            //     updateAnimation_2(90f, leftDoor, rightDoor);
            // }

            updateAnimation_3();
        }
    }


    private void updateAnimation(float targetAngle, GameObject posDoor, GameObject negDoor)
    {
        currentRotation = Vector3.Lerp(posDoor.GetComponent<Transform>().localEulerAngles, Vector3.up * targetAngle, 0.05f);
        
        posDoor.GetComponent<Transform>().localEulerAngles = currentRotation;
        negDoor.GetComponent<Transform>().localEulerAngles = -currentRotation;

        fadeAlpha = Mathf.Abs(currentRotation.y - targetAngle) / targetAngle;
        posDoor.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, fadeAlpha));
        negDoor.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, fadeAlpha));


        if (Mathf.Abs(currentRotation.y - targetAngle) <= 0.001f)
        {
            posDoor.GetComponent<Transform>().localEulerAngles = Vector3.up * targetAngle;
            negDoor.GetComponent<Transform>().localEulerAngles = Vector3.up * -targetAngle;
            
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);

            animationIsOver = true;
        }
    }


    private void updateAnimation_2(float targetAngle, GameObject posDoor, GameObject negDoor)
    {
        rotateVal += rotateInc;
        currentRotation = targetAngle * (1f + Mathf.Sin(rotateVal)) * Vector3.up;
        
        posDoor.GetComponent<Transform>().localEulerAngles = currentRotation;
        negDoor.GetComponent<Transform>().localEulerAngles = -currentRotation;

        fadeAlpha = (targetAngle - currentRotation.y) / targetAngle;
        posDoor.GetComponent<Renderer>().material.SetColor("_Color", new Color(.8f, .8f, .8f, fadeAlpha));
        negDoor.GetComponent<Renderer>().material.SetColor("_Color", new Color(.8f, .8f, .8f, fadeAlpha));


        if (fadeAlpha <= 0.01f)
        {
            posDoor.GetComponent<Transform>().localEulerAngles = Vector3.up * targetAngle;
            negDoor.GetComponent<Transform>().localEulerAngles = Vector3.up * -targetAngle;
            
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);

            Debug.Log("door is gone!");

            animationIsOver = true;
        }
    }


    private void updateAnimation_3()
    {
        leftDoor.GetComponent<Transform>().localPosition = Vector3.Lerp(leftDoor.GetComponent<Transform>().localPosition, targetPosition_left, 0.05f);
        rightDoor.GetComponent<Transform>().localPosition = Vector3.Lerp(rightDoor.GetComponent<Transform>().localPosition, targetPosition_right, 0.05f);

        if (Mathf.Abs((leftDoor.GetComponent<Transform>().localPosition - targetPosition_left).x) <= 0.001f)
        {
            leftDoor.GetComponent<Transform>().localPosition = targetPosition_left;
            rightDoor.GetComponent<Transform>().localPosition = targetPosition_right;
            
            animationIsOver = true;
        }
    }


    public bool isAnimationOver()
    {
        return animationIsOver;
    }

    public void setOpenDoorNormal(bool openDoorNormal)
    {
        this.openDoorNormal = openDoorNormal;
        
        fullDoor.SetActive(false);
        leftDoor.SetActive(true);
        rightDoor.SetActive(true);
    }
}
