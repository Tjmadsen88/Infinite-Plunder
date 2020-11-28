# Explanation of how the terrain is generated:

The terrain-generation aspect of this game is easily the largest and most complicated part of this entire thing. This is comprised of 14 different files that are located in the “Assets\Scripts\ 3_Game\Terrain Generation” folder.

Most of the terrain-generation occurs within a class called TerrainBuilder, which in turn calls upon the other files along the way. The TerrainBuilder class is used inside a secondary thread, which is started inside a function at the very bottom of the PersistentData, which is in turn called from within the ShipSelection class in the Ship Selection Scene.

There are several steps involved in this process, so let’s explain them all one at a time.

\
**Part 1, Generating the terrain’s colors:**

Generating the terrain’s colors could happen at any time, and I just happened to do it first. This process is called in the ShipSelection class, as soon as the player presses the ‘begin’ button, and before the terrain-generation thread is officially started.

The code for this part is located in a class called TerrainColorGenerator.

The terrain only uses four colors. It uses one for the water, one for the sand along the beach, and two colors for the hills. As of right now three of them are randomized, but the color of the sand will be the same every time.

The colors are randomly generated, but they’re also highly curated since… quite a few colors would look terrible. The game will first select a color theme, then generate the specific colors from a range within that. There are six themes in total, which are as follows:
-	Greens
    -	The game will pick this theme 50% of the time, making it by far the most common.
    -	Green landscapes will vary in intensity, so it might look as though the area is a lush forest, a grassy field, or a murky swamp.
    -	This theme is a little restrictive on which hues it can work with. It ensures that the green color channel will always be the highest, which prevents the landscape from becoming too yellow or too aqua.
    -	This theme will have a wide variety of water colors, which include blues and greens of varying intensities. One-tenth of the time, this theme will also pick a brown, dirtier water color instead.
-	Autumn:
    - The game will pick this theme 20% of the time.
    -	This theme is broken into three subthemes, which it will pick with equal probability. The first subtheme will only pick shades of red, the second shades of brown, and the third shades of orange.
    -	The autumn colors have proven to be very fickle, so the range of colors for each of these is kinda small… but the three of them together give it a satisfying amount of variation.
-	Snowy:
    -	The game will also pick this theme 20% of the time.
    -	This theme has two subthemes, which it will pick with equal probability. The first theme will make the landscapes pure white, making it look like heavy snow. These whites have a range of intensities, so sometimes the snow might appear lighter or darker.
    -	The second subtheme will look like lighter snow. The highest positions will be covered in white, while the areas closer to the beaches will be light greens or browns.
    -	The water in this theme has a more limited range of colors. This will only pick from lighter, paler colors to make the water look colder and icier.
-	Desert:
    -	The game will pick this 5% of the time.
    -	This theme also has two subthemes, and they function a lot like the snowy theme. The first will make the landscapes appear sandy and brown, with no vegetation whatsoever. The second will do that for the areas close to the beach, but the top of higher areas will be given green or brown shades, to make the area look partially-vegetated.
    -	The water in this theme will also have a much higher chance of being muddy. This will be the case 80% of the time, so the water will only end up being blue or green 20% of the time.
-	Purples:
    -	The game chooses this about 3% of the time.
    -	This theme will make the landscape purple or pink tones, to make it look like you’re sailing through a field of flowers. It looks a little weird sometimes, but I like having it show up now and again.
-	Darks:
    -	The game chooses this about 2% of the time.
    -	In this theme, the game will only pick shades of black and dark grey to color the landscape. It also colors the water using dark tones, which gives it a striking appearance.
    -	I do think this theme looks quite nice, but… it doesn’t look very natural. As such, I’ve made this happen only very rarely in the game.

These colors are then passed into the Persistent Data, where the GameView class can use it later. The terrain-generation thread is then started soon after, and the Scene changes to the Loading Scene.

\
**Part 2, Arranging a Simple Layout:**

I start off the terrain-generation process by building an abstract version of the maze’s layout, to ensure the game is actually completable. 

