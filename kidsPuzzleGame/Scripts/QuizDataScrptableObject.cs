using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "QuestionOptions", menuName = "Questions", order = 1)]
public class QuizDataScrptableObject : ScriptableObject
{
public List<QuestionOptionsData> questions;
}
