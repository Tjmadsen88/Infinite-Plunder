using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace InteractableObjectSpace
{
    public class InteractableObject_PortTownPier : InteractableObject_Abstract
    {
        public InteractableObject_PortTownPier(GameObject objectPrefab, byte objectID, short objectOrder)
        {
            this.objectPrefab = objectPrefab;
            this.objectID = objectID;
            this.objectOrder = objectOrder;
        }

        public override bool checkIfCloseEnoughToInteractWith(Vector3 boatPos)
        {
            return Constants.checkIfObjectIsCloseEnough(boatPos, 
                                                    objectPrefab.transform.position, 
                                                    Constants.footprintRadius_portTownPier);
        }

        public override void advanceAnimation(Vector3 boatPos)
        {
            // Piers have no animations...
        }
    }
}
