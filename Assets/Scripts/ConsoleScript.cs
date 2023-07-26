using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ConsoleScript : MonoBehaviour
{
    [Header("External Scripts")]
    private SpikesScript spikesScript;
    private DoorScript doorScript;
    private LaserBeamScript laserScript;
    private SawScript sawScript;
    private OrbTurret turretScript;
    private LaserTurretScript laserTurretScript;

    public List<GameObject> gameObjectsToDeactivate;
    public GameObject persistantParticleSystem;

    [HideInInspector] public bool resetNow;
    private bool playerIsInTrigger;

    // Start is called before the first frame update
    void Start()
    {
        playerIsInTrigger = false;
        resetNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForReset();
        if (Input.GetKeyDown(KeyCode.E) && playerIsInTrigger)
        {
            if (persistantParticleSystem != null)
            {
                persistantParticleSystem.SetActive(false);
            }
            foreach(GameObject obj in gameObjectsToDeactivate)
            {
                spikesScript = obj.GetComponent<SpikesScript>();
                doorScript = obj.GetComponent<DoorScript>();
                sawScript = obj.GetComponent<SawScript>();
                turretScript = obj.GetComponent<OrbTurret>();
                laserScript = obj.GetComponent<LaserBeamScript>();
                laserTurretScript = obj.GetComponent<LaserTurretScript>();

                if (spikesScript != null)
                {
                    spikesScript.spikesUp = false;
                } else if (doorScript != null)
                {
                    doorScript.isOpen = true;
                } else if (sawScript != null)
                {
                    //Debug.Log("Saw is deactivated");
                    sawScript.sawActive = false;
                } else if (turretScript != null)
                {
                    turretScript.isActive = false;
                } else if (laserScript != null)
                {
                    laserScript.laserActive = false;
                } else if (laserTurretScript != null)
                {
                    laserTurretScript.isActive = false;
                }
            }
            this.GetComponent<Animator>().SetBool("isOpen", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)        
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)        
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsInTrigger = false;
        }
    }

    private void CheckForReset()
    {
        if (resetNow)
        {
            this.GetComponent<Animator>().SetBool("isOpen", false);
            resetNow = false;
        }
    }
}
