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
    private string[] instructions = {"1. Click on character to select them.", "2. Move with WASD.", "3. Click enemies within red circle to Attack.", "4. Click allies in green circle to assist or trade.", "5. Click selected character again to view info, use items, or end turn.", "6. Ranged units can attack from afar, but are vulnerable up close."};
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
                if (index != 2)
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
    public void DisableTutorial()
    {
        active = false;
        bookObject.SetActive(false);

    }
    private IEnumerator NextPage()
    {   
        contents.SetActive(false);
        bookObject.GetComponent<Animator>().SetTrigger("flipPage");
        flipPageAudio.Play();

        //Update contents here
        if (index == 0)
        {
            contents.transform.Find("LeftText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[0];
            contents.transform.Find("RightText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[1];
        }
        else if (index == 1)
        {
            contents.transform.Find("LeftText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[2];
            contents.transform.Find("RightText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[3];
        }
        else if (index == 2)
        {
            contents.transform.Find("LeftText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[4];
            contents.transform.Find("RightText").gameObject.GetComponent<TextMeshProUGUI>().text = instructions[5];
        }

        
        yield return new WaitForSeconds(.5f);
        contents.SetActive(true);
    }

}