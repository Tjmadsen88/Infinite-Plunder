using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using ConstantsSpace;

using TerrainBuilderSpace;
using TerrainColorGeneratorSpace;

public class TitleScreen : MonoBehaviour
{
    public TitleScreenCustomizationMenuManager customizationMenuManager;

    public GameObject playButton;

    public PersistentData persistentData;

    private bool buttonsCanBePressed = true;

    public GameObject waterPlane;
    public Material terrainMaterial;

    public Image fadeToBlackScreen;
    private const byte animationFrames_delayBeforeFadeToBlack = 10;
    private const byte animationFrames_fadeToBlack = 20;



    // Start is called before the first frame update
    void Start()
    {
        // Make the game run at 60fps:
        Application.targetFrameRate = 60;

        persistentData.resetAllValues();

        generateSurroundingTerrain();
    }

    public void setButtonsCanBePressed(bool buttonsCanBePressed)
    {
        this.buttonsCanBePressed = buttonsCanBePressed;
    }

    public void buttonPressed_BeginGame()
	{
        // if (buttonsCanBePressed)
        // {
        //     playButton.GetComponent<Image>().color = new Color32(255, 220, 170, 255);
        //     buttonsCanBePressed = false;
        //     persistentData.setTerrainColors( TerrainColorGenerator.GenerateTerrainColors() );
        //     persistentData.chooseValuesForRandomizedCustomizationData();
        //     persistentData.createLoadingDataWithThread();
        //     StartCoroutine("playFadeToBlackAnimation");

        //     // SceneManager.LoadScene("GameScene");
        //     // SceneManager.LoadScene("LoadingScene");
        // }

        if (buttonsCanBePressed)
        {
            buttonsCanBePressed = false;

            playButton.GetComponent<Image>().color = new Color32(255, 220, 170, 255);
            SceneManager.LoadScene("ShipSelectionScene");
        }
	}

    public void buttonPressed_BeginGame_SecretDemo()
	{
        if (buttonsCanBePressed)
        {
            playButton.GetComponent<Image>().color = new Color32(255, 220, 170, 255);
            buttonsCanBePressed = false;
            persistentData.setTerrainColors_default();
            persistentData.setShipData_default();
            persistentData.setCustomizationData_Demo();
            persistentData.createLoadingDataWithThread_Demo();
            StartCoroutine("playFadeToBlackAnimation");
        }
	}

    public void buttonPressed_Customize()
    {
        if (buttonsCanBePressed && customizationMenuManager.requestOpenMenu())
        {
            buttonsCanBePressed = false;
            customizationMenuManager.setInitialButtonValues();
        }
    }

    public void absorbTouch()
    {
        Debug.Log("Touch Absorbed!");
    }







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


    private void generateSurroundingTerrain()
    {
        TerrainBuilderReturnPacket terrainPacket = TerrainBuilder.generateTerrain_titleVer(Constants.roomWidthHeight, Constants.numOfVertsPerEdge);
        Mesh[,] terrainMeshes = terrainPacket.getTerrainMeshes();
        // Mesh[,] terrainMeshes = TerrainBuilder.sliceHeightArrayIntoMultipleMeshes(terrainPacket.getHeightArray(), Constants.roomWidthHeight, Constants.numOfVertsPerEdge);

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
                                                                            ((-indexY * Constants.roomWidthHeight) + 3.5f*Constants.roomWidthHeight));
                newTerrain.GetComponent<Transform>().parent = newTerrainParent.GetComponent<Transform>();
            }
        }

        newTerrainParent.GetComponent<Transform>().localScale = new Vector3(100f, 100f, 100f);
        newTerrainParent.GetComponent<Transform>().eulerAngles = new Vector3(2.3f, 0f, 0f);
        newTerrainParent.GetComponent<Transform>().position = new Vector3(0f, -223f, 0f);
    }
}
