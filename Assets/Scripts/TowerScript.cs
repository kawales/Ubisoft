using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    private int hp = 2;
    public GameObject gate;
    bool gateOpen;
    bool gateMoving;
    bool cantClose;

    [Header("SFX")]
    [SerializeField] AudioClip openGateSound;
    [SerializeField] AudioClip closeGateSound;


    void Start()
    {
        
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            StartCoroutine(openGate());
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            StartCoroutine(closeGate());
        }
    }

    public void takeDamage() {
        if(--hp == 1) {
            StartCoroutine(openGate());
            cantClose = true;
            //pokrene se kapija na gore

        }
        else {
            playerComboScr.lostGame(LayerMask.LayerToName(transform.gameObject.layer));
        }
    }
    public void openGateFun() {
        StartCoroutine(openGate());
    }
    public void closeGateFun() {
        StartCoroutine(closeGate());
    }
    public void OpenAndClose(float delay) {
        StartCoroutine(OpenAndCloseCoroutine(delay));
    }
    IEnumerator OpenAndCloseCoroutine(float delay) {
        StartCoroutine(openGate());
        
        yield return new WaitForSeconds(delay);
        StartCoroutine(closeGate());
    }
    IEnumerator openGate() {
        float y = 0;
        
        if (gateOpen != true && gateMoving == false && cantClose == false) {
            gateOpen = true;
            gateMoving = true;
            Transform gateTransformNew = gate.transform;
            SoundManager.instance.PlaySound(openGateSound);

            while (y < 1.02) {
                //Debug.Log(Mathf.Abs(gate.transform.position.y) + " " + y);
                y += 0.07f;
                gate.transform.position += new Vector3(0, 0.07f, 0);
                yield return new WaitForSeconds(0.05f);
            }
            gateMoving = false;
        }
        yield return null;
    }
    IEnumerator closeGate() {
        if (gateOpen != false && gateMoving == false && cantClose == false) {
            gateOpen = false;
            gateMoving = true;
            float y = 0;
            Transform gateTransformNew = gate.transform;

            SoundManager.instance.PlaySound(closeGateSound);
            while (y < 1.02) {
                y += 0.07f;
                gate.transform.position -= new Vector3(0, 0.07f, 0);
                yield return new WaitForSeconds(0.05f);
            }
            gateMoving = false;
        }
        yield return null;
    }
}
