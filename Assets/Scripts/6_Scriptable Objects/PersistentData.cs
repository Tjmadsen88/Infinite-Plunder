using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

using ConstantsSpace;

using TerrainBuilderSpace;
using MinimapBaseColorMakerSpace;

[CreateAssetMenu(fileName = "New PersistentData", menuName = "Persistent Data", order = 1)]
public class PersistentData : ScriptableObject
{
    // Customization stuff:
    private CustomizationPacket customizationPacket;

    // Mid-Game Settings stuff:
    private MidGameSettingsPacket midGameSettingsPacket;

    // Stat stuff:
    private int moneyCollected;
    private int moneyLost;
    // private int finalTreasure;
    private bool finalTreasure;
    // private int moneyTotal;

    private short foesSunk;
    private short shipsLost;

    private short area_explored;
    private short area_total;
    private byte keys_found;
    private byte keys_total;
    private short doors_found;
    private short doors_total;

    private bool playerHasLost = false;

    private Color32[] terrainColors;
    private bool hasTerrainColors = false;

    private Color32[] shipColors;
    private byte sailPatternSelection;
    private bool sailIsMirrored_horizontal;
    private bool sailIsMirrored_vertical;
    private byte cannonSelection;
    private bool hasShipData = false;

    private byte cannonSelection_previousRandom1 = 200;
    private byte cannonSelection_previousRandom2 = 200;

    TerrainBuilderReturnPacket terrainPacket;
    Color32[] minimapColorArray;
    bool hasLoadingData = false;



    public void resetAllValues()
    {
        loadCustomizationData();
        loadMidGameSettingsData();
        eraseLoadingData();

        moneyCollected = 0;
        moneyLost = 0;
        // finalTreasure = 0;
        finalTreasure = false;
        // moneyTotal = 0;

        foesSunk = 0;
        shipsLost = 0;

        area_explored = 0;
        area_total = 0;
        keys_found = 0;
        keys_total = 0;
        doors_found = 0;
        doors_total = 0;

        playerHasLost = false;
    }


