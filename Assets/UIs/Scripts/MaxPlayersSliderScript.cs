using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaxPlayersSliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public TMP_Text MaxPlayersText;
    public void UpdateMaxPlayersDisplay()
    {
        MaxPlayersText.text = $"Number of players: {slider.value}";
    }
}
