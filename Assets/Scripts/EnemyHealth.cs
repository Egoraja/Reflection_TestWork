using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "«апас здоровь€" реализован через массив int, св€занный с размером массива изображений его здоровь€
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image[] healthImage;
    [SerializeField] private PlayerState playerState;
    private int[] healthVolume;
    private int currentIndex;
    private bool isAlive;

    private void Start()
    {
        isAlive = true;
        healthVolume = new int[healthImage.Length];
        for (int i = 0; i < healthVolume.Length; i++)
        {
            healthVolume[i] = 1;
        }
        currentIndex = healthVolume.Length - 1;
        CheckIsAlive();        
    }

    public bool IsAlive
    { get { return isAlive; } }

   
    public void GotDamage()
    {
        playerState.HealthUp();
        healthImage[currentIndex].fillAmount = 0;
        healthVolume[currentIndex] = 0;
        if (currentIndex != 0)
        {
            currentIndex -= 1;
        }
        CheckIsAlive();
    }    

    private void CheckIsAlive()
    {
        if (healthVolume[0] > 0)
            isAlive = true;
        else isAlive = false;
    }
}
