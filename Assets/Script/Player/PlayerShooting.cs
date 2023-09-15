using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffsetRight;
    [SerializeField] private float shootCooldown = 0.1f;
    private float shootTimer = 0f;
    private Animator anim;
    private PlayerMovement movement;

    [SerializeField] private AudioSource shootingSoundEffect;

    private void Start() 
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if(shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }

        Shoot();
    }

    public void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.K) && shootTimer <=0f)
        {

            shootingSoundEffect.Play();
            Vector3 launchPosition = LaunchOffsetRight.position;
            Quaternion launchRotation = transform.rotation;

            if(!movement.isFacingRight)
            {
                launchPosition = new Vector3(LaunchOffsetRight.position.x, LaunchOffsetRight.position.y, LaunchOffsetRight.position.z);
                launchRotation *= Quaternion.Euler(0f, 180f, 0f);
            }
            anim.SetTrigger("Shoot");
            Instantiate(ProjectilePrefab, launchPosition, launchRotation);

            shootTimer = shootCooldown;
        }
    }
}
