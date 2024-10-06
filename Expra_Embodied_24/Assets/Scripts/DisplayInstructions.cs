using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DualSenseUnity;
using UXF;
using UnityEngine.UI;
using System.Linq;

public class DisplayInstructions : MonoBehaviour

{
    private CursorControls CursorControls;

    public Session session;

    private string[] instructions;

    private string[] practiceInstructions;

    private string instructionOverlapR;

    private string instructionOverlapL;

    private string instructionNonOverlapR;

    private string instructionNonOverlapL;

    private string instructionNonOverlapR2;

    private string instructionNonOverlapL2;

    private bool instructionsOrdered = false;

    private string[] ConditionInstructions;

    private string[] ConditionPictures;

    private int ConditionInstructionCounter = 0;

    private string[] questionnaire;

    private GameObject canvas;
    private CanvasGroup canvasGroup;
    public GameObject cursor;
    public GameObject zurückButton;
    public GameObject weiterButton;
    public CursorClass cursorClass;
    private GameObject examplePic;
    private RawImage examplePicRawImage;
    private TextMeshProUGUI tmpUGUI;

    private int instruCounter;
    //private int practInstruCounter;

    private int instruLength;
    private int previousBlockNum;

    private bool instruFlag;
    private float pauseTime;
    private Texture instruPic;


    private UnityEngine.Video.VideoPlayer Video;
    private bool VideoPlayed = false;

    private GameObject VideoRawImage;

    private float waitTimeBetweenSlides = 3.00f;

