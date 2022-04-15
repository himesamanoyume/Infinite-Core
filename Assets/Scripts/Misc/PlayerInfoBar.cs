using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBar : MonoBehaviour
{
    public Text playerName;
    public Slider healthSlider;
    public Slider shieldSlider;

    [SerializeField]
    int m_actorNumber;

    float m_maxHealth;
    float m_currentHealth;
    float m_maxShield;
    float m_currentShield;

    GameObject m_recorder = null;
    GameObject m_playerModel = null;
    CharBase charBase = null;
    Camera m_camera = null;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_recorder && charBase)
        {
            playerName.text = charBase.PlayerName;
            healthSlider.maxValue = charBase.MaxHealth;
            healthSlider.value = charBase.CurrentHealth;

            if (charBase.Shield > m_maxShield)
            {
                m_maxShield = charBase.Shield;
            }
            shieldSlider.value = charBase.Shield;
            if (shieldSlider.value == 0)
            {
                m_maxShield = 0;
            }

            gameObject.transform.position = new Vector3(m_playerModel.transform.position.x, m_playerModel.transform.position.y + 2.5f, m_playerModel.transform.position.z);

            gameObject.transform.LookAt(m_camera.transform);
        }
    }


    public void InitPlayerInfoBar(GameObject recorder, GameObject playerModel)
    {
        m_recorder = recorder;
        charBase = m_recorder.GetComponent<CharBase>();
        m_playerModel = playerModel;
        m_actorNumber = charBase.ActorNumber;
        playerName.text = charBase.PlayerName;
        healthSlider.maxValue = charBase.MaxHealth;
        healthSlider.value = charBase.CurrentHealth;
        shieldSlider.maxValue = charBase.Shield;
        shieldSlider.value = charBase.Shield;
    }
}
