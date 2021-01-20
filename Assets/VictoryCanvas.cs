using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class VictoryCanvas : MonoBehaviour
{
    [Header("Drops")]
    public GameObject AftermathPanel;
    public GameObject Drops;
    public TextMeshProUGUI Currency;
    public GameObject CurrencyHeader;

    [Header("LevelUp")]
    public TextMeshProUGUI LvlNumber;
    public GameObject Line;
    public TextMeshProUGUI NewLvl;
    public Image ExpBar;
    public GameObject ExpBG;
    public GameObject StatPanel;
    public Statline HP;
    public Statline MP;
    public Statline Muscle;
    public Statline Vigor;
    public Statline Will;
    public Statline Instinct;
    public Statline Agility;
    [Header("Prefabs")]
    public GameObject ItemDropPrefab;
    public GameObject skillDropPrefab;

    void Start()
    {
        Drops.SetActive(false);
        Currency.gameObject.SetActive(false);
        CurrencyHeader.gameObject.SetActive(false);

        LvlNumber.transform.parent.gameObject.SetActive(false);
        LvlNumber.gameObject.SetActive(false);
        NewLvl.gameObject.SetActive(false);
        Line.SetActive(false);
        ExpBar.fillAmount = 0;
        ExpBar.gameObject.SetActive(false);
        ExpBG.gameObject.SetActive(false);

        DisableLine(HP);
        DisableLine(MP);
        DisableLine(Muscle);
        DisableLine(Vigor);
        DisableLine(Will);
        DisableLine(Instinct);
        DisableLine(Agility);
        StatPanel.SetActive(false);
        AftermathPanel.SetActive(false);
    }
    public void DisableLine(Statline statline)
    {
        statline.Line.gameObject.SetActive(false);
        statline.SecondStat.gameObject.SetActive(false);
        statline.gameObject.SetActive(false);
    }
    public IEnumerator Victory()
    {
        QuickScale(AftermathPanel);
        yield return new WaitForSeconds(0.5f);
        QuickScale(Drops);
        yield return new WaitForSeconds(0.7f);

        QuickScale(Currency.gameObject);
        QuickScale(CurrencyHeader);
        Currency.text = "0";
        int tempCurrency = 0;        
        do{
            tempCurrency++;
            Currency.text = tempCurrency.ToString();
            //Play SFX
            yield return new WaitForSeconds(0.01f);
        }while(tempCurrency != GameManager.Instance.BattleManager.MoneyPayout);
        yield return new WaitForSeconds(1);

    }
    void QuickScale(GameObject go){
        go.SetActive(true);
        go.GetComponent<RectTransform>().localScale = Vector3.zero;
        go.LeanScale(Vector3.one, 0.7f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
