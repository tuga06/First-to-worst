using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;


public class tableCards : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public TMP_InputField carte1;
    public TMP_InputField carte2;
    public TMP_InputField carte3;
    public TMP_InputField carte4;
    public TMP_InputField carte5;
    public TMP_Text masterPlayer;
    public TMP_Text Player1;
    public TMP_Text Player2;
    public TMP_Text Player3;
    public TMP_Text Player4;
    public TMP_Text Player5;
    public TMP_Text Player6;
    string[] carti = {
        "Cooking",
        "Beach",
        "Mountain",
        "Forest",
        "Anime",
        "Pizza",
        "Sushi",
        "Comedy",
        "Horror",
        "Documentary",
        "Gragas",
        "China",
        "Jax",
        "Cat",
        "Dog",
        "Parrot",
        "Spaghetti",
        "ChatGPT",
        "Roblox",
        "Airplane",
        "New York City",
        "Bucharest",
        "PS5",
        "Deodorant",
        "Loyalty",
        "Fortnite",
        "Zoo",
        "Lebron James",
        "Michael Jackson",
        "Water",
        "Christmas music",
        "Riding a bike",
        "Picnics",
        "Sarcasm",
        "Coffee shop",
        "Peanut butter",
        "Bucket",
        "Sports",
        "Hairy legs",
        "Cleaning",
        "Iphone",
        "Android",
        "Elon Musk",
        "Jeff Bezos",
        "Donald Trump",
        "Joe Biden",
        "Political Debate",
        "Podcasts",
        "Karaoke",
        "Nicknames",
        "Wake up early",
        "Vegans",
        "Woke",
        "Running",
        "Spiders",
        "Graffiti",
        "Crossdress",
        "Alone time",
        "Subtitles",
        "Loaf (cat)"
    };
    List<string> cartiList;
    int nrCarte = 0;
    public int cplayer = 0;
    public int playerIndex=0;
    public List<TMP_Text> playerTextFields;
    private const byte SendListEventCode = 1;
    public finishBB fb;
    public roleGiver rg;
    public Dropdowns DD;
    void Awake()
    {
        fb = GameObject.Find("finBB").GetComponent<finishBB>();
        DD = GameObject.Find("masterPlayerDD").GetComponent<Dropdowns>();
        rg = GameObject.Find("roleGive").GetComponent<roleGiver>();
    }
    void Start()
    {
        cartiList = new List<string>(carti);
        playerTextFields = new List<TMP_Text> { Player1, Player2, Player3, Player4, Player5, Player6 };
        if(PhotonNetwork.IsMasterClient)
        {
            ShuffleList(cartiList);
            SendShuffledListToAll(cartiList);
        }
        PhotonNetwork.AddCallbackTarget(this);
    }
    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    void SendShuffledListToAll(List<string> shuffledList)
    {
        object[] data = new object[] { shuffledList.ToArray() };
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(SendListEventCode, data, options, SendOptions.SendReliable);
    }
    string SerializeList(List<string> list)
    {
        return string.Join(",", list);
    }
    List<string> DeserializeList(string serializedData)
    {
        return new List<string>(serializedData.Split(','));
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == SendListEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            string[] receivedArray = (string[])data[0];
            List<string> receivedList = new List<string>(receivedArray);
            ReceiveList(receivedList);
        }
    }
    void ReceiveList(List<string> shuffledList)
    {
        cartiList = shuffledList;
        GameStart();
    }
    void ShuffleList(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    void GameStart()
    {
        masterPlayer.text=PhotonNetwork.PlayerList[cplayer].NickName;
        for(int i=0;i<PhotonNetwork.PlayerList.Length;i++)
        {
            if (i != cplayer && playerIndex < playerTextFields.Count)
            {
                TMP_Text currentTextField = playerTextFields[playerIndex];
                if (currentTextField != null)
                {
                    currentTextField.text = PhotonNetwork.PlayerList[i].NickName;
                    playerIndex++;
                }
            }
        }
        if (nrCarte + 5 <= cartiList.Count)
        {
            carte1.text = cartiList[nrCarte];
            carte2.text = cartiList[nrCarte + 1];
            carte3.text = cartiList[nrCarte + 2];
            carte4.text = cartiList[nrCarte + 3];
            carte5.text = cartiList[nrCarte + 4];
            nrCarte += 5;
        }
        else
        {
            StopGame();
        }
        playerIndex = 0;
        fb.ScoreAdding = true;
        rg.LoadGame();
        DD.StartSetUp();
    }
    public void EndGame()
    {
        if(PhotonNetwork.PlayerList.Length == cplayer+1)
        {
            cplayer=0;
        }
        else
            cplayer++;
        foreach (TMP_Text playerTextField in playerTextFields)
        {
            if (playerTextField != null)
            {
                playerTextField.text = "";
            }
        }
        GameStart();
    }
    private void StopGame()
    {
        SceneManager.LoadScene("leaderboard");
    }
}
