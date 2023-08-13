using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Class references")]
    public LoadUserInfo loadUserInfo;
    public _VsOnline vsOnline;
    public _VsComputer _vsComputer;
    public AdManager adManager;

    [Header("Ui Components")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject drawScreen;
    public GameObject opponentLeftRoomScene;
    public GameObject rematch;
    public List<GameObject> winingLines;

    [Header("Ui scale")]
    public Vector3 targetScale;
    

    private void Start()
    {
        if(loadUserInfo != null)
        {
            loadUserInfo.RestartLevel();
        }
    }



    //=========================  Scene Manager ==============================
    public void MultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobbyScene");
    }
    public void Lobby()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void LoadingScene()
    {
        
        LeaveCurrentRoom();

        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MultiplayerLobbyScene");
        }
        else
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public void VsComputerScene()
    {
        
        SceneManager.LoadScene("Computer");
    }
    public void Logout()
    {
        PlayerPrefs.DeleteKey("UserID");
        SceneManager.LoadScene("LoginRegScene");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void OfflineScene()
    {
        SceneManager.LoadScene("Offline");
    }


    //============================= UI manager ==============================

    public void WinScreen()
    {
        Invoke("WinDelay", 1);
        ScaleEffect(winScreen);
    }
    void WinDelay()
    {
        winScreen.SetActive(true);
    }
    public void LoseScreen()
    {
        Invoke("LoseDelay", 1);

    }
    void LoseDelay()
    {
        loseScreen.SetActive(true);
        ScaleEffect(loseScreen);
    }
    public void DrawScreen()
    {
        Invoke("DrawDelay", 1);

    }
    void DrawDelay()
    {
        drawScreen.SetActive(true);
        ScaleEffect(drawScreen);
    }

    public void RematchRequ()
    {
        print("h");
        rematch.SetActive(true);
    }

    public void OpponentLeftRoomScene()
    {
        Invoke("OpponentLeftDelay", 1);

    }
    void OpponentLeftDelay()
    {
        opponentLeftRoomScene.SetActive(true);
        ScaleEffect(opponentLeftRoomScene);
    }


    //Closes all windows
    public void CloseResultWindow()
    {
        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);
        if (drawScreen) drawScreen.SetActive(false);
        if (rematch) rematch.SetActive(false);
        if (opponentLeftRoomScene) opponentLeftRoomScene.SetActive(false);
    }

    public void SetWinningLine(int lineIndex)
    {
        winingLines[lineIndex].SetActive(true);
    }

    public void ChoseSymbol(int a)
    {
        _vsComputer.ChoosePlayerSymbol(a);
    }

    public void HideLines()
    {
        foreach (var l in winingLines)
        {
            l.SetActive(false);
        }
    }


    //======================= Functions =========================


    public void LeaveCurrentRoom()
    {
        // Check if the player is in a room before leaving
        if (PhotonNetwork.InRoom)
        {
            // Leave the current room and return to the lobby or disconnect from the server
            PhotonNetwork.LeaveRoom();
            vsOnline.CheckOnYouLeaveRoom();
        }
        else
        {
            Debug.Log("Player is not in a room.");
        }
    }

    public void Rematch()
    {
        vsOnline.PlayerOneRematch();
    }


    void ScaleEffect(GameObject g)
    {
        LeanTween.scale(g, targetScale, .3f);
    }


}

