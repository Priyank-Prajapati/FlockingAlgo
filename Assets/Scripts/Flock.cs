using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public GameObject boidPrefab;
    public int totalBoids = 30;
    public float spawnRadius = 10;

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;

    void Start()
    {
        for(int i=0; i<totalBoids; i++)
        {
            boidPrefab.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.white, 6f);
            Vector3 boidPos = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            GameObject boid = Instantiate(boidPrefab, this.transform.position + boidPos, Quaternion.identity) as GameObject;
            boid.transform.parent = this.gameObject.transform;
        }
    }
}
