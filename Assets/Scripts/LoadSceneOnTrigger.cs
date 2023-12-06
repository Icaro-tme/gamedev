using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public float triggerRadius = 1f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, triggerRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // Carrega a nova cena
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}
