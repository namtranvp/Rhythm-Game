using System.Collections;
using UnityEngine;

public class Vfx : MonoBehaviour
{
    WaitForSeconds waitFor;
    private void Awake()
    {
        float duration = GetComponentInChildren<ParticleSystem>().main.duration;
        waitFor = new WaitForSeconds(duration);
    }
    private void OnEnable()
    {
        StartCoroutine(ReturnToPoolAfterDelay());
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return waitFor;
        PoolManager.Instance.ReturnVFX(gameObject);
    }
}
