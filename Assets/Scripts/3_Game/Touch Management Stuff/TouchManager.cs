using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConstantsSpace;

using TouchManagerSpace;
using DoorHitboxManagerSpace;

public class TouchManager : MonoBehaviour
{
    public GameObject playerBoat_target;
    private Transform playerBoatTransform_target;
    public GameObject playerBoat_delayed;
    private Transform playerBoatTransform_delayed;
    public GameObject playerBoat_actual; // The transform for this is handled in the rotationManager.

    public GameObject targetingArrow;
    private RectTransform targetingArrow_transform;
    public GameObject reloadWheel;
    private RectTransform reloadWheel_transform;
    private Image reloadWheel_image;
    public GameObject reloadPointer;
    private RectTransform reloadPointer_transform;

    public UIManager_GameView uiManager;
    public PersistentData persistentData;
    public TargetArrowAnimationHandler targetArrowAnimationHandler;
    public CannonballManager cannonballManager;
    public EnemyManager enemyManager;
    public EquippedCannonData cannonData;
    public CannonFiringManager cannonFiringManager;

    private TouchReader touchReader;
    private TouchReader_KeyboardControls touchReader_keyboard;
    private TouchReturnPacket touchReturnPacket;

    private bool normalStickPositions;
    private byte shootingMode;

    private float[] stickAngles_raw;
    private float[] stickMagnitudes_raw;
    private bool[] hasTouches_raw;

    private float stickAngle_movement;
    private float stickAngle_cannon;
    private float stickMagnitude_movement;
    private float stickMagnitude_cannon;
    private bool hasTouch_movement;
    private bool hasTouch_cannon;

    private bool hasTouchPrevious_movement = false;

    private const byte doubleTapMax = 20;
    private byte doubleTap1_movement = 0;
    private byte doubleTap2_movement = 0;

    // private bool autodrive = false;
    // private Vector3 autodriveForward_current = new Vector3(0f, 0f, 0f);
    // private Vector3 autodriveForward_target = new Vector3(0f, 0f, 0f);

    private bool targetingArrowIsVisible = false;
    private bool reloadWheelIsVisible = false;

    private float reloadWheelReductionAmount = 1f / 60f;

    private Vector3 manualTargetingArrowRotation = new Vector3(0f, 0f, 0f);
    private Vector3 closestEnemyPosition;
    private const float assitedModeWedgeAngle = 30;

    private Vector3 worldspaceVector_movement = new Vector3(0f, 0f, 0f);
    private Vector3 worldspaceVector_cannon = new Vector3(0f, 0f, 0f);



    // Start is called before the first frame update
    void Start()
    {
        touchReader = new TouchReader(Camera.main);
        touchReader_keyboard = new TouchReader_KeyboardControls();

        normalStickPositions = persistentData.getNormalStickPositions();
        shootingMode = persistentData.getShootingMode();
        updateThumbstickBounds();

        playerBoatTransform_target = playerBoat_target.GetComponent<Transform>();
        playerBoatTransform_delayed = playerBoat_delayed.GetComponent<Transform>();

        targetingArrow_transform = targetingArrow.GetComponent<RectTransform>();
        reloadWheel_transform = reloadWheel.GetComponent<RectTransform>();
        reloadWheel_image = reloadWheel.GetComponent<Image>();
        reloadPointer_transform = reloadPointer.GetComponent<RectTransform>();

        reloadWheelReductionAmount = cannonData.getReloadWheelReductionAmount();
    }


    public void manageTouches_canMove(bool[,] boolArray, DoorHitboxManager doorHitboxManager)
    {
        if (touchReader_keyboard.getHasMouseOrKeyInput())
        {
            touchReturnPacket = touchReader_keyboard.getThumbstickData(normalStickPositions, shootingMode);
        } else {
            touchReturnPacket = touchReader.getThumbstickData();
        }

        uiManager.setThumbsticks(touchReturnPacket);

        stickAngles_raw = touchReturnPacket.getStickAngles();
        stickMagnitudes_raw = uiManager.getRealStickMagnitude();
        hasTouches_raw = touchReturnPacket.getHasTouch();
        
        setStickData();
        // checkForDoubleTap_movement();
        
        moveShip_target(stickAngle_movement, stickMagnitude_movement, boolArray, doorHitboxManager);
        
        checkCannonStick();
        cannonFiringManager.checkCannonReload(playerBoatTransform_delayed.position);

        hasTouchPrevious_movement = hasTouch_movement;
        // hasTouchPrevious_cannon = hasTouch_cannon;
    }

