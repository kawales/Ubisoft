using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public void disableObject() {
        this.gameObject.SetActive(false);
    }
}
