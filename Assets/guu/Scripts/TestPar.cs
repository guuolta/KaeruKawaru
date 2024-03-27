using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPar : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    void Start()
    {
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
