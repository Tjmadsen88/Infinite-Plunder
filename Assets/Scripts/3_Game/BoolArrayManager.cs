using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ConstantsSpace;

using DoorHitboxManagerSpace;

namespace BoolArrayManagerSpace
{
    public static class BoolArrayManager
    {
        public static Vector3 adjustMovementBasedOnWalls(bool[,] boolArray, Vector3 boatPos, Vector3 movementVec, DoorHitboxManager doorHitboxManager)
        {
            // Use lerping to find if the boat's next position would be inside the land (and later, if it'd be inside a door)
            // If no conflict, return movementVec as is?

            if (checkForConflict_V3(boolArray, doorHitboxManager, boatPos + movementVec)) return movementVec;

            // If there is a conflict... keep track of which box it is that we landed on.
            // Check the 5x5 box around this point. For any non-conflicts, add the constants' vector to 
            // a running total. Then, normalize this vector.

            Vector3 reflectionVector = calculateReflectionVector(boolArray, doorHitboxManager, boatPos + movementVec);

            // Now, nudge the boat in the direction of the reflection vector until it lands in the water:

            Vector3 newMoveVec = new Vector3(movementVec.x, 0, movementVec.z);

            int breaker = 180;

            do {
                newMoveVec += reflectionVector * Constants.wallRepelSpeed;
                if (newMoveVec.magnitude > Constants.shipMoveSpeed)
                    newMoveVec = newMoveVec.normalized * Constants.shipMoveSpeed;

                if (breaker-- <= 0)
                {
                    return new Vector3(0f, 0f, 0f);
                }
            } while (!checkForConflict_V3(boolArray, doorHitboxManager, boatPos + newMoveVec));

            // I'll need a function like... 'checkForConflict', which will check the position against the boolArray and the doors,
            // and return true/false if it can move there or not.

            // I'll likely need a proper Lerp-like function to find a smooth line between each discrete vertex... and then decide
            // whether or not a point is on the 'safe' side of that line.

            // I'll need a function like... 'calculateReflectionVector', which 'checkForConflict's every box within a 5x5 square. 
            // (I should be able to assume the player will never be within 3 of the walls...)
            // For every conflict it finds, it will use the Constants' vector array to produce and return a new 'normal' vector.

            // I... should be able to handle the rest in here?

            return newMoveVec;

            // I image each door could have four integers... two for 'startXY' and two for 'endXY'.
            // If the checked position lies within these integers, it will be considered a conflict...
            // And when a door opens... either this object is removed from the list, or the four verts
            // get set to 0?
            // Perhaps a door has two lists... one that controls the 3D model, and one that controls the
            // blocking area.
            // WHen a key is collected, every 'hitbox' for that door is immediately removed from that list.
            // When the door is approached, the model is 'opened' and that controller is removed from the list...
            // This sounds good?
            // In this case, the 3D model one would be considered a collectable, so would be stored in the same
            // list as the keys... and when collected, the model will play the 'open' animation instead of being
            // destroyed along with the in-memory version...

            // The collectables should be handleable with the distance between the real XYZ vectors...
            // Like how I intend to handle the auto targeting as well, using the enemies...
            // This could also be done every 1/10 of a second or so, to reduce overhead?
        }

        public static bool checkForConflict_V3(bool[,] boolArray, DoorHitboxManager doorHitboxManager, Vector3 posToCheck)
        {
            return checkForConflict_V2(boolArray, doorHitboxManager, new Vector2(posToCheck.x, posToCheck.z));
        }

        public static bool checkForConflict_V3_noDoors(bool[,] boolArray, Vector3 posToCheck)
        {
            return checkForConflict_V2_noDoors(boolArray, new Vector2(posToCheck.x, posToCheck.z));
        }

        public static bool checkForConflict_V2(bool[,] boolArray, DoorHitboxManager doorHitboxManager, Vector2 posToCheck)
        {
            if (doorHitboxManager.checkIfPosIsInDoor(posToCheck)) return false;

            return checkForConflict_V2_noDoors(boolArray, posToCheck);
        }

