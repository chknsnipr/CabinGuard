using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static bool inCabin=true;
    public static bool WaveEnds=false;
    public static int kills=0;

    public GameObject Door;

    public static bool inWave=false;

    private bool LastDoorState=true;

    public static int WaveCount=1;

    public GameObject[] SpawnPoints;

    public static float IntermissionTime=0f;
    public SpawnLogic spawnLogic;

    public static bool PlayWaveAudio=false;

    public static bool End=false;

    [Header("End Game Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float holdAfterFade = 0.5f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private bool endSequenceStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnLogic = GetComponent<SpawnLogic>();
        
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        WaveLogic();
        
    }

    void WaveLogic()
    {
        if (End && !endSequenceStarted)
        {
            endSequenceStarted = true;
            StartCoroutine(EndGameFade());
            return;
        }

        if(!inCabin && inWave==false)
        {

            StartWave();
            inWave=true;
            
            PlayWaveAudio=true;
            
            
            
        }

        DoorLogic();
        // Intermission();
        //DoorLogic();
        EOW();



        
    }

    private IEnumerator EndGameFade()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;

            float t = 0f;
            float startAlpha = fadeCanvasGroup.alpha;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = 1f;
        }

        yield return new WaitForSeconds(holdAfterFade);

        NewWave.CleanVars();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void DoorLogic()
    {
        if(!inCabin && LastDoorState==false)
        {
            Door.SetActive(true);
            LastDoorState=true;
            
        }
        if(inCabin && LastDoorState==true)
        {
            LastDoorState=false;
            Door.SetActive(false);
        }
    }

    void Wave1()
    {   WaveCount+=1;
        spawnLogic.SpawnWave(new int[] { 0, 0,1,1});
        
        
    }

    void Wave2()
    {
        WaveCount+=1;
        spawnLogic.SpawnWave(new int[] { 3, 1 ,1,1});
    }

    void Wave3()
    {
        WaveCount+=1;
        spawnLogic.SpawnWave(new int[] { 4, 3 ,1,2});
    }

    void Wave4()
    {
        WaveCount+=1;
        spawnLogic.SpawnWave(new int[] { 5, 4 ,2,1});
    }

    void Wave5()
    {
        End=true;

    }

    void EOW()
    {
        GameObject enemy = GameObject.FindWithTag("enemy");

        if(enemy==null && inWave==true)
        {
            inWave=false;
            WaveEnds=true;

            PlayerController.BonusDMG+=10f;
            PlayerController.CritChance+=0.1f;
            PlayerController.TeleportToTarget();
            inCabin=true;
            
            
        }

        
    }

void Spawn()
{
    spawnLogic.SpawnWave(new int[] { 2, 1 ,0});
}

// void Intermission()
//     {
//         if(inWave==false)
//         {
//             IntermissionTime+=Time.deltaTime;
            
//         }
//     }

    void StartWave()
    {
        if(WaveCount==1)
        {
            Wave1();

        }
        else if(WaveCount==2)
        {
            Wave2();

        }
        else if(WaveCount==3)
        {
            Wave3();
        }
        else if(WaveCount==4)
        {
            Wave4();
        }
        else if(WaveCount==5)
        {
            Wave5();
        }


    }





}