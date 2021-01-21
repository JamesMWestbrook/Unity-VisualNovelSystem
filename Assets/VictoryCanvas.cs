using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using QFSW.PL;
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
    public SFXObject EXPSound;
    public SFXObject LevelUpSound;

    public AudioListener AudioListener;
    public AudioClip LevelUpClip;
    public AudioSource AudioSource;
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
    bool PauseProcessing;
    public IEnumerator Victory()
    {
        QuickScale(AftermathPanel);
        //  yield return new WaitForSeconds(0.5f);
        QuickScale(Drops);
        //    yield return new WaitForSeconds(0.7f);

        QuickScale(Currency.gameObject);
        QuickScale(CurrencyHeader);
        Currency.text = "0";
        int tempCurrency = 0;
        // yield return new WaitForSeconds(0.7f);
        do
        {
            tempCurrency++;
            Currency.text = tempCurrency.ToString();
            //Play SFX
            yield return new WaitForSeconds(0.08f);
        } while (tempCurrency != GameManager.Instance.BattleManager.MoneyPayout);

        //  yield return new WaitForSeconds(1);
        GameObject itemDrop = Instantiate<GameObject>(ItemDropPrefab, transform);
        itemDrop.GetComponent<RectTransform>().localPosition = new Vector3(-329, 106.6f, 0);
        QuickScale(itemDrop);

        //Lvl
        for (int i = 0; i < GameManager.Instance.BattleManager.Party.Count; i++)
        {
            StartCoroutine(LevelProcess(GameManager.Instance.BattleManager.Party[i]));
            if (PauseProcessing) yield return null;
        }
    }

    private IEnumerator LevelProcess(ActorSlot actor)
    {
        PerformanceLogger.StartLogger();
        PauseProcessing = true;
     
        GameManager.Instance.BattleManager.SlideActor(actor);
        int expDrain = GameManager.Instance.BattleManager.EXPPayout;
        bool Leveled = false;
        actor.Actor.EXP += expDrain;
        if (actor.Actor.EXP >= actor.Actor.EXPToLevel()) Leveled = true;
        QuickScale(LvlNumber.transform.parent.gameObject);
        QuickScale(ExpBar.gameObject);
        QuickScale(ExpBG.gameObject);
        QuickScale(LvlNumber.gameObject);
        LvlNumber.text = actor.Actor.Lvl.ToString();

        float plays = 0;
        bool StopLoop = false;
        do
        {
            plays++;
            float exp = actor.Actor.EXP;
            if (Leveled)
            {
                ExpBar.fillAmount = plays / 28;

            }
            else
            {
                float tempExp = actor.Actor.EXP;
                float Jon = tempExp / actor.Actor.EXPToLevel();
                if (plays >= Jon) StopLoop = true;
            }
            SFXManager.Main.Play(EXPSound);
            //works here
            
            yield return new WaitForSeconds(0.08f);
        } while (plays < 29 && !StopLoop);
        Debug.Log(plays);


        if (Leveled)
        {
            yield return null;
            SFXManager.Main.Play(LevelUpSound); // Not here???

            //tween in Statpanel
            QuickScale(StatPanel);
            //tween each stat base
            QuickScale(HP.gameObject);
            QuickScale(MP.gameObject);
            QuickScale(Muscle.gameObject);
            QuickScale(Vigor.gameObject);
            QuickScale(Will.gameObject);
            QuickScale(Instinct.gameObject);
            QuickScale(Agility.gameObject);
            SetStats(actor);

            yield return new WaitForSeconds(1);
            //tween in -> and newLvl 
            QuickScale(Line);
            QuickScale(NewLvl.gameObject);
            //Add lvl and run lvl function on actor
            actor.Actor.LevelUp();
            NewLvl.text = actor.Actor.Lvl.ToString();
            yield return new WaitForSeconds(1);
            //Tween in each new stat
            ShowArrow();
            SetStats(actor, true);
            yield return new WaitForSeconds(3);
            //drop skills
            //works here for some reason?????
        }
        PauseProcessing = false;



    }
    public void Test()
    {
        PerformanceLogger.LogCustomEvent("Tried to run sound");

    }
    public void ShowArrow()
    {
        QuickScale(HP.Line);
        QuickScale(MP.Line);
        QuickScale(Muscle.Line);
        QuickScale(Vigor.Line);
        QuickScale(Will.Line);
        QuickScale(Instinct.Line);
        QuickScale(Agility.Line);

        QuickScale(HP.SecondStat.gameObject);
        QuickScale(MP.SecondStat.gameObject);
        QuickScale(Muscle.SecondStat.gameObject);
        QuickScale(Vigor.SecondStat.gameObject);
        QuickScale(Will.SecondStat.gameObject);
        QuickScale(Instinct.SecondStat.gameObject);
        QuickScale(Agility.SecondStat.gameObject);
    }
    private void SetStats(ActorSlot actor, bool SecondStat = false)
    {
        SetSingleLine(HP, actor.Actor.MaxStats.HP, SecondStat);
        SetSingleLine(MP, actor.Actor.MaxStats.MP, SecondStat);
        SetSingleLine(Muscle, actor.Actor.MaxStats.Muscle, SecondStat);
        SetSingleLine(Vigor, actor.Actor.MaxStats.Vigor, SecondStat);
        SetSingleLine(Will, actor.Actor.MaxStats.Will, SecondStat);
        SetSingleLine(Instinct, actor.Actor.MaxStats.Instinct, SecondStat);
        SetSingleLine(Agility, actor.Actor.MaxStats.Agility, SecondStat);
    }
    private void SetSingleLine(Statline line, int stat, bool SecondStat = false)
    {
        if (!SecondStat)
        {
            line.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}", line.gameObject.name, stat.ToString());
        }
        else
        {
            line.SecondStat.text = stat.ToString();
        }
    }
    void QuickScale(GameObject go)
    {
        go.SetActive(true);
        go.GetComponent<RectTransform>().localScale = Vector3.zero;
        go.LeanScale(Vector3.one, 0.7f);
    }

}
