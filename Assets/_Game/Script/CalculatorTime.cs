using UnityEngine;
using UnityEngine.Events;

public enum EHitType
{
    Perfect,
    Good,
    Missed
}
public static class CalculatorTime
{
    public static event UnityAction<EHitType> OnHitResult;
    static float hitPerfect = 0.1f;
    static float hitGood = 0.2f;
    public static float timeMoveNote = 1f;

    static EHitType hitType;
    public static void CalculateTime(float currentTime, float perfectTime)
    {
        float checkHit = Mathf.Abs(currentTime - perfectTime);
        //Debug.Log("CheckTime!" + checkHit + " PerfectTime: " + perfectTime + " CurrentTime: " + currentTime);
        if (checkHit <= hitPerfect)
        {
            hitType = EHitType.Perfect;
        }
        else if (checkHit <= hitGood)
        {
            hitType = EHitType.Good;
        }
        else
        {
            hitType = EHitType.Missed;
        }
        OnHitResult?.Invoke(hitType);
    }
}
