using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ModelsLoader
{
    
    private Camera _mainCamera;
    
    public ModelsLoader(Camera mainCamera)
    {
        _mainCamera = Camera.main;
    }
    
    /// <summary>
    ///     Returns array of Game Objects containing MeshRenderers from given Path.
    /// </summary>
    /// <param name="path"> Path to folder inside Resources folder.</param>
    internal GameObject[] LoadModels(string path)
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
    ///    Checks if GameObjects contains any MeshRenderers on self and children
    /// </summary>
    /// <param name="model"> Game Object / prefab to check </param>
    private bool IsMeshRenderer(GameObject model)
    {
        MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
        return meshRenderers.Length > 0;
    }
}
