using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public float triggerRadius = 5f;
    public KeyCode interactKey = KeyCode.E;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // Salvando o horário atual antes de mudar de cena
                    SaveCurrentTime();

                    // Carregando a nova cena
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }

    private void SaveCurrentTime()
    {
        if (TimeController.instance != null)
        {
            TimeController.instance.currentTime = DateTime.Now.Date + TimeSpan.FromHours(TimeController.instance.startHour);
        }
    }
}
