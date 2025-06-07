using UnityEngine;
using System.Collections.Generic;

public class AttackRange : MonoBehaviour
{
    public List<GameObject> enemiesInRange;

    void Awake() 
    {
        enemiesInRange = new List<GameObject>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    void LateUpdate()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.tag == "enemy") {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {   
        if (other.tag == "enemy") {
            enemiesInRange.Remove(other.gameObject);
        }
    }


}
