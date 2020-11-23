using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using ConstantsSpace;

[Serializable]
public class CustomizationPacket
{
    private byte money_selection;
    private byte enemies_selection;
    private byte keys_selection;
    private byte area_selection;
    
    private int money_value;
    private float enemies_value;
    private byte keys_value;
    private byte area_value;

    private int money_custom;
    private float enemies_custom;
    private byte keys_custom;
    private byte area_custom;

    public CustomizationPacket() 
    {
        money_selection = 2;
        enemies_selection = 3;
        keys_selection = 2;
        area_selection = 3;

        money_value = Constants.getCustomization_StartingMoney(money_selection);
        enemies_value = Constants.getCustomization_DensityOfEnemeies(enemies_selection);
        keys_value = Constants.getCustomization_NumberOfKeys(keys_selection);
        area_value = Constants.getCustomization_SizeOfArea(area_selection);

        money_custom = 375000;
        enemies_custom = 1.5f;
        keys_custom = 3;
        area_custom = 1;
    }


    // Selection stuff:
    public void setMoney_selection(byte money_selection)
    {
        this.money_selection = money_selection;
    }

    public void setEnemies_selection(byte enemies_selection)
    {
        this.enemies_selection = enemies_selection;
    }

    public void setKeys_selection(byte keys_selection)
    {
        this.keys_selection = keys_selection;
    }

    public void setArea_selection(byte area_selection)
    {
        this.area_selection = area_selection;
    }

    public byte getMoney_selection()
    {
        return money_selection;
    }

    public byte getEnemies_selection()
    {
        return enemies_selection;
    }

    public byte getKeys_selection()
    {
        return keys_selection;
    }

    public byte getArea_selection()
    {
        return area_selection;
    }

    
    // Value stuff:
    public void setMoney_value(int money_value)
    {
        this.money_value = money_value;
    }

    public void setEnemies_value(float enemies_value)
    {
        this.enemies_value = enemies_value;
    }

    public void setKeys_value(byte keys_value)
    {
        this.keys_value = keys_value;
    }

    public void setArea_value(byte area_value)
    {
        this.area_value = area_value;
    }

    public int getMoney_value()
    {
        return money_value;
    }

    public float getEnemies_value()
    {
        return enemies_value;
    }

    public byte getKeys_value()
    {
        return keys_value;
    }

    public byte getArea_value()
    {
        return area_value;
    }

    
    // Custom stuff:
    public void setMoney_custom(int money_custom)
    {
        this.money_custom = money_custom;
    }

    public void setEnemies_custom(float enemies_custom)
    {
        this.enemies_custom = enemies_custom;
    }

    public void setKeys_custom(byte keys_custom)
    {
        this.keys_custom = keys_custom;
    }

    public void setArea_custom(byte area_custom)
    {
        this.area_custom = area_custom;
    }

    public int getMoney_custom()
    {
        return money_custom;
    }

    public float getEnemies_custom()
    {
        return enemies_custom;
    }

    public byte getKeys_custom()
    {
        return keys_custom;
    }

    public byte getArea_custom()
    {
        return area_custom;
    }
}
