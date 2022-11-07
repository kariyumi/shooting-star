using System;
using System.Collections;
using UnityEngine;

public class ShieldModel : MonoBehaviour
{
    public int ShieldCounter { get; private set; }
    public TimeSpan Timer { get => GetTimer(); }
    public DateTime ReadyTime { get; private set; }
    public int ShieldPrice { get => 5; }
    public int AccelerationPrice { get => 5; }

    public void Initialize()
    {
        ShieldCounter = 0;
        ReadyTime = DateTime.UtcNow;
    }

    public void OnBuyShield()
    {
        ReadyTime = DateTime.UtcNow + new TimeSpan(0,1,0);
        StartCoroutine(UpdateShield(Timer));
    }

    IEnumerator UpdateShield(TimeSpan timer)
    {
        yield return new WaitForSecondsRealtime((int) timer.TotalSeconds);

        ShieldCounter += 1;
        Debug.Log("oi");
    }

    public void OnBuyAcceleration()
    {
        ReadyTime = DateTime.UtcNow;
        StopAllCoroutines();
        StartCoroutine(UpdateShield(Timer));
    }

    private TimeSpan GetTimer()
    {
        if (ReadyTime - DateTime.UtcNow < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        return ReadyTime - DateTime.UtcNow;
    }

    public void Terminate()
    {
        StopAllCoroutines();
    }
}
