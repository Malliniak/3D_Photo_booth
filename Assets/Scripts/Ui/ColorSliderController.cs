using System;
using DG.Tweening;
using PhotoBooth.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoBooth.Ui
{
    public class ColorSliderController : MonoBehaviour
    {

        #region Private Fields

        private Slider _slider;
        private Camera _camera;
        private Light _light;
        private PhotoBoothController _photoBoothController;
        private ControlMode _currentMode;
        
        [SerializeField] private Rgb _colorToControl = Rgb.RED;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();
            _camera = Camera.main;
            _light = FindObjectOfType<Light>();
            _photoBoothController = FindObjectOfType<PhotoBoothController>();
        }

        private void Start()
        {
            _slider.DOValue(_camera.backgroundColor.GetColorByEnum(_colorToControl), 0.5f).SetEase(Ease.InOutCubic);
            _slider.onValueChanged.AddListener(UpdateColor);
            _photoBoothController.ControlModeChanged += PhotoBoothControllerOnControlModeChanged;
        }

        private void PhotoBoothControllerOnControlModeChanged(object sender, ControlMode e)
        {
            _currentMode = e;
        }

        #endregion

        #region Color Control

        private void UpdateColor(float value)
        {
            switch (_currentMode)
            {
                case ControlMode.MODEL:
                    UpdateCameraColor(value);
                    break;
                case ControlMode.LIGHT:
                    UpdateLightColor(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateLightColor(float value)
        {
            Color newColor = GetColorBasedOnControlValue(_colorToControl, _light.color, value);
            _light.DOColor(newColor, .1f).SetEase(Ease.InOutCubic);
        }

        private void UpdateCameraColor(float value)
        {
            Color newColor = GetColorBasedOnControlValue(_colorToControl, _camera.backgroundColor, value);
            _camera.DOColor(newColor, .1f).SetEase(Ease.InOutCubic);
        }

        private Color GetColorBasedOnControlValue(Rgb colorToControl, Color baseColor, float value)
        {
            Color newColor;
            switch (colorToControl)
            {
                case Rgb.RED:
                    newColor = new Color(value, baseColor.g, baseColor.b);
                    break;
                case Rgb.GREEN:
                    newColor = new Color(baseColor.r, value, baseColor.b);
                    break;
                case Rgb.BLUE:
                    newColor = new Color(baseColor.r, baseColor.g, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return newColor;
        }

        #endregion
    }
}