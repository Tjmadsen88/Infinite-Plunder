using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace TouchManagerSpace
{
    public class TouchReader_KeyboardControls
    {
        // Index 0 is for left touches, 1 for right touches.
        private float[] touchX_combined = new float[2];
        private float[] touchY_combined = new float[2];

        private bool[] hasTouch_combined = new bool[2];

        private float[] stickCenterX_combined = {0f, 0f};
        private float[] stickCenterY_combined = {0f, 0f};

        private float[] stickAngle_combined = {0f, 0f};


        private bool hasTouch_arrows = false;
        private float stickAngle_arrows;
        private const float stickCenterXY_arrows = -10000f;
        // private const float stickCenterXY_arrows = 0;
        private float touchX_arrows;
        private float touchY_arrows;

        private byte switchByte_arrows;
        private const float stickOffsetPos_arrows = 100f;
        private const float stickOffsetPos_arrows_diag = 70.71f;

        private Vector2 direction_arrows_target = Vector2.zero;
        private Vector2 direction_arrows_current = Vector2.zero;
        private float lerpSpeed_arrows = 0.125f;


        private bool hasTouch_mouse = false;
        private float stickAngle_mouse;
        private float stickCenterX_mouse;
        private float stickCenterY_mouse;
        private float touchX_mouse;
        private float touchY_mouse;

        private bool stickHasAppeared_mouse = false;


        private TouchReturnPacket returnPacket = new TouchReturnPacket();



        public bool getHasMouseOrKeyInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)
                || Input.GetMouseButton(0))
            {
                return true;
            } else {
                hasTouch_arrows = false;
                direction_arrows_target.x = 0;
                direction_arrows_target.y = 0;
                direction_arrows_current.x = 0;
                direction_arrows_current.y = 0;

                hasTouch_mouse = false;
                stickHasAppeared_mouse = false;
                return false;
            }

            // return true;
        }



        public TouchReturnPacket getThumbstickData(bool normalStickPositions, byte shootingMode)
        {
            getArrowKeyData();
            getMouseData(shootingMode);

            setStickData(normalStickPositions);

            return returnPacket;
        }



        private void getArrowKeyData()
        {
            switchByte_arrows = 0;

            //Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
            //        || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) switchByte_arrows += 1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) switchByte_arrows += 2;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) switchByte_arrows += 4;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) switchByte_arrows += 8;

            if (switchByte_arrows == 0)
            {
                hasTouch_arrows = false;
                return;
            }

            hasTouch_arrows = true;

            switch (switchByte_arrows)
            {
                case 1: // Up
                case 11: // Up, Left and Right
                    direction_arrows_target.x = 0.01f;
                    direction_arrows_target.y = stickOffsetPos_arrows;
                    break;
                    
                case 2: // Left
                case 7: // Up, Left and Down
                    direction_arrows_target.x = -stickOffsetPos_arrows;
                    direction_arrows_target.y = -0.01f;
                    break;
                    
                case 3: // Up and Left
                    direction_arrows_target.x = -stickOffsetPos_arrows_diag;
                    direction_arrows_target.y = stickOffsetPos_arrows_diag;
                    break;
                    
                case 4: // Down
                case 14: // Left, Down, and Right
                    direction_arrows_target.x = 0.01f;
                    direction_arrows_target.y = -stickOffsetPos_arrows;
                    break;
                    
                case 5: // Up and Down
                case 10: // Left and Right
                case 15: // Up, Left, Down and Right
                    direction_arrows_target.x = 0;
                    direction_arrows_target.y = 0;
                    break;
                    
                case 6: // Left and Down
                    direction_arrows_target.x = -stickOffsetPos_arrows_diag;
                    direction_arrows_target.y = -stickOffsetPos_arrows_diag;
                    break;
                    
                case 8: // Right
                case 13: // Up, Down and Right
                    direction_arrows_target.x = stickOffsetPos_arrows;
                    direction_arrows_target.y = -0.01f;
                    break;
                    
                case 9: // Up and Right
                    direction_arrows_target.x = stickOffsetPos_arrows_diag;
                    direction_arrows_target.y = stickOffsetPos_arrows_diag;
                    break;
                    
                // case 12: // Down and Right
                default:
                    direction_arrows_target.x = stickOffsetPos_arrows_diag;
                    direction_arrows_target.y = -stickOffsetPos_arrows_diag;
                    break;
            }

            direction_arrows_current = Vector2.Lerp(direction_arrows_current, direction_arrows_target, lerpSpeed_arrows);

            stickAngle_arrows = Mathf.Atan2(direction_arrows_current.y, direction_arrows_current.x);

            touchX_arrows = stickCenterXY_arrows + direction_arrows_current.x;
            touchY_arrows = stickCenterXY_arrows + direction_arrows_current.y;
        }



        private void getMouseData(byte shootingMode)
        {
            // If shooting mode is auto, abort:
            if (shootingMode == Constants.shootingMode_auto)
            {
                hasTouch_mouse = false;
                return;
            }


            hasTouch_mouse = Input.GetMouseButton(0);

            if (hasTouch_mouse)
            {
                touchX_mouse = Input.mousePosition.x;
                touchY_mouse = Input.mousePosition.y;
            }


            // see if you need to add a stick base.
            if (hasTouch_mouse && !stickHasAppeared_mouse)
            {
                stickHasAppeared_mouse = true;
                stickCenterX_mouse = touchX_mouse;
                stickCenterY_mouse = touchY_mouse;
            }
            //then... see if you need to remove a stick base.
            else if (!hasTouch_mouse && stickHasAppeared_mouse)
            {
                stickHasAppeared_mouse = false;
            }

            //then... get the angle of the stick:
            if (hasTouch_mouse)
            {
                // stickAngle_mouse = Mathf.Atan2(yAxis, xAxis);
                stickAngle_mouse = Mathf.Atan2(touchY_mouse - stickCenterY_mouse, touchX_mouse - stickCenterX_mouse);
            } else {
                stickAngle_mouse = 0f;
            }
        }

        

        public void clearBothTouches()
        {
            hasTouch_arrows = false;
            hasTouch_mouse = false;
            stickHasAppeared_mouse = false;
        }

        

        private void setStickData(bool normalStickPositions)
        {
            if (normalStickPositions)
            {
                stickAngle_combined[0] = stickAngle_arrows;
                stickAngle_combined[1] = stickAngle_mouse;
                hasTouch_combined[0] = hasTouch_arrows;
                hasTouch_combined[1] = hasTouch_mouse;
                stickCenterX_combined[0] = stickCenterXY_arrows;
                stickCenterX_combined[1] = stickCenterX_mouse;
                stickCenterY_combined[0] = stickCenterXY_arrows;
                stickCenterY_combined[1] = stickCenterY_mouse;
                touchX_combined[0] = touchX_arrows;
                touchX_combined[1] = touchX_mouse;
                touchY_combined[0] = touchY_arrows;
                touchY_combined[1] = touchY_mouse;
            } else {
                stickAngle_combined[1] = stickAngle_arrows;
                stickAngle_combined[0] = stickAngle_mouse;
                hasTouch_combined[1] = hasTouch_arrows;
                hasTouch_combined[0] = hasTouch_mouse;
                stickCenterX_combined[1] = stickCenterXY_arrows;
                stickCenterX_combined[0] = stickCenterX_mouse;
                stickCenterY_combined[1] = stickCenterXY_arrows;
                stickCenterY_combined[0] = stickCenterY_mouse;
                touchX_combined[1] = touchX_arrows;
                touchX_combined[0] = touchX_mouse;
                touchY_combined[1] = touchY_arrows;
                touchY_combined[0] = touchY_mouse;
            }
            
            returnPacket.setStickAngles(stickAngle_combined);
            returnPacket.setHasTouch(hasTouch_combined);
            returnPacket.setStickCenterX(stickCenterX_combined);
            returnPacket.setStickCenterY(stickCenterY_combined);
            returnPacket.setTouchX(touchX_combined);
            returnPacket.setTouchY(touchY_combined);
        }
    }
}
