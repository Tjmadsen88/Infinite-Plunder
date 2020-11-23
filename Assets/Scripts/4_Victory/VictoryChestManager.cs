using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeightArrayTo3DMeshConverterSpace;

public class VictoryChestManager : MonoBehaviour
{
    private const byte goldMeshVerts_horiz = 30;
    private const byte goldMeshVerts_verti = 22;
    private const float widthBetweenMeshVerts = 0.15f;
    private const float maxHeight = 2.65f;
    private readonly Vector3 goldStartPos = new Vector3(-2.17f, 0f, 1.57f);

    private const float perlinWidth = 0.1f;
    private const byte gemRadius = 7;


    public Material goldMaterial;

    public GameObject chestBox;
    public GameObject chestLid;
    public VictoryScreen victoryScreen;

    public GameObject gemstone1;
    public GameObject gemstone2;
    public GameObject gemstone3;
    public GameObject gemstone4;
    public GameObject gemstone5;
    public GameObject gemstone6;
    public GameObject gemstone7;
    public GameObject gemstone8;

    private Transform chestTransform_parent;
    private Transform chestTransform_box;
    private Transform chestTransform_lid;

    private bool shouldPlayAnimation = false;
    private bool animationIsOver = true;
    private byte animationState = 0;

    private const byte animFrames_0 = 12;  // pause before animation starts

    private const float lowestScrunchHeight = 0.875f;
    private const float totalLeanAmount = 7.5f;
    private const byte animFrames_1 = 10;  // Scrunch down
    private const byte animFrames_delay1 = 5;  // pause during scrunch
    private const byte animFrames_2 = 7;  // time during leaning back, but before lid opening

    private const float lidMaxAngle = -145f;
    private const byte animFrames_3_lidMovement = 12; // lid swings back

    private const float lidReboundAngle = -125f;
    private const byte animFrames_4 = 8;  // lid bounces a little

    private const byte animFrames_5 = 12;  // lid returns to fully open

    private const byte animFrames_after = 5;  // delay before stats appear




    public void playAnimation()
    {
        // I assume this is attached to the chest parent...
        chestTransform_parent = gameObject.GetComponent<Transform>();
        chestTransform_box = chestBox.GetComponent<Transform>();
        chestTransform_lid = chestLid.GetComponent<Transform>();

        float[,] heightArray = generateHeightArray();

        Mesh goldMesh = HeightArrayTo3DMeshConverter.create_total(heightArray, widthBetweenMeshVerts);

        attachGoldToBox(goldMesh);
        addGemstonesToGold(heightArray);

        shouldPlayAnimation = true;
    }

    private float[,] generateHeightArray()
    {
        float[,] returnArray = new float[goldMeshVerts_horiz, goldMeshVerts_verti];

        float maxPerlin = Random.Range(0.3f, 0.5f) * maxHeight;
        Debug.Log("MexPerlin: "+(maxPerlin/maxHeight));

        float lowestHeight = maxHeight - maxPerlin;

        float perlinOffsetX = Random.Range(0f, 200f);
        float perlinOffsetY = Random.Range(0f, 200f);

        for (int indexX=0; indexX<goldMeshVerts_horiz; indexX++)
        {
            for (int indexY=0; indexY<goldMeshVerts_verti; indexY++)
            {
                returnArray[indexX, indexY] = lowestHeight + maxPerlin * Mathf.PerlinNoise((indexX + perlinOffsetX) * 
                                                perlinWidth, (indexY + perlinOffsetY) * perlinWidth);
            }
        }

        return returnArray;
    }

    private void attachGoldToBox(Mesh goldMesh)
    {
        GameObject goldObject = new GameObject("GoldSheet");
        MeshFilter goldMeshFilter = goldObject.AddComponent<MeshFilter>();
        MeshRenderer goldMeshRenderer = goldObject.AddComponent<MeshRenderer>();
        //goldObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        goldMeshFilter.mesh = goldMesh;
        goldMeshRenderer.material = goldMaterial;

        goldObject.transform.parent = gameObject.GetComponent<Transform>();
        goldObject.transform.localPosition = goldStartPos;
        goldObject.transform.localScale = Vector3.one;
    }

