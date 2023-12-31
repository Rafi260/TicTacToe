# TicTacToe

## Project Details:
* Project Name: Tic Tac Toe Game  
* Game Engine: Unity 3D  
* Platform: Android  
* SDKs: Photon, Firebase, Admob  

## Project Features:
* Single Player Mode (Player vs Computer)  
* Offline Mode (Local Player vs Player)  
* Multiplayer Mode (Player vs Player)  
* Password Protected Match System  
* Player Authentication via Firebase  
* Player Statistics Tracking (Games Played, Games Won, Games Lost)  
* Admob SDK Integration for Ads  

## Below are some screenshots of the app:

### Splash screen

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/32f86f68-cb1f-4193-90bc-ccc35f62b461" width="300px">


### Login Registration page
Player needs to be online to login or register.<br><br>

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/9bd78694-040c-4d2d-8ed9-a47260cdd3aa" width="400px">

### Home / Player Profile page
If player has loged in once, he can come to home in offline afterward.  
Here player can see all the game states. After each multiplayer game, the state gets updated automatically.  
Player can play there different mode.  
At the bottom of the screen, ade is shown using Google Admob.  <br><br>

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/b4f3442d-701c-4633-896c-ba93663ea159" width="300px">

### Menu section
Player is provided quick menu section to instant logout and quit game.  <br><br>

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/c20f2fdd-1ae2-4711-ada0-8bd1ca814205" width="300px">

### Inside Game
In vsComputer, player get to choose his symbol.  
After a game, a Result window is shown to tell player about the result of the game.  <br><br>

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/db6221cd-5a31-4641-ba53-f46d8dbd2e24" width="300px">

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/6700a396-87f0-4037-9564-d02719cfc83a" width="300px">

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/6aaf4d88-66cc-45b6-ba9a-6d160679e2cc" width="300px">

### Multiplayer
Player needs to be connected to network to access this option.  <br><br>
<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/1032b0c9-7ee1-45c2-95c2-686897284051" width="300px">

After connecting to server, he can either create or enter a room with room id and password. Password is optional.  <br><br>
<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/b52a8cba-11d8-45f7-9d88-e3b534381e29" width="300px">

When two player joins a game, the game starts.  <br><br>

<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/1283e6e3-ebfa-41a2-b13c-996d9f130c80" width="300px">

After each game, player can invite for Rematch and other player has to accept the request to play again in the same game.  <br><br>
<img src="https://github.com/Rafi260/TicTacToe/assets/85826615/f83514fc-13b3-4422-aab3-b1ef7ce8b4c4" width="300px">

## Game optimization:

* I reused resources when ever possible. (One main GameLogic class for all mode)
* Used inheritence .
* In whole game, Update() function has been used one time.
* To animate UI, used script.
* Used low size, high quality images.

## UX

* UI is very user friendly.
* Easy to understand what user should do next.
* User can undo any operation anytime.
* Clean, uncluttered user interface.
* All type of error masseges are shown to give user proper feed back.
