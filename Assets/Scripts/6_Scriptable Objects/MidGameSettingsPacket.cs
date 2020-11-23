using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using ConstantsSpace;

[Serializable]
public class MidGameSettingsPacket
{
    private bool normalStickPositions;
    private byte shootingMode;

    public MidGameSettingsPacket() 
    {
        normalStickPositions = true;
        shootingMode = Constants.shootingMode_assisted;
    }

    public MidGameSettingsPacket(bool normalStickPositions, byte shootingMode)
    {
        this.normalStickPositions = normalStickPositions;
        this.shootingMode = shootingMode;
    }


    public void setNormalStickPositions(bool normalStickPositions)
    {
        this.normalStickPositions = normalStickPositions;
    }

    public void setShootingMode(byte shootingMode)
    {
        this.shootingMode = shootingMode;
    }

    public bool getNormalStickPositions()
    {
        return normalStickPositions;
    }

    public byte getShootingMode()
    {
        return shootingMode;
    }
}
