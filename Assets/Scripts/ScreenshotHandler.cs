using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScreenshotHandler
    {
        private Camera _camera;
        private Canvas[] _uiCanvases;

        private RenderTexture _scCameraRenderTexture;
        private string _dataPath = Application.dataPath;

        public ScreenshotHandler(Camera camera, Canvas[] uiCanvases)
        {
            _camera = camera;
            _uiCanvases = uiCanvases;
        }
        
        internal async void TakeScreenShot(string filename)
        {
            int width = Screen.width;
            int height = Screen.height;

            _scCameraRenderTexture = new RenderTexture (Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            _camera.targetTexture = _scCameraRenderTexture;
            _camera.Render();
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, width, height);
            screenShot.ReadPixels(rect, 0,0);
            screenShot.Apply();
            _camera.targetTexture = null;

            byte[] byteArray = screenShot.EncodeToPNG();
            Task<string> filenameResult = CheckFileName(filename);
            await filenameResult;
            
            System.IO.File.WriteAllBytes($"{_dataPath}/Output/{filenameResult.Result}.png", byteArray);
            
            for (int index = 0; index < _uiCanvases.Length; index++)
            {
                Canvas uiCanvas = _uiCanvases[index];
                uiCanvas.gameObject.SetActive(true);
            }
        }

        private async Task<string> CheckFileName(string filename)
        {
            int i = 1;
            await Task.Run(() =>
            {
                while (System.IO.File.Exists($"{_dataPath}/Output/{filename}{i}.png"))
                {
                    i++;
                }
            });
            return filename+i;
        }
    }
}