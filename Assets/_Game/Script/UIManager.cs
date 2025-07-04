using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hitText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] Image progressImage;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Image backgroundImage;
    void Start()
    {

    }

    void OnEnable()
    {
        CalculatorTime.OnHitResult += OnHitResult;
        GameManager.OnUpdateScore += UpdateScore;
        GameManager.OnUpdateProgress += UpdateProgress;
        GameManager.OnGameOver += ShowGameOverPanel;
        GameManager.OnBackgroundReactivate += BackgroundReactivate;
    }
    void OnDisable()
    {
        CalculatorTime.OnHitResult -= OnHitResult;
        GameManager.OnUpdateScore -= UpdateScore;
        GameManager.OnUpdateProgress -= UpdateProgress;
        GameManager.OnGameOver -= ShowGameOverPanel;
        GameManager.OnBackgroundReactivate -= BackgroundReactivate;
    }
    private void OnHitResult(EHitType hitType)
    {
        hitText.transform.localRotation = Quaternion.identity;
        hitText.transform.localScale = Vector3.zero * 0.1f;
        hitText.text = hitType.ToString();
        hitText.transform.DOShakeRotation(0.1f, new Vector3(0, 0, 30), vibrato: 10, randomness: 90);
        hitText.transform.DOScale(1.5f, 0.2f);
    }

    private void UpdateScore(int score, int combo)
    {
        scoreText.transform.localScale = Vector3.one;
        comboText.transform.localScale = Vector3.one;

        scoreText.text = score.ToString();
        scoreText.transform.DOShakeRotation(0.1f, new Vector3(0, 0, 30), vibrato: 10, randomness: 90);
        scoreText.transform.DOScale(1.5f, 0.2f);

        if (combo > 1)
        {
            comboText.text = "X " + combo.ToString();
            comboText.transform.DOScale(1.5f, 0.1f);
        }
        else
        {
            comboText.text = string.Empty;
        }
    }

    private void UpdateProgress(float value)
    {
        progressImage.fillAmount = value;
    }

    private void BackgroundReactivate()
    {
        backgroundImage.DOKill();
        backgroundImage.DOFade(0.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    private void ShowGameOverPanel()
    {
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }
}
