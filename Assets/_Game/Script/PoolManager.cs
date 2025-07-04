using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [Header("Note Pool")]
    [SerializeField] private Note notePrefab;
    [SerializeField] private Transform parentTransform;
    private Queue<Note> notePool = new Queue<Note>();

    [Header("VFX Pool")]
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private Transform vfxParentTransform;
    private Queue<GameObject> vfxPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        notePool = new Queue<Note>();
        vfxPool = new Queue<GameObject>();
    }

    public Note GetNote()
    {
        if (notePool.Count > 0)
        {
            Note note = notePool.Dequeue();
            note.gameObject.SetActive(true);
            return note;
        }
        else
        {
            return Instantiate(notePrefab, parentTransform);
        }
    }

    public void ReturnNote(Note note)
    {
        note.gameObject.SetActive(false);
        notePool.Enqueue(note);
    }

    // VFX Pool
    public GameObject GetVFX()
    {
        if (vfxPool.Count > 0)
        {
            GameObject vfx = vfxPool.Dequeue();
            vfx.gameObject.SetActive(true);
            return vfx;
        }
        else
        {
            return Instantiate(vfxPrefab, vfxParentTransform);
        }
    }

    public void ReturnVFX(GameObject vfx)
    {
        vfx.gameObject.SetActive(false);
        vfxPool.Enqueue(vfx);
    }
}
