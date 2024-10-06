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
    public GameObject zur�ckButton;
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
            "Willkommen zum Experiment!\n\nBitte lesen Sie sich die nachfolgenden Textinstruktionen gut durch. Neben den Textinstruktionen werden teilweise Videoaufnahmen abgespielt, die den Ablauf und die Steuerung des Experiments veranschaulichen.\n\nAchten Sie bitte auch aufmerksam auf die <b>Textinstruktionen</b>, die <b>zwischen den Bl�cken</b> angezeigt werden. Diese k�ndigen mitunter <b>relevante Steuerungs�nderungen</b> in nachfolgenden Bl�cken an.\n\nBitte unterhalten Sie sich nicht mit anderen Personen w�hrend des Versuchs.\n\nSie k�nnen fortfahren, indem Sie die rechte obere Schultertaste mit dem rechten Zeigefinger bet�tigen.",
            "In diesem Versuch m�ssen Sie einem Lachs helfen, Str�mungen auszugleichen, einem Grizzly-B�ren zu entkommen und m�glichst viele gro�e Algen-Sterne einzusammeln.",
            "Ein Durchgang startet, wenn Sie den Controller gerade halten.\n\n<b>Bitte halten Sie den Controller w�hrend des gesamten Versuchs mit beiden H�nden fest, so wie es rechts zu sehen ist</b>. Ihr linker und rechter Daumen sollte jeweils auf dem linken bzw. rechten Joystick ruhen.",
            "Der Lachs wird sich <b>automatisch nach vorne</b> bewegen.\n\nVon Anfang an wird er jedoch durch eine <b>Str�mung</b> beeinflusst, die ihn nach <b>rechts oder links</b> dr�ckt.\n\n<b>In einem Versuchsteil</b> m�ssen Sie entweder mit dem <b>rechten oder linken Joystick</b> eine <b>Ausgleichbewegung nach rechts oder links</b> ausf�hren, um den <b>Lachs entgegen der Str�mung nach rechts oder links</b> zu bewegen.\n\nWelchen Joystick Sie daf�r nutzen m�ssen, wird Ihnen vor den Versuchsbl�cken mitgeteilt.",
            "Im <b>anderen Versuchsteil</b> m�ssen Sie den <b>linken bzw. rechten Joystick entweder nach vorne bzw. hinten</b> bewegen, um den Lachs zur�ck in die Mitte zu bewegen. \n\n<b>Achten Sie auf die Instruktionen vor den Versuchsbl�cken</b>, die Ihnen nochmal die jeweilige <b>Steuerungsbelegung</b> erkl�rt.",
            "Sie m�ssen die <b>Ausgleichbewegung wiederholt ausf�hren</b>, um die Str�mung auszugleichen.\n\nAchten Sie darauf, m�glichst <b>geradlinige Joystickbewegungen</b> auszuf�hren, wie sie auch rechts im Video zu sehen sind.\n\nVermeiden Sie <b>schr�ge Bewegungen</b> in die jeweilige Richtung, da diese <b>keinen Effekt</b> auf den Lachs haben.",
            "Nach einiger Zeit erscheinen <b>Algen-Sterne</b> im linken bzw. rechten Flusslauf.\n\nIn einigen Durchg�ngen sind die <b>Sterne unterschiedlich gro�</b> und Sie erhalten <b>mehr Punkte</b>, wenn Sie den <b>gr��eren Stern einsammeln</b>.\n\nSind die <b>Sterne gleich gro�</b>, erhalten Sie f�r beide <b>gleich viele Punkte</b>.",
            "Sobald sich der <b>Lachs hinter den Markierungen</b> kurz vor dem Grizzly befindet, k�nnen Sie mit dem <b>anderen Joystick (mit dem Sie die Str�mung aktuell nicht ausgleichen) in einen der beiden Fl�sse wechseln</b>.\n\nEine <b>Joystickbewegung nach links</b> f�hrt dazu, dass der <b>Lachs nach links springt.</b>\n\nEine <b>Joystickbewegung nach rechts</b> f�hrt dazu, dass der <b>Lachs nach rechts springt.</b>",
            "Auch <b>nach dem Flusswechsel</b> m�ssen Sie die <b>Str�mungen noch ausgleichen</b>.\n\nWenn der <b>Lachs den jeweiligen Stern erreicht</b>, erhalten sie <b>automatisch</b> entsprechend der Sterngr��e 30 (klein), 50 (mittel) oder 70 (gro�) <b>Punkte</b>.\n\nDer n�chste Durchgang startet, wenn Sie den Controller f�r einige Zeit gerade gehalten haben.",
            "<b>Sammeln Sie so viele Punkte</b>, wie m�glich!\n\nBeachten Sie dabei Folgendes:\n\n<b>Gleichen Sie die Str�mung nicht genug aus, wird der Lachs angeschwemmt und Sie erhalten keine Punkte.</b>\n\n<b>Wechseln Sie den Fluss nicht rechtzeitig, wird der Lachs vom Grizzly gefressen und Sie erhalten keine Punkte.</b>\n\n<b>Halten Sie den Controller vor der Sprungzone zu schief oder f�hren Sie mit dem Entscheidungsjoystick eine Bewegung vor der Sprungzone aus, z�hlt dies als Fehler und Sie erhalten keine Punkte.</b>\n\n<b>Nehmen Sie bitte eine aufrechte und entspannte Haltung ein und halten Sie w�hrend des gesamten Experiments ungef�hr den gleichen Abstand zum Bildschirm ein.</b>",
            "Wenn Sie sich die <b>Instruktionen nochmals durchlesen m�chten</b>, nutzen Sie bitte die <b>linke obere Schultertaste</b>, um zur�ckzubl�ttern.\n\nWenn Sie fortfahren, werden Ihnen die Instruktionen und die Steuerungsbelegung f�r den ersten Versuchsteil angezeigt. <b>Lesen Sie sich diese bitte aufmerksam durch.</b>"
        };

        instruLength = instructions.Length;

        instructionOverlapR = "<b>Str�mungsausgleich</b>:\n" +
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>rechten Joystick</b> aus:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = <b>Lachs nach rechts</b> bewegen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = <b>Lachs nach links</b> bewegen</b>\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";

        instructionOverlapL = "<b>Str�mungsausgleich</b>:\n" + 
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = <b>Lachs nach rechts</b> bewegen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = <b>Lachs nach links</b> bewegen</b>\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";

        instructionNonOverlapR = "<b>Str�mungsausgleich</b>:\n" +
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>rechten Joystick</b> aus.\n\n" +
            "<b>Unabh�ngig von der Str�mungsrichtung</b> m�ssen sie den <b>rechten Joystick nach vorne bewegen</b>, um den Lachs entgegen der Str�mung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";

        instructionNonOverlapL = "<b>Str�mungsausgleich</b>:\n" + 
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "<b>Unabh�ngig von der Str�mungsrichtung</b> m�ssen sie den <b>linken Joystick nach vorne bewegen</b>, um den Lachs entgegen der Str�mung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";

        instructionNonOverlapR2 = "<b>Str�mungsausgleich</b>:\n" + 
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>rechten Joystick</b> aus.\n\n" +
            "<b>Unabh�ngig von der Str�mungsrichtung</b> m�ssen sie den <b>rechten Joystick nach hinten bewegen</b>, um den Lachs entgegen der Str�mung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>linken Joystick</b>:\n\n" +
            "Linken <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Linken <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";

        instructionNonOverlapL2 = "<b>Str�mungsausgleich</b>:\n" + 
            "In den n�chsten Bl�cken gleichen Sie die <b>Str�mung</b> mit dem <b>linken Joystick</b> aus:\n\n" +
            "<b>Unabh�ngig von der Str�mungsrichtung</b> m�ssen sie den <b>linken Joystick nach hinten bewegen</b>, um den Lachs entgegen der Str�mung zu bewegen.\n\n\n" +
            "<b>Flusswechsel</b>:\n" +
            "<b>Entscheidungen</b> treffen Sie mit dem <b>rechten Joystick</b>:\n\n" +
            "Rechten <b>Joystick nach rechts</b> bewegen = In den <b>rechten Fluss</b> springen\n\n" +
            "Rechten <b>Joystick nach links</b> bewegen = In den <b>linken Fluss</b> springen" +
            "\n\nSammeln Sie so viele Punkte wie m�glich!\n\n" +
            "Fahren Sie fort, um mit dem �bungsblock zu beginnen.";


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
        zur�ckButton = GameObject.Find("Zur�ck");
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
                    zur�ckButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                    zur�ckButton.GetComponent<Image>().color = Color.black;
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
            currentText = "Als N�chstes folgt der " + blockNumCorrector + ". Versuchsblock.\n\n" +
                    "Die Steuerungsbelegung im nachfolgenden Block ist die gleiche, wie im vorherigen Block.\n\n" +
                    "Sammeln Sie so viele Punkte wie m�glich!\n\nWenn Sie m�chten, k�nnen Sie eine kurze Pause einlegen.\n\nDr�cken Sie R1, um mit dem Block zu beginnen.";
        }
        else
        {
            int maxPoints = (blockNumCorrector - 1) * 3040;

            currentText = "Als N�chstes folgt der " + blockNumCorrector + ". Versuchsblock.\n\n" +
                        "Ihr Punktestand betr�gt aktuell " + (score - trainingScore) + " von bisher " + maxPoints + " m�glichen Punkten (Trainingsbl�cke nicht inkludiert).\n\n" +
                        "Die Steuerungsbelegung im nachfolgenden Block ist die gleiche, wie im vorherigen Block.\n\n" +
                        "Sammeln Sie weiterhin so viele Punkte wie m�glich!\n\nWenn Sie m�chten, k�nnen Sie eine kurze Pause einlegen.\n\nDr�cken Sie R1, um mit dem Block zu beginnen.";
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
