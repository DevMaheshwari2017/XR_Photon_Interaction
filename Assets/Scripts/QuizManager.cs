using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class QuizManager : MonoBehaviourPunCallbacks
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

    private GraphicRaycaster graphicRaycaster;
    private InputSystemUIInputModule inputSystemUIInputModule;
    private int currentQuestionNumber = 0;
    private int currentCubePlaced = 0;
    private int CorrectAnswerScore = 10;
    private int wrongAnswerScore = -5;
    // a functio that tracks number of cube has been placed - on selected in XR socket
    // when the num is 3 make the panel visible 
    // add a score component in the game, whenever a right answer is given +10 and on wrong answer -5 - will be handled by score manager -  need to call the fn here only
    // a set function which set's the score on UI  - will be rpc call in score manager
    // [RPC] call using photonview

    #region MonoBehaviour
    public override void OnEnable()
    {
        base.OnEnable();
        nextBtn.onClick.AddListener(NextQuestion);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        nextBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        inputSystemUIInputModule = FindAnyObjectByType<InputSystemUIInputModule>();
        if (PhotonNetwork.IsMasterClient)
        {
            inputSystemUIInputModule.enabled = false;
            graphicRaycaster.enabled = false;
        }
        else 
        {
            inputSystemUIInputModule.enabled = true;
            graphicRaycaster.enabled = true;
        }
        currentQuestionNumber = 0;
        currentCubePlaced = 0;
        questionText.text = questionsList[currentQuestionNumber].question;
        option1Text.text = questionsList[currentQuestionNumber].Option1;
        option2Text.text = questionsList[currentQuestionNumber].Option2;
        option3Text.text = questionsList[currentQuestionNumber].Option3;
        option4Text.text = questionsList[currentQuestionNumber].Option4;

        questionText.gameObject.SetActive(true);
        optionsParent.SetActive(true);
        finishMessage.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    #region Public Functions
    //Adding it on option Btn in inspector
    public void CheckCorrectAnswer(int optionIndex) 
    {
        if (optionIndex == questionsList[currentQuestionNumber].correctAnswer)
        {
            ScoreManager.Instance.AddScore(CorrectAnswerScore, PlayerSetup.Instance.photonView);
            optionsButtonsList[optionIndex].GetComponent<Image>().color = Color.green;
        }
        else 
        {
            ScoreManager.Instance.AddScore(wrongAnswerScore, PlayerSetup.Instance.photonView);
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

    // On Selected in XRSocketInteractable 
    public void CubeHasBeenPlaced() 
    {
        currentCubePlaced++;
        if (currentCubePlaced == 3) 
        {
            photonView.RPC("ActivateQuizPanel", RpcTarget.AllBuffered);
        }
    }
    #endregion

    #region Private Functions

    [PunRPC]
    private void ActivateQuizPanel() 
    {
        gameObject.SetActive(true);
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
            btn.GetComponent<Image>().color = Color.white; // Back to default color after next question
        }

        nextBtn.gameObject.SetActive(false);
    }
    #endregion
}


