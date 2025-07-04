using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Note : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    NoteData noteData;
    AudioSource audioSource;
    float ySize;
    Image image;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        ySize = rectTransform.sizeDelta.y;
    }
    void OnEnable()
    {
        GameManager.OnMovingNote += OnMovingNote;
        GameManager.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        GameManager.OnMovingNote -= OnMovingNote;
        GameManager.OnGameOver -= OnGameOver;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CheckTime();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PoolManager.Instance.GetVFX()
            .transform.position = rectTransform.position;

        PoolManager.Instance.ReturnNote(this);
    }

    private void OnMovingNote(float speed)
    {
        rectTransform.anchoredPosition += Vector2.down * speed;


        if (rectTransform.anchoredPosition.y <= -1200f) // Game over
        {
            Debug.Log("OnGameOver called in Note");

            PoolManager.Instance.ReturnNote(this);
            GameManager.GameOver();
        }
    }

    private void CheckTime()
    {
        float currentTime = audioSource.time;
        float perfectTime = noteData.time;

        CalculatorTime.CalculateTime(currentTime, perfectTime);
    }

    private void OnGameOver()
    {
        PoolManager.Instance.ReturnNote(this);
    }
    public void SetNote(Vector2 position, NoteData noteData, AudioSource audio)
    {
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = position;
        this.noteData = noteData;
        audioSource = audio;

        Vector2 size = new Vector2(rectTransform.sizeDelta.x,
            noteData.noteType == ENoteType.Short
            ? ySize
            : ySize * 2f);

        rectTransform.sizeDelta = size;
    }
}
