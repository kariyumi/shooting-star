using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.PlayFab;
using UnityEngine;

public class ShieldModel : MonoBehaviour
{
    public int ShieldCounter;
    public DateTime ReadyTime;
    public bool ShieldRetrieved;
    public TimeSpan Timer { get => GetTimer(); }
    public int ShieldPrice { get => 5; }
    public int AccelerationPrice { get => 5; }

    public void OnBuyShield()
    {
        ReadyTime = DateTime.UtcNow + new TimeSpan(0,1,0);
        ShieldRetrieved = false;

        Dictionary<string, string> userData = new Dictionary<string, string>();
        userData.Add(PlayFabPlayerData.LAST_SHIELD_BUY_KEY, Convert.ToString(ReadyTime));
        userData.Add(PlayFabPlayerData.LAST_SHIELD_RETRIEVED_KEY, ShieldRetrieved.ToString());
        PlayFabPlayerData.UpdateUserData(userData);

        StartCoroutine(UpdateShield(Timer));
    }

    IEnumerator UpdateShield(TimeSpan timer)
    {
        if (!ShieldRetrieved)
        {
            yield return new WaitForSecondsRealtime((int)timer.TotalSeconds);

            ShieldCounter += 1;
            PlayFabPlayerData.PurchaseItem(PlayFabPlayerData.SHIELD_ID, PlayFabPlayerData.SOFT_CURRENCY_KEY, 0);

            ShieldRetrieved = true;

            Dictionary<string, string> userData = new Dictionary<string, string>();
            userData.Add(PlayFabPlayerData.LAST_SHIELD_RETRIEVED_KEY, ShieldRetrieved.ToString());
            PlayFabPlayerData.UpdateUserData(userData);
        }
    }

    public void OnShieldUsed()
    {
        ShieldCounter -= 1;
        PlayFabPlayerData.ConsumeItem(PlayFabPlayerData.SHIELD_INSTANCE_ID);
    }

    public void OnBuyAcceleration()
    {
        ReadyTime = DateTime.UtcNow;

        Dictionary<string, string> userData = new Dictionary<string, string>();
        userData.Add(PlayFabPlayerData.LAST_SHIELD_BUY_KEY, Convert.ToString(ReadyTime));
        PlayFabPlayerData.UpdateUserData(userData);

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
