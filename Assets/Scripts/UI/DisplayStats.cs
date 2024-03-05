using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time, kills, diamonds;

    private StatsManager statsManager;
    private Image loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        statsManager = StatsManager.instance;
        loadingScreen = GameObject.Find("LoadingScreen").GetComponent<Image>();
        loadingScreen.enabled = false;

        InitializeText();
    }

    private void InitializeText()
    {
        string formatedTimer = FormatTime(statsManager.GetTimer());

        time.text = $"Time  {formatedTimer}";
        kills.text = $"Kills    {statsManager.GetKills()}";
        diamonds.text = $"Diamonds  {statsManager.GetDiamonds()}";
    }

    public string FormatTime(float timeInSeconds)
    {
        // Calculate hours, minutes and seconds from the total seconds
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)(timeInSeconds % 3600) / 60;
        int seconds = (int)(timeInSeconds % 60);

        // Format the time as HH:MM:SS
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        return formattedTime;
    }
}
