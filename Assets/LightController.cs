using System;
using DG.Tweening;
using PhotoBooth.Core;
using UnityEngine;

public class LightController : MonoBehaviour
{

    #region Private Fields

    private PhotoBoothController _photoBoothController;
    private Light _light;
    private bool _shouldListenToInput;
    private Quaternion _originalRotation;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        _light = GetComponent<Light>();
        _originalRotation = transform.rotation;
        _photoBoothController = FindObjectOfType<PhotoBoothController>();
    }

    private void Start()
    {
        _photoBoothController.ControlModeChanged += PhotoBoothControllerOnControlModeChanged;
    }

    private void PhotoBoothControllerOnControlModeChanged(object sender, ControlMode e)
    {
        _shouldListenToInput = e == ControlMode.LIGHT;
    }

    private void Update()
    {
        if (_shouldListenToInput == false)
            return;
        
        ChangeIntensity();
        
        if (!Input.anyKey) 
            return;

        UpdateRotation();

        if (Input.GetKeyDown(KeyCode.T))
            _light.intensity = 1;

        if (Input.GetKeyDown(KeyCode.R))
            transform.rotation = _originalRotation;
    }

    #endregion
    
    #region Light Control
    
    private void ChangeIntensity()
    {
        float mouseScrollAxis = Input.GetAxis("Mouse ScrollWheel");
        if (!(Math.Abs(mouseScrollAxis) > 0.05f)) 
            return;
        float newIntensity = _light.intensity + mouseScrollAxis;
        _light.DOIntensity(newIntensity, 0.4f);
    }
        
        
    private void UpdateRotation()
    {
        Vector3 newRotation = new Vector3(Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal"), 0);
        transform.Rotate(newRotation);
    }
    
    #endregion
}
