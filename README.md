# Blockchain
> ### Check out the itch.io page for more info!
> https://qusr.itch.io/blockchain

> ### Known Bugs
> - In Level 12, it is possible to clip through the door by activating the button and then moving overtop the door. This causes the button to be unpressed and the door to be closed again. However, since the player moves into the door as this happens, they can occupy the same space as the door and just move through it. It is also possible to skip the door completely and move to the exit because there are enough blocks to do so.
> - In Level 13, you can create a shape to go up the left side of the level and reach the goal without pressing the buttons to unlock the door

> ### Features To Implement
> - Undo Button
> - More tutorial-type text in-game
> - More levels
> - Accessibility settings / features
> - Have level number / name show up when paused in-game
> - Add post-processing effects
> - Add particle effects
> - Have free-look feature
> - Add transitions between main menu states

# Version Log
> ### jv1.0
> #### This is the version that was submitted to the GMTK 2021 Game Jam

> ### bv1.1
> #### Major update that adds a lot of the features we wanted to add during the jam into the game
> - Fixed some major puzzle-breaking bugs in certain levels
> - Modified and improved UI
> - Disabled block colliders when they fall off so there is no delay/interferance when moving
> - Fixed sound effects being played multiple times over each other if two objects did something at the same time
> - Made music fade in and out instead of hard-cutting at the end of songs
> - Made sure two songs don't play back-to-back
> - --- Made level name and number show up in level complete and pause menus
> - Made background move with mouse on the title screen
> - Made background move slightly in-game when the player moves
> - --- Added transitions between main menu states
> - --- Added some post-processing effects
> - Added "floating" animation to UI text and buttons
> - Made UI buttons turn a random block color when the mouse is hovered over them
> - The level now auto-restarts when the player block dies
> - --- An undo button was added to help with puzzle solving
> - --- You can now free-look around the player by holding down the right mouse button and moving the mouse around
> - Implemented multiple developer tools
>	- Added bitmasking for surrounding edge tiles to limit the time it takes to build levels
>	- --- Added grouping system for buttons/doors to make it easier to build levels
>	- --- General code cleanup and improvements