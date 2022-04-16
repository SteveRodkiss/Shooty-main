using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LaserGunScript : MonoBehaviourPunCallbacks
{
    public TrailRenderer laserTrailPrefab;
    public ParticleSystem hitParticleSystem;
    public Transform originTransform;
    public Animator animator;
    [Tooltip("The amount of damage in one hit")]
    public int damage = 35;
    public AudioSource audioSource;
    public AudioClip damageHitMarkerAudioClip;
    public AudioClip laserSoundAudioClip;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && photonView.IsMine)
        {
            //wo am I?
            int userid = PhotonNetwork.LocalPlayer.ActorNumber;
            //run the rpc so even the clone shoots
            //photonView.RPC("Shoot", RpcTarget.All, userid);
            photonView.RPC("ShootVisuals", RpcTarget.All);
            //now check hit- client side hit detection
            ShootCheckHit(userid);
        }
    }

    ////Every clone of this local player shoots too
    //[PunRPC]
    //public void Shoot(int shooterActornumber)
    //{
    //    Debug.Log($"Calling Shoot PunRPC with photonView {photonView} localplayer = {photonView.IsMine}");
    //    //play the shoot animation
    //    animator.SetTrigger("Shoot");
    //    //playe the sound
    //    audioSource.PlayOneShot(laserSoundAudioClip);
    //    var trail = Instantiate(laserTrailPrefab, originTransform.position, originTransform.rotation);
    //    trail.AddPosition(originTransform.position);
    //    //cast forward to see where we hit
    //    var ray = new Ray(originTransform.position, originTransform.forward);
    //    Vector3 hitPoint = originTransform.position + originTransform.forward * 1000f;
    //    if (Physics.Raycast(ray, out RaycastHit hit,1000f))
    //    {
    //        hitPoint = hit.point;
    //        Instantiate(hitParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
    //        //check what we hit and if we are we are the local player do damage to the player we hit
    //        var playerHealth = hit.collider.GetComponent<PlayerHealth>();
    //        if(playerHealth != null && photonView.IsMine)
    //        {
    //            Debug.Log($"Player {photonView} is damaging player {hit.collider.GetComponent<PhotonView>()}");
    //            //playerHealth.TakeDamage(500, shooterActornumber);
    //            audioSource.PlayOneShot(damageHitMarkerAudioClip);
    //        }
    //    }
    //    trail.transform.position = hitPoint;
    //}

    [PunRPC]
    public void ShootVisuals()
    {
        //play the shoot animation
        animator.SetTrigger("Shoot");
        //playe the sound
        audioSource.PlayOneShot(laserSoundAudioClip);
        var trail = Instantiate(laserTrailPrefab, originTransform.position, originTransform.rotation);
        trail.AddPosition(originTransform.position);
        //cast forward to see where we hit
        var ray = new Ray(originTransform.position, originTransform.forward);
        Vector3 hitPoint = originTransform.position + originTransform.forward * 1000f;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            hitPoint = hit.point;
            Instantiate(hitParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
        }
        trail.transform.position = hitPoint;
    }


    //local hit detection- see if a ray from the local player hits a clone
    //if it does, make the clone run an RPC for taking damage/dieing etc.
    public void ShootCheckHit(int shooterActorNumber)
    {
        //cast forward to see where we hit
        var ray = new Ray(originTransform.position, originTransform.forward);
        Vector3 hitPoint = originTransform.position + originTransform.forward * 1000f;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            hitPoint = hit.point;
            Instantiate(hitParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
            //check what we hit and if we are we are the local player do damage to the player we hit
            var playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null && photonView.IsMine)
            {
                Debug.Log($"Player {photonView} is damaging player {hit.collider.GetComponent<PhotonView>()}");
                //call the rpc on the clone's photon view. It gets synced across the network.
                //Client side hit detection!! Very dodgy and easy to hack. ust like roblox!
                playerHealth.photonView.RPC("TakeDamageRPC", RpcTarget.All, damage, shooterActorNumber);
                audioSource.PlayOneShot(damageHitMarkerAudioClip);
            }
        }
    }


}