        public static bool checkForConflict_V2_noDoors(bool[,] boolArray, Vector2 posToCheck)
        {
            // First, I need to use some math to find the four vertices that this spot is bound by.
            int boundIndex_L = (int)(posToCheck.x / Constants.vertDistances);
            int boundIndex_U = (int)(posToCheck.y / -Constants.vertDistances);
            
            // If two or more of these verts are water spaces, return 'okay.'
            byte numOfWaterSpaces = 0;
            if (boolArray[boundIndex_L, boundIndex_U]) numOfWaterSpaces++;
            if (boolArray[boundIndex_L+1, boundIndex_U]) numOfWaterSpaces++;
            if (boolArray[boundIndex_L, boundIndex_U+1]) numOfWaterSpaces++;
            if (boolArray[boundIndex_L+1, boundIndex_U+1]) numOfWaterSpaces++;

            if (numOfWaterSpaces >= 2) return true;
            if (numOfWaterSpaces == 0) return false;

            // Otherwise, use the X and Y coords of posToCheck to find which edge is the closest.
            float distToLeft = posToCheck.x / Constants.vertDistances - boundIndex_L;
            float distToTop = posToCheck.y / -Constants.vertDistances - boundIndex_U;

            byte shortestEdge_ID = 0; //0 is left, 1 is up, 2 is down, 3 is right
            float shortestEdge_val = distToLeft;

            if (distToTop < shortestEdge_val)
            {
                shortestEdge_ID = 1;
                shortestEdge_val = distToLeft;
            }
            if (1f - distToTop < shortestEdge_val)
            {
                shortestEdge_ID = 2;
                shortestEdge_val = 1f - distToTop;
            }
            if (1f - distToLeft < shortestEdge_val)
            {
                shortestEdge_ID = 3;
                shortestEdge_val = 1f - distToLeft;
            }

            // If either of the verts on this edge are water, return 'okay.'
            switch (shortestEdge_ID)
            {
                case 0: // Left
                    if (boolArray[boundIndex_L, boundIndex_U] || boolArray[boundIndex_L, boundIndex_U+1]) return true;
                    break;
                case 1: // Up
                    if (boolArray[boundIndex_L, boundIndex_U] || boolArray[boundIndex_L+1, boundIndex_U]) return true;
                    break;
                case 2: // Down
                    if (boolArray[boundIndex_L, boundIndex_U+1] || boolArray[boundIndex_L+1, boundIndex_U+1]) return true;
                    break;
                default: // Right
                    if (boolArray[boundIndex_L+1, boundIndex_U] || boolArray[boundIndex_L+1, boundIndex_U+1]) return true;
                    break;
            }

            // If neither of these are the case, return 'not okay.'
            return false;
        }

        private static Vector3 calculateReflectionVector(bool[,] boolArray, DoorHitboxManager doorHitboxManager, Vector3 collisionPoint)
        {
            Vector2 collision_butV2 = new Vector2(collisionPoint.x, collisionPoint.z);
            Vector2 tempVector = new Vector2(0.0000001f, 0f);

            // This will call 'checkForConflict' on a bunch of points in a circle around the collisionPoint.
            //   (these vectors shouldn't require the use of calculation, except maybe to multiply by the boat's move speed...)
            // If any of these points are in the water (ie, it returns true), then add that vector to a running total.
            for (int index=0; index<Constants.walls_hittestVectors.Length; index++)
            {
                if (checkForConflict_V2(boolArray, doorHitboxManager, collision_butV2 + Constants.walls_hittestVectors[index] * Constants.wallHittestRadius))
                {
                    tempVector += Constants.walls_hittestVectors[index] * Constants.wallHittestRadius;
                }
            }

            // After all the checks are done, normalize the vector and return it.
            tempVector = tempVector.normalized;
            return new Vector3(tempVector.x, 0, tempVector.y);
        }
    }
}