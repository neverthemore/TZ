// Networking/NetworkManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private const string API_URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AUTH_TOKEN = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendInventoryEvent(string itemID, string eventType)
    {
        StartCoroutine(PostRequest(itemID, eventType));
    }

    private IEnumerator PostRequest(string itemID, string eventType)
    {
        var postData = new PostData { item_id = itemID, event_type = eventType };
        string json = JsonUtility.ToJson(postData);
        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(json);

        using UnityWebRequest request = new UnityWebRequest(API_URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(rawData),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {AUTH_TOKEN}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success: {request.downloadHandler.text}");
         
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }


    [System.Serializable]
    private class PostData
    {
        public string item_id;
        public string event_type;
    }
}