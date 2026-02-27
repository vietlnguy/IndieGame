using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public GameObject bookObject;
    public GameObject contents;
    public bool active = false;
    private int index = 0;
    private AudioSource flipPageAudio;
    void Awake()
    {
        flipPageAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (index != 0)
                {
                    index--;
                    StartCoroutine(NextPage());
                } 
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (index != 3)
                {
                    index++;
                    StartCoroutine(NextPage());
                }
            }
        }
    }

    public void EnableTutorial()
    {
        bookObject.SetActive(true);
        active = true;
        flipPageAudio.Play();

    }
    private IEnumerator NextPage()
    {   
        contents.SetActive(false);
        bookObject.GetComponent<Animator>().SetTrigger("flipPage");
        flipPageAudio.Play();

        //Update contents here
        
        yield return new WaitForSeconds(.5f);
        contents.SetActive(true);
    }

}