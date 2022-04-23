using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoBar : MonoBehaviour
{
    public Slider healthSlider;
    Transform targetMonsterPos;
    MonsterController mController;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (targetMonsterPos && mController != null)
        {
            healthSlider.value = mController.currentHealth;

            Vector3 worldInfoBarPos = new Vector3(targetMonsterPos.position.x, targetMonsterPos.position.y + 1.5f, targetMonsterPos.transform.position.z);

            gameObject.transform.position = Camera.main.WorldToScreenPoint(worldInfoBarPos);
        }
    }

    public void InitMonsterInfoBar(Transform monsterPos, MonsterController monsterController)
    {
        targetMonsterPos = monsterPos;
        mController = monsterController;
        healthSlider.maxValue = mController.currentHealth;
        healthSlider.value = mController.maxHealth;
    }

}


