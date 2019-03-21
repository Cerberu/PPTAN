using UnityEngine;
using UnityEngine.UI;
using System;

public class PropertyInterface : MonoBehaviour {

    //道具界面的观看视频按钮
    public GameObject watchVideoButton;

    //道具界面的观看视频禁用图片
    public GameObject watchVideoDisableImage;

    //道具界面的观看视频冷却时间文本
    public ColdTime watchVideoColdTime;

    //道具界面的分享按钮
    public GameObject facebookShareButton;

    //道具界面的评论按钮
    public GameObject rateGameButton;

    //道具Logo
    public RectTransform propertyLogo;

    //显示差额道具点数的文本
    public Text differentialPropertyPointsText;

    //当道具界面启用时调用
    void OnEnable()
    {
        //更新差额道具点数
        RefreshDifferentialPropertyPoints();

        //从玩家偏好中读取上次观看视频的时间，默认时间为2016年1月1日0点0分0秒
        MyClass.lastWatchVideoYear = PlayerPrefs.GetInt("lastWatchVideoYear", 2016);
        MyClass.lastWatchVideoMonth = PlayerPrefs.GetInt("lastWatchVideoMonth", 1);
        MyClass.lastWatchVideoDay = PlayerPrefs.GetInt("lastWatchVideoDay", 1);
        MyClass.lastWatchVideoHour = PlayerPrefs.GetInt("lastWatchVideoHour", 0);
        MyClass.lastWatchVideoMinute = PlayerPrefs.GetInt("lastWatchVideoMinute", 0);
        MyClass.lastWatchVideoSecond = PlayerPrefs.GetInt("lastWatchVideoSecond", 0);

        //将上次观看视频的时间转化为DateTime格式
        DateTime tempDateTime = new DateTime(MyClass.lastWatchVideoYear,
                                             MyClass.lastWatchVideoMonth,
                                             MyClass.lastWatchVideoDay,
                                             MyClass.lastWatchVideoHour,
                                             MyClass.lastWatchVideoMinute,
                                             MyClass.lastWatchVideoSecond);

        //如果当前时间和上次时间相差大于等于观看视频的冷却时间
        if ((DateTime.Now - tempDateTime).TotalSeconds >= MyClass.watchVideoColdTime)
        {
            //观看视频按钮激活
            watchVideoButton.SetActive(true);

            //观看视频禁用图片禁用
            watchVideoDisableImage.SetActive(false);
        }

        //否则
        else
        {
            //观看视频按钮禁用
            watchVideoButton.SetActive(false);

            //观看视频禁用图片激活
            watchVideoDisableImage.SetActive(true);

            //初始冷却时间
            watchVideoColdTime.startColdTime = MyClass.watchVideoColdTime - (float)(DateTime.Now - tempDateTime).TotalSeconds;

            //开始倒计时
            watchVideoColdTime.TimeCountDown();
        }
    }

    //方法，执行道具界面入场动画结束之后的操作
    public void PropertyInterfaceStartCompleted()
    {
        //道具界面响应点击
        MenuController.Instance.propertyInterfaceClickEnable = true;
    }

    //方法，执行道具界面离场动画结束之后的操作
    public void PropertyInterfaceEndCompleted()
    {
        //道具界面暂时禁用
        gameObject.SetActive(false);
    }

    //方法，更新差额道具点数
    public void RefreshDifferentialPropertyPoints()
    {
        //差额点数
        int differentialPoints = MyClass.propertyPoints - MyClass.charactorCost[MenuController.Instance.centerPlayerIndex];

        //显示差额点数
        differentialPropertyPointsText.text = differentialPoints.ToString();

        //获得差额道具点数文本的本地位置
        Vector3 tempLocalPosition = propertyLogo.anchoredPosition3D;

        //如果差额(-10,0)
        if(differentialPoints > -10 && differentialPoints < 0)
        {
            //调整本地位置
            tempLocalPosition.x = 35;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额(-100,-10]
        if (differentialPoints > -100 && differentialPoints <= -10)
        {
            //调整本地位置
            tempLocalPosition.x = 50;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额(-1000,-100]
        if (differentialPoints > -1000 && differentialPoints <= -100)
        {
            //调整本地位置
            tempLocalPosition.x = 65;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额(-10000,-1000]
        if (differentialPoints > -10000 && differentialPoints <= -1000)
        {
            //调整本地位置
            tempLocalPosition.x = 80;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额[0,10)
        if (differentialPoints >= 0 && differentialPoints < 10)
        {
            //调整本地位置
            tempLocalPosition.x = 30;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额[10,100)
        if (differentialPoints >= 10 && differentialPoints < 100)
        {
            //调整本地位置
            tempLocalPosition.x = 45;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额[100,1000)
        if (differentialPoints >= 100 && differentialPoints < 1000)
        {
            //调整本地位置
            tempLocalPosition.x = 60;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }

        //如果差额[1000,10000)
        if (differentialPoints >= 1000 && differentialPoints < 10000)
        {
            //调整本地位置
            tempLocalPosition.x = 75;
            propertyLogo.anchoredPosition3D = tempLocalPosition;
        }
    }
}
