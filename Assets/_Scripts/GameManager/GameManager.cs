using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float gameTimer; // 20 seconds level if player speed is 7
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
    public GameObject wellDoneScreen,
                      gameOverScreen,
                      controlsScreen, 
                      restartButton;

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

    public static GameManager Instance
    {
        get; private set;
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
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
        сolorToEatListPointer = 0;
        сolorToAvoidListPointer = 0;
        gameOverScreen.SetActive(false);
        wellDoneScreen.SetActive(false);
        // controlsScreen.SetActive(true);   //Check enable if is active another variant of controls in player controller
        restartButton.gameObject.SetActive(false);
        diamondsScore.text = "0";
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
    public Color GetColorForPeopleToEat()
    {
        if ( сolorToEatListPointer >= toEatColors.Count )
        {
            сolorToEatListPointer = 0;
        }
        Color pColorToEat = toEatColors[сolorToEatListPointer];
        сolorToEatListPointer++;
        return pColorToEat;
    }
    public Color GetColorForPeopleToAvoid()
    {
        if (сolorToAvoidListPointer >=toAVoidColors.Count )
        {
            сolorToAvoidListPointer = 0;
        }
        Color pColorToAvoid = toAVoidColors[сolorToAvoidListPointer];
        сolorToAvoidListPointer++;
        return pColorToAvoid;
    }
    
    IEnumerator GameCountdown()
    {
        yield return new WaitForSeconds(gameTimer);
        isTimeOut = true;
    }
}
