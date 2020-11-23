using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchManagerSpace
{
    public class TouchReturnPacket
    {
        private float[] stickAngles;
        private bool[] hasTouch;
        private float[] stickCenterX;
        private float[] stickCenterY;
        private float[] touchX;
        private float[] touchY;


        public void setStickAngles(float[] stickAngles)
        {
            this.stickAngles = stickAngles;
        }

        public void setHasTouch(bool[] hasTouch)
        {
            this.hasTouch = hasTouch;
        }

        public void setStickCenterX(float[] stickCenterX)
        {
            this.stickCenterX = stickCenterX;
        }

        public void setStickCenterY(float[] stickCenterY)
        {
            this.stickCenterY = stickCenterY;
        }

        public void setTouchX(float[] touchX)
        {
            this.touchX = touchX;
        }

        public void setTouchY(float[] touchY)
        {
            this.touchY = touchY;
        }


        public float[] getStickAngles()
        {
            return stickAngles;
        }

        public bool[] getHasTouch()
        {
            return hasTouch;
        }

        public float[] getStickCenterX()
        {
            return stickCenterX;
        }

        public float[] getStickCenterY()
        {
            return stickCenterY;
        }

        public float[] getTouchX()
        {
            return touchX;
        }

        public float[] getTouchY()
        {
            return touchY;
        }

    }
}