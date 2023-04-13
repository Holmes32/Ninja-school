using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    public int quantity = 10;
    // Start is called before the first frame update
    public void setQuatity(int q) {
        quantity = q;
    }

     private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            NinjaController player = other.gameObject.GetComponent<NinjaController>();
            
            Destroy(gameObject);
        }
    }
}
