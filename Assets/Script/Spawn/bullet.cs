using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float destroyTime = 3f;
    private float speed = 0.2f;
    Rigidbody2D rb;
    private Vector2 direction;
    float timerDestroy;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       timerDestroy = destroyTime;
    }
    void Update()
    {
        
        if(timerDestroy>=0)
        {
            timerDestroy -= Time.deltaTime;
        }
        else{
            //gameObject.SetActive(false);
            ObjectPool.instance.DeSpawn(gameObject);
            timerDestroy=destroyTime;
        }
    }
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(direction.x, direction.y);
        transform.Rotate(Vector3.forward, 30f);
        transform.position = Vector2.MoveTowards(transform.position, direction, speed);
    }
    public void SetDirection(Vector2 _direction)
    {
        direction = _direction;
        
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 7) //Layer "monster" = 7
        {
            monsterController mc = collider.gameObject.GetComponent<monsterController>();
            mc.Hit(1);
           //gameObject.SetActive(false);
           ObjectPool.instance.DeSpawn(gameObject);
        }
        
    }
}
