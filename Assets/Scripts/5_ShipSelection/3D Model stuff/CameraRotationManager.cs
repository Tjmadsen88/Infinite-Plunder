using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationManager : MonoBehaviour
{
    public Transform cameraParentTransform;
    public RectTransform canvasRect;

    private bool isPortrait = true;

    private bool hasTouch = false;
    private bool hasPreviousTouch = false;

    private Vector2 currentTouch;
    private Vector2 previousTouch;

    private Vector2 tempTouch;

    private const float rotationSpeed_horiz = 0.33f;
    private const float rotationSpeed_vert = 0.165f;
    private const float rotationMin = -5f;
    private const float rotationMax = 85f;

    private Vector3 currentCamRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentCamRotation = cameraParentTransform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        findCurrentTouchPosition();

        if (hasTouch)
        {
            if (hasPreviousTouch)
            {
                rotateCamera();
            }

            previousTouch = currentTouch;
            hasPreviousTouch = true;
        } else if (hasPreviousTouch)
        {
            hasPreviousTouch = false;
        }
    }


    public void setIsPortrait(bool isPortrait)
    {
        this.isPortrait = isPortrait;
    }






    private void findCurrentTouchPosition()
    {
        foreach(Touch touch in Input.touches)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, touch.position, null, out tempTouch);

            if (isPortrait)
            {
                if (checkIfTouchIsOnCamera_portrait(tempTouch))
                {
                    currentTouch = tempTouch;
                    hasTouch = true;
                    return;
                }
            } else {
                if (checkIfTouchIsOnCamera_landscape(tempTouch))
                {
                    currentTouch = tempTouch;
                    hasTouch = true;
                    return;
                }
            }
        }

        // Check for mouse inputs:
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out tempTouch);

        if (isPortrait)
        {
            if (Input.GetMouseButton(0) && checkIfTouchIsOnCamera_portrait(tempTouch))
            {
                currentTouch = tempTouch;
                hasTouch = true;
                return;
            } 
        } else {
            if (Input.GetMouseButton(0) && checkIfTouchIsOnCamera_landscape(tempTouch))
            {
                currentTouch = tempTouch;
                hasTouch = true;
                return;
            } 
        }

        hasTouch = false;
    }


    private bool checkIfTouchIsOnCamera_portrait(Vector2 touchPos)
    {
        // Debug.Log(touchPos);
        // return true;

        return touchPos.y > 1080;
    }

    private bool checkIfTouchIsOnCamera_landscape(Vector2 touchPos)
    {
        // Debug.Log(touchPos);
        // return true;

        return touchPos.x < -1080;
    }


    private void rotateCamera()
    {
        currentCamRotation.y = currentCamRotation.y + (currentTouch.x - previousTouch.x) * rotationSpeed_horiz;
        currentCamRotation.x = Mathf.Clamp(currentCamRotation.x - (currentTouch.y - previousTouch.y) * rotationSpeed_vert, rotationMin, rotationMax);

        cameraParentTransform.eulerAngles = currentCamRotation;
    }
}
