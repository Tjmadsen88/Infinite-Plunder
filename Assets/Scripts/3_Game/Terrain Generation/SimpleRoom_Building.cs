using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace SimplifiedLayoutBuilderSpace
{
    public class SimpleRoom_Building
    {
        private bool isNotEmpty = false;
        private bool hasntBeenTried = true;
        private byte clearanceLevel;
        private byte previousRoomDirection;
        private byte containsDoor;
        private byte containsItem;

        public void setInformation(byte clearanceLevel, byte previousRoomDirection, byte containsDoor, byte containsItem)
        {
            this.clearanceLevel = clearanceLevel;
            this.previousRoomDirection = previousRoomDirection;
            this.containsDoor = containsDoor;
            this.containsItem = containsItem;
            isNotEmpty = true;
        }

        public void copyDataFromOneToAnother(SimpleRoom_Building otherRoom)
        {
            clearanceLevel = otherRoom.getClearanceLevel();
            previousRoomDirection = otherRoom.getPreviousRoomDirection();
            containsDoor = otherRoom.getContainsDoor();
            containsItem = otherRoom.getContainsItem();
            isNotEmpty = otherRoom.getIsNotEmpty();
        }

        public void clearRoom()
        {
            isNotEmpty = false;
        }

        public void setHasntBeenTried(bool hasntBeenTried)
        {
            this.hasntBeenTried = hasntBeenTried;
        }

        public bool getIsNotEmpty()
        {
            return isNotEmpty;
        }

        public bool getHasntBeenTried()
        {
            return hasntBeenTried;
        }

        public bool getCanARoomBePlacedHere()
        {
            return !isNotEmpty && hasntBeenTried;
        }

        public byte getClearanceLevel()
        {
            return clearanceLevel;
        }

        public byte getPreviousRoomDirection()
        {
            return previousRoomDirection;
        }

        public byte getContainsDoor()
        {
            return containsDoor;
        }

        public byte getContainsItem()
        {
            return containsItem;
        }
    }
}