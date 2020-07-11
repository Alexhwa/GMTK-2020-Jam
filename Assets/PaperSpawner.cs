﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PaperSpawner : MonoBehaviour
{
    public Camera cam;
    public Collider2D tableCollider;
    public List<GameObject> paperPrefabs;

    public bool canSpawn { get; set; }

    public int waveSize;
    public float spawnTime;
    public float paperLifetime;
    public Rect spawnRegion;
    public float moveTime;
    public float maxRotate;

    private float spawnTimer;

    private int topPaperLayer;

    private void Start() {
        spawnTimer = spawnTime;
    }

    private void Update() {
        if (canSpawn) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime) {
                SpawnPapers();
                spawnTimer -= spawnTime;
            }
        }
    }


    public void SpawnPapers() {
        for (int i = 0; i < waveSize; i++) {
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
            paper.lifeTime = paperLifetime;

            paperObj.transform.DOMove(
                spawnRegion.min + new Vector2(Random.Range(0, spawnRegion.size.x), Random.Range(0, spawnRegion.size.y)), moveTime
            ).SetEase(Ease.OutQuad);
            paperObj.transform.DORotate(
                new Vector3(0, 0, Random.Range(-maxRotate, maxRotate)), moveTime
            ).SetEase(Ease.OutQuad);
        }

        topPaperLayer += 2 * waveSize;
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
}
