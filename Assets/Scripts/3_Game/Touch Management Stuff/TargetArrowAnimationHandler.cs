using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetArrowAnimationHandler : MonoBehaviour
{
    public Image targetingArrow;

    public Sprite arrowSprite_01;
    public Sprite arrowSprite_02;
    public Sprite arrowSprite_03;
    public Sprite arrowSprite_04;
    public Sprite arrowSprite_05;
    public Sprite arrowSprite_06;
    public Sprite arrowSprite_07;
    public Sprite arrowSprite_08;
    public Sprite arrowSprite_09;
    public Sprite arrowSprite_10;
    public Sprite arrowSprite_11;
    public Sprite arrowSprite_12;
    public Sprite arrowSprite_13;
    public Sprite arrowSprite_14;
    public Sprite arrowSprite_15;
    public Sprite arrowSprite_16;
    public Sprite arrowSprite_17;
    public Sprite arrowSprite_18;
    public Sprite arrowSprite_19;
    public Sprite arrowSprite_20;
    public Sprite arrowSprite_21;
    public Sprite arrowSprite_22;
    public Sprite arrowSprite_23;
    public Sprite arrowSprite_24;
    public Sprite arrowSprite_25;
    public Sprite arrowSprite_26;
    public Sprite arrowSprite_27;
    public Sprite arrowSprite_28;
    public Sprite arrowSprite_29;
    public Sprite arrowSprite_30;
    public Sprite arrowSprite_31;
    public Sprite arrowSprite_32;
    public Sprite arrowSprite_33;
    public Sprite arrowSprite_34;
    public Sprite arrowSprite_35;
    public Sprite arrowSprite_36;
    public Sprite arrowSprite_37;
    public Sprite arrowSprite_38;
    public Sprite arrowSprite_39;
    public Sprite arrowSprite_40;
    public Sprite arrowSprite_41;
    public Sprite arrowSprite_42;
    public Sprite arrowSprite_43;
    public Sprite arrowSprite_44;
    public Sprite arrowSprite_45;
    public Sprite arrowSprite_46;
    public Sprite arrowSprite_47;
    public Sprite arrowSprite_48;
    public Sprite arrowSprite_49;
    public Sprite arrowSprite_50;
    public Sprite arrowSprite_51;
    public Sprite arrowSprite_52;
    public Sprite arrowSprite_53;
    public Sprite arrowSprite_54;
    public Sprite arrowSprite_55;
    public Sprite arrowSprite_56;
    public Sprite arrowSprite_57;
    public Sprite arrowSprite_58;
    public Sprite arrowSprite_59;
    public Sprite arrowSprite_60;

    private Sprite[] arrowSprites = new Sprite[60];

    private byte currentSprite = 0;



    // Start is called before the first frame update
    void Start()
    {
        arrowSprites[0] = arrowSprite_01;
        arrowSprites[1] = arrowSprite_02;
        arrowSprites[2] = arrowSprite_03;
        arrowSprites[3] = arrowSprite_04;
        arrowSprites[4] = arrowSprite_05;
        arrowSprites[5] = arrowSprite_06;
        arrowSprites[6] = arrowSprite_07;
        arrowSprites[7] = arrowSprite_08;
        arrowSprites[8] = arrowSprite_09;
        arrowSprites[9] = arrowSprite_10;
        arrowSprites[10] = arrowSprite_11;
        arrowSprites[11] = arrowSprite_12;
        arrowSprites[12] = arrowSprite_13;
        arrowSprites[13] = arrowSprite_14;
        arrowSprites[14] = arrowSprite_15;
        arrowSprites[15] = arrowSprite_16;
        arrowSprites[16] = arrowSprite_17;
        arrowSprites[17] = arrowSprite_18;
        arrowSprites[18] = arrowSprite_19;
        arrowSprites[19] = arrowSprite_20;
        arrowSprites[20] = arrowSprite_21;
        arrowSprites[21] = arrowSprite_22;
        arrowSprites[22] = arrowSprite_23;
        arrowSprites[23] = arrowSprite_24;
        arrowSprites[24] = arrowSprite_25;
        arrowSprites[25] = arrowSprite_26;
        arrowSprites[26] = arrowSprite_27;
        arrowSprites[27] = arrowSprite_28;
        arrowSprites[28] = arrowSprite_29;
        arrowSprites[29] = arrowSprite_30;
        arrowSprites[30] = arrowSprite_31;
        arrowSprites[31] = arrowSprite_32;
        arrowSprites[32] = arrowSprite_33;
        arrowSprites[33] = arrowSprite_34;
        arrowSprites[34] = arrowSprite_35;
        arrowSprites[35] = arrowSprite_36;
        arrowSprites[36] = arrowSprite_37;
        arrowSprites[37] = arrowSprite_38;
        arrowSprites[38] = arrowSprite_39;
        arrowSprites[39] = arrowSprite_40;
        arrowSprites[40] = arrowSprite_41;
        arrowSprites[41] = arrowSprite_42;
        arrowSprites[42] = arrowSprite_43;
        arrowSprites[43] = arrowSprite_44;
        arrowSprites[44] = arrowSprite_45;
        arrowSprites[45] = arrowSprite_46;
        arrowSprites[46] = arrowSprite_47;
        arrowSprites[47] = arrowSprite_48;
        arrowSprites[48] = arrowSprite_49;
        arrowSprites[49] = arrowSprite_50;
        arrowSprites[50] = arrowSprite_51;
        arrowSprites[51] = arrowSprite_52;
        arrowSprites[52] = arrowSprite_53;
        arrowSprites[53] = arrowSprite_54;
        arrowSprites[54] = arrowSprite_55;
        arrowSprites[55] = arrowSprite_56;
        arrowSprites[56] = arrowSprite_57;
        arrowSprites[57] = arrowSprite_58;
        arrowSprites[58] = arrowSprite_59;
        arrowSprites[59] = arrowSprite_60;
    }
    
    public void advanceAnimation()
    {
        currentSprite = (byte)((currentSprite + 1) % 60);

        targetingArrow.sprite = arrowSprites[currentSprite];
    }

    public void resetAnimation()
    {
        currentSprite = 0;
        targetingArrow.sprite = arrowSprites[0];
    }

    public void setArrowColor(Color32 arrowColor)
    {
        targetingArrow.color = arrowColor;
    }
}
