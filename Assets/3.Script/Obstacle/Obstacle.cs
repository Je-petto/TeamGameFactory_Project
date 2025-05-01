using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData data;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerBehaviour player = col.transform.parent.parent.GetComponent<PlayerBehaviour>();

            if (player.health - data.damage < 0) player.health = 0;
            else player.health -= data.damage;

            Destroy(gameObject);
        }
    }
}
