using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoosts : MonoBehaviour
{
    public PlayerController pController;
    public PlayerShooting pShooting;

    public float pistolSpeedBoost = 2f;
    private float pistolSpeedCopy;
    public int shotgunBulletNoBoost = 2;
    private int shotgunBulletsCopy;
    public float grenadeCouldownBoost = 1f;
    private float grenadeCouldownCopy;

    public float BoostCounter = 5f;
    private float actualBoostCounter;
    public enum BoostType { PISTOL, SHOTGUN, GRENADE, NOTHING};
    public BoostType Boost;
    public Image BoostImage;
    public Sprite pistol, shotgun, grenade, nothing;
    // Start is called before the first frame update
    void Start()
    {
        actualBoostCounter = BoostCounter;
        grenadeCouldownCopy = pShooting.grenadeCooldown;
        pistolSpeedCopy = pShooting.pistolCooldown;
        shotgunBulletsCopy = pShooting.shotgunBulletsNo;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualBoostCounter > 0f)
        {
            Color tempColor = BoostImage.color;
            tempColor.a = (actualBoostCounter / BoostCounter);
            BoostImage.color = tempColor;

            actualBoostCounter -= Time.deltaTime;
            if (actualBoostCounter <= 0f)
            {
                EndBoost();
            }
        }
        

    }
    public void StartBoost()
    {
        actualBoostCounter = BoostCounter;
        switch (Boost)
        {
            case BoostType.PISTOL:
                pShooting.pistolCooldown /= pistolSpeedBoost;
                BoostImage.sprite = pistol;
                break;
            case BoostType.SHOTGUN:
                pShooting.shotgunBulletsNo *= shotgunBulletNoBoost;
                BoostImage.sprite = shotgun;
                break;
            case BoostType.GRENADE:
                pShooting.grenadeCooldown = grenadeCouldownBoost;
                pShooting.grenadeActualCooldown = 0f;
                BoostImage.sprite = grenade;
                break;
            default:
                break;

        }
    }
    public void EndBoost()
    {
        switch (Boost)
        {
            case BoostType.PISTOL:
                pShooting.pistolCooldown = pistolSpeedCopy;
                break;
            case BoostType.SHOTGUN:
                pShooting.shotgunBulletsNo = shotgunBulletsCopy;
                break;
            case BoostType.GRENADE:
                pShooting.grenadeCooldown = grenadeCouldownCopy;
                break;
            default:
                break;

        }
        Boost = BoostType.NOTHING;
        BoostImage.sprite = nothing;
    }
}
