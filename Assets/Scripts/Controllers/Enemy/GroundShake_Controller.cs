using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundShake_Controller : MonoBehaviour
{
    private Transform enemy;
    private float speed;
    private int movingDir;
    private float maxDistance;
    private float time;

    public void Setup(Transform enemy, float speed, int movingDir, float maxDistance)
    {
        this.enemy = enemy;
        this.speed = speed;
        this.movingDir = movingDir;
        this.maxDistance = maxDistance;

        time = maxDistance/ speed;
        gameObject.transform.parent = null;
        transform.position = new Vector3(enemy.transform.position.x, transform.position.y, 0);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movingDir * maxDistance, transform.position.y), speed * Time.deltaTime);

        if (time < 0)
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = enemy;
        }
    }
}
