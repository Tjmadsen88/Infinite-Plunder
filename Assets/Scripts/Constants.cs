using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BoolArrayManagerSpace;
using DoorHitboxManagerSpace;

namespace ConstantsSpace
{
    public static class Constants
    {
        public const string fileName_customization = "/custFile.DAT";
        public const string fileName_midGamSettings = "/setFile.DAT";
        public const string fileName_shipData = "/shipDataFile.DAT";

        // public const float shipMoveSpeed = 0.18f;
        public const float shipMoveSpeed = 0.3f;
        public const float wallHittestRadius = 1.5f;
        public const float wallRepelSpeed = 0.05f;

        public const byte shootingMode_auto = 0;
        public const byte shootingMode_assisted = 1;
        public const byte shootingMode_manual = 2;

        public const byte gameState_canMove = 0;
        public const byte gameState_movementPaused = 1;
        public const byte gameState_everythingPaused = 2;


        public const byte doorID_left = 0;
        public const byte doorID_up = 1;
        public const byte doorID_down = 2;
        public const byte doorID_right = 3;
        public const byte doorID_null = 4;

        public const byte cornerID_ul = 0;
        public const byte cornerID_ur = 1;
        public const byte cornerID_dl = 2;
        public const byte cornerID_dr = 3;

        // terrain generation:
        // public const byte clearanceLevel_none = 0;
        // public const byte clearanceLevel_red = 1;
        // public const byte clearanceLevel_yellow = 2;
        // public const byte clearanceLevel_green = 3;
        // public const byte clearanceLevel_cyan = 4;
        // public const byte clearanceLevel_blue = 5;
        // public const byte clearanceLevel_magenta = 6;

        public const byte simplified_doorStatus_undefined = 0;
        public const byte simplified_doorStatus_open = 1;
        public const byte simplified_doorStatus_wall = 2;
        public const byte simplified_doorStatus_key1 = 3;
        public const byte simplified_doorStatus_key2 = 4;
        public const byte simplified_doorStatus_key3 = 5;
        public const byte simplified_doorStatus_key4 = 6;
        public const byte simplified_doorStatus_key5 = 7;
        public const byte simplified_doorStatus_key6 = 8;

        // public const byte simplified_containsItem_none = 0;
        // public const byte simplified_containsItem_treasure = 1;
        // public const byte simplified_containsItem_key1 = 2;
        // public const byte simplified_containsItem_key2 = 3;
        // public const byte simplified_containsItem_key3 = 4;
        // public const byte simplified_containsItem_key4 = 5;
        // public const byte simplified_containsItem_key5 = 6;
        // public const byte simplified_containsItem_key6 = 7;

        public const float roomWidthHeight = 30f;
        public const int numOfVertsPerEdge = 50;
        public const float vertDistances = roomWidthHeight / (float)numOfVertsPerEdge;

        public readonly static Vector2[] walls_hittestVectors = { new Vector2(1f, 0f), 
                                                                new Vector2(0.923879533f, 0.382683432f), 
                                                                new Vector2(0.707106781f, 0.707106781f), 
                                                                new Vector2(0.382683432f, 0.923879533f), 
                                                                new Vector2(0f, 1f), 
                                                                new Vector2(-0.382683432f, 0.923879533f), 
                                                                new Vector2(-0.707106781f, 0.707106781f), 
                                                                new Vector2(-0.923879533f, 0.382683432f), 
                                                                new Vector2(-1f, 0f), 
                                                                new Vector2(-0.923879533f, -0.382683432f), 
                                                                new Vector2(-0.707106781f, -0.707106781f), 
                                                                new Vector2(-0.382683432f, -0.923879533f), 
                                                                new Vector2(0f, -1f), 
                                                                new Vector2(0.382683432f, -0.923879533f), 
                                                                new Vector2(0.707106781f, -0.707106781f), 
                                                                new Vector2(0.923879533f, -0.382683432f) };



        // Interactables stuff:
        public const byte interactableID_none = 0;
        public const byte interactableID_key1 = 1;
        public const byte interactableID_key2 = 2;
        public const byte interactableID_key3 = 3;
        public const byte interactableID_key4 = 4;
        public const byte interactableID_key5 = 5;
        public const byte interactableID_key6 = 6;
        public const byte interactableID_door1 = 7;
        public const byte interactableID_door2 = 8;
        public const byte interactableID_door3 = 9;
        public const byte interactableID_door4 = 10;
        public const byte interactableID_door5 = 11;
        public const byte interactableID_door6 = 12;
        public const byte interactableID_treasureSmall = 13;
        public const byte interactableID_treasureLarge = 14;
        public const byte interactableID_treasureCannon = 15;
        public const byte interactableID_treasureShip = 16;
        public const byte interactableID_treasureFinal = 17;
        public const byte interactableID_portTownPier = 18;

