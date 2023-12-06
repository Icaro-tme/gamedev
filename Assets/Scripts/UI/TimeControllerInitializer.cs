using UnityEngine;

public class TimeControllerInitializer : MonoBehaviour
{
    public TimeController timeControllerPrefab;

    void Start()
    {
        if (TimeController.instance == null)
        {
            TimeController.instance = Instantiate(timeControllerPrefab);
            DontDestroyOnLoad(TimeController.instance.gameObject);
        }
    }
}
