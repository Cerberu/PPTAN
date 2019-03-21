using UnityEngine;

public class MenuCallBack : MonoBehaviour {

    //方法，供IOS调用，玩家成功观看视频后，获得道具点数作为奖励
    public void GetPropertyPointsByVideo(string scence)
    {
        //如果看视频送点数的奖励激活
        if (MyClass.videoRewardEnable == 1)
        {
            //如果奖励类型为0
            if (MyClass.videoRewardType == 0)
            {
                //金币的类型
                MenuController.Instance.flyingCoinType = 0;

                //金币的起始位置
                MenuController.Instance.flyingCoinStartPosition = MenuController.Instance.propertyInterface.GetComponent<PropertyInterface>().watchVideoButton.GetComponent<RectTransform>().anchoredPosition3D;

                //金币的终止位置
                MenuController.Instance.flyingCoinEndPosition = MenuController.Instance.charactorInterface.GetComponent<CharactorInterface>().propertyLogo.GetComponent<RectTransform>().anchoredPosition3D;

                //金币数清0
                MenuController.Instance.flyingCoinNumber = 0;

                //创建多个金币
                MenuController.Instance.CreatMultiFlyingCoin();

                //道具界面不再响应点击
                MenuController.Instance.propertyInterfaceClickEnable = false;

                //播放道具界面离场动画
                MenuController.Instance.propertyInterface.GetComponent<Animator>().SetTrigger("PropertyInterfaceOut");
            }
        }
    }

    //方法，供IOS调用，玩家Facebook成功分享后，获得道具点数作为奖励
    public void GetPropertyPointsByFacebookShare(string scence)
    {
        //如果facebook分享送道具点的奖励激活
        if (MyClass.facebookShareRewardEnable == 1)
        {
            //金币的类型
            MenuController.Instance.flyingCoinType = 1;

            //金币的起始位置
            MenuController.Instance.flyingCoinStartPosition = MenuController.Instance.propertyInterface.GetComponent<PropertyInterface>().facebookShareButton.GetComponent<RectTransform>().anchoredPosition3D;

            //金币的终止位置
            MenuController.Instance.flyingCoinEndPosition = MenuController.Instance.charactorInterface.GetComponent<CharactorInterface>().propertyLogo.GetComponent<RectTransform>().anchoredPosition3D;

            //金币数清0
            MenuController.Instance.flyingCoinNumber = 0;

            //创建多个金币
            MenuController.Instance.CreatMultiFlyingCoin();

            //道具界面不再响应点击
            MenuController.Instance.propertyInterfaceClickEnable = false;

            //播放道具界面离场动画
            MenuController.Instance.propertyInterface.GetComponent<Animator>().SetTrigger("PropertyInterfaceOut");
        }
    }

    //方法，供IOS调用，玩家微信成功分享后，获得道具点数作为奖励
    public void GetPropertyPointsByWeChatShare(string scence)
    {
        //如果微信分享送道具点的奖励激活
        if (MyClass.weChatShareRewardEnable == 1)
        {
            //金币的类型
            MenuController.Instance.flyingCoinType = 1;

            //金币的起始位置
            MenuController.Instance.flyingCoinStartPosition = MenuController.Instance.propertyInterface.GetComponent<PropertyInterface>().facebookShareButton.GetComponent<RectTransform>().anchoredPosition3D;

            //金币的终止位置
            MenuController.Instance.flyingCoinEndPosition = MenuController.Instance.charactorInterface.GetComponent<CharactorInterface>().propertyLogo.GetComponent<RectTransform>().anchoredPosition3D;

            //金币数清0
            MenuController.Instance.flyingCoinNumber = 0;

            //创建多个金币
            MenuController.Instance.CreatMultiFlyingCoin();

            //道具界面不再响应点击
            MenuController.Instance.propertyInterfaceClickEnable = false;

            //播放道具界面离场动画
            MenuController.Instance.propertyInterface.GetComponent<Animator>().SetTrigger("PropertyInterfaceOut");
        }
    }

    //方法，通过1美元内购获得60个道具点
    public void GetPropertyPointsByIAPOneDollar()
    {
        //玩家获得道具点
        MyClass.propertyPoints += 60;

        //刷新道具点
        MenuController.Instance.RefreshPropertyPoints();
    }

    //方法，通过5美元内购获得360个道具点
    public void GetPropertyPointsByIAPFiveDollar()
    {
        //玩家获得道具点
        MyClass.propertyPoints += 360;

        //刷新道具点
        MenuController.Instance.RefreshPropertyPoints();
    }
}
