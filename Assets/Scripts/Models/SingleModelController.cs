using System;
using DG.Tweening;
using PhotoBooth.Core;
using UnityEngine;

namespace PhotoBooth.Models
{
    public class SingleModelController : MonoBehaviour
    {

        #region Private Fields

        private PhotoBoothController _photoBoothController;
        private bool _shouldListenToInput;
        private float _translationSpeed = 1;
        private float _modelMaxSize = 1;
        private float _modelMinSize = 0;

        #endregion

        #region Public Properties

        public PhotoBoothController PhotoBoothController
        {
            set
            {
                if(_photoBoothController != null)
                    _photoBoothController.ControlModeChanged -= PhotoBoothControllerOnControlModeChanged;
            
                _photoBoothController = value;
            
                _translationSpeed = _photoBoothController.ModelTranslationSpeed;
                _modelMaxSize = _photoBoothController.ModelMaxScale;
                _modelMinSize = _photoBoothController.ModelMinScale;
            
                _photoBoothController.ControlModeChanged += PhotoBoothControllerOnControlModeChanged;
            }
        }

        private void PhotoBoothControllerOnControlModeChanged(object sender, ControlMode controlMode)
        {
            _shouldListenToInput = controlMode == ControlMode.MODEL;
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (_shouldListenToInput == false || gameObject.activeSelf == false)
                return;
        
            ZoomModel();

            if (!Input.anyKey) 
                return;
        
            Vector3 newTransform = GetAxisBaseOnKeyPressed(KeyCode.LeftShift);
            transform.position += newTransform * (Time.deltaTime * _translationSpeed);

            Vector3 newRotation = GetAxisBaseOnKeyPressed(KeyCode.LeftShift, true);
            transform.Rotate(newRotation);
        
            if(Input.GetKeyDown(KeyCode.T))
                transform.position = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.R))
                transform.rotation = Quaternion.identity;
        }

        #endregion

        #region Movement Handlers

        /// <summary>
        ///     Based on modifier, changes Vector3 axis translation.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        private Vector3 GetAxisBaseOnKeyPressed(KeyCode keyCode, bool shouldUseArrows = false)
        {
            Vector3 newTransform;

            if (shouldUseArrows)
            {
                if (Input.GetKey(keyCode))
                    newTransform = new Vector3(0, -Input.GetAxisRaw("HorizontalArrow"), Input.GetAxisRaw("VerticalArrow"));
                else
                    newTransform = new Vector3(Input.GetAxisRaw("VerticalArrow"), -Input.GetAxisRaw("HorizontalArrow"), 0);
                return newTransform;
            }
            
            if (Input.GetKey(keyCode))
                newTransform = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            else
                newTransform = new Vector3(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        
            return newTransform;
        }

        /// <summary>
        ///     Zooms model based on Mouse ScrollWheel
        /// </summary>
        private void ZoomModel()
        {
            float mouseScrollAxis = Input.GetAxis("Mouse ScrollWheel");
            if (!(Math.Abs(mouseScrollAxis) > 0.05f)) 
                return;
            float endScale = transform.localScale.x + mouseScrollAxis;
            endScale = Mathf.Min(_modelMaxSize, endScale);
            endScale = Mathf.Max(_modelMinSize, endScale);
            transform.DOScale(endScale, 0.4f);
        }

        #endregion
    }
}
