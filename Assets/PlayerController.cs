using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool canJump, isJumping, isDead, hasWon;

    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    public Transform jumpTarget;

    public GlassController jumpTargetGlass;

    public int currentGlassIndex;

    public Animator animator;

    public Transform ragdolHips;
    public GameObject realPlayerBody, ragdollBody;
    public CameraFollow cameraFollow;

    public AudioClip fallSfx, jumpSfx, glassBreakSfx;
    AudioSource audioSource;
    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PressJump(false);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            PressJump(true);
        }

        Jump();
    }
    void PressJump(bool jumpToRight)
    {
        if (!canJump || isJumping) return;

        canJump = false;

        StartCoroutine(PressJumpCoroutine(jumpToRight));
        IEnumerator PressJumpCoroutine(bool jumpToRight)
        {
            animator.SetTrigger("Jump");

            yield return new WaitForSeconds(0.5f);
            audioSource.PlayOneShot(jumpSfx);
            isJumping = true;
            rb.velocity = Vector3.up * 5;

            if (currentGlassIndex >= BridgeManager.instance.totalRow)
            {
                hasWon = true;
                jumpTarget = BridgeManager.instance.goalPivot;
                yield break;
            }


            if (jumpToRight)
            {
                jumpTarget = BridgeManager.instance.glasses[currentGlassIndex, 1].transform;
                jumpTargetGlass = jumpTarget.GetComponent<GlassController>();
            }
            else
            {
                jumpTarget = BridgeManager.instance.glasses[currentGlassIndex, 0].transform;
                jumpTargetGlass = jumpTarget.GetComponent<GlassController>();
            }

            currentGlassIndex++; 

        }
    }

    void Jump()
    {
        if (!isJumping) return;

        transform.position = Vector3.MoveTowards(transform.position, jumpTarget.position, 5 * Time.deltaTime);

        if (Vector3.Distance(transform.position, jumpTarget.position) < 0.5f)
        {
            animator.SetTrigger("Idle");
            CheckGlass();
        }
    }

    void CheckGlass()
    {
        if (jumpTargetGlass.isBroken)
        {
            audioSource.PlayOneShot(glassBreakSfx);

            isJumping = false;
            capsuleCollider.height = 1;
            animator.SetTrigger("Falling");
            print("Kaca Pecah");
            jumpTarget.GetComponent<GlassController>().BreakGlass();

            StartCoroutine(DelaySfxFall());
            IEnumerator DelaySfxFall()
            {
                yield return new WaitForSeconds(0.5f);
                audioSource.PlayOneShot(fallSfx);
            }
            
        }
        else
        {
            isJumping = false;
            canJump = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Death>())
        {
            Death();
        }
    }

    void Death()
    {
        realPlayerBody.SetActive(false);
        ragdollBody.SetActive(true);

        cameraFollow.playerTarget = ragdolHips;

        StartCoroutine(RestratCoroutine());
        IEnumerator RestratCoroutine()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
