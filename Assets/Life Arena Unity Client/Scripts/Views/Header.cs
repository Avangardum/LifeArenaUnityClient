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
        
        public event EventHandler<ZoomChangedEventArgs> ZoomPercentageChanged;

        public int Generation
        {
            set
            {
                _generation = value;
                _generationText.text = _generation.ToString();
            }
        }

        public TimeSpan NextGenerationInterval { private get; set; }

        public TimeSpan TimeUntilNextGeneration
        {
            set
            {
                _timeUntilNextGeneration = value;
                _timeUntilNextGenerationText.text = _timeUntilNextGeneration.TotalSeconds.ToString("F1");
                NextGenerationInterval = TimeSpan.FromSeconds(5); // TODO get from server instead
                Assert.AreNotEqual(TimeSpan.Zero, NextGenerationInterval);
                var fillAmount = Mathf.InverseLerp(0, (float)NextGenerationInterval.TotalSeconds, 
                    (float)_timeUntilNextGeneration.TotalSeconds);
                if (_generation % 2 == 1)
                {
                    fillAmount = 1 - fillAmount;
                }
                _nextGenerationTimerClockFilling.fillAmount = fillAmount;
                _nextGenerationTimerClockFilling.fillClockwise = _generation % 2 == 1;
            }
        }

        public int CellsLeft
        {
            set => _cellsLeftText.text = value.ToString();
        }

        public float ZoomPercentage
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}