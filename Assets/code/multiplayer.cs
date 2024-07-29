using Photon.Realtime;
using TMPro;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;

public class multiplayer : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerName;
    public TMP_InputField createRoomInp;
    public TMP_InputField joinRoomInp;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (playerName != null)
        {
            playerName.characterLimit = 27;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(createRoomInp.text) && !string.IsNullOrEmpty(playerName.text))
            CreateRoom(createRoomInp.text);
        if (Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(joinRoomInp.text) && !string.IsNullOrEmpty(playerName.text))
            JoinRoom(joinRoomInp.text);
    }
    private void CreateRoom(string roomName)
    {
        PhotonNetwork.LocalPlayer.SetScore(0);
        PhotonNetwork.LocalPlayer.NickName = playerName.text;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 7 };
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
    private void JoinRoom(string roomName)
    {
        PhotonNetwork.LocalPlayer.SetScore(0);
        PhotonNetwork.LocalPlayer.NickName = playerName.text;
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnCreatedRoom()
    {
        SceneManager.LoadScene("waiting");
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("waiting");
    }
}
