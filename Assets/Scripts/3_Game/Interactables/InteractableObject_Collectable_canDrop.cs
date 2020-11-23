using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace InteractableObjectSpace
{
    public class InteractableObject_Collectable_canDrop : InteractableObject_Abstract
    {
        private WakeController_Stationary wakeController;
        private float moveAmount = 0f;
        private float chainDist = 0f;

        public InteractableObject_Collectable_canDrop(GameObject objectPrefab, byte objectID, short objectOrder)
        {
            this.objectPrefab = objectPrefab;
            this.objectID = objectID;
            this.objectOrder = objectOrder;

            wakeController = objectPrefab.GetComponent<WakeController_Stationary>();
        }

        public override bool checkIfCloseEnoughToInteractWith(Vector3 boatPos)
        {
            if (notBeingDrawnIn)
            {
                return Constants.checkIfObjectIsCloseEnough(boatPos, 
                                                    objectPrefab.transform.position, 
                                                    Constants.footprintRadius_collectables_drawIn);
            } else {
                return Constants.checkIfObjectIsCloseEnough(boatPos, 
                                                    objectPrefab.transform.position, 
                                                    Constants.footprintRadius_collectables);
            }
        }

        public override void advanceAnimation(Vector3 boatPos)
        {
            objectPrefab.transform.eulerAngles += Vector3.up * Constants.itemRotationAmount;
            wakeController.updateWakes();

            if (!notBeingDrawnIn)
            {
                if (chainDist < 1f)
                {
                    chainDist += Constants.itemChainMoveSpeed;
                    if (chainDist > 1f) chainDist = 1f;
                } else {
                    moveAmount += Constants.itemAcceleration;
                    objectPrefab.transform.position = Vector3.MoveTowards(objectPrefab.transform.position, boatPos, moveAmount);
                }

                updateChainPosition(boatPos);
            }
        }

        private void updateChainPosition(Vector3 boatPos)
        {
            chainObject.transform.position = boatPos;
            chainObject.transform.LookAt(objectPrefab.transform.position);

            chainObject.transform.localScale = new Vector3(1f, 1f, (objectPrefab.transform.position - boatPos).magnitude * chainDist);
        }

        public override void prepareToRemoveObject()
        {
            // wakeController.removeAllWakes();
            wakeController.hideAllWakes();
        }
    }
}