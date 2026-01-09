using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Обязательно для TMP_InputField и TextMeshProUGUI

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
        if (titleText != null)
        {
            titleText.text = "Ожидание...";
            titleText.color = Color.white;
        }
        
        // Создаем данные для отправки
        var authData = new { Login = loginInput.text, Password = passInput.text };
        string json = JsonUtility.ToJson(authData);
        
        UnityWebRequest request = new UnityWebRequest(apiUrl + endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (titleText != null) titleText.color = Color.green;

            if (endpoint == "/login") 
            {
                // Принимаем данные от сервера
                LoginResponse data = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                
                // Сохраняем ID
                PlayerSession.UserId = data.userId;
                PlayerSession.Login = data.login;

                if (txt_uuid != null) txt_uuid.text = "ID: " + data.userId.ToString();
                if (titleText != null) titleText.text = "УСПЕШНЫЙ ВХОД";
                
                ShowModeSelection();
            } 
            else 
            {
                if (titleText != null) titleText.text = "УСПЕШНАЯ РЕГИСТРАЦИЯ";
            }
        }
        else
        {
            if (titleText != null)
            {
                titleText.color = Color.red;
                titleText.text = request.downloadHandler.text; 
            }
        }
    }

    private void ShowModeSelection()
    {
        if (loginInput != null) loginInput.gameObject.SetActive(false);
        if (passInput != null) passInput.gameObject.SetActive(false);
        
        if (localGameButton != null) localGameButton.SetActive(true);
        if (onlineGameButton != null) onlineGameButton.SetActive(true);
    }

    [System.Serializable]
    public class LoginResponse { public int userId; public string login; }
}