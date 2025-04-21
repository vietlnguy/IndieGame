using UnityEngine;

public class Controller: MonoBehaviour
{
    public ArrowToggle blinker;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            blinker.StartBlinking(); // triggers the flashing
        }
    }
}
