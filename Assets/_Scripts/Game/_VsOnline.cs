using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//this class is used when playing in multiplayer
public class _VsOnline : MonoBehaviourPunCallbacks
{
    _GameLogic _gameLogic = new _GameLogic();

    [Header("Class References")]
    public Menu menu;
    public SendData sendData;
    public LoadUserInfo loadUserInfo;
    public _UpdatePlayerData updatePlayerData;


    [HideInInspector] public int currentPlayer = 0;
    [HideInInspector] public int player1;
    [HideInInspector] public bool bRun = true, bDraw = false;
    [HideInInspector] public bool myTurn = true;
    [HideInInspector] public bool gameCanBeStart = false, canGiveMove = false;

    [Header("Ui Components")]
    public GameObject[] gridSceneComponents;
    public List<GameObject> XOs;
    public GameObject[] rematchBtnBefore, rematchBtnAfter;

    [HideInInspector] public bool opponentEntered = false;
    bool playerTwoRematch = false, playerOneRematch = false;
    bool youWon = false, youLost = false, youDraw = false;
    bool opponentLeftRoom = false;


    private void Start()
    {
        //Initializing grid and count array
        _gameLogic.IntializeGrid();



        SetPlayerSymbol();
        sendData.SendEntered();
    }

    private void Update()
    {
        //when two player has entered room, then anyone can give their move.
        if (!gameCanBeStart )
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                canGiveMove = true;
                gameCanBeStart = true;
                sendData.SendUID();
                print("Sent");
            }
        }
    }

    //whoever opens the room, plays as O
    void SetPlayerSymbol()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) player1 = 0;
        else player1 = 1;
    }

    //Called when opponent successfully enter room
    public void PlayerTwoEntered()
    {
        opponentEntered = true;
        
    }


    //Populate grid in _gameLogic
    public void _PlaceMove(int x, int y)
    {
        
        if (bRun)
        {
            myTurn = false;
            SendDataUnit sendDataUnit = new SendDataUnit()
            {
                x = x,
                y = y
            };
            sendData.SendUID();
            sendData.SendDataX(sendDataUnit);
            _gameLogic.PlaceMove(x, y, player1);

            //checks for win lose draw
            bRun = _gameLogic.running;
            bDraw = _gameLogic.draw;

            if (bRun == false)
            {
                if (bDraw == true)
                {
                    loadUserInfo.Draw();
                    youDraw = true;
                    menu.DrawScreen();
                    return;
                }
                else
                {
                    youWon = true;
                    loadUserInfo.Won();
                    menu.SetWinningLine(_gameLogic.winingLine);
                    menu.WinScreen();
                    return;
                }
            }
        }
    }

    //PlayerTwoMove = opponent
    //grid coordinate value from opponent. Populating grid with opponents move coordinate
    //SendDataUnit unit is received from opponent. 
    public void PlayerTwoMove(SendDataUnit unit)
    {
        myTurn = true;

        print(myTurn);
        print(bRun);
        print(canGiveMove);
        print(gameCanBeStart);
        print(updatePlayerData);

        _gameLogic.PlaceMove(unit.x, unit.y, 1 - player1);
        updatePlayerData.PlaceSymbol(unit.x, unit.y, player1);

        //checks win lose draw
        bRun = _gameLogic.running;
        bDraw = _gameLogic.draw;

        if (bRun == false)
        {
            if (bDraw == true)
            {
                loadUserInfo.Draw();
                youDraw = true;
                menu.DrawScreen();
                return;
            }
            else
            {
                loadUserInfo.Lost();
                youLost = true;
                menu.SetWinningLine(_gameLogic.winingLine);
                menu.LoseScreen();
                return;
            }
        }
    }

    //Called if opponents wants rematch
    public void PlayerTwoRematch()
    {
        print("Player2 rematch");

        playerTwoRematch = true;
        if (!playerOneRematch)
        {
            menu.RematchRequ();
        }

        //both player wants rematch
        else
        {
            ResetLevel();
        }

    }

    //Called when you want rematch
    public void PlayerOneRematch()
    {
        print("Player1 rematch");
        playerOneRematch = true;
        sendData.SendRematch();

        //both player wants rematch
        if (playerOneRematch && playerTwoRematch)
        {
            ResetLevel();
        }

    }


    //if both player wants rematch,, resetting all value to their initial state.
    //so that they can play again in a fresh board
    void ResetLevel()
    {
        _gameLogic.Initialize();

        DestroyAllXOs();
        ResetGridSceneComponents();
        myTurn = true;

        menu.CloseResultWindow();

        gameCanBeStart = true;
        canGiveMove = true;

        bRun = true; bDraw = false;

        playerOneRematch = false;
        playerTwoRematch = false;

        youWon = false;
        youLost = false;
        youDraw = false;

        menu.HideLines();

        loadUserInfo.RestartLevel();

        for(int i=0;i<3;i++)
        {
            rematchBtnBefore[i].SetActive(true);
            rematchBtnAfter[i].SetActive(false);
        }
    }

    //adding X O prefab 
    public void AddToXOs(GameObject g)
    {
        XOs.Add(g);
    }

    // destroying X O prefabs if wants to rematch
    void DestroyAllXOs()
    {
        foreach (GameObject g in XOs)
        {
            Destroy(g);
        }
        XOs.Clear();
    }

    //Resetting the grid buttons to true. They were set to false after using them
    void ResetGridSceneComponents()
    {
        foreach (GameObject g in gridSceneComponents)
        {
            g.SetActive(true);
        }
    }

    /*public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (opponentEntered)
        {
            // handle actions when a player leaves the room
            Debug.Log("Player " + otherPlayer.NickName + " has left the room.");
            CheckOnOpponentLeaveRoom();
        }

    }

    //checkes win lose if opponent leave room
    public void CheckOnOpponentLeaveRoom()
    {
        if (!youWon || !youLost || !youDraw )
        {
            if(opponentEntered)
            {
                print("CheckOnOpponentLeaveRoom");
                opponentLeftRoom = true;
                youWon = true;
                loadUserInfo.Won();                     // update your data base
                menu.OpponentLeftRoomScene();
            }
            
        }
    }

    //checkes win lose if you leave room
    public void CheckOnYouLeaveRoom()
    {
        if (!youWon && !youLost && !youDraw && !opponentLeftRoom && opponentEntered)
        {
            youLost = true;
            print("YouLose");
            loadUserInfo.Lost();
        }

    }*/
}
