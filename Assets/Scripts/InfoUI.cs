using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    private bool isErrasing;
    private Image currentSprite;
    private float erraseTime;
    private float timer;

    private void Start()
    {
        isErrasing = false;
        timer = 0;
    }   

    /// <summary>
    /// Старт визуализации таймера потери здоровья
    /// </summary>
    /// <param name="time">длина таймера</param>
    /// <param name="image">изображение визуализирующее таймер</param>
    public void StartErrasingProcess(float time, Image image)
    {
        isErrasing = true;
        currentSprite = image;
        erraseTime = time;
        timer = time;
    }

    /// <summary>
    /// Стоп таймер
    /// </summary>
    public void StopErrasingProcess()
    {
        isErrasing = false;
        currentSprite.fillAmount = 1;
    }

    /// <summary>
    /// Моментальное стирание очка здоровья
    /// </summary>
    /// <param name="image"></param>
    public void ErraseUI(Image image)
    {
        image.fillAmount = 0;
        isErrasing = false;
    }


    private void ErrasingUI()
    {
        currentSprite.fillAmount = timer/erraseTime;
        
        if (currentSprite.fillAmount <= 0.001f)
        {
            isErrasing = false;
            timer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isErrasing == true)
        { 
            ErrasingUI(); 
            timer -= Time.deltaTime;
        }
        else        
            timer = 0;        
    }
}
