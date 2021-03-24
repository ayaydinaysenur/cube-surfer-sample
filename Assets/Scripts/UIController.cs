using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Image loadingPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider slider;
    Scene lastScene;
    // Start is called before the first frame update
    void Awake()
    {
        startButton.onClick.AddListener(delegate () { StartButtonFunction(); });
        replayButton.onClick.AddListener(delegate () { ReplayButtonFunction(); });
        startButton.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 1f).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
        lastScene = SceneManager.GetActiveScene();
    }

    public void StartButtonFunction()
    {
        startButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        GameController.OnGameStart?.Invoke();
    }

    public void ReplayButtonFunction()
    {
        loadingPanel.gameObject.SetActive(true);
        StartCoroutine(SceneSwitch());
    }

    IEnumerator SceneSwitch()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        yield return null;
        SceneManager.UnloadSceneAsync(lastScene);
        lastScene = SceneManager.GetActiveScene();
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void OpenStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    public void OpenReplayButton()
    {
        replayButton.gameObject.SetActive(true);
    }

    public void UpdateSlider(float coveredRoadRatio)
    {
        slider.value = coveredRoadRatio;
    }
}
