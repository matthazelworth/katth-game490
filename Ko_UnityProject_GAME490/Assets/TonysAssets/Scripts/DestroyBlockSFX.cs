using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlockSFX : MonoBehaviour
{

    [SerializeField]
    public AudioClip[] blockPops;

    private AudioSource audioSource;

    private ParticleSystem thisParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioClip blockPops = GetRandomClip();
        audioSource.PlayOneShot(blockPops);
    }

    // Update is called once per frame
    void Update()
    {   
   
    }

    private AudioClip GetRandomClip()
    {
        return blockPops[UnityEngine.Random.Range(0, blockPops.Length)];
    }

}
