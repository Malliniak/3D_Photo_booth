using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ModelsLoader : MonoBehaviour
{
    private const string ASSETS_FOLDER_DIRECTORY = "Input";
    
    private GameObject[] _meshes;
    private Camera _mainCamera;

    [SerializeField] private Vector3 _defaultScale = new Vector3(.3f,.3f,.3f);

    private void Awake()
    {
        // Objects initialization
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _meshes = LoadModels(ASSETS_FOLDER_DIRECTORY);
        InstantiateModels(_meshes);
    }

    private void InstantiateModels(GameObject[] meshes)
    {
        
        // In this scenario, where we are generating everything at once and in order to save time, I am not going to write Object Pool system.
        for (int i = 0; i < meshes.Length; i++)
        {
            _meshes[i] = Instantiate(_meshes[i], Vector3.zero, Quaternion.identity, transform);
            _meshes[i].transform.localScale = _defaultScale;
            _meshes[i].AddComponent<SingleModelController>();
            if(i>0)
                _meshes[i].SetActive(false);
        }
    }

    /// <summary>
    /// Returns array of Game Objects containing MeshRenderers from given Path.
    /// </summary>
    /// <param name="path"> Path to folder inside Resources folder.</param>
    private GameObject[] LoadModels(string path)
    {
        // Models Loading. I use Resources because according to Unity Docs in this particular scenario with working inside editor
        // this approach is good enough.
        // In normal projects I would use Streaming assets from pre-built bundles.

        // Check if input folder contains any GameObjects (Unity transforms fbx to game objects internally)
        GameObject[] objects = Resources.LoadAll<GameObject>(path);
        if (objects.Length == 0)
        {
            Debug.Log("<color=red> Input Folder empty </color>");
            return null;
        }
        
        List<GameObject> gameObjectsList = new List<GameObject>();
        gameObjectsList.AddMembersFromArray(objects);

        for (int i = gameObjectsList.Count-1; i >= 0; i--)
        {
            if (IsMeshRenderer(gameObjectsList[i]) == false)
                gameObjectsList.RemoveAt(i);
        }

        return gameObjectsList.ToArray();
    }

    /// <summary>
    ///     Checks if GameObjects contains any MeshRenderers on self and children
    /// </summary>
    /// <param name="model"> Game Object / prefab to check </param>
    private bool IsMeshRenderer(GameObject model)
    {
        MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
        return meshRenderers.Length > 0;
    }
}