    public void manageTouches_halfPaused()
    {
        // checkCannonStick();
        moveShip_model();

        // stickAnglePrevious_cannon = stickAngle_cannon;
        // stickMagnitudePrevious_cannon = stickMagnitude_cannon;
    }



    private void setStickData()
    {
        if (normalStickPositions)
        {
            stickAngle_movement = stickAngles_raw[0];
            stickAngle_cannon = stickAngles_raw[1];
            stickMagnitude_movement = stickMagnitudes_raw[0];
            stickMagnitude_cannon = stickMagnitudes_raw[1];
            hasTouch_movement = hasTouches_raw[0];
            hasTouch_cannon = hasTouches_raw[1];
        } else {
            stickAngle_movement = stickAngles_raw[1];
            stickAngle_cannon = stickAngles_raw[0];
            stickMagnitude_movement = stickMagnitudes_raw[1];
            stickMagnitude_cannon = stickMagnitudes_raw[0];
            hasTouch_movement = hasTouches_raw[1];
            hasTouch_cannon = hasTouches_raw[0];
        }

        worldspaceVector_movement = convertAngMagToVector3_updateExisting(stickAngle_movement + Mathf.PI/4f, Mathf.Min(1f, stickMagnitude_movement*2f), worldspaceVector_movement);
        worldspaceVector_cannon = convertAngMagToVector3_updateExisting(stickAngle_cannon + Mathf.PI/4f, Mathf.Min(1f, stickMagnitude_cannon*2f), worldspaceVector_cannon);
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Ship movement stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    // private Vector3 convertAngMagToVector2(float angle, float magnitude)
    // {
    //     if (magnitude != 0)
    //         return new Vector2(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
    //     else return new Vector2(0f, 0f);
    // }

    private Vector3 convertAngMagToVector3(float angle, float magnitude)
    {
        if (magnitude != 0)
            return new Vector3(Mathf.Cos(angle) * magnitude, 0, Mathf.Sin(angle) * magnitude);
        else return new Vector3(0f, 0f, 0f);
    }

    private Vector3 convertAngMagToVector3_updateExisting(float angle, float magnitude, Vector3 vectorToUpdate)
    {
        if (magnitude != 0)
            vectorToUpdate.Set(Mathf.Cos(angle) * magnitude, 0f, Mathf.Sin(angle) * magnitude);
        else vectorToUpdate.Set(0f, 0f, 0f);

        return vectorToUpdate;
    }

    private void moveShip_target(float angle, float magnitude, bool[,] boolArray, DoorHitboxManager doorHitboxManager)
    {
        // if (autodrive)
        // {
        //     if (magnitude >= 0.5f)
        //     {
        //         autodriveForward_target = convertAngMagToVector3_updateExisting(angle + Mathf.PI/4f, Mathf.Min(1f, magnitude*1.25f), autodriveForward_target);
        //         // autodriveForward_target = convertAngMagToVector3(angle + Mathf.PI/4f, Mathf.Min(1f, magnitude*1.25f));
        //         updateAutodriveForward();
        //     }
                
            

        //     Constants.moveGameObject_andChangeForward(playerBoatTransform_target, 
        //                                                 autodriveForward_current * Constants.shipMoveSpeed,
        //                                                 boolArray, 
        //                                                 doorHitboxManager);
        // } else {
            Constants.moveGameObject_andChangeForward(playerBoatTransform_target, 
                                                    // convertAngMagToVector3(angle + Mathf.PI/4f, Mathf.Min(1f, magnitude*1.25f)) * Constants.shipMoveSpeed,
                                                    worldspaceVector_movement * Constants.shipMoveSpeed,
                                                    boolArray, 
                                                    doorHitboxManager);
        // }
    }

    // private void updateAutodriveForward()
    // {
    //     // autodriveForward_current = Vector3.Slerp(autodriveForward_current, autodriveForward_target, 0.1f).normalized;
    //     autodriveForward_current = Vector3.Slerp(autodriveForward_current, autodriveForward_target, 0.1f);
    // }

    private void moveShip_model()
    {
        playerBoatTransform_delayed.position = Vector3.Lerp(playerBoatTransform_delayed.position, playerBoatTransform_target.position, 0.2f);
        playerBoatTransform_delayed.forward = Vector3.Lerp(playerBoatTransform_delayed.forward, playerBoatTransform_target.forward, 0.2f);
    }


    // private void checkForDoubleTap_movement()
    // {
    //     if (hasTouch_movement && !hasTouchPrevious_movement)
    //     {
    //         if (doubleTap1_movement == 0)
    //         {
    //             doubleTap1_movement = doubleTapMax;
    //         } else {
    //             doubleTap2_movement = doubleTapMax;
    //         } 
    //     } else if (!hasTouch_movement && hasTouchPrevious_movement && doubleTap2_movement != 0)
    //     {
    //         doubleTap1_movement = 0;
    //         doubleTap2_movement = 0;
    //         performDoubleClick_movement();
    //     }

    //     if (doubleTap1_movement > 0 && --doubleTap1_movement == 0)
    //     {
    //         doubleTap1_movement = doubleTap2_movement;
    //         doubleTap2_movement = 0;
    //     }

    //     if (doubleTap2_movement > 0)
    //     {
    //         doubleTap2_movement--;
    //     }
    // }

    // private void performDoubleClick_movement()
    // {
    //     if (autodrive) autodrive = false;
    //     else {
    //         autodrive = true;
    //         autodriveForward_current = playerBoatTransform_target.forward;
    //         autodriveForward_target = autodriveForward_current;
    //     }
    // }

    public void moveShipToSpecificLocation(Vector3 newLocation)
    {
        playerBoatTransform_delayed.position = newLocation;
        playerBoatTransform_target.position = newLocation;
    }


    

    // ------------------------------------------------------------------------------------------------------
    // ----------- Cannon-firing stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void checkCannonStick()
    {
        switch (shootingMode)
        {
            case Constants.shootingMode_manual:
                makeTargetingArrowAppearDisappearWithTouches();
                if (targetingArrowIsVisible) 
                {
                    updateTargetingArrowPosition_manual();
                    if (cannonData.getShotType() == Constants.cannonShotType_fireInDirection || cannonData.getShotType() == Constants.cannonShotType_secret)
                        fireCannon(targetingArrow_transform.up);
                }
                break;

            case Constants.shootingMode_assisted:
                makeTargetingArrowAppearDisappearWithTouches();
                if (targetingArrowIsVisible) 
                {
                    updateTargetingArrowPosition_assisted();
                    if (cannonData.getShotType() == Constants.cannonShotType_fireInDirection || cannonData.getShotType() == Constants.cannonShotType_secret)
                        fireCannon(targetingArrow_transform.up);
                }
                break;

            default: //Constants.shootingMode_auto:
                checkAutoTargeting();
                break;
        }

        if (reloadWheelIsVisible) updateReloadWheel();
        // if (!canFireCannons) incrementCannonCountdown();
    }

    private void makeTargetingArrowAppearDisappearWithTouches()
    {
        if (hasTouch_cannon && !targetingArrowIsVisible && stickMagnitude_cannon >= 0.5f)
        {
            targetingArrowIsVisible = true;
            targetingArrow.SetActive(true);
            targetArrowAnimationHandler.resetAnimation();
        } else if (!hasTouch_cannon && targetingArrowIsVisible)
        {
            targetingArrowIsVisible = false;
            targetingArrow.SetActive(false);
            fireCannon(targetingArrow_transform.up);
            // cannonFiringManager.requestFireCannon(playerBoat_delayed.transform.position, targetingArrow_transform.up);
        }
    }

    private void fireCannon(Vector3 cannonballDirection)
    {
        // if (canFireCannons)
        if (cannonFiringManager.requestFireCannon(playerBoat_delayed.transform.position, cannonballDirection))
        {
            // canFireCannons = false;
            if (cannonData.getShotType() != Constants.cannonShotType_fireInDirection)
            {
                reloadWheel.SetActive(true);
                reloadPointer.SetActive(true);
                reloadWheel_image.fillAmount = 1f;
                reloadWheelIsVisible = true;
            }

            // if (shootingMode != Constants.shootingMode_auto) reloadTimer_current = reloadTimer_max;
            // else reloadTimer_current = reloadTimer_max_auto;
            
            // cannonballManager.placeCannonball_player(playerBoat_delayed.transform.position, cannonballDirection);
        }
    }

    private void updateReloadWheel()
    {
        reloadWheel_transform.position = playerBoatTransform_delayed.position;
        reloadPointer_transform.position = playerBoatTransform_delayed.position;

        reloadWheel_image.fillAmount -= reloadWheelReductionAmount;
        reloadPointer_transform.localEulerAngles = new Vector3(0f, 0f, reloadWheel_image.fillAmount * 360f + 45f);

        if (reloadWheel_image.fillAmount <= 0)
        {
            reloadWheel.SetActive(false);
            reloadPointer.SetActive(false);
            reloadWheelIsVisible = false;
        }
    }
    
    // private void incrementCannonCountdown()
    // {
    //     if (reloadTimer_current > 1)
    //     {
    //         reloadTimer_current--;
    //     } else {
    //         reloadCannon();
    //     }
    // }

    // private void reloadCannon()
    // {
    //     canFireCannons = true;
    // }


    private void updateTargetingArrowPosition_manual()
    {
        manualTargetingArrowRotation.z = stickAngle_cannon * 180f / Mathf.PI - 45f;

        targetingArrow_transform.position = playerBoatTransform_delayed.position;
        targetingArrow_transform.localEulerAngles = manualTargetingArrowRotation;
        targetArrowAnimationHandler.advanceAnimation();
    }


    private void updateTargetingArrowPosition_assisted()
    {
        // closestEnemyPosition = enemyManager.findClosestEnemy_withinWedge(playerBoatTransform_delayed.position, Constants.cannonballAutoTargetingDistance_assisted,
        //                                                                 Constants.cannonballAutoTargetingDistance_assisted_secondary, worldspaceVector_cannon, assitedModeWedgeAngle);
        closestEnemyPosition = enemyManager.findClosestEnemy_withinWedge(playerBoatTransform_delayed.position, cannonData.getAutoTargetingDistance_assisted(),
                                                                        Constants.cannonballAutoTargetingDistance_assisted_secondary, worldspaceVector_cannon, assitedModeWedgeAngle);

        if (closestEnemyPosition != Constants.closestEnemyPosition_null)
        {
            manualTargetingArrowRotation.z = Vector3.SignedAngle(Vector3.forward, closestEnemyPosition - playerBoatTransform_delayed.position, Vector3.down);

            targetingArrow_transform.position = playerBoatTransform_delayed.position;
            targetingArrow_transform.localEulerAngles = manualTargetingArrowRotation;
            targetArrowAnimationHandler.advanceAnimation();
        } else {
            updateTargetingArrowPosition_manual();
        }
    }


    private void checkAutoTargeting()
    {
        // if (canFireCannons)
        // {
            // closestEnemyPosition = enemyManager.findClosestEnemy_noRestriction(playerBoatTransform_delayed.position, Constants.cannonballAutoTargetingDistance_auto);
            closestEnemyPosition = enemyManager.findClosestEnemy_noRestriction(playerBoatTransform_delayed.position, cannonData.getAutoTargetingDistance_auto());

            if (closestEnemyPosition != Constants.closestEnemyPosition_null)
            {
                fireCannon(closestEnemyPosition - playerBoatTransform_delayed.transform.position);
            }
        // }
    }



    

    // ------------------------------------------------------------------------------------------------------
    // ----------- Mid-Game Settings stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void clearBothTouches()
    {
        touchReader.clearBothTouches();
        touchReader_keyboard.clearBothTouches();
    }

    public void setThumbstickPositions(bool normalStickPositions)
    {
        this.normalStickPositions = normalStickPositions;
        updateThumbstickBounds();

        persistentData.setNormalStickPositions(normalStickPositions);
        persistentData.saveMidGameSettingsData();
    }

    public void setShootingMode(byte shootingMode)
    {
        this.shootingMode = shootingMode;
        updateThumbstickBounds();

        persistentData.setShootingMode(shootingMode);
        persistentData.saveMidGameSettingsData();
    }

    private void updateThumbstickBounds()
    {
        if (shootingMode != Constants.shootingMode_auto)
        {
            touchReader.setStickBoundary(0.5f);
        } else {
            if (normalStickPositions)
            {
                touchReader.setStickBoundary(1f);
            } else {
                touchReader.setStickBoundary(0f);
            }
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Misc stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void setArrowAndWheelColors(Color32 waterColor)
    {
        // Color32 darkerColor = Color32.Lerp(waterColor, Color.white, 0.5f);
        // Color32 lighterColor = Color32.Lerp(waterColor, Color.white, 0.75f);
        Color32 darkerColor = Color32.Lerp(waterColor, Color.white, 0.33f);
        // Color32 darkerColor = Color32.Lerp(waterColor, Color.red, 0.5f);
        Color32 lighterColor = Color32.Lerp(waterColor, Color.white, 0.66f);

        reloadWheel_image.color = darkerColor;
        reloadPointer.GetComponent<Image>().color = darkerColor;
        targetingArrow.GetComponent<Image>().color = lighterColor;
    }

    public void removeCannonArrow()
    {
        hasTouch_cannon = false;
        targetingArrowIsVisible = false;
        targetingArrow.SetActive(false);

        // canFireCannons = true;
        reloadWheel.SetActive(false);
        reloadPointer.SetActive(false);
    }

}
