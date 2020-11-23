using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using LootTailSpace;

public class LootTailManager : MonoBehaviour
{
    public GameView gameView;

    private LootPieceIndividual[] lootTailArray = new LootPieceIndividual[16];
    private GameObject[] lootTailArray_chains = new GameObject[16];
    private GameObject[] lootTailArray_selling = new GameObject[16];
    private int firstOpenSlotInArray = 0;

    private Vector3 previousShipPosition = Vector3.zero;
    private const float moveMinimum = 0.2f;
    // private const float moveMinimum = Constants.shipMoveSpeed;

    private Vector3 sellingPortLocation;
    private int numOfLootBeingSold_total = 0;
    private int numOfLootBeingSold_stillMoving = 0;
    private bool soldLootIsMoving = false;
    private const float sellingMoveSpeed_initial = 0.3f;
    private const float sellingMoveSpeed_inc = 0.0075f;
    private float sellingMoveSpeed_current;

    

    public void updateTailPosition(Vector3 shipDelayPosition, Vector3 shipDelayEulerAngles)
    {
        if (firstOpenSlotInArray > 0 && (shipDelayPosition-previousShipPosition).magnitude >= moveMinimum)
        {
            previousShipPosition = shipDelayPosition;
            lootTailArray[0].updateLootPosition(shipDelayPosition, shipDelayEulerAngles);
            updateChainPosition(lootTailArray_chains[0], lootTailArray[0].getCurrentPosition(), shipDelayPosition);

            for (int index=1; index<firstOpenSlotInArray; index++)
            {
                lootTailArray[index].updateLootPosition(lootTailArray[index-1].getCurrentPosition(), lootTailArray[index-1].getCurrentEulerAngles());
                updateChainPosition(lootTailArray_chains[index], lootTailArray[index].getCurrentPosition(), lootTailArray[index-1].getCurrentPosition());
            }
        }

        moveAllSoldLoot();
    }


    private void updateChainPosition(GameObject chain, Vector3 lootPosition, Vector3 aheadPosition)
    {
        chain.transform.position = lootPosition;
        chain.transform.LookAt(aheadPosition);
        chain.transform.localScale = new Vector3(1f, 1f, (aheadPosition - lootPosition).magnitude);
    }


    private void moveAllSoldLoot()
    {
        if (soldLootIsMoving)
        {
            sellingMoveSpeed_current += sellingMoveSpeed_inc;

            for (int index=0; index<numOfLootBeingSold_total; index++)
            {
                if (lootTailArray_selling[index].activeSelf)
                {
                    lootTailArray_selling[index].transform.position = Vector3.MoveTowards(lootTailArray_selling[index].transform.position, sellingPortLocation, sellingMoveSpeed_current);
                    if (lootTailArray_selling[index].transform.position == sellingPortLocation)
                    {
                        // Debug.Log("Deactivating a loot piece: "+index+" "+numOfLootBeingSold_stillMoving);
                        lootTailArray_selling[index].SetActive(false);
                        numOfLootBeingSold_stillMoving--;

                        if (numOfLootBeingSold_stillMoving == 0)
                        {
                            destroyAllSoldLoot();
                            soldLootIsMoving = false;
                        }
                    }
                }
            }
        }
    }


    public void addLootToEndOfTail(GameObject lootObject, GameObject newChain, byte lootID, Vector3 BoatPos)
    {
        if (firstOpenSlotInArray >= lootTailArray.Length) expandArray();

        if (firstOpenSlotInArray != 0)
        {
            // Debug.Log("Loot tail has items: "+firstOpenSlotInArray);
            lootTailArray[firstOpenSlotInArray] = new LootPieceIndividual(lootObject, lootID, 
                                                                        lootTailArray[firstOpenSlotInArray-1].getLootID(),
                                                                        lootTailArray[firstOpenSlotInArray-1].getCurrentPosition(),
                                                                        lootTailArray[firstOpenSlotInArray-1].getCurrentEulerAngles());
        } else {
            // Debug.Log("Loot tail is empty: "+firstOpenSlotInArray);

            GameObject temp_obj = new GameObject("invisiLoot");
            temp_obj.SetActive(false);
                                                                        
            lootTailArray[0] = new LootPieceIndividual(temp_obj, Constants.interactableID_none, 
                                                        Constants.interactableID_none,
                                                        BoatPos,
                                                        temp_obj.transform.eulerAngles);
                                                                        
            lootTailArray[1] = new LootPieceIndividual(lootObject, lootID, 
                                                        Constants.interactableID_treasureSmall,
                                                        BoatPos,
                                                        lootObject.transform.eulerAngles);

            temp_obj = Instantiate(newChain, Vector3.zero, Quaternion.identity);
            lootTailArray_chains[0] = temp_obj;
            lootTailArray_chains[0].transform.localScale = Vector3.zero;

            firstOpenSlotInArray = 1;
        }

        lootTailArray_chains[firstOpenSlotInArray] = newChain;
        lootTailArray_chains[firstOpenSlotInArray].transform.localScale = Vector3.zero;
        
        firstOpenSlotInArray++;
    }


