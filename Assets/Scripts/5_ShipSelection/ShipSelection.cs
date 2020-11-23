using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ConstantsSpace;

using TerrainBuilderSpace;
using TerrainColorGeneratorSpace;
using NameGeneratorSpace;
using ShipDataSpace;

public class ShipSelection : MonoBehaviour
{
    public ShipTextureManager shipTextureManager;
    public ShipButtonManager shipButtonManager;
    public ShipSelectionDropdownManager dropdownManager;
    public ShipSelectionPopupManager popupManager;
    public ShipEditMenuManager editMenuManager;

    public Image titleBarImage;

    public GameObject cancelButton;
    public GameObject beginButton;

    public GameObject randomQuestionmarkIcon;
    public GameObject newPlusIcon;

    public Image dropDownButton_image;

    private int currentlySelectedButton = 0;

    public GameObject playerShip_parent;
    public GameObject playerShip_normal;
    public GameObject playerShip_invis;
    public Renderer playerShip_invis_main;
    public Renderer playerShip_invis_sail;
    private ShipRotationManager_ShipSelection shipRotationManager;
    private WakeController_ShipSelection shipWakeController;

    public GameObject waterPlane;
    public Material terrainMaterial;

    public Sprite sprite_titleBar_selection;
    public Sprite sprite_titleBar_edit;

    public Sprite sprite_dropdown_down;
    public Sprite sprite_dropdown_up;
    public Sprite sprite_begin_orange;

    public PersistentData persistentData;

    private bool buttonsCanBePressed_general = true;

    private Color32[] tempShipColor = new Color32[6];
    private byte tempSailPatternSelection;
    private bool tempSailIsMirrored_horizontal;
    private bool tempSailIsMirrored_vertical;
    private byte tempCannonSelection;

    private ShipDataPacket shipDataPacket;

    public Image fadeToBlackScreen;
    private const byte animationFrames_delayBeforeFadeToBlack = 10;
    private const byte animationFrames_fadeToBlack = 20;



    // Start is called before the first frame update
    void Start()
    {
        // Make the game run at 60fps:
        Application.targetFrameRate = 60;


        shipRotationManager = playerShip_parent.GetComponent<ShipRotationManager_ShipSelection>();
        shipWakeController = playerShip_parent.GetComponent<WakeController_ShipSelection>();

        shipDataPacket = loadShipData();
        shipButtonManager.loadShipButtons(shipDataPacket);
        shipTextureManager.initializeShipMaterials(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);

        generateSurroundingTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        shipRotationManager.updateShipRotation();
        shipWakeController.updateWakes();
    }


