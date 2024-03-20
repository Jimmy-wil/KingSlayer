using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessageScript : MonoBehaviour
{
    public void DestroyErrorMessagePanel()
    {
        Destroy(this.gameObject);
    }
}
