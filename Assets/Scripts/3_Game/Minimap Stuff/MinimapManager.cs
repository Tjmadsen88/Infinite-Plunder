using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System; //Temp

using ConstantsSpace;

using MinimapSpace;
using SimplifiedLayoutBuilderSpace;
using PortTownSpace;

public class MinimapManager : MonoBehaviour
{
    public GameView gameView;
    public GameObject MinimapParent_all;
    public GameObject MinimapParent_tiles;
    public GameObject MinimapParent_doors;
    public GameObject MinimapParent_items;

    private RectTransform ParentTransform_all;
    private RectTransform ParentTransform_tiles;
    private RectTransform ParentTransform_doors;
    private RectTransform ParentTransform_items;

    private bool[,] roomExploredArray;

    private Vector2 centerCoords;
    // float centerX;
    // float centerY;

    private byte[] roomCoords_previous = new byte[2];
    private byte[] roomCoords_current = new byte[2];

    private const byte flashTimer_max = 35;
    private byte flashTimer_current = flashTimer_max;
    private bool nextFlashState = true;

    Texture2D minimapTexture;

    private float borderThickness;
    private const float doorCenterRadius = 13.5f;
    private const float playerIconRadius = 11f;

    private GameObject playerIcon;
    private RectTransform playerIconTransform;
    private Sprite itemOfInterestSprite;
    private Sprite portTownSprite;
    private Sprite doorSprite_red;
    private Sprite doorSprite_yellow;
    private Sprite doorSprite_green;
    private Sprite doorSprite_cyan;
    private Sprite doorSprite_blue;
    private Sprite doorSprite_magenta;

    private Color32 color_walls = new Color32(108, 94, 75, 255);
    private Color32 color_unexplored = new Color32(170, 138, 90, 255);
    private Color32 color_explored = new Color32(217, 203, 182, 255);
    private Color32 color_invisible = new Color32(0, 0, 0, 0);
    private Color32 color_invisible2 = new Color32(217, 203, 182, 0);

    // private Color32 color_doorRed = new Color32(224, 68, 68, 255);
    // private Color32 color_doorYellow = new Color32(189, 191, 65, 255);
    // private Color32 color_doorGreen = new Color32(76, 179, 76, 255);
    // private Color32 color_doorCyan = new Color32(138, 189, 190, 255);
    // private Color32 color_doorBlue = new Color32(120, 120, 251, 255);
    // private Color32 color_doorMagenta = new Color32(228, 101, 226, 255);
    private Color32 color_doorRed = new Color32(223, 60, 60, 255);
    private Color32 color_doorYellow = new Color32(224, 228, 92, 255);
    private Color32 color_doorGreen = new Color32(50, 167, 50, 255);
    private Color32 color_doorCyan = new Color32(158, 229, 229, 255);
    private Color32 color_doorBlue = new Color32(115, 115, 255, 255);
    private Color32 color_doorMagenta = new Color32(245, 129, 243, 255);
    private Color32 color_doorBorder = new Color32(51, 51, 51, 255);
    
    private Color32 color_playerIcon = new Color32(255, 255, 255, 255);

    private MinimapIcon[] keyIcons = new MinimapIcon[6];
    private MinimapIcon[] doorIcons;
    private MinimapIcon finalTreasureIcon;
    // private MinimapIcon[] shipTreasureIcons = new MinimapIcon[4];

    private short numOfLockedDoors;
    // private short firstOpenSlot_shipTreasureIcons = 0;

    // private const float checkDistance_door = 0.6f;
    // private const float checkDistance_item = 1.6f; // If only adj, then 1.1. If all eight, then 1.6.
    // private const float checkDistance_portTown = 0.2f;
    private const float checkDistance_door = 0.3f; // Now, using the sqrMagnitudes... If only adj: 0.3. if 'near' adjacents: 1.5. if all adjacent: 2.3
    private const float checkDistance_item = 2.1f; // If only adj, then 1.1. If all eight, then 2.1.
    private const float checkDistance_portTown = 0.2f;

    private byte[,] portTownCoords_minimap;
    private float[,] portTownCoords_world;
    private float[] portTownYRotations;
    private byte numOfPortTowns;
    private MinimapIcon[] portTownIcons;


