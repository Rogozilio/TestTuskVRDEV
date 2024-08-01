using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Load : MonoBehaviour
{
    public static async UniTask<Sprite> FromWebOrDisk(string url)
    {
        var name = Path.GetFileName(url);
        var image = FromDisk(name);

        if (!image)
        {
            await FromWeb(url);
            image = FromDisk(name);
        }

        return image;
    }

    private static async UniTask FromWeb(string url)
    {
        var name = Path.GetFileName(url);
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            await webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                byte[] bytes = ((DownloadHandlerTexture)webRequest.downloadHandler).data;
                File.WriteAllBytes(Application.streamingAssetsPath + "/" + name, bytes);
                Debug.Log("Image downloaded successfully!");
            }
        }
    }
    
    private static Sprite FromDisk(string imageName)
    {
        string imagePath = Path.Combine(Application.streamingAssetsPath, imageName);

        if (File.Exists(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            return sprite;
        }

        return null;
    }
}
