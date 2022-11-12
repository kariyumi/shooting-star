# Shooting Star

<img width="205" alt="Screen Shot 2022-11-11 at 20 47 24" src="https://user-images.githubusercontent.com/68155326/201451201-918986ee-0c92-4f18-a2cb-06ae64222180.png">

## Overview

This repository is the home of Shooting Star, a basic spaceship shooting game. Shooting Star is made in Unity and is driven by PlayFab's back-end technology.

## Some considerations

### About Authentication

<img width="206" alt="Screen Shot 2022-11-11 at 20 48 02" src="https://user-images.githubusercontent.com/68155326/201451234-cee50ce2-38d7-45f2-924f-514ce748a85f.png">

Authentication is made with the device ID. I did not use any kind of username, because of that, everything related to a username uses the device ID.
If you logged in one time, this step will be made automatically the next time.

### About Star Garage

<img width="203" alt="Screen Shot 2022-11-11 at 20 47 47" src="https://user-images.githubusercontent.com/68155326/201451259-fe3dbb88-1296-4b1d-9a7f-bb38756ed321.png">

Star Garage is the place where you can buy an upgrade to your spaceship. This upgrade is a limited-by-time shield. 

When you buy a shield for five stars, our engineers start to construct it. They take one minute to make it (it is an adjustable value, but it is set one minute to be okay to wait in tests). But, if you can not wait, you can pay a little fee of five red stars to them and they will construct it instantly.

There are + buttons to get more stars. In a final project, there would be a window for purchasing some star packs in exchange for real money, but I did not implement that part. So, right now, you can just push those buttons and receive ten stars.

### About Leaderboard

<img width="202" alt="Screen Shot 2022-11-11 at 20 48 11" src="https://user-images.githubusercontent.com/68155326/201451274-d116e7aa-d05a-4878-916f-c23d57dae1b1.png">

In the leaderboard, the top ten scores will be displayed, but only one per player. As I told you before, I did not implement a nickname system, so your name will appear as your device ID.

### About Gameplay

<img width="206" alt="Screen Shot 2022-11-11 at 20 47 14" src="https://user-images.githubusercontent.com/68155326/201451296-d7bdf995-e942-4e61-812d-da1b6a87b353.png">

In the gameplay, you need to shoot the alien spaceships. Sometimes, some stars will appear and you can collect them, but only the white ones. The red ones are special. 

There is a virtual joystick for movement, a button to open fire, and another button to use the shield. 

In this prototype, there is no level increase. So enemies and stars will appear in a constant cycle. And there is only one type of enemy. 
You make a score when you hit an enemy with lasers or with yourself (which I truthfully do not recommend).

### About Game Over

<img width="203" alt="Screen Shot 2022-11-11 at 20 47 36" src="https://user-images.githubusercontent.com/68155326/201451336-e0c44013-a741-47a7-99e7-59b921f1f5b4.png">

The game is over when you hit an enemy with yourself. Then you can choose between buying some shields, playing again, or check if you are the best Star Shooter ever. 

## Final Words
Thank you so much for your time. :)

