using System;
using DG.Tweening;
using PhotoBooth.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoBooth.Ui
{
    public class ColorSliderController : MonoBehaviour
    {
    
        private Slider _slider;
        private Camera _camera;
        [SerializeField] private Rgb _colorToControl = Rgb.RED;
    
        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _camera = Camera.main;
        }

        private void Start()
        {
            _slider.DOValue(_camera.backgroundColor.GetColorByEnum(_colorToControl), 0.5f).SetEase(Ease.InOutCubic);
            _slider.onValueChanged.AddListener(UpdateColor);
        }

        public void UpdateColor(float value)
        {
            Color newColor;
            Color backgroundColor = _camera.backgroundColor;
            switch (_colorToControl)
            {
                case Rgb.RED:
                    newColor = new Color(value, backgroundColor.g, backgroundColor.b);
                    break;
                case Rgb.GREEN:
                    newColor = new Color(backgroundColor.r, value, backgroundColor.b);
                    break;
                case Rgb.BLUE:
                    newColor = new Color(backgroundColor.r, backgroundColor.g, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _camera.DOColor(newColor, .1f).SetEase(Ease.InOutCubic);
        }
    }
}