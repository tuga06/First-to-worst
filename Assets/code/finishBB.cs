using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class finishBB : MonoBehaviourPun
{
    public roleGiver roles;
    public Dropdowns DD;
    public CheckScore CS;
    public tableCards tb;
    private Dictionary<Player, bool> playerLockStates;
    int[] currentPlayerAnswers;
    public bool ScoreAdding;
    public bool NextGame = false;
    void Awake()
    {
        roles = GameObject.Find("roleGive").GetComponent<roleGiver>();
        DD = GameObject.Find("masterPlayerDD").GetComponent<Dropdowns>();
        CS = GameObject.Find("CheckScoreOBJ").GetComponent<CheckScore>();
        tb = GameObject.Find("tableCards").GetComponent<tableCards>();
        playerLockStates = new Dictionary<Player, bool>();
        foreach (Player player in PhotonNetwork.PlayerList)
            playerLockStates[player] = false;
    }
    public void OnClick()
    {
        if(AreValuesSet() && ScoreAdding)
        {
            ScoreAdding = false;
            currentPlayerAnswers = new int[DD.dropdownsForRole.Length];
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string role = roles.GetPlayerRole(player);
                string currentPlayerRole = roles.GetPlayerRole(PhotonNetwork.LocalPlayer);
                if (DD.playerRoleDropdowns.TryGetValue(role, out DD.dropdownsForRole))
                {
                    for (int i = 0; i < DD.dropdownsForRole.Length; i++)
                    {
                        TMP_Dropdown dropdown = DD.dropdownsForRole[i];
                        if (role == currentPlayerRole)
                        {
                            dropdown.interactable = false;
                            currentPlayerAnswers[i] = dropdown.value;
                        }
                    }
                }
            }
            photonView.RPC("NotifyLockState", RpcTarget.All, PhotonNetwork.LocalPlayer, true);
        }
        else
            Debug.Log("nu au fost setate");
        if(NextGame && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NextLevel", RpcTarget.All);
        }
    }
    [PunRPC]
    private void NextLevel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerLockStates[player] = false;
        }
        NextGame = false;
        tb.EndGame();
    }
    public bool AreValuesSet()
    {
        bool v1=false,v2=false,v3=false,v4=false,v5=false;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string role = roles.GetPlayerRole(player);
            string currentPlayerRole = roles.GetPlayerRole(PhotonNetwork.LocalPlayer);
            if (DD.playerRoleDropdowns.TryGetValue(role, out DD.dropdownsForRole))
            {
                foreach (TMP_Dropdown dropdown in DD.dropdownsForRole)
                {
                    if (role == currentPlayerRole)
                    {
                        if(dropdown.value==1)
                            v1=true;
                        else if(dropdown.value==2)
                            v2=true;
                        else if(dropdown.value==3)
                            v3=true;
                        else if(dropdown.value==4)
                            v4=true;
                        else if(dropdown.value==5)
                            v5=true;
                    }
                }
            }
        }
        if(v1 && v2 && v3 && v4 && v5)
            return true;
        else
            return false;
    }
    [PunRPC]
    private void NotifyLockState(Player player, bool isLocked)
    {
        if (playerLockStates.ContainsKey(player))
        {
            playerLockStates[player] = isLocked;
            if (AreAllPlayersLocked())
            {
                photonView.RPC("ShareAnswers", RpcTarget.All, PhotonNetwork.LocalPlayer, currentPlayerAnswers);
                StartCoroutine(WaitForSeconds());
            }
        }
    }
    [PunRPC]
    private void ShareAnswers(Player player, int[] answers)
    {
        string role = roles.GetPlayerRole(player);
        if (DD.playerRoleDropdowns.TryGetValue(role, out DD.dropdownsForRole))
        {
            for (int i = 0; i < DD.dropdownsForRole.Length; i++)
            {
                TMP_Dropdown dropdown = DD.dropdownsForRole[i];
                dropdown.value = answers[i];
            }
        }
    }
    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(3f);
        CS.UpdateScores();
    }
    private bool AreAllPlayersLocked()
    {
        foreach (var lockState in playerLockStates.Values)
        {
            if (!lockState)
            {
                return false;
            }
        }
        return true;
    }
}
