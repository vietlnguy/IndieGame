using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class CampController : MonoBehaviour
{
    private SaveManager scm;
    public AudioSource everdellAudio;
    public AudioSource seledoAudio;
    public AudioSource cilyAudio;
    public AudioSource brunthAudio;
    private AudioSource backgroundAudio;
    public AudioSource walkingAudio;
    public AudioSource enterScreenAudio;
    public Image blackScreen;
    public GameObject armoryObj;
    public GameObject shopObj;
    public GameObject everdellScenery;
    public GameObject seledoScenery;
    public GameObject cilyScenery;
    public GameObject brunthScenery;
    public GameObject astridSpace;
    public GameObject pauseMenu;
    public GameObject saveMenu;
    public GameObject settingsMenu;
    public GameObject exitGameMenu;
    public GameObject exitMainMenu;
    public GameObject mainCharacterObj;
    public GameObject campTutorial;
    private bool isPaused = false;
    public bool movementEnabled = true;
    void Awake()
    {
        scm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 0.5f);
        ChooseScenery();
   
    }
    void Start()
    {
        StartCoroutine(Intro());
    }
    void Update()
    {
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(false);
                saveMenu.SetActive(false);
                settingsMenu.SetActive(false);
                exitGameMenu.SetActive(false);
                exitMainMenu.SetActive(false);
                isPaused = false;
            }
    
        }
        else
        {
            //Pause the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (movementEnabled)
        {
            HandleMovement(mainCharacterObj);
        }
            
    }
    private IEnumerator Intro()
    {
        StartCoroutine(Helpers.FadeOutImageAlpha(blackScreen, 1f));
        yield return StartCoroutine(Helpers.FadeInAudio(backgroundAudio, 2f));

        enterScreenAudio.Play();
        campTutorial.SetActive(true);
        yield return null;
    }
    private void ChooseScenery()
    {
        if (scm.loadedData.currentChapter == "Chapter 2")
        {
            everdellScenery.SetActive(true);
            backgroundAudio = everdellAudio;
            astridSpace.SetActive(true);
        }

        //Anything after Chapter 2 should enable the shops
        else if (scm.loadedData.currentChapter == "Chapter 3")
        {
            EnableShop();
            EnableArmory();
        }
        else if (scm.loadedData.currentChapter == "Chapter 4")
        {
            EnableShop();
            EnableArmory();
        }
    }
    private void EnableShop()
    {
        shopObj.SetActive(true);

        //Populate shop items based on chapter
    }
    private void EnableArmory()
    {
        armoryObj.SetActive(true);  

        //Populate armory items based on chapter
    }
    private void HandleMovement(GameObject mainCharacterObj)
    {
        Rigidbody2D rb = mainCharacterObj.GetComponent<Rigidbody2D>();
        Animator animator = mainCharacterObj.GetComponent<Animator>();

        Vector2 direction = Vector2.zero;
        float moveSpeed = 5f;

        if (Input.GetKey(KeyCode.W)) direction.y += 1;
        if (Input.GetKey(KeyCode.S)) direction.y -= 1;

        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= 1;

            Vector3 localScale = mainCharacterObj.transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            mainCharacterObj.transform.localScale = localScale;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction.x += 1;

            Vector3 localScale = mainCharacterObj.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            mainCharacterObj.transform.localScale = localScale;
        }

        direction = direction.normalized;

        // Move using physics
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        // Animation
        animator.SetBool("isWalking", direction != Vector2.zero);
        if (direction != Vector2.zero)
        {
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        else
        {
            if (walkingAudio.isPlaying)
            {
                walkingAudio.Stop();
            }
        }
    }
}