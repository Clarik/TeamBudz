using UnityEngine;

public class EnemyBaseBehavior : MonoBehaviour
{
    HP hp;

    public GameObject soldier;
    public GameObject defender;
    public GameObject allies;

    private void Update()
    {
        hp = GetComponent<HP>();
        if((float)hp.getHp() / 1000f < 0.7)
        {   
            spawnSoldier();
            spawnSoldier();
            spawnSoldier();
            spawnAllies();
        }
    }

    private void spawnSoldier()
    {
        Vector2 spawnLocation = new Vector2(transform.position.x, Random.Range(-8, 8));
        Instantiate(soldier, spawnLocation, Quaternion.identity);
    }

    private void spawnAllies()
    {
        Vector2 spawnLocation = new Vector2(transform.position.x, Random.Range(-8, 8));
        Instantiate(allies, spawnLocation, Quaternion.identity);
    }
}
