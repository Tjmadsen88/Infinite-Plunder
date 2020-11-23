using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainColorGeneratorSpace
{
    public static class TerrainColorGenerator
    {


        public static Color32[] GenerateTerrainColors()
        {
            // This will return four colors. These are the water, sand, first grass, and higher grass.

            //return GenerateColors_desert();


            switch (Random.Range(0, 20))
            {
                case 0:
                    return GenerateColors_desert();
                    
                case 1:
                    return GenerateColors_unusual();

                case 2:
                case 3:
                case 4:
                case 5:
                    return GenerateColors_snowy();

                case 6:
                case 7:
                case 8:
                case 9:
                    return GenerateColors_autumn();

                default:
                    return GenerateColors_greens();
            }
        }




        private static Color32 getSandColor_normal()
        {
            return new Color32(172, 176, 145, 255);
        }

        private static Color32 getWaterColor_normal()
        {
            // Debugging some colors... temp:
            /* if we want all the 'end' combinations, then... there would be eight for each one. These... would be:

                normal:           ->   when Lerp'd with normal sand:
            000:	0, 150, 150   ->   86, 163, 147   (+ could be darker, perhaps?)
            001:	0, 150, 255   ->   86, 163, 200   (  good bound... maybe a little intensly blue?)
            010:	0, 255, 150   ->   86, 215, 147   (  good bound... although, quite an intense green...)
            011:	0, 255, 255   ->   86, 215, 200   (- perhaps a little too intense now.)
            100:	120, 150, 150   ->   146, 163, 147   (  good... although the greyness might be a bit boring)
            101:	120, 150, 255   ->   146, 163, 200   (- the blue is a little odd looking... looks more like sidewalk chalk than water?)
            110:	120, 255, 150   ->   146, 215, 147   (- the green looks a little weird... again, like chalk instead of water)
            111:	120, 255, 255   ->   146, 215, 200   (  very good! no complaints)
                NOTES: Barring the last one, every instance of 255 appears to be too intense. I should probably lower that...
                        I could also play around with lowering red to lower than I could previously...
                The Lerp'd range appears to be: (86-146, 163-215, 147-200);

                snowy:
            000:	80, 180, 180   ->   126, 178, 162  (- possibly too dark and too blue for snowy waters...)
            001:	80, 180, 220   ->   126, 178, 182  (- possibly too blue for snowy waters?)
            010:	80, 220, 180   ->   126, 198, 163  (  good. might be a little greeny though)
            011:	80, 220, 220   ->   126, 198, 182  (  great. No complaints.)
            100:	120, 180, 180   ->   146, 178, 162  (- seems a little too dark for the snow?)
            101:	120, 180, 220   ->   146, 178, 182  (  good, no complaints.)
            110:	120, 220, 180   ->   146, 198, 162  (- should probably dial back the green...)
            111:	120, 220, 220   ->   146, 198, 182  (+ I should make this lighter...)
                NOTES: it seems like I need to dial back on the green a little bit, and make the colors overall a bit lighter on both ends.
                The Lerp'd range appears to be: (126-146, 178-198, 162-182);

                muddy: (assuming the smaller values are 0 and the larger ones 1)
            000:	190, 160, 80   ->   181, 168, 112  (--Waay to intensly yellow! I need to dial that back)
            001:	190, 160, 130   ->   181, 168, 137  (Excellent! No complaints)
            010:	190, 180, 100   ->   181, 178, 122  (- Hmm... a little too yellow. Maybe not all that bad though...?)
            011:	190, 180, 150   ->   181, 178, 147  (  Very good! No complaints)
            100:	215, 160, 80   ->   193, 168, 112  (- a little too intensly colored perhaps...)
            101:	215, 160, 130   ->   193, 168, 137  (  quite good! No complaints)
            110:	215, 180, 100   ->   193, 178, 122  (- too yellow again.)
            111:	215, 180, 150   ->   193, 178, 147  (  pretty good... a little light, but nothing too agregious)
                NOTES: every lower-end bound for the blues was too much. I should raise that...
                        Otherwise, the upper bound on blue has been consistently good!
                The Lerp'd range here... is incorrect for the 215's. However, this appears to be: (181-193, 168-178, green <minus> (30-80));

                muddy (corrected):
            100:	215, 185, 105   ->   193, 180, 125  (  pretty good! wouldn't hurt if the yellow were dialed back...)
            101:	215, 185, 155   ->   193, 180, 150  (  quite good! could be darker, I suppose)
            110:	215, 205, 125   ->   193, 190, 135  (  not bad... but would't hurt if the yellow was dialed back a bit?)
            111:	215, 205, 175   ->   193, 190, 160  (--Waay too light! blends in with the sand too much)
                NOTES: The yellow-ness is more controlled now, but... I think I like the previous version better anyways?
                NOTES: Consider trying: (190-215, 160-180, 120-150) -> (181-193, 168-178, 132-147);

            */

            //return new Color32(180, 190, 145, 255);
            //return getWaterColor_normal_noMud();

            if (Random.Range(0, 10) != 0) return getWaterColor_normal_noMud();

            return getWaterColor_muddy();
        }

        private static Color32 getWaterColor_normal_noMud()
        {
            return new Color32((byte)(Random.Range(85, 145)), (byte)(Random.Range(160, 215)), (byte)(Random.Range(145, 200)), 255);
        }

        private static Color32 getWaterColor_snowy()
        {
            return new Color32((byte)(Random.Range(160, 180)), (byte)(Random.Range(190, 210)), (byte)(Random.Range(190, 210)), 255);
        }

        private static Color32 getWaterColor_muddy()
        {
            return new Color32((byte)(Random.Range(180, 200)), (byte)(Random.Range(170, 180)), (byte)(Random.Range(125, 145)), 255);
        }

        private static Color32 getWaterColor_desert()
        {
            if (Random.Range(0, 5) != 0) return getWaterColor_muddy();

            return getWaterColor_normal_noMud();
        }



        private static Color32[] GenerateColors_greens()
        {
            Color32[] returnColors = new Color32[4];

            byte grassGreen = (byte)Random.Range(100, 150);
            byte grassRed = (byte)Random.Range(50, grassGreen);
            byte grassBlue = (byte)Random.Range(50, grassGreen-30);
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }




        private static Color32[] GenerateColors_autumn()
        {
            switch(Random.Range(0, 6))
            {
                case 0: return GenerateColors_autumn_reds();
                case 1: return GenerateColors_autumn_browns();
                default: return GenerateColors_autumn_oranges();
            }
        }

        private static Color32[] GenerateColors_autumn_reds()
        {
            Color32[] returnColors = new Color32[4];

            byte grassRed = (byte)Random.Range(130, 170);
            byte grassGreen = (byte)Random.Range(40, 80);
            byte grassBlue = (byte)(grassGreen-Random.Range(0, 20));
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_autumn_oranges()
        {
            Color32[] returnColors = new Color32[4];

            byte grassRed = (byte)Random.Range(120, 180);
            byte grassGreen = (byte)(grassRed - Random.Range(50, 80));
            byte grassBlue = (byte)Random.Range(30, 60);
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_autumn_browns()
        {
            Color32[] returnColors = new Color32[4];

            byte grassRed = (byte)Random.Range(65, 120);
            byte grassGreen = (byte)Random.Range(45, grassRed-20);
            byte grassBlue = (byte)Random.Range(25, grassGreen-20);
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }




        private static Color32[] GenerateColors_snowy()
        {
            switch(Random.Range(0, 2))
            {
                case 0: return GenerateColors_snowy_white();
                default: return GenerateColors_snowy_colored();
            }
        }

        private static Color32[] GenerateColors_snowy_white()
        {
            Color32[] returnColors = new Color32[4];

            byte greyVal = (byte)Random.Range(185, 205);
            byte lighteningVal = 20;

            returnColors[0] = getWaterColor_snowy();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(greyVal, greyVal, greyVal, 255);
            returnColors[3] = new Color32((byte)(greyVal+lighteningVal), (byte)(greyVal+lighteningVal), (byte)(greyVal+lighteningVal), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_snowy_colored()
        {
            Color32[] returnColors = new Color32[4];

            byte grassGreen = (byte)Random.Range(105, 165);
            byte grassRed = (byte)Random.Range(85, 145);
            byte grassBlue = (byte)(grassGreen - Random.Range(20, 80));
            byte snowColor = 185;

            returnColors[0] = getWaterColor_snowy();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32(snowColor, snowColor, snowColor, 255);

            return returnColors;
        }


        

        private static Color32[] GenerateColors_desert()
        {
            switch(Random.Range(0, 2))
            {
                case 0: return GenerateColors_desert_sandy();
                default: return GenerateColors_desert_someGrass();
            }
        }

        private static Color32[] GenerateColors_desert_sandy()
        {
            Color32[] returnColors = new Color32[4];

            returnColors[0] = getWaterColor_desert();
            returnColors[1] = getSandColor_normal();

            byte grassRed = returnColors[1].r;
            byte grassGreen = returnColors[1].g;
            byte grassBlue = returnColors[1].b;
            float darkeningVal = Random.Range(1.05f, 1.1f);

            returnColors[2] = new Color32((byte)(grassRed*darkeningVal), (byte)(grassGreen*darkeningVal), (byte)(grassBlue*darkeningVal), 255);
            returnColors[3] = new Color32((byte)(grassRed*darkeningVal*darkeningVal), (byte)(grassGreen*darkeningVal*darkeningVal), (byte)(grassBlue*darkeningVal*darkeningVal), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_desert_someGrass()
        {
            Color32[] returnColors = new Color32[4];

            returnColors[0] = getWaterColor_desert();
            returnColors[1] = getSandColor_normal();

            byte grassRed = returnColors[1].r;
            byte grassGreen = returnColors[1].g;
            byte grassBlue = returnColors[1].b;
            float darkeningVal = Random.Range(1.05f, 1.1f);

            returnColors[2] = new Color32((byte)(grassRed*darkeningVal), (byte)(grassGreen*darkeningVal), (byte)(grassBlue*darkeningVal), 255);

            grassGreen = (byte)Random.Range(105, 175);
            grassRed = (byte)(grassGreen - Random.Range(10, 80));
            grassBlue = (byte)(grassGreen - Random.Range(20, 80));

            returnColors[3] = new Color32(grassRed, grassGreen, grassBlue, 255);

            return returnColors;
        }



        private static Color32[] GenerateColors_unusual()
        {
            switch(Random.Range(0, 3))
            {
                case 0: return GenerateColors_greys();
                // case 1: return GenerateColors_blues();
                default: return GenerateColors_purples();
            }
        }


        private static Color32[] GenerateColors_greys()
        {
            Color32[] returnColors = new Color32[4];

            byte greyVal = (byte)Random.Range(90, 150);
            byte darkeningVal = (byte)Random.Range(10, 30);
            returnColors[0] = new Color32(greyVal, greyVal, (byte)(greyVal - darkeningVal), 255);

            greyVal = (byte)Random.Range(50, 80);
            darkeningVal = 20;

            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(greyVal, greyVal, greyVal, 255);
            returnColors[3] = new Color32((byte)(greyVal-darkeningVal), (byte)(greyVal-darkeningVal), (byte)(greyVal-darkeningVal), 255);

            return returnColors;
        }



        
        private static Color32[] GenerateColors_purples()
        {
            Color32[] returnColors = new Color32[4];

            byte grassGreen = (byte)Random.Range(50, 130);
            byte grassRed = (byte)(grassGreen + Random.Range(10, 50));
            byte grassBlue = (byte)(grassGreen + Random.Range(0, 20));
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal_noMud();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }
        
        private static Color32[] GenerateColors_blues()
        {
            Color32[] returnColors = new Color32[4];

            byte grassGreen = (byte)Random.Range(70, 145);
            byte grassRed = (byte)(grassGreen -30);
            byte grassBlue = (byte)(grassGreen + Random.Range(10, 30));
            byte darkeningVal = 20;

            returnColors[0] = getWaterColor_normal_noMud();
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32(grassRed, grassGreen, grassBlue, 255);
            returnColors[3] = new Color32((byte)(grassRed-darkeningVal), (byte)(grassGreen-darkeningVal), (byte)(grassBlue-darkeningVal), 255);

            return returnColors;
        }






        

        private static Color32[] GenerateColors_candyMountain()
        {
            return new Color32[4];
        }


        private static Color32[] GenerateColors_hell()
        {
            switch(Random.Range(0, 2))
            {
                case 0: return GenerateColors_hellRed();
                default: return GenerateColors_hellBlack();
            }
        }

        private static Color32[] GenerateColors_hellBlack()
        {
            Color32[] returnColors = new Color32[4];


            byte blackVal = (byte)Random.Range(0, 101);
            byte lighteningVal = 20;

            returnColors[0] = new Color32((byte)Random.Range(120, 150), (byte)Random.Range(0, 50), 0, 255);
            returnColors[1] = new Color32(blackVal, blackVal, blackVal, 255);
            returnColors[2] = new Color32((byte)(blackVal+lighteningVal), (byte)(blackVal+lighteningVal), (byte)(blackVal+lighteningVal), 255);
            returnColors[3] = new Color32((byte)(blackVal+lighteningVal*2), (byte)(blackVal+lighteningVal*2), (byte)(blackVal+lighteningVal*2), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_hellRed()
        {
            Color32[] returnColors = new Color32[4];

            byte greyVal = (byte)Random.Range(50, 80);
            byte redVal = (byte)Random.Range(100, 180);
            byte greenVal = (byte)Random.Range(0, 50);
            byte adjustmentVal = 20;

            returnColors[0] = new Color32(greyVal, greyVal, greyVal, 255);
            returnColors[1] = new Color32(redVal, greenVal, 0, 255);
            returnColors[2] = new Color32((byte)(redVal-adjustmentVal), (byte)(greenVal+adjustmentVal), adjustmentVal, 255);
            returnColors[3] = new Color32((byte)(redVal-adjustmentVal*2), (byte)(greenVal+adjustmentVal*2), (byte)(adjustmentVal*2), 255);

            return returnColors;
        }

        private static Color32[] GenerateColors_trueRandom()
        {
            Color32[] returnColors = new Color32[4];

            returnColors[0] = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
            // returnColors[1] = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
            returnColors[1] = getSandColor_normal();
            returnColors[2] = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
            returnColors[3] = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 128);

            return returnColors;
        }


    }
}