using UnityEngine;
using System.Collections;

public class SC_DamageReceiver : MonoBehaviour, IEntity
{
    [SerializeField] private AudioClip takeDamage;
    [SerializeField] private AudioClip playerDead;
    //This script will keep track of player HP
    public float playerHP = 100;
    public SC_CharacterController playerController;
    public SC_WeaponManager weaponManager;
    public CameraShake cameraShake;
    //public StressReceiver stressReceiver; 

    public void ApplyDamage(float points)
    {
        playerHP -= points;
        SoundManager.Instance.Play(takeDamage);
        StartCoroutine(cameraShake.Shake(.2f,.25f));
        //stressReceiver.InduceStress(1);

        if (playerHP == 0)
        {
            //Player is dead
            SoundManager.Instance.Play(playerDead);
            playerController.canMove = false;
            playerHP = 0;
            if (Score.scoreValue > PlayerPrefs.GetInt("highscore"))
            {
                PlayerPrefs.SetInt("highscore", Score.scoreValue);
            }
        }
    }
} 