        public const byte outOfBoundsKeyPosition = 200;

        public const float footprintRadius_collectables = 2f;
        public const float footprintRadius_collectables_drawIn = 8f;
        public const float footprintRadius_portTownPier = 5f;

        public const float footprintRadius_gateWidth = 10f;
        public const float footprintRadius_gateForward = 5f;

        public const float itemRotationAmount = 1.5f;
        public const float itemAcceleration = 0.014f;
        public const float itemChainMoveSpeed = 0.07f;

        // public const float itemFlying_maxDistance = 2.5f;
        // public const byte itemFlying_duration = 20;
        // public const float itemFlying_sinInc = Mathf.PI / itemFlying_duration;

        
        // Enemy stuff:
        // public const float cannonballAutoTargetingDistance_auto = 10f;
        // public const float cannonballAutoTargetingDistance_assisted = 15f;
        public const float cannonballAutoTargetingDistance_assisted_secondary = 10f;

        public const float cannonballEffectiveDistance_player = 15f;
        // public const float cannonballEffectiveDistance_enemy = 30f;
        public const float cannonballEffectiveDistance_enemy = 27.3f;

        public static readonly Vector3 closestEnemyPosition_null = new Vector3(-200f, -200f, -200f);

        // public const byte enemyID_barrel = 0;
        // public const byte enemyID_box = 1;
        public const byte enemyID_raft = 0;
        public const byte enemyID_ring = 1;
        public const byte enemyID_anchor = 2;
        // public const byte enemyID_vessel = 5;

            // Raft stuff:
        // public const float cannonballEffectiveDistance_raft = 15f;
        public const float cannonTrackSpeed_raft = 0.03f;
        public const byte cannonCooldown_raft_max = 120;
        public const float maxRotationAfterFiring_raft = 5f;

            // Ring stuff:
        public const byte rotationFrames_max = 120;
        public const float rotationAmountPerFrame_min = 30f / rotationFrames_max;
        public const float rotationAmountPerFrame_max = 60f / rotationFrames_max;

            // Anchor stuff:
        public const float cannonTrackSpeed_anchor = 0.01f;
        // public const float cannonTrackSpeed_anchor = 0.0075f;
        // public const byte cannonCooldown_anchor_main_max = 90;
        public const byte cannonCooldown_anchor_main_max = 120;
        // public const byte cannonCooldown_anchor_sub_max = 30;
        public const byte cannonCooldown_anchor_sub_max = 30;



        // Cannon knockback stuff:
        public const float knockback_StartingAmount_player = 0.4f;
        public const float knockback_EndingAmount = 0.005f;
        public const byte knockback_Duration = 30;
        public static readonly float knockback_dragMultiplier_player = Mathf.Pow(knockback_EndingAmount / knockback_StartingAmount_player, 1f / knockback_Duration);
        
        // public const float knockback_StartingAmount_enemy = 0.5f;
        public const float knockback_StartingAmount_enemy = 0.15f;
        public static readonly float knockback_dragMultiplier_enemy = Mathf.Pow(knockback_EndingAmount / knockback_StartingAmount_enemy, 1f / knockback_Duration);

        



        // Minimap stuff:
        public const byte minimapRoomID_none = 0;

        public const byte minimapRoomID_u = 1;
        public const byte minimapRoomID_d = 2;
        public const byte minimapRoomID_l = 3;
        public const byte minimapRoomID_r = 4;

        public const byte minimapRoomID_ul_c = 5;
        public const byte minimapRoomID_ul_o = 6;
        public const byte minimapRoomID_ur_c = 7;
        public const byte minimapRoomID_ur_o = 8;
        public const byte minimapRoomID_dl_c = 9;
        public const byte minimapRoomID_dl_o = 10;
        public const byte minimapRoomID_dr_c = 11;
        public const byte minimapRoomID_dr_o = 12;
        public const byte minimapRoomID_ud = 13;
        public const byte minimapRoomID_lr = 14;
        
