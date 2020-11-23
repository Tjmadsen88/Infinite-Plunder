using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ShipDataIndividual
{
    private string shipName;
    private byte sailPatternSelection;
    private bool sailIsMirrored_horizontal;
    private bool sailIsMirrored_vertical;
    private byte[] color_SailPrimary = new byte[3];
    private byte[] color_SailPattern = new byte[3];
    private byte[] color_MastAndDeck = new byte[3];
    private byte[] color_Rails = new byte[3];
    private byte[] color_Hull = new byte[3];
    private byte cannonSelection;
    private byte[] color_Cannonball = new byte[3];


    public void setShipName(string shipName)
    {
        this.shipName = shipName;
    }

    public void setSailPatternSelection(byte sailPatternSelection)
    {
        this.sailPatternSelection = sailPatternSelection;
    }

    public void setSailIsMirrored(bool sailIsMirrored_horizontal, bool sailIsMirrored_vertical)
    {
        this.sailIsMirrored_horizontal = sailIsMirrored_horizontal;
        this.sailIsMirrored_vertical = sailIsMirrored_vertical;
    }

    public void setColor_SailPrimary(Color32 color_SailPrimary)
    {
        this.color_SailPrimary[0] = color_SailPrimary.r;
        this.color_SailPrimary[1] = color_SailPrimary.g;
        this.color_SailPrimary[2] = color_SailPrimary.b;
    }

    public void setColor_SailPattern(Color32 color_SailPattern)
    {
        this.color_SailPattern[0] = color_SailPattern.r;
        this.color_SailPattern[1] = color_SailPattern.g;
        this.color_SailPattern[2] = color_SailPattern.b;
    }

    public void setColor_MastAndDeck(Color32 color_MastAndDeck)
    {
        this.color_MastAndDeck[0] = color_MastAndDeck.r;
        this.color_MastAndDeck[1] = color_MastAndDeck.g;
        this.color_MastAndDeck[2] = color_MastAndDeck.b;
    }

    public void setColor_Rails(Color32 color_Rails)
    {
        this.color_Rails[0] = color_Rails.r;
        this.color_Rails[1] = color_Rails.g;
        this.color_Rails[2] = color_Rails.b;
    }

    public void setColor_Hull(Color32 color_Hull)
    {
        this.color_Hull[0] = color_Hull.r;
        this.color_Hull[1] = color_Hull.g;
        this.color_Hull[2] = color_Hull.b;
    }

    public void setCannonSelection(byte cannonSelection)
    {
        this.cannonSelection = cannonSelection;
    }

    public void setColor_Cannonball(Color32 color_Cannonball)
    {
        this.color_Cannonball[0] = color_Cannonball.r;
        this.color_Cannonball[1] = color_Cannonball.g;
        this.color_Cannonball[2] = color_Cannonball.b;
    }


    public string getShipName()
    {
        return shipName;
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

    public Color32 getColor_SailPrimary()
    {
        // return new Color32(color_SailPrimary[0], color_SailPrimary[1], color_SailPrimary[2], 255);
        return new Color32(color_SailPrimary[0], color_SailPrimary[1], color_SailPrimary[2], 13);
    }

    public Color32 getColor_SailPattern()
    {
        return new Color32(color_SailPattern[0], color_SailPattern[1], color_SailPattern[2], 13);
    }

    public Color32 getColor_MastAndDeck()
    {
        return new Color32(color_MastAndDeck[0], color_MastAndDeck[1], color_MastAndDeck[2], 13);
    }

    public Color32 getColor_Rails()
    {
        return new Color32(color_Rails[0], color_Rails[1], color_Rails[2], 13);
    }

    public Color32 getColor_Hull()
    {
        return new Color32(color_Hull[0], color_Hull[1], color_Hull[2], 13);
    }

    public byte getCannonSelection()
    {
        return cannonSelection;
    }

    public Color32 getColor_Cannonball()
    {
        return new Color32(color_Cannonball[0], color_Cannonball[1], color_Cannonball[2], 255);
    }
}
