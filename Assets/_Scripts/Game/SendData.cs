using UnityEngine;
using Photon.Pun;
using TMPro;

//responsible for sending data to opponent in same room
public class SendData : MonoBehaviour
{

    //
    public void SendEntered()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("SentEntered", RpcTarget.Others, 0);
    }

    //sending coordinate value of my move
    public void SendDataX(SendDataUnit sendDataUnit)
    {
        object[] serializedClass = new object[2];
        serializedClass[0] = sendDataUnit.x;
        serializedClass[1] = sendDataUnit.y;

        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("SentDataX", RpcTarget.Others, serializedClass);
    }

    //sending rematch request
    public void SendRematch()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("SentRematch", RpcTarget.Others, 0);
    }

    //sending my Uid
    public void SendUID()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("SentUID", RpcTarget.Others, (string)LoadUserInfo.UserId);
    }
}
