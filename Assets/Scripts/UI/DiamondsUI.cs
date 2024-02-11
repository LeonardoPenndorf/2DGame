using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondsUI : MonoBehaviour
{
    // [SerializeField] variables
    [SerializeField] private TextMeshProUGUI counter;

    // private variables
    private PlayerManager playerManager;
    private int currentDiamonds;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance; // there is only ever a single player manager instance
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDiamonds != playerManager.GetDiamonds())
        {
            currentDiamonds = playerManager.GetDiamonds();
            counter.text = currentDiamonds.ToString();
        }
    }
}
