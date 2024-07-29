using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class roleGiver : MonoBehaviourPunCallbacks
{
    public tableCards tableCards;
    private const string ROLE_KEY = "role";
    void Awake()
    {
        tableCards = GameObject.Find("tableCards").GetComponent<tableCards>();
    }
    public void LoadGame()
    {
        SetPlayerRole(PhotonNetwork.PlayerList[tableCards.cplayer],"maestru");
        for(int i=0;i<PhotonNetwork.PlayerList.Length;i++)
        {
            if (i != tableCards.cplayer && tableCards.playerIndex < tableCards.playerTextFields.Count)
            {
                if(i<tableCards.cplayer)
                    SetPlayerRole(PhotonNetwork.PlayerList[i],"player"+(i+1).ToString());
                else
                    SetPlayerRole(PhotonNetwork.PlayerList[i],"player"+i.ToString());
            }
        }
    }
    public void SetPlayerRole(Player player, string role)
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new  ExitGames.Client.Photon.Hashtable();
        playerProperties[ROLE_KEY] = role;
        player.SetCustomProperties(playerProperties);
    }
    public string GetPlayerRole(Player player)
    {
        if (player.CustomProperties.ContainsKey(ROLE_KEY))
            return player.CustomProperties[ROLE_KEY] as string;
        return null;
    }
    public bool CheckRole()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (GetPlayerRole(player) == null)
                return false;
        }
        return true;
    }
}
