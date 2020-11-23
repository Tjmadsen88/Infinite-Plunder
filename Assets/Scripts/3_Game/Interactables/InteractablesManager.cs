using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using InteractableObjectSpace;

public class InteractablesManager : MonoBehaviour
{
    private InteractableObject_Abstract[] objectArray = new InteractableObject_Abstract[32];
    private int firstOpenSlotInArray = 0;
    public GameView gameView;
    public LootTailManager lootTailManager;

    private const byte interactCounter_max = 6;
    private byte interactCounter_current = interactCounter_max;

    private byte initialDelay = 30;

    public GameObject prefab_lootChain;


    public void checkInteractionWithAllObjects(Vector3 boatPos)
    {
        if (interactCounter_current-- <= 0)
        {
            interactCounter_current = interactCounter_max;

            for (int index=0; index<firstOpenSlotInArray; index++)
            {
                if (checkIfObjectIsCloseEnough(boatPos, index))
                {
                    interactWithObject(index, boatPos);
                }
            }
        }
    }

    public void advanceAllPrefabAnimations(Vector3 boatPos)
    {
        if (initialDelay == 0)
        {
            for (byte index=0; index<firstOpenSlotInArray; index++)
            {
                // Debug.Log("Object Index: "+index);
                objectArray[index].advanceAnimation(boatPos);
            }
        } else {
            initialDelay--;
        }
    }

    private void interactWithObject(int indexOfObject, Vector3 boatPos)
    {
        switch(objectArray[indexOfObject].getObjectID())
        {
            case Constants.interactableID_key1:
            case Constants.interactableID_key2:
            case Constants.interactableID_key3:
            case Constants.interactableID_key4:
            case Constants.interactableID_key5:
            case Constants.interactableID_key6:
                if (objectArray[indexOfObject].getNotBeingDrawnIn())
                {
                    objectArray[indexOfObject].beginDrawingIn(Instantiate(prefab_lootChain, Vector3.zero, Quaternion.identity));
                } else {
                    objectArray[indexOfObject].prepareToRemoveObject();
                    Destroy(objectArray[indexOfObject].getObjectPrefab());
                    Destroy(objectArray[indexOfObject].getChainObject());
                    gameView.collectedKey(objectArray[indexOfObject].getObjectID(), objectArray[indexOfObject].getObjectOrder());
                    removeObjectFromArray(indexOfObject);
                }
                break;
                
            case Constants.interactableID_door1:
            case Constants.interactableID_door2:
            case Constants.interactableID_door3:
            case Constants.interactableID_door4:
            case Constants.interactableID_door5:
            case Constants.interactableID_door6:
                if (gameView.doIHaveTheKey(objectArray[indexOfObject].getObjectID()))
                {
                    if (objectArray[indexOfObject].requestInteraction(boatPos))
                    {
                        gameView.openedDoor(objectArray[indexOfObject].getObjectOrder());
                    }
                    
                    if (objectArray[indexOfObject].shouldItemBeRemovedWhenInteracted())
                    {
                        removeObjectFromArray(indexOfObject);
                    }
                }
                break;

            case Constants.interactableID_treasureSmall:
            case Constants.interactableID_treasureLarge:
            case Constants.interactableID_treasureCannon:
                if (objectArray[indexOfObject].getNotBeingDrawnIn())
                {
                    objectArray[indexOfObject].beginDrawingIn(Instantiate(prefab_lootChain, Vector3.zero, Quaternion.identity));
                } else {
                    objectArray[indexOfObject].prepareToRemoveObject();
                    // lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getObjectID(), boatPos);
                    lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getChainObject(),
                                                        objectArray[indexOfObject].getObjectID(), boatPos);
                    removeObjectFromArray(indexOfObject);
                }
                break;

            case Constants.interactableID_treasureShip:
                if (objectArray[indexOfObject].getNotBeingDrawnIn())
                {
                    objectArray[indexOfObject].beginDrawingIn(Instantiate(prefab_lootChain, Vector3.zero, Quaternion.identity));
                } else {
                    objectArray[indexOfObject].prepareToRemoveObject();
                    // lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getObjectID(), boatPos);
                    lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getChainObject(),
                                                        objectArray[indexOfObject].getObjectID(), boatPos);

                    // gameView.collectedShipTreasure(objectArray[indexOfObject].getObjectOrder());
                    removeObjectFromArray(indexOfObject);
                }
                break;
                
