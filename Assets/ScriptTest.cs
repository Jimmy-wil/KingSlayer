using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScriptTest : NetworkBehaviour
{

    private void Start()
    {
        MethServerRpc("I'm dead");
    }

    [ServerRpc(RequireOwnership=false)]
    private void MethServerRpc(string s)
    {
        Debug.Log(s);
        MethClientRpc(s);
    }

    [ClientRpc]
    private void MethClientRpc(string s)
    {
        Debug.Log(s);
    }

}
