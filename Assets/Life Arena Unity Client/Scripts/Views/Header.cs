using System;
using Avangardum.LifeArena.UnityClient.Data;
using Avangardum.LifeArena.UnityClient.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class Header : MonoBehaviour, IHeader
    {
        [SerializeField] private TextMeshProUGUI _generationText;
        [SerializeField] private TextMeshProUGUI _timeUntilNextGenerationText;
        [SerializeField] private Image _nextGenerationTimerClockFilling;
        [SerializeField] private TextMeshProUGUI _cellsLeftText;
        [SerializeField] private Slider _zoomSlider;

        private int _generation;
        private TimeSpan _timeUntilNextGeneration;
        
        public event EventHandler<ZoomPercentageChangedEventArgs> ZoomPercentageChanged;

        public int Generation
        {
            private get => _generation;
            set
            {
                _generation = value;
                _generationText.text = _generation.ToString();
            }
        }

        public TimeSpan NextGenerationInterval { private get; set; }

        public TimeSpan TimeUntilNextGeneration
        {
            private get => _timeUntilNextGeneration;
            set
            {
                _timeUntilNextGeneration = value;
                _timeUntilNextGenerationText.text = _timeUntilNextGeneration.TotalSeconds.ToString("F1");
                NextGenerationInterval = TimeSpan.FromSeconds(5); // TODO get from server instead
                Assert.AreNotEqual(TimeSpan.Zero, NextGenerationInterval);
                var fillAmount = Mathf.InverseLerp(0, (float)NextGenerationInterval.TotalSeconds, 
                    (float)_timeUntilNextGeneration.TotalSeconds);
                if (Generation % 2 == 1)
                {
                    fillAmount = 1 - fillAmount;
                }
                _nextGenerationTimerClockFilling.fillAmount = fillAmount;
                _nextGenerationTimerClockFilling.fillClockwise = Generation % 2 == 1;
            }
        }

        public int CellsLeft
        {
            set => _cellsLeftText.text = value.ToString();
        }

        public float ZoomPercentage
        {
            get => _zoomSlider.value;
            set => _zoomSlider.value = value;
        }

        private void Awake()
        {
            _zoomSlider.onValueChanged.AddListener(OnZoomSliderValueChanged);
        }

        private void Update()
        {
            TimeUntilNextGeneration -= TimeSpan.FromSeconds(Time.deltaTime);
            if (TimeUntilNextGeneration < TimeSpan.Zero)
            {
                TimeUntilNextGeneration = TimeSpan.Zero;
            }
        }

        private void OnZoomSliderValueChanged(float value)
        {
            ZoomPercentageChanged?.Invoke(this, new ZoomPercentageChangedEventArgs(value));
        }
    }
}