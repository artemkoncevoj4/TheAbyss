using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Scene1 {

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
}