    private void addGemstonesToGold(float[,] heightArray)
    {
        byte gemID_1 = (byte)Random.Range(0, 8);
        byte gemID_2;
        byte gemID_3;

        do {
            gemID_2 = (byte)Random.Range(0, 8);
        } while (gemID_2 == gemID_1);

        do {
            gemID_3 = (byte)Random.Range(0, 8);
        } while (gemID_3 == gemID_1 || gemID_3 == gemID_2);


        GameObject gemObj_1 = getGemFromID(gemID_1);
        GameObject gemObj_2 = getGemFromID(gemID_2);
        GameObject gemObj_3 = getGemFromID(gemID_3);

        applyBasicTransformsToGem(gemObj_1);
        applyBasicTransformsToGem(gemObj_2);
        applyBasicTransformsToGem(gemObj_3);
        

        byte[] gemCoords1 = new byte[2];
        byte[] gemCoords2 = new byte[2];
        byte[] gemCoords3 = new byte[2];

        gemCoords1[0] = (byte)Random.Range(3, goldMeshVerts_horiz-4);
        gemCoords1[1] = (byte)Random.Range(6, goldMeshVerts_verti-4);

        byte breaker = 210;

        do {
            gemCoords2[0] = (byte)Random.Range(3, goldMeshVerts_horiz-4);
            gemCoords2[1] = (byte)Random.Range(6, goldMeshVerts_verti-4);
            breaker--;
        } while (checkIfGemIsTooCloseToPrevious(gemCoords2[0], gemCoords2[1], gemCoords1[0], gemCoords1[1], 200, 200) && breaker > 10);

        breaker = 210;

        do {
            gemCoords3[0] = (byte)Random.Range(3, goldMeshVerts_horiz-4);
            gemCoords3[1] = (byte)Random.Range(6, goldMeshVerts_verti-4);
            breaker--;
        } while (checkIfGemIsTooCloseToPrevious(gemCoords3[0], gemCoords3[1], gemCoords1[0], gemCoords1[1], gemCoords2[0], gemCoords2[1]) && breaker > 10);

        setPositionOfGemstones(gemObj_1, heightArray, gemCoords1[0], gemCoords1[1]);
        setPositionOfGemstones(gemObj_2, heightArray, gemCoords2[0], gemCoords2[1]);
        setPositionOfGemstones(gemObj_3, heightArray, gemCoords3[0], gemCoords3[1]);
    }

    private void applyBasicTransformsToGem(GameObject gemObj)
    {
        gemObj.SetActive(true);
        gemObj.GetComponent<Renderer>().material.SetColor("_Color", getRandomGemColor());
        gemObj.transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        gemObj.transform.localScale *= Random.Range(0.5f, 1f);
    }

    private GameObject getGemFromID(byte gemID)
    {
        switch(gemID)
        {
            case 0: return gemstone1;
            case 1: return gemstone2;
            case 2: return gemstone3;
            case 3: return gemstone4;
            case 4: return gemstone5;
            case 5: return gemstone6;
            case 6: return gemstone7;
            default: return gemstone8;
        }
    }

    private Color32 getRandomGemColor()
    {
        byte greyVal;

        switch (Random.Range(0, 7))
        {
            case 0: //red:
                return new Color32((byte)Random.Range(200, 255), (byte)Random.Range(0, 50), (byte)Random.Range(0, 50), 220);
            case 1: //green:
            case 2: //green:
                return new Color32((byte)Random.Range(0, 50), (byte)Random.Range(200, 255), (byte)Random.Range(0, 50), 220);
            case 3: //blue:
                return new Color32((byte)Random.Range(75, 125), (byte)Random.Range(75, 125), (byte)Random.Range(200, 255), 220);
            case 4: //cyans:
                return new Color32((byte)Random.Range(0, 50), (byte)Random.Range(200, 255), (byte)Random.Range(200, 255), 220);
            case 5: //magentas:
                return new Color32((byte)Random.Range(200, 255), (byte)Random.Range(0, 50), (byte)Random.Range(200, 255), 220);
            case 6: //white:
                greyVal = (byte)Random.Range(200, 255);
                return new Color32(greyVal, greyVal, greyVal, 220);
            default: //dark:
                return new Color32((byte)Random.Range(50, 100), (byte)Random.Range(50, 100), (byte)Random.Range(50, 100), 220);
        }


    }

    private bool checkIfGemIsTooCloseToPrevious(byte x1, byte y1, byte x2, byte y2, byte x3, byte y3)
    {
        return (Mathf.Abs(x1 - x2) < gemRadius && Mathf.Abs(y1 - y2) < gemRadius) || 
                    (Mathf.Abs(x1 - x3) < gemRadius && Mathf.Abs(y1 - y3) < gemRadius);
    }

    private void setPositionOfGemstones(GameObject gemObj, float[,] heightArray, byte gemX, byte gemY)
    {
        gemObj.transform.localPosition = goldStartPos + new Vector3(gemX * widthBetweenMeshVerts, heightArray[gemX, gemY], gemY * -widthBetweenMeshVerts);
    }






    void Update()
    {
        if (shouldPlayAnimation && animationIsOver)
        {
            switch (animationState)
            {
                case 0:
                    StartCoroutine("chestAnimation_1_scrunchDown");
                    break;
                    
                case 1:
                    StartCoroutine("chestAnimation_delay1_pauseDuringScrunch");
                    break;
                    
                case 2:
                    StartCoroutine("chestAnimation_2_3_leanParentBack");
                    StartCoroutine("chestAnimation_2_bounceUp");
                    break;
                    
                case 3:
                    StartCoroutine("chestAnimation_3_lidSwingsOpen");
                    break;
                    
                case 4:
                    StartCoroutine("chestAnimation_4_lidBounces");
                    break;
                    
                case 5:
                    StartCoroutine("chestAnimation_5_lidEases");
                    break;
                    
                case 6:
                    StartCoroutine("chestAnimation_6_delayBeforeStats");
                    break;

                default:
                    victoryScreen.beginShowingStats();
                    break;
            }
            
            animationIsOver = false;
            animationState++;
        }
    }