    // ------------------------------------------------------------------------------------------------------
    // ----------- Initialization stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    void Start()
    {
        ParentTransform_all = MinimapParent_all.GetComponent<RectTransform>();
        ParentTransform_tiles = MinimapParent_tiles.GetComponent<RectTransform>();
        ParentTransform_doors = MinimapParent_doors.GetComponent<RectTransform>();
        ParentTransform_items = MinimapParent_items.GetComponent<RectTransform>();
    }

    public void initializeMinimapData(SimplifiedLayoutReturnPacket simplePacket, bool[,] boolArray, PersistentData persistentData)
    {

        int simpleRoomArrayWidth = simplePacket.getAreaWidth();
        int simpleRoomArrayHeight = simplePacket.getAreaHeight();
        int boolArrayWidth = boolArray.GetLength(0);
        int boolArrayHeight = boolArray.GetLength(1);

        roomExploredArray = new bool[simpleRoomArrayWidth, simpleRoomArrayHeight];

        // centerX = (simpleRoomArrayWidth-1) /2f;
        // centerY = (simpleRoomArrayHeight-1) /2f;
        centerCoords = new Vector2( (simpleRoomArrayWidth-1) /2f, (simpleRoomArrayHeight-1) /2f );

        float parentScaler = 343.65f / Mathf.Max(simpleRoomArrayWidth, simpleRoomArrayHeight);
        ParentTransform_all.localScale = new Vector3(parentScaler, parentScaler, 1f);
        ParentTransform_all.localEulerAngles = new Vector3(0f, 0f, -45f);


        borderThickness = findBorderThickness(boolArrayWidth, boolArrayHeight, 5f);
        
        
        // GameObject fullMap = createNewIcon(convertBoolArrayToMinimapSprite(boolArray, boolArrayWidth, boolArrayHeight), ParentTransform_tiles);
        GameObject fullMap = createNewIcon(applyColorArrayToMinimapSprite(persistentData.getMinimapColorArray(), boolArrayWidth, boolArrayHeight), ParentTransform_tiles);

        fullMap.GetComponent<RectTransform>().localScale = new Vector3(simpleRoomArrayWidth, -simpleRoomArrayHeight, 0f);

        // playerIcon = createNewIcon(drawCircleSprite_noBorder(playerIconRadius, color_playerIcon), ParentTransform_all);
        playerIcon = createNewIcon(drawCircleSprite_withBorder(playerIconRadius, borderThickness, color_playerIcon, color_walls), ParentTransform_all);
        //drawDoorSprite
        playerIconTransform = playerIcon.GetComponent<RectTransform>();

        itemOfInterestSprite = drawCircleSprite_noBorder(borderThickness, color_walls, color_invisible);
        doorSprite_red = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorRed, color_doorBorder);
        doorSprite_yellow = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorYellow, color_doorBorder);
        doorSprite_green = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorGreen, color_doorBorder);
        doorSprite_cyan = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorCyan, color_doorBorder);
        doorSprite_blue = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorBlue, color_doorBorder);
        doorSprite_magenta = drawCircleSprite_withBorder(doorCenterRadius, borderThickness, color_doorMagenta, color_doorBorder);
        // portTownSprite = drawDiamondSprite_noBorder(color_explored);
        // portTownSprite = drawSquareSprite_noBorder(doorCenterRadius, color_playerIcon);
        portTownSprite = drawCircleSprite_noBorder(playerIconRadius, color_playerIcon, color_invisible2);


        // Initialize the final treasure icon:
        finalTreasureIcon = new MinimapIcon( createNewIcon(itemOfInterestSprite, ParentTransform_items), 
                                            convertRoomCoordsToUICoords(simplePacket.getFinalTreasureLocation()[0], simplePacket.getFinalTreasureLocation()[1]), 
                                            false);

        // Initialize all the key icons:
        bool[] hasKey = simplePacket.getHasKey();
        for (int index=0; index<6; index++)
        {
            if (hasKey[index])
            {
                keyIcons[index] = new MinimapIcon( createNewIcon(itemOfInterestSprite, ParentTransform_items), 
                                                convertRoomCoordsToUICoords(simplePacket.getKeyLocations()[index, 0], simplePacket.getKeyLocations()[index, 1]), 
                                                false);
            } else {
                keyIcons[index] = new MinimapIcon( createNewIcon(itemOfInterestSprite, ParentTransform_items), 
                                                convertRoomCoordsToUICoords(-2, -2), 
                                                false);
            }
            
        }

        // Initialize the door icons:
        numOfLockedDoors = simplePacket.getNumOfLockedDoors();
        doorIcons = new MinimapIcon[numOfLockedDoors];
        for (int index=0; index<numOfLockedDoors; index++)
        {
            doorIcons[index] = new MinimapIcon( createDoorIconFromID(simplePacket.getDoorColors()[index]), 
                                            convertRoomCoordsToUICoords_forDoors(simplePacket.getDoorLocations()[index, 0], simplePacket.getDoorLocations()[index, 1], simplePacket.getDoorSides()[index]), 
                                            false);
        }


        // exploreRoom(simplePacket.getPlayerStartingLocation()[0], simplePacket.getPlayerStartingLocation()[1]);
        // exploreRoom((byte)(simplePacket.getPlayerStartingLocation()[0]+1), simplePacket.getPlayerStartingLocation()[1]);
        // exploreRoom((byte)(simplePacket.getPlayerStartingLocation()[0]-1), simplePacket.getPlayerStartingLocation()[1]);
        // exploreRoom(simplePacket.getPlayerStartingLocation()[0], (byte)(simplePacket.getPlayerStartingLocation()[1]+1));
        // exploreRoom(simplePacket.getPlayerStartingLocation()[0], (byte)(simplePacket.getPlayerStartingLocation()[1]-1));


        byte[] startLocation = simplePacket.getPlayerStartingLocation();
        byte[] tempLocation = new byte[2];

        exploreRoom(startLocation[0], startLocation[1]);

        if (startLocation[0] < simpleRoomArrayWidth-1)
        {
            tempLocation[0] = (byte)(startLocation[0]+1);
            tempLocation[1] = startLocation[1];
            if (simplePacket.getSimplifiedRoomArray()[tempLocation[0], tempLocation[1]].getIsNotEmpty())
                exploreRoom(tempLocation[0], tempLocation[1]);
        }

        if (startLocation[0] > 0)
        {
            tempLocation[0] = (byte)(startLocation[0]-1);
            tempLocation[1] = startLocation[1];
            if (simplePacket.getSimplifiedRoomArray()[tempLocation[0], tempLocation[1]].getIsNotEmpty())
                exploreRoom(tempLocation[0], tempLocation[1]);
        }
        
        if (startLocation[1] < simpleRoomArrayHeight-1)
        {
            tempLocation[0] = startLocation[0];
            tempLocation[1] = (byte)(startLocation[1]+1);
            if (simplePacket.getSimplifiedRoomArray()[tempLocation[0], tempLocation[1]].getIsNotEmpty())
                exploreRoom(tempLocation[0], tempLocation[1]);
        }
            
        if (startLocation[1] > 0)
        {
            tempLocation[0] = startLocation[0];
            tempLocation[1] = (byte)(startLocation[1]-1);
            if (simplePacket.getSimplifiedRoomArray()[tempLocation[0], tempLocation[1]].getIsNotEmpty())
                exploreRoom(tempLocation[0], tempLocation[1]);
        }
    }


    public void setPortTownData(PortTownReturnPacket portTownPacket)
    {
        numOfPortTowns = portTownPacket.getNumofPortTowns_actual();

        portTownCoords_minimap = new byte[numOfPortTowns, 2];
        portTownCoords_world = new float[numOfPortTowns, 2];
        portTownYRotations = new float[numOfPortTowns];
        portTownIcons = new MinimapIcon[numOfPortTowns];

        PortTownIndividualPacket tempRoom;

        for (byte index=0; index<numOfPortTowns; index++)
        {
            tempRoom = portTownPacket.getPortTownArray()[index];

            portTownCoords_minimap[index, 0] = tempRoom.getPortTownCoords_simple()[0];
            portTownCoords_minimap[index, 1] = tempRoom.getPortTownCoords_simple()[1];
            portTownCoords_world[index, 0] = tempRoom.getPortTownCoords_upscaled()[0];
            portTownCoords_world[index, 1] = tempRoom.getPortTownCoords_upscaled()[1];
            portTownYRotations[index] = tempRoom.getPortTownYRotation() + 90f;

            portTownIcons[index] = new MinimapIcon( createNewIcon(portTownSprite, ParentTransform_tiles), 
                                                convertRoomCoordsToUICoords(tempRoom.getPortTownCoords_simple()[0], tempRoom.getPortTownCoords_simple()[1]), 
                                                false);
        }
    }


    private GameObject createNewIcon(Sprite iconSprite, RectTransform parentTransform)
    {
        GameObject newIcon = new GameObject("Icon");
        newIcon.AddComponent<Image>();
        newIcon.GetComponent<Image>().sprite = iconSprite;
        newIcon.GetComponent<RectTransform>().SetParent(parentTransform);

        newIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        newIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
        newIcon.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0f);
        newIcon.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 0f);

        return newIcon;
    }


    private GameObject createDoorIconFromID(byte doorID)
    {
        switch (doorID)
        {
            case Constants.interactableID_door1:
                return createNewIcon(doorSprite_red, ParentTransform_doors);
                
            case Constants.interactableID_door2:
                return createNewIcon(doorSprite_yellow, ParentTransform_doors);
                
            case Constants.interactableID_door3:
                return createNewIcon(doorSprite_green, ParentTransform_doors);
                
            case Constants.interactableID_door4:
                return createNewIcon(doorSprite_cyan, ParentTransform_doors);
                
            case Constants.interactableID_door5:
                return createNewIcon(doorSprite_blue, ParentTransform_doors);
                
            default: //case Constants.interactableID_door6:
                return createNewIcon(doorSprite_magenta, ParentTransform_doors);
        }
    }




    

    // ------------------------------------------------------------------------------------------------------
    // ----------- Exploration stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    public void updateMinimap_canMove(Vector3 boatPos)
    {
        movePlayerIcon(boatPos);

        getRoomCoordsFromBoatPosition(boatPos);
        if (roomCoords_previous[0] != roomCoords_current[0] || roomCoords_previous[1] != roomCoords_current[1])
        {
            exploreRoom(roomCoords_current[0], roomCoords_current[1]);
            // movePlayerIcon_2(boatPos, roomCoords_current[0], roomCoords_current[1]);
            
            roomCoords_previous[0] = roomCoords_current[0];
            roomCoords_previous[1] = roomCoords_current[1];
        }
    }

    public void updateMinimap_halfPaused()
    {
        if (--flashTimer_current <= 5)
        {
            flashTimer_current = flashTimer_max;

            makeIconsFlash();
        }
    }


    private void exploreRoom(byte roomX, byte roomY)
    {
        if (!roomExploredArray[roomX, roomY])
        {
            roomExploredArray[roomX, roomY] = true;

            colorRoomAsExplored(roomX, roomY, Constants.numOfVertsPerEdge);
            checkIfDiscoveredItem(roomX, roomY);
            
            gameView.exploredANewArea();
        }

        checkIfNearPortTown(roomX, roomY);
    }

    private void checkIfDiscoveredItem(byte roomX, byte roomY)
    {
        if (!finalTreasureIcon.getHasBeenDiscovered() && checkIfCloseEnoughToItem(roomX, roomY, finalTreasureIcon.getUICoords(), checkDistance_item))
        {
            finalTreasureIcon.setHasBeenDiscovered(true);
            finalTreasureIcon.getIconObject().SetActive(nextFlashState);
        }

        for (int index=0; index<6; index++)
        {
            if (!keyIcons[index].getHasBeenDiscovered() && checkIfCloseEnoughToItem(roomX, roomY, keyIcons[index].getUICoords(), checkDistance_item))
            {
                keyIcons[index].setHasBeenDiscovered(true);
                keyIcons[index].getIconObject().SetActive(nextFlashState);
            }
        }

        for (int index=0; index<numOfLockedDoors; index++)
        {
            if (!doorIcons[index].getHasBeenDiscovered() && checkIfCloseEnoughToItem(roomX, roomY, doorIcons[index].getUICoords(), checkDistance_door))
            {
                doorIcons[index].setHasBeenDiscovered(true);
                doorIcons[index].getIconObject().SetActive(true);
            }
        }

        // for (int index=0; index<numOfPortTowns; index++)
        // {
        //     if (!portTownIcons[index].getHasBeenDiscovered() && checkIfCloseEnoughToItem(roomX, roomY, portTownIcons[index].getUICoords(), checkDistance_portTown))
        //     {
        //         portTownIcons[index].setHasBeenDiscovered(true);
        //         portTownIcons[index].getIconObject().SetActive(true);
        //     }
        // }
    }

    private bool checkIfCloseEnoughToItem(byte roomX, byte roomY, Vector2 itemCoords, float checkDistance)
    {
        // return (convertRoomCoordsToUICoords(roomX, roomY) - itemCoords).magnitude <= checkDistance;
        return (convertRoomCoordsToUICoords(roomX, roomY) - itemCoords).sqrMagnitude <= checkDistance;
    }

    private void checkIfNearPortTown(byte roomX, byte roomY)
    {
        for (byte index=0; index<numOfPortTowns; index++)
        {
            if (roomX == portTownCoords_minimap[index, 0] && roomY == portTownCoords_minimap[index, 1])
            {
                gameView.bookmarkNewPortTownLocation(portTownCoords_world[index, 0], portTownCoords_world[index, 1], portTownYRotations[index]);

                if (!portTownIcons[index].getHasBeenDiscovered())
                {
                    portTownIcons[index].setHasBeenDiscovered(true);
                    portTownIcons[index].getIconObject().SetActive(true);
                }

                return;
            }
        }
    }



    private void movePlayerIcon(Vector3 boatPos)
    {
        playerIconTransform.anchoredPosition = new Vector2(boatPos.x / Constants.roomWidthHeight - centerCoords.x -0.5f, boatPos.z / Constants.roomWidthHeight + centerCoords.y +0.5f);
    }

    private void makeIconsFlash()
    {
        nextFlashState = !playerIcon.activeSelf;
        playerIcon.SetActive(nextFlashState);

        if (finalTreasureIcon.getHasBeenDiscovered() && finalTreasureIcon.getIsStillThere()) 
            finalTreasureIcon.getIconObject().SetActive(nextFlashState);

        for (int index=0; index<6; index++)
        {
            if (keyIcons[index].getHasBeenDiscovered() && keyIcons[index].getIsStillThere()) 
                keyIcons[index].getIconObject().SetActive(nextFlashState);
        }

        // for (int index=0; index<firstOpenSlot_shipTreasureIcons; index++)
        // {
        //     shipTreasureIcons[index].getIconObject().SetActive(nextFlashState);
        // }
    }

    private void getRoomCoordsFromBoatPosition(Vector3 boatPos)
    {
        roomCoords_current[0] = (byte)(boatPos.x / Constants.roomWidthHeight);
        roomCoords_current[1] = (byte)(boatPos.z / -Constants.roomWidthHeight);
    }

    private Vector2 convertRoomCoordsToUICoords(int roomX, int roomY)
    {
        // This was the 'diagonal' way:
        // return new Vector2(((roomX-centerX) - (roomY-centerY)) * 0.5f, ((roomX-centerX) + (roomY-centerY)) * -0.5f);

        // This is the rotated-rectangle way:
        return new Vector2(roomX-centerCoords.x, centerCoords.y-roomY);
    }

    private Vector2 convertRoomCoordsToUICoords_forDoors(int roomX, int roomY, byte doorSide)
    {
        switch (doorSide)
        {
            case Constants.doorID_left:
                return new Vector2(roomX-centerCoords.x-0.5f, centerCoords.y-roomY);
                
            case Constants.doorID_up:
                return new Vector2(roomX-centerCoords.x, centerCoords.y-roomY+0.5f);
                
            case Constants.doorID_down:
                return new Vector2(roomX-centerCoords.x, centerCoords.y-roomY-0.5f);
                
            default: //case Constants.doorID_right:
                return new Vector2(roomX-centerCoords.x+0.5f, centerCoords.y-roomY);
        }
    }

    private Vector2 convertUICoordsToRoomCoords(Vector2 uiCoords)
    {
        return new Vector2(uiCoords.x + centerCoords.x, -(uiCoords.y + centerCoords.y));
    }



    public void collectedKey(short keyIndex)
    {
        keyIcons[keyIndex].setIsStillThere(false);
        keyIcons[keyIndex].getIconObject().SetActive(false);
    }

    public void openedDoor(short doorIndex)
    {
        doorIcons[doorIndex].setIsStillThere(false);
        doorIcons[doorIndex].getIconObject().SetActive(false);
    }

    public void collectedFinalTreasure()
    {
        finalTreasureIcon.setIsStillThere(false);
        finalTreasureIcon.getIconObject().SetActive(false);
    }

    public void droppedFinalTreasure(Vector3 dropPosition)
    {
        // finalTreasureIcon.moveIcon(new Vector2(dropPosition.x / Constants.roomWidthHeight - centerCoords.x -0.5f, dropPosition.z / Constants.roomWidthHeight + centerCoords.y +0.5f),
        finalTreasureIcon.moveIcon(new Vector2((byte)(dropPosition.x / Constants.roomWidthHeight) - centerCoords.x, centerCoords.y - (byte)(dropPosition.z / -Constants.roomWidthHeight)),
                                                true, true);
    }

    // public void collectedShipTreasure(short shipTreasureIndex)
    // {
    //     // shipTreasureIcons[shipTreasureIndex].setIsStillThere(false);
    //     // shipTreasureIcons[shipTreasureIndex].getIconObject().SetActive(false);

    //     Destroy(shipTreasureIcons[shipTreasureIndex].getIconObject());
    //     removeObjectFromArray(shipTreasureIndex);
    // }

    // public short droppedShipTreasure(Vector3 dropPosition)
    // {
    //     addShipTreasureIconToArray(dropPosition);
    //     return (short)(firstOpenSlot_shipTreasureIcons-1);
    // }



    



    // ------------------------------------------------------------------------------------------------------
    // ----------- Sprite-drawing stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------


    // private Sprite convertBoolArrayToMinimapSprite(bool[,] boolArray, int boolArrayWidth, int boolArrayHeight)
    // {
    //     long startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    //     minimapTexture = new Texture2D(boolArrayWidth, boolArrayHeight);
    //     minimapTexture.wrapMode = TextureWrapMode.Clamp;
    //     // minimapTexture.filterMode = FilterMode.Point;


    //     Debug.Log("Before creating wall filter: "+(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-startTime));
    //     bool[,] wallFilter = createWallFilter(findBorderThickness(boolArrayWidth, boolArrayHeight, 5));
    //     Debug.Log("After creating wall filter: "+(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-startTime));

    //     for (int indexX=0; indexX<boolArrayWidth; indexX++)
    //     {
    //         for (int indexY=0; indexY<boolArrayHeight; indexY++)
    //         {
    //             minimapTexture.SetPixel(indexX, indexY, getPixelColor(boolArray, boolArrayWidth, boolArrayHeight, wallFilter, indexX, indexY));
    //         }
    //     }
    //     Debug.Log("After setting all pixels: "+(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-startTime));
        
    //     minimapTexture.Apply();
    //     Debug.Log("After applying texture: "+(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()-startTime));

    //     return Sprite.Create(minimapTexture, new Rect(0.0f, 0.0f, minimapTexture.width, minimapTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    // }


    private Sprite applyColorArrayToMinimapSprite(Color32[] minimapColorArray, int boolArrayWidth, int boolArrayHeight)
    {
        minimapTexture = new Texture2D(boolArrayWidth, boolArrayHeight);
        minimapTexture.wrapMode = TextureWrapMode.Clamp;
        // minimapTexture.filterMode = FilterMode.Point;
        
        minimapTexture.SetPixels32(minimapColorArray, 0);
        minimapTexture.Apply();

        return Sprite.Create(minimapTexture, new Rect(0.0f, 0.0f, minimapTexture.width, minimapTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    }


    private float findBorderThickness(int imageWidth, int imageHeight, float targetThickness)
    {
        return targetThickness * Mathf.Max(imageWidth, imageHeight) / 343.65f;
    }


    // private bool[,] createWallFilter(float radius)
    // {
    //     int intRadius = (int)Mathf.Round(radius);
    //     int returnArrayWidthHeight = 2*intRadius+1;
    //     bool[,] returnArray = new bool[returnArrayWidthHeight, returnArrayWidthHeight];

    //     for (int indexX=0; indexX<returnArrayWidthHeight; indexX++)
    //     {
    //         for (int indexY=0; indexY<returnArrayWidthHeight; indexY++)
    //         {
    //             returnArray[indexX, indexY] = Mathf.Sqrt(Mathf.Pow(indexX-intRadius, 2) + Mathf.Pow(indexY-intRadius, 2)) <= intRadius;
    //         }
    //     }

    //     // Debug.Log("Border thickness: "+intRadius);

    //     return returnArray;
    // }


    // private Color32 getPixelColor(bool[,] boolArray, int boolArrayWidth, int boolArrayHeight, bool[,] wallFilter, int pixelX, int pixelY)
    // {
    //     if (boolArray[pixelX, pixelY]) return color_unexplored;

    //     int wallFilterSize = wallFilter.GetLength(0);

    //     int filterCenter = (wallFilterSize - 1)/2;
    //     int entryX;
    //     int entryY;

    //     for (int indexX=0; indexX<wallFilterSize; indexX++)
    //     {
    //         for (int indexY=0; indexY<wallFilterSize; indexY++)
    //         {
    //             entryX = pixelX + filterCenter - indexX;
    //             entryY = pixelY + filterCenter - indexY;

    //             if (entryX >= 0 && entryY >= 0 && entryX < boolArrayWidth && entryY < boolArrayHeight)
    //             {
    //                 if (boolArray[entryX, entryY] && wallFilter[indexX, indexY]) return color_walls;
    //             }
    //         }
    //     }

    //     return color_invisible;
    // }


    private void colorRoomAsExplored(int roomX, int roomY, int numOfVertsPerEdge)
    {
        int tempX;
        int tempY;

        for (int indexX=0; indexX<numOfVertsPerEdge; indexX++)
        {
            for (int indexY=0; indexY<numOfVertsPerEdge; indexY++)
            {
                tempX = roomX * numOfVertsPerEdge + indexX;
                tempY = roomY * numOfVertsPerEdge + indexY;

                if (minimapTexture.GetPixel(tempX, tempY) == color_unexplored)
                {
                    minimapTexture.SetPixel(tempX, tempY, color_explored);
                }
            }
        }
        
        minimapTexture.Apply();
    }


    // private void colorCircleAroundShip(Vector2 boatPos, int detectionRadius)
    // {
    //     int boatX = (int)((boatPos.x + centerCoords.x +0.5f) * Constants.numOfVertsPerEdge);
    //     int boatY = (int)((boatPos.y - centerCoords.y -0.5f)* -Constants.numOfVertsPerEdge);

    //     // Debug.Log("BoatPos: "+boatPos.x+", "+boatPos.y);
        
    //     int tempX;
    //     int tempY;

    //     for (int indexX=0; indexX<detectionRadius*2; indexX++)
    //     {
    //         for (int indexY=0; indexY<detectionRadius*2; indexY++)
    //         {
    //             tempX = boatX + indexX - detectionRadius;
    //             tempY = boatY + indexY - detectionRadius;

    //             if (Mathf.Sqrt(Mathf.Pow(tempX - boatX, 2) + Mathf.Pow(tempY - boatY, 2)) < detectionRadius &&
    //                 minimapTexture.GetPixel(tempX, tempY) == color_unexplored)
    //             {
    //                 minimapTexture.SetPixel(tempX, tempY, color_explored);
    //             }
    //         }
    //     }
        
    //     minimapTexture.Apply();
    // }


    
    
    private Sprite drawCircleSprite_withBorder(float centerRadius, float borderThickness, Color32 centerColor, Color32 borderColor)
    {
        Texture2D doorTexture = new Texture2D(50, 50);
        doorTexture.wrapMode = TextureWrapMode.Clamp;
        // minimapTexture.filterMode = FilterMode.Point;

        float tempRadius;

        for (int indexX=0; indexX<50; indexX++)
        {
            for (int indexY=0; indexY<50; indexY++)
            {
                tempRadius = Mathf.Sqrt(Mathf.Pow(indexX - 24.5f, 2) + Mathf.Pow(indexY - 24.5f, 2));

                if (tempRadius <= centerRadius)
                    doorTexture.SetPixel(indexX, indexY, centerColor);
                else if (tempRadius <= centerRadius + borderThickness)
                    doorTexture.SetPixel(indexX, indexY, borderColor);
                else doorTexture.SetPixel(indexX, indexY, color_invisible);
            }
        }
        
        doorTexture.Apply();

        return Sprite.Create(doorTexture, new Rect(0.0f, 0.0f, doorTexture.width, doorTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    }
    
    private Sprite drawCircleSprite_noBorder(float radius, Color32 imageColor, Color32 invisibleColor)
    {
        Texture2D doorTexture = new Texture2D(50, 50);
        doorTexture.wrapMode = TextureWrapMode.Clamp;
        // minimapTexture.filterMode = FilterMode.Point;

        float tempRadius;

        for (int indexX=0; indexX<50; indexX++)
        {
            for (int indexY=0; indexY<50; indexY++)
            {
                tempRadius = Mathf.Sqrt(Mathf.Pow(indexX - 24.5f, 2) + Mathf.Pow(indexY - 24.5f, 2));

                if (tempRadius <= radius)
                    doorTexture.SetPixel(indexX, indexY, imageColor);
                else doorTexture.SetPixel(indexX, indexY, invisibleColor);
            }
        }
        
        doorTexture.Apply();

        return Sprite.Create(doorTexture, new Rect(0.0f, 0.0f, doorTexture.width, doorTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    }
    
    private Sprite drawDiamondSprite_noBorder(Color32 imageColor)
    {
        Texture2D doorTexture = new Texture2D(1, 1);
        doorTexture.wrapMode = TextureWrapMode.Clamp;
        // minimapTexture.filterMode = FilterMode.Point;

        // float tempRadius;

        // for (int indexX=0; indexX<iconWidth; indexX++)
        // {
        //     for (int indexY=0; indexY<iconWidth; indexY++)
        //     {
        //         doorTexture.SetPixel(indexX, indexY, imageColor);
        //     }
        // }

        doorTexture.SetPixel(0, 0, imageColor);
        
        doorTexture.Apply();

        return Sprite.Create(doorTexture, new Rect(0.0f, 0.0f, doorTexture.width, doorTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    }
    
    private Sprite drawSquareSprite_noBorder(float radius, Color32 imageColor)
    {
        Texture2D doorTexture = new Texture2D(50, 50);
        doorTexture.wrapMode = TextureWrapMode.Clamp;
        // minimapTexture.filterMode = FilterMode.Point;

        float tempRadius;

        for (int indexX=0; indexX<50; indexX++)
        {
            for (int indexY=0; indexY<50; indexY++)
            {
                tempRadius = Mathf.Abs(indexX - 24.5f) + Mathf.Abs(indexY - 24.5f);

                if (tempRadius < radius)
                    doorTexture.SetPixel(indexX, indexY, imageColor);
                else doorTexture.SetPixel(indexX, indexY, color_invisible2);
            }
        }

        doorTexture.SetPixel(0, 0, imageColor);
        
        doorTexture.Apply();

        return Sprite.Create(doorTexture, new Rect(0.0f, 0.0f, doorTexture.width, doorTexture.height), new Vector2(0.5f, 0.5f), 50.0f);
    }



    // ------------------------------------------------------------------------------------------------------
    // ----------- Ship treasure array management stuff: --------------------------------------------------------
    // ------------------------------------------------------------------------------------------------------

    // public void addShipTreasureIconToArray(Vector3 dropPosition)
    // {
    //     if (firstOpenSlot_shipTreasureIcons >= shipTreasureIcons.Length) expandArray();

    //     // Debug.Log("Adding item with ID: "+objectID);


    //     shipTreasureIcons[firstOpenSlot_shipTreasureIcons] = new MinimapIcon( createNewIcon(itemOfInterestSprite, ParentTransform_items), 
    //                                         // new Vector2(dropPosition.x / Constants.roomWidthHeight - centerCoords.x -0.5f, dropPosition.z / Constants.roomWidthHeight + centerCoords.y +0.5f), 
    //                                         new Vector2((byte)(dropPosition.x / Constants.roomWidthHeight) - centerCoords.x, centerCoords.y - (byte)(dropPosition.z / -Constants.roomWidthHeight)),
    //                                         true);

    //     firstOpenSlot_shipTreasureIcons++;
    // }


    // private void expandArray()
    // {
    //     MinimapIcon[] newArray = new MinimapIcon[shipTreasureIcons.Length * 2];

    //     for (int index=0; index<firstOpenSlot_shipTreasureIcons; index++)
    //     {
    //         newArray[index] = shipTreasureIcons[index];
    //     }

    //     shipTreasureIcons = newArray;
    // }


    // private void removeObjectFromArray(short indexOfObject)
    // {
    //     for (short index=indexOfObject; index<firstOpenSlot_shipTreasureIcons-1; index++)
    //     {
    //         shipTreasureIcons[index] = shipTreasureIcons[index+1];
    //     }

    //     firstOpenSlot_shipTreasureIcons--;
    // }


}
