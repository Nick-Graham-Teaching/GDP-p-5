# Windy

## Introduction

*Windy* is a 3D Fly Simulator, played on PC while controlled by a mobile phone. The main character, called Wendy, could fly, but is not good at it. Players need to control Wendy and fly through bunch of obsticles and solve puzzles. The game is built with Unity.

## Game Contents

### Map
+ There are two floating islands in the game, which are connected by a cloud bridge. And they are the only place for players to take a rest. 
+ Around the islands, there are four smaller islands, surrounded by several clouds. Players may find it highly challenging to stand still on those islands, but the clouds around these islands are going to offer a great deal of benefits to players once they succeed in landing. 
+ Beneath these islands, it is the sea of clouds, which can help players fly in air.

### Take off
Players have three different methods on taking off, which are 
+ Consuming energy, 
+ Falling off from the islands, 
+ Arriving at certain speed when running.

### Motion Mode
+ Walk: On Ground Mode, for walking on the ground.
+ Take Off: Transition Mode, from Walk to Glide/Dive
+ Glide: Flying Mode, stronger buoyancy and a default forward force
+ Dive: Flying Mode, weaker buoyanc and no default forward force
+ Land: Transition Mode, from flying to Walk
+ Trapped: Special Mode, when getting into gravitational zone of waystones

### Waystone
Waystone is a special stone which can float in the air. And they have a kind of gravitational zones, which can capture players when they get into the scope of the special zones. Players then lost control of the character and have to consume certain energy to get rid of restrictions. And don't worry, even players run out of energy, they are still able to run away from gravitational zones by the same method. And sometimes, existance of a waystone may help players win the game.

### Puzzle
The puzzle is to let players find letters in the game, and via touching answer stones on one of the islands to answer the puzzle. (Warning, latter part is about solving the puzzle) Players can solve the puzzle via looking at the shape of different groups of waystones. So it forces players to reach at certain altitude to observe those waystones. It could be really high or really low.

### Energy System
The energy system is to set restrictions on taking off. Once players run out of energy, it wouldn't be easy for them to arrive at a really high altitude than usual. Certainly, players can recharge the energy system. But the only way is to try their best to get close to clouds, while being careful about pushing too much and flying through the clouds. Because it may make you go beyond game boundaries and lose the game.

### Winning/Losing the Game
To win the game, players have to solve the puzzle. And the only way to lose the game, it's players go beyond the game boundaries.

## Game Control

### Direction Keys
+ Walk Mode: control walking directions;
+ Flying Mode 
  + Down: Negative Pitch Rotation and lower the nose of the character
  + Left: Negative Row Rotation, lift right wing and lower left wing
  + Right: Positive Row Rotation, lift left wing and lower right wing
  + Up: In glide mode, it is doing postive pitch rotation but it has time limit, when exceeding the limit, a powerful downward force will be applied to the character and make him fall. In dive mode, up key only makes the character move forward, it can't perform positive pitch. In other words, when none of key is pressed in dive mode, the character doesn't move.

### Jump Key
Jump on the ground, or long pressing the key to consume energy and take off.

### Switch Mode Key
Switch flying mode in the air, and disabled on the ground.

## Mobile Controller

The game allowes players to use their phones like a gamepad. But make sure LAN of the computer where the game runs on is visible to other users in the same LAN. !!!Only avaiable on Android!!!
