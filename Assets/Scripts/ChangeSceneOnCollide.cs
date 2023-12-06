using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerChangeScene(Collider other)
    {
        if (other.tag == "Player")
        {
            // SceneManager.LoadScene();    PARÂMETRO: NÚMERO DA CENA DESEJADA NO BUILDER
            print("Troca cena está funcionando");
        }
    }
}