        public const byte minimapRoomID_udl_cc = 15;
        public const byte minimapRoomID_udl_co = 16;
        public const byte minimapRoomID_udl_oc = 17;
        public const byte minimapRoomID_udl_oo = 18;
        public const byte minimapRoomID_udr_cc = 19;
        public const byte minimapRoomID_udr_co = 20;
        public const byte minimapRoomID_udr_oc = 21;
        public const byte minimapRoomID_udr_oo = 22;
        public const byte minimapRoomID_ulr_cc = 23;
        public const byte minimapRoomID_ulr_co = 24;
        public const byte minimapRoomID_ulr_oc = 25;
        public const byte minimapRoomID_ulr_oo = 26;
        public const byte minimapRoomID_dlr_cc = 27;
        public const byte minimapRoomID_dlr_co = 28;
        public const byte minimapRoomID_dlr_oc = 29;
        public const byte minimapRoomID_dlr_oo = 30;
        
        public const byte minimapRoomID_udlr_cccc = 31;
        public const byte minimapRoomID_udlr_ccco = 32;
        public const byte minimapRoomID_udlr_ccoc = 33;
        public const byte minimapRoomID_udlr_ccoo = 34;
        public const byte minimapRoomID_udlr_cocc = 35;
        public const byte minimapRoomID_udlr_coco = 36;
        public const byte minimapRoomID_udlr_cooc = 37;
        public const byte minimapRoomID_udlr_cooo = 38;
        public const byte minimapRoomID_udlr_occc = 39;
        public const byte minimapRoomID_udlr_occo = 40;
        public const byte minimapRoomID_udlr_ococ = 41;
        public const byte minimapRoomID_udlr_ocoo = 42;
        public const byte minimapRoomID_udlr_oocc = 43;
        public const byte minimapRoomID_udlr_ooco = 44;
        public const byte minimapRoomID_udlr_oooc = 45;
        public const byte minimapRoomID_udlr_oooo = 46;



        // Loot tail stuff:
        public const byte lootFollowDistance_ship = 11;
        // public const byte lootFollowDistance_emptychain = 2;
        public const byte lootFollowDistance_emptychain = 4;
        public const byte lootFollowDistance_small = 3;
        public const byte lootFollowDistance_large = 4;
        // public const byte lootFollowDistance_shipTreasure = 5;
        public const byte lootFollowDistance_shipTreasure = 6;
        public const byte lootFollowDistance_final = 5;
        
        public const int lootValue_small = 2500;
        // public const int lootValue_large = 7500;
        public const int lootValue_large = 5000;
        public const int lootValue_cannon = 10000;
        public const int lootValue_ship = 40000;

        public const int repairCost = 15000;

        public const float portTownDistanceFromLand = 2f;


        // health bar stuff:
        public const byte healthbarState_idle = 0;
        public const byte healthbarState_damaged = 1;
        public const byte healthbarState_healed = 2;
        public const byte healthbarState_fadingOut = 3;


        // Ship selection stuff:
        public const string defaultShipName = "The Forlorn Swallow";
        public const byte defaultSelection_SailPattern = 0;
        public const bool defaultSelection_SailIsMirrored_horizontal = false;
        public const bool defaultSelection_SailIsMirrored_vertical = false;
        public static readonly Color32 defaultColor_SailPrimary = new Color32(86, 86, 86, 13);
        public static readonly Color32 defaultColor_SailPattern = new Color32(221, 221, 221, 13);
        public static readonly Color32 defaultColor_MastAndDeck = new Color32(169, 116, 69, 13);
        public static readonly Color32 defaultColor_Rails = new Color32(205, 175, 145, 13);
        public static readonly Color32 defaultColor_Hull = new Color32(169, 116, 69, 13);
        public const byte defaultSelection_cannon = 0;
        public static readonly Color32 defaultColor_Cannonball = new Color32(80, 80, 80, 255);

        public const byte popupID_delete = 0;
        public const byte popupID_cancel = 1;
        public const byte popupID_value_red = 2;
        public const byte popupID_value_green = 3;
        public const byte popupID_value_blue = 4;


        // Cannon selection stuff:
        public const byte cannonShotType_pointAndRelease = 0;
        public const byte cannonShotType_pointAndRelease_spread = 1;
        public const byte cannonShotType_pointAndRelease_chain = 2;
        public const byte cannonShotType_pointAndRelease_mystical = 3;
        public const byte cannonShotType_fireInDirection = 4;
        public const byte cannonShotType_secret = 5;

        // public const byte cannonEffect_normal = 0;
        // public const byte cannonEffect_autoTarget = 1;
        // public const byte cannonEffect_explosive = 2;
        // public const byte cannonEffect_burning = 3;
        // public const byte cannonEffect_secret = 4;

        public const byte cannonballPrefabID_normal = 0;
        public const byte cannonballPrefabID_grapeshot= 1;
        public const byte cannonballPrefabID_chainShot= 2;
        public const byte cannonballPrefabID_rapid = 3;
        public const byte cannonballPrefabID_mystical = 4;
        public const byte cannonballPrefabID_explosive = 5;
        public const byte cannonballPrefabID_hotShot = 6;
        public const byte cannonballPrefabID_secret = 7;

