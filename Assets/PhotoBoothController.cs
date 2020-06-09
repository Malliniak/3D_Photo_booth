using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBoothController : MonoBehaviour
{
    private const string ASSETS_FOLDER_DIRECTORY = "Input";

    private GameObject[] _models;
    private ModelsLoader _modelsLoader;
    private int activeModel;
    [SerializeField] private float _modelTranslationSpeed = 2f;
    [SerializeField] private float _modelMinScale = 0.05f;
    [SerializeField] private float _modelMaxScale = 4f;
    [SerializeField] private Vector3 _defaultScale = new Vector3(.3f, .3f, .3f);
    
    public float ModelTranslationSpeed => _modelTranslationSpeed;
    public float ModelMinScale => _modelMinScale;
    public float ModelMaxScale => _modelMaxScale;

    public event EventHandler<ControlMode> ControlModeChanged;
    private void OnControlModeChanged(ControlMode e)
    {
        ControlModeChanged?.Invoke(this, e);
    }

    private void Awake()
    {
        _modelsLoader = new ModelsLoader(Camera.main);
    }

    private void Start()
    {
        _models = _modelsLoader.LoadModels(ASSETS_FOLDER_DIRECTORY);
        InstantiateModels(_models);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnControlModeChanged(ControlMode.MODEL);
        }
    }

    /// <summary>
    ///  Instantiates models from given arrays, adds SingleModelController components to them, set first to active. 
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
            meshes[i].transform.localScale = _defaultScale;
            SingleModelController controller = meshes[i].AddComponent<SingleModelController>();
            controller.PhotoBoothController = this;
            if(i>0)
                meshes[i].SetActive(false);
        }

        activeModel = 0;
    }

    /// <summary>
    ///     Changes current model rendering.
    /// </summary>
    /// <param name="previous">if true, active model will trigger next model, if false will trigger previous one.</param>
    public void SwapModel(bool previous = false)
    {
        _models[activeModel].SetActive(false);
        activeModel += previous ? -1 : 1;
        activeModel = Mathf.Min(activeModel, _models.Length - 1);
        activeModel = Mathf.Max(activeModel, 0);
        _models[activeModel].SetActive(true);
    }

}
