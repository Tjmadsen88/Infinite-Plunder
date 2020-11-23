using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace SimplifiedLayoutBuilderSpace
{
    public class SimpleRoom_Output
    {
        private bool isNotEmpty = false;
        private bool[] wallIsOpen = {false, false, false, false};
        private bool[] cornerIsOpen = {false, false, false, false};
        private byte containsItem;

        public void setInformation(bool[] wallIsOpen, bool[] cornerIsOpen, byte containsItem)
        {
            this.wallIsOpen = wallIsOpen;
            this.cornerIsOpen = cornerIsOpen;
            this.containsItem = containsItem;
            isNotEmpty = true;
        }

        public void setIsNotEmpty(bool isNotEmpty)
        {
            this.isNotEmpty = isNotEmpty;
        }

        public void setWallAsOpen(byte wallNum)
        {
            wallIsOpen[wallNum] = true;
        }

        public void setCornerAsOpen(byte cornerNum)
        {
            cornerIsOpen[cornerNum] = true;
        }

        public void setContainsItem(byte containsItem)
        {
            this.containsItem = containsItem;
        }

        public bool getIsNotEmpty()
        {
            return isNotEmpty;
        }

        // public byte getDoorStatus_interactable(byte doorNum)
        // {
        //     switch (doorStatus[doorNum])
        //     {
        //         case Constants.simplified_doorStatus_key1:
        //             return Constants.interactableID_door1;
                    
        //         case Constants.simplified_doorStatus_key2:
        //             return Constants.interactableID_door2;
                    
        //         case Constants.simplified_doorStatus_key3:
        //             return Constants.interactableID_door3;
                    
        //         case Constants.simplified_doorStatus_key4:
        //             return Constants.interactableID_door4;
                    
        //         case Constants.simplified_doorStatus_key5:
        //             return Constants.interactableID_door5;
                    
        //         default: //case Constants.simplified_doorStatus_key6:
        //             return Constants.interactableID_door6;
        //     }
        // }

        // public bool getRoomContainsALockedDoor()
        // {
        //     for (byte index=0; index<4; index++)
        //     {
        //         if (getDoorIsLocked(index))
        //             return true;
        //     }
            
        //     return false;
        // }

        // public bool getDoorIsLocked(byte doorNum)
        // {
        //     if (doorStatus[doorNum] == Constants.simplified_doorStatus_key1 || 
        //         doorStatus[doorNum] == Constants.simplified_doorStatus_key2 || 
        //         doorStatus[doorNum] == Constants.simplified_doorStatus_key3 || 
        //         doorStatus[doorNum] == Constants.simplified_doorStatus_key4 || 
        //         doorStatus[doorNum] == Constants.simplified_doorStatus_key5 || 
        //         doorStatus[doorNum] == Constants.simplified_doorStatus_key6 )
        //         return true;

        //     return false;
        // }

        public bool getWallIsOpen(byte wallNum)
        {
            return wallIsOpen[wallNum];
        }

        public bool getCornerIsOpen(byte doorNum)
        {
            return cornerIsOpen[doorNum];
        }

        public byte getItem()
        {
            return containsItem;
        }

        public bool getRoomContainsAnItem()
        {
            return containsItem != Constants.interactableID_none;
        }
    }
}