    private void expandArray()
    {
        LootPieceIndividual[] newArray = new LootPieceIndividual[lootTailArray.Length * 2];
        GameObject[] newArray_chains = new GameObject[lootTailArray.Length * 2];
        GameObject[] newArray_selling = new GameObject[lootTailArray.Length * 2];

        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            newArray[index] = lootTailArray[index];
            newArray_chains[index] = lootTailArray_chains[index];
            newArray_selling[index] = lootTailArray_selling[index];
        }

        lootTailArray = newArray;
        lootTailArray_chains = newArray_chains;
        lootTailArray_selling = newArray_selling;
    }


    public int sellAllLoot(Vector3 portTownlocation)
    {
        sellingPortLocation = portTownlocation;
        int totalMoneyEarned = 0;

        if (soldLootIsMoving) destroyAllSoldLoot();

        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            if (lootTailArray[index].getLootID() == Constants.interactableID_treasureFinal)
                gameView.soldFinalTreasure();

            totalMoneyEarned += getMoneyTotalFromLootID(lootTailArray[index].getLootID());
            // Destroy(lootTailArray[index].getLootPrefab());
            lootTailArray_selling[index] = lootTailArray[index].getLootPrefab();
            Destroy(lootTailArray_chains[index]);
        }

        numOfLootBeingSold_total = firstOpenSlotInArray;
        numOfLootBeingSold_stillMoving = numOfLootBeingSold_total-1;
        sellingMoveSpeed_current = sellingMoveSpeed_initial;
        soldLootIsMoving = true;

        firstOpenSlotInArray = 0;

        return totalMoneyEarned;
    }


    // public void destroyAllLoot(Vector3 sinkingPosition)
    public void destroyAllLoot()
    {
        for (int index=0; index<firstOpenSlotInArray; index++)
        {
            if (lootTailArray[index].getLootID() != Constants.interactableID_treasureFinal)
            {
                Destroy(lootTailArray[index].getLootPrefab());
            } else {
                gameView.droppedFinalTreasure(lootTailArray[index].getLootPrefab());
            }

            Destroy(lootTailArray_chains[index]);

            // switch (lootTailArray[index].getLootID())
            // {
            //     case Constants.interactableID_treasureFinal:
            //         gameView.droppedFinalTreasure(lootTailArray[index].getLootPrefab());
            //         break;

            //     case Constants.interactableID_treasureSmall:
            //     case Constants.interactableID_treasureLarge:
            //         gameView.droppedOtherTreasure(lootTailArray[index].getLootPrefab(), lootTailArray[index].getLootID());
            //         break;

            //     case Constants.interactableID_treasureShip:
            //         gameView.droppedShipTreasure(lootTailArray[index].getLootPrefab().transform.position);
            //         Destroy(lootTailArray[index].getLootPrefab());
            //         break;

            //     default:
            //         Destroy(lootTailArray[index].getLootPrefab());
            //         break;
            // }
        }

        firstOpenSlotInArray = 0;
    }


    public void destroyAllSoldLoot()
    {
        // Debug.Log("Destroying all loot...");
        
        for (int index=0; index<numOfLootBeingSold_total; index++)
        {
            Destroy(lootTailArray_selling[index]);
        }

        numOfLootBeingSold_total = 0;
    }


    private int getMoneyTotalFromLootID(byte lootID)
    {
        switch(lootID)
        {
            case Constants.interactableID_treasureSmall:
                return Constants.lootValue_small;
            
            case Constants.interactableID_treasureLarge:
                return Constants.lootValue_large;
            
            case Constants.interactableID_treasureCannon:
                return Constants.lootValue_cannon;
            
            case Constants.interactableID_treasureShip:
                return Constants.lootValue_ship;

            default:
                return 0;
        }
    }

    public bool getTailHasLoot()
    {
        return firstOpenSlotInArray > 0;
    }

}
