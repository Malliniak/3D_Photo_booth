using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
public class PhotoBoothController : MonoBehaviour
{

    #region Constant Fields

    private const string ASSETS_FOLDER_DIRECTORY = "Input";

    #endregion

    #region Private Fields

    private GameObject[] _models;
    private ModelsLoader _modelsLoader;
    private ScreenshotHandler _screenshotHandler;
    private Canvas[] _uiCanvas;
    private Camera _camera;
    private int _activeModel;
    
    [SerializeField] private float _modelTranslationSpeed = 2f;
    [SerializeField] private float _modelMinScale = 0.05f;
    [SerializeField] private float _modelMaxScale = 4f;
    [SerializeField] private float _defaultScale = .3f;

    #endregion

    #region Public Properties

    public float ModelTranslationSpeed => _modelTranslationSpeed;
    public float ModelMinScale => _modelMinScale;
    public float ModelMaxScale => _modelMaxScale;

    #endregion

    #region Public Events
    public event EventHandler<ControlMode> ControlModeChanged;
    private void OnControlModeChanged(ControlMode e)
    {
        ControlModeChanged?.Invoke(this, e);
    }
    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        // Gets all Canvases in order to turn them off during screenshot.
        _uiCanvas = FindObjectsOfType<Canvas>();
        _camera = Camera.main;
        
        _modelsLoader = new ModelsLoader(_camera);
        _screenshotHandler = new ScreenshotHandler(_camera, _uiCanvas);
    }

    private void Start()
    {
        // Load models from directory.
        _models = _modelsLoader.LoadModels(ASSETS_FOLDER_DIRECTORY);
        
        //Instantiate models game objects.
        InstantiateModels(_models);
    }

    private void Update()
    {
        // Changes translation mode.
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnControlModeChanged(ControlMode.MODEL);
        }
    }

    #endregion

    #region Models Handling

    /// <summary>
    ///  Instantiates models from given array, adds SingleModelController components to them, sets first to active. 
    /// </summary>
    /// <param name="meshes"></param>
    private void InstantiateModels(IList<GameObject> meshes)
    {
        GameObject modelsParent = new GameObject("Models");
        modelsParent.transform.parent = transform;
        
        // In this scenario, where we are generating everything at once and in order to save time, I am not going to write Object Pool system.
        for (int i = 0; i < meshes.Count; i++)
        {
            meshes[i] = Instantiate(meshes[i], Vector3.zero, Quaternion.identity, modelsParent.transform);
            meshes[i].transform.localScale = new Vector3(_defaultScale, _defaultScale, _defaultScale);
            SingleModelController controller = meshes[i].AddComponent<SingleModelController>();
            controller.PhotoBoothController = this;
            if(i>0)
                meshes[i].SetActive(false);
        }

        _activeModel = 0;
    }

    /// <summary>
    ///     Changes current model rendering.
    /// </summary>
    /// <param name="previous">if true, active model will trigger next model, if false will trigger previous one.</param>
    public void SwapModel(bool previous = false)
    {
        _models[_activeModel].SetActive(false);
        _activeModel += previous ? -1 : 1;
        _activeModel = Mathf.Min(_activeModel, _models.Length - 1);
        _activeModel = Mathf.Max(_activeModel, 0);
        _models[_activeModel].SetActive(true);
    }

    #endregion

    #region ScreenShots Handling

    /// <summary>
    ///     Waits for end of the frame rendering, and saves snapshot from camera.
    /// </summary>
    /// <param name="withUi"> Decide if UI should be rendered on screenshot</param>
    public void TakeSnapshot(bool withUi = false)
    {
        StartCoroutine(TakeScreenshot(withUi));
    }

    private IEnumerator TakeScreenshot(bool withUi)
    {

        if (withUi == false)
        {
            for (int index = 0; index < _uiCanvas.Length; index++)
            {
                Canvas uiCanvas = _uiCanvas[index];
                uiCanvas.gameObject.SetActive(false);
            }
        }
        
        yield return new WaitForEndOfFrame();
        _screenshotHandler.TakeScreenShot(_models[_activeModel].name);
    }

    #endregion
}
