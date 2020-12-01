using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using System; //temp?

using ConstantsSpace;

using TouchManagerSpace;
using TerrainBuilderSpace;
using TerrainColorGeneratorSpace;
using BoolArrayManagerSpace;
using DoorHitboxManagerSpace;
using PortTownSpace;

public class GameView : MonoBehaviour
{
    public GameObject playerBoat_target;
    private Transform playerBoatTransform_target;
    public GameObject playerBoat_delayed;
    private Transform playerBoatTransform_delayed;
    public GameObject playerBoat_actual; // The transform for this is handled in the rotationManager.
    public Renderer playerBoat_actual_renderer_main;
    public Renderer playerBoat_actual_renderer_sail;
    public Material terrainMaterial;

    private Camera mainCam;

    public CameraManager cameraManager;
    public TouchManager touchManager;

    private byte gameState = Constants.gameState_canMove;

    private bool[,] boolArray;

    private bool[] collectedKeys = new bool[6];
    public InteractablesManager interactablesManager;
    private DoorHitboxManager doorHitboxManager;

    public MidGameSettingsManager midGameSettingsManager;
    
    private bool settingsMenuIsOpen = false;
    private bool fairyIsVisible = false;
    private bool shipIsSinking_1 = false;
    private bool shipIsSinking_2 = false;
    private bool shipIsSinking_3 = false;
    private bool gameIsOver = false;

    public UIManager_GameView uiManager;

    public PersistentData persistentData;
    public WaterManager waterManager;

    private int currentMoney;

    public ShipRotationManager shipRotationmanager;
    public WakeController_Moving wakeController;

    public EnemyManager enemyManager;
    byte numOfRooms_horizontal;
    byte numOfRooms_vertical;

    private bool isVulnerable = true;
    private byte invulnerabilityDuration_max = 95;
    private byte invulnerabilityDuration_current = 0;
    
    public MinimapManager minimapManager;
    public CannonballManager cannonballManager;
    public LootTailManager lootTailManager;
    public HealthBarManager healthBarManager;
    public LootPrefabManager lootPrefabManager;
    public ShipTextureManager shipTextureManager;

    private Vector3 bookmarkedPortTownLocation = Vector3.zero;
    private Vector3 bookmarkedPortTownRotation = new Vector3(0f, -45f, 0f);

    private const byte shipHealth_max = 3;
    private byte shipHealth_current = shipHealth_max;

    private byte endGameCountdownTimer = 31;

    private const byte sinkingTimer_1_max = 30;
    private const byte sinkingTimer_2_max = 75;
    private const byte sinkingTimer_3_max = 30;
    private byte sinkingTimer_1_current;
    private byte sinkingTimer_2_current;
    private byte sinkingTimer_3_current;


    private Vector3 knockbackDirection_normalized;
    private Vector3 knockbackDirection_current;

    private byte knockbackDuration_current = 0;
    private bool isBeingKnockedBack = false;
    

    public GameObject prefab_key1;
    public GameObject prefab_key2;
    public GameObject prefab_key3;
    public GameObject prefab_key4;
    public GameObject prefab_key5;
    public GameObject prefab_key6;

    public GameObject prefab_door1;
    public GameObject prefab_door2;
    public GameObject prefab_door3;
    public GameObject prefab_door4;
    public GameObject prefab_door5;
    public GameObject prefab_door6;
    
    // public GameObject prefab_shipTreasure;
    public GameObject prefab_treasureFinal;

    public GameObject prefab_portTownPier;
    public GameObject prefab_portTownHouse;


