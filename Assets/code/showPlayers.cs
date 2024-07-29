using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class showPlayers : MonoBehaviourPunCallbacks
{
    public TMP_Text playerList;
    void Start()
    {
        UpdatePlayerList();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    private void UpdatePlayerList()
    {
        playerList.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.text += player.NickName + "\n";
        }
    }
}