The code for this part is located in SimplifiedLayoutBuilder, as well as the SimplifiedLayoutDataPacket.

I’ll admit that this part is… difficult to explain. It makes sense in my head, but I’m not sure how will I’ll be able to explain it here with words. I promise this is the hardest part though!

Essentially, I imagine the terrain as being a grid of boxes. This grid initially contains only a handful of starting spaces in the middle, which I then add a bunch of ‘pathways’ onto. These pathways are placed in random unoccupied spaces that are adjacent to ones that are occupied, so I can guarantee the maze remains connected the entire time.

After placing a certain number of pathways, the game will place a pathway which contains a key in the final room. This “key pathway” is also guaranteed to include a gate somewhere that requires the previous key to open. Gates associated with the 0th key will not be placed, and the final ‘key’ placed will later be interpreted as the final treasure chest instead.

At this stage, the game places all six keys into the maze, even if the player has chosen for fewer keys to appear in the game. Some of these keys will be removed in the next step.

Every room being placed will know which keys are required to access it, using a number I’ve been calling a ‘clearance level.’ Any spaces the player can reach without finding any keys are considered to have a clearance level of 0, rooms that are behind the first door will have a clearance of 1, and the room containing the final treasure will always have a clearance of 6.

When placing a new path, the new rooms will know which previous room they are connected to. These new rooms will borrow the clearance level of the previous room, unless they contain a gate, in which case they may use the clearance requirement for the gate if it’s higher. These clearance levels will be used in the next step.

This algorithm will always place a minimum of seven pathways, since it needs to place the six keys and the final treasure, but the “Explorable Area Size” customization from the Title Screen may ask it to place more. Mazes that are Normal sized will add one additional path before each “key path”, the Large sized mazes will add 2… Small will add zero, and Tiny… will make the maze even smaller by forcing the key paths to be half their normal length. A path is usually 1 to 6 spaces long, but Tiny paths are only 1 to 3 spaces long.

When placing one of these ‘filler’ paths, the game might place a random gate in one of the rooms. These gates will always require a key that has already been placed in the maze, so the player is guaranteed to be able to open it. If one of these gates has a clearance that is higher than the pathway being placed, then that room and every subsequent room in the path will be given the higher clearance level.

Like I said, this part is pretty hard to follow… but I hope that made at least a little sense.

\
**Part 3, Connecting the Boxes Together, and Creating the SimplifiedLayoutReturnPacket:**

Once this grid of boxes has been finalized, we know the maze is arranged in such a way that the game is beatable. In this step, we need to connect the tiles together and define where the walls should be.

As a result of some poor planning, this part uses some of the same files as in Part 2… as well as a very similar naming scheme. That said, the code for this part can be found in the second half of the SimplifiedLayoutBuilder, as well as in the SimplifiedLayoutReturnPacket.

In this part, we need to convert the abstract boxes from the previous step into… slightly less abstract boxes. These new boxes will keep track of whether their four edges should have walls placed there, or if they should be left open for the player to sail through.

The game will comb through the previous step’s array of boxes and analyze them. If it finds two adjacent rooms with the same clearance levels, or if the two are connected by a gate, then it will decide they should be connected and open the walls between them. Otherwise, the game will assume the player shouldn’t be able to sail between them and places a wall between the two spaces. 

This step will also reference the “Number of Keys” customization from the Title Screen, then remove some of the keys from the game. It will pick a handful of clearance levels to remove at random until the target number is met, and keys from missing clearances will be ignored when they’re discovered.

Any gates corresponding to these missing clearances will be ‘demoted’ to the closest available clearance beneath it. If there are no clearances below, it will use clearance 0, and then be ignored.

All of this data is then stored in a class called a SimplifiedLayoutReturnPacket, which is then sent back to the TerrainBuilder class.

\
**Part 4, Upscaling the Simplified Layout:**

