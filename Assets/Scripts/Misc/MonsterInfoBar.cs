using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoBar : MonoBehaviour
{
    public Slider healthSlider;
    Transform targetMonsterPos;
    Paramater targetParam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetMonsterPos && targetParam != null)
        {
            Debug.LogWarning("isUpdate");
            healthSlider.value = targetParam.health;

            Vector3 worldInfoBarPos = new Vector3(targetMonsterPos.position.x, targetMonsterPos.position.y + 1.5f, targetMonsterPos.transform.position.z);

            gameObject.transform.position = Camera.main.WorldToScreenPoint(worldInfoBarPos);
        }
    }

    public void InitMonsterInfoBar(Transform monsterPos, Paramater paramater)
    {
        Debug.LogWarning("isInit");
        targetParam = paramater;
        targetMonsterPos = monsterPos;
        healthSlider.maxValue = targetParam.health;
        healthSlider.value = targetParam.health;
    }

}


