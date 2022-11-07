using UnityEngine;

public class CurrencyModel : MonoBehaviour
{
    public int SoftCurrencyCounter { get; private set; }
    public int HardCurrencyCounter { get; private set; }

    public void Initialize()
    {
        SoftCurrencyCounter = 0;
        HardCurrencyCounter = 0;
    }

    public void UpdateSoftCurrency(int value)
    {
        SoftCurrencyCounter += value;
    }

    public void UpdateHardCurrency(int value)
    {
        HardCurrencyCounter += value;
    }
}
