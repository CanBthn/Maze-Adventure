using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public float Health;
    float maxHealth = 100;
    public Coins coin;

    public Slider healthSlider;
    private void Update(){
        healthSlider.value = Health;
        if(Health >= 100) Health = 100;
        if(Input.GetKeyDown(KeyCode.H)){
            GetDamage(20);
        }
    }
    public void GetDamage(float damageAmount){
        Health -= damageAmount;
        if(Health <= 0 ){
            SceneManager.LoadScene("Died");
            Debug.Log("YOU ARE DEAD");
        }
    }
    
}