using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MiscLevel
{
    Normal,
    Artifact,
    Epic,
    Strange
}

public class MiscInfo : MonoBehaviour
{
    Image m_Bg;
    //Transform miscInfoPanel;
    public Image miscLevelBg;
    public Text miscInfoText;

    Color normalColor = new Color(0.679f, 0.672f, 0.679f);
    Color artifactColor = new Color(0.886f, 0.313f, 0.886f);
    Color epicColor = new Color(1, 0.739f, 0.297f);
    Color strangeColor = new Color(1, 0, 0.270f);

    Color normalColorBg = new Color(0.679f, 0.672f, 0.679f, 0.1490196f);
    Color artifactColorBg = new Color(0.886f, 0.313f, 0.886f, 0.1490196f);
    Color epicColorBg = new Color(1, 0.739f, 0.297f, 0.1490196f);
    Color strangeColorBg = new Color(1, 0, 0.270f, 0.1490196f);


    // Start is called before the first frame update
    void Start()
    {
        //miscInfoPanel = GameObject.FindGameObjectWithTag("MiscInfoMenu").transform;
        //transform.SetParent(miscInfoPanel.transform);
        StartCoroutine(DestoryMyself());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitMiscInfo(MiscLevel miscLevel, string text)
    {
        m_Bg = GetComponent<Image>();
        
        switch (miscLevel)
        {
            case MiscLevel.Normal:
                m_Bg.color = normalColorBg;
                miscLevelBg.color = normalColor;
                break;
            case MiscLevel.Artifact:
                m_Bg.color = artifactColorBg;
                miscLevelBg.color = artifactColor;
                break;
            case MiscLevel.Epic:
                m_Bg.color = epicColorBg;
                miscLevelBg.color = epicColor;
                break;
            case MiscLevel.Strange:
                m_Bg.color = strangeColorBg;
                miscLevelBg.color = strangeColor;
                break;
            default:
                m_Bg.color = normalColorBg;
                miscLevelBg.color = normalColor;
                break;
        }
        miscInfoText.text = text;
        
    }

    IEnumerator DestoryMyself()
    {
        //Debug.LogWarning("DestoryMyself");
        yield return new WaitForSeconds(8);
        Destroy(gameObject);
    }
}