        public const float cannonAutoTargetingSpeed = 0.025f;
        public const float cannonExplosionRadius = 15f;

        public const float cannonChainRadius = 1f;
        public const float cannonChainRotationInc = 12.5f;







        public static int getValueOfFinalTreasure()
        {
            //return Random.Range(0, 9) * 25000 + 200000;
            return 300000;
        }

        public static int getCustomization_StartingMoney(byte buttonNum)
        {
            switch(buttonNum)
            {
                case 1: return 75000;
                case 2: return 150000;
                case 3: return 225000;
                case 4: return 300000;
                case 5: return 375000; //For now, anyways...
                default: return 375000; //For now, anyways...
            }
        }

        public static float getCustomization_DensityOfEnemeies(byte buttonNum)
        {
            switch(buttonNum)
            {
                case 1: return 0f;
                case 2: return 0.5f;
                case 3: return 1f;
                case 4: return 2f;
                case 5: return 1.5f; //For now, anyways...
                default: return 1.5f; //For now, anyways...
            }
        }

        public static byte getCustomization_NumberOfKeys(byte buttonNum)
        {
            switch(buttonNum)
            {
                case 1: return 6; //For now, anyways...
                default: return 0; //For now, anyways...
            }
        }

        public static byte getCustomization_SizeOfArea(byte buttonNum)
        {
            switch(buttonNum)
            {
                case 1: return 0;
                case 2: return 1;
                case 3: return 2;
                case 4: return 3;
                case 5: return 4; //For now, anyways...
                default: return 10; //For now, anyways...
            }
        }

        // The secret camera settings:
        
        //private const float cust_camAngle_base = 1.047197551f;
        //private const float cust_camAngle_inc = -0.052359878f;
        private const float cust_camAngle_base = 0.043633231f;
        private const float cust_camAngle_inc = 0.058177642f;
        // private const float cust_camDist_base = 5f;
        private const float cust_camDist_base = 15f;
        private const float cust_camDist_inc = 2f;

        public static float getSetting_CamAngle(byte settingNum)
        {
            return cust_camAngle_base + settingNum * cust_camAngle_inc;
        }

        public static float getSetting_CamDist(byte settingNum)
        {
            return cust_camDist_base + settingNum * cust_camDist_inc;
        }



        // Common functions:

        public static bool checkIfObjectIsCloseEnough(Vector3 unitPos, Vector3 itemPos, float itemRadius)
        {
            if (Mathf.Abs(unitPos.x - itemPos.x) > itemRadius || Mathf.Abs(unitPos.z - itemPos.z) > itemRadius)
                return false;

            if (Mathf.Sqrt(Mathf.Pow(unitPos.x - itemPos.x, 2) + Mathf.Pow(unitPos.z - itemPos.z, 2)) <= itemRadius)
                return true;
            
            return false;
        }

        public static bool checkIfObjectIsCloseEnough_rectangular(Vector3 unitPos, Vector3 itemPos, 
                                                                    Vector3 itemRight, Vector3 itemForward, 
                                                                    float itemRange_halfSide, float itemRange_halfForward)
        {
            if (Mathf.Abs(unitPos.x - itemPos.x) > Mathf.Max(itemRange_halfSide, itemRange_halfForward) 
                    || Mathf.Abs(unitPos.z - itemPos.z) > Mathf.Max(itemRange_halfSide, itemRange_halfForward))
                return false;

            if (Vector3.Project((unitPos - itemPos), itemRight).magnitude <= itemRange_halfSide 
                    && Vector3.Project((unitPos - itemPos), itemForward).magnitude <= itemRange_halfForward)
                return true;
            
            return false;
        }


        public static void moveGameObject_andChangeForward(Transform objectTransform, Vector3 moveVec, bool[,] boolArray, DoorHitboxManager doorHitboxManager)
        {
            if (moveVec.magnitude > 0.05f)
            {
                objectTransform.forward = moveVec.normalized;
                objectTransform.position += BoolArrayManager.adjustMovementBasedOnWalls(boolArray, objectTransform.position, moveVec, doorHitboxManager);
            }
        }

        public static void moveGameObject_sameForward(Transform objectTransform, Vector3 moveVec, bool[,] boolArray, DoorHitboxManager doorHitboxManager)
        {
            objectTransform.position += BoolArrayManager.adjustMovementBasedOnWalls(boolArray, objectTransform.position, moveVec, doorHitboxManager);
        }
    }
}
