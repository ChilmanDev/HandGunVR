using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

enum Difficulty { None, Hard, Medium, Easy}

public class GameManager : MonoBehaviour
{
    [SerializeField] ObjectSpawner spawner;
    [SerializeField] Player player;
    public Music music;
    AudioSource audioSource;

    [SerializeField] Difficulty difficulty;
    [SerializeField] int delayStart = 5;
    bool startedPlaying = false;

    [SerializeField]int currentScore, scorePerTarget = 10;

    int currentMultiplier, multiplierTracker;
    [SerializeField] int[] multiplierThreshold;

    [SerializeField] TextMeshPro scoreText;
    [SerializeField] TextMeshPro multiplierText;

    int totalTargets, targetHits, targetMisses;
   
    [SerializeField] GameObject resultScreen;
    [SerializeField] TextMeshProUGUI hitsText, missesText, finalScoreText;
    
    [SerializeField] LineRenderer leftRenderer, rightRenderer;
    [SerializeField] XRInteractorLineVisual leftInteractor, rightInteractor;

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {

        currentScore = 0;
        currentMultiplier = 1;
        totalTargets = targetHits = targetMisses = 0;

        Instance = this;

        audioSource = music.audioSource;

        StartCoroutine("StartMusic");

        spawner.Activate(music.beatTempo, (int)difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        CheckMusicState();
        CheatCodes();
    }

    IEnumerator StartMusic(){
        yield return new WaitForSeconds(music.beatTempo * delayStart);

        startedPlaying = true;
        audioSource.Play();
    }

    public void TargetDestroyed()
    {
        if(currentMultiplier - 1 < multiplierThreshold.Length)
        {
            multiplierTracker++;

            if(multiplierThreshold[currentMultiplier -1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        currentScore += scorePerTarget * currentMultiplier;

        targetHits++;
    }

    public void TargetMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;

        targetMisses++;
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        multiplierText.text = "Multiplier: " + currentMultiplier + "x";
    }
    
    public void AddTarget()
    {
        totalTargets++;
    }

    void CheckMusicState()
    {
        if(!startedPlaying)
            return;

        if(!audioSource.isPlaying && !resultScreen.activeInHierarchy)
        {
            spawner.Deactivate();
            resultScreen.SetActive(true);

            hitsText.text = "" + targetHits;
            missesText.text = "" + targetMisses;
            finalScoreText.text = "" + currentScore;
            
            leftRenderer.enabled = false;
            rightRenderer.enabled = false;
            
            leftInteractor.enabled = true;
            rightInteractor.enabled = true;
        }
    }

    void CheatCodes()
    {
        if(Input.GetKeyDown(KeyCode.F7))
            audioSource.Stop();
    }
}
