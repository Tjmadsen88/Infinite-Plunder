using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinimapSpace
{
    public class MinimapIcon
    {
        private bool hasBeenDiscovered;
        private bool isStillThere = true;
        private GameObject iconObject;
        private Vector2 uiCoords;

        public MinimapIcon(GameObject iconObject, Vector2 uiCoords, bool hasBeenDiscovered)
        {
            this.iconObject = iconObject;
            this.uiCoords = uiCoords;
            this.hasBeenDiscovered = hasBeenDiscovered;

            this.iconObject.GetComponent<RectTransform>().anchoredPosition = uiCoords;
            this.iconObject.SetActive(hasBeenDiscovered);
        }

        public void moveIcon(Vector2 uiCoords, bool hasBeenDiscovered, bool isStillThere)
        {
            this.uiCoords = uiCoords;
            this.hasBeenDiscovered = hasBeenDiscovered;
            this.isStillThere = isStillThere;

            iconObject.GetComponent<RectTransform>().anchoredPosition = uiCoords;
        }


        public void setHasBeenDiscovered(bool hasBeenDiscovered)
        {
            this.hasBeenDiscovered = hasBeenDiscovered;
        }
        
        public void setIsStillThere(bool isStillThere)
        {
            this.isStillThere = isStillThere;
        }


        public bool getHasBeenDiscovered()
        {
            return hasBeenDiscovered;
        }
        
        public bool getIsStillThere()
        {
            return isStillThere;
        }

        public GameObject getIconObject()
        {
            return iconObject;
        }

        public Vector2 getUICoords()
        {
            return uiCoords;
        }
    }
}