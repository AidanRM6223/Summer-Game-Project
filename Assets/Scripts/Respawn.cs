using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
public class Respawn : MonoBehaviour
{
    public Transform lastCheckpoint, startPoint;

    public GameObject PlayerPrefab;
    public bool isRespawned, isRespawning;
    private CinemachineVirtualCamera vcam;
    public GameObject deathScreen, startScreen, winScreen;
    public float deathTimer, deathtime = 3f;
    public int Lives = 3;
    public int Coins = 0;
    GameObject PlayerInstance;
    public bool LevelComplete = false;
    //public GameObject[] coins;
    // Start is called before the first frame update
    private void Start() {
        Lives = 3;
        Coins = 0;
        winScreen.SetActive(false);
        LevelComplete = false;
        lastCheckpoint = startPoint;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(startScreen.GetComponent<CanvasGroup>().alpha == 0) {
            startScreen.SetActive(false);
        }
        if(!LevelComplete) {
            if(GameObject.Find("Player") == null) {
                deathScreen.SetActive(true);
                deathTimer += Time.deltaTime;
                if(deathTimer >= deathtime)
                {
                    //isRespawning = true;
                    if(!isRespawned && deathScreen.GetComponent<Image>().color.a == 1)
                    {
                        PlayerInstance = Instantiate(PlayerPrefab, lastCheckpoint, true);
                        vcam = GameObject.FindGameObjectWithTag("CMCam").GetComponent<CinemachineVirtualCamera>();
                        vcam.Follow = PlayerInstance.transform;
                        PlayerInstance.GetComponent<MeleeCombat>().currentHealth = 100;
                        PlayerInstance.name = "Player";
                        PlayerInstance.SetActive(false);
                        Lives -= 1;
                        isRespawned = true;
                    }
                    if(deathTimer >= deathtime * 2) {
                        deathScreen.SetActive(false);
                        startScreen.SetActive(true);
                        PlayerInstance.SetActive(true);
                        deathTimer = 0;
                    }
                    
                } 
            }
        } else {
            winScreen.SetActive(true);
            if(winScreen.GetComponent<CanvasGroup>().alpha >= 0.5f) {
                GameObject.Find("Player").SetActive(false);
            }
        }
    }
    private void OnApplicationQuit() {
        deathTimer = 0;
        Lives = 3;
    }
}