At this point, we know the layout of the maze, the positions of the walls, and the positions and colors of the gates and keys… but we still only know them in terms of grid coordinates. In this section, we finally define the terrain in terms of world-space vertex positions.

The code for this part can be found in the UpscaledLayoutBuilder, the UpscaledLayoutReturnPacket, and the UpscaledRoomTemplates files.

Here, the game creates an enormous 2D array of floats for every single vertex. These will represent vertex positions that are equally spaced in a grid along the horizontal plane, and the float value stored in each slot will represent that vertex’s vertical position.

It then combs through every room in the previous SimplifiedLayoutReturnPacket, and uses the positions of the walls to create a ‘roomID.’ Since each room has four sides, and each side can either have a wall or not, we know there can be 16 possible roomIDs.

From here, it will run through a switch statement to pull a ‘room template’ from the UpscaledRoomTemplates class. These templates contain a set of vertices that result in a very smooth, sinusoidal trench connecting each of the open edges (These room templates were originally calculated using a lot of math, but I later copied the values into read-only variables for quicker loading times).

After the game has finished combing through all the previous rooms, we’re left with a ‘height array’ that contains nothing but the really smooth trench data, which would technically work, but would look too mechanical to pass as a believable landscape. 

This smooth map can, however, be used to generate a clean and readable minimap, so it takes a quick diversion at this point to handle that. This smooth landscape is converted into what I call a ‘boolArray,’ which is a giant 2D array of Boolean values where any vertex lower than the water level is set to ‘true,’ and any vertex above it is set to ‘false.’ This array is then stored in the UpscaledArrayReturnPacket, where it can later be used to generate the minimap.

Now, the game returns and roughs up the smoothness of the values. It does so by using Unity’s built-in Perlin Noise algorithm, which adds some random, smooth, and continuous lumps to the values in the array. The values for this process are very sensitive, and took quite a bit of trial-and-error to get right…

After this, the new roughed-up height array is used to create another boolArray, which will later be used in the GameView, and both it and the height array are pushed into the Packet and returned to the TerrainBuilder.

\
**Part 5, Generating the 3D Mesh Data:**

We now have all the vertices for the terrain, but we still need a little bit more information if we want to convert them to 3D models within the world. In this part, we go and calculate all of these missing pieces.

Some of the code for this section can be found in the TerrainBuilder class, although most of it is inside the HeightArrayTo3DMeshConverter and TerrainMeshDataPacket files.

Now, I learned the hard way that Unity has a limit to the number of vertices a dynamically-generated 3D model is allowed to have… which is 65,536. As it turns out, the terrain in my game has, umm… let’s just say that’s not big enough for my purposes. So in the TerrainBuilder, I first needed to split my height array into multiple pieces which could be instantiated into several smaller objects.

After splitting them up, I pass each section of the height array into their own instance of a TerrainMeshDataPacket object, which automatically uses the HeightArrayTo3DMeshConverter to get the rest of the required data.

I needed to calculated four things in order to dynamically-generate a 3D model in Unity. The first is the position of the vertices in world space, which is what we calculated in Part 4.

As for the others, I needed to tell Unity how the vertices fit together by specifying which vertices formed triangular faces… I needed to calculate the surface normal vectors at each vertex, and I also needed to define how the image texture should be drawn along the resulting surface by defining its UVs.

The triangles were pretty easy. The vertices are defined as a rectangular grid, so all I needed to do was tell Unity that each vertex used its neighbors to form triangles.

As for the surface normals… I initially did this the proper way, using the average of the vector cross products between each of the vertex’s neighbors, but after running a few tests… it became clear that this was unacceptably slow. I had a lot of vertices, and the sheer number of cross products resulted in a delay of several seconds before the start of each game…

So instead, I used an approximation which required significantly fewer steps. This approximation only works because each of the vertices’ immediately-adjacent neighbors lie along the four cardinal axes, so I could approximate each of their tangent vectors using 2D trigonometry instead of 3D cross products… 

