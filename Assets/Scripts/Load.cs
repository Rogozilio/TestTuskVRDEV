using System;
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
                // Save the downloaded image to the Assets folder
                byte[] bytes = ((DownloadHandlerTexture)webRequest.downloadHandler).data;
                System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/" + name, bytes);
                Debug.Log("Image downloaded successfully!");
            }
        }
    }
    
    private static Sprite FromDisk(string imageName)
    {
        // Construct the path to the image
        string imagePath = Path.Combine(Application.streamingAssetsPath, imageName);

        if (File.Exists(imagePath))
        {
            // Read all bytes from the image file
            byte[] imageData = File.ReadAllBytes(imagePath);

            // Create a new Texture2D and load the image data
            Texture2D texture = new Texture2D(2, 2); // Initialize with any size, will be resized automatically
            texture.LoadImage(imageData);

            // Convert the Texture2D to a Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            // Apply the Sprite to the Image component
            return sprite;
        }

        return null;
    }
}
