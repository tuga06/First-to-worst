using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Linq;


public class showScore : MonoBehaviourPunCallbacks
{
    public TMP_Text playerList;
    void Start()
    {
        StartCoroutine(WaitForSeconds());
    }
    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(3f);
        CreateLeaderboard();
    }
    private void CreateLeaderboard()
    {
        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();
        foreach (Player player in sortedPlayerList)
        {
            playerList.text += player.NickName + " with the score of " + player.GetScore() + "\n";
        }
    }
}