    // ---------------------------------------------------------------------------------------------------
    // ------------ Customization stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    public void setAndSaveCustomizationData(byte money_selection, byte enemies_selection, byte keys_selection, byte area_selection,
                                            int money_value, float enemies_value, byte keys_value, byte area_value,
                                            int money_custom, float enemies_custom, byte keys_custom, byte area_custom)
    {
        customizationPacket.setMoney_selection(money_selection);
        customizationPacket.setEnemies_selection(enemies_selection);
        customizationPacket.setKeys_selection(keys_selection);
        customizationPacket.setArea_selection(area_selection);

        customizationPacket.setMoney_value(money_value);
        customizationPacket.setEnemies_value(enemies_value);
        customizationPacket.setKeys_value(keys_value);
        customizationPacket.setArea_value(area_value);

        customizationPacket.setMoney_custom(money_custom);
        customizationPacket.setEnemies_custom(enemies_custom);
        customizationPacket.setKeys_custom(keys_custom);
        customizationPacket.setArea_custom(area_custom);

        saveData_cust(customizationPacket, Constants.fileName_customization);
    }

    public void setCustomizationData_Demo()
    {
        customizationPacket.setMoney_value(150000);
        customizationPacket.setEnemies_value(0);
        // customizationPacket.setKeys_value(1);
        // customizationPacket.setArea_value(0);
    }

    private void loadCustomizationData()
    {
        customizationPacket = loadData_cust(Constants.fileName_customization);
    }
    
    public void chooseValuesForRandomizedCustomizationData()
    {
        // byte tempSelection;

        if (customizationPacket.getMoney_selection() == 0)
        {
            // tempSelection = (byte)UnityEngine.Random.Range(1, 5);
            // customizationPacket.setMoney_value(Constants.getCustomization_StartingMoney(tempSelection));
            customizationPacket.setMoney_value(UnityEngine.Random.Range(500, 3251) * 100);
        }

        if (customizationPacket.getEnemies_selection() == 0)
        {
            // tempSelection = (byte)UnityEngine.Random.Range(1, 5);
            // customizationPacket.setEnemies_value(Constants.getCustomization_DensityOfEnemeies(tempSelection));
            customizationPacket.setEnemies_value(UnityEngine.Random.Range(0f, 2f));
        }

        if (customizationPacket.getArea_selection() == 0)
        {
            // tempSelection = (byte)UnityEngine.Random.Range(1, 5);
            // customizationPacket.setArea_value(Constants.getCustomization_SizeOfArea(tempSelection));
            customizationPacket.setArea_value((byte)UnityEngine.Random.Range(0, 4));
        }

        // Key settings:
        switch(customizationPacket.getKeys_selection())
        {
            case 0: // Random:
                customizationPacket.setKeys_value((byte)UnityEngine.Random.Range(0, 7));
                break;

            case 2: // Lower:
                customizationPacket.setKeys_value((byte)UnityEngine.Random.Range(1, 4));
                break;

            case 3: // Higher:
                customizationPacket.setKeys_value((byte)UnityEngine.Random.Range(3, 7));
                break;
        }
    }

    public byte getStartingMoney_selection()
    {
        return customizationPacket.getMoney_selection();
    }

    public byte getDensityOfEnemies_selection()
    {
        return customizationPacket.getEnemies_selection();
    }

    public byte getNumberOfKeys_selection()
    {
        return customizationPacket.getKeys_selection();
    }

    public byte getSizeOfExplorableArea_selection()
    {
        return customizationPacket.getArea_selection();
    }

    
    public int getStartingMoney_value()
    {
        return customizationPacket.getMoney_value();
    }

    public float getDensityOfEnemies_value()
    {
        return customizationPacket.getEnemies_value();
    }

    public byte getNumberOfKeys_value()
    {
        return customizationPacket.getKeys_value();
    }

    public byte getSizeOfExplorableArea_value()
    {
        return customizationPacket.getArea_value();
    }
    
    
    public int getStartingMoney_custom()
    {
        return customizationPacket.getMoney_custom();
    }

    public float getDensityOfEnemies_custom()
    {
        return customizationPacket.getEnemies_custom();
    }

    public byte getNumberOfKeys_custom()
    {
        return customizationPacket.getKeys_custom();
    }

    public byte getSizeOfExplorableArea_custom()
    {
        return customizationPacket.getArea_custom();
    }




    // ---------------------------------------------------------------------------------------------------
    // ------------ Mid-Game Settings stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    public void saveMidGameSettingsData()
    {
        saveData_set(midGameSettingsPacket, Constants.fileName_midGamSettings);
    }

    private void loadMidGameSettingsData()
    {
        midGameSettingsPacket = loadData_set(Constants.fileName_midGamSettings);
    }


    public void setNormalStickPositions(bool normalStickPositions)
    {
        midGameSettingsPacket.setNormalStickPositions(normalStickPositions);
    }

    public void setShootingMode(byte shootingMode)
    {
        midGameSettingsPacket.setShootingMode(shootingMode);
    }

    public bool getNormalStickPositions()
    {
        return midGameSettingsPacket.getNormalStickPositions();
    }

    public byte getShootingMode()
    {
        return midGameSettingsPacket.getShootingMode();
    }


    // ---------------------------------------------------------------------------------------------------
    // ------------ Recorded stat stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    // Money stuff:
    // Not sure how to handle this just yet.
    public void earnedMoney(int amount)
    {
        moneyCollected += amount;
    }

    public void lostMoney(int amount)
    {
        moneyLost += amount;
    }

    public void soldFinalTreasure()
    {
        // finalTreasure = Constants.getValueOfFinalTreasure();
        finalTreasure = true;
    }

    public int getMoneyCollected()
    {
        return moneyCollected;
    }

    public int getMoneyLost()
    {
        return moneyLost;
    }

    public int getFinalTreasure()
    {
        // return finalTreasure;
        if (finalTreasure)
            return keys_found * 17500 + area_explored * 5000 + doors_found * 2500;

        return 0;
    }


    // Combat stuff:
    public void sunkAFoe()
    {
        foesSunk++;
    }

    public void lostAShip()
    {
        shipsLost++;
    }

    public short getFoesSunk()
    {
        return foesSunk;
    }

    public short getShipsLost()
    {
        return shipsLost;
    }

    // Exploration stuff:
    public void exploredANewArea()
    {
        area_explored++;
    }

    public void setAreaTotal(short area_total)
    {
        this.area_total = area_total;
    }

    public void foundAKey()
    {
        keys_found++;
    }

    public void setKeysTotal(byte keys_total)
    {
        this.keys_total = keys_total;
    }

    public void openedADoor()
    {
        doors_found++;
    }

    public void setDoorsTotal(short doors_total)
    {
        this.doors_total = doors_total;
    }

    public short getAreaExplored()
    {
        return area_explored;
    }

    public short getAreaTotal()
    {
        return area_total;
    }

    public byte getKeysFound()
    {
        return keys_found;
    }

    public byte getKeysTotal()
    {
        return keys_total;
    }

    public short getDoorsFound()
    {
        return doors_found;
    }

    public short getDoorsTotal()
    {
        return doors_total;
    }

    public void thePlayerHasLost()
    {
        playerHasLost = true;
    }

    public bool getPlayerHasLost()
    {
        return playerHasLost;
    }


    // ---------------------------------------------------------------------------------------------------
    // ------------ Terrain color stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    public Color32[] getTerrainColors()
    {
        if (hasTerrainColors) return terrainColors;

        return getTerrainColors_default();
    }

    private Color32[] getTerrainColors_default()
    {
        terrainColors = new Color32[4];
        terrainColors[0] = new Color32(101, 165, 164, 255); // Water
        terrainColors[1] = new Color32(172, 176, 145, 255); // Sand
        terrainColors[2] = new Color32(64, 103, 60, 255);   // Grass 1
        terrainColors[3] = new Color32(44, 83, 40, 255);    // Grass 2
        
        return terrainColors;
    }

    public void setTerrainColors(Color32[] terrainColors)
    {
        this.terrainColors = terrainColors;
        hasTerrainColors = true;
    }

    public void setTerrainColors_default()
    {
        this.terrainColors = getTerrainColors_default();
        hasTerrainColors = true;
    }

    public void removeTerrainColorData()
    {
        hasTerrainColors = false;
    }


    // ---------------------------------------------------------------------------------------------------
    // ------------ Ship customization stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------


    public Color32[] getShipColors()
    {
        if (hasShipData) return shipColors;

        return getShipColors_default();
    }

    public Color32[] getShipColors_default()
    {
        shipColors = new Color32[6];
        shipColors[0] = Constants.defaultColor_SailPrimary;
        shipColors[1] = Constants.defaultColor_SailPattern;
        shipColors[2] = Constants.defaultColor_MastAndDeck;
        shipColors[3] = Constants.defaultColor_Rails;
        shipColors[4] = Constants.defaultColor_Hull;
        shipColors[5] = Constants.defaultColor_Cannonball;
        
        return shipColors;
    }

    public byte getSailPatternSelection()
    {
        return sailPatternSelection;
    }

    public bool getSailIsMirrored_horizontal()
    {
        return sailIsMirrored_horizontal;
    }

    public bool getSailIsMirrored_vertical()
    {
        return sailIsMirrored_vertical;
    }

    public byte getCannonSelection()
    {
        return cannonSelection;
    }

    public void setShipData(Color32[] shipColors, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical, byte cannonSelection)
    {
        this.shipColors = shipColors;
        this.sailPatternSelection = sailPatternSelection;
        this.sailIsMirrored_horizontal = sailIsMirrored_horizontal;
        this.sailIsMirrored_vertical = sailIsMirrored_vertical;
        this.cannonSelection = cannonSelection;
        hasShipData = true;
    }

    public void setShipData_default()
    {
        shipColors = getShipColors_default();
        sailPatternSelection = Constants.defaultSelection_SailPattern;
        sailIsMirrored_horizontal = Constants.defaultSelection_SailIsMirrored_horizontal;
        sailIsMirrored_vertical = Constants.defaultSelection_SailIsMirrored_vertical;
        cannonSelection = Constants.defaultSelection_cannon;
        hasShipData = true;
    }

    public void removeShipData()
    {
        hasShipData = false;
    }


    public void setPreviousCannonSelection(byte cannonSelection)
    {
        cannonSelection_previousRandom2 = cannonSelection_previousRandom1;
        cannonSelection_previousRandom1 = cannonSelection;
    }

    public byte getPreviousCannonSelection1()
    {
        return cannonSelection_previousRandom1;
    }

    public byte getPreviousCannonSelection2()
    {
        return cannonSelection_previousRandom2;
    }

    // ---------------------------------------------------------------------------------------------------
    // ------------ Save and Load stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    private void saveData_cust(CustomizationPacket packet, string fileName)
    {
        FileStream file = null;

        try {
            BinaryFormatter formatter = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + fileName);

            formatter.Serialize(file, packet);

        } catch (Exception e) {
            Debug.Log("There was an exception...? Saving Cust");

        } finally {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    private void saveData_set(MidGameSettingsPacket packet, string fileName)
    {
        FileStream file = null;

        try {
            BinaryFormatter formatter = new BinaryFormatter();

            file = File.Create(Application.persistentDataPath + fileName);

            formatter.Serialize(file, packet);

        } catch (Exception e) {
            Debug.Log("There was an exception...? Saving Set");

        } finally {
            if (file != null)
            {
                file.Close();
            }
        }
    }


    private CustomizationPacket loadData_cust(string fileName)
    {
        FileStream file = null;
        CustomizationPacket returnPacket = null;

        try {
            BinaryFormatter formatter = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

            returnPacket = formatter.Deserialize(file) as CustomizationPacket;
        } catch (Exception e)
        {
            Debug.Log("There was an exception...? Loading Cust");
            returnPacket = new CustomizationPacket();
        } finally {
            if (file != null)
            {
                file.Close();
            }
        }

        //Debug.Log("Path is: " + (Application.persistentDataPath + fileName));

        return returnPacket;
    }

    private MidGameSettingsPacket loadData_set(string fileName)
    {
        FileStream file = null;
        MidGameSettingsPacket returnPacket = null;

        try {
            BinaryFormatter formatter = new BinaryFormatter();

            file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

            returnPacket = formatter.Deserialize(file) as MidGameSettingsPacket;
        } catch (Exception e)
        {
            Debug.Log("There was an exception...? Loading Set");
            returnPacket = new MidGameSettingsPacket();
        } finally {
            if (file != null)
            {
                file.Close();
            }
        }

        Debug.Log("Path is: " + (Application.persistentDataPath + fileName));

        return returnPacket;
    }


    // ---------------------------------------------------------------------------------------------------
    // ------------ Loading Screen stuff ----------------------------------------------------------------
    // ---------------------------------------------------------------------------------------------------

    private static void ThreadFunction(object parentClass) {

        PersistentData dataClass = ((PersistentData)parentClass);

        TerrainBuilderReturnPacket terrainPacket_thread = TerrainBuilder.generateTerrain(Constants.roomWidthHeight, Constants.numOfVertsPerEdge, dataClass.getSizeOfExplorableArea_value(), dataClass.getNumberOfKeys_value());

        bool[,] boolArray_noNoise = terrainPacket_thread.getBoolArray_noNoise();
        Color32[] minimapColorArray = MinimapBaseColorMaker.convertBoolArrayToMinimapColorArray(boolArray_noNoise, boolArray_noNoise.GetLength(0), boolArray_noNoise.GetLength(1));

        dataClass.setLoadingData(terrainPacket_thread, minimapColorArray);
    }

    private static void ThreadFunction_Demo(object parentClass) {

        PersistentData dataClass = ((PersistentData)parentClass);

        TerrainBuilderReturnPacket terrainPacket_thread = TerrainBuilder.generateTerrain_DemoVer(Constants.roomWidthHeight, Constants.numOfVertsPerEdge);

        bool[,] boolArray_noNoise = terrainPacket_thread.getBoolArray_noNoise();
        Color32[] minimapColorArray = MinimapBaseColorMaker.convertBoolArrayToMinimapColorArray(boolArray_noNoise, boolArray_noNoise.GetLength(0), boolArray_noNoise.GetLength(1));

        dataClass.setLoadingData(terrainPacket_thread, minimapColorArray);
    }


    public void createLoadingDataWithThread()
    {
        Thread theThread = new Thread(new ParameterizedThreadStart(ThreadFunction));
        theThread.Start(this);
    }

    public void createLoadingDataWithThread_Demo()
    {
        Thread theThread = new Thread(new ParameterizedThreadStart(ThreadFunction_Demo));
        theThread.Start(this);
    }

    public void setLoadingData(TerrainBuilderReturnPacket terrainPacket, Color32[] minimapColorArray)
    {
        this.terrainPacket = terrainPacket;
        this.minimapColorArray = minimapColorArray;
        hasLoadingData = true;
    }

    public void eraseLoadingData()
    {
        hasLoadingData = false;
        terrainPacket = null;
        minimapColorArray = null;
    }



    public TerrainBuilderReturnPacket getTerrainPacket()
    {
        return terrainPacket;
    }

    public Color32[] getMinimapColorArray()
    {
        return minimapColorArray;
    }

    public bool getHasLoadingData()
    {
        return hasLoadingData;
    }
}