    // Start is called before the first frame update
    void Start()
    {
        instructions = new string[] { 
            "Willkommen zum Experiment!\n\nBitte lesen Sie sich die nachfolgenden Textinstruktionen gut durch. Neben den Textinstruktionen werden teilweise Videoaufnahmen abgespielt, die den Ablauf und die Steuerung des Experiments veranschaulichen.\n\nAchten Sie bitte auch aufmerksam auf die <b>Textinstruktionen</b>, die <b>zwischen den Blöcken</b> angezeigt werden. Diese kündigen mitunter <b>relevante Steuerungsänderungen</b> in nachfolgenden Blöcken an.\n\nBitte unterhalten Sie sich nicht mit anderen Personen während des Versuchs.\n\nSie können fortfahren, indem Sie die rechte obere Schultertaste mit dem rechten Zeigefinger betätigen.",
            "In diesem Versuch müssen Sie einem Lachs helfen, Strömungen auszugleichen, einem Grizzly-Bären zu entkommen und möglichst viele große Algen-Sterne einzusammeln.",
            "Ein Durchgang startet, wenn Sie den Controller gerade halten.\n\n<b>Bitte halten Sie den Controller während des gesamten Versuchs mit beiden Händen fest, so wie es rechts zu sehen ist</b>. Ihr linker und rechter Daumen sollte jeweils auf dem linken bzw. rechten Joystick ruhen.",
            "Der Lachs wird sich <b>automatisch nach vorne</b> bewegen.\n\nVon Anfang an wird er jedoch durch eine <b>Strömung</b> beeinflusst, die ihn nach <b>rechts oder links</b> drückt.\n\n<b>In einem Versuchsteil</b> müssen Sie entweder mit dem <b>rechten oder linken Joystick</b> eine <b>Ausgleichbewegung nach rechts oder links</b> ausführen, um den <b>Lachs entgegen der Strömung nach rechts oder links</b> zu bewegen.\n\nWelchen Joystick Sie dafür nutzen müssen, wird Ihnen vor den Versuchsblöcken mitgeteilt.",
            "Im <b>anderen Versuchsteil</b> müssen Sie den <b>linken bzw. rechten Joystick entweder nach vorne bzw. hinten</b> bewegen, um den Lachs zurück in die Mitte zu bewegen. \n\n<b>Achten Sie auf die Instruktionen vor den Versuchsblöcken</b>, die Ihnen nochmal die jeweilige <b>Steuerungsbelegung</b> erklärt.",
            "Sie müssen die <b>Ausgleichbewegung wiederholt ausführen</b>, um die Strömung auszugleichen.\n\nAchten Sie darauf, möglichst <b>geradlinige Joystickbewegungen</b> auszuführen, wie sie auch rechts im Video zu sehen sind.\n\nVermeiden Sie <b>schräge Bewegungen</b> in die jeweilige Richtung, da diese <b>keinen Effekt</b> auf den Lachs haben.",
            "Nach einiger Zeit erscheinen <b>Algen-Sterne</b> im linken bzw. rechten Flusslauf.\n\nIn einigen Durchgängen sind die <b>Sterne unterschiedlich groß</b> und Sie erhalten <b>mehr Punkte</b>, wenn Sie den <b>größeren Stern einsammeln</b>.\n\nSind die <b>Sterne gleich groß</b>, erhalten Sie für beide <b>gleich viele Punkte</b>.",
            "Sobald sich der <b>Lachs hinter den Markierungen</b> kurz vor dem Grizzly befindet, können Sie mit dem <b>anderen Joystick (mit dem Sie die Strömung aktuell nicht ausgleichen) in einen der beiden Flüsse wechseln</b>.\n\nEine <b>Joystickbewegung nach links</b> führt dazu, dass der <b>Lachs nach links springt.</b>\n\nEine <b>Joystickbewegung nach rechts</b> führt dazu, dass der <b>Lachs nach rechts springt.</b>",
            "Auch <b>nach dem Flusswechsel</b> müssen Sie die <b>Strömungen noch ausgleichen</b>.\n\nWenn der <b>Lachs den jeweiligen Stern erreicht</b>, erhalten sie <b>automatisch</b> entsprechend der Sterngröße 30 (klein), 50 (mittel) oder 70 (groß) <b>Punkte</b>.\n\nDer nächste Durchgang startet, wenn Sie den Controller für einige Zeit gerade gehalten haben.",
            "<b>Sammeln Sie so viele Punkte</b>, wie möglich!\n\nBeachten Sie dabei Folgendes:\n\n<b>Gleichen Sie die Strömung nicht genug aus, wird der Lachs angeschwemmt und Sie erhalten keine Punkte.</b>\n\n<b>Wechseln Sie den Fluss nicht rechtzeitig, wird der Lachs vom Grizzly gefressen und Sie erhalten keine Punkte.</b>\n\n<b>Halten Sie den Controller vor der Sprungzone zu schief oder führen Sie mit dem Entscheidungsjoystick eine Bewegung vor der Sprungzone aus, zählt dies als Fehler und Sie erhalten keine Punkte.</b>\n\n<b>Nehmen Sie bitte eine aufrechte und entspannte Haltung ein und halten Sie während des gesamten Experiments ungefähr den gleichen Abstand zum Bildschirm ein.</b>",
            "Wenn Sie sich die <b>Instruktionen nochmals durchlesen möchten</b>, nutzen Sie bitte die <b>linke obere Schultertaste</b>, um zurückzublättern.\n\nWenn Sie fortfahren, werden Ihnen die Instruktionen und die Steuerungsbelegung für den ersten Versuchsteil angezeigt. <b>Lesen Sie sich diese bitte aufmerksam durch.</b>"
        };

        instruLength = instructions.Length;

        instructionOverlapR = "<b>Strömungsausgleich</b>:\n" +
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>rechten Joystick</b> aus:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = <b>Lachs nach rechts</b> bewegen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = <b>Lachs nach links</b> bewegen</b>\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";

        instructionOverlapL = "<b>Strömungsausgleich</b>:\n" + 
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = <b>Lachs nach rechts</b> bewegen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = <b>Lachs nach links</b> bewegen</b>\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";

        instructionNonOverlapR = "<b>Strömungsausgleich</b>:\n" +
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>rechten Joystick</b> aus.\n\n" +
            "<b>Unabhängig von der Strömungsrichtung</b> müssen sie den <b>rechten Joystick nach vorne bewegen</b>, um den Lachs entgegen der Strömung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";

        instructionNonOverlapL = "<b>Strömungsausgleich</b>:\n" + 
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "<b>Unabhängig von der Strömungsrichtung</b> müssen sie den <b>linken Joystick nach vorne bewegen</b>, um den Lachs entgegen der Strömung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";

        instructionNonOverlapR2 = "<b>Strömungsausgleich</b>:\n" + 
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>rechten Joystick</b> aus.\n\n" +
            "<b>Unabhängig von der Strömungsrichtung</b> müssen sie den <b>rechten Joystick nach hinten bewegen</b>, um den Lachs entgegen der Strömung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";

        instructionNonOverlapL2 = "<b>Strömungsausgleich</b>:\n" + 
            "In den nächsten Blöcken gleichen Sie die <b>Strömung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "<b>Unabhängig von der Strömungsrichtung</b> müssen sie den <b>linken Joystick nach hinten bewegen</b>, um den Lachs entgegen der Strömung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie möglich!\n\n" +
            "Fahren Sie fort, um mit dem Übungsblock zu beginnen.";


        cursor = GameObject.Find("Cursor");
        cursorClass = cursor.GetComponent<CursorClass>();
        instruCounter = 0;
        canvas = GameObject.Find("Canvas");
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        instruFlag = false;

        previousBlockNum = 0;

        practiceInstructions = new string[] {
            "PRACTICE PLACEHOLDER OVERLAP",
            "PRACTICE PLACEHOLDER NON-OVERLAP"
        };

                
        //practInstruCounter = 0;

        examplePic = GameObject.Find("ExamplePic");
        examplePicRawImage = examplePic.GetComponent<RawImage>();

        pauseTime = Time.time;

        CursorControls = GameObject.Find("Controller").GetComponent<CursorControls>();

        canvasGroup.alpha = 0;

        instruPic = Resources.Load("MLTT3D_Instru1") as Texture;

        tmpUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        zurückButton = GameObject.Find("Zurück");
        weiterButton = GameObject.Find("Weiter");

        Video = GameObject.Find("Video Player").GetComponent<UnityEngine.Video.VideoPlayer>();

        VideoRawImage = GameObject.Find("RawImage");

        VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 0);

    }

    public void Display_Instructions()
    {
        if (cursorClass.displayingInstructions && !session.InTrial && session.hasInitialised && !(CursorControls._controllerCount == 0))
        {
            int[] counterbalanceArray = { 1, 2, 5, 6};

            int counterbalance = (int)session.participantDetails["counterbalance"];

            bool isCounterbalanceOdd = (int)session.participantDetails["counterbalance"] % 2 != 0;

            if (!instructionsOrdered)
            {
                ConditionInstructions = CreateConditionInstructionArray(counterbalance);
                ConditionPictures = CreateConditionPictureArray(counterbalance);
                instructionsOrdered = true;
            }

            if (!instruFlag)
            {
                canvasGroup.alpha = 1;
                if (CursorControls.get_bumper_press()[1] == ButtonState.Down && Time.time - pauseTime > waitTimeBetweenSlides)
                {
                    Proceed();
                    VideoPlayed = false;
                    pauseTime = Time.time;
                }
                else if (CursorControls.get_bumper_press()[0] == ButtonState.Down && Time.time - pauseTime > waitTimeBetweenSlides)
                {
                    Return();
                    VideoPlayed = false;
                    pauseTime = Time.time;
                }

                if ((instruCounter >= 1 && instruCounter < 9) && !VideoPlayed)
                {
                    VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 1);

                    Video.url = SelectVideo(instruCounter, counterbalance, isCounterbalanceOdd);

                    Video.Prepare();

                    Video.isLooping = true;
                    Video.Play();
                    VideoPlayed = true;
                }

                else if (instruCounter == 9)
                {
                    VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 0);
                    VideoPlayed = false;
                    Video.Stop();
                }

                //examplePicRawImage.texture = instruPic;
                if (instruCounter < 1 || instruCounter > 7)
                    examplePicRawImage.color = Color.black;
                else
                    examplePicRawImage.color = Color.black;

                if (instruCounter >= instruLength)
                {

                    cursorClass.displayingInstructions = true;
                    cursorClass.displayingConditionsInstructions = true;
                    instruFlag = true;
                    zurückButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                    zurückButton.GetComponent<Image>().color = Color.black;
                    Wait();

                }
                else
                {
                    var currentText = instructions[instruCounter];
                    tmpUGUI.text = currentText;
                }

            }

            else if (instruFlag && cursorClass.displayingInstructions && (cursorClass.currentBlock == 1 || cursorClass.currentBlock == 5))
            {
                
                var currentText = "This text should not be displayed.";
                if (cursorClass.displayingConditionsInstructions)
                {
                    examplePic.GetComponent<RectTransform>().sizeDelta = new Vector2(540.0f, 960.0f);

                    switch (ConditionInstructionCounter)
                    {
                        case 0:
                            currentText = ConditionInstructions[0];
                            instruPic = Resources.Load(ConditionPictures[0]) as Texture;
                            break;
                        case 1:
                            currentText = ConditionInstructions[1];
                            instruPic = Resources.Load(ConditionPictures[1]) as Texture;
                            break;
                    }

                    if (!VideoPlayed)
                    {
                        VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 1);
                        Video.url = SelectVideo(instruCounter, counterbalance, isCounterbalanceOdd);

                        Video.Prepare();

                        Video.isLooping = true;
                        Video.Play();
                        VideoPlayed = true;
                    }

                    //Debug.Log(currentText);
                }

                var currentBlockNum = cursorClass.currentBlock;

                tmpUGUI.text = currentText;

                if (!cursorClass.displayingConditionsInstructions && CursorControls.get_bumper_press()[1] == ButtonState.Down && Time.time - pauseTime > waitTimeBetweenSlides)
                {
                    cursorClass.stillStart = Time.time;
                    ControlCanvasDisplay(false);
                    cursorClass.displayingInstructions = false;
                    cursorClass.stillStart = Time.time;
                    pauseTime = Time.time;
                    previousBlockNum = currentBlockNum;
                    tmpUGUI.text = "";
                    VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 0);
                    VideoPlayed = false;
                    Video.Stop();
                }

                else if (cursorClass.displayingConditionsInstructions && CursorControls.get_bumper_press()[1] == ButtonState.Down && Time.time - pauseTime > waitTimeBetweenSlides)
                {
                    cursorClass.stillStart = Time.time;
                    ControlCanvasDisplay(false);
                    cursorClass.displayingInstructions = false;
                    cursorClass.displayingConditionsInstructions = false;
                    pauseTime = Time.time;
                    previousBlockNum = currentBlockNum;
                    tmpUGUI.text = "";
                    ConditionInstructionCounter++;
                    
                }

                else if ((currentBlockNum == 1 || currentBlockNum == 5) && (currentBlockNum != previousBlockNum))
                {
                    ControlCanvasDisplay(true);
                    cursorClass.displayingInstructions = true;
                    cursorClass.experimentActive = false;
                }
            }
            
            else if (instruFlag && cursorClass.displayingInstructions && !(cursorClass.currentBlock == 1 || cursorClass.currentBlock == 5))
            {
                //Debug.Log("Block Summary Display");
                int blockNumCorrected = cursorClass.currentBlock < 5 ? cursorClass.currentBlock - 1 : cursorClass.currentBlock - 2;

                string currentText = SetFeedbackText(cursorClass.currentBlock, blockNumCorrected, cursorClass.score, cursorClass.trainingScore);
                int currentBlockNum = cursorClass.currentBlock;

                tmpUGUI.text = currentText;
                examplePicRawImage.color = Color.black;

                VideoRawImage.GetComponent<RawImage>().color = new Vector4(1, 1, 1, 0);
                VideoPlayed = false;
                Video.Stop();

                if (!cursorClass.experimentActive && CursorControls.get_bumper_press()[1] == ButtonState.Down & Time.time - pauseTime > waitTimeBetweenSlides)
                {

                    ControlCanvasDisplay(false);
                    cursorClass.displayingInstructions = false;
                    cursorClass.stillStart = Time.time;
                    cursorClass.blockScore = 0;
                    pauseTime = Time.time;
                    previousBlockNum = currentBlockNum;
                    tmpUGUI.text = "";
                }
                else if (!(currentBlockNum == 1 || currentBlockNum == 5) && (currentBlockNum != previousBlockNum))
                {
                    cursorClass.trainingScore = 0;
                    ControlCanvasDisplay(true);
                    cursorClass.experimentActive = false;
                }

            }
                        
        }

    }

    private void Proceed()
    {
        //Debug.Log("Right button pressed.");
        if (instruCounter < instruLength)
            instruCounter++;
    }

    private void Return()
    {
        if (instruCounter >= 1)
            instruCounter--;
    }

    public bool startExperiment()
    {
        if (instruCounter < instruLength)
            return false;
        else { 
            return true;
    }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }

    private void ControlCanvasDisplay(bool display)
    {
        canvasGroup.alpha = display ? 1 : 0;
        canvasGroup.interactable = display;
        canvasGroup.blocksRaycasts = display;
    }

    private string SetFeedbackText(int currentBlock, int blockNumCorrector, int score, int trainingScore)
    {
        string currentText = "";

        if (blockNumCorrector == 1)
        {
            currentText = "Als Nächstes folgt der " + blockNumCorrector + ". Versuchsblock.\n\n" +
                    "Die Steuerungsbelegung im nachfolgenden Block ist die gleiche, wie im vorherigen Block.\n\n" +
                    "Sammeln Sie so viele Punkte wie möglich!\n\nWenn Sie möchten, können Sie eine kurze Pause einlegen.\n\nDrücken Sie R1, um mit dem Block zu beginnen.";
        }
        else
        {
            int maxPoints = (blockNumCorrector - 1) * 3040;

            currentText = "Als Nächstes folgt der " + blockNumCorrector + ". Versuchsblock.\n\n" +
                        "Ihr Punktestand beträgt aktuell " + (score - trainingScore) + " von bisher " + maxPoints + " möglichen Punkten (Trainingsblöcke nicht inkludiert).\n\n" +
                        "Die Steuerungsbelegung im nachfolgenden Block ist die gleiche, wie im vorherigen Block.\n\n" +
                        "Sammeln Sie weiterhin so viele Punkte wie möglich!\n\nWenn Sie möchten, können Sie eine kurze Pause einlegen.\n\nDrücken Sie R1, um mit dem Block zu beginnen.";
        }

        return currentText;
    }

    private string SelectVideo(int instruCounter, int counterbalance, bool isCounterbalanceOdd)
    {
        string m_path = Application.dataPath;

        //Debug.Log("data path: " + m_path);

        string videoURL = m_path + "/Resources/ExPra_24_Videoshowcase-converted.webm";

        bool counterbalanceSmallerFive = counterbalance < 5;

        if (!cursorClass.displayingConditionsInstructions)
        {
            Debug.Log(instruCounter);
            if (instruCounter == 2)
                videoURL = m_path + "/StreamingAssets/Hold_Controller-converted.webm";
            else if ((instruCounter == 3) && isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm";
            else if (instruCounter == 3 && !isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm";
            else if (instruCounter == 4 && counterbalanceSmallerFive && isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Forward-converted.webm";
            else if (instruCounter == 4 && !counterbalanceSmallerFive && isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Backward-converted.webm";
            else if (instruCounter == 4 && counterbalanceSmallerFive && !isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Forward-converted.webm";
            else if (instruCounter == 4 && !counterbalanceSmallerFive && !isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Backward-converted.webm";
            else if ((instruCounter == 5 || instruCounter == 7) && isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm";
            else if ((instruCounter == 5 || instruCounter == 7) && !isCounterbalanceOdd)
                videoURL = m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm";
            else
                videoURL = m_path + "/StreamingAssets/ExPra_24_Videoshowcase-converted.webm";
        }
        else if (cursorClass.displayingConditionsInstructions)
        {
            Dictionary<int, string> videoURLsCondition0 = new Dictionary<int, string>
        {
            { 1, m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm" },
            { 2, m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm" },
            { 3, m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Forward-converted.webm" },
            { 4, m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Forward-converted.webm" },
            { 5, m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm" },
            { 6, m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm" },
            { 7, m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Backward-converted.webm" },
            { 8, m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Backward-converted.webm" }
        };

            Dictionary<int, string> videoURLsCondition1 = new Dictionary<int, string>
        {
            { 1, m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Forward-converted.webm" },
            { 2, m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Forward-converted.webm" },
            { 3, m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm" },
            { 4, m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm" },
            { 5, m_path + "/StreamingAssets/Track_Switch_Right_NonOverlap_Backward-converted.webm" },
            { 6, m_path + "/StreamingAssets/Track_Switch_Left_NonOverlap_Backward-converted.webm" },
            { 7, m_path + "/StreamingAssets/Track_Switch_Right_Overlap-converted.webm" },
            { 8, m_path + "/StreamingAssets/Track_Switch_Left_Overlap-converted.webm" }
        };

            if (ConditionInstructionCounter == 0)
            {
                videoURLsCondition0.TryGetValue(counterbalance, out videoURL);
            }
            else if (ConditionInstructionCounter == 1)
            {
                videoURLsCondition1.TryGetValue(counterbalance, out videoURL);
            }

            if (!string.IsNullOrEmpty(videoURL))
            {
                return videoURL;
            }
            else
            {
                Debug.LogError("Invalid counterbalance value or video URL not found.");
                return videoURL;
            }
        }

        return videoURL;
    }

    private string[] CreateConditionInstructionArray(int counterbalance)
    {
        string[] conditionInstructions = new string[] {"", ""};

        if (counterbalance == 1)
        {
            conditionInstructions[0] = instructionOverlapR;
            conditionInstructions[1] = instructionNonOverlapR;
        }

        else if (counterbalance == 2)
        {
            conditionInstructions[0] = instructionOverlapL;
            conditionInstructions[1] = instructionNonOverlapL;
        }

        else if (counterbalance == 3)
        {
            conditionInstructions[0] = instructionNonOverlapR;
            conditionInstructions[1] = instructionOverlapR;
        }

        else if (counterbalance == 4)
        {
            conditionInstructions[0] = instructionNonOverlapL;
            conditionInstructions[1] = instructionOverlapL;
        }

        else if (counterbalance == 5)
        {
            conditionInstructions[0] = instructionOverlapR;
            conditionInstructions[1] = instructionNonOverlapR2;
        }

        else if (counterbalance == 6)
        {
            conditionInstructions[0] = instructionOverlapL;
            conditionInstructions[1] = instructionNonOverlapL2;
        }

        else if (counterbalance == 7)
        {
            conditionInstructions[0] = instructionNonOverlapR2;
            conditionInstructions[1] = instructionOverlapR;
        }

        else if (counterbalance == 8)
        {
            conditionInstructions[0] = instructionNonOverlapL2;
            conditionInstructions[1] = instructionOverlapL;
        }

        return conditionInstructions;
    }

    private string[] CreateConditionPictureArray(int counterbalance)
    {
        string[] conditionPictures = new string[] { "", ""};

        if (counterbalance == 1)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 2)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 3)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 4)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 5)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 6)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 7)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        else if (counterbalance == 8)
        {
            conditionPictures[0] = "PLACEHOLDER";
            conditionPictures[1] = "PLACEHOLDER";
        }

        return conditionPictures;
    }

}
