using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    private int Coin = 0;

    public int CoinCount
    {
        get { return Coin; }
        set { Coin = value; }
    }

    public TextMeshProUGUI coinText;
    public Shoot shootScript;

    private void Start(){
        if (shootScript == null){
            shootScript = FindObjectOfType<Shoot>();
        }
    }
    public void Update(){
        if(Input.GetKeyDown(KeyCode.C)){
            Coin+=20;
            coinText.text = "Coin: " + CoinCount.ToString();
            Debug.Log(CoinCount);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.transform.tag == "Coin"){
            CoinCount++;
            coinText.text = "Coin: " + CoinCount.ToString();
            Debug.Log(CoinCount);
            Destroy(other.gameObject);
        }
    }
}