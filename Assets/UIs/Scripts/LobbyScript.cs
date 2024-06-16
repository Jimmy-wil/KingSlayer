using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScript : NetworkBehaviour
{
    public NetworkObject PlayerCharacterPrefab;
    public GameObject PlayerSpawnPoints;
    private NetworkList<int> SpawnPointList;

    public Canvas Canvas;
    public Camera currentCamera;

    public GameObject MainMenuGUI;
    public GameObject LoadingScreenGUI;
    public BlackScreenFadeScript BlackScreen;
    public GameObject GameGUI;
    public GameObject WinScreen;

    public GameObject MessagePanelObject;
    public GameObject MainMenu;
    public GameObject CreateLobbyMenu;
    public GameObject CurrentLobby;
    public GameObject PlayMenu;

    public TMP_Text HostName;
    public TMP_Text LobbyName;
    public Button StartButton;
    public Toggle HostPrivateToggle;
    public GameObject ListOfPlayers;

    public TMP_Text LobbyCode;
    public TMP_InputField _playerName;


    public TMP_InputField _CodeInput;
    public TMP_InputField _lobbyNameInput;
    public Slider _maxPlayersInput;
    public Toggle _private;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbyUpdateTimer;

    private UnityTransport _transport;


    // ----- Initialize unity services ------
    private async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // DisplayErrorMessage("Welcome to my game!");
    }

    private async void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();

        await Authenticate();

        SpawnPointList = new NetworkList<int>();
    }

    private void Start()
    {
        MainMenuGUI.SetActive(true);
        LoadingScreenGUI.SetActive(false);
        GameGUI.SetActive(false);

        NetworkManager.OnClientStopped += OnClientDisconnected;
        NetworkManager.OnServerStopped += OnServerDisconnected;

    }

    private void Update()
    {
        HandleLobbyHeartbeat(); // send heartbeat to hostLobby
        HandleLobbyPollForUpdates(); // get updates from joinedLobby and leave lobby if host left

    }

    // ---------------------------------------
    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer += heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                foreach (var client in NetworkManager.ConnectedClients)
                {
                    Debug.Log(client.ToString());
                }
            }
        }

    }
    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer += lobbyUpdateTimerMax;

                if (!IsPlayerInLobby())
                {

                    DisplayErrorMessage("You have left the lobby because the host left or disconnected!");
                    joinedLobby = null;
                    hostLobby = null;

                    CurrentLobby.SetActive(false);
                    MainMenu.SetActive(true);

                    NetworkManager.Shutdown(); // stop client/host

                }
                else
                {
                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                    joinedLobby = lobby;

                    DisplayListOfPlayers();

                }

            }
        }
    }

    private void DisplayErrorMessage(string messageContent)
    {
        GameObject clone = Instantiate(MessagePanelObject);
        GameObject child = clone.transform.GetChild(0).gameObject;
        TMP_Text TMPtext = child.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        TMPtext.text = messageContent;

        clone.transform.SetParent(Canvas.transform, false);

        clone.SetActive(true);
    }

    private bool IsPlayerInLobby()
    {
        if (joinedLobby != null && joinedLobby.Players != null)
        {
            foreach (var player in joinedLobby.Players)
            {
                if (player.Id == AuthenticationService.Instance.PlayerId)
                {
                    return true;
                }
            }
        }

        return false;
    }
    // Display current players
    private void DisplayListOfPlayers()
    {
        try
        {
            if (joinedLobby != null)
            {
                // Displays all current players
                for (int i = 0; i < joinedLobby.Players.Count; i++)
                {
                    ListOfPlayers.transform.GetChild(i).gameObject.SetActive(true);
                    GameObject playerPanel = ListOfPlayers.transform.GetChild(i).gameObject;
                    playerPanel.transform.GetChild(1).gameObject.SetActive(false);
                    playerPanel.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = joinedLobby.Players[i].Data["PlayerName"].Value;
                }
                // Display the rest (no players)
                for (int i = joinedLobby.Players.Count; i < joinedLobby.MaxPlayers; i++)
                {
                    ListOfPlayers.transform.GetChild(i).gameObject.SetActive(true);
                    GameObject playerPanel = ListOfPlayers.transform.GetChild(i).gameObject;
                    playerPanel.transform.GetChild(1).gameObject.SetActive(true);
                    playerPanel.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "...";
                }
                for (int i = joinedLobby.MaxPlayers; i < 8; i++)
                {
                    ListOfPlayers.transform.GetChild(i).gameObject.SetActive(false);
                }

            }

        } catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private Player GetPlayer()
    {
        return new Player()
        {
            Data = new Dictionary<string, PlayerDataObject> {
               {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _playerName.text) } }
        };
    }


    // list all current players in a lobby through Debug.Log
    public void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in " + lobby.Players[0].Data["PlayerName"].Value + "'s lobby:");
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " : " + player.Data["PlayerName"].Value);
        }
    }


    // --------------------- create and join lobby ---------------------
    public async void CreateLobby()
    {
        try
        {
            // Check correct lobby name
            if (string.IsNullOrWhiteSpace(_lobbyNameInput.text))
            {
                DisplayErrorMessage("Invalid lobby name!");
                return;
            }


            Allocation a = await RelayService.Instance.CreateAllocationAsync((int)_maxPlayersInput.value);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);



            // defining lobby options from player input
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject> { { "JoinCodeKey", new DataObject(DataObject.VisibilityOptions.Public, joinCode) } },
                IsPrivate = _private.isOn,
                Player = GetPlayer() // the host's
            };

            // create the lobby
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyNameInput.text, (int)_maxPlayersInput.value, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;

            Debug.Log("Created lobby with name " + _lobbyNameInput.text + ", code: " + hostLobby.LobbyCode);
            Debug.Log("Allocation code: " + hostLobby.Data["JoinCodeKey"].Value);
            LobbyCode.text = hostLobby.LobbyCode;
            LobbyCode.gameObject.SetActive(true);


            _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();

            OnLobbyJoined();

            CreateLobbyMenu.SetActive(false);




        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to create lobby!");
        }

    }

    public async void JoinLobbyByCode()
    {

        try
        {
            // if input code is blank, do nothing
            if (string.IsNullOrWhiteSpace(_CodeInput.text) || _CodeInput.text.Length != 6)
            {
                DisplayErrorMessage("Invalid input!");
                return;
            }

            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions() { Player = GetPlayer() };

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(_CodeInput.text, joinLobbyByCodeOptions);

            OnLobbyJoined();

            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinedLobby.Data["JoinCodeKey"].Value);
            _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

            NetworkManager.Singleton.StartClient(); // you join as a client

            Debug.Log("Joined " + joinedLobby.Players[0].Data["PlayerName"].Value + " lobby with code " + _CodeInput.text + "!");

            PrintPlayers(joinedLobby);


        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to join a lobby by code!");
        }

    }
    public async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions() { Player = GetPlayer() };

            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);

            OnLobbyJoined();

            Debug.Log("Quick joining a lobby");
            PrintPlayers(joinedLobby);

            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinedLobby.Data["JoinCodeKey"].Value);
            _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

            NetworkManager.Singleton.StartClient(); // you join as a client

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to quick join a lobby!");
        }
    }

    public void OnLobbyJoined()
    {
        if (joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
        {
            HostPrivateToggle.isOn = _private.isOn;
            HostPrivateToggle.gameObject.SetActive(true);
            StartButton.gameObject.SetActive(true);
        }
        else
        {
            HostPrivateToggle.gameObject.SetActive(false);
            StartButton.gameObject.SetActive(false);

        }

        LobbyName.text = joinedLobby.Name;
        HostName.text = joinedLobby.Players[0].Data["PlayerName"].Value + "'s lobby";


        PlayMenu.gameObject.SetActive(false);
        CurrentLobby.SetActive(true);

    }

    public async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryOptions = new QueryLobbiesOptions()
            {
                // Only show lobbies that contains >0 players
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },

                // Order by number of players
                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.AvailableSlots)
                }

            };

            // Query all lobbies with queryOptions as a filter
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryOptions);


            // display all lobbies on console 
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (var lobby in queryResponse.Results)
            {
                Debug.Log("Lobby name: " + lobby.Name + " | " + lobby.Players.Count + "/" + lobby.MaxPlayers + " | IsPrivate: " + lobby.IsPrivate);

            }

        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to list lobbies!");
        }

    }



    // --------------------- start the game -------------------

    // [-------- TEST PURPOSES --------
    [ClientRpc]
    void PongClientRpc(string content)
    {
        Debug.Log(content);
    }
    [ServerRpc]
    void MyServerRpc(string content)
    {
        Debug.Log(content);
        PongClientRpc("Pong");
    }

    public void Ping()
    {
        MyServerRpc("Ping");
    }
    // ---------------------------------]

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            Debug.Log("Scene loaded");
            StartCoroutine(OnSceneLoadedCooldown());
        }
    }
    private IEnumerator OnSceneLoadedCooldown()
    {
        Debug.Log("OnSceneLoadedCooldown");

        yield return new WaitForSeconds(2);

        BlackScreen.Fade();

        LoadingScreenGUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(3);

        GameGUI.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        if (hostLobby.Players.Count <= 1)
        {
            DisplayErrorMessage("Not enough players in your lobby!");
            // return;
        }

        StartGameServerRpc();

    }
    
    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc() 
    {
        NetworkManager.SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        
        SpawnPointList.Clear();
        for (int i = 0; i < _maxPlayersInput.value; i++)
        {
            SpawnPointList.Add(i+1);
        }
        ShuffleList(SpawnPointList);

        StartGameClientRpc();
    }

    private void ShuffleList(NetworkList<int> networkList)
    {

        for (int i = 0; i < networkList.Count; i++)
        {
            int i1 = UnityEngine.Random.Range(0, networkList.Count-1);
            int i2 = UnityEngine.Random.Range(0, networkList.Count-1);

            (networkList[i1], networkList[i2]) = (networkList[i2], networkList[i1]);

        }

    }
    
    [ClientRpc]
    private void StartGameClientRpc()
    {
        CurrentLobby.SetActive(false);
        MainMenuGUI.gameObject.SetActive(false);
        LoadingScreenGUI.gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded; // trigger OnSceneLoaded if sceneLoaded (subscribe)

        SpawnPlayerServerRpc();
    }


    [ServerRpc(RequireOwnership=false)]
    private void SpawnPlayerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var PlayerCharacter = NetworkManager.SpawnManager.InstantiateAndSpawn(PlayerCharacterPrefab, clientId);

        PlayerCharacter.transform.position = PlayerSpawnPoints.transform.GetChild(SpawnPointList[0]).transform.position;
        SpawnPointList.RemoveAt(0);

    }

    // ------------------- update lobby display -------------------
    public async void UpdateLobby()
    {
        try
        {
            // redefining lobby options from player input
            UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions()
            {
                IsPrivate = _private.isOn
            };
            // do not forget to check if maxplayersInput is correct
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, updateLobbyOptions);
            joinedLobby = hostLobby;
            PrintPlayers(hostLobby);

        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to update lobby!");
        }
        
    }

    public async void UpdatePlayerName()
    {
        try
        {
            UpdatePlayerOptions updatePlayerOptions = new UpdatePlayerOptions()
            {
                Data = new Dictionary<string, PlayerDataObject> {
                    { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, _playerName.text) }     
                }
            };

            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, updatePlayerOptions);

        } catch (LobbyServiceException e) 
        { 
            Debug.Log(e);
            DisplayErrorMessage("Failed to update player's name!");
        }
    }

    // ------------------ disconnection ---------------------------
    // return to main menu
    public void BackToMainMenu()
    {
        MainMenu.SetActive(true);
        MainMenuGUI.gameObject.SetActive(true);
        GameGUI.gameObject.SetActive(false);
        WinScreen.transform.GetChild(0).gameObject.SetActive(false);
    }


    public async void LeaveLobby()  
    {
        try
        {
            if (joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                Debug.Log("Deleting lobby...");
                StartCoroutine(DeleteLobby());
            }
            else
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                Debug.Log("You left " + joinedLobby.Players[0].Data["PlayerName"].Value + "'s lobby!");

            }
            NetworkManager.Shutdown();
            joinedLobby = null;
            hostLobby = null;

        } catch (LobbyServiceException e) 
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to leave lobby!");
        }
    }

    public async void KickPlayer(int i)
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[i].Id);
            Debug.Log("You have kicked from your lobby!");

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to kick player!");
        }
    }

    public async void DisconnectAllPlayersInLobby()
    {
        try
        {
            foreach(var player in joinedLobby.Players)
            {
                if(player.Id != joinedLobby.HostId)
                {
                    Debug.Log("porto");
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, player.Id);
                    Debug.Log("rico");

                }

            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to disconnect player!");
        }
    }

    // not needed for now
    public async void MigrateLobbyHost()
    {
        try
        {
            // redefining lobby options from player input
            UpdateLobbyOptions updateLobbyOptions = new UpdateLobbyOptions()
            {
                HostId = joinedLobby.Players[1].Id
            };

            // do not forget to check if maxplayersInput is correct

            hostLobby = await LobbyService.Instance.UpdateLobbyAsync(hostLobby.Id, updateLobbyOptions);

            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to migrate lobby's host!");
        }
    }

    public IEnumerator DeleteLobby()
    {
        DisconnectAllPlayersInLobby();
        while (joinedLobby.Players.Count != 1)
        {
            yield return new WaitForSeconds(0.2f);

        }
        DeleteLobbyHandler();
    }
    public async void DeleteLobbyHandler()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

            joinedLobby = null;
            hostLobby = null;

        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            DisplayErrorMessage("Failed to delete lobby!");
        }
    }

    // gets called by event Network.OnServerStopped in Update()
    public void OnServerDisconnected(bool obj)
    {
        try
        {
            Debug.Log("Server disconnected! Shutting down connection.");

            if (SceneManager.GetSceneAt(1).isLoaded)
                SceneManager.UnloadSceneAsync(1);

        }
        catch
        {
            BackToMainMenu();

        }
    }

    private void OnClientDisconnected(bool obj)
    {
        try
        {
            Debug.Log("You've got disconnected as a client pal, client gens man...");

            hostLobby = null;
            joinedLobby = null;

            Debug.Log("End of heya");

            if (SceneManager.GetSceneAt(1).isLoaded)
                SceneManager.UnloadSceneAsync(1);

            BackToMainMenu();
        }
        catch
        {
            BackToMainMenu();
        }
    }

    /*
    private async void OnDestroy()
    {
        try
        {
            if (joinedLobby != null)
            {
                StopAllCoroutines();
                if (joinedLobby.HostId == AuthenticationService.Instance.PlayerId)
                {
                    StartCoroutine (DeleteLobby());
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                }
            }
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    */
}
