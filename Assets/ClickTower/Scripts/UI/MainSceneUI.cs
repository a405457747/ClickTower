using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using xmaolol.com;

public class MainSceneUI : MonoSingleton<MainSceneUI>
{
    public Conent conent;
    public Slider musicSlider;
    public Slider audioSlider;
    public GameObject MainPanel;
    public GameObject SelectPanel;
    public GameObject SpoilsOfWarPanel;
    public GameObject OptPanel;
    public GameObject DetailPanel;

    public void QuitGame()
    {
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
        Application.Quit();
    }

    void OnEnable()
    {
        this.Delay(2.3f, () =>
        {
            MyYomobManager.Instance.PlayBannerAD();
        });
    }

    void OnDisable()
    {
        MyYomobManager.Instance.CloseBannerAD();
    }

    public void OpenSelect()
    {
        SelectPanel.SetActive(true);
        MainPanel.SetActive(false);
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    public void CloseSelect()
    {
        SelectPanel.SetActive(false);
        MainPanel.SetActive(true);
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }

    public void OpenSpoilsOfWaf()
    {
        SpoilsOfWarPanel.SetActive(true);
        MainPanel.SetActive(false);
        GoodsItemParent.Instance.CreateObj();
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    public void CloseSpoilsOfWaf()
    {
        SpoilsOfWarPanel.SetActive(false);
        MainPanel.SetActive(true);
        GoodsItemParent.Instance.DestoryAllChildren();
        DemageManager.Instance.UpdateHaveBoolFixed();
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }

    public void OpenOpt()
    {
        OptPanel.SetActive(true);
        MainPanel.SetActive(false);
        musicSlider.value = MyAudioManager.GetInstance().musicVolume;
        audioSlider.value = MyAudioManager.GetInstance().soundVolume;
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    public void CloseOpt()
    {
        OptPanel.SetActive(false);
        MainPanel.SetActive(true);
        MySaveManager.Instance.SaveMapping.musicVolume = (double)musicSlider.value;
        MySaveManager.Instance.SaveMapping.soundVolume = (double)audioSlider.value;
        MySaveManager.Instance.Save();
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }

     public void OpenDetailPanel()
    {
        DetailPanel.SetActive(true);
        MainPanel.SetActive(false);
        MyAudioManager.GetInstance().PlaySound(Consts.enterEffect);
    }

    public void CloseDetailPanel()
    {
        DetailPanel.SetActive(false);
        MainPanel.SetActive(true);
        MyAudioManager.GetInstance().PlaySound(Consts.backEffect);
    }

    public void MusicMute(float f)
    {
      MyAudioManager.GetInstance().musicVolume = musicSlider.value;
    }

    public void AudioMute(float f)
    {
        MyAudioManager.GetInstance().soundVolume = audioSlider.value;
    }
}
