using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondsUI : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] private TextMeshProUGUI counter;

    public void SetDiamonds(int diamonds)
    {
        counter.text = diamonds.ToString();
    }
}
