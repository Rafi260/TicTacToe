using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;


public class CreateNJoinRoom : MonoBehaviourPunCallbacks
{

    public TMP_InputField roomIdInput;
    public TMP_InputField passwordInput;
    public TMP_InputField JoinroomIdInput;
    public TMP_InputField JoinpasswordInput;

    public TMP_Text errorText;

    bool joining = false;

    public void CreateOrJoinRoom()
    {
        string roomID = roomIdInput.text;
        string password = passwordInput.text;

        if(roomIdInput.text != "")
        {
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
            table.Add("password", password);

            RoomOptions options = new RoomOptions
            {
                IsVisible = true,
                MaxPlayers = 4,
                CustomRoomProperties = table,
                CustomRoomPropertiesForLobby = new string[] { "password" }
            };
            try
            {
                PhotonNetwork.CreateRoom(roomID, options, TypedLobby.Default);
            }
            catch
            {

            }
        }
        else
        {
            errorText.text = "Give a room ID.";
        }

       
    }

    public void Join_Room()
    {
        string roomID = JoinroomIdInput.text;
        if (JoinroomIdInput.text != "")
        {
            joining = true;
            try
            {
                PhotonNetwork.JoinRoom(roomID);
            }
            catch
            {

            }
            
           
        }
        else
        {
            errorText.text = "Give a room ID.";
        }
        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join the room: " + message);
        errorText.text = "Failed to join the room: " + message;
    }

    public override void OnJoinedRoom()
    {
        if(joining)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("password", out object roomPassword))
            {
                string enteredPassword = JoinpasswordInput.text;
                if (roomPassword.ToString() == enteredPassword)
                {
                    Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
                    PhotonNetwork.LoadLevel("Online");
                }
                else
                {
                    Debug.Log("Incorrect password!");
                    errorText.text = "Incorrect password!";
                    PhotonNetwork.LeaveRoom();
                }
            }
            else
            {
                Debug.Log("Room does not have a password!");
                
                PhotonNetwork.LoadLevel("Online");
            }

        }
        else
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LoadLevel("Online");
        }
       
    }
}
