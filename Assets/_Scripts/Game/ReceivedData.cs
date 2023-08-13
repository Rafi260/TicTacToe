using UnityEngine;
using Photon.Pun;
using TMPro;


//resoponsible for receiving data from opponent in same room
public class ReceivedData : MonoBehaviourPunCallbacks
{
    [Header("Class references")]
    public _VsOnline _vsOnline;
    public LoadUserInfo loadUserInfo;


    
    [PunRPC]
    void SentEntered(int result)
    {
        _vsOnline.PlayerTwoEntered();
    }

    //Opponents move Coordinate
    [PunRPC]
    void SentDataX(object[] serializedClass)
    {
        SendDataUnit deserializedClass = new SendDataUnit();
        deserializedClass.x = (int)serializedClass[0];
        deserializedClass.y = (int)serializedClass[1];

        _vsOnline.PlayerTwoMove(deserializedClass);

    }
  
    //Opponent wants rematch
    [PunRPC]
    void SentRematch(int result)
    {
        _vsOnline.PlayerTwoRematch();
    }

    //Opponents Uid
    [PunRPC]
    void SentUID(string UID)
    {
        loadUserInfo.LoadReceivedUser(UID);
        Debug.Log(UID);
    }
}
