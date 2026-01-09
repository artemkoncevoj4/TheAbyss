using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class AuthUI : MonoBehaviour 
{
    [Header("UI Элементы (TextMeshPro)")]
    public TMP_InputField loginInput; 
    public TMP_InputField passInput;  
    public TextMeshProUGUI titleText; 
    public TextMeshProUGUI txt_uuid;  

    [Header("Кнопки режимов")]
    public GameObject localGameButton;
    public GameObject onlineGameButton;

    private string apiUrl = "http://45.80.228.172/api/auth"; 

    public void OnRegisterClick() => StartCoroutine(SendAuth("/register"));
    public void OnLoginClick() => StartCoroutine(SendAuth("/login"));

    IEnumerator SendAuth(string endpoint)
    {
        titleText.text = "Ожидание...";
        titleText.color = Color.white;
        
        // 1. Создаем объект данных строго по классу
        AuthData authData = new AuthData();
        authData.Login = loginInput.text;
        authData.Password = passInput.text;

        string json = JsonUtility.ToJson(authData);
        
        // 2. Настраиваем запрос
        UnityWebRequest request = new UnityWebRequest(apiUrl + endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // 3. Обработка результата
        if (request.result == UnityWebRequest.Result.Success)
        {
            titleText.color = Color.green;

            if (endpoint == "/login") 
            {
                // Парсим JSON ответ от сервера
                string responseText = request.downloadHandler.text;
                LoginResponse data = JsonUtility.FromJson<LoginResponse>(responseText);
                
                // Сохраняем в глобальную сессию
                PlayerSession.UserId = data.userId;
                PlayerSession.Login = data.login;

                if (txt_uuid != null) txt_uuid.text = "ID: " + data.userId.ToString();
                
                titleText.text = "УСПЕШНЫЙ ВХОД";
                ShowModeSelection();
            } 
            else 
            {
                titleText.text = "УСПЕШНАЯ РЕГИСТРАЦИЯ";
            }
        }
        else
        {
            titleText.color = Color.red;
            
            // Если сервер прислал текстовое пояснение, выводим его
            if (!string.IsNullOrEmpty(request.downloadHandler.text))
            {
                titleText.text = request.downloadHandler.text; 
                Debug.LogError("Детали ошибки от сервера: " + request.downloadHandler.text);
            }
            else
            {
                titleText.text = "Ошибка соединения: " + request.error;
                Debug.LogError("Сетевая ошибка: " + request.error);
            }
        }
    }

    private void ShowModeSelection()
    {
        loginInput.gameObject.SetActive(false);
        passInput.gameObject.SetActive(false);
        
        if (localGameButton != null) localGameButton.SetActive(true);
        if (onlineGameButton != null) onlineGameButton.SetActive(true);
    }

    // ВАЖНО: Эти классы должны быть ВНЕ методов
    [System.Serializable]
    public class AuthData { public string Login; public string Password; }

    [System.Serializable]
    public class LoginResponse 
    { 
        public int userId; // Маленькая буква как в JSON сервера
        public string login; 
    }
}