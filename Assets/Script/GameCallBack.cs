using UnityEngine;

public class GameCallBack : MonoBehaviour {

    //方法，供IOS调用，玩家成功观看视频后，获得道具点数作为奖励
    public void GetPropertyPointsByVideo(string scence)
    {
        //如果看视频送点数的奖励激活
        if (MyClass.videoRewardEnable == 1)
        {
            //如果奖励类型为0
            if (MyClass.videoRewardType == 0)
            {
                //玩家获得道具点
                MyClass.propertyPoints += 4;

                //刷新道具点
                GameController.Instance.RefreshPropertyPoints();
            }

            //如果奖励类型为1
            if(MyClass.videoRewardType == 1)
            {
                //复活界面不允许点击
                GameController.Instance.resurgenceInterfaceClickEnable = false;

                //复活界面立即禁用
                GameController.Instance.resurgenceInterface.SetActive(false);

                //游戏复活
                GameController.Instance.GameResurgence();
            }
        }
    }

    ////方法，供IOS调用，玩家观看插页广告之后，获得道具点数作为奖励
    //public void GetPropertyPointsByChartboost(string scene)
    //{
    //    //如果观看插页送点数的奖励激活
    //    if(MyClass.chartboostRewardEnble == 1)
    //    {
    //        //玩家获得道具点
    //        MyClass.propertyPoints += 2;

    //        //刷新道具点
    //        GameController.Instance.RefreshPropertyPoints();
    //    }
    //}

    //方法，供IOS调用，玩家Facebook成功分享后，获得道具点数作为奖励
    public void GetPropertyPointsByFacebookShare(string scence)
    {
        //如果facebook分享送道具点的奖励激活
        if (MyClass.facebookShareRewardEnable == 1)
        {
            //玩家获得道具点
            MyClass.propertyPoints += Random.Range(1, 6);

            //刷新道具点
            GameController.Instance.RefreshPropertyPoints();
        }
    }

    //方法，供IOS调用，玩家微信成功分享后，获得道具点数作为奖励
    public void GetPropertyPointsByWeChatShare(string scence)
    {
        //如果微信分享送道具点的奖励激活
        if (MyClass.weChatShareRewardEnable == 1)
        {
            //玩家获得道具点
            MyClass.propertyPoints += Random.Range(1, 6);

            //刷新道具点
            GameController.Instance.RefreshPropertyPoints();
        }
    }

    //方法，通过1美元内购获得60个道具点
    public void GetPropertyPointsByIAPOneDollar()
    {
        //玩家获得道具点
        MyClass.propertyPoints += 60;

        //刷新道具点
        GameController.Instance.RefreshPropertyPoints();
    }

    //方法，通过5美元内购获得360个道具点
    public void GetPropertyPointsByIAPFiveDollar()
    {
        //玩家获得道具点
        MyClass.propertyPoints += 360;

        //刷新道具点
        GameController.Instance.RefreshPropertyPoints();
    }
}
