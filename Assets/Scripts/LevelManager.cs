using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public bool levelActive;
    private bool levelVictory;

    private Castle[] theCastles;

    public List<EnemyHealthController> activeEnemies = new List<EnemyHealthController>();
    //private SimpleEnemySpawner enemySpawner;
    private EnemyWaveSpawner[] waveSpawners;

    public string nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        theCastles = FindObjectsOfType<Castle>();
        //enemySpawner = FindObjectOfType<SimpleEnemySpawner>();
        waveSpawners = FindObjectsOfType<EnemyWaveSpawner>();

        levelActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelActive)
        {
            float totalCastleHealth = 0;
            foreach(Castle castle in theCastles)
            {
                totalCastleHealth += castle.currentHealth;
            }

            if(totalCastleHealth <= 0)
            {
                levelActive = false;
                levelVictory = false;

                //Debug.Log("Level Failed!");
                
                UIController.instance.towerButton.SetActive(false);
            }

            bool wavesComplete = true;
            foreach(EnemyWaveSpawner waveSpawn in waveSpawners)
            {
                if(waveSpawn.wavesToSpawn.Count > 0)
                {
                    wavesComplete = false;
                }
            }

            if(activeEnemies.Count == 0 && wavesComplete)
            {
                levelActive = false;
                levelVictory = true;

                //Debug.Log("Level Won!");
                
                UIController.instance.towerButton.SetActive(false);
            }

            if(!levelActive)
            {
                UIController.instance.levelFailScreen.SetActive(!levelVictory);
                UIController.instance.levelCompleteScreen.SetActive(levelVictory);

                UIController.instance.CloseTowerUpgradePanel();
            }
        }
    }
}
