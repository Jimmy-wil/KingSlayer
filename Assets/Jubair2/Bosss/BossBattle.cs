using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [SerializeField] private ColliderTrigger colliderTrigger;

    private void Start(){
        colliderTrigger.OnPlayerEnterTrigger += ColliderTrigger_OnPlayerEnterTrigger;

    }
    private void ColliderTrigger_OnPlayerEnterTrigger(object sender,System.EventArgs e){
        StartBattle();
        colliderTrigger.OnPlayerEnterTrigger -= ColliderTrigger_OnPlayerEnterTrigger;

    }
    private void StartBattle(){
        Debug.Log("caca");
    }
}