    public void setButtonsCanBePressed(bool buttonsCanBePressed)
    {
        buttonsCanBePressed_general = buttonsCanBePressed;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Bottom-bar button stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_Cancel()
	{
        if (buttonsCanBePressed_general)
        {
            buttonsCanBePressed_general = false;

            SceneManager.LoadScene("TitleScene");
        }
	}

    public void buttonPressed_Begin()
	{
        if (buttonsCanBePressed_general)
        {
            switch (currentlySelectedButton)
            {
                case -2: // 'New' is selected.
                    openEditMenu();
                    break;

                case -1: // 'Random' is selected.
                    getTempDataFromShipData(shipDataPacket.getShipData_oneRandom());
                    beginGame();
                    break;

                default: // A ship button is selected.
                    beginGame();
                    break;
            }
        }
	}

    private void beginGame()
    {
        beginButton.GetComponent<Image>().sprite = sprite_begin_orange;

        // beginButton.GetComponent<Image>().color = new Color32(255, 220, 170, 255);
        buttonsCanBePressed_general = false;
        persistentData.setTerrainColors( TerrainColorGenerator.GenerateTerrainColors() );
        persistentData.chooseValuesForRandomizedCustomizationData();
        persistentData.createLoadingDataWithThread();
        // StartCoroutine("playFadeToBlackAnimation");

        persistentData.setShipData(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempCannonSelection);

        shipDataPacket.recordSelectedShip(currentlySelectedButton);
        saveShipData();

        // SceneManager.LoadScene("LoadingScene");
        StartCoroutine("playFadeToBlackAnimation");
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Ship button stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_SelectNew()
    {
        if (buttonsCanBePressed_general)
        {
            if (currentlySelectedButton != -2)
            {
                playerShip_normal.SetActive(false);
                playerShip_invis.SetActive(true);

                playerShip_invis_main.material.SetColor("_Color", new Color32(185, 185, 185, 255));
                playerShip_invis_sail.material.SetColor("_Color", new Color32(185, 185, 185, 255));

                randomQuestionmarkIcon.SetActive(false);
                newPlusIcon.SetActive(true);
            }

            currentlySelectedButton = -2;
            shipButtonManager.selectButton(-2);
        }
    }

    public void buttonPressed_SelectRandom()
    {
        if (buttonsCanBePressed_general)
        {
            if (currentlySelectedButton != -1)
            {
                playerShip_normal.SetActive(false);
                playerShip_invis.SetActive(true);

                playerShip_invis_main.material.SetColor("_Color", new Color32(110, 110, 110, 255));
                playerShip_invis_sail.material.SetColor("_Color", new Color32(110, 110, 110, 255));

                randomQuestionmarkIcon.SetActive(true);
                newPlusIcon.SetActive(false);
            }
            
            currentlySelectedButton = -1;
            shipButtonManager.selectButton(-1);
        }
    }

    public void buttonPressed_ShipButton(int selectedButton)
    {
        if (buttonsCanBePressed_general)
        {
            buttonPressed_ShipButton_effect(selectedButton);
        }
    }

    public void buttonPressed_ShipButton_effect(int selectedButton)
    {
        if (currentlySelectedButton < 0)
        {
            playerShip_normal.SetActive(true);
            playerShip_invis.SetActive(false);

            randomQuestionmarkIcon.SetActive(false);
            newPlusIcon.SetActive(false);
        }
        
        currentlySelectedButton = selectedButton;
        shipButtonManager.selectButton_effect(selectedButton);

        getTempDataFromShipData(shipDataPacket.getShipData()[selectedButton]);
        shipTextureManager.changeColors(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);
    }

    public void buttonPressed_DeleteSelected()
    {
        if (buttonsCanBePressed_general && dropdownManager.getButtonsCanBePressed() && currentlySelectedButton >= 0)
        {
            popupManager.requestOpenMenu_popup(Constants.popupID_delete);
        }
    }

    public void deleteSelectedShip()
    {
        shipDataPacket.removeShip(currentlySelectedButton);
        shipButtonManager.removeShipButton(currentlySelectedButton);

        buttonPressed_DropDownArrow();

        saveShipData();
    }

    public void buttonPressed_reorder(int orderDirection)
    {
        if (buttonsCanBePressed_general && dropdownManager.getButtonsCanBePressed())
        {
            shipDataPacket.swapShipData(currentlySelectedButton, currentlySelectedButton +orderDirection);

            shipButtonManager.swapShipButtons(currentlySelectedButton, currentlySelectedButton +orderDirection);

            saveShipData();
        }
    }

    public void attachButtonListenerToButton(ShipButtonScript shipButton, int shipButtonIndex)
    {
        Button newButtonComponent = shipButton.getButtonComponent();
        newButtonComponent.onClick.AddListener(delegate {buttonPressed_ShipButton(shipButtonIndex); });
    }

    private void getTempDataFromShipData(ShipDataIndividual shipData)
    {
        tempShipColor[0] = shipData.getColor_SailPrimary();
        tempShipColor[1] = shipData.getColor_SailPattern();
        tempShipColor[2] = shipData.getColor_MastAndDeck();
        tempShipColor[3] = shipData.getColor_Rails();
        tempShipColor[4] = shipData.getColor_Hull();
        tempShipColor[5] = shipData.getColor_Cannonball();

        tempSailPatternSelection = shipData.getSailPatternSelection();
        tempSailIsMirrored_horizontal = shipData.getSailIsMirrored_horizontal();
        tempSailIsMirrored_vertical = shipData.getSailIsMirrored_vertical();
        tempCannonSelection = shipData.getCannonSelection();
    }

    private void fillTempDataWithDefaults()
    {
        tempShipColor[0] = Constants.defaultColor_SailPrimary;
        tempShipColor[1] = Constants.defaultColor_SailPattern;
        tempShipColor[2] = Constants.defaultColor_MastAndDeck;
        tempShipColor[3] = Constants.defaultColor_Rails;
        tempShipColor[4] = Constants.defaultColor_Hull;
        tempShipColor[5] = Constants.defaultColor_Cannonball;

        tempSailPatternSelection = Constants.defaultSelection_SailPattern;
        tempSailIsMirrored_horizontal = Constants.defaultSelection_SailIsMirrored_horizontal;
        tempSailIsMirrored_vertical = Constants.defaultSelection_SailIsMirrored_vertical;
        tempCannonSelection = Constants.defaultSelection_cannon;
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Edit menu stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_editSelected()
    {
        if (buttonsCanBePressed_general)
        {
            openEditMenu();
        }
    }

    private void openEditMenu()
    {
        shipButtonManager.requestCloseMenu_selectionMenu();

        string shipName;

        if (currentlySelectedButton >= 0)
        {
            shipName = shipDataPacket.getShipData()[currentlySelectedButton].getShipName();
        } else {
            shipName = RandomNameGenerator.generateRandomName();
            fillTempDataWithDefaults();
            shipTextureManager.changeColors(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical);

            playerShip_normal.SetActive(true);
            playerShip_invis.SetActive(false);

            newPlusIcon.SetActive(false);
        }

        editMenuManager.requestOpenMenu_editMenu(tempShipColor, tempSailPatternSelection, tempSailIsMirrored_horizontal, tempSailIsMirrored_vertical, tempCannonSelection, shipName, currentlySelectedButton);
        
        titleBarImage.sprite = sprite_titleBar_edit;
    }

    public void closeEditMenu()
    {
        shipButtonManager.requestOpenMenu_selectionMenu();
        editMenuManager.requestCloseMenu_editMenu();

        if (currentlySelectedButton >= 0)
            buttonPressed_ShipButton(currentlySelectedButton);
        else {
            playerShip_normal.SetActive(false);
            playerShip_invis.SetActive(true);
            buttonPressed_SelectNew();
        }

        titleBarImage.sprite = sprite_titleBar_selection;
    }


    public void editShip(Color32[] shipColor, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical, byte cannonSelection, string shipName)
    {
        // First update the shipDataPacket:
        shipDataPacket.updateShip( createShipDataFromRawData(shipColor, sailPatternSelection, sailIsMirrored_horizontal, sailIsMirrored_vertical, cannonSelection, shipName), currentlySelectedButton );

        //Then update the button:
        shipButtonManager.updateShipButton(currentlySelectedButton, shipName, cannonSelection);

        // Then save the data
        saveShipData();
    }


    public void createNewShip(Color32[] shipColor, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical, byte cannonSelection, string shipName)
    {
        // First create a new shipDataPacket:
        shipDataPacket.addNewShip( createShipDataFromRawData(shipColor, sailPatternSelection, sailIsMirrored_horizontal, sailIsMirrored_vertical, cannonSelection, shipName) );

        //Then create a button:
        shipButtonManager.createNewShipButtonAtEnd_andSelectIt(shipName, cannonSelection);

        // Then save the data
        saveShipData();
    }


    private ShipDataIndividual createShipDataFromRawData(Color32[] shipColor, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical, byte cannonSelection, string shipName)
    {
        ShipDataIndividual newShipData = new ShipDataIndividual();

        newShipData.setShipName(shipName);

        newShipData.setSailPatternSelection(sailPatternSelection);
        newShipData.setSailIsMirrored(sailIsMirrored_horizontal, sailIsMirrored_vertical);
        newShipData.setColor_SailPrimary(shipColor[0]);
        newShipData.setColor_SailPattern(shipColor[1]);

        newShipData.setColor_MastAndDeck(shipColor[2]);
        newShipData.setColor_Rails(shipColor[3]);
        newShipData.setColor_Hull(shipColor[4]);

        newShipData.setCannonSelection(cannonSelection);
        newShipData.setColor_Cannonball(shipColor[5]);

        return newShipData;
    }
    


    // ------------------------------------------------------------------------------------------------------
    // ----------- Drop Down stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void buttonPressed_DropDownArrow()
	{
        if (buttonsCanBePressed_general)
        {
            if (dropdownManager.changeMenuState())
            {
                dropDownButton_image.sprite = sprite_dropdown_up;
            } else {
                dropDownButton_image.sprite = sprite_dropdown_down;
            }
        }
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Saving and Loading stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void saveShipData()
    {
        ShipDataSaveAndLoadHandler.saveData_shipData(shipDataPacket, Constants.fileName_shipData);
        // saveData_shipData(shipDataPacket, Constants.fileName_shipData);
    }
    
    private ShipDataPacket loadShipData()
    {
        return ShipDataSaveAndLoadHandler.loadData_shipData(Constants.fileName_shipData);
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Misc stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    IEnumerator playFadeToBlackAnimation()
    {
        byte amountToFade = (byte)(255 / animationFrames_fadeToBlack +1);

        for(byte index = 0; index < animationFrames_delayBeforeFadeToBlack; index++)
        {
            yield return null;
        }

        for (byte index = 1; index<=animationFrames_fadeToBlack; index++) 
        {
            fadeToBlackScreen.color = new Color32(59, 59, 59, (byte)(Mathf.Min(amountToFade * index, 255)));
            yield return null;
        }

        SceneManager.LoadScene("LoadingScene");
    }


    // ------------------------------------------------------------------------------------------------------
    // ----------- Terrain Generation stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    private void generateSurroundingTerrain()
    {
        TerrainBuilderReturnPacket terrainPacket = TerrainBuilder.generateTerrain_shipSelectionVer(Constants.roomWidthHeight, Constants.numOfVertsPerEdge);
        Mesh[,] terrainMeshes = terrainPacket.getTerrainMeshes();

        int numOfRooms_horizontal = terrainMeshes.GetLength(0);
        int numOfRooms_vertical = terrainMeshes.GetLength(1);


        Color32[] terrainColors = persistentData.getTerrainColors();

        Texture2D terrainTexture = new Texture2D(4, 1);
        terrainTexture.wrapMode = TextureWrapMode.Clamp;
        terrainTexture.filterMode = FilterMode.Point;

        terrainMaterial.mainTexture = terrainTexture;

        terrainTexture.SetPixel(0, 0, terrainColors[0]);
        terrainTexture.SetPixel(1, 0, terrainColors[1]);
        terrainTexture.SetPixel(2, 0, terrainColors[2]);
        terrainTexture.SetPixel(3, 0, terrainColors[3]);
        terrainTexture.Apply();

        waterPlane.GetComponent<Renderer>().material.SetColor("_Color", new Color32(terrainColors[0].r, terrainColors[0].g, terrainColors[0].b, 64));
        
        byte fogInc = 153;
        float fogScale = 0.4f;
        RenderSettings.fogColor = new Color32((byte)(terrainColors[2].r*fogScale+fogInc), (byte)(terrainColors[2].g*fogScale+fogInc), (byte)(terrainColors[2].b*fogScale+fogInc), 255);

        // Instantiate the 3D terrain meshes:
        GameObject newTerrainParent = new GameObject("Empty");

        for (int indexX=0; indexX<numOfRooms_horizontal; indexX++)
        {
            for (int indexY=0; indexY<numOfRooms_vertical; indexY++)
            {
                GameObject newTerrain = new GameObject("Empty");
                newTerrain.AddComponent<MeshFilter>();
                newTerrain.AddComponent<MeshRenderer>();
                newTerrain.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                newTerrain.GetComponent<MeshFilter>().mesh = terrainMeshes[indexX, indexY];
                newTerrain.GetComponent<Renderer>().material = terrainMaterial;

                newTerrain.GetComponent<Transform>().position = new Vector3(((indexX * Constants.roomWidthHeight) - 1.5f*Constants.roomWidthHeight), 0, 
                                                                            ((-indexY * Constants.roomWidthHeight) + 2.5f*Constants.roomWidthHeight));
                newTerrain.GetComponent<Transform>().parent = newTerrainParent.GetComponent<Transform>();
            }
        }

        newTerrainParent.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
        newTerrainParent.GetComponent<Transform>().eulerAngles = new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        newTerrainParent.GetComponent<Transform>().position = new Vector3(0f, -0.4f, 0f);
    }
}
