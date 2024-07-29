using UnityEngine;
using Photon.Pun;

public class startButton : MonoBehaviourPun
{
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public void OnButtonClick()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("game");
    }
}
