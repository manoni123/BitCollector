using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform playerTrans;
    private float shakeAmplitude;
    private Vector3 shakeActive;
    public bool isShake;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera Shake
        if (shakeAmplitude > 0)
        {
            shakeActive = new Vector3(Random.Range(-shakeAmplitude, shakeAmplitude), Random.Range(-shakeAmplitude, shakeAmplitude), 0);
            shakeAmplitude -= Time.deltaTime;
        }
        else
        {
            shakeActive = Vector3.zero;
        }

        if (isShake)
        {
            transform.position += shakeActive;
        }
    }

    public void CameraShake(float _shakeAmount)
    {
        shakeAmplitude = _shakeAmount;
    }
}
