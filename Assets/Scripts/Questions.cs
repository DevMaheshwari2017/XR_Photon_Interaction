using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/QuizManager/Questions", order = 1)]
public class Questions : ScriptableObject
{
    public string question;
    public string Option1;
    public string Option2;
    public string Option3;
    public string Option4;
    public int correctAnswer;
}
