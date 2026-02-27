using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OutroController : MonoBehaviour
{
    private SaveManager scm;
    private Image blackScreen;

    void Awake()
    {
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
        VictorySubscribe();
    }
    private void VictorySubscribe()
    {
        VictoryContinueButton.OnStartOutro += Outro;
    }
    private void Outro()
    {
        StartCoroutine(OutroHelper());
    }
    private IEnumerator OutroHelper()
    {   
        scm.loadedData.introBattleOutro = "Outro";
        scm.OverwriteSave();

        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
        //Should there be on overworld outro scene or should I just transition to

        yield return StartCoroutine(scm.SceneTransition());
        //SceneManager.LoadScene("");
    }
}
    