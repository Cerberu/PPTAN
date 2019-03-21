using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BackgroundScene : MonoBehaviour {

    //播放器
    public GameObject audioPlayer;

	// Use this for initialization
	void Start () {

        //内部计数器
        int i = 0;

        #if (IOS_SDK)

        //SDK开关打开
        SdkToU3d.iosSDKEnable = true;

        #endif

        //测试代码
        PlayerPrefs.DeleteAll();

        //播放器在切换场景时依旧保存，不销毁
        DontDestroyOnLoad(audioPlayer);

        //第一个角色永远处于解锁状态
        MyClass.charactorUnlockState[0] = 1;

        //角色遍历
        for (i = 1; i < MyClass.charactorUnlockState.Length; i++)
        {
            //从玩家偏好中读取该角色的解锁状态
            MyClass.charactorUnlockState[i] = PlayerPrefs.GetInt("charactorUnlockState" + i, 0);
        }

        //从玩家偏好中读取当前选中的角色索引
        MyClass.selectedPlayerIndex = PlayerPrefs.GetInt("selectedPlayerIndex", 0);

        //从玩家偏好中读取游戏加载方式
        MyClass.gameStartWay = PlayerPrefs.GetInt("gameStartWay", 0);

        //从玩家偏好中读取道具点数
        MyClass.propertyPoints = PlayerPrefs.GetInt("propertyPoints", 5000);

        //从玩家偏好中读取历史最高分
        MyClass.bestScore = PlayerPrefs.GetInt("bestScore", 0);

        //从玩家偏好中读取背景音乐的开关状态
        MyClass.musicEnable = PlayerPrefs.GetInt("musicEnable", 1);

        //从玩家偏好中读取音效的开关状态
        MyClass.soundEnable = PlayerPrefs.GetInt("soundEnable", 1);

        //从玩家偏好中读取手机振动的开关状态
        MyClass.vibrateEnable = PlayerPrefs.GetInt("vibrateEnable", 1);

        //读取玩家是否是第一次通过道具界面评论该游戏
        MyClass.firstRateFromPropertyInterface = PlayerPrefs.GetInt("firstRate", 1);

        //读取当天时间内玩家已经玩了多少次
        MyClass.playedNumerToday = PlayerPrefs.GetInt("playedNumerToday", 0);

        //读取当天是否允许弹出强制评论界面
        MyClass.popRateEnableToday = PlayerPrefs.GetInt("popRateEnableToday", 0);

        //读取是否激活新手引导
        MyClass.tutorialEnable = PlayerPrefs.GetInt("tutorialEnable", 1);

        //读取玩家上次玩游戏的时间，包括年，月，日
        MyClass.lastPlayYear = PlayerPrefs.GetInt("lastPlayYear", DateTime.Today.Year);
        MyClass.lastPlayMonth = PlayerPrefs.GetInt("lastPlayMonth", DateTime.Today.Month);
        MyClass.lastPlayDay = PlayerPrefs.GetInt("lastPlayDay", DateTime.Today.Day);

        //如果此次玩游戏的时间在上次玩游戏的时间之后，并且两者不是同一天
        if ((DateTime.Today.Year > MyClass.lastPlayYear) ||
            (DateTime.Today.Month > MyClass.lastPlayMonth) || 
            (DateTime.Today.Day > MyClass.lastPlayDay))
        {            
            //玩家当天玩游戏的次数重置
            MyClass.playedNumerToday = 0;

            //当天允许弹出强制评论界面
            MyClass.popRateEnableToday = 1;
        }

        //更新上次玩游戏的时间
        MyClass.lastPlayYear = DateTime.Today.Year;
        MyClass.lastPlayMonth = DateTime.Today.Month;
        MyClass.lastPlayDay = DateTime.Today.Day;

        //将上次玩游戏的时间存入玩家偏好中
        PlayerPrefs.SetInt("lastPlayYear", MyClass.lastPlayYear);
        PlayerPrefs.SetInt("lastPlayMonth", MyClass.lastPlayMonth);
        PlayerPrefs.SetInt("lastPlayDay", MyClass.lastPlayDay);

        //获得游戏本地化语言索引
        if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            //汉语
            MyClass.localizationLanguageIndex = 0;
        }

        else
        {
            //非汉语
            MyClass.localizationLanguageIndex = 1;
        }

        //如果全新开局
        if (MyClass.gameStartWay == 0)
        {
            //加载首页场景
            SceneManager.LoadScene("Menu");
        }

        //如果加载上次进度
        else
        {
            //直接进入游戏场景
            SceneManager.LoadScene("Game");
        }
	}
}
