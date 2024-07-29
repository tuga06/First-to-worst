using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

public class CheckScore : MonoBehaviourPun
{
    public roleGiver roles;
    public Dropdowns DD;
    public finishBB fb;
    void Awake()
    {
        roles = GameObject.Find("roleGive").GetComponent<roleGiver>();
        DD = GameObject.Find("masterPlayerDD").GetComponent<Dropdowns>();
        fb = GameObject.Find("finBB").GetComponent<finishBB>();
    }

    public void UpdateScores()
    {
        Player maestruPlayer = GetMaestruPlayer();
        TMP_Dropdown[] maestruDropdowns = GetDropdownsForPlayer(maestruPlayer);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player == maestruPlayer) continue;
            TMP_Dropdown[] playerDropdowns = GetDropdownsForPlayer(player);
            int scoreToAdd = 0;
            for (int i = 0; i < maestruDropdowns.Length; i++)
            {
                if (maestruDropdowns[i].value == playerDropdowns[i].value)
                {
                    scoreToAdd++;
                }
            }
            photonView.RPC("UpdatePlayerScore", RpcTarget.All, player, scoreToAdd);
        }
    }

    [PunRPC]
    private void UpdatePlayerScore(Player player, int scoreToAdd)
    {
        if (player != null)
        {
            int currentScore = player.GetScore();
            player.SetScore(currentScore + scoreToAdd);
            fb.NextGame = true;
        }
    }
    private Player GetMaestruPlayer()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (roles.GetPlayerRole(player) == "maestru")
            {
                return player;
            }
        }
        return null;
    }

    private TMP_Dropdown[] GetDropdownsForPlayer(Player player)
    {
        string role = roles.GetPlayerRole(player);
        TMP_Dropdown[] dropdowns;
        if (DD.playerRoleDropdowns.TryGetValue(role, out dropdowns))
        {
            return dropdowns;
        }
        return null;
    }
}
