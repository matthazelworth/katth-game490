using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips_LF;

    [SerializeField]
    private AudioClip[] clips_RF;

    [SerializeField]
    private AudioClip[] clips_LFR;

    [SerializeField]
    private AudioClip[] clips_RFR;

    private bool isWalking;
    private bool isRunning;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
    }

    private void LeftFootstep()
    {
        if (isWalking)
        {
            AudioClip clips_LF = GetRandomClip_LF();
            audioSource.PlayOneShot(clips_LF);
        }
    }
    private void RightFootstep()
    {
        if (isWalking)
        {
            AudioClip clips_RF = GetRandomClip_RF();
            audioSource.PlayOneShot(clips_RF);
        }
    }
    private void LeftFootRun()
    {
        if (isRunning)
        {
            AudioClip clips_LFR = GetRandomClip_LFR();
            audioSource.PlayOneShot(clips_LFR);
        }
    }
    private void RightFootRun()
    {
        if (isRunning)
        {
            AudioClip clips_RFR = GetRandomClip_RFR();
            audioSource.PlayOneShot(clips_RFR);
        }
    }

    private AudioClip GetRandomClip_LF()                                    //Get random clips for left foot and right foot
    {
        return clips_LF[UnityEngine.Random.Range(0, clips_LF.Length)];
    }
    private AudioClip GetRandomClip_RF()
    {
        return clips_RF[UnityEngine.Random.Range(0, clips_RF.Length)];
    }

    private AudioClip GetRandomClip_LFR()
    {
        return clips_LFR[UnityEngine.Random.Range(0, clips_LFR.Length)];    //get random clips for left foot and right foot when running
    }
    private AudioClip GetRandomClip_RFR()
    {
        return clips_RFR[UnityEngine.Random.Range(0, clips_RFR.Length)];
    }

}
