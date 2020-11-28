# Introduction
Infinite Plunder is a game designed for mobile devices, and is built using the Unity game engine. In this game, the player sails a tiny pirate ship through a randomly-generated maze of water in search of hidden treasures. It was created by Thomas Madsen as a capstone project for CSUCI in the Fall semester of 2020.


## Software/Hardware requirements:

This game was made in Unity, so naturally you will need to have that installed on your computer in order to run this code. I built this using the 2019.3.7f1 version of Unity, although I would assume any newer version will work as well.

You will also need to download a Unity module for either Android Build Support or iOS Build Support, which can be done from within the Unity Hub. I can guarantee this game works as intended on Android devices, but I have not tested iOS devices so those might require additional work to run properly.

In addition, this game does not have any keyboard controls, so a mobile device is required in order to play this game.

You may optionally choose to install “Unity Remote 5” onto your device. This will allow you to play the game from within the Unity editor by using your device as a controller.


## Opening the project to edit:

Opening the project in Unity is really easy! Assuming you have the software installed, all you need to do is navigate to the ‘Assets’ folder, move into the ‘_Scenes’ folder, then double-click any of the .unity files within. I recommend ‘TitleScene.unity,’ since that’s the first scene in the game, but any of them will do!


## Running the game on a mobile device:

Before your first build, you might need to adjust Unity’s ‘target platform.’ To do this, click “File -> Build Settings,” then select your desired mobile device and press “Switch Platform.” Assuming you have the proper Build Support installed, this should go without a hitch.

Installing the game onto your device is fairly easy. Simply plug your mobile device into your computer and press “File -> Build And Run” within the Unity editor.

If you’re using an Android device, you might need to enable “USB Debugging” in the Developer Options before it will allow Unity to install anything to it. The Developer Options are hidden on Android Phones by default, so you might need to look online for how to make these appear.

I would assume that iOS devices require something similar to this… but I cannot say for certain.

You can also play this without needing to install anything, since you can run the game within the Unity editor and play using the Unity Remote 5 app. The instructions for how to use this app can be found online.


## Explanation of the game’s code:

This game has gotten… quite large. It uses a total of 105 different C# scripts, and explaining everything within each of these would take quite a bit of time.

Instead, I will divide the game into seven major parts, and in the following sections I’ll explain the most important details about each part.

\
**Part 1, The Title Scene:**