After working through all the math, I also found that many of the terms were cancelling out! In the end, my approximation reduced to using only two additions and one multiplication per vertex, as opposed to the eight cross products from earlier! The loading delay pretty much vanished completely, which was awesome.

(As a side note, I must admit that, due to the cancellation of terms from earlier, it is no longer apparent in the code what the underlying logic was for my approximation. I could explain that here, but it’s kinda mathy and a little off topic)

The last thing that I needed to calculate was how I wanted the 3D mesh to display the image texture, which involves defining the mesh’s UV map. My approach to this was a little… unconventional… but I’m nonetheless quite proud of what I ended up doing.

I ended up defining the terrain’s UVs based exclusively on their height. Any vertex that below the water level was given a UV coordinate of 0,0, any vertex that was above some maximum height was given a coordinate of 1,0, and any height in between was given an interpolation of these two.

This way, all I needed to do was create a texture that was four pixels wide, where each pixel was given one of the colors defined in Part 1, and the terrain would automatically draw the colors as clean horizontal bars based on its height!

\
**Part 6, Placing the Port Towns:**

At this point, we’ve calculated everything that’s needed to create 3D models for the terrain and populate the world with gates and keys, but it still needs one more thing. In this step, the game decides where and how to place the various port towns across the landscape.

The code for this can be found in the PortTownPlacementGenerator, the PortTownIndividualPacket, the PortTownReturnPacket, and TerrainHeightLerping.

The PortTownPlacementGenerator will always place the first port town in the same place the player starts in. It will then place additional port towns in random places, with the total number of towns being proportional to the size of the maze.

The process of placing a port town starts with the position of its pier. This is initially placed in the very center of a room, in the water, where it is then slightly nudged towards one of the edges until it touches the land.

Every port town is given a few random properties, so each one ends up looking a little different. Each town is given a random ‘town roof color,’ as well as a radius for the town’s total area, and a target number of houses that should be placed around the town.

The game will attempt to place the target number of houses in random places within the town’s radius. If one of the houses ends up in the water, or is too close to another house, that house will be ignored and not be placed.

Each house that ends up in a valid spot is then assigned some randomized individual properties. Each house will be given a subtle transformation, which causes the scale and rotation to be slightly different, as well as some slight adjustments to the colors on the walls, and a slight adjustment to the town’s chosen roof color. This ensures that each house looks unique, doesn’t look too ugly, and keeps a consistent roof color throughout the entire town.

The horizontal positions for the houses are easy to generate this way, but the vertical positions are a little harder. For this, I needed to find the height of the terrain at that location, which requires the use of some bilinear interpolation on the nearby vertices.

\
**Part 7, Instantiating all of the Data in the Game Scene:**

Now, finally, the TerrainBuilder has finished calculating all of the data we need. All that’s left is to put all this stuff into the world, which… we can’t do on a secondary thread, so we need to pass this back to the main thread first.

The code for this is largely handled in the GameView class, which is located in the “Assets\Scripts\3_Game” folder. It also makes use of the Persistent Data in order to transfer this data between the Loading Scene and the Game Scene.

When the TerrainBuilder is done with Part 6, the terrain-generating thread pushes all of the data into the Persistent Data, and tells the Loading Scene that its time to change to the Game Scene.

Now, in the Game Scene, most of the hard work has already been done, so all it needs to do is comb through the terrain data and instantiate it all into the world.

For most of these items, like the gates, keys, and port towns, it simply looks at the placements of each one, as well as the rotations, scales, and colors if available, and instantiates new instances of pre-made 3D models with those parameters.

For the terrain, however, there isn’t a pre-made model available, so it needs to build one at runtime. It first creates a new 4x1 Texture using the generated color data, then, for each chunk of the terrain data, it creates a new Mesh object, assigns the various vertex-related information to it, applies the generated texture, and moves this chunk to the appropriate place within the world.

It then pulls out the terrain’s boolArray from Part 4 to use with collision detection, and sends the smoother boolArray to the MinimapManager so it can construct the minimap.

And at this point, the terrain has completely generated! 
