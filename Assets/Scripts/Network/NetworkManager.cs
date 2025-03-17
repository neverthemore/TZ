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
    /// Метод для отправки события инвентаря на сервер.
    /// </summary>
    /// <param name="itemID">Идентификатор предмета.</param>
    /// <param name="eventType">Тип события (e.g., "stored" или "removed").</param>
    public void SendInventoryEvent(string itemID, string eventType)
    {
        StartCoroutine(PostRequest(itemID, eventType));
    }

    /// <summary>
    /// Метод для выполнения POST-запроса на сервер.
    /// </summary>
    /// <param name="itemID">Идентификатор предмета.</param>
    /// <param name="eventType">Тип события.</param>
    /// <returns>Корутин для выполнения запросов асинхронно.</returns>
    private IEnumerator PostRequest(string itemID, string eventType)
    {
        PostData postData = new PostData { item_id = itemID, event_type = eventType };
        string json = JsonUtility.ToJson(postData);

        Debug.Log($"Отправка JSON данных: {json}"); // Логируем данные, которые отправляются

        byte[] rawData = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(rawData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {AUTH_TOKEN}");

            // Логируем отправку запроса
            Debug.Log($"Отправка запроса на сервер: {API_URL}");

            yield return request.SendWebRequest();

            // Обработка ответа сервера
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Успех: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"Ошибка: {request.error} - Код состояния: {request.responseCode}");
            }
        }
    }

    [System.Serializable]
    private class PostData
    {
        public string item_id; // Идентификатор предмета
        public string event_type; // Тип события
    }
}