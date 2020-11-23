using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace InteractableObjectSpace
{
    public class InteractableObject_Gate : InteractableObject_Abstract
    {
        private bool doorShouldOpen = false;
        private bool animationIsOver = false;

        private DoorAnimationHandler doorAnimationHandler;


        public InteractableObject_Gate(GameObject objectPrefab, byte objectID, short objectOrder)
        {
            this.objectPrefab = objectPrefab;
            this.objectID = objectID;
            this.objectOrder = objectOrder;

            doorAnimationHandler = objectPrefab.GetComponent<DoorAnimationHandler>();
        }

        public override bool checkIfCloseEnoughToInteractWith(Vector3 boatPos)
        {
            if (!animationIsOver)
            {
                return Constants.checkIfObjectIsCloseEnough_rectangular(boatPos,
                                                                        objectPrefab.transform.position - objectPrefab.transform.forward.normalized, 
                                                                        objectPrefab.transform.right, 
                                                                        objectPrefab.transform.forward, 
                                                                        Constants.footprintRadius_gateWidth,
                                                                        Constants.footprintRadius_gateForward);
            } else {
                // If the animation is over, make the hitbox much larger so it can be removed:
                return Constants.checkIfObjectIsCloseEnough_rectangular(boatPos,
                                                                        objectPrefab.transform.position, 
                                                                        objectPrefab.transform.right, 
                                                                        objectPrefab.transform.forward, 
                                                                        10000f,
                                                                        10000f);
            }
            
        }

        public override bool requestInteraction(Vector3 boatPos)
        {  
            if (!doorShouldOpen)
            {
                doorShouldOpen = true;

                if (Vector3.Angle((boatPos - objectPrefab.transform.position), objectPrefab.transform.forward) <= 90)
                    doorAnimationHandler.setOpenDoorNormal(true);
                else doorAnimationHandler.setOpenDoorNormal(false);

                return true;
            }

            return false;
        }

        public override bool shouldItemBeRemovedWhenInteracted()
        {
            return animationIsOver;
        }

        public override void advanceAnimation(Vector3 boatPos)
        {
            if (doorShouldOpen)
            {
                doorAnimationHandler.updateAnimation();

                animationIsOver = doorAnimationHandler.isAnimationOver();
            }
        }
    }
}