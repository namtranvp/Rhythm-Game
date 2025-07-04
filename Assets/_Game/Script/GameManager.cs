using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region  Properties
    public static event UnityAction<float> OnMovingNote;
    public static event UnityAction<int, int> OnUpdateScore;
    public static event UnityAction<float> OnUpdateProgress;
    public static event UnityAction OnGameOver;
    public static event UnityAction OnBackgroundReactivate;

    [SerializeField] AudioSource audioSource;
    [SerializeField] RectTransform topPoint;
    [SerializeField] RectTransform bottomPoint;
    [SerializeField] RectTransform[] spawnPoints;
    float noteSpeed = 100f;

    NoteDataList loadedNotes;

    [Header("Score")]
    [SerializeField] int pointPerfect = 2;
    [SerializeField] int pointGood = 1;
    private int score = 0;
    private int combo = 0;

    private bool isGameOver = false;
    #endregion

    #region Unity Methods
    void Awake()
    {
        noteSpeed = (topPoint.anchoredPosition.y - bottomPoint.anchoredPosition.y) / CalculatorTime.timeMoveNote;
        LoadNotesFromJson();
    }
    void OnEnable()
    {
        CalculatorTime.OnHitResult += OnHitResult;
        OnGameOver += ResetGame;
    }
    void OnDisable()
    {
        CalculatorTime.OnHitResult -= OnHitResult;
        OnGameOver -= ResetGame;
    }

    void FixedUpdate()
    {
        if (isGameOver || audioSource == null || audioSource.clip == null)
            return;
        float speed = Time.deltaTime * noteSpeed;
        OnMovingNote?.Invoke(speed);
        OnUpdateProgress?.Invoke(audioSource.time / audioSource.clip.length);
    }

    #endregion

    #region Note Spawning
    public void PlayGame()
    {
        isGameOver = false;
        audioSource.Play();
        StartCoroutine(SpawnNotesCoroutine());
    }

    private void SpawnNote(NoteData noteData)
    {
        Vector2 localPos = new Vector2(spawnPoints[noteData.lane].localPosition.x,
            topPoint.localPosition.y);
        PoolManager.Instance.GetNote()
            .SetNote(localPos, noteData, audioSource);
    }

    private void LoadNotesFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(audioSource.clip.name);
        if (jsonFile != null)
        {
            loadedNotes = JsonUtility.FromJson<NoteDataList>(jsonFile.text);
        }
        else
        {
            Debug.LogError("notes.json not found in Resources!");
            loadedNotes = new NoteDataList();
        }
    }
    IEnumerator SpawnNotesCoroutine()
    {
        if (loadedNotes == null || loadedNotes.notes.Count == 0)
            yield break;

        int idx = 0;
        int reactivateIdx = 0;
        while (idx < loadedNotes.notes.Count)
        {
            NoteData note = loadedNotes.notes[idx];
            if (audioSource.time >= note.time - CalculatorTime.timeMoveNote)
            {
                SpawnNote(note);
                idx++;
            }
            if (reactivateIdx < loadedNotes.notes.Count && audioSource.time >= loadedNotes.notes[reactivateIdx].time)
            {
                OnBackgroundReactivate?.Invoke();
                reactivateIdx++;
            }

            yield return null;
        }
    }

    #endregion


    private void OnHitResult(EHitType hitType)
    {
        switch (hitType)
        {
            case EHitType.Perfect:
                score += pointPerfect;
                combo++;
                break;
            case EHitType.Good:
                score += pointGood;
                combo = 0;
                break;
            case EHitType.Missed:
                combo = 0;
                break;
        }

        OnUpdateScore?.Invoke(score, combo);
    }

    private void ResetGame()
    {
        isGameOver = true;
        score = 0;
        combo = 0;
        audioSource.Stop();
        OnUpdateScore?.Invoke(score, combo);
        OnUpdateProgress?.Invoke(0f);
    }
    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }

    public void RestartGame()
    {
        audioSource.Stop();
        PlayGame();
    }
}

public enum ENoteType
{
    Short,
    Long
}