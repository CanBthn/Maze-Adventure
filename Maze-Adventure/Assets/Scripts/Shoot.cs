using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Shoot : MonoBehaviour
{
    RaycastHit hit;
    public GameObject BulletExitPoint;
    public bool CanFire;
    float GunTimer;
    public float FireSpeed;
    public ParticleSystem MuzzleFlash;
    AudioSource VoiceSource;
    public AudioClip FireVoice;
    public float Range;
    public RecoilAndTrace recoil;
    public GameObject AmmoEffect;
    public Coins coin;
    public float damageEnemy;
    public int totalAmmo = 210;
    public int magazineSize = 30;
    public float reloadTime =5f;
    private int currentAmmo;
    private bool isReloading = false;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI Trigger;
    public TextMeshProUGUI ReloadText;

    void Start(){
        VoiceSource = GetComponent<AudioSource>();
        currentAmmo = magazineSize;
        UpdateAmmoUI();
        
        if (coin == null) {
            coin = FindObjectOfType<Coins>();
        }
    }

    void Update(){

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
        if(totalAmmo==0 && currentAmmo == 0){
            SceneManager.LoadScene("Fail");
        }
        if(Input.GetKey(KeyCode.Mouse0) && CanFire == true && Time.time > GunTimer){
            Fire();
            GunTimer = Time.time + FireSpeed;
            recoil.FireFunc();
            Instantiate(AmmoEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    IEnumerator Reload()
    {
    if(totalAmmo>0){
        ReloadText.text = "Reloading...";
        StartCoroutine(ClearTextAfterDelay2(5f));
        if (isReloading)
        {
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);
        if(currentAmmo==0){
        currentAmmo = magazineSize;
        Debug.Log("Reloaded! Current ammo: " + currentAmmo);
        totalAmmo-=30;
        }
        else{
            if(currentAmmo>totalAmmo && currentAmmo+totalAmmo<=30){
                currentAmmo += totalAmmo;
                totalAmmo=0;
            }
            else{
            int calcAmmo = magazineSize - currentAmmo;
            currentAmmo = magazineSize;
            Debug.Log("Reloaded! Current ammo: " + currentAmmo);
            totalAmmo-= calcAmmo;
            if(totalAmmo<=0)totalAmmo = 0;
            }
        }

        isReloading = false;

        CanFire = true;
        UpdateAmmoUI();
    }
    else{
        Debug.Log("Finish Ammo");
    }
    
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo.ToString() + "/" + totalAmmo.ToString();
        }
    }
    IEnumerator ClearTextAfterDelay(float delay) //For LastWall
    {
        yield return new WaitForSeconds(delay);
        Trigger.text = "";
    }
    IEnumerator ClearTextAfterDelay2(float delay) //For Reloading
    {
        yield return new WaitForSeconds(delay);
        ReloadText.text = "";
    }

    
    public void Fire(){
        if(Physics.Raycast(BulletExitPoint.transform.position, BulletExitPoint.transform.forward, out hit, Range)){
            MuzzleFlash.Play();
            VoiceSource.Play();
            VoiceSource.clip = FireVoice;
            Debug.Log(hit.transform.name);
            if (hit.collider.CompareTag("LastWall")){
                if(coin.CoinCount >= 10){
                    SceneManager.LoadScene("Complete");
                }
                else{
                    Trigger.text = "You do not have enough coins!!";
                    StartCoroutine(ClearTextAfterDelay(3f));
                }
            }
            if(hit.transform.tag == "Enemy"){
                EnemyHealth EnemyHealthScr = hit.transform.GetComponent<EnemyHealth>();
                EnemyHealthScr.DeductHealth(damageEnemy);
            }
            if (currentAmmo > 0)
            {
            currentAmmo--;
            Debug.Log("Fired! Remaining ammo: " + currentAmmo);
            UpdateAmmoUI();
            }
            else{
                CanFire=false;
                Debug.Log("Out of ammo! Reloading...");
            }
        }
    }
}