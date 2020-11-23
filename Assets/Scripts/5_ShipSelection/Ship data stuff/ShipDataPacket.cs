using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using ConstantsSpace;

using ShipDataSpace;

[Serializable]
public class ShipDataPacket
{
    private ShipDataIndividual[] shipData;
    private int numOfSavedShips = 0;

    private int previouslySelectedShip = 0;


    public ShipDataIndividual[] getShipData()
    {
        if (numOfSavedShips == 0) addNewShip_default();

        return shipData;
    }

    public ShipDataIndividual getShipData_oneRandom()
    {
        if (numOfSavedShips == 0) addNewShip_default();

        return shipData[UnityEngine.Random.Range(0, numOfSavedShips)];
    }

    public int getNumOfSavedShips()
    {
        return numOfSavedShips;
    }

    public int getPreviouslySelectedShip()
    {
        return previouslySelectedShip;
    }


    public void recordSelectedShip(int selectedShip)
    {
        previouslySelectedShip = selectedShip;
    }


    public void addNewShip(ShipDataIndividual newShipData)
    {
        ShipDataIndividual[] largerShipArray = new ShipDataIndividual[numOfSavedShips+1];

        for (int index=0; index<numOfSavedShips; index++)
        {
            largerShipArray[index] = shipData[index];
        }

        largerShipArray[numOfSavedShips] = newShipData;

        shipData = largerShipArray;
        numOfSavedShips++;
    }


    public void updateShip(ShipDataIndividual newShipData, int shipIndex)
    {
        shipData[shipIndex] = newShipData;
    }


    public void removeShip(int shipToRemove)
    {
        ShipDataIndividual[] smallerShipArray = new ShipDataIndividual[numOfSavedShips-1];

        for (int index=0; index<shipToRemove; index++)
        {
            smallerShipArray[index] = shipData[index];
        }

        for (int index=shipToRemove+1; index<numOfSavedShips; index++)
        {
            smallerShipArray[index-1] = shipData[index];
        }

        shipData = smallerShipArray;
        numOfSavedShips--;

        if (shipToRemove == previouslySelectedShip)
            previouslySelectedShip = 0;
    }


    public void swapShipData(int selectedShip, int newSlot)
    {
        ShipDataIndividual tempShipData = shipData[newSlot];
        
        shipData[newSlot] = shipData[selectedShip];
        shipData[selectedShip] = tempShipData;

        if (previouslySelectedShip == selectedShip)
            previouslySelectedShip = newSlot;
        else if (previouslySelectedShip == newSlot)
            previouslySelectedShip = selectedShip;
    }



    private void addNewShip_default()
    {
        ShipDataIndividual defaultShip = new ShipDataIndividual();

        defaultShip.setShipName(Constants.defaultShipName);

        defaultShip.setSailPatternSelection(Constants.defaultSelection_SailPattern);
        defaultShip.setColor_SailPrimary(Constants.defaultColor_SailPrimary);
        defaultShip.setColor_SailPattern(Constants.defaultColor_SailPattern);

        defaultShip.setColor_MastAndDeck(Constants.defaultColor_MastAndDeck);
        defaultShip.setColor_Rails(Constants.defaultColor_Rails);
        defaultShip.setColor_Hull(Constants.defaultColor_Hull);

        defaultShip.setCannonSelection(Constants.defaultSelection_cannon);
        defaultShip.setColor_Cannonball(Constants.defaultColor_Cannonball);

        addNewShip(defaultShip);
    }



    
}
