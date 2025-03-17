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

    /// <summary>
    /// ����� ��� �������� ������� ��������� �� ������.
    /// </summary>
    /// <param name="itemID">������������� ��������.</param>
    /// <param name="eventType">��� ������� (e.g., "stored" ��� "removed").</param>
    public void SendInventoryEvent(string itemID, string eventType)
    {
        StartCoroutine(PostRequest(itemID, eventType));
    }

    /// <summary>
    /// ����� ��� ���������� POST-������� �� ������.
    /// </summary>
    /// <param name="itemID">������������� ��������.</param>
    /// <param name="eventType">��� �������.</param>
    /// <returns>������� ��� ���������� �������� ����������.</returns>
    private IEnumerator PostRequest(string itemID, string eventType)
    {
        PostData postData = new PostData { item_id = itemID, event_type = eventType };
        string json = JsonUtility.ToJson(postData);

        Debug.Log($"�������� JSON ������: {json}"); // �������� ������, ������� ������������

        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(rawData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {AUTH_TOKEN}");

            // �������� �������� �������
            Debug.Log($"�������� ������� �� ������: {API_URL}");

            yield return request.SendWebRequest();

            // ��������� ������ �������
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"�����: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"������: {request.error} - ��� ���������: {request.responseCode}");
            }
        }
    }

    [System.Serializable]
    private class PostData
    {
        public string item_id; // ������������� ��������
        public string event_type; // ��� �������
    }
}