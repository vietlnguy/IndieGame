using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class OutroController : MonoBehaviour
{
    private SaveManager scm;
    public Image blackScreen;
    public Image background;
    private bool active = false;
    public bool nextLine = false;
    private bool isTyping = false;
    public List<CharacterDialogue> dialogues;
    public GameObject parentGroup;
    public GameObject mainCharacter;
    public GameObject astridCharacter;
    public GameObject topBlackBar;
    public GameObject bottomBlackBar;
    public GameObject textBox;
    public GameObject textBoxNameBox;
    public Coroutine typingCoroutine;
    private string lineToBeTyped = "";
    public AudioSource typingAudio;

    void Awake()
    {
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        VictorySubscribe();
        dialogues = new List<CharacterDialogue>();
        dialogues.Add(new CharacterDialogue(mainCharacter, scm.loadedData.mainCharacterName, new string[] {"Phew... I don't know the last time I swung a sword.", "Or killed a man..", "It's done now, we're safe."}));
        dialogues.Add(new CharacterDialogue(astridCharacter, "Astrid", new string[]{"..."}));
        dialogues.Add(new CharacterDialogue(mainCharacter, scm.loadedData.mainCharacterName, new string[] {"What's wrong? Are you okay?"}));
        dialogues.Add(new CharacterDialogue(astridCharacter, "Astrid", new string[] {"What are we going to do now? We can't stay here.", "As long as I have these bracelets, we'll never be safe.","Maybe I should've just handed them over."}));
        dialogues.Add(new CharacterDialogue(mainCharacter, scm.loadedData.mainCharacterName, new string[] {"Absolutely not.", "We're going to speak with Lord Beesly, in town. I've known him to be an honorable man.", "He will speak to the royal envoy and fix this."}));
        dialogues.Add(new CharacterDialogue(astridCharacter, "Astrid", new string[] {"*sigh* And just as we were getting settled in..."}));

    }
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    typingCoroutine = null;
                    textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = lineToBeTyped;
                    typingAudio.Stop();
                    isTyping = false;
                }
                else
                {
                    nextLine = true;
                }
            }
        }
    }
    private void VictorySubscribe()
    {
        VictoryContinueButton.OnStartOutro += Outro;
    }
    private void Outro()
    {
        StartCoroutine(OutroHelper());
    }
    public IEnumerator OutroHelper()
    {   
        parentGroup.SetActive(true);
        scm.loadedData.introBattleOutro = "Outro";
        scm.OverwriteSave();

        yield return StartCoroutine(Helpers.FadeInImageAlpha(blackScreen, 1f));
      
        topBlackBar.SetActive(true);
        bottomBlackBar.SetActive(true);

        yield return StartCoroutine(Helpers.FadeInImageAlpha(background, 1f));

        StartCoroutine(Helpers.MoveRectTransform(mainCharacter, new Vector2(-890f, 0f), new Vector2(-250f, 0f), 0.25f));
        StartCoroutine(Helpers.MoveRectTransform(astridCharacter, new Vector2(1000f, -15f), new Vector2(320f, -15f), 0.35f));

        yield return new WaitForSeconds(1f);

        //Go through each dialogue
        active = true;
        for (int index = 0; index < dialogues.Count; index++)
        {
            //Update name text
            textBoxNameBox.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = dialogues[index].name;

            //Update nameBox position
            textBoxNameBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(dialogues[index].characterImage.GetComponent<RectTransform>().anchoredPosition.x, textBoxNameBox.GetComponent<RectTransform>().anchoredPosition.y);

            //Fade in text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 10f), .25f));
            StartCoroutine(Helpers.FadeInCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(.25f);
            //Type each line
            for (int index2 = 0; index2 < dialogues[index].lines.Length; index2++)
            {
                nextLine = false;
                typingCoroutine = StartCoroutine(TypeLine(dialogues[index].lines[index2], dialogues[index].name, typingAudio, textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>(), .05f));
                lineToBeTyped = dialogues[index].lines[index2];

                while (isTyping || !nextLine)
                {
                    yield return new WaitForSeconds(.25f);
                }
                textBox.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            
            }

            //Fade out text box
            StartCoroutine(Helpers.MoveRectTransform(textBox, textBox.GetComponent<RectTransform>().anchoredPosition, textBox.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, -10f), .25f));
            StartCoroutine(Helpers.FadeOutCanvasGroup(textBox.GetComponent<CanvasGroup>(), 0.25f));

            yield return new WaitForSeconds(0.25f);

        }

        yield return StartCoroutine(scm.SceneTransition(true));
        scm.loadedData.currentChapter = "Chapter2";
        scm.loadedData.introBattleOutro = "Intro";
        SceneManager.LoadScene("Overworld");
    }
    public IEnumerator TypeLine(string line, string speaker, AudioSource audioSource, TextMeshProUGUI textBox, float textSpeed) {
        if (speaker == "Astrid")
        {
            audioSource.pitch = 1.2f;
        }
        else
        {
            audioSource.pitch = 1.0f;
        }
        isTyping = true;
        audioSource.Play();
        foreach (char c in line.ToCharArray()) {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Stop();
        isTyping = false;

    }
    public struct CharacterDialogue {
        public string[] lines;
        public string name;
        public GameObject characterImage;
        public CharacterDialogue(GameObject characterImage, string name,string[] lines)
        {
            this.lines = lines;
            this.name = name;
            this.characterImage = characterImage;
        }
    }
}
    