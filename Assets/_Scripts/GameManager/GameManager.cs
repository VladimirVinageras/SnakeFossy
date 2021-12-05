using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTimer; // 30 for a 40 seconds level if player speed is 8
    [SerializeField] private List<Color> toEatColors;
    [SerializeField] private List<Color> toAVoidColors;
    private int сolorToEatListPointer = 0;
    private int сolorToAvoidListPointer = 0;
    
    private bool
        isFeverRush,
        hasSnakeToGrow,
        isTimeOut;
    
    private int 
        diamondCounter,
        peopleCounter,
        rushCounter=0;
        
    public TextMeshProUGUI diamondsScore;
    public TextMeshProUGUI peopleScore;
    public TextMeshProUGUI gameOverText;
    public GameObject wellDoneScreen;
    public GameObject gameOverScreen;
    public GameObject controlsScreen;
    public GameObject restartButton;

    public bool IsTimeOut => isTimeOut;
    public bool IsFeverRush
    {
        get => isFeverRush;
        set => isFeverRush = value;
    }

    public int RushCounter => rushCounter;


    public bool SnakeHasToGrow
    {
        get => hasSnakeToGrow;
        set => hasSnakeToGrow = value;
    }
    
    
    public static GameManager Instance { get; private set;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
        
    }
    
    public void Start()
    {
     StartGame();  
     
    }


    public void StartGame()
    {
        isTimeOut = false;
        gameOverScreen.SetActive(false);
        wellDoneScreen.SetActive(false);
        controlsScreen.SetActive(true);
        restartButton.gameObject.SetActive(false);
        diamondsScore.text = "0" ;
        peopleScore.text = "0";
        peopleCounter = 0;
        diamondCounter = 0;
        rushCounter = 0;
        Time.timeScale = 1;
        StartCoroutine("GameCountdown");
        
        
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverScreen.SetActive(true);
        controlsScreen.SetActive(false);
        Time.timeScale = 0;
    }
    
    public void WellDone()
    {
        restartButton.gameObject.SetActive(true);
        controlsScreen.SetActive(false);
        wellDoneScreen.SetActive(true);
        Time.timeScale = 0;
    }


    
    public void UpdateDiamondScore(int scoreToAdd)
    {
        diamondCounter += scoreToAdd;
        diamondsScore.text = diamondCounter.ToString();
    }

    public void RestartDiamondScore()
    {
        diamondCounter = 0;
        diamondsScore.text = diamondCounter.ToString();
    }

    public void UpdatePeopleScore(int scoreToAdd)
    {
        peopleCounter += scoreToAdd;
        peopleScore.text = peopleCounter.ToString();
    }

    public void UpdateRushCounter()
    {
        rushCounter++;
    }

    
    IEnumerator GameCountdown()
    {
        yield return new WaitForSeconds(gameTimer);
        isTimeOut = true;
    }

    public Color GetColorForPeopleToEat()
    {
        Color pColor = toEatColors[сolorToEatListPointer];
        Debug.Log(pColor +" in "+сolorToEatListPointer +"Position");
        сolorToEatListPointer++;
        if (toEatColors.Count < сolorToEatListPointer)
        {
            Debug.Log(toEatColors.Count);
            Debug.Log(сolorToEatListPointer);
            сolorToEatListPointer = 0;
        }

        return pColor;
    }

    
    public Color GetColorForPeopleToAvoid()
    {
        Color pColor = toAVoidColors[сolorToAvoidListPointer];
        Debug.Log(pColor +" in "+сolorToAvoidListPointer +"Position");
        сolorToAvoidListPointer++;
        if (toAVoidColors.Count < сolorToAvoidListPointer)
        {
            Debug.Log(toEatColors.Count);
            Debug.Log(сolorToAvoidListPointer);
            сolorToAvoidListPointer = 0;
        }

        return pColor;
    }
}
