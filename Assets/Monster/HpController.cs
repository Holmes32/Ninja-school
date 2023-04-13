using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public int hp = 1;

    public void setHp(int hp)
    {
        this.hp = hp;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            NinjaController player = other.gameObject.GetComponent<NinjaController>();
            player.ChangeHealth(hp);
            Destroy(gameObject);
        }
    }
}
