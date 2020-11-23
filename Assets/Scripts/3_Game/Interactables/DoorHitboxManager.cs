using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace DoorHitboxManagerSpace
{
    public class DoorHitboxManager
    {
        private DoorHitboxObject[] hitboxArray;
        byte lastFilledSlot;

        private const float doorWidth = 1f; // This is relative to the half-thickness of the door

        public DoorHitboxManager(byte[,] lockedDoorLocations, byte[] lockedDoorSide, byte[] lockedDoorColors)
        {
            hitboxArray = new DoorHitboxObject[lockedDoorSide.Length];
            lastFilledSlot = (byte)(lockedDoorSide.Length);

            for (int index=0; index<lockedDoorSide.Length; index++)
            {
                hitboxArray[index] = buildOneDoorObject(lockedDoorLocations[index, 0], lockedDoorLocations[index, 1], lockedDoorSide[index], lockedDoorColors[index]);
            }
        }

        private DoorHitboxObject buildOneDoorObject(byte doorX, byte doorY, byte lockedDoorSide, byte lockedDoorColor)
        {
            switch(lockedDoorSide)
            {
                case Constants.doorID_left:
                    return new DoorHitboxObject(lockedDoorColor, 
                                                (doorX ) * Constants.roomWidthHeight - doorWidth, 
                                                (doorY ) * -Constants.roomWidthHeight, 
                                                (doorX ) * Constants.roomWidthHeight + doorWidth, 
                                                (doorY + 1) * -Constants.roomWidthHeight );
                    
                case Constants.doorID_up:
                    return new DoorHitboxObject(lockedDoorColor, 
                                                (doorX ) * Constants.roomWidthHeight, 
                                                (doorY) * -Constants.roomWidthHeight + doorWidth, 
                                                (doorX + 1) * Constants.roomWidthHeight, 
                                                (doorY ) * -Constants.roomWidthHeight - doorWidth);
                    
                case Constants.doorID_down:
                    return new DoorHitboxObject(lockedDoorColor, 
                                                (doorX ) * Constants.roomWidthHeight, 
                                                (doorY + 1) * -Constants.roomWidthHeight + doorWidth, 
                                                (doorX + 1) * Constants.roomWidthHeight, 
                                                (doorY + 1) * -Constants.roomWidthHeight - doorWidth);

                default: //Constants.doorID_right:
                    return new DoorHitboxObject(lockedDoorColor, 
                                                (doorX + 1) * Constants.roomWidthHeight - doorWidth, 
                                                (doorY ) * -Constants.roomWidthHeight, 
                                                (doorX + 1) * Constants.roomWidthHeight + doorWidth, 
                                                (doorY + 1) * -Constants.roomWidthHeight );
            }
        }


        public bool checkIfPosIsInDoor(Vector2 posToCheck)
        {
            for (byte index=0; index<lastFilledSlot; index++)
            {
                if (hitboxArray[index].checkIfPosIsInDoor(posToCheck))
                    return true;
            }

            return false;
        }

        public void openAllDoorsByColor(byte keyID)
        {
            byte numOfDoorsFound = 0;
            byte doorToOpen = convertKeyIDtoDoorID(keyID);

            for (byte index=0; index<lastFilledSlot; index++)
            {
                if (hitboxArray[index].getKeyID() == doorToOpen)
                {
                    numOfDoorsFound++;
                } else {
                    hitboxArray[index-numOfDoorsFound] = hitboxArray[index];
                }
            }

            lastFilledSlot -= numOfDoorsFound;
        }

        private byte convertKeyIDtoDoorID(byte keyID)
        {
            switch (keyID)
            {
                case Constants.interactableID_key1:
                    return Constants.interactableID_door1;

                case Constants.interactableID_key2:
                    return Constants.interactableID_door2;
                    
                case Constants.interactableID_key3:
                    return Constants.interactableID_door3;
                    
                case Constants.interactableID_key4:
                    return Constants.interactableID_door4;
                    
                case Constants.interactableID_key5:
                    return Constants.interactableID_door5;
                    
                case Constants.interactableID_key6:
                    return Constants.interactableID_door6;

                default:
                    return (byte)200;
            }
        }
    }
}
