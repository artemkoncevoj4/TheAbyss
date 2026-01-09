using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerLoader : MonoBehaviour
{
    void Start()
    {
        // Находим объект игрока и перемещаем его
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = PlayerSession.LastPosition;
        }
    }
}