using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitWagon : MonoBehaviour
{
    SpriteRenderer sr;
    CampMoveCircle cmc;
    public GameObject window;
    private SaveManager scm;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cmc = FindFirstObjectByType<CampMoveCircle>();
        scm = FindFirstObjectByType<SaveManager>();
    }
    private void OnMouseDown()
    {
        if (cmc.alliesInRange.Contains(gameObject))
        {
            enableNextSceneWindow();
        }
    }
    public void highlight()
    {
        sr.color = new Color(0.7f, 0.7f, 1f, 1f);
    }
    public void unhighlight()
    {
        sr.color = Color.white;
    }
    private void enableNextSceneWindow()
    {
        window.SetActive(true);
    }
    public IEnumerator nextScene()
    {
       yield return StartCoroutine(scm.SceneTransition(true));
       scm.loadedData.introBattleOutro = "Intro";
       scm.OverwriteSave();
       SceneManager.LoadScene(scm.loadedData.currentChapter);
    }

}