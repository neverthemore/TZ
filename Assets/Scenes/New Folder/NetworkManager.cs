// NetworkManager.cs
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string TOKEN = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    private void Awake() => Instance = this;

    public void SendItemUpdate(int itemID, string action)
    {
        StartCoroutine(PostRequest(itemID, action));
    }

    private IEnumerator PostRequest(int itemID, string action)
    {
        var data = new { item_id = itemID, event_type = action };
        string json = JsonUtility.ToJson(data);

        using UnityWebRequest request = new(URL, "POST");
        request.SetRequestHeader("Authorization", $"Bearer {TOKEN}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError($"Error: {request.error}");
    }
}