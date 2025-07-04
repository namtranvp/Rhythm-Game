using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NoteData
{
    public float time;
    public int lane;
    public ENoteType noteType;
    public NoteData(float t, int l, ENoteType type)
    {
        time = t; lane = l; noteType = type;
    }
}

[System.Serializable]
public class NoteDataList
{
    public List<NoteData> notes = new List<NoteData>();
}
public class NoteRecorder : MonoBehaviour
{
    public AudioSource audioSource;
    public KeyCode[] recordKeys = { KeyCode.A, KeyCode.S, KeyCode.K, KeyCode.L };
    public List<NoteData> recordedNotes = new List<NoteData>();
    private Dictionary<int, float> keyDownTimes = new Dictionary<int, float>();
    private float longNoteThreshold = 0.2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Playing audio");
            audioSource.Play();
        }

        for (int i = 0; i < recordKeys.Length; i++)
        {
            if (Input.GetKeyDown(recordKeys[i]))
            {
                keyDownTimes[i] = audioSource.time;
            }

            if (Input.GetKeyUp(recordKeys[i]) && keyDownTimes.ContainsKey(i))
            {
                float startTime = keyDownTimes[i];
                float duration = audioSource.time - startTime;
                ENoteType type = duration < longNoteThreshold ? ENoteType.Short : ENoteType.Long;
                recordedNotes.Add(new NoteData(startTime, i, type));
                Debug.Log($"Note at: {startTime}, lane: {i}, type: {type}");
                keyDownTimes.Remove(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SaveToJson();
        }
    }

    void SaveToJson()
    {
        NoteDataList dataList = new NoteDataList();
        dataList.notes = recordedNotes;
        string json = JsonUtility.ToJson(dataList, true);
        string path = Path.Combine(Application.dataPath, $"Resources/{audioSource.clip.name}.json");
        File.WriteAllText(path, json);
        Debug.Log("Saved to: " + path);
    }
}