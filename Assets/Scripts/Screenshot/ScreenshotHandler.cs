using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace PhotoBooth.Screenshot
{
    public class ScreenshotHandler
    {

        #region Private Fields

        private RenderTexture _scCameraRenderTexture;
        private readonly Camera _camera;
        private readonly Canvas[] _uiCanvases;

        private readonly string _dataPath = $"{Application.dataPath}/Output";

        #endregion


        public ScreenshotHandler(Camera camera, Canvas[] uiCanvases)
        {
            _camera = camera;
            _uiCanvases = uiCanvases;
        }
        
        #region Screenshot Handling

        /// <summary>
        ///     Async
        ///     Renders a texture based on camera render and saves it as PNG file with given file name.
        ///     If the file with this name exists, it will add an int number to the filename.
        /// </summary>
        /// <param name="filename"></param>
        internal async void TakeScreenShot(string filename)
        {
            // The script sorts photos based on model viewed. It will create folders to group them.
            if (Directory.Exists($"{_dataPath}/{filename}") == false)
                Directory.CreateDirectory($"{_dataPath}/{filename}");
            
            _scCameraRenderTexture = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            _camera.targetTexture = _scCameraRenderTexture;
            _camera.Render();
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            screenShot.ReadPixels(rect, 0,0);
            screenShot.Apply();
            _camera.targetTexture = null;

            byte[] byteArray = screenShot.EncodeToPNG();
            Task<string> filenameResult = CheckFileName(filename);
            TurnOnUi();

            await filenameResult;
            
            if (filenameResult.Result == null)
                return;
            
            try
            {
                File.WriteAllBytes($"{_dataPath}/{filename}/{filenameResult.Result}.png", byteArray);
            }
            catch (Exception e)
            {
                Debug.Log($"<color=red> File save error: </color> {e}");
                return;
            }
            Debug.Log($"<color=green><b>Screenshot successfully saved in directory: </b></color> {_dataPath}/{filename}/{filenameResult.Result}.png");
        }

        private void TurnOnUi()
        {
            for (int index = 0; index < _uiCanvases.Length; index++)
            {
                Canvas uiCanvas = _uiCanvases[index];
                uiCanvas.gameObject.SetActive(true);
            }
        }

        /// <summary>
        ///     Checks files in directory for the given name. If file exists, add number to name.
        /// </summary>
        /// <param name="filename"></param>
        private async Task<string> CheckFileName(string filename)
        {
            int i = 1;
            await Task.Run(() =>
            {
                while (File.Exists($"{_dataPath}/{filename}/{filename}{i}.png"))
                {
                    i++;
                }
            });
            return filename+i;
        }

        #endregion
    }
}