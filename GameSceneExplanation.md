# A Deeper Explanation of the Game Scene:

The Game Scene is where the main gameplay takes place. This Scene is responsible for about half of the code in the entire project, and is broken up into 59 different C# scripts.

Here, I will attempt to explain the most important parts of this Scene. There are 10 aspects of this scene that I want to explain, so I’ll go over each of them one at a time.

\
**Part 1: GameView**

This class is the ‘main’ class for the Game Scene. It handles the tasks that occur on the Scene’s first frame, like instantiating 3D objects into world, as well as managing the progress of the game itself.

This class’s first job is to keep track of any variables associated with the game’s progress, or with the player’s ship in general. This means it keeps track of the number of keys the player has collected, the current amount of money they possess, as well as the remaining durability of the player’s ship. Most of the functions associated with these variables are also located within this class.

This class is also responsible for instantiating most of the 3D models at the start of a game. This includes the collectable items like keys and the final treasure, as well as the 3D models for the terrain and port towns. All of the positions, rotations, scales, and colors for these 3D models have been calculated earlier in the terrain-generation thread, so all this class really does is comb through the data and place things as instructed.

Lastly, this class is responsible for pausing and resuming the game when needed. The game will pause if the mid-game settings popup is opened, and will resume when it is later closed. If the game is considered ‘unpaused,’ this class will send a message to all the other classes each frame giving them permission to proceed.

\
**Part 2: The TouchReader and TouchManager**

These two classes are responsible for organizing and interpreting all of the user’s touch inputs.

The TouchReader will analyze every touch on the screen to positions the positions and tilts of the two virtual thumbsticks. This data is then sent to the TouchManager, who then uses this data to determine how the ship should move, where the cannon should aim, and whether or not the cannon should fire.

Another version of the TouchReader exists in the code, which is named TouchReader_KeyboardControls. This one handles the game's keyboard and mouse inputs, and will shortcut the regular TouchReader if these inputs are ever detected. 

\
**Part 3: ShipRotationManager**

As the player’s ship moves around, there are several things in the game that will cause the 3D model to rotate. This class’s job is to apply all of these rotations in such a way that they don’t interfere with one another.

The most obvious rotation occurs when the ship changes direction, but it will also bob and sway as if buffeted by the waves, and lean a little bit when rounding corners. 

\
**Part 4: BoolArrayManager**

The name of this class might not make sense at first glance, but this class is used to perform hit detections between objects and the surrounding terrain.

It is true that these kinds of collisions can be detected using Unity's built-in MeshColider, but... in this case, I figured it would be impractical to do it this way. I knew that the vast majority of the faces and vertices it would be checking would either be too high or too low for an object to ever hit them, and I was afraid these extra checks would significantly impact the performance of my game.

So instead, I decided to write all of the collision-detection stuff myself. I created a giant 2D array of Boolean values, with each entry recording whether a position in the world is in the water or on land.

Some mathematical trickery is needed to get a continuous boundary between each of these discrete points, which is primarily what this manager does. If some other class passes a vector to this class, in either 2D or 3D space, this manager will return 'true' or 'false' for "this position is in the water" or "this position is on land." The other class will need to interpret this response accordingly.

\
**Part 5: MinimapManager**

This game has a minimap in the upper-right corner of the screen. As the player sails around the maze, this minimap will update to reveal important things they may have found.

The minimap keeps track of several things. It keeps track of the player’s position, general regions the player has previously visited, the positions of all the doors, keys, and the final treasure, as well as the positions of every port town. Most of this data is hidden at the start of the game, but becomes revealed when the player sails close to them.

When a key, or the final treasure, is collected, their icons will disappear. If the player sinks and drops the final treasure, its icon will reappear in the center of the region the player is considered to be in.

The main image of the minimap is generated as part of the terrain-generation algorithm. It uses the upscaled terrain data before noise is applied, then draws all the vertices onto an image based on their height.

When a new region is explored, the pixels in this image are recolored to use a lighter color. 

\
**Part 6: InteractablesManager**

