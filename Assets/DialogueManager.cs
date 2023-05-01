using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;
    private int selectedChoiceIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the same scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;

        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ContinueStory();
        }

        if (currentStory.currentChoices.Count > 0)
        {
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                selectedChoiceIndex = (selectedChoiceIndex + 1) % currentStory.currentChoices.Count;
                DisplayChoices();
            }
            else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                selectedChoiceIndex = (selectedChoiceIndex - 1 + currentStory.currentChoices.Count) % currentStory.currentChoices.Count;
                DisplayChoices();
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                MakeChoice(selectedChoiceIndex);
            }
        }
    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

 
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }


    // ReSharper disable Unity.PerformanceAnalysis
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given:" + currentChoices.Count);
        }

        for (int i = 0; i < choices.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                choices[i].SetActive(true);
                choicesText[i].text = currentChoices[i].text;
                if (i == selectedChoiceIndex)
                {
                    choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
                }
                else
                {
                    choices[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
            }
            else
            {
                choices[i].SetActive(false);
            }
        }
    }



    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
