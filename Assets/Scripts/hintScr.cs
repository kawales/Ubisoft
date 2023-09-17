using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class hintScr : MonoBehaviour
{
    [SerializeField]string[] hints;
    [SerializeField]float[] delays;
    TextMeshProUGUI tHints;
    // Start is called before the first frame update
    void Start()
    {
        tHints=GetComponent<TextMeshProUGUI>();
        StartCoroutine(giveHints());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator giveHints()
    {
        int i=0;
        while(true)
        {
            yield return new WaitForSeconds(delays[i]);
            tHints.text=hints[i];
            i++;
        }
    }
}
