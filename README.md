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

This game is… quite large. It uses a total of 105 different C# scripts, so explaining everything within each of these would take quite a bit of time.

Instead, I will divide the game into seven major parts, and in the following sections I’ll explain the most important details about each part.


**Part 1, The Title Scene:**

![Image of the Title Scene](/Screenshots/01 Title Scene.jpg)
Format: ![Alt Text](url)

This Scene is the first one the player will see when opening the app, and it gives the player a chance to customize the game before they begin.

The scripts for this Scene aren’t especially complicated. The script named “TitleScreen” is the main one, which contains a bunch of public methods that are called when the buttons in this Scene are pressed.

The verbosely-named “TitleScreenCustomizationMenuManager” is the longest script for this Scene, and it handles all of the stuff related to the customization menu. Most of this file handles how the visuals change when the buttons are pressed… but it will also communicate with a Scriptable Object named “PersistentData” to store the player’s selections, which later carries the data off to other Scenes that need them. The Persistent Data will be explained in Part 7.

This Scene can only transition to one other Scene. When the ‘begin game’ button in the middle of the screen is pressed, Unity will switch over to the Ship Selection Scene.
