using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.WaveEnds==true)
        {
            ApplyPU();
            GameManager.WaveEnds=false;
        }
    }

 public void ApplyPU()
    {
        int Proll=Random.Range(1,6);
        if(Proll==1)
        {
            DMGBONUS();
        }
        else if(Proll==2)
        {
            CRITDMGPLUS();
        }
        else if(Proll==3)
        {
            CRITCHANCEPLUS();
            
        }
        else if(Proll==4)
        {
            STAMINAPLUS();
        }
        else if(Proll==5)
        {
            LOADREDUCE();
        }
        Debug.Log(Proll);

        
    }
void DMGBONUS()
    {
        PlayerController.BonusDMG+=10f;
    }
void CRITDMGPLUS()
    {
        PlayerController.CritDMG+=0.3f;
    }    
void CRITCHANCEPLUS()
    {
        PlayerController.CritChance+=0.1f;
    }
void STAMINAPLUS()
    {
        PlayerController.StaminaLimit+=100f;
    }
void LOADREDUCE()
    {
        GunManager.CurrentLoad=GunManager.CurrentLoad*0.8f;
    }       



}
