using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private Transform target;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //float randomAngle = Random.Range(-20, 21);
        //transform.Rotate(Vector3.up, randomAngle * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
