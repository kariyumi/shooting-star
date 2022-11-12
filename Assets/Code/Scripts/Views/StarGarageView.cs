using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.Views
{
    public class StarGarageView : MonoBehaviour
    {
        [SerializeField] TMP_Text _softCurrencyCount;
        [SerializeField] TMP_Text _hardCurrencyCount;
        [SerializeField] Button _buySoftCurrencyButton;
        [SerializeField] Button _buyHardCurrencyButton;
        [SerializeField] TMP_Text _shieldCounter;
        [SerializeField] TMP_Text _shieldTimer;
        [SerializeField] Button _buyShieldButton;
        [SerializeField] Button _accelerateShieldButton;
        [SerializeField] Button _returnButton;

        private bool _shouldUpdateShieldCounter = false;

        public void Initialize(Action onBuySoftCurrency,
            Action onBuyHardCurrency,
            Action onBuyShieldButton,
            Action onAccelerateShield,
            Action onReturnButton
            )
        {
            _buySoftCurrencyButton.onClick.AddListener(onBuySoftCurrency.Invoke);
            _buyHardCurrencyButton.onClick.AddListener(onBuyHardCurrency.Invoke);
            _buyShieldButton.onClick.AddListener(onBuyShieldButton.Invoke);
            _accelerateShieldButton.onClick.AddListener(onAccelerateShield.Invoke);
            _returnButton.onClick.AddListener(onReturnButton.Invoke);
        }

        public void OnActive(int softCurrency, int hardCurrency, int shieldCounter, TimeSpan timer)
        {
            UpdateSoftCurrecy(softCurrency);
            UpdateHardCurrecy(hardCurrency);
            UpdateShieldCounter(shieldCounter);
            StartCoroutine(UpdateTimer(timer));
        }

        IEnumerator UpdateTimer(TimeSpan timer)
        {
            if (timer > TimeSpan.Zero)
            {
                _shouldUpdateShieldCounter = true;
            }

            while (timer >= TimeSpan.Zero)
            {
                _shieldTimer.text = "TIME TO BE READY: " + timer.ToString();

                yield return new WaitForSecondsRealtime(1);

                timer -= new TimeSpan(0, 0, 1);
            }

            _shieldTimer.text = "TIME TO BE READY: " + TimeSpan.Zero;

            if (_shouldUpdateShieldCounter)
            {
                UpdateShieldCounter(Int32.Parse(_shieldCounter.text) + 1);
                _shouldUpdateShieldCounter = false;
            }
        }

        public void OnBuyShield(int softCurrency, TimeSpan timer)
        {
            StartCoroutine(UpdateTimer(timer));
            UpdateSoftCurrecy(softCurrency);
        }

        public void OnAccelerateShield(int hardCurrency, TimeSpan timer)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateTimer(timer));
            UpdateHardCurrecy(hardCurrency);
        }

        public void UpdateSoftCurrecy(int value)
        {
            _softCurrencyCount.text = value.ToString();
        }

        public void UpdateHardCurrecy(int value)
        {
            _hardCurrencyCount.text = value.ToString();
        }

        public void UpdateShieldCounter(int value)
        {
            _shieldCounter.text = value.ToString();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            _buySoftCurrencyButton.onClick.RemoveAllListeners();
            _buyHardCurrencyButton.onClick.RemoveAllListeners();
            _buyShieldButton.onClick.RemoveAllListeners();
            _accelerateShieldButton.onClick.RemoveAllListeners();
            _returnButton.onClick.RemoveAllListeners();
        }
    }
}