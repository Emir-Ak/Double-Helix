using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{

    public PlayerController[] players;
    public GameController gameController;
    public int cost = 1000;

    int dna;
    /// <summary>
    /// Upgrades players
    /// </summary>
    /// <param name="upgradeID">1: Health 2: Speed 3: Recharge T 4: Restore T 5: Bullets</param>
    public void UpgradeStat(int upgradeID)
    {
        float health = players[0].standardHealth, speed = players[0].standardSpeed, rechargeTime = players[0].bulletRechargeTime, restoreTime = players[0].restoreTime;
        int bulletNum = players[0].bulletNum;

        dna = gameController.dna;

        cost = PlayerPrefs.GetInt("UpgradeCost");



        switch (upgradeID)
        {
            case 1:
                if (dna - cost * 2 > 0)
                {
                    health *= (1f + 10f / 100f);
                    gameController.dnaText.text = "-" + (cost * 1.5f).ToString();
                    Invoke("ReturnDNAText", 1f);
                    dna -= (cost * 1);
                }
                break;
            case 2:
                if (dna - cost * 2 > 0)
                {
                    speed *= (1f + 7.5f/ 100f);
                    gameController.dnaText.text = "-" + (cost * 1.5f).ToString();
                    Invoke("ReturnDNAText", 1f);
                    dna -= (cost * 2);
                }
                break;
            case 3:

                if (dna - cost * 3 > 0)
                {
                    if (rechargeTime > 0.05f)
                    {
                        rechargeTime -= 0.05f;
                        gameController.dnaText.text = "-" + (cost * 1.5f).ToString();
                        Invoke("ReturnDNAText", 1f);
                        dna -= (cost * 3);
                    }             
                }
                break;
            case 4:

                if (dna - cost * 3 > 0)
                {
                    if (restoreTime > 0.5f)
                    {
                        restoreTime -= 0.25f;
                        gameController.dnaText.text = "-" + (cost * 1.5f).ToString();
                        Invoke("ReturnDNAText", 1f);
                        dna -= (cost * 3);
                    }
                }
                break;
            case 5:

                if (dna - cost * 5 > 0)
                {
                    if (bulletNum < 20)
                    {
                        bulletNum++;
                        gameController.dnaText.text = "-" + (cost * 1.5f).ToString();
                        Invoke("ReturnDNAText", 1f);
                        dna -= (cost * 5);
                    }
                }
                break;
            default: break;
        }
        if(dna < gameController.dna) {
            cost += 1500;
        }
       
        PlayerPrefs.SetInt("UpgradeCost", cost);
        for (int i = 0; i < players.Length; i++)
        {

            players[i].currentHealth = health;
            players[i].hpText.text = "HP: " + ((int)health).ToString();
            players[i].standardHealth = health;

            players[i].standardSpeed = speed;
            players[i].currentSpeed = speed;

            players[i].bulletRechargeTime = rechargeTime;

            players[i].restoreTime = restoreTime;

            players[i].bulletNum = bulletNum;

            PlayerPrefs.SetFloat("Health", health);
            PlayerPrefs.SetFloat("Speed", speed);
            PlayerPrefs.SetFloat("RechargeTime", rechargeTime);
            PlayerPrefs.SetFloat("RestoreTime", restoreTime);
            PlayerPrefs.SetInt("BulletNum", bulletNum);

        }
    }
    void ReturnDNAText()
    {
        gameController.dna = dna;
        PlayerPrefs.SetInt("DNA", dna);
        gameController.dnaText.text = "DNA: " + gameController.dna.ToString();
    }
}
