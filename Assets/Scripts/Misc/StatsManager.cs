using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance; // Singleton pattern

    [SerializeField] int kills, diamonds;
    [SerializeField] float timer;

    private TogglePauseGame togglePauseGame;
    private Image loadingScreen;

    private void Awake()
    {
        int ManagerAmount = FindObjectsOfType<StatsManager>().Length;

        if (ManagerAmount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        togglePauseGame = GameObject.FindWithTag("UI").GetComponent<TogglePauseGame>();
        loadingScreen = GameObject.Find("LoadingScreen").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (togglePauseGame.GetGameIsPaused() || loadingScreen.enabled) return;

        timer += Time.deltaTime;
    }

    public void RegisterKill() { kills++; }

    public void RegisterDaimond() {  diamonds++; }

    public float GetTimer() { return timer; }

    public int GetKills() { return kills; }

    public int GetDiamonds() { return diamonds; }
}
