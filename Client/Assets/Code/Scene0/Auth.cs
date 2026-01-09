using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
[System.Serializable]
public class LoginResponse
{
    public int userId;
    public string login;
    public float posX;
    public float posY;
    public float posZ;
}
public class AuthUI : MonoBehaviour
{
    public InputField loginInput;
    public InputField passInput;
    public Text statusText; 
    public Text infoText;   
    public Text txt_uuid; // Ссылка на ваш текстовый объект для ID

    private string apiUrl = "http://45.80.228.172/api/auth";

    public void OnRegisterClick() => StartCoroutine(SendAuth("/register"));
    public void OnLoginClick() => StartCoroutine(SendAuth("/login"));

    IEnumerator SendAuth(string endpoint)
    {
        // Создаем объект для отправки
        var authData = new { Login = loginInput.text, Password = passInput.text };
        string json = JsonUtility.ToJson(authData);
        
        var request = new UnityWebRequest(apiUrl + endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (endpoint == "/login") 
            {
                // Парсим ответ от сервера
                var data = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                
                // Сохраняем данные в сессию
                PlayerSession.UserId = data.userId;
                PlayerSession.Login = data.login;
                PlayerSession.LastPosition = new Vector3(data.posX, data.posY, data.posZ);

                // ВЫВОДИМ ID В ВАШ ТЕКСТОВЫЙ ОБЪЕКТ
                if (txt_uuid != null)
                {
                    txt_uuid.text = "ID: " + data.userId.ToString();
                }

                infoText.text = $"Logged in as: {PlayerSession.Login}";
                statusText.text = "Вход выполнен!";
            } 
            else 
            {
                statusText.text = "Регистрация успешна!";
            }
        }
        else
        {
            statusText.text = "Ошибка: " + request.downloadHandler.text;
        }
    }
}
