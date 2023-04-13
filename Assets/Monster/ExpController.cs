using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpController : MonoBehaviour
{
    public int exp = 2;
    // Start is called before the first frame update

    public void setExp(int e)
    {
        exp = e;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            NinjaController player = other.gameObject.GetComponent<NinjaController>();
            player.ChangeEXP(exp);
            Destroy(gameObject);
        }
    }
}
