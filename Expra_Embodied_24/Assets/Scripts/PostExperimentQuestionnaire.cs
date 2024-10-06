using UnityEngine;
using UXF;
using UnityEngine.UI;
using TMPro;

public class PostExperimentQuestionnaire : MonoBehaviour
{

    public Session session;
    public GameObject cursor;
    public GameObject canvas;
    public GameObject weiterButton;
    public GameObject zur�ckButton;

    public string[] questionnaire;
    private float pauseTime;

    public int questionCounter;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        weiterButton = GameObject.Find("Weiter");
        zur�ckButton = GameObject.Find("Zur�ck");
        canvas = GameObject.Find("Canvas");

        questionnaire = new string[] {"Das war der Hauptteil des Experiments. Sie haben insgesamt " + (cursor.GetComponent<CursorClass>().score - cursor.GetComponent<CursorClass>().trainingScore) + " Punkte gesammelt. Sehr gut!\n\n" + "Es folgt nun noch ein kurzer Fragebogen zum Experiment. Bitte lesen Sie sich die Fragen genau durch. Um eine Antwort einzutippen, m�ssen Sie eventuell mit der Maus das Eingabefeld anklicken. Anschlie�end k�nnen Sie die Tastatur f�r die Antworteingabe zu nutzen.",
            "Haben Sie eine Vermutung, um was es in diesem Versuch ging?\n\n(Sie k�nnen erst nach 5 Sekunden zu n�chsten Frage fortfahren)",
            "Haben Sie bestimmte Strategien bei der Durchf�hrung des Experiments angewandt?\n\n(Sie k�nnen erst nach 5 Sekunden zu n�chsten Frage fortfahren)",
            "Hatten Sie Probleme bei der Durchf�hrung des Experiments?\n\n(Sie k�nnen erst nach 5 Sekunden zu n�chsten Frage fortfahren)",
            "Wie viele Stunden pro Woche spielen Sie durchschnittlich Videospiele?\n\n(Sie k�nnen erst nach 5 Sekunden zu n�chsten Frage fortfahren)",
            "Wie viele Stunden pro Woche spielen Sie durchschnittlich Videospiele <b>mit einem Controller</b>?\n\n(Sie k�nnen erst nach 5 Sekunden zu n�chsten Frage fortfahren)",
            "M�chten Sie uns sonst noch etwas zum Versuch mitteilen?\n\n(Sie k�nnen erst nach 5 Sekunden fortfahren)",
            "Mit der rechten Maustaste beenden Sie das Experiment. Melden Sie sich danach bei der Versuchsleitung. Vielen Dank f�r Ihre Teilnahme!"
        };

        questionCounter = 0;
    }

    public void runQuestionnaire()
    {
        if (true)
        {
            Cursor.visible = true;
            var postExperimentQuestionnaire = GameObject.Find("PostExperimentQuestionnaire");
            var textInput = postExperimentQuestionnaire.GetComponent<TMP_InputField>();

            weiterButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Weiter = rechte Maustaste";
            zur�ckButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            zur�ckButton.GetComponent<Image>().color = Color.black;

            questionnaire[0] = "Das war der Hauptteil des Experiments. Sie haben insgesamt " + (cursor.GetComponent<CursorClass>().score - cursor.GetComponent<CursorClass>().trainingScore) + " von 18240 m�glichen Punkten gesammelt. Sehr gut!\n\n\n" + "Es folgt nun noch ein kurzer Fragebogen zum Experiment.\n\n<b>Bitte lesen Sie sich die Fragen genau durch.</b> Um eine Antwort einzutippen, m�ssen Sie eventuell mit der linken Maustaste das Eingabefeld anklicken. Anschlie�end k�nnen Sie die Tastatur f�r die Antworteingabe zu nutzen.\n\nBet�tigen Sie die rechte Maustaste, um zum Fragebogen fortzufahren.";

            if (questionCounter <= questionnaire.Length - 1)
            {

                var currentText = questionnaire[questionCounter];
                if (postExperimentQuestionnaire.GetComponent<CanvasGroup>().alpha == 0)
                {
                    ActivateCanvas();
                }

                if (questionCounter >= 1)
                {
                    ActivateInputField(postExperimentQuestionnaire, true);
                }

                GameObject.Find("Instruction").GetComponent<TextMeshProUGUI>().text = currentText;

                if (postExperimentQuestionnaire.GetComponent<TMP_InputField>().interactable == false)
                    postExperimentQuestionnaire.GetComponent<TMP_InputField>().interactable = true;

                if (questionCounter == (questionnaire.Length - 1))
                {
                    postExperimentQuestionnaire.GetComponent<CanvasGroup>().alpha = 0;
                }

                if (Input.GetMouseButtonDown(1) & Time.time - pauseTime > 5.0f & !cursor.gameObject.GetComponent<CursorClass>().experimentActive & questionCounter < questionnaire.Length)
                {
                    session.participantDetails.Add("question" + questionCounter, textInput.text);
                    questionCounter++;
                    pauseTime = Time.time;
                    postExperimentQuestionnaire.GetComponent<TMP_InputField>().text = "";
                }

            }

            else
            {
                var currentText = "";
                GameObject.Find("Instruction").GetComponent<TextMeshProUGUI>().text = currentText;
                weiterButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                weiterButton.GetComponent<Image>().color = Color.black;
                ActivateInputField(postExperimentQuestionnaire, false);
            }
        }

    }

    private void ActivateCanvas()
    {
        cursor.gameObject.GetComponent<CursorClass>().experimentActive = false;
        canvas.GetComponent<CanvasGroup>().alpha = 1;
        canvas.GetComponent<CanvasGroup>().interactable = true;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void ActivateInputField(GameObject postExperimentQuestionnaire, bool activated)
    {
        switch (activated)
        {
            case false:
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().alpha = 0;
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().interactable = false;
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().blocksRaycasts = false;
                break;
            case true:
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().alpha = 1;
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().interactable = true;
                postExperimentQuestionnaire.GetComponent<CanvasGroup>().blocksRaycasts = true;
                break;
        }

    }

}
