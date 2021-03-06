﻿using System.Collections;
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
    public SFXObject EXPSound;
    public SFXObject LevelUpSound;

    public SFXObject VictorySong;
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
    public SFXObject VictorySound;
    public IEnumerator Victory()
    {
        MusicManager.Main.Stop(1);
        yield return new WaitForSeconds(1f);
        SFXManager.Main.Play(VictorySong);
        QuickScale(AftermathPanel);
        //  yield return new WaitForSeconds(0.5f);
        QuickScale(Drops);
        //    yield return new WaitForSeconds(0.7f);
        QuickScale(Currency.gameObject);
        QuickScale(CurrencyHeader);
        Currency.text = "0";
        int tempCurrency = 0;
        yield return new WaitForSeconds(0.7f);
        SFXManager.Main.Play(VictorySound);
        GameManager.Instance.BattleManager.SlideActor(GameManager.Instance.BattleManager.Party[1]);

        do
        {
            tempCurrency++;
            Currency.text = tempCurrency.ToString();
            SFXManager.Main.Play(EXPSound);
            yield return new WaitForSeconds(0.08f);
        } while (tempCurrency != GameManager.Instance.BattleManager.MoneyPayout);
        yield return new WaitForSeconds(1.5f);

        GameObject itemDrop = Instantiate<GameObject>(ItemDropPrefab, transform);
        itemDrop.GetComponent<RectTransform>().localPosition = new Vector3(-329, 106.6f, 0);
        QuickScale(itemDrop);
        SFXManager.Main.Play(LevelUpSound);

        yield return new WaitForSeconds(0.7f);
        StartCoroutine(LevelProcess(GameManager.Instance.BattleManager.Party[0], 0));
    }

    private IEnumerator LevelProcess(ActorSlot actor, int index)
    {

        Debug.Log(index);
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
        float time = 4;
        float curTime = 0;
        if (Leveled)
        {
            LeanTween.value(ExpBar.gameObject, (float x) => ExpBar.fillAmount = x, 0, 1, 4);

        }
        else
        {
            //time not = to 4 but idfc atm and won't bother setting this up completely as things currently stand
        }

        do
        {
            curTime += Time.deltaTime;
       
            if (curTime / time < .86) SFXManager.Main.Play(EXPSound);
            yield return null;
        } while (curTime < time);
        if (Leveled)
        {
            SFXManager.Main.Play(LevelUpSound); 

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
            yield return new WaitForSeconds(2);
            //drop skills
            SkillCheck(actor);
            //wait x time
            yield return new WaitForSeconds(2 * Multiplier);
            foreach (GameObject go in skillDrops)
            {
                go.GetComponent<SkillDrop>().SelfDestruct();
            }
            skillDrops.Clear();
            Multiplier = 0;

        }
        DisableLine(HP);
        DisableLine(MP);
        DisableLine(Muscle);
        DisableLine(Vigor);
        DisableLine(Will);
        DisableLine(Instinct);
        DisableLine(Agility);
        StatPanel.SetActive(false);

        index++;
        if (index < GameManager.Instance.BattleManager.Party.Count)
        {
            StartCoroutine(LevelProcess(GameManager.Instance.BattleManager.Party[index], index));
        }
        else
        {

        }
    }

    int Multiplier;
    public List<GameObject> skillDrops = new List<GameObject>();
    public void SkillCheck(ActorSlot actor)
    {
        List<Skills> skills = actor.Actor.Skills;

        //Forloop check for skills that 1 are <= current level, but havent been marked as learned
        for (int i = 0; i < actor.Actor.Skills.Count; i++)
        {
            if (skills[i].LevelLearned <= actor.Actor.Lvl && !skills[i].Learned)
            {
                //mark as learned && summon
                skills[i].Learned = true;
                GameObject go = Instantiate<GameObject>(skillDropPrefab, gameObject.transform);
                go.transform.localPosition = new Vector3(125.86f, -142f + Multiplier * -34.56f);
                Multiplier++;
                QuickScale(go);
                skillDrops.Add(go);
                go.GetComponent<SkillDrop>().Name.text = skills[i].Name;
            }
        }

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
