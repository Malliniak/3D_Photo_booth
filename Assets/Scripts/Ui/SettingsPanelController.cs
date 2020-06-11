using System.Collections.Generic;
using DG.Tweening;
using PhotoBooth.Core;
using TMPro;
using UnityEngine;

namespace PhotoBooth.Ui
{
    public class SettingsPanelController : MonoBehaviour
    {

        #region Private Fields

        private bool _isVisible = true;
        private RectTransform _rect;
        private PhotoBoothController _photoBoothController;
        private readonly Dictionary<ControlMode, string[]> _descriptions = new Dictionary<ControlMode, string[]>
        {
            {
                ControlMode.LIGHT, new []
                {
                    "Current mode - Light Control",
                    "1 - Model mode, 2 - light mode",
                    "WASD - Rotate Direction Light",
                    "Scroll - Change Intensity",
                    "R - reset rotation /  T - Reset Intensity",
                    "Light Color"
                }
            },
            {
                ControlMode.MODEL, new []
                {
                    "Current mode - Model Control",
                    "1 - Model mode, 2 - light mode",
                    "WASD - Move current mode object",
                    "Shift + W/A - Change axis",
                    "Arrows - Rotate current mode object",
                    "Shift + up/down - Change axis",
                    "Scroll - zoom",
                    "R - reset rotation /  T - reset position",
                    "Background Color",
                }
            }
        };
        [SerializeField] private TextMeshProUGUI[] _textMeshProUguis;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _photoBoothController = FindObjectOfType<PhotoBoothController>();
            _rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _photoBoothController.ControlModeChanged += PhotoBoothControllerOnControlModeChanged;
            UpdateTextMeshes(_descriptions[ControlMode.MODEL]);
        }
        
        private void PhotoBoothControllerOnControlModeChanged(object sender, ControlMode e)
        {
            UpdateTextMeshes(_descriptions[e]);
        }
        
        #endregion

        #region Panel Control

        private void UpdateTextMeshes(IReadOnlyList<string> descriptions)
        {
            int texts = descriptions.Count - 1;
            for (int i = _textMeshProUguis.Length - 1; i >= 0; i--)
            {
                if (texts < 0)
                {
                    _textMeshProUguis[i].text = "";
                    continue;
                }
                _textMeshProUguis[i].text = descriptions[texts];
                texts--;
            }
        }

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
            _rect.DOAnchorPosX(_rect.anchoredPosition.x - _rect.rect.width * (_isVisible ? -1 : 1), 0.5f).SetEase(Ease.InOutCubic);
        }

        #endregion
    }
}
