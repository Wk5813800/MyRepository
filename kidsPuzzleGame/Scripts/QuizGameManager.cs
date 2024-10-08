using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizGameManager : MonoBehaviour
{
    public static QuizGameManager instance;

    [SerializeField]
    private CharsWordDataScript[] answerCharactersList;
    [SerializeField]
    private CharsWordDataScript[] optionsSelectableCharactersList;

    private List<int> pickedOptionsIndexList;
    [SerializeField]
    private QuizDataScrptableObject questionsData;
    [SerializeField] private GameObject gameOverCanvas;
    private GameState gameState = GameState.isPlaying;
    private char[] charsList = new char[18];
    private int currentSelecteAnswerindex = 0;
    private string answerCharacter;
    [SerializeField]
    private Image questionAskedImage;
    private bool isCorrect;
    public TextMeshProUGUI HintText;
    // private int hintQuestionIndex;

    // New list to store available question indexes
    private List<int> availableQuestionIndexes;
    // private AudioSource audioSource;
    // public AudioClip clickClip;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        pickedOptionsIndexList = new List<int>();

        // Initialize the list of available question indexes
        availableQuestionIndexes = Enumerable.Range(0, questionsData.questions.Count).ToList();
        // audioSource.GetComponent<AudioSource>();
    }

    private void Start()
    {
        // AdsManager.Instance.ShowRewardedInterstitialAd();
        SetQuestionOptions();
    }

    private void SetQuestionOptions()
    {
        if (availableQuestionIndexes.Count == 0)
        {
            // If no questions are left, show the game over canvas
            if (!gameOverCanvas.activeInHierarchy)
            {
                gameOverCanvas.SetActive(true);
            }
            return;
        }


        // Pick a random index from the available list
        int randomIndex = UnityEngine.Random.Range(0, availableQuestionIndexes.Count);
        int selectedQuestionIndex = availableQuestionIndexes[randomIndex];

        // Remove the picked question from the available list so it's not repeated
        availableQuestionIndexes.RemoveAt(randomIndex);

        // hintQuestionIndex = randomIndex
        // Set the question and answer based on the selected index
        currentSelecteAnswerindex = 0;
        pickedOptionsIndexList.Clear();
        questionAskedImage.sprite = questionsData.questions[selectedQuestionIndex].questionImage;
        answerCharacter = questionsData.questions[selectedQuestionIndex].answer;

        ResetQuestionCharacters();

        // Populate charsList with the answer characters and extra random characters
        for (int questionChars = 0; questionChars < answerCharacter.Length; questionChars++)
        {
            charsList[questionChars] = char.ToUpper(answerCharacter[questionChars]);
        }
        for (int extraChars = answerCharacter.Length; extraChars < optionsSelectableCharactersList.Length; extraChars++)
        {
            charsList[extraChars] = (char)UnityEngine.Random.Range(65, 91);  // Random A-Z characters
        }

        // Shuffle and display the characters
        charsList = ShuffleList.ShuffleListItems<char>(charsList.ToList()).ToArray();
        for (int charsToDisplay = 0; charsToDisplay < optionsSelectableCharactersList.Length; charsToDisplay++)
        {
            optionsSelectableCharactersList[charsToDisplay].SetAndDisplayCharacter(charsList[charsToDisplay]);
        }

        gameState = GameState.isPlaying;
    }
    public void HintBtnCallback()
    {
        HintText.text = answerCharacter;
        if (!HintText.gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitForHintDisappear());
        }
    }
    IEnumerator WaitForHintDisappear()
    {
        HintText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        HintText.gameObject.SetActive(false);
    }
    public void SelectedCharFromOptions(CharsWordDataScript selectedChar)
    {
        // audioSource.PlayOneShot(clickClip);/
        if (gameState == GameState.Next || currentSelecteAnswerindex >= answerCharacter.Length) return;

        // if(currentSelecteAnswerindex <= questionsData.questions.Count)
        // {
        pickedOptionsIndexList.Add(selectedChar.transform.GetSiblingIndex());
        answerCharactersList[currentSelecteAnswerindex].SetAndDisplayCharacter(selectedChar.charsValue);
        selectedChar.GetComponent<Button>().interactable = false;
        currentSelecteAnswerindex++;

        if (currentSelecteAnswerindex >= answerCharacter.Length)
        {
            isCorrect = true;

            for (int i = 0; i < answerCharacter.Length; i++)
            {
                if (Char.ToUpper(answerCharacter[i]) != Char.ToUpper(answerCharactersList[i].charsValue))
                {
                    answerCharactersList[i].GetComponent<Image>().color = Color.red; // Incorrect letter in red
                    isCorrect = false;
                    // break;
                }
                else
                {
                    answerCharactersList[i].GetComponent<Image>().color = Color.green; // Correct letter in green
                }
                // answerCharactersList[i].GetComponent<Image>().color = Color.red;
            }

            if (isCorrect)
            {
                Debug.Log("Hurry! Your answer is correct!");
                gameState = GameState.Next;

                // If questions remain, set up the next question
                if (availableQuestionIndexes.Count > 0)
                {
                    Invoke(nameof(SetQuestionOptions), 0.5f);
                }
                else
                {
                    AdsManager.Instance.ShowRewardedInterstitialAd();
                    if (!gameOverCanvas.activeInHierarchy)
                    {
                        gameOverCanvas.SetActive(true);
                    }
                }
            }
            else
            {
                Debug.Log("Oh no! Your answer is not correct.");
                   Invoke(nameof(ResetAnswer), 1.0f);

            }
        }
    }
 void ResetAnswer()
    {
        currentSelecteAnswerindex = 0;
        foreach (var charObj in answerCharactersList)
        {
            charObj.SetAndDisplayCharacter('_'); // Clear the displayed characters
            charObj.GetComponent<Image>().color = Color.white; // Reset color
        }

        foreach (var pickedIndex in pickedOptionsIndexList)
        {
            optionsSelectableCharactersList[pickedIndex].GetComponent<Button>().interactable = true;
        }

        pickedOptionsIndexList.Clear();
    }
    void ResetQuestionCharacters()
    {
        for (int i = 0; i < answerCharactersList.Length; i++)
        {
            answerCharactersList[i].gameObject.SetActive(true);
            answerCharactersList[i].SetAndDisplayCharacter('_');
            answerCharactersList[i].GetComponent<Image>().color = Color.white;
        }

        for (int i = answerCharacter.Length; i < answerCharactersList.Length; i++)
        {
            answerCharactersList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < optionsSelectableCharactersList.Length; i++)
        {
            optionsSelectableCharactersList[i].GetComponent<Button>().interactable = true;
        }
    }

    public void ResetLastCharacter()
    {
        if (pickedOptionsIndexList.Count > 0)
        {
            int index = pickedOptionsIndexList[pickedOptionsIndexList.Count - 1];
            optionsSelectableCharactersList[index].GetComponent<Button>().interactable = true;
            answerCharactersList[currentSelecteAnswerindex].GetComponent<Image>().color = Color.white;
            pickedOptionsIndexList.RemoveAt(pickedOptionsIndexList.Count - 1);
            currentSelecteAnswerindex--;
            answerCharactersList[currentSelecteAnswerindex].SetAndDisplayCharacter('_');
        }
    }

    public void ResetAllCharacters()
    {
        if (pickedOptionsIndexList.Count > 0)
        {
            for (int i = 0; i < answerCharactersList.Length; i++)
            {
                answerCharactersList[i].SetAndDisplayCharacter('_');
                optionsSelectableCharactersList[i].GetComponent<Button>().interactable = true;
                answerCharactersList[i].GetComponent<Image>().color = Color.white;
            }
            for (int i = pickedOptionsIndexList.Count - 1; i >= 0; i--)
            {
                int index = pickedOptionsIndexList[i];
                if (index >= 0 && index < optionsSelectableCharactersList.Length)
                {
                    optionsSelectableCharactersList[index].GetComponent<Button>().interactable = true;
                    pickedOptionsIndexList.RemoveAt(i);
                }
                optionsSelectableCharactersList[i].GetComponent<Button>().interactable = true;
            }
            // Shuffle and display the characters
            currentSelecteAnswerindex = 0;
            charsList = ShuffleList.ShuffleListItems<char>(charsList.ToList()).ToArray();
        }
    }
}
public enum GameState
{
    isPlaying, Next
}

//////////////////////////////////////////////////////////////////////
///

[System.Serializable]
public class QuestionOptionsData
{
    public Sprite questionImage;
    public string answer;
}