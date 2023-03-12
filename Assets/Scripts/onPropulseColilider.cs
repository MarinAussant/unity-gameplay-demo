using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onPropulseColilider : MonoBehaviour
{

    public bool propulseTriggering = false;
    private GameObject actualPropulse;

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("propulse")){
            propulseTriggering = true;
            actualPropulse = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("propulse")){
            propulseTriggering = false;
        }

    }

    public bool isPropulseTriggering(){
        return propulseTriggering;
    }

    public GameObject getActualPropulse(){
        return actualPropulse;
    }

    public void setIsPropulseTriggering(bool isOrNot){
        propulseTriggering = isOrNot;
    }
}
