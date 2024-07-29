using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Dropdowns : MonoBehaviourPunCallbacks
{
    public roleGiver roles;
    public TMP_Dropdown[] playerDropdowns;
    public Dictionary<string, TMP_Dropdown[]> playerRoleDropdowns;
    public TMP_Dropdown[] dropdownsForRole;
    string role;
    string currentPlayerRole;
    void Awake()
    {
        roles = GameObject.Find("roleGive").GetComponent<roleGiver>();
        playerRoleDropdowns = new Dictionary<string, TMP_Dropdown[]>();
    }

    public void StartSetUp()
    {
        foreach (var dropdown in playerDropdowns)
        {
            dropdown.gameObject.SetActive(false);
            dropdown.value = 0;
        }
        StartCoroutine(WaitForSeconds());
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2f);
        
        if (roles.CheckRole())
            Debug.Log("Roles assigned");
        else
            Debug.Log("Roles not assigned");
        SetupDropdowns();
    }

    public void SetupDropdowns()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            role = roles.GetPlayerRole(player);
            currentPlayerRole = roles.GetPlayerRole(PhotonNetwork.LocalPlayer);
            if (playerRoleDropdowns.TryGetValue(role, out dropdownsForRole))
            {
                foreach (TMP_Dropdown dropdown in dropdownsForRole)
                {
                    dropdown.gameObject.SetActive(true);
                    if (role == currentPlayerRole)
                        dropdown.interactable = true;
                    else
                        dropdown.interactable = false;
                }
            }
        }
    }
    public void AssignDropdownsToRoles(string role, TMP_Dropdown[] dropdowns)
    {
        playerRoleDropdowns[role] = dropdowns;
    }
}