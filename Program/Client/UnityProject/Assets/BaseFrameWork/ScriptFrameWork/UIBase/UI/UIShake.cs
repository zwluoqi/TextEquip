using UnityEngine;
using System.Collections;

public class UIShake : MonoBehaviour
{
    float shake = 10;
    bool shakeSwitch = false;
    Vector3 originalPos;

    // Use this for initialization
    void Start ()
    {
        
    }

    public void Shake()
    {
        shakeSwitch = true;
        shake = 10;
        originalPos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update ()
    {
        if (shakeSwitch == true)
        {
            gameObject.transform.localPosition = new Vector3(originalPos.x + Random.Range(0f, shake * 2f) - shake, originalPos.y, originalPos.z);
            //gameObject.transform.localPosition = new Vector3(originalPos.x, originalPos.y + Random.Range(0f, shake * 1f) - shake, originalPos.z);
            shake = shake / 1.1f;

            if (shake < 0.05)
            {
                shake = 0;
                shakeSwitch = false;
                gameObject.transform.localPosition = originalPos;
            }
        }
    }
}
