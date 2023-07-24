using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SupanthaPaul;
using UnityEngine.SceneManagement;
using TPEnemies;

public class GameManager : MonoBehaviour
{
    [Header("External Scripts")]
    private ConsoleScript consoleScript;
    private SpikesScript spikesScript;
    private DoorScript doorScript;
    private LaserBeamScript laserScript;
    private SawScript sawScript;
    private RewindFunction rewindFunc;
    private EnemyScript enemyScript;
    private EnemyPlayerInteraction enemyDroidScript;
    private OrbTurret turretScript;
    private LaserTurretScript laserTurretScript;
    
    [Header("Game Objects")]
    public GameObject pauseMenu;
    public GameObject GameAudio;
    public TextMeshProUGUI countdownText; 
    public TextMeshProUGUI loopNumText;
    public GameObject spawn;
    public GameObject player;
    public GameObject gameCam;
    public GameObject playerSprite;
    private GameObject[] nonPersistentObjects;
    private GameObject playerCharacter;
    public Sprite rewindSprite;

    [Header("Game Values")]
    public float inputTime = 60f; 
    [HideInInspector] public float countdownTime; 
    int currentSceneIndex = 0;
    private bool isPaused;
    private bool needToCheck = false;
    private bool invicible = false;
    private int loopNum = 0;
    public bool enemiesArePresent;
    private bool pauseCooldownActive = false;
    public Color OGColor;
    [HideInInspector] public bool playerIsDead = false;

    private Coroutine countdownCoroutine;
    public static GameManager instance = null;

