using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// "����� �������� ������" ���������� ����� ������ int, ��������� � �������� ������� ����������� ��� ��������.
/// ������� ������ ����� �������� - ������ 0 ������� = 0
/// </summary>
public class PlayerState : MonoBehaviour
{
    [SerializeField] private Image[] healthImage;
    [SerializeField] private float actionTime;
    [SerializeField] private InfoUI infoUI;    
    [SerializeField] private LevelController levelController;

    private Animator playerAnimation;

    private bool isAlive;    
    private bool isAttacking;       // "�����������" ���������
    private int[] healthVolume;
    private int currentIndex;
    private float timer = 0;


    private void Start()
    {
        playerAnimation = GetComponent<Animator>();
        isAlive = true;
        isAttacking = false;
        healthVolume = new int[healthImage.Length];
        for (int i = 0; i < healthVolume.Length; i++)
        {
            healthVolume[i] = 1; // ������������� ����� ��������
        }
        currentIndex = healthVolume.Length - 1;
        CheckIsAlive();      
    }

    public bool IsAlive
    { get { return isAlive; } }

    public bool IsAttacking
    { get { return isAttacking; } }

    /// <summary>
    /// ��������� ����� ��������� ������ "������������" ���������
    /// �������� UI � ������� ������� � ������� ������� ����������� ��� ������������ �������
    /// </summary>
    public void GotDamage()
    {
        if (isAttacking == false)
        {
            infoUI.StartErrasingProcess(actionTime, healthImage[currentIndex]);
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            DecliningHealthProcess(true);
        }
    }

    public void HealthUp()
    {
        isAttacking = false;
        infoUI.StopErrasingProcess();
        timer = 0;
    }

    /// <summary>
    /// ������� �������� ��������
    /// </summary>
    /// <param name="doubleDamage">�������� ����� ���?</param>
    private void DecliningHealthProcess(bool doubleDamage)
    {
        if (doubleDamage == false)
        {                 
            timer += Time.deltaTime;
            if (timer >= actionTime)
            {
                healthVolume[currentIndex] = 0;
                if (currentIndex != 0)                
                    currentIndex -= 1;
                
                CheckIsAlive();
                isAttacking = false;
                timer = 0;
            }
        }
        else
        {
            infoUI.StopErrasingProcess();
            isAttacking = false;
            infoUI.ErraseUI(healthImage[currentIndex]);
            healthVolume[currentIndex] = 0;
            if (currentIndex != 0)
            {
                currentIndex -= 1;
                infoUI.ErraseUI(healthImage[currentIndex]);
                healthVolume[currentIndex] = 0;
                if (currentIndex != 0)
                    currentIndex -= 1;
            }            
            CheckIsAlive();            
            timer = 0;
        }        
    }    

    private void CheckIsAlive()
    {
        if (healthVolume[0] > 0)
        { 
            isAlive = true;
        }
        else
        {
            isAlive = false;
            levelController.GameOverEvent();
            playerAnimation.SetBool("IsAlive", false);            
        }              
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("DeathArea"))
        {
            foreach (Image item in healthImage)
            {
                item.fillAmount = 0;
            }
            isAlive = false;
            levelController.GameOverEvent();
            playerAnimation.SetBool("IsAlive", false);
        }
    }    

    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
            DecliningHealthProcess(false);            
        }                
    }
}
