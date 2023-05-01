using UnityEngine;

public class Track : MonoBehaviour {

    [Header("Pickable param's")]
    [SerializeField] private int spawnChance;
    [SerializeField] private GameObject[] pickables;
    [SerializeField] private Transform[] pickablePoints;

    [Header("Obstacle param's")]
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private Transform[] obstaclePoints;

    private void Start() {
        SpawnRandomPickable();
        SpawnRandomObstacle();
    }

    private void SpawnRandomPickable() {
        foreach (var i in pickablePoints) {
            var rand = Random.Range(0, spawnChance);

            if (rand == 1) {
                Instantiate(pickables[Random.Range(0, pickables.Length)], i.position, Quaternion.identity, i);
            }
        }
    }

    private void SpawnRandomObstacle() {
        foreach (var i in obstaclePoints) {
            Instantiate(obstacles[Random.Range(0, obstacles.Length)], i.position, RandomRotation(), i);
        }
    }

    private Quaternion RandomRotation() {
        var randRot = Random.Range(0, 2);
        var rot = Quaternion.identity;

        switch (randRot) {
            case 0:
                rot = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                rot = Quaternion.Euler(0, 180, 0);
                break;
            default:
                break;
        }

        return rot;
    }
}