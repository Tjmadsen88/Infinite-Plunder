using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchManagerSpace
{
    public class TouchReader
    {
        private Camera mainCam;

        // Index 0 is for left touches, 1 for right touches.
        private float[] touchX = new float[2];
        private float[] touchY = new float[2];

        private bool[] hasTouch = new bool[2];

        private float[] stickCenterX = {0f, 0f};
        private float[] stickCenterY = {0f, 0f};

        private float[] stickAngle = {0f, 0f};

        private bool[] stickHasAppeared = {false, false};

        private float stickBoundary = 0.5f;

        private int[] storedTouchIds = new int[2];
        
        private bool[] touchFound = new bool[2];

        private TouchReturnPacket returnPacket = new TouchReturnPacket();


        public TouchReader(Camera mainCam)
        {
            this.mainCam = mainCam;
        }

        public void setStickBoundary(float stickBoundary)
        {
            this.stickBoundary = stickBoundary;
        }



        public TouchReturnPacket getThumbstickData()
        {
            organizeTouches();
            analyzeTouches();

            returnPacket.setStickAngles(stickAngle);
            returnPacket.setHasTouch(hasTouch);
            returnPacket.setStickCenterX(stickCenterX);
            returnPacket.setStickCenterY(stickCenterY);
            returnPacket.setTouchX(touchX);
            returnPacket.setTouchY(touchY);

            return returnPacket;
        }



        private void organizeTouches()
        {
            // touchX[0] = 0f;
            // touchX[1] = 0f;
            // touchY[0] = 0f;
            // touchY[1] = 0f;

            touchFound[0] = false;
            touchFound[1] = false;

            foreach(Touch touch in Input.touches)
            {
                if (touch.position.x < stickBoundary * mainCam.pixelWidth)
                {
                    if (!hasTouch[0] && !(hasTouch[1] && touch.fingerId == storedTouchIds[1]))
                    {
                        hasTouch[0] = true;
                        storedTouchIds[0] = touch.fingerId;
                        touchX[0] = touch.position.x;
                        touchY[0] = touch.position.y;
                    }
                } else {
                    if (!hasTouch[1] && !(hasTouch[0] && touch.fingerId == storedTouchIds[0]))
                    {
                        hasTouch[1] = true;
                        storedTouchIds[1] = touch.fingerId;
                        touchX[1] = touch.position.x;
                        touchY[1] = touch.position.y;
                    }
                }

                // if (hasTouch[0] && touch.fingerId == storedTouchIds[0])
                if (hasTouch[0] && touch.fingerId == storedTouchIds[0] && (touch.position.x - touchX[0])/Mathf.Max(mainCam.pixelWidth, mainCam.pixelHeight) < 0.1f)
                {
                    touchX[0] = touch.position.x;
                    touchY[0] = touch.position.y;
                    touchFound[0] = true;
                }

                // if (hasTouch[1] && touch.fingerId == storedTouchIds[1])
                if (hasTouch[1] && touch.fingerId == storedTouchIds[1] && (touch.position.x - touchX[1])/Mathf.Max(mainCam.pixelWidth, mainCam.pixelHeight) < 0.1f)
                {
                    touchX[1] = touch.position.x;
                    touchY[1] = touch.position.y;
                    touchFound[1] = true;
                }
            }

            if (!touchFound[0]) hasTouch[0] = false;
            if (!touchFound[1]) hasTouch[1] = false;
        }



        private void analyzeTouches()
        {
            //Start with left (0), then do right (1)
            for (int screenSide = 0; screenSide < 2; screenSide++)
            {
                //first... see if you need to add a stick base.
                if (hasTouch[screenSide] && !stickHasAppeared[screenSide])
                {
                    stickHasAppeared[screenSide] = true;
                    stickCenterX[screenSide] = touchX[screenSide];
                    stickCenterY[screenSide] = touchY[screenSide];
                }
                //then... see if you need to remove a stick base.
                else if (!hasTouch[screenSide] && stickHasAppeared[screenSide])
                {
                    stickHasAppeared[screenSide] = false;
                }

                //then... for any stick with a touch, get the angle and magnitude:
                if (hasTouch[screenSide])
                {
                    stickAngle[screenSide] = getStickAngle(screenSide);
                } else {
                    stickAngle[screenSide] = 0f;
                }
            }

            
        }



        private float getStickAngle(int screenSide)
        {
            float xAxis = touchX[screenSide] - stickCenterX[screenSide];
            float yAxis = touchY[screenSide] - stickCenterY[screenSide];

            return Mathf.Atan2(yAxis, xAxis);
        }
        

        public void clearBothTouches()
        {
            hasTouch[0] = false;
            hasTouch[1] = false;
            stickHasAppeared[0] = false;
            stickHasAppeared[1] = false;
        }
    }
}
