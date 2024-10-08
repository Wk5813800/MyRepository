// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class testQuizGameManager : MonoBehaviour
// {
//     public static testQuizGameManager instance;

//     [SerializeField]
//     private CharsWordDataScript[] answerCharactersList;
//     [SerializeField]
//     private CharsWordDataScript[] optionsSelectableCharactersList;

//     private List<int> pickedOptionsIndexList;
//     [SerializeField]
//     private QuizDataScrptableObject questionsData;
//     [SerializeField] private GameObject gameOverCanvas;
//     private GameState gameState = GameState.isPlaying;
//     private char[] charsList = new char[18];
//     private int currentSelectedAnswerIndex = 0;
//     private string answerCharacter;
//     [SerializeField]
//     private Image questionAskedImage;
//     private bool isCorrect;
//     public TextMeshProUGUI HintText;

//     private List<int> availableQuestionIndexes;

//     // private void Awake()
//     // {
//     //     if (instance == null) instance = this;
//     //     else Destroy(this.gameObject);

//     //     pickedOptionsIndexList = new List<int>();
//     //     availableQuestionIndexes = Enumerable.Range(0, questionsData.questions.Count).ToList();
//     // }

//     // private void Start()
//     // {
//     //     SetQuestionOptions();
//     // }

//     private void testSetQuestionOptions()
//     {
//         if (availableQuestionIndexes.Count == 0)
//         {
//             if (!gameOverCanvas.activeInHierarchy)
//             {
//                 gameOverCanvas.SetActive(true);
//             }
//             return;
//         }

//         int randomIndex = UnityEngine.Random.Range(0, availableQuestionIndexes.Count);
//         int selectedQuestionIndex = availableQuestionIndexes[randomIndex];
//         availableQuestionIndexes.RemoveAt(randomIndex);

//         currentSelectedAnswerIndex = 0;
//         pickedOptionsIndexList.Clear();
//         questionAskedImage.sprite = questionsData.questions[selectedQuestionIndex].questionImage;
//         answerCharacter = questionsData.questions[selectedQuestionIndex].answer;

//         ResetQuestionCharacters();

//         for (int questionChars = 0; questionChars < answerCharacter.Length; questionChars++)
//         {
//             charsList[questionChars] = char.ToUpper(answerCharacter[questionChars]);
//         }
//         for (int extraChars = answerCharacter.Length; extraChars < optionsSelectableCharactersList.Length; extraChars++)
//         {
//             charsList[extraChars] = (char)UnityEngine.Random.Range(65, 91);
//         }

//         charsList = ShuffleList.ShuffleListItems<char>(charsList.ToList()).ToArray();
//         for (int charsToDisplay = 0; charsToDisplay < optionsSelectableCharactersList.Length; charsToDisplay++)
//         {
//             optionsSelectableCharactersList[charsToDisplay].SetAndDisplayCharacter(charsList[charsToDisplay]);
//         }

//         gameState = GameState.isPlaying;
//     }

//     public void  testSelectedCharFromOptions(CharsWordDataScript selectedChar)
//     {
//         if (gameState == GameState.Next || currentSelectedAnswerIndex >= answerCharacter.Length) return;

//         pickedOptionsIndexList.Add(selectedChar.transform.GetSiblingIndex());
//         answerCharactersList[currentSelectedAnswerIndex].SetAndDisplayCharacter(selectedChar.charsValue);
//         selectedChar.GetComponent<Button>().interactable = false;
//         currentSelectedAnswerIndex++;

//         if (currentSelectedAnswerIndex >= answerCharacter.Length)
//         {
//             isCorrect = true;
//             for (int i = 0; i < answerCharacter.Length; i++)
//             {
//                 if (Char.ToUpper(answerCharacter[i]) != Char.ToUpper(answerCharactersList[i].charsValue))
//                 {
//                     answerCharactersList[i].GetComponent<Image>().color = Color.red; // Incorrect letter in red
//                     isCorrect = false;
//                 }
//                 else
//                 {
//                     answerCharactersList[i].GetComponent<Image>().color = Color.green; // Correct letter in green
//                 }
//             }

//             if (isCorrect)
//             {
//                 Debug.Log("Hurry! Your answer is correct!");
//                 gameState = GameState.Next;

//                 if (availableQuestionIndexes.Count > 0)
//                 {
//                     Invoke(nameof(SetQuestionOptions), 0.5f);
//                 }
//                 else
//                 {
//                     AdsManager.Instance.ShowRewardedInterstitialAd();
//                     if (!gameOverCanvas.activeInHierarchy)
//                     {
//                         gameOverCanvas.SetActive(true);
//                     }
//                 }
//             }
//             else
//             {
//                 Debug.Log("Oh no! Your answer is not correct.");
//                 Invoke(nameof(ResetAnswer), 1.0f);
//             }
//         }
//     }

//     void testResetAnswer()
//     {
//         currentSelectedAnswerIndex = 0;
//         foreach (var charObj in answerCharactersList)
//         {
//             charObj.SetAndDisplayCharacter('_'); // Clear the displayed characters
//             charObj.GetComponent<Image>().color = Color.white; // Reset color
//         }

//         foreach (var pickedIndex in pickedOptionsIndexList)
//         {
//             optionsSelectableCharactersList[pickedIndex].GetComponent<Button>().interactable = true;
//         }

//         pickedOptionsIndexList.Clear();
//     }

//     void testResetQuestionCharacters()
//     {
//         for (int i = 0; i < answerCharactersList.Length; i++)
//         {
//             answerCharactersList[i].gameObject.SetActive(true);
//             answerCharactersList[i].SetAndDisplayCharacter('_');
//             answerCharactersList[i].GetComponent<Image>().color = Color.white;
//         }

//         for (int i = answerCharacter.Length; i < answerCharactersList.Length; i++)
//         {
//             answerCharactersList[i].gameObject.SetActive(false);
//         }

//         for (int i = 0; i < optionsSelectableCharactersList.Length; i++)
//         {
//             optionsSelectableCharactersList[i].GetComponent<Button>().interactable = true;
//         }
//     }
// }