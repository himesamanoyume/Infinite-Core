using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MonoBehaviour
{
    public Text playerName;
    public Slider healthSlider;
    public Slider shieldSlider;
        
    public int actorNumber;

    float m_maxShield;
    bool isInit = false;

    GameObject m_recorder = null;
    GameObject m_playerModel = null;
    CharBase charBase = null;
    Camera m_camera = null;

    public Image fill;

    Color redHp = new Color(1, 0, 0.2f, 1);
    Color blueHp = new Color(0.21f, 0.38f, 0.62f, 1);

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_recorder && charBase)
        {
            playerName.text = charBase.PlayerName;

            healthSlider.maxValue = charBase.MaxHealth;
            healthSlider.value = charBase.CurrentHealth;

            //Debug.LogWarning(charBase.CurrentHealth);

            if (charBase.Shield > m_maxShield)
            {
                m_maxShield = charBase.Shield;
            }
            shieldSlider.value = charBase.Shield;
            if (shieldSlider.value == 0)
            {
                m_maxShield = 0;
            }

            if (m_playerModel)
            {
                Vector3 worldInfoBarPos = new Vector3(m_playerModel.transform.position.x, m_playerModel.transform.position.y + 2.5f, m_playerModel.transform.position.z);

                gameObject.transform.position = m_camera.WorldToScreenPoint(worldInfoBarPos);
            }

            if (charBase.CurrentHealth == 0 && isInit)
            {
                Destroy(gameObject);
            }
        }
    }


    public void InitPlayerInfoBar(GameObject recorder, GameObject playerModel)
    {
        m_recorder = recorder;
        charBase = m_recorder.GetComponent<CharBase>();
        m_playerModel = playerModel;
        actorNumber = charBase.ActorNumber;
        playerName.text = charBase.PlayerName;
        healthSlider.maxValue = charBase.MaxHealth;
        healthSlider.value = charBase.CurrentHealth;
        shieldSlider.maxValue = charBase.Shield;
        shieldSlider.value = charBase.Shield;
        if (charBase.PlayerTeam == TeamEnum.Red)
        {
            fill.color = redHp;
        }
        else if (charBase.PlayerTeam == TeamEnum.Blue)
        {
            fill.color = blueHp;
        }
        isInit = true;
    }
}
