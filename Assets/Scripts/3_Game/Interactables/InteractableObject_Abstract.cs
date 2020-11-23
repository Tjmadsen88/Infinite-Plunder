using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

namespace InteractableObjectSpace
{
    abstract public class InteractableObject_Abstract
    {
        protected GameObject objectPrefab;
        protected GameObject chainObject;
        protected byte objectID;
        protected short objectOrder;
        protected bool notBeingDrawnIn = true;


        public abstract bool checkIfCloseEnoughToInteractWith(Vector3 boatPos);

        public virtual bool requestInteraction(Vector3 boatPos)
        {
            // By default, this will do nothing. Currently, only the gates override this...
            return true;
        }

        public virtual bool shouldItemBeRemovedWhenInteracted()
        {
            // By default, all items will vanish when interacted with.
            // Doors will need to overwrite this, however.
            return true;
        }

        public abstract void advanceAnimation(Vector3 boatPos);

        public virtual void prepareToRemoveObject()
        {

        }


        public GameObject getObjectPrefab()
        {
            return objectPrefab;
        }

        public GameObject getChainObject()
        {
            return chainObject;
        }

        public byte getObjectID()
        {
            return objectID;
        }

        public short getObjectOrder()
        {
            return objectOrder;
        }

        public bool getNotBeingDrawnIn()
        {
            return notBeingDrawnIn;
        }

        public void beginDrawingIn(GameObject chainObject)
        {
            notBeingDrawnIn = false;
            this.chainObject = chainObject;
        }
    }
}
