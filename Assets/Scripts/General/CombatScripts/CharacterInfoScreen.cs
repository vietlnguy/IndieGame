using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterInfoScreen : MonoBehaviour
{

    public bool active = true;
    private int index = 0;
    private int upDownIndex = 0;
    private int leftRightIndex = 0;
    public GameObject selector;
    public AudioSource selectorAudio;
    public AudioSource deselectAudio;
    public GameObject blackScreen;
    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI intStat;
    public TextMeshProUGUI sklStat;
    public TextMeshProUGUI defStat;
    public TextMeshProUGUI resStat;
    public TextMeshProUGUI spdStat;


    void Awake()
    {
    }
    void LateUpdate()
    {
        if (active)
        {
            //Move the selector
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (index == 0 && upDownIndex == 1)
                {
                    if (leftRightIndex == 0)
                    {
                        topLeft();
                        leftRightIndex = 0;
                        upDownIndex = 0;
                    }
                    else if (leftRightIndex == 1)
                    {
                        topRight();
                        leftRightIndex = 1;
                        upDownIndex = 0;

                    }
                }
                else if (index > 0)
                {
                    moveSelectorUp();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                moveSelectorDown();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (leftRightIndex == 1)
                {
                    if (upDownIndex == 0)
                    {
                        topLeft();
                        leftRightIndex = 0;
                        upDownIndex = 0;
                    }
                    else if (upDownIndex == 1)
                    {
                        bottomLeft();
                        leftRightIndex = 0;
                        upDownIndex = 1;
                    }

                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (leftRightIndex == 0)
                {
                    if (upDownIndex == 0)
                    {
                        topRight();
                        leftRightIndex = 1;
                        upDownIndex = 0;
                    }
                    else if (upDownIndex == 1)
                    {
                        bottomRight();
                        leftRightIndex = 1;
                        upDownIndex = 1;
                    } 
                }
            }

        }
    }
    private void moveSelectorDown()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;

        if (leftRightIndex == 0 && upDownIndex == 0)
        {
            if (index < 5)
            {
                anchoredPos.y -= 50f;
                index++;
            }
        }
        else if (leftRightIndex == 0 && upDownIndex == 1)
        {
            index++;       
        }
        else if (leftRightIndex == 1 && upDownIndex == 0)
        {
            index++;
        }
        else if (leftRightIndex == 1 && upDownIndex == 1)
        {
            index++;
        }

        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorUp()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;

        if (leftRightIndex == 0 && upDownIndex == 0)
        {
            anchoredPos.y += 50f;
            index--;
        }
        else if (leftRightIndex == 0 && upDownIndex == 1)
        {
            index--;   
        }
        else if (leftRightIndex == 1 && upDownIndex == 0)
        {
            index--;
        }
        else if (leftRightIndex == 1 && upDownIndex == 1)
        {
            index--;
        }
        rt.anchoredPosition = anchoredPos;

    }
    private void moveSelectorLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y += 27f;
        rt.anchoredPosition = anchoredPos;
    }
    private void moveSelectorRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y -= 27f;
        rt.anchoredPosition = anchoredPos;
    }
    private void topLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 393f;
        anchoredPos.x = 62f;
        rt.anchoredPosition = anchoredPos;
    }
    private void topRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = 368f;
        anchoredPos.x = 605f;
        rt.anchoredPosition = anchoredPos;
    }
    private void bottomLeft()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = -132f;
        anchoredPos.x = 60f;
        rt.anchoredPosition = anchoredPos;
    }
    private void bottomRight()
    {
        selectorAudio.Play();
        RectTransform rt = selector.GetComponent<RectTransform>();
        Vector2 anchoredPos = rt.anchoredPosition;
        anchoredPos.y = -162f;
        anchoredPos.x = 605f;
        rt.anchoredPosition = anchoredPos;
    }

    public void populateInitialData(GameObject character)
    {
        //Populate Stats

    }
}

