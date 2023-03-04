using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCollector : MonoBehaviour
{

    private int bananas = 0;

    [SerializeField] private Text bananasScore; 

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("collectible")){
            Destroy(collision.gameObject);
            bananas ++;
            bananasScore.text = "Bananes : " + bananas; 
        }

    }

}
