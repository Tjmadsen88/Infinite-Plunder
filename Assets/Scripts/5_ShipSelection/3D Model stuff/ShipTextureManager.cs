using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTextureManager : MonoBehaviour
{
    public Material shipAndLootMaterial;
    public Material sailMaterial;

    public Texture2D sailPattern_01;
    public Texture2D sailPattern_02;
    public Texture2D sailPattern_03;
    public Texture2D sailPattern_04;
    public Texture2D sailPattern_05;
    public Texture2D sailPattern_06;
    public Texture2D sailPattern_07;
    public Texture2D sailPattern_08;
    public Texture2D sailPattern_09;
    public Texture2D sailPattern_10;
    public Texture2D sailPattern_11;
    public Texture2D sailPattern_12;
    public Texture2D sailPattern_13;
    public Texture2D sailPattern_14;
    public Texture2D sailPattern_15;
    public Texture2D sailPattern_16;
    public Texture2D sailPattern_17;
    public Texture2D sailPattern_18;
    public Texture2D sailPattern_19;
    public Texture2D sailPattern_20;
    public Texture2D sailPattern_21;
    public Texture2D sailPattern_none;

    private Texture2D currentSailPattern;

    public PersistentData persistentData;

    private Texture2D shipTexture;
    private Texture2D sailTexture;

    private Color[] sailPixels;
    private Color[] sailPixels_copy;
    private int sailPixelsLength;

    private const byte patternWidth = 150;
    private const byte patternHeight = 100;
    
    private int tempIndexY;
    private int tempIndexY_mirrored;

    private bool finishedSettingUp = false;

    private Color color_hullBackMult = new Color(0.8f, 0.8f, 0.8f, 1f);
    private Color color_railDarkMult = new Color(0.85f, 0.85f, 0.85f, 1f);


    // Start is called before the first frame update
    void Start()
    {
        // initializeShipMaterial();
        // initializeSailMaterial();
        // finishedSettingUp = true;
    }


    public void initializeShipMaterials(Color32[] shipColors, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical)
    {
        // Initialize the ship's texture:
        Color[] textureColors = new Color[9];

        shipTexture = new Texture2D(3, 3);
        shipTexture.wrapMode = TextureWrapMode.Clamp;
        shipTexture.filterMode = FilterMode.Point;

        shipAndLootMaterial.mainTexture = shipTexture;


        // Initialize the ship's sail material:
        // Color[] sailColors = getSailPatternFromSelection(sailPatternSelection);

        sailTexture = new Texture2D(patternWidth, patternHeight);
        sailTexture.wrapMode = TextureWrapMode.Clamp;
        sailTexture.filterMode = FilterMode.Bilinear;

        sailMaterial.mainTexture = sailTexture;

        finishedSettingUp = true;

        // Then set both of their colors:
        changeColors(shipColors, sailPatternSelection, sailIsMirrored_horizontal, sailIsMirrored_vertical);
    }


    private Texture2D getSailPatternFromSelection(byte sailPatternSelection)
    {
        switch(sailPatternSelection)
        {
            case 0:
                return sailPattern_01;
                
            case 1:
                return sailPattern_02;
                
            case 2:
                return sailPattern_03;
                
            case 3:
                return sailPattern_04;
                
            case 4:
                return sailPattern_05;
                
            case 5:
                return sailPattern_06;
                
            case 6:
                return sailPattern_07;
                
            case 7:
                return sailPattern_08;
                
            case 8:
                return sailPattern_09;
                
            case 9:
                return sailPattern_10;
                
            case 10:
                return sailPattern_11;
                
            case 11:
                return sailPattern_12;
                
            case 12:
                return sailPattern_13;
                
            case 13:
                return sailPattern_14;
                
            case 14:
                return sailPattern_15;
                
            case 15:
                return sailPattern_16;
                
            case 16:
                return sailPattern_17;
                
            case 17:
                return sailPattern_18;
                
            case 18:
                return sailPattern_19;
                
            case 19:
                return sailPattern_20;
                
            case 20:
                return sailPattern_21;
                
            default:
                return sailPattern_none;
        }
    }




    
    public void changeColors(Color32[] shipColors, byte sailPatternSelection, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical)
    {
        if (!finishedSettingUp) return;
        
        Color[] textureColors = new Color[9];

        // First row:
        textureColors[6] = shipColors[4];  // Hull walls
        // textureColors[7] = (Color)shipColors[4] * 0.8f;  // Hull back
        textureColors[7] = (Color)shipColors[4] * color_hullBackMult;  // Hull back
        textureColors[8] = shipColors[0];  // Sail main
        // Second row:
        textureColors[3] = shipColors[3];  // Rails (lighter)
        // textureColors[4] = (Color)shipColors[3] * 0.85f;  // Rails (darker)
        textureColors[4] = (Color)shipColors[3] * color_railDarkMult;  // Rails (darker)
        textureColors[5] = shipColors[2];   // Mast and ship deck
        // Third row:
        // textureColors[0] = new Color32(236, 207, 109, 13);  // Gold
        textureColors[0] = new Color32(236, 207, 109, 145);  // Gold
        textureColors[1] = new Color32(98, 98, 98, 179);     // Cannon (lighter)
        textureColors[2] = new Color32(0, 0, 0, 179);        // Cannon (darker)

        shipTexture.SetPixels(textureColors);
        shipTexture.Apply();


        currentSailPattern = getSailPatternFromSelection(sailPatternSelection);

        updateSailTexture(shipColors[0], shipColors[1], sailIsMirrored_horizontal, sailIsMirrored_vertical);
    }


    private void updateSailTexture(Color32 sailPrimaryColor, Color32 sailPatternColor, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical)
    {
        sailPixels = currentSailPattern.GetPixels();
        sailPixelsLength = sailPixels.Length;

        if (!sailIsMirrored_horizontal && !sailIsMirrored_vertical)
        {
            setSailPixels_mirrorNone(sailPrimaryColor, sailPatternColor);
        } else if (sailIsMirrored_horizontal && !sailIsMirrored_vertical)
        {
            setSailPixels_mirrorHorizontal(sailPrimaryColor, sailPatternColor);
        } else if (!sailIsMirrored_horizontal && sailIsMirrored_vertical)
        {
            setSailPixels_mirrorVertical(sailPrimaryColor, sailPatternColor);
        } else {
            setSailPixels_mirrorBoth(sailPrimaryColor, sailPatternColor);
        }

        sailTexture.SetPixels(sailPixels);
        sailTexture.Apply();
    }

    private void setSailPixels_mirrorNone(Color32 sailPrimaryColor, Color32 sailPatternColor)
    {
        for (int index=0; index<sailPixelsLength; index++)
        {
            if (sailPixels[index] == Color.black)
            {
                sailPixels[index] = sailPrimaryColor;
            } else {
                sailPixels[index] = sailPatternColor;
            }
        }
    }

    private void setSailPixels_mirrorBoth(Color32 sailPrimaryColor, Color32 sailPatternColor)
    {
        sailPixels_copy = new Color[sailPixelsLength];

        for (int index=0; index<sailPixelsLength; index++)
        {
            if (sailPixels[index] == Color.black)
            {
                sailPixels_copy[sailPixelsLength -1 -index] = sailPrimaryColor;
            } else {
                sailPixels_copy[sailPixelsLength -1 -index] = sailPatternColor;
            }
        }

        sailPixels = sailPixels_copy;
    }

    private void setSailPixels_mirrorHorizontal(Color32 sailPrimaryColor, Color32 sailPatternColor)
    {
        sailPixels_copy = new Color[sailPixelsLength];

        for (int indexY = 0; indexY < patternHeight; indexY++)
        {
            tempIndexY = patternWidth * indexY;

            for (int indexX = 0; indexX < patternWidth; indexX++)
            {
                if (sailPixels[indexX + tempIndexY] == Color.black)
                {
                    sailPixels_copy[(patternWidth -1 -indexX) + tempIndexY] = sailPrimaryColor;
                } else {
                    sailPixels_copy[(patternWidth -1 -indexX) + tempIndexY] = sailPatternColor;
                }
            }
        }

        sailPixels = sailPixels_copy;
    }

    private void setSailPixels_mirrorVertical(Color32 sailPrimaryColor, Color32 sailPatternColor)
    {
        sailPixels_copy = new Color[sailPixelsLength];

        for (int indexY = 0; indexY < patternHeight; indexY++)
        {
            tempIndexY = patternWidth * indexY;
            tempIndexY_mirrored = patternWidth * (patternHeight -1 -indexY);

            for (int indexX = 0; indexX < patternWidth; indexX++)
            {
                if (sailPixels[indexX + tempIndexY] == Color.black)
                {
                    sailPixels_copy[indexX + tempIndexY_mirrored] = sailPrimaryColor;
                } else {
                    sailPixels_copy[indexX + tempIndexY_mirrored] = sailPatternColor;
                }
            }
        }

        sailPixels = sailPixels_copy;
    }





    public void adjustSailPattern(byte sailPatternNum, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical,
                                    Color32 primaryColor, Color32 patternColor)
    {
        currentSailPattern = getSailPatternFromSelection(sailPatternNum);
        updateSailTexture(primaryColor, patternColor, sailIsMirrored_horizontal, sailIsMirrored_vertical);
    }


    public void adjustColor_Sails(Color32 primaryColor, Color32 patternColor, bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical)
    {
        // First do the main texture:
        shipTexture.SetPixel(2, 2, primaryColor);
        shipTexture.Apply();

        // Then adjust the sail:
        updateSailTexture(primaryColor, patternColor, sailIsMirrored_horizontal, sailIsMirrored_vertical);
    }


    public void adjustColor_MastAndDeck(Color32 newColor)
    {
        shipTexture.SetPixel(2, 1, newColor);
        shipTexture.Apply();
    }


    public void adjustColor_Rails(Color32 newColor)
    {
        shipTexture.SetPixel(0, 1, newColor);
        shipTexture.SetPixel(1, 1, (Color)newColor * 0.85f);
        shipTexture.Apply();
    }


    public void adjustColor_Hull(Color32 newColor)
    {
        shipTexture.SetPixel(0, 2, newColor);
        shipTexture.SetPixel(1, 2, (Color)newColor * 0.8f);
        shipTexture.Apply();
    }
}