    IEnumerator chestAnimation_1_scrunchDown()
    {
        float totalOffset = 1f - lowestScrunchHeight;
        float sinInc = Mathf.PI /animFrames_1;

        for (int index = 0; index<animFrames_0; index++) 
        {
            yield return null;
        }

        for (int index = 0; index<animFrames_1; index++) 
        {
            chestTransform_parent.localScale = new Vector3(1f, 1f - totalOffset * (Mathf.Sin(index * sinInc -Mathf.PI/2f) + 1f)/2f, 1f);
            chestTransform_parent.localEulerAngles = new Vector3(totalLeanAmount * (Mathf.Sin(index * sinInc-Mathf.PI/2f) + 1f)/2f, 0f, 0f);
            yield return null;
        }

        chestTransform_parent.localScale = new Vector3(1f, lowestScrunchHeight, 1f);
        chestTransform_parent.localEulerAngles = new Vector3(totalLeanAmount, 0f, 0f);

        animationIsOver = true;
    }

    IEnumerator chestAnimation_delay1_pauseDuringScrunch()
    {
        for (int index = 0; index<animFrames_delay1; index++) 
        {
            yield return null;
        }

        animationIsOver = true;
    }

    IEnumerator chestAnimation_2_bounceUp()
    {
        // float totalOffset = 1f - lowestScrunchHeight;
        // float sinInc = Mathf.PI /2f /animFrames_2;

        for (int index = 0; index<animFrames_2; index++) 
        {
            yield return null;
        }

        animationIsOver = true;
    }

    IEnumerator chestAnimation_2_3_leanParentBack()
    {
        byte animFrames_leaning = animFrames_2 + animFrames_3_lidMovement;
        float totalOffset = 1f - lowestScrunchHeight;
        float sinInc = Mathf.PI *3f/2f/ animFrames_leaning;

        for (int index = 0; index<animFrames_leaning; index++) 
        {
            chestTransform_parent.localEulerAngles = new Vector3(totalLeanAmount * Mathf.Sin(index * sinInc + Mathf.PI/2f), 0f, 0f);
            chestTransform_parent.localScale = new Vector3(1f, 1f - totalOffset * (Mathf.Sin(index * sinInc + Mathf.PI/2f)), 1f);
            yield return null;
        }

        chestTransform_parent.localEulerAngles = new Vector3(0f, 0f, 0f);
        chestTransform_parent.localScale = new Vector3(1f, 1f, 1f);

        //animationIsOver = true;
    }

    IEnumerator chestAnimation_3_lidSwingsOpen()
    {
        float amountToRotate = lidMaxAngle/animFrames_3_lidMovement;

        for (int index = 0; index<animFrames_3_lidMovement; index++) 
        {
            chestTransform_lid.localEulerAngles = new Vector3(amountToRotate * index, 0f, 0f);
            yield return null;
        }

        chestTransform_lid.localEulerAngles = new Vector3(lidMaxAngle, 0f, 0f);

        animationIsOver = true;
    }

    IEnumerator chestAnimation_4_lidBounces()
    {
        float totalOffset = lidMaxAngle - lidReboundAngle;
        float sinInc = Mathf.PI /2f /animFrames_4;

        for (int index = 0; index<animFrames_4; index++) 
        {
            chestTransform_lid.localEulerAngles = new Vector3(lidMaxAngle - totalOffset * Mathf.Sin(index * sinInc), 0f, 0f);
            yield return null;
        }

        chestTransform_lid.localEulerAngles = new Vector3(lidReboundAngle, 0f, 0f);

        animationIsOver = true;
    }

    IEnumerator chestAnimation_5_lidEases()
    {
        float totalOffset = lidMaxAngle - lidReboundAngle;
        float sinInc = Mathf.PI /2f /animFrames_5;

        for (int index = 0; index<animFrames_5; index++) 
        {
            chestTransform_lid.localEulerAngles = new Vector3(lidMaxAngle - totalOffset * Mathf.Sin(index * sinInc + Mathf.PI/2f), 0f, 0f);
            yield return null;
        }

        chestTransform_lid.localEulerAngles = new Vector3(lidMaxAngle, 0f, 0f);

        animationIsOver = true;
    }

    IEnumerator chestAnimation_6_delayBeforeStats()
    {
        for (int index = 0; index<animFrames_after; index++) 
        {
            yield return null;
        }

        animationIsOver = true;
    }
}