    // Ensures that there is only one instance of GameManager
    void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Destroy this, this enforces our singleton pattern so there can only be one instance of GameManager.
            Destroy(gameObject);
        }
    }

    // Called at the start of the game
    private void Start()
    {
        InitializeGameManager();
    }

    // Called each frame, used to check for pause and rewind states
    private void Update()
    {
        CheckRewindState();
        CheckPauseState();
    }

    // Initializes variables required for other functions
    private void InitializeGameManager()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nonPersistentObjects = GameObject.FindGameObjectsWithTag("NonPersistent");
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        rewindFunc = playerCharacter.GetComponent<RewindFunction>();
        gameCam.GetComponent<ChromaticAberration>().enabled = false;

        StartNewCountdown();
    }

    // Checks if the player has finished rewinding, and if so, respawns them
    private void CheckRewindState()
    {
        if(needToCheck && !rewindFunc.isRewinding)
        {
            respawnPlayer();
            needToCheck = false;
        }
    }

    // Checks each frame if the player has pressed the pause button
    private void CheckPauseState()
    {
        if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            Unpause();
        }
    }

    // The IEnumerator used for the countdown timer
    private IEnumerator StartCountdown()
    {
        countdownTime = inputTime;
        while (countdownTime > 0)
        {
            int seconds = Mathf.FloorToInt(countdownTime);
            int milliseconds = Mathf.FloorToInt((countdownTime * 1000) % 1000);

            countdownText.text = string.Format("{0:00}:{1:000}", seconds, milliseconds);

            yield return null; 

            countdownTime -= Time.deltaTime;

            if (countdownTime <= inputTime/3)
            {
                countdownText.color = Color.red;
            }
        }
        countdownText.text = "00:000";
        if (!playerIsDead){
            killPlayer();
        }
    }

    // Called to reset the scene, called by the player when they die
    public void ResetScene()
    {
        if(!invicible){
            foreach (GameObject obj in nonPersistentObjects)
            {
                consoleScript = obj.GetComponent<ConsoleScript>();
                spikesScript = obj.GetComponent<SpikesScript>();
                sawScript = obj.GetComponent<SawScript>();
                doorScript = obj.GetComponent<DoorScript>();
                laserScript = obj.GetComponent<LaserBeamScript>();
                laserTurretScript = obj.GetComponent<LaserTurretScript>();
                if (consoleScript != null)
                {
                    consoleScript.resetNow = true;
                } else if (spikesScript != null)
                {
                    spikesScript.resetNow = true;
                } else if (sawScript != null)
                {
                    sawScript.resetNow = true;
                } else if (doorScript != null)
                {
                    doorScript.isOpen = false;
                } else if (laserScript != null)
                {
                    laserScript.laserActive = true;
                } else if (laserTurretScript != null)
                {
                    laserTurretScript.TurretReset();
                }  
            }
            Pause();
        }
    }
    // Toggle the pause (death) menu, will rewind the player once un-paused
    private void Pause()
    {
        if (!pauseCooldownActive)
        {
            StartCoroutine(PauseCooldown());
            GameAudio.GetComponent<AudioLowPassFilter>().enabled = true;
            loopNum++;
            pauseMenu.SetActive(true);
            isPaused = true;
            loopNumText.text = "LOOP #" + loopNum.ToString();
            gameCam.GetComponent<ChromaticAberration>().enabled = true;
            Time.timeScale = 0f;
        }
    }
    private void Unpause()
    {
        countdownText.color = OGColor;
        playerSprite.GetComponent<Animator>().SetBool("IsDead", false);
        Time.timeScale = 1f;
        isPaused = false;
        rewindPlayer();
    }
    // kills the player
    public void killPlayer()
    {
        if (!invicible)
        {
            playerIsDead = true;
            //Debug.Log("Player has been killed");
            rewindFunc.shouldRecord = false;
            playerSprite.GetComponent<Animator>().SetBool("IsDead", true);
            
            //player.GetComponent<PlayerController>().isCurrentlyPlayable = false;
            player.GetComponent<PlayerController>().canMove = false;
            
            StartCoroutine(KillPlayerCoroutine());
        }
    }

    // Respawns the player, resets all variables to their default values
    public void respawnPlayer()
    {
        //Debug.Log("Player has been respawned");
        GameAudio.GetComponent<AudioLowPassFilter>().enabled = false;
        StartCoroutine(InvicibilityTimer());
        player.transform.position = spawn.transform.position;
        //player.GetComponent<PlayerController>().isCurrentlyPlayable = true;
        player.GetComponent<PlayerController>().canMove = true;
        rewindFunc.shouldRecord = true;
        playerSprite.GetComponent<Animator>().enabled = true;
        playerSprite.GetComponent<Animator>().SetBool("IsDead", false);
        pauseMenu.SetActive(false);
        gameCam.GetComponent<ChromaticAberration>().enabled = false;
        if (enemiesArePresent){
            foreach (GameObject obj in nonPersistentObjects)
            {
                enemyScript = obj.GetComponent<EnemyScript>();
                enemyDroidScript = obj.GetComponent<EnemyPlayerInteraction>();
                if (enemyScript != null)
                {
                    enemyScript.enemyCanMove = true;
                } else if (enemyDroidScript != null)
                {
                    enemyDroidScript.forceReset();
                }
            }
        }
        
        playerIsDead = false;
        StartNewCountdown();
    } 

    // Waits for the player to finish their death animation, then calls ResetScene()
    private IEnumerator KillPlayerCoroutine()
    {
        yield return new WaitForSeconds(1.1f);
        ResetScene();
    }

    // Rewinds the player, called when the player unpauses
    private void rewindPlayer()
    {
        playerSprite.GetComponent<Animator>().enabled = false;
        playerSprite.GetComponent<SpriteRenderer>().sprite = rewindSprite; // To be replaced with animation at some point
        
        rewindFunc.StartRewind();
        playerIsDead = false;
        needToCheck = true;
        //Debug.Log("Attempting to rewind player");
        StopCoroutine(countdownCoroutine);

        foreach (GameObject obj in nonPersistentObjects)
        {
            enemyDroidScript = obj.GetComponent<EnemyPlayerInteraction>();
            enemyScript = obj.GetComponent<EnemyScript>();
            turretScript = obj.GetComponent<OrbTurret>();
            if (enemyScript != null)
            {
                enemiesArePresent = true;
                enemyScript.ForceResetEnemy();
                enemyScript.enemyCanMove = false;
            } else if (turretScript != null)
            {
                turretScript.TurretReset();
            } else if (enemyDroidScript != null)
            {
                enemyDroidScript.forceReset();
            }
        }
    }

    // Starts a new countdown, called when the player respawns
    private void StartNewCountdown()
    {
        if(countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(StartCountdown());
    }

    // Bad solution to prevent player dying immediatly after respawn
    private IEnumerator InvicibilityTimer()
    {
        invicible = true;
        //Debug.Log("Player is invicible");
        yield return new WaitForSeconds(0.7f);
        invicible = false;
        //Debug.Log("Player is no longer invicible");
    }

    private IEnumerator PauseCooldown()
    {
        pauseCooldownActive = true;
        yield return new WaitForSeconds(0.5f);
        pauseCooldownActive = false;
    }
}
