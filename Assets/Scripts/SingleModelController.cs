using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
public class SingleModelController : MonoBehaviour
{
    private PhotoBoothController _photoBoothController;

    private bool _shouldListenToInput;
    private float _translationSpeed = 1;
    private float _modelMaxSize = 1;
    private float _modelMinSize = 0;

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

    private void Update()
    {
        if (_shouldListenToInput == false || gameObject.activeSelf == false)
            return;
        
        ZoomModel();

        if (!Input.anyKey) 
            return;
        
        Vector3 newTransform = GetTransformAxisBaseOnKeyPressed(KeyCode.LeftShift);
        transform.position += newTransform * (Time.deltaTime * _translationSpeed);

        Vector3 newRotation = GetRotationAxisBasedOnKeyPressed(KeyCode.LeftShift);
        transform.Rotate(newRotation);
        
        if(Input.GetKeyDown(KeyCode.T))
            transform.position = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.R))
            transform.rotation = Quaternion.identity;
    }

    private Vector3 GetRotationAxisBasedOnKeyPressed(KeyCode keyCode)
    {
        Vector3 newTransform;

        if (Input.GetKey(keyCode))
            newTransform = new Vector3(0, -Input.GetAxisRaw("HorizontalArrow"), Input.GetAxisRaw("VerticalArrow"));
        else
            newTransform = new Vector3(Input.GetAxisRaw("VerticalArrow"), -Input.GetAxisRaw("HorizontalArrow"), 0);
        
        return newTransform;
    }

    private Vector3 GetTransformAxisBaseOnKeyPressed(KeyCode keyCode)
    {
        Vector3 newTransform;

        if (Input.GetKey(keyCode))
            newTransform = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        else
            newTransform = new Vector3(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        
        return newTransform;
    }

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
}
