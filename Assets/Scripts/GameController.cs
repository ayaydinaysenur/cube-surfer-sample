using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Transform finishPoint;
    private float roadLength;
    private Vector3 playerStartPos;
    public const int gameSpeed = 20;
    public static Action OnGameStart;
    private int score;
    [SerializeField]
    private UIController uiController;
    private bool isGameStarted;
    // Start is called before the first frame update
    public static GameController Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        //if (player == null)
        //{   
        //    player = GameObject.FindGameObjectWithTag("Player");
        //    playerStartPos = player.transform.position;
        //}
        Physics.gravity = new Vector3(0, -500.0F, 0);//burasi gamecontrollera tasinabilir
        //roadLength = Vector3.Distance(finishPoint.position, player.transform.position);
        OnGameStart += StartGame;
    }

    private void Update()
    {
        if (isGameStarted)
        {
            CalculateCoveredRoadRatio();
        }
    }

    public void TriggerPlayerAnimation(string triggerName)
    {
        player.GetComponent<Animator>().SetTrigger(triggerName);
    }

    public void IncreaseScore()
    {
        score++;
        uiController.UpdateScoreText(score);
    }

    public void GameOverFunction(bool isVictory)
    {
        if (isVictory)
        {
            TriggerPlayerAnimation(Constants.TRIGGER_PLAYER_ANIMATION_VICTORY);
        }
        else //fail
        {
            TriggerPlayerAnimation(Constants.TRIGGER_PLAYER_ANIMATION_FALL);
        }
        isGameStarted = false;
        //uiController.OpenReplayButton();
    }

    public void CalculateCoveredRoadRatio()
    {
        if (player!=null)
        {
            float ratio = (float) Vector3.Distance(player.transform.position, playerStartPos) / roadLength;
            uiController.UpdateSlider(ratio);
        }
    }   

    private void StartGame()
    {
        Debug.Log("START GAME");
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerStartPos = player.transform.position;
        }
        roadLength = Vector3.Distance(finishPoint.position, player.transform.position);
        uiController.UpdateSlider(0);
        isGameStarted = true;
    }
}
