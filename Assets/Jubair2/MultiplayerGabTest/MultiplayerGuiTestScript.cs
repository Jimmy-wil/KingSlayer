using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MultiplayerGuiTestScript : NetworkBehaviour
{
    [ServerRpc]
    private void StartGameServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("JubairSceneTest2", LoadSceneMode.Additive);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        StartGameServerRpc();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