    // Start is called before the first frame update
    void Start()
    {
        // Make the game run at 60fps:
        Application.targetFrameRate = 60;
        Input.simulateMouseWithTouches = false;

        mainCam = Camera.main;
        currentMoney = persistentData.getStartingMoney_value();
        uiManager.setMoneyAndLivesText_initial(currentMoney);

        playerBoatTransform_target = playerBoat_target.GetComponent<Transform>();
        playerBoatTransform_delayed = playerBoat_delayed.GetComponent<Transform>();

        cameraManager.setInitialCameraSettings(6, 6);
        midGameSettingsManager.setInitialButtonValues(persistentData.getNormalStickPositions(), persistentData.getShootingMode(), 6, 6);

        shipTextureManager.initializeShipMaterials(persistentData.getShipColors(), persistentData.getSailPatternSelection(), 
                                                    persistentData.getSailIsMirrored_horizontal(), persistentData.getSailIsMirrored_vertical());

        generateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case Constants.gameState_everythingPaused:
                break;

            case Constants.gameState_movementPaused:
                playHalfPausedAnimations();

                tellTheOtherClasses_TheGameIsHalfPaused();
                break;

            // Player can move:
            default:
                checkVulnerability();

                tellTheOtherClasses_ThePlayerCanMove();
                break;
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Interactables stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void collectedKey(byte keyID, short keyOrder)
    {
        doorHitboxManager.openAllDoorsByColor(keyID);
        uiManager.collectedAKey(keyID);
        persistentData.foundAKey();
        minimapManager.collectedKey(keyOrder);

        switch(keyID)
        {
            case Constants.interactableID_key1:
                collectedKeys[0] = true;
                break;
                
            case Constants.interactableID_key2:
                collectedKeys[1] = true;
                break;
                
            case Constants.interactableID_key3:
                collectedKeys[2] = true;
                break;
                
            case Constants.interactableID_key4:
                collectedKeys[3] = true;
                break;
                
            case Constants.interactableID_key5:
                collectedKeys[4] = true;
                break;
                
            case Constants.interactableID_key6:
                collectedKeys[5] = true;
                break;

            default:
                break;
        }
    }

    public void openedDoor(short doorIndex)
    {
        persistentData.openedADoor();
        minimapManager.openedDoor(doorIndex);
    }

    public bool doIHaveTheKey(byte doorID)
    {
        switch(doorID)
        {
            case Constants.interactableID_door1:
                return collectedKeys[0];
                
            case Constants.interactableID_door2:
                return collectedKeys[1];
                
            case Constants.interactableID_door3:
                return collectedKeys[2];
                
            case Constants.interactableID_door4:
                return collectedKeys[3];
                
            case Constants.interactableID_door5:
                return collectedKeys[4];
                
            case Constants.interactableID_door6:
                return collectedKeys[5];

            default: return false;
        }
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Terrain generation and initialization stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void generateTerrain()
    {
        // TerrainBuilderReturnPacket terrainPacket = TerrainBuilder.generateTerrain(Constants.roomWidthHeight, Constants.numOfVertsPerEdge);
        TerrainBuilderReturnPacket terrainPacket = persistentData.getTerrainPacket();
        Mesh[,] terrainMeshes = terrainPacket.getTerrainMeshes();
        // Mesh[,] terrainMeshes = TerrainBuilder.sliceHeightArrayIntoMultipleMeshes(terrainPacket.getHeightArray(), Constants.roomWidthHeight, Constants.numOfVertsPerEdge);

        boolArray = terrainPacket.getBoolArray();
        persistentData.setAreaTotal(terrainPacket.getNumOfRooms());
        persistentData.setKeysTotal(terrainPacket.getNumOfKeys());
        persistentData.setDoorsTotal(terrainPacket.getNumOfLockedDoors());

        numOfRooms_horizontal = (byte)terrainMeshes.GetLength(0);
        numOfRooms_vertical = (byte)terrainMeshes.GetLength(1);

        waterManager.setWaterSize(numOfRooms_horizontal, numOfRooms_vertical, Constants.roomWidthHeight);

        // color the pixels in the terrainMaterial?
        // Color32[] terrainColors = TerrainColorGenerator.GenerateTerrainColors();
        Color32[] terrainColors = persistentData.getTerrainColors();

        Texture2D terrainTexture = new Texture2D(4, 1);
        terrainTexture.wrapMode = TextureWrapMode.Clamp;
        terrainTexture.filterMode = FilterMode.Point;

        terrainMaterial.mainTexture = terrainTexture;

        //terrainTexture.SetPixel(0, 0, terrainColors[3]);
        terrainTexture.SetPixel(0, 0, terrainColors[0]);
        terrainTexture.SetPixel(1, 0, terrainColors[1]);
        terrainTexture.SetPixel(2, 0, terrainColors[2]);
        terrainTexture.SetPixel(3, 0, terrainColors[3]);
        terrainTexture.Apply();

        //waterManager.setWaterTint(terrainColors[3]);
        waterManager.setWaterTint(terrainColors[0]);

        Debug.Log("Water color is: "+(terrainColors[0]));

        persistentData.setTerrainColors(terrainColors);

        // Instantiate the 3D terrain meshes:
        GameObject newTerrainParent = new GameObject("terrainParent");

        MeshFilter terrainMeshFilter;
        MeshRenderer terrainMeshRenderer;

        for (int indexX=0; indexX<numOfRooms_horizontal; indexX++)
        {
            for (int indexY=0; indexY<numOfRooms_vertical; indexY++)
            {
                GameObject newTerrain = new GameObject("terrainPiece");

                terrainMeshFilter = newTerrain.AddComponent<MeshFilter>();
                terrainMeshFilter.mesh = terrainMeshes[indexX, indexY];

                terrainMeshRenderer = newTerrain.AddComponent<MeshRenderer>();
                terrainMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                terrainMeshRenderer.material = terrainMaterial;

                newTerrain.transform.position = new Vector3(((indexX * Constants.roomWidthHeight) - 2f*Constants.roomWidthHeight), -0.5f, 
                                                                            ((-indexY * Constants.roomWidthHeight) + 2f*Constants.roomWidthHeight));
                newTerrain.transform.parent = newTerrainParent.transform;
            }
        }

        // Move the player's boat to the starting location:
        playerBoatTransform_target.position = new Vector3((((float)terrainPacket.getPlayerStartingLocation()[0]) + 0.5f) * Constants.roomWidthHeight, 
                                                    0f, (((float)terrainPacket.getPlayerStartingLocation()[1]) + 0.5f) * -Constants.roomWidthHeight);
        playerBoatTransform_delayed.position = playerBoatTransform_target.position;
        // And bookmark the first 'port town' where the player spawns:
        // bookmarkNewPortTownLocation(playerBoatTransform_target.position.x, playerBoatTransform_target.position.z);

        
        // Instantiate the interactables:
        doorHitboxManager = new DoorHitboxManager(terrainPacket.getDoorLocations(), terrainPacket.getDoorSides(), terrainPacket.getDoorColors());

        // first instantiate the final treasure:
        interactablesManager.addObjectToArray(instantiateItem_initial(terrainPacket.getFinalTreasureLocation()[0], terrainPacket.getFinalTreasureLocation()[1], Constants.interactableID_treasureFinal), 
                                            Constants.interactableID_treasureFinal,
                                            0);

        // Then instantiate all the keys:
        byte[,] itemLocations = terrainPacket.getKeyLocations();
        bool[] hasKey = terrainPacket.getHasKey();
        byte[] itemIDs = {Constants.interactableID_key1, Constants.interactableID_key2, Constants.interactableID_key3, Constants.interactableID_key4, Constants.interactableID_key5, Constants.interactableID_key6};
        for (short index=0; index<6; index++)
        {
            if (hasKey[index])
            {
                interactablesManager.addObjectToArray(instantiateItem_initial(itemLocations[index, 0], itemLocations[index, 1], itemIDs[index]), 
                                                    itemIDs[index],
                                                    index);
            }
        }
        
        // then instantiate all the door prefabs:
        itemLocations = terrainPacket.getDoorLocations();
        itemIDs = terrainPacket.getDoorColors();
        byte[] doorSide = terrainPacket.getDoorSides();
        
        for (short index=0; index<itemIDs.Length; index++)
        {
            interactablesManager.addObjectToArray(instantiateDoor_initial(itemLocations[index, 0], itemLocations[index, 1], itemIDs[index], doorSide[index]), 
                                                itemIDs[index],
                                                index);
        }

        // Now instantiate the minimap:
        minimapManager.initializeMinimapData(terrainPacket.getSimplePacket(), terrainPacket.getBoolArray_noNoise(), persistentData);

        // Now instantiate all the port towns:
        PortTownReturnPacket portTownPacket = terrainPacket.getPortTownPacket();

        placeAllPortTowns(portTownPacket);
        minimapManager.setPortTownData(portTownPacket);


        // Instantiate the enemy manager:
        enemyManager.placeStartingEnemies(playerBoatTransform_target.position, boolArray, doorHitboxManager, 
                                        (byte)(numOfRooms_horizontal-3), (byte)(numOfRooms_vertical-3), 
                                        portTownPacket, persistentData.getDensityOfEnemies_value());


        // Cannonball related stuff:
        touchManager.setArrowAndWheelColors(terrainColors[0]);
        cannonballManager.initializeCannonballData(boolArray, doorHitboxManager, persistentData.getDensityOfEnemies_value(), persistentData.getShipColors()[5]);
    }

    private GameObject instantiateItem_initial(byte itemX, byte itemZ, byte itemID)
    {
        Vector3 newPosition = new Vector3((((float)itemX) + 0.5f) * Constants.roomWidthHeight, 0f, (((float)itemZ) + 0.5f) * -Constants.roomWidthHeight);

        switch(itemID)
        {
            case Constants.interactableID_key1:
                return Instantiate(prefab_key1, newPosition, Quaternion.identity);
                
            case Constants.interactableID_key2:
                return Instantiate(prefab_key2, newPosition, Quaternion.identity);
                
            case Constants.interactableID_key3:
                return Instantiate(prefab_key3, newPosition, Quaternion.identity);
                
            case Constants.interactableID_key4:
                return Instantiate(prefab_key4, newPosition, Quaternion.identity);
                
            case Constants.interactableID_key5:
                return Instantiate(prefab_key5, newPosition, Quaternion.identity);
                
            case Constants.interactableID_key6:
                return Instantiate(prefab_key6, newPosition, Quaternion.identity);
                
                
            default: //case Constants.interactableID_treasureFinal:
                return Instantiate(prefab_treasureFinal, newPosition, Quaternion.identity);
        }
    }

    private GameObject instantiateDoor_initial(byte itemX, byte itemZ, byte itemID, byte itemSide)
    {
        GameObject returnDoor;

        switch(itemID)
        {
            case Constants.interactableID_door1:
                returnDoor = Instantiate(prefab_door1);
                break;
                
            case Constants.interactableID_door2:
                returnDoor = Instantiate(prefab_door2);
                break;
                
            case Constants.interactableID_door3:
                returnDoor = Instantiate(prefab_door3);
                break;
                
            case Constants.interactableID_door4:
                returnDoor = Instantiate(prefab_door4);
                break;
                
            case Constants.interactableID_door5:
                returnDoor = Instantiate(prefab_door5);
                break;
                
            default: //case Constants.interactableID_door6:
                returnDoor = Instantiate(prefab_door6);
                break;
        }
        
        switch (itemSide)
        {
            case Constants.doorID_left:
                returnDoor.transform.position = new Vector3((((float)itemX) ) * Constants.roomWidthHeight, 0f, (((float)itemZ) + 0.5f) * -Constants.roomWidthHeight);
                returnDoor.transform.forward = new Vector3(1f, 0f, 0f);
                break;

            case Constants.doorID_up:
                returnDoor.transform.position = new Vector3((((float)itemX) + 0.5f) * Constants.roomWidthHeight, 0f, (((float)itemZ) ) * -Constants.roomWidthHeight);
                returnDoor.transform.forward = new Vector3(0f, 0f, -1f);
                break;
                
            case Constants.doorID_down:
                returnDoor.transform.position = new Vector3((((float)itemX) + 0.5f) * Constants.roomWidthHeight, 0f, (((float)itemZ) + 1f) * -Constants.roomWidthHeight);
                returnDoor.transform.forward = new Vector3(0f, 0f, 1f);
                break;
                
            default: //case Constants.doorID_right:
                returnDoor.transform.position = new Vector3((((float)itemX) + 1f) * Constants.roomWidthHeight, 0f, (((float)itemZ) + 0.5f) * -Constants.roomWidthHeight);
                returnDoor.transform.forward = new Vector3(-1f, 0f, 0f);
                break;
        }

        return returnDoor;
    }

    private void placeAllPortTowns(PortTownReturnPacket portTownPacket)
    {
        GameObject tempBuilding;
        PortTownIndividualPacket portTownIndividual;
        int numOfHouses;
        Color32 tempRoofColor;
        Color32 tempWallColor;
        // int brightnessOffset;

        for (byte index1=0; index1<portTownPacket.getNumofPortTowns_actual(); index1++)
        {
            // Place the pier:
            tempBuilding = Instantiate(prefab_portTownPier, portTownPacket.getPortTownLocation_asVector3(index1), Quaternion.identity);
            tempBuilding.transform.eulerAngles = portTownPacket.getPortTownRotation_asVector3(index1);

            interactablesManager.addObjectToArray(tempBuilding, Constants.interactableID_portTownPier, 0);

            // Now place all the houses:
            portTownIndividual = portTownPacket.getPortTownArray()[index1];
            numOfHouses = portTownIndividual.getHouseLocations().Length;

            for (byte index2=0; index2<numOfHouses; index2++)
            {
                tempBuilding = Instantiate(prefab_portTownHouse, portTownIndividual.getHouseLocations()[index2], Quaternion.identity);
                tempBuilding.transform.eulerAngles = portTownIndividual.getHouseRotation_asVector3(index2);
                tempBuilding.transform.localScale = portTownIndividual.getHouseScales()[index2];

                tempRoofColor = portTownIndividual.getHouseRoofColors()[index2];
                tempWallColor = portTownIndividual.getHouseWallColors()[index2];

                setHouseColors(tempRoofColor, tempWallColor, tempBuilding.GetComponent<MeshRenderer>().material);
            }
        }
    }

    private void setHouseColors(Color32 roofColor, Color32 wallsColor, Material houseMaterial)
    {
        Texture2D houseTexture = new Texture2D(2, 1);
        houseTexture.wrapMode = TextureWrapMode.Clamp;
        houseTexture.filterMode = FilterMode.Point;

        houseMaterial.mainTexture = houseTexture;

        houseTexture.SetPixel(0, 0, roofColor);
        houseTexture.SetPixel(1, 0, wallsColor);
        houseTexture.Apply();
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Combat stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public bool checkIfCannonballHasHitPlayer(Vector3 cannonballPosition, Vector3 cannonballDirection)
    {
        if (Constants.checkIfObjectIsCloseEnough(playerBoatTransform_delayed.position, cannonballPosition, 1f))
        {
            cannonballStrikesPlayer(cannonballDirection);
            return true;
        }

        return false;
    }

    private void cannonballStrikesPlayer(Vector3 cannonballDirection)
    {
        //damage player,
        //make them invulnerable,
        //turn them red
        //begin knocking them back

        if (isVulnerable)
        {
            isVulnerable = false;
            invulnerabilityDuration_current = invulnerabilityDuration_max;

            playDamagePlayerShipAnimation(cannonballDirection);

            // playerBoat_actual.GetComponent<Renderer>().material.SetColor("_Color", new Color32(180, 180, 180, 255));
            playerBoat_actual_renderer_main.material.SetColor("_Color", new Color32(180, 180, 180, 255));
            playerBoat_actual_renderer_sail.material.SetColor("_Color", new Color32(180, 180, 180, 255));

            // TODO: Damage health, sink ship...
            shipHealth_current--;
            if (shipHealth_current == 0)
            {
                playerShipSinks_part1();
            }

            healthBarManager.playerHasBeenDamaged();


            // For now:
            // lostMoney(25000);
        }
    }

    private void checkVulnerability()
    {
        if (!isVulnerable)
        {
            if (isBeingKnockedBack)
            {
                knockbackDirection_current *= Constants.knockback_dragMultiplier_player;
                if (BoolArrayManager.checkForConflict_V3(boolArray, doorHitboxManager, playerBoatTransform_target.position + knockbackDirection_current))
                {
                    playerBoatTransform_target.position += knockbackDirection_current;
                    playerBoatTransform_delayed.position += knockbackDirection_current;
                }

                if (knockbackDuration_current > 0)
                {
                    knockbackDuration_current--;
                    if (knockbackDuration_current == 0)
                    {
                        isBeingKnockedBack = false;
                    }
                }
            }

            if (--invulnerabilityDuration_current <= 5)
            {
                isVulnerable = true;
                // playerBoat_actual.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
                playerBoat_actual_renderer_main.material.SetColor("_Color", new Color32(255, 255, 255, 255));
                playerBoat_actual_renderer_sail.material.SetColor("_Color", new Color32(255, 255, 255, 255));
            }
        }
    }

    private void playDamagePlayerShipAnimation(Vector3 cannonballDirection)
    {
        cameraManager.playDamageShakeAnimation();

        isBeingKnockedBack = true;
        knockbackDirection_normalized = cannonballDirection.normalized;
        knockbackDirection_current = new Vector3(knockbackDirection_normalized.x, 0f, knockbackDirection_normalized.z) * Constants.knockback_StartingAmount_player;
        knockbackDuration_current = Constants.knockback_Duration;
    }


    public void bookmarkNewPortTownLocation(float portTownX, float portTownZ, float portTownYRotation)
    {
        bookmarkedPortTownLocation.x = portTownX;
        bookmarkedPortTownLocation.z = portTownZ;

        bookmarkedPortTownRotation.y = portTownYRotation;

        // Debug.Log("Obtained new bookmark! "+bookmarkedPortTownLocation);
    }
    


    // ------------------------------------------------------------------------------------------------------
    // ----------- Sinking stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void playerShipSinks_part1()
    {
        // half-pause the game, wait for a short time...
        // make the ship invisible, spawn ship treasure, drop final treasure if towed.
        // wait for a short time...
        // move the ship target to the bookmarked coord, wait for a short time...
        // resume the game.
        
        sinkingTimer_1_current = sinkingTimer_1_max;
        sinkingTimer_2_current = sinkingTimer_2_max;
        sinkingTimer_3_current = sinkingTimer_3_max;

        shipIsSinking_1 = true;
        isBeingKnockedBack = false;
        updateGameState();

        persistentData.lostAShip();
    }

    private void advanceSinkingAnimation_1()
    {
        if (sinkingTimer_1_current > 0)
        {
            sinkingTimer_1_current--;
        } else {
            shipIsSinking_1 = false;

            //make the shipActual vanish
            playerBoat_actual.SetActive(false);
            wakeController.setShouldPlaceWakes(false);
            //destroy all the loot, and potentially drop the final treasure,
            lootTailManager.destroyAllLoot();
            //drop a ship treasure
            droppedShipTreasure(playerBoatTransform_delayed.position);

            //subtract 100000 moneys
            if (currentMoney > 100000)
            {
                lostMoney(100000);
                shipIsSinking_2 = true;
            } else {
                lostMoney(currentMoney);
                theGameIsLost();
            }
        }
    }

    private void advanceSinkingAnimation_2()
    {
        if (sinkingTimer_2_current > 0)
        {
            sinkingTimer_2_current--;
        } else {
            shipIsSinking_2 = false;
            shipIsSinking_3 = true;

            // move the ship target to the bookmarked coord,
            playerBoatTransform_target.position = bookmarkedPortTownLocation;
            playerBoatTransform_target.eulerAngles = bookmarkedPortTownRotation;
        }
    }

    private void advanceSinkingAnimation_3()
    {
        if (sinkingTimer_3_current > 0)
        {
            sinkingTimer_3_current--;
        } else {
            shipIsSinking_3 = false;
            updateGameState();

            // make the shipActual reappear
            playerBoat_actual.SetActive(true);
            wakeController.setShouldPlaceWakes(true);
            // restore the player's health
            shipHealth_current = shipHealth_max;
            healthBarManager.playerHasBeenHealed(shipHealth_current);
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Money stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void earnedMoney(int amount)
    {
        persistentData.earnedMoney(amount);

        currentMoney += amount;
        uiManager.updateMoneyAndLivesText(currentMoney);
    } 

    public void lostMoney(int amount)
    {
        persistentData.lostMoney(amount);

        currentMoney -= amount;
        if (currentMoney < 0) currentMoney = 0;
        uiManager.updateMoneyAndLivesText(currentMoney);
    }

    public void interactWithPortTown(Vector3 portTownLocation)
    {
        if (lootTailManager.getTailHasLoot())
        {
            earnedMoney(lootTailManager.sellAllLoot(portTownLocation));
        }

        if (shipHealth_current < shipHealth_max)
        {
            while (shipHealth_current < shipHealth_max && currentMoney >= Constants.repairCost)
            {
                lostMoney(Constants.repairCost);
                shipHealth_current++;
            }

            healthBarManager.playerHasBeenHealed(shipHealth_current);
        }
    }

    public void secretCannonLifeDrain()
    {
        if (shipHealth_current < shipHealth_max)
        {
            shipHealth_current = shipHealth_max;
            healthBarManager.playerHasBeenHealed(shipHealth_current);
        }
    }

    public void collectedFinalTreasure()
    {
        minimapManager.collectedFinalTreasure();
    }

    // public void droppedFinalTreasure(Vector3 dropPosition, GameObject finalTreasureModel)
    public void droppedFinalTreasure(GameObject finalTreasureModel)
    {
        // minimapManager.droppedFinalTreasure(dropPosition);
        minimapManager.droppedFinalTreasure(finalTreasureModel.transform.position);
        interactablesManager.addObjectToArray(finalTreasureModel, Constants.interactableID_treasureFinal, 0);
    }

    public void droppedOtherTreasure(GameObject treasureModel, byte treasureID)
    {
        interactablesManager.addObjectToArray(treasureModel, treasureID, 0);
    }

    // public void collectedShipTreasure(short treasureOrder)
    // {
    //     minimapManager.collectedShipTreasure(treasureOrder);
    // }
    
    public void droppedShipTreasure(Vector3 dropPosition)
    {
        // short shipTreasureOrder = minimapManager.droppedShipTreasure(dropPosition);

        // interactablesManager.addObjectToArray(lootPrefabManager.getLootObject_ship(dropPosition), Constants.interactableID_treasureShip, shipTreasureOrder);
        interactablesManager.addObjectToArray(lootPrefabManager.getLootObject_ship(dropPosition), Constants.interactableID_treasureShip, 0);
    }

    public void soldFinalTreasure()
    {
        persistentData.soldFinalTreasure();
        // uiManager.setFinalTreasureValue(Constants.getValueOfFinalTreasure());
        uiManager.setFinalTreasureValue(persistentData.getFinalTreasure());
        // endGameCountdownTimer = 30;
        endGameCountdownTimer = 90;

        gameIsOver = true;
        // gameState = Constants.gameState_movementPaused;
        updateGameState();
        // touchManager.removeCannonArrow();
    }

    public void theGameIsLost()
    {
        persistentData.thePlayerHasLost();
        endGameCountdownTimer = 150;

        gameIsOver = true;
        updateGameState();
    }

    private void advanceEndGameAnimation()
    {
        if (endGameCountdownTimer > 1)
        {
            endGameCountdownTimer--;
        } else {
            endGameCountdownTimer--;
            SceneManager.LoadScene("VictoryScene");
        }
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Minimap stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void exploredANewArea()
    {
        persistentData.exploredANewArea();
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Pausing and Resuming stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void tellTheOtherClasses_ThePlayerCanMove()
    {
        // I need to communicate with:
        // later...
        // the enemy manager, 
        // the cannonball manager, 
        // the treasure-tail manager...

        touchManager.manageTouches_canMove(boolArray, doorHitboxManager);

        tellTheOtherClasses_TheGameIsHalfPaused();

        interactablesManager.checkInteractionWithAllObjects(playerBoatTransform_delayed.position);
        enemyManager.manageEnemies(playerBoatTransform_delayed.position);
        minimapManager.updateMinimap_canMove(playerBoatTransform_delayed.position);
    }

    private void tellTheOtherClasses_TheGameIsHalfPaused()
    {
        // I need to communicate with:
        // --the wake controller,
        // --the ship-rocking controller,
        // --the interactables manager,
        // __potentially the water or lighting managers, if those do anyting in update...
        //     not currently.
        // later...
        // the enemy manager, 
        // the treasure-tail manager...

        touchManager.manageTouches_halfPaused();
        cannonballManager.moveAllActiveCannonballs();
        wakeController.updateWakes();
        shipRotationmanager.updateShipRotation();
        interactablesManager.advanceAllPrefabAnimations(playerBoatTransform_delayed.position);
        enemyManager.advanceAllPrefabAnimations();
        minimapManager.updateMinimap_halfPaused();
        uiManager.advanceMoneyRollAnimation();
        healthBarManager.updateHealthBar(playerBoatTransform_delayed.position);
        lootTailManager.updateTailPosition(playerBoatTransform_delayed.position, playerBoatTransform_delayed.eulerAngles);
    }

    private bool shouldTheGameBeUnpaused()
    {
        // TODO: check other factors and decide if the game can unpause.
        // These factors will ultimately be...
        // now: is the Mid-Game Settings menu open?
        // part 4: is the final treasure sold?
        // part 3: is the ship sinking/respawning?
        // part 99... is the fairy on screen?

        // But for now, in parts 1 or 2:
        return true;
    }

    private void updateGameState()
    {
        if (settingsMenuIsOpen || fairyIsVisible)
        {
            gameState = Constants.gameState_everythingPaused;

            touchManager.clearBothTouches();
            uiManager.removeBothThumbsticks();
            // touchManager.removeCannonArrow();
        } else if (shipIsSinking_1 || shipIsSinking_2 || shipIsSinking_3 || gameIsOver)
        {
            gameState = Constants.gameState_movementPaused;

            touchManager.clearBothTouches();
            uiManager.removeBothThumbsticks();
            touchManager.removeCannonArrow();
        } else {
            gameState = Constants.gameState_canMove;
        }
    }

    private void playHalfPausedAnimations()
    {
        if (shipIsSinking_1)
        {
            advanceSinkingAnimation_1();
        } else if (shipIsSinking_2)
        {
            advanceSinkingAnimation_2();
        } else if(shipIsSinking_3)
        {
            advanceSinkingAnimation_3();
        } else if(gameIsOver)
        {
            advanceEndGameAnimation();
        }
    }

    // ------------------------------------------------------------------------------------------------------
    // ----------- Mid-Game Settings stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void gearButtonPressed()
    {
        if (settingsMenuIsOpen)
        {
            midGameSettingsManager.requestCloseMenu();
        } else {
            if (midGameSettingsManager.requestOpenMenu())
            {
                settingsMenuIsOpen = true;
                // touchManager.clearBothTouches();
                // uiManager.removeBothThumbsticks();
                // gameState = Constants.gameState_everythingPaused;
                updateGameState();
            }
        }
    }

    public void theMenuIsNowClosed()
    {
        settingsMenuIsOpen = false;
        updateGameState();
        
        // if (shouldTheGameBeUnpaused())
        // {
        //     gameState = Constants.gameState_canMove;
        // }
    }

    public void setCameraDistance(byte cameraDistanceSetting)
    {
        cameraManager.adjustCameraDistance(cameraDistanceSetting);
    }

    public void setCameraAngle(byte cameraAngleSetting)
    {
        cameraManager.adjustCameraAngle(cameraAngleSetting);
    }

    public void buttonPressed_returnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
