using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    //单例
    public static MenuController Instance;

    /****************************************公有变量****************************************/

    //显示道具点数的文本
    public Text propertyPointsText;

    //显示角色单价的文本
    public Text charactorCostText;

    //角色组
    public Image charactorGroup;

    //左右墙壁
    public RectTransform leftWall;
    public RectTransform rightWall;

    //bool值，指示首页界面是否允许点击
    public bool menuInterfaceClickEnable = false;

    //bool值，指示设置界面是否响应点击
    public bool settingInterfaceClickEnable = false;

    //bool值，角色界面是否允许点击
    public bool charactorInterfaceClickEnable = false;

    //bool值，指示道具界面是否响应点击
    public bool propertyInterfaceClickEnable = false;

    //bool值，指示成就界面是否响应点击
    public bool achieveInterfaceClickEnable = false;

    //角色选择界面显示角色名字的文本组件
    public Text nameText;

    //游戏体，首页中的玩家角色
    public Image menuPlayer;

    //金币池
    public Transform coinPool;

    //游戏体，设置界面
    public GameObject settingInterface;

    //游戏体，角色界面
    public GameObject charactorInterface;

    //游戏体，两个道具界面
    public GameObject propertyInterfaceWithRate;
    public GameObject propertyInterfaceWithoutRate;

    //游戏体，道具界面
    public GameObject propertyInterface;

    //游戏体，成就界面
    public GameObject achieveInterface;

    //游戏体，开始游戏按钮
    public GameObject playButton;

    //游戏体，购买角色按钮
    public GameObject buyCharactorButton;

    //游戏体，角色确认按钮
    public GameObject charactorOkButton;

    //整数值，当前处于菜单的第几页
    public int currentMenuPage;

    //整数值，处于屏幕正中间的玩家的索引
    public int centerPlayerIndex;

    //整数值，飞行金币的类型，0表示看视频获得金币，1表示分享获得金币，2表示评论获得金币
    public int flyingCoinType;

    //整数值，创建的飞行金币的数量
    public int flyingCoinNumber = 0;

    //vector3向量，飞行金币的起始位置
    public Vector3 flyingCoinStartPosition;

    //vector3向量，飞行金币的终止位置
    public Vector3 flyingCoinEndPosition;

    //所有角色名字文字的颜色
    public Color[] nameColor;

    /****************************************私有变量****************************************/

    //音效播放器
    AudioSource soundPlayer;

    //在start之前执行
    void Awake()
    {
        //指定目标帧率
        Application.targetFrameRate = 100;

        //不支持多点触控
        Input.multiTouchEnabled = false;

        //获得单例自身
        Instance = this;

        //获得音效播放器
        soundPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        //游戏状态
        MyClass.gameState = GameState.Load;

        //如果是IOS平台
        #if (IOS_SDK)

        //页面跟踪
        SdkToU3d.TrackPageBeginWithName("Menu");

        #endif

        //首页界面允许点击
        menuInterfaceClickEnable = true;

        //设置界面允许点击
        settingInterfaceClickEnable = true;

        //根据屏幕宽高比，计算左右墙壁的本地位置
        leftWall.anchoredPosition3D = new Vector3(-50 - Camera.main.aspect * 4000 / 6, 255, 0);
        rightWall.anchoredPosition3D = new Vector3(50 + Camera.main.aspect * 4000 / 6, 255, 0);

        //刷新首页中的玩家角色
        RefreshMenuPlayer();
    }

    //当该场景禁用时调用
    void OnDisable()
    {
        //如果是IOS平台
        #if (IOS_SDK)

        //页面跟踪
        SdkToU3d.TrackPageEndWithName("Menu");

        #endif
    }

    //方法，开始游戏按钮点击之后的操作
    public void PlayButtonClick()
    {
        //如果首页界面允许点击
        if (menuInterfaceClickEnable)
        {
             #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("PlayButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //首页界面不再允许点击
            menuInterfaceClickEnable = false;

            //加载游戏场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Game");
        }
    }

    //方法，角色选择按钮被点击之后的操作
    public void CharactorButtonClick()
    {
        //如果首页界面允许点击
        if (menuInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("CharactorButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //首页界面不再允许点击
            menuInterfaceClickEnable = false;

            //设置界面不再允许点击
            settingInterfaceClickEnable = false;

            //角色界面不允许点击
            charactorInterfaceClickEnable = false;

            //角色区域不允许点击
            charactorGroup.enabled = false;

            //角色界面激活
            charactorInterface.SetActive(true);
        }
    }

    //方法，执行设置按钮被点击之后的操作
    public void SettingButtonClick()
    {
        //如果首页界面允许点击
        if (menuInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("SettingButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //首页界面不再允许点击
            menuInterfaceClickEnable = false;

            //设置界面不响应点击
            settingInterfaceClickEnable = false;

            //设置界面激活
            settingInterface.SetActive(true);
        }
    }

    //方法，执行成就按钮被点击之后的操作
    public void AchievementButtonClick()
    {
        //如果首页界面允许点击
        if (menuInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("AchieveButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //首页界面不再允许点击
            menuInterfaceClickEnable = false;

            //成就界面不响应点击
            achieveInterfaceClickEnable = false;

            //成就界面激活
            achieveInterface.SetActive(true);
        }
    }

    //方法，退出角色界面按钮点击之后的操作
    public void ExitCharactorButtonClick()
    {
        //如果角色界面允许点击
        if (charactorInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //角色界面不再允许点击
            charactorInterfaceClickEnable = false;

            //角色区域不再允许滑动
            charactorGroup.enabled = false;

            //播放角色界面离场动画
            charactorInterface.GetComponent<Animator>().SetTrigger("CharactorInterfaceEnd");
        }
    }

    //方法，购买角色按钮点击之后的操作
    public void BuyCharactorButtonClick()
    {
        //如果角色界面允许点击
        if (charactorInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("BuyCharactorButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //如果玩家的道具点数足够购买对应的角色
            if(MyClass.propertyPoints >= MyClass.charactorCost[centerPlayerIndex])
            {
                //扣除相应的道具点数
                MyClass.propertyPoints -= MyClass.charactorCost[centerPlayerIndex];

                //更新道具点数
                RefreshPropertyPoints();

                //对应的角色解锁
                charactorGroup.transform.GetChild(centerPlayerIndex).GetComponent<MenuCharactor>().CharactorUnlock();
            }

            //如果玩家的道具点数不足以购买对应的角色
            else
            {
                //道具界面不允许点击
                propertyInterfaceClickEnable = false;

                //如果玩家是第一次通过道具界面评论该游戏
                if (MyClass.firstRateFromPropertyInterface == 1)
                {
                    #if (IOS_SDK)

                    //如果评论按钮开关打开
                    if (SdkToU3d.GetIfHaveACommentButton())
                    {
                        //道具界面带评论按钮
                        propertyInterface = propertyInterfaceWithRate;
                    }

                    //如果评论按钮开关关闭
                    else
                    {
                        //道具界面不带评论按钮
                        propertyInterface = propertyInterfaceWithoutRate;
                    }

                    #else

                    //道具界面带评论按钮
                    propertyInterface = propertyInterfaceWithRate;

                    #endif
                }

                //如果玩家已经通过道具界面评论过该游戏
                else
                {
                    //道具界面不带评论按钮
                    propertyInterface = propertyInterfaceWithoutRate;
                }

                //道具界面激活
                propertyInterface.SetActive(true);
            }
        }
    }

    //方法，角色确认按钮点击之后的操作
    public void CharactorOkButtonClick()
    {
        //如果角色界面允许点击
        if (charactorInterfaceClickEnable)
        {            
            //该角色即为当前选中的角色
            MyClass.selectedPlayerIndex = centerPlayerIndex;

            //将该索引存入玩家偏好中
            PlayerPrefs.SetInt("selectedPlayerIndex", MyClass.selectedPlayerIndex);

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //角色界面不再允许点击
            charactorInterfaceClickEnable = false;

            //角色区域不再允许点击
            charactorGroup.enabled = false;

            //播放角色界面离场动画
            charactorInterface.GetComponent<Animator>().SetTrigger("CharactorInterfaceEnd");

            //刷新首页中的玩家角色
            RefreshMenuPlayer();
        }       
    }

    //方法，退出设置界面
    public void ExitSettingInterface()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //设置界面不再响应点击
            settingInterfaceClickEnable = false;

            //播放设置界面离场动画
            settingInterface.GetComponent<Animator>().SetTrigger("SettingInterfaceEnd");
        }
    }

    //方法，执行评论按钮被点击之后的操作
    public void RateButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //评论
            SdkToU3d.Comment();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("RateButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，执行新手引导按钮被点击之后的操作
    public void TutorialButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("TutorialButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //设置界面不再允许点击
            settingInterfaceClickEnable = false;

            //允许引导
            MyClass.tutorialEnable = 1;

            //将该状态标志位存入玩家偏好中
            PlayerPrefs.SetInt("tutorialEnable", MyClass.tutorialEnable);

            //游戏重新开局
            MyClass.gameStartWay = 0;

            //将该状态标志位存入玩家偏好中
            PlayerPrefs.SetInt("gameStartWay", MyClass.gameStartWay);

            //加载游戏场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Game");
        }
    }

    //方法，执行联系我们按钮点击之后的操作
    public void ContactUsButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //发送Email
            SdkToU3d.SendEmail();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("ContactUsButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，执行支持我们按钮点击之后的操作
    public void SupportUsButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //看视频送点数的奖励关闭
            MyClass.videoRewardEnable = 0;

            //发送Email
            SdkToU3d.ShowGameEndADs();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("SupportUsButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，执行排行榜按钮点击之后的操作
    public void RankButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //打开GameCenter
            SdkToU3d.OpenGameCenter();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("RankButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，执行MoreGame按钮被点击之后的操作
    public void MoreGameButtonClick()
    {
        //如果设置界面响应点击
        if (settingInterfaceClickEnable)
        {
            #if(IOS_SDK)

            //评论
            SdkToU3d.MoreGames();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("MoreGameButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，执行退出成就界面按钮被点击之后的操作
    public void ExitAchieveButtonClick()
    {
        //如果成就界面响应点击
        if (achieveInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //成就界面不再响应点击
            achieveInterfaceClickEnable = false;

            //播放成就界面离场动画
            achieveInterface.GetComponent<Animator>().SetTrigger("AchieveInterfaceEnd");
        }
    }

    //方法，观看视频按钮被点击之后的操作
    public void WatchVideoButtonClick()
    {
        //如果道具界面响应点击
        if (propertyInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //看视频送点数的奖励激活
            MyClass.videoRewardEnable = 1;

            //视频奖励类型为0
            MyClass.videoRewardType = 0;

            //展示视频广告
            SdkToU3d.ShowUnityAds();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("WatchVideoButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //观看视频按钮禁用
            propertyInterface.GetComponent<PropertyInterface>().watchVideoButton.SetActive(false);

            //观看视频禁用图片激活
            propertyInterface.GetComponent<PropertyInterface>().watchVideoDisableImage.SetActive(true);

            //初始冷却时间
            propertyInterface.GetComponent<PropertyInterface>().watchVideoColdTime.startColdTime = MyClass.watchVideoColdTime;

            //开始倒计时
            propertyInterface.GetComponent<PropertyInterface>().watchVideoColdTime.TimeCountDown();

            //将此时的时间存入玩家偏好中
            PlayerPrefs.SetInt("lastWatchVideoYear", System.DateTime.Now.Year);
            PlayerPrefs.SetInt("lastWatchVideoMonth", System.DateTime.Now.Month);
            PlayerPrefs.SetInt("lastWatchVideoDay", System.DateTime.Now.Day);
            PlayerPrefs.SetInt("lastWatchVideoHour", System.DateTime.Now.Hour);
            PlayerPrefs.SetInt("lastWatchVideoMinute", System.DateTime.Now.Minute);
            PlayerPrefs.SetInt("lastWatchVideoSecond", System.DateTime.Now.Second);
        }
    }

    //方法，分享链接按钮被点击之后的操作
    public void ShareLinkButtonClick()
    {
        //如果道具界面响应点击
        if (propertyInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //如果本地化语言为中文
            if (MyClass.localizationLanguageIndex == 0)
            {
                //微信分享送道具的奖励激活
                MyClass.weChatShareRewardEnable = 1;

                #if (IOS_SDK)

                //微信分享
                SdkToU3d.WeChatShare();

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("WeChatShareButton");

                #endif
            }

            //否则
            else
            {
                //facebook分享送道具的奖励激活
                MyClass.facebookShareRewardEnable = 1;

                #if (IOS_SDK)

                //Facebook分享
                SdkToU3d.FacebookShare();

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("FacebookShareButton");

                #endif
            }
        }
    }

    //方法，道具界面的评论按钮点击之后的操作
    public void RateGameButtonClick()
    {
        //如果道具界面响应点击
        if (propertyInterfaceClickEnable)
        {
            //如果玩家是第一次通过道具界面评论该游戏
            if (MyClass.firstRateFromPropertyInterface == 1)
            {
                #if (IOS_SDK)

                //评论
                SdkToU3d.Comment();

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("RateGameButton");

                #endif

                //播放按钮音效
                MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

                //玩家获得道具点数奖励
                MyClass.propertyPoints += 100;

                //更新道具点数
                RefreshPropertyPoints();

                //更新差额道具点数
                propertyInterface.GetComponent<PropertyInterface>().RefreshDifferentialPropertyPoints();

                //状态标识位改变
                MyClass.firstRateFromPropertyInterface = 0;

                //将玩家是否是第一次通过道具界面评论该游戏的状态标志位存入玩家偏好中
                PlayerPrefs.SetInt("firstRate", MyClass.firstRateFromPropertyInterface);               

                //道具界面不再响应点击
                propertyInterfaceClickEnable = false;

                //播放道具界面离场动画
                propertyInterface.GetComponent<Animator>().SetTrigger("PropertyInterfaceOut");
            }
        }
    }

    //方法，退出道具界面
    public void ExitPropertyInterface()
    {
        //如果道具界面响应点击
        if (propertyInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //道具界面不再响应点击
            propertyInterfaceClickEnable = false;

            //播放道具界面离场动画
            propertyInterface.GetComponent<Animator>().SetTrigger("PropertyInterfaceOut");
        }
    }

    //方法，刷新道具点数
    public void RefreshPropertyPoints()
    {
        //如果道具点数大于上限
        if (MyClass.propertyPoints > MyClass.maxPropertyPoints)
        {
            //数值调为上限
            MyClass.propertyPoints = MyClass.maxPropertyPoints;
        }

        //将道具点数存入玩家偏好中
        PlayerPrefs.SetInt("propertyPoints", MyClass.propertyPoints);

        //显示道具点数
        propertyPointsText.text = MyClass.propertyPoints.ToString();
    }

    //方法，刷新首页中的玩家角色
    public void RefreshMenuPlayer()
    {
        //玩家角色的本地位置
        menuPlayer.GetComponent<RectTransform>().anchoredPosition3D = (100 * MyClass.charactorRadius[MyClass.selectedPlayerIndex] - 155) * Vector3.up;

        //玩家角色的图片
        menuPlayer.sprite = Resources.Load<Sprite>("PlayerSprites/" + MyClass.selectedPlayerIndex);

        //自适应大小
        menuPlayer.SetNativeSize();

        //设置角色碰撞范围
        menuPlayer.GetComponent<CircleCollider2D>().radius = 100 * MyClass.charactorRadius[MyClass.selectedPlayerIndex];

        //玩家角色的初始速度
        menuPlayer.GetComponent<Rigidbody2D>().velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(45 * Mathf.Deg2Rad), Mathf.Sin(45 * Mathf.Deg2Rad));

        //如果角色索引为6
        if(MyClass.selectedPlayerIndex == 6)
        {
            //显示对应的粒子效果
            menuPlayer.transform.GetChild(0).gameObject.SetActive(true);
            menuPlayer.transform.GetChild(1).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(2).gameObject.SetActive(false);
        }

        //如果角色索引为7
        else if (MyClass.selectedPlayerIndex == 7)
        {
            //显示对应的粒子效果
            menuPlayer.transform.GetChild(0).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(1).gameObject.SetActive(true);
            menuPlayer.transform.GetChild(2).gameObject.SetActive(false);
        }

        //如果角色索引为8
        else if (MyClass.selectedPlayerIndex == 8)
        {
            //显示对应的粒子效果
            menuPlayer.transform.GetChild(0).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(1).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(2).gameObject.SetActive(true);
        }

        //其他角色
        else
        {
            //不显示粒子效果
            menuPlayer.transform.GetChild(0).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(1).gameObject.SetActive(false);
            menuPlayer.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    //方法，金币增长
    public void CoinIncrease()
    {
        //金币增长的步长
        int coinIncrement;

        //如果是视频金币
        if (flyingCoinType == 0)
        {
            //金币增长步长
            coinIncrement = 5;
        }

        //分享和评论金币
        else
        {
            //金币增长步长
            coinIncrement = 10;
        }

        //金币增长
        MyClass.propertyPoints += coinIncrement;

        //更新金币
        RefreshPropertyPoints();

        //更新差额道具点数
        propertyInterface.GetComponent<PropertyInterface>().RefreshDifferentialPropertyPoints();

        //金币文本缩放
        propertyPointsText.GetComponent<Animator>().SetTrigger("PropertyPointsTextIncrease");
    }

    //方法，创建单个飞行金币
    void CreatSingleFlyingCoin()
    {
        //临时的飞行金币
        GameObject tempFlyingCoin;

        //飞行金币的脚本
        FlyingCoin targetFlyingCoin;

        //缓存池中取出一枚飞行金币
        tempFlyingCoin = CoinPool.GetCoin();

        //设定飞行金币的位置
        tempFlyingCoin.GetComponent<RectTransform>().anchoredPosition3D = flyingCoinStartPosition;

        //设定飞行金币的父物体
        tempFlyingCoin.transform.SetParent(coinPool,false);

        //获得飞行金币的脚本组件
        targetFlyingCoin = tempFlyingCoin.GetComponent<FlyingCoin>();

        //金币飞行
        targetFlyingCoin.FlyingCoinMove();

        //播放获得音效
        MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/GetCoin"), MyClass.soundEnable);
    }

    //方法，创建多个飞行金币
    public void CreatMultiFlyingCoin()
    {
        //创建单个飞行金币
        CreatSingleFlyingCoin();

        //金币数量增加
        flyingCoinNumber++;

        //如果创建的金币数量不够
        if (flyingCoinNumber < 10)
        {
            //0.1秒之后，创建一个新的金币
            Invoke("CreatMultiFlyingCoin", 0.1F);
        }
    }
}
