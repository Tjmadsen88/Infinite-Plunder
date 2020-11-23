using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace LootTailSpace
{
    public class LootPieceIndividual
    {
        GameObject lootPrefab;
        private byte lootID;
        private byte arrayLength;
        private Vector3[] positionArray;
        private Vector3[] eulerAngleArray;

        private byte currentArrayPosition = 0;


        public LootPieceIndividual(GameObject lootPrefab, byte lootID, byte pieceAheadID, Vector3 pieceAheadPosition, Vector3 pieceAheadEulerAngles)
        {
            this.lootPrefab = lootPrefab;
            this.lootID = lootID;

            arrayLength = (byte)(getArrayLengthFromID(lootID) + getArrayLengthFromID(pieceAheadID));

            positionArray = new Vector3[arrayLength];
            eulerAngleArray = new Vector3[arrayLength];

            for (int index=0; index < arrayLength; index++)
            {
                positionArray[index] = pieceAheadPosition;
                eulerAngleArray[index] = pieceAheadEulerAngles;
            }

            lootPrefab.transform.position = pieceAheadPosition;
        }


        private byte getArrayLengthFromID(byte lootID)
        {
            switch(lootID)
            {
                // case 0: //The ship's ID... sort of.
                //     return Constants.lootFollowDistance_ship;

                case Constants.interactableID_none:
                    return Constants.lootFollowDistance_emptychain;

                case Constants.interactableID_treasureSmall:
                    return Constants.lootFollowDistance_small;
                
                case Constants.interactableID_treasureLarge:
                    return Constants.lootFollowDistance_large;

                default: //case Constants.interactableID_treasureFinal:
                    return Constants.lootFollowDistance_final;
            }
        }


        public void updateLootPosition(Vector3 pieceAheadPosition, Vector3 pieceAheadEulerAngles)
        {
            positionArray[currentArrayPosition] = pieceAheadPosition;
            eulerAngleArray[currentArrayPosition] = pieceAheadEulerAngles;

            currentArrayPosition++;
            if (currentArrayPosition >= arrayLength) currentArrayPosition = 0;

            lootPrefab.transform.position = positionArray[currentArrayPosition];
            lootPrefab.transform.eulerAngles = eulerAngleArray[currentArrayPosition];
        }


        public GameObject getLootPrefab()
        {
            return lootPrefab;
        }


        public byte getLootID()
        {
            return lootID;
        }


        public Vector3 getCurrentPosition()
        {
            return positionArray[currentArrayPosition];
        }

        public Vector3 getCurrentEulerAngles()
        {
            return eulerAngleArray[currentArrayPosition];
        }
    }
}