            case Constants.interactableID_treasureFinal:
                if (objectArray[indexOfObject].getNotBeingDrawnIn())
                {
                    objectArray[indexOfObject].beginDrawingIn(Instantiate(prefab_lootChain, Vector3.zero, Quaternion.identity));
                } else {
                    objectArray[indexOfObject].prepareToRemoveObject();
                    // lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getObjectID(), boatPos);
                    lootTailManager.addLootToEndOfTail(objectArray[indexOfObject].getObjectPrefab(), objectArray[indexOfObject].getChainObject(),
                                                        objectArray[indexOfObject].getObjectID(), boatPos);

                    gameView.collectedFinalTreasure();
                    removeObjectFromArray(indexOfObject);
                }
                break;
            
                
            // case Constants.interactableID_treasureFinal:
            //     removeObjectFromArray(indexOfObject);
            //     // For now...
            //     gameView.soldFinalTreasure();
            //     break;
            
                
            case Constants.interactableID_portTownPier:
                gameView.interactWithPortTown(objectArray[indexOfObject].getObjectPrefab().transform.position);
                break;
        }
    }

    public void addObjectToArray(GameObject objectPrefab, byte objectID, short objectOrder)
    {
        if (firstOpenSlotInArray >= objectArray.Length) expandArray();

        // Debug.Log("Adding item with ID: "+objectID);

        switch(objectID)
        {
            case Constants.interactableID_key1:
            case Constants.interactableID_key2:
            case Constants.interactableID_key3:
            case Constants.interactableID_key4:
            case Constants.interactableID_key5:
            case Constants.interactableID_key6:
            case Constants.interactableID_treasureSmall:
            case Constants.interactableID_treasureLarge:
            case Constants.interactableID_treasureCannon:
            case Constants.interactableID_treasureShip:
                objectArray[firstOpenSlotInArray] = new InteractableObject_Collectable(objectPrefab, objectID, objectOrder);
                break;
                
            case Constants.interactableID_door1:
            case Constants.interactableID_door2:
            case Constants.interactableID_door3:
            case Constants.interactableID_door4:
            case Constants.interactableID_door5:
            case Constants.interactableID_door6:
                objectArray[firstOpenSlotInArray] = new InteractableObject_Gate(objectPrefab, objectID, objectOrder);
                break;

            // case Constants.interactableID_treasureSmall:
            // case Constants.interactableID_treasureLarge:
            // case Constants.interactableID_treasureShip:
            case Constants.interactableID_treasureFinal:
            // default:
                objectArray[firstOpenSlotInArray] = new InteractableObject_Collectable_canDrop(objectPrefab, objectID, objectOrder);
                break;
                
            default: //case Constants.interactableID_portTownPier:
                objectArray[firstOpenSlotInArray] = new InteractableObject_PortTownPier(objectPrefab, objectID, objectOrder);
                break;
        }

        firstOpenSlotInArray++;
    }


    private bool checkIfObjectIsCloseEnough(Vector3 boatPos, int indexOfObject)
    {
        return objectArray[indexOfObject].checkIfCloseEnoughToInteractWith(boatPos);
    }


    private void expandArray()
    {
        InteractableObject_Abstract[] newArray = new InteractableObject_Abstract[objectArray.Length * 2];

        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            newArray[index] = objectArray[index];
        }

        objectArray = newArray;
    }


    private void removeObjectFromArray(int indexOfObject)
    {
        for (int index=indexOfObject; index<firstOpenSlotInArray-1; index++)
        {
            objectArray[index] = objectArray[index+1];
        }

        firstOpenSlotInArray--;
    }
}
