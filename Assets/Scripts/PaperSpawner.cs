using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PaperSpawner : MonoBehaviour
{
    public Camera cam;
    public Collider2D tableCollider;
    public List<GameObject> paperPrefabs;
    public LevelPlayer levelPlayer;

    public bool canSpawn { get { return levelPlayer != null && levelPlayer.levelTimer > 0 && levelPlayer.TimeRemaining > 0 && currentWave < waveDatas.Count; } }

    public List<WaveData> waveDatas;
    public TextMeshProUGUI debugText;
    [HideInInspector] public int currentWave;
    [HideInInspector] public int currentRep;
    [HideInInspector] public float nextRepTime;

    public Rect spawnRegion;
    public float moveTime;
    public float maxRotate;

    private int topPaperLayer;

    private void Start() {
        currentWave = 0;
        currentRep = 0;
        nextRepTime = 0;
    }

    private void Update() {
        if (canSpawn) {
            debugText.text = waveDatas[currentWave].ToString() + " (rep: " + currentRep + ")";
            
            if (levelPlayer.levelTimer > nextRepTime) {
                if (currentRep + 1 >= waveDatas[currentWave].repetitions) {
                    // need next wave
                    currentWave++;
                    currentRep = 0;

                    if (currentWave < waveDatas.Count) {
                        SpawnPapers();
                    }
                }
                else {
                    // need next rep
                    currentRep++;
                    SpawnPapers();
                }
            }
        }
    }


    public void SpawnPapers() {
        WaveData wave = waveDatas[currentWave];

        for (int i = 0; i < wave.waveSize; i++) {
            GameObject paperObj = Instantiate(
                paperPrefabs[Random.Range(0, paperPrefabs.Count)],
                transform.position, Quaternion.identity
            );

            Canvas canvas = paperObj.GetComponent<Canvas>();
            canvas.worldCamera = cam;
            canvas.sortingOrder = 2 * i + topPaperLayer;

            BigPaper paper = paperObj.GetComponent<BigPaper>();
            paper.paperSpawner = this;
            paper.table = tableCollider;
            paper.lifeTime = wave.paperLifetime;

            paperObj.transform.DOMove(
                spawnRegion.min + new Vector2(Random.Range(0, spawnRegion.size.x), Random.Range(0, spawnRegion.size.y)), moveTime
            ).SetEase(Ease.OutQuad);
            paperObj.transform.DORotate(
                new Vector3(0, 0, Random.Range(-maxRotate, maxRotate)), moveTime
            ).SetEase(Ease.OutQuad);
        }

        topPaperLayer += 2 * wave.waveSize;
        nextRepTime += Mathf.Lerp(wave.startSpawnTime, wave.endSpawnTime, (float)currentRep / Mathf.Max(1, wave.repetitions - 1));
    }

    public bool SortToTop(BigPaper paper) {
        if (paper.canvas.sortingOrder == topPaperLayer - 2) {
            return false;
        }

        Debug.Log("Sort " + paper + " to top");
        paper.canvas.sortingOrder = topPaperLayer;
        topPaperLayer += 2;
        
        return true;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(spawnRegion.center, spawnRegion.size.ToVector3(1));
    }


    [System.Serializable]
    public struct WaveData {
        public int repetitions;
        public int waveSize;
        public float startSpawnTime;
        public float endSpawnTime;
        public float paperLifetime;

        public override string ToString() {
            return "WAVE: [" + repetitions + ", " + waveSize + ", " + startSpawnTime + "-" + endSpawnTime + ", " + paperLifetime + "]";
        }
    }
}
