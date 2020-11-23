using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorHitboxManagerSpace
{
    public class DoorHitboxObject
    {
        private byte keyID;
        private float startX;
        private float startY;
        private float endX;
        private float endY;

        public DoorHitboxObject(byte keyID, float startX, float startY, float endX, float endY)
        {
            this.keyID = keyID;
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }

        public bool checkIfPosIsInDoor(Vector2 posToCheck)
        {
            return (posToCheck.x >= startX && posToCheck.x <= endX && posToCheck.y <= startY && posToCheck.y >= endY);
        }

        public byte getKeyID()
        {
            return keyID;
        }
    }
}