![Image of the Title Scene](https://github.com/Tjmadsen88/Infinite-Plunder/blob/main/Screenshots/01%20Title%20Scene.jpg)

This Scene is the first one the player will see when opening the app, and it gives the player a chance to customize the game before they begin.

This Scene has five scripts associated with it. These are named TitleScreen, AspectRatioManager_Title, TitleScreenCustomizationMenuManager, TitleScreenInputTextManager, and TitleShipMotionManager.

Relatively speaking, the scripts for this Scene aren’t especially complicated. The script named TitleScreen is the main one, and it contains a bunch of public methods that are called when the buttons in this Scene are pressed.

But the longest and most impactful script is the one named TitleScreenCustomizationMenuManager. This one handles all of the stuff related to the customization menu, although most of this is just controlling how the visuals change when its buttons are pressed.

The user’s selections are saved to a Scriptable Object named “PersistentData”, and later on the Game Scene and the terrain-generation thread reference these selections to make decisions. The Persistent Data will be explained in greater detail in Part 7.

This Scene can only transition to one other Scene. When the ‘begin game’ button in the middle of the screen is pressed, Unity will switch over to the Ship Selection Scene.

\
**Part 2, The Ship Selection Scene:**

![Image of the Title Scene](https://github.com/Tjmadsen88/Infinite-Plunder/blob/main/Screenshots/02%20Ship%20Selection.jpg)

This Scene will allow the player to create and save many different ships to play with. They can select which ship they want to use with the ship-selection menu, and can create or customize their ships using the ship-edit menu.

This part has 20 different scripts associated with it. I won’t name them all right here, but these files can be found in the “Assets\Scripts\5_ShipSelection” folder and its subfolders.

Most of the buttons in the ship-selection menu are handled by a class called “ShipSelection”. The larger buttons in the middle of the menu are managed by the “ShipButtonManager”. This class creates a button for every ship the player has saved, is responsible for arranging them, and also handles how they’re highlighted when the player presses them.

The ship-editing menu is largely handled by a class named “ShipEditMenuManager”. This receives all the inputs from the various buttons and sliders in this menu.

A single ship’s data is stored in a class called “ShipDataIndividual,” and an array containing one of these for each ship is stored in the “ShipDataPacket” class. If a ship is created, deleted, or modified, or if the order of any ship is changed, the entire ShipDataPacket is converted to a binary string and saved to a file, so it can be recovered in its entirety the next time this Scene is entered.

There is no practical limit to the number of ships a player can create. If there are more buttons than would appear on screen, the user can scroll the area to make them appear.

The ship data is indexed using an integer, so there exists a technical limit of 2,147,483,647 ships. Of course, I’m assuming this is larger than anyone could possibly reach in their lifetime.

A class called the “ShipTextureManager” is responsible for changing the colors and sail pattern on the preview ship in this scene. The 3D model for this ship does not have a texture initially, and instead the ShipTextureManager creates an image at runtime to assign to the model. This texture is only nine pixels large (3x3), and the ship’s 3D model is specifically designed so that entire sections of the ship can be colored with only a single pixel of this texture.

This Scene can transition into two other Scenes. Pressing the ‘cancel’ button from the ship-selection menu will move back to the Title Scene… but if ‘begin’ is pressed, the terrain-generation thread will be told to start, and the Scene will change to the Loading Scene.

\
**Part 3, The Loading Scene, and the Terrain-Generation Thread:**

All of the terrain-generation in the game is handled on a separate thread. While this thread is running, the Loading Scene will occupy the player with a simple animation until the thread is finished.

There are 14 scripts dedicated to the terrain-generation process, and 2 for the Loading Scene. These files are located in the “Assets\Scripts\ 3_Game\Terrain Generation” and “Assets\Scripts\2_Loading” folders respectively.

This thread will generate a bunch of things all at once. It will determine the layout of the maze, the positions of the keys and doors within it, the placements the port towns, as well as calculating all the data needed for the Game Scene to generate the 3D models for the terrain.

This process is very long, very complicated, and… to be honest, it has been extremely difficult for me to wrap my head around. An explanation of this process would be too long to fit right here, but if you’re interested in knowing how all this is generated you can read a detailed explanation over here.

When this thread finishes, it will push all the data it’s generated into the Persistent Data (which is detailed in Part 7), and the Loading Scene will automatically change to the Game Scene at the same time.

\
**Part 4, The Game Scene:**

![Image of the Title Scene](https://github.com/Tjmadsen88/Infinite-Plunder/blob/main/Screenshots/03%20Game%20Scene.jpg)

This Scene is where all the fun stuff happens. This is responsible for the major gameplay, which includes moving the ship, shooting the cannon, collecting items, opening doors, controlling enemies, receiving and repairing damages, updating the minimap… all kinds of stuff.

This part is comprised of 59 different files, all of which can be found in the “Assets\Scripts\3_Game” folder and its subfolders.

These files contain thousands upon thousands of lines of code… so it’d be difficult for me to explain all of what is going on right here. If you’re interested a more in-depth explanation of this Scene, you should take a look over here instead.

This Scene can transition into two other Scenes. Pressing the “Return to Title” button in the mid-game settings popup will move back to the Title Scene, and winning or losing the game will cause the Scene to transition to the Victory Scene.

\
**Part 5, The Victory Scene:**

![Image of the Title Scene](https://github.com/Tjmadsen88/Infinite-Plunder/blob/main/Screenshots/04a%20Victory%20Scene.jpg)
![Image of the Title Scene](https://github.com/Tjmadsen88/Infinite-Plunder/blob/main/Screenshots/04b%20Defeat%20Scene.jpg)

This Scene will tell the player whether they have won or lost the game, and will also display some statistics about how the game was played. Generally speaking, this is the smallest and least complicated Scene in the game.

This scene only has four scripts associated with it. These are the VictoryScreen, AspectRatioManager_Victory, VictoryChestManager, and DefeatWheelManager. These are all located in the “Assets\Scripts\4_Victory” folder.

Most of the code in this scene is used to play the animations. The functions which control the text appearing on screen can be found in a file called “VictoryScreen,” while the chest-opening animation and wheel-falling animation are handled in the “VictoryChestManager” and “DefeatWheelManager” classes respectively.

Also, the lump of gold and jewels that appear within the treasure chest is somewhat randomly positioned each time this Scene is entered. The code which does this is also located inside the VictoryChestManager script.

This Scene can only transition to one other Scene. When the ‘return’ button in the bottom corner is pressed, the Scene will change to the Title Scene.

\
**Part 6, The Constants Class:**

Looming above all the other scripts in this game is a static class named Constants. This class can be seen by all other scripts in the game, and contains a rather lengthy list of predefined constant variables. 

This class is located in the “Assets\Scripts” folder.

The Constants class contains almost all of the game’s hard-coded values, and is referenced by almost every script in the game. This makes it very easy to adjust numbers across several files, as well as ensuring that any data that will be passed between different files contains consistent values.

This isn’t the biggest or most exciting class in the game, but its ubiquitousness in the code is cause for honorable mention.

\
**Part 7, The Scriptable Object Named PersistentData:**

This game uses one instance of what Unity calls a ‘Scriptable Object.’ These are particularly helpful for transferring data across Scenes in Unity, which is primarily what I’ve been using this for.

This file is located in the “Assets\Scripts\6_Scriptable Objects” folder.

I will admit that I’m not very familiar with Scriptable Objects (this game is my first time using one), but Scriptable Objects, unlike other objects, are not destroyed when Unity changes Scenes. Any changes made to these objects in one Scene can be seen in the next, allowing any values stored within these to persist from one scene to the next. As such, I’ve decided to name this file “Persistent Data.”

I had meant to create several Scriptable Objects, but for some reason a bug in the Unity editor has prevented me from doing so. When I create a second one, the Unity editor somehow forgets about the first and throws up a bunch of ‘not found’ errors. As such, I was forced to stuff several files worth of functionality into just this one…

By necessity, this file is visible to every class which ended up needing it. It ended up handling four separate tasks:
- The Persistent Data records the user selections from the Title Scene, so the Game Scene, the Victory Scene, and the terrain-generation algorithm can reference them. If the user selected ‘Random’ for any of these, this will also generate the random values before the start of each game.
- The Game Scene uses the Persistent Data to record the mid-game statistics as they happen, so the Victory Scene can later display the final numbers.
- The Ship Selection Scene uses the Persistent Data to store the currently-selected ship’s data, so the Game Scene can adjust its cannon data and the 3D model of the ship accordingly.
- The terrain-generation thread deposits its data here so the Game Scene can later use it to instantiate all the 3D models.


## Conclusion:

I hope this explanation was helpful. If you want to read the more in-depth explanation of the terrain-generation process you can do that here, and if you want a more thorough explanation of the Game Scene you can do that over here.

I’ve been spending close to five months building this thing, and I’m quite happy with how it has turned out.

If you’d like, you can find some supplementary information at my CIKeys website, which includes additional screenshots and a video of the game in action.

Thank you for reading!
