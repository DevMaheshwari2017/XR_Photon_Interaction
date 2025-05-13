using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuizManager : MonoBehaviour
{
    [SerializeField] private List<Questions> questionsList;
    [SerializeField] private List<Button> optionsButtonsList;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;
    [SerializeField] private TextMeshProUGUI option4Text;
    [SerializeField] private TextMeshProUGUI finishMessage;
    [SerializeField] private Button nextBtn;
    [SerializeField] private GameObject optionsParent;
    private int currentQuestionNumber = 0;

    private void OnEnable()
    {
        nextBtn.onClick.AddListener(NextQuestion);
    }
    private void OnDisable()
    {
        nextBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        currentQuestionNumber = 0;
        questionText.text = questionsList[currentQuestionNumber].question;
        option1Text.text = questionsList[currentQuestionNumber].Option1;
        option2Text.text = questionsList[currentQuestionNumber].Option2;
        option3Text.text = questionsList[currentQuestionNumber].Option3;
        option4Text.text = questionsList[currentQuestionNumber].Option4;

        questionText.gameObject.SetActive(true);
        optionsParent.SetActive(true);
        finishMessage.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
    }
    //Adding it on option Btn in inspector
    public void CheckCorrectAnswer(int optionIndex) 
    {
        if (optionIndex == questionsList[currentQuestionNumber].correctAnswer)
        {
            optionsButtonsList[optionIndex].GetComponent<Image>().color = Color.green;
        }
        else 
        {
            optionsButtonsList[questionsList[currentQuestionNumber].correctAnswer].GetComponent<Image>().color = Color.green;
            optionsButtonsList[optionIndex].GetComponent<Image>().color = Color.red;
        }

        currentQuestionNumber++;

        foreach (Button btn in optionsButtonsList)
        {
            btn.interactable = false;
        }
        if (currentQuestionNumber != questionsList.Count)
        {
            nextBtn.gameObject.SetActive(true);
        }
        else 
        {
            StartCoroutine(FinishQuiz());
        }
    }

    private IEnumerator FinishQuiz() 
    {
        yield return new WaitForSeconds(1f);
        optionsParent.SetActive(false);
        questionText.gameObject.SetActive(false);

        finishMessage.gameObject.SetActive(true);
    }
    private void NextQuestion() 
    {
        questionText.text = questionsList[currentQuestionNumber].question;
        option1Text.text = questionsList[currentQuestionNumber].Option1;
        option2Text.text = questionsList[currentQuestionNumber].Option2;
        option3Text.text = questionsList[currentQuestionNumber].Option3;
        option4Text.text = questionsList[currentQuestionNumber].Option4;

        foreach (Button btn in optionsButtonsList)
        {
            btn.interactable = true;
            btn.GetComponent<Image>().color = Color.white; // default color
        }

        nextBtn.gameObject.SetActive(false);
    }
}


