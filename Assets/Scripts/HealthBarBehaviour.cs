using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color color;
    public Vector3 offset;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start() {
        offset = new Vector3(0, 1, 0);
        slider.fillRect.GetComponentInChildren<Image>().color = color;
    }
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
    public void SetHealth(float health, float maxHealth) {
        //Debug.Log("HEALTH JEEEEEEE" + health);
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;
        //slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        slider.fillRect.GetComponentInChildren<Image>().color = color;
    }
}