There are several items in this game that the player can interact with just by sailing near them. These items include collectable objects like treasures and keys, but also the skull gates and the port towns’ piers. All of these items have scripts attached that extend an abstract class called “InteractableObject_Abstract”, and this allows them all to be managed by the InteractableManager.

When the player sails close enough to one of these items, the InteractablesManager will identify what kind of item it is and decide what is supposed to happen. Items may be collected, gates may be opened, and port towns can buy treasures or repair the player’s ship.

This class keeps track of all the interactable items using an array. If more treasure is dropped than can fit in this array, it will automatically expand the array to make room.

When one of these interactable objects has been successfully interacted with, they will be removed from the array. Each object will have different interaction radii and effects when interacted with, which are defined inside their respective implementations of the abstract class.

\
**Part 7: LootTailManager**

This class makes all the treasures which the player has collected will follow behind the player in a long chain.

When a piece of treasure is collected by the player, it is removed from the InteractablesManager, its attached class is destroyed, and the 3D model is passed to the LootTailManager where it attaches a script named “LootPieceIndividual” to it.

Each treasure will keep a history of where the previous treasure in the chain had been moving, and will follow those same movements a certain number of frames later.

The first piece in the chain will only move if the player’s ship has moved more than a certain minimum distance, preventing the entire chain from scrunching up if the ship chooses to move slowly.

When the player sells all their treasures, the LootTailManager will play a special animation where all the treasures zoom away from the player and towards the pier. Once this animation is over, it will destroy all the 3D models and empty the array.

\
**Part 8: EnemyManager**

This class handles the generation and operation of all the hostile characters in the game. 

There are three enemy types in the game. They each have a script attached to them which extends the “EnemyObject_Abstract” class, which the EnemyManager uses to control them.

These classes include all the details of how these enemies move, how they target the enemy, and how they choose to fire their cannons. They also record how much damage the character can sustain before they sink. 

The EnemyManager will create enemies at random times and in random places. These enemies will never appear too close to the player’s ship, too close to another enemy, nor too close to the pier of a port town.

At the start of a game, each enemy type will be given a randomized ‘spawn frequency.’ Based on how these numbers turn out, some enemy types may become much more common than others during that game.

Enemy characters will only move or target the player if they’re within a certain range of the player’s ship. When they want to fire a cannonball, they simply send a message to the CannonballManager and ask for a cannonball to be placed at their location.

The EnemyManager also has the job of damaging any enemy that the player’s cannonballs collide with. If the enemy takes enough damage to sink, it will message the InteractablesManager so it can place a small treasure there, then destroy the enemy’s 3D model and data.

\
**Part 9: The CannonballManager, CannonSmokeManager, and CannonBonkManager**

These classes handles all the code related to cannonballs, both the player’s and the enemies’. This includes making the cannonballs appear, making them move in a direction, and checking for collisions with valid targets.

The CannonballManager has a set number of cannonballs, which it makes appear or disappear as they are needed.

When a cannonball is fired, it will leave a smoke trail in its wake. Each cannonball object has a CannonSmokeManager attached to it which handles this.

And when a cannonball strikes a valid target, it will play a simple impact animation. I’ve been playfully referring to these animations as ‘cannon bonks,’ and handling these animations is the job of the CannonBonkManager. Like with the cannonballs, there are a limited number of these animations in the Scene which appear and disappear as needed.

These cannon bonks are tinted to become a similar color to the cannonball that summoned it, although the bonks are typically a lighter color.

If the player has an explosive cannon equipped, then any cannonball that strikes an enemy or the landscape will cause a huge explosion. The animation and damage caused by this explosion is handled within the ExplosionDamageManager class.

\
**Part 10: HealthBarManager**

This class seems really easy on paper, but the implementation was actually rather tricky. The HealthBarManager is in charge of playing the animations associated with the health bar when the player takes damage, or when they are repaired when visiting a port town. 

The healthbar will only appear if the player has taken damage. If the player is later fully repaired, the health bar fade away after a short time.
