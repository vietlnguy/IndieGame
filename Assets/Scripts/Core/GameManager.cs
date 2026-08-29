using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    public bool DebugMode;

    protected override void Awake()
    {
        base.Awake();
    }

    private void DebugModeSetupOnAwake()
    {
        if (!DebugMode) return;



    }


}
