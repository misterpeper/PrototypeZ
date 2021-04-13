using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public SC_DamageReceiver player;
    public Texture crosshairTexture;
    public float spawnInterval = 2; //Spawn new enemy each n seconds
    public int enemiesPerWave = 5; //How many enemies per wave
    public Transform[] spawnPoints;

    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;
    //How many enemies we already eliminated in the current wave
    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;

    public GameObject ScoreText;

    GameObject[] clonesToDestroy;

    public Font pixel;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ScoreText.gameObject.SetActive(true);
        //Wait 5 seconds for new wave to start
        newWaveTimer = 5;
        waitingForWave = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForWave)
        {
            if (newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                //Initialize new wave
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
        }
        else
        {
            if (Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;

                //Spawn enemy 
                if (totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    SC_NPCEnemy npc = enemy.GetComponent<SC_NPCEnemy>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }

        // Find clones with tag enemy and destroy them when game is end
        clonesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

        if (player.playerHP == 0)
        {
            ScoreText.gameObject.SetActive(false);
            for (int i=0; i<clonesToDestroy.Length; i++)
            {
                Destroy(clonesToDestroy[i].gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        myButtonStyle.alignment = TextAnchor.MiddleCenter;
        myButtonStyle.font = pixel;

        GUI.backgroundColor = new Color(0, 0, 0, 0);

        GUI.Box(new Rect(10, Screen.height - 105, 300, 75), ((int)player.playerHP).ToString() + " HP", myButtonStyle);
        GUI.Box(new Rect(Screen.width / 2 - 105, Screen.height - 105, 210, 75), player.weaponManager.selectedWeapon.bulletsPerMagazine.ToString(), myButtonStyle);

        if (player.playerHP <= 0)
        {
            GUIStyle myButtonStyle2 = new GUIStyle(GUI.skin.button);
            myButtonStyle2.fontSize = 50;
            myButtonStyle2.alignment = TextAnchor.MiddleCenter;
            myButtonStyle2.font = pixel;
            GUI.Box(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 120, 900, 240), "Game Over\nPress 'Space' to Restart", myButtonStyle2);
        }

        GUI.Box(new Rect(Screen.width / 2 - 105, 35, 210, 75), (enemiesToEliminate - enemiesEliminated).ToString(), myButtonStyle);

        if (waitingForWave)
        {
            GUIStyle myButtonStyle3 = new GUIStyle(GUI.skin.button);
            myButtonStyle3.fontSize = 50;
            myButtonStyle3.alignment = TextAnchor.MiddleCenter;
            myButtonStyle3.font = pixel;
            GUI.Box(new Rect(Screen.width / 2 - 720, Screen.height / 4 - 0, 1440, 80), "Waiting for Wave " + waveNumber.ToString() + ". " + ((int)newWaveTimer).ToString() + " seconds left...", myButtonStyle3);
        }
    }

    public void EnemyEliminated(SC_NPCEnemy enemy)
    {
        enemiesEliminated++;

        if (enemiesToEliminate - enemiesEliminated <= 0)
        {
            //Start next wave
            newWaveTimer = 5;
            waitingForWave = true;
            waveNumber++;
        }
    }
}