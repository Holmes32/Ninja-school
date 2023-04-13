using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class monsterController : MonoBehaviour
{
    // Di chuyen
    public float speed = 1.5f;
    public float changeTime = 3.0f;
    float timer;
    float direction = 0.5f;

    // Component
    Rigidbody2D rigidbody2D_;
    Collider2D colider2D_;
    Animator animator;
    SpriteRenderer sr;
    BoxCollider2D bc;
    public GameObject health;
    UiMonsterHP uiMonster;
    ParticleSystem particle_System;
    public TextMeshProUGUI textMesh;
    public GameObject expPrefab;
    public GameObject hpPrefab;
    public GameObject goldPrefab;
    public LayerMask playerLayer;
    public GameObject player;
    NinjaController player_controller;

    // Thuoc tinh
    public float maxHp = 5.0f;
    public float hp;
    public int atk = 1;
    public int exp;
    public float attackRange = 2;

    // Respawn
    public float timeRespawn = 5.0f;
    float timerRespawn;
    bool isDead;

    // Attack
    public float timeAttack = 3.0f;
    float timerAttack;
    bool isAttacked;

    // Stop attack
    float timeStop = 3.0f;
    float timerStop;




    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D_ = GetComponent<Rigidbody2D>();
        colider2D_ = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //bc = GetComponent<BoxCollider2D>();
        timer = changeTime;
        hp = maxHp;
        isDead = false;
        uiMonster = health.gameObject.GetComponent<UiMonsterHP>();
        particle_System = GetComponent<ParticleSystem>();
        isAttacked = false;
        timerAttack = -1;
        player_controller = player.gameObject.GetComponent<NinjaController>();
        timerStop = timeStop;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            timerRespawn -= Time.deltaTime;
            if (timerRespawn >= 0)
            {
                return;
            }
            isDead = false;
            hp = maxHp;
            sr.enabled = true;
            colider2D_.enabled = true;
            rigidbody2D_.simulated = true;
            health.SetActive(true);
            uiMonster.SetValue(hp / maxHp);
            isAttacked = false;
        }

        if (isAttacked)
        {

            lookPlayer();
            timerAttack -= Time.deltaTime;
            Vector2 distance = player.transform.position - transform.position;
            if (timerAttack < 0)
            {
                Attack();
                if (distance.magnitude > attackRange)
                {
                    isAttacked = false;
                }
                timerAttack = timeAttack;
            }
            return;
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        animator.SetFloat("X", direction);


    }

    public void lookPlayer()
    {
        Vector2 directionAtk = player.transform.position - transform.position;
        if (directionAtk.x > 0)
        {
            animator.SetFloat("X", 0.5f);
        }
        else
        {
            animator.SetFloat("X", -0.5f);
        }
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        foreach (Collider2D player in players)
        {
            player_controller.ChangeHealth(-atk);
        }
    }

    public void Hit(float damege)
    {
        animator.SetTrigger("hit");
        hp -= damege;
        isAttacked = true;
        timerStop = timeStop;
        uiMonster.SetValue(hp / maxHp);
        textMesh.text = "-" + (damege);
        particle_System.Play();
        Debug.Log("hp" + hp);
        if (hp <= 0)
        {
            Dead();
        }

    }

    private void Dead()
    {
        isDead = true;
        timerRespawn = timeRespawn;
        sr.enabled = false;
        rigidbody2D_.simulated = false;
        colider2D_.enabled = false;
        health.SetActive(false);
        dropExp();
        dropGold();
        dropHp();
    }

    void dropExp()
    {
        GameObject exp_obj = Instantiate(expPrefab, transform.position, Quaternion.identity);
        ExpController ec = exp_obj.GetComponent<ExpController>();
        ec.setExp(exp);
    }

    void dropHp()
    {
        int r = Random.Range(1, 3);
        if (r == 1)
        {
            GameObject hp_obj = Instantiate(hpPrefab, transform.position, Quaternion.identity);
            HpController hc = hp_obj.GetComponent<HpController>();
            hc.setHp(1);
        }
    }

    void dropGold()
    {
        int r = Random.Range(100, 1000);
        GameObject gold_obj = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        GoldController gc = gold_obj.GetComponent<GoldController>();
        gc.setQuatity(r);
    }

    private void FixedUpdate()
    {
        if (isDead || isAttacked)
        {
            return;
        }

        Vector2 postition = rigidbody2D_.position;
        postition.x = postition.x + Time.deltaTime * speed * direction;
        rigidbody2D_.MovePosition(postition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isAttacked)
        {
            Attack();
        }
    }
}
