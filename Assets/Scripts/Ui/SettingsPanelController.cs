using DG.Tweening;
using UnityEngine;

namespace PhotoBooth.Ui
{
    public class SettingsPanelController : MonoBehaviour
    {

        private bool _isVisible = true;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void ToggleVisibilty()
        {
            _isVisible = !_isVisible;
            _rect.DOAnchorPosX(_rect.anchoredPosition.x - _rect.rect.width * (_isVisible ? -1 : 1), 0.5f).SetEase(Ease.InOutCubic);
        }
    }
}
