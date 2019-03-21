using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class SdkToU3d {

	[DllImport( "__Internal" )]
	//展示插屏广告，在结算界面初始化时调用
	private static extern void showChartboostInterstitial();

    [DllImport("__Internal")]
    //展示Unity视频广告，在结算界面初始化时调用
    private static extern void showUnityAds();

    [DllImport("__Internal")]
    //展示插页广告，在结算界面初始化时调用
    private static extern void showAppBadgeADs();

    [DllImport("__Internal")]
    //展示广告，在结算界面初始化时调用
    private static extern void showGameEndADs();

	[DllImport( "__Internal" )]
	//评论按钮
	private static extern void commentBtn();

	[DllImport( "__Internal" )]
	//打开排行版
	private static extern void openGameCenter();

	[DllImport( "__Internal" )]
	//截图，在游戏结束时候，结算页面显示之前调用
	private static extern void screenShot();

	[DllImport( "__Internal" )]
	//结算页面上分享按钮
	private static extern void share();
	
	[DllImport( "__Internal" )]
	//去广告
    private static extern void buy(string productName);

    [DllImport( "__Internal" )]
	//恢复购买
	private static extern void restore();

	[DllImport( "__Internal" )] 
	//上传最高分
	private static extern void reportScore(int A);

    [DllImport("__Internal")]
    //上传成就
    private static extern void reportAchievement(string identifier, float percentComplete);

    [DllImport("__Internal")]
    //上传最高分，按照不同模式
    private static extern void reportScoreByGameMode(int modeIndex, int score);

    [DllImport("__Internal")]
    //是否弹出带评论按钮的道具界面
    private static extern bool getIfHaveACommentButton();

	[DllImport( "__Internal" )]
	//是否评论过
	private static extern bool getIfRate();

	[DllImport( "__Internal" )]
	//特殊评论
	private static extern void popRate();

	[DllImport( "__Internal" )]
	//联系我们
	private static extern void sendEmail();

	[DllImport( "__Internal" )]
	//更多游戏
	private static extern void moreGames();

    [DllImport("__Internal")]
    //跟踪页面，开始
    private static extern void trackPageBeginWithName(string Name);

    [DllImport("__Internal")]
    //跟踪页面，结束
    private static extern void trackPageEndWithName(string Name);

    [DllImport("__Internal")]
    //跟踪按钮使用
    private static extern void trackButtonActionWithName(string Name);

    [DllImport("__Internal")]
    //微信分享
    private static extern void MobShare();

    [DllImport("__Internal")]
    //微信分享
    private static extern void MobShareWithScore(int Score);

    [DllImport("__Internal")]
    //登录FackBook Share
    private static extern void facebookShare();

    [DllImport("__Internal")]
    //FackBook 邀请好友
    private static extern void facebookInvatefriends();

    [DllImport("__Internal")]
    //录制视频开始
    private static extern void recordButtonStart();

    [DllImport("__Internal")]
    //录制视频结束
    private static extern void recordButtonStop();

    [DllImport("__Internal")]
    //调用录制视屏界面
    private static extern void recordButtonPlay();

    [DllImport("__Internal")]
    //当前是否在录制
    private static extern bool ifIsRecording();

    [DllImport("__Internal")]
    //手机振动
    private static extern void vibrate();

    //bool值，指示SDK的相关功能是否打开，true表示打开，false表示关闭，默认关闭
    public static bool iosSDKEnable = false;

	//展示插屏广告，在结算界面初始化时调用
	public static void ShowChartboostInterstitial()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            showChartboostInterstitial();
        }
	}

    //展示Unity视频广告，在结算界面初始化时调用
    public static void ShowUnityAds()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            showUnityAds();
        }
    }

    //展示插页广告，在结算界面初始化时调用
    public static void ShowAppBadgeADs()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            showAppBadgeADs();
        }
    }

    //展示广告，在结算界面初始化时调用
    public static void ShowGameEndADs()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            showGameEndADs();
        }
    }

	//评论
	public static void Comment()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            commentBtn();
        }
	}

	//排行版
	public static void OpenGameCenter()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            openGameCenter();
        }
	}

	//截图，在游戏结束时候，结算页面显示之前调用
	public static void ScreenShot()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            screenShot();
        }
	}

	//分享按钮
	public static void Share()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            share();
        }
	}

	//去广告
	public static void Buy(string productName)
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            buy(productName);
        }
	}

	//恢复购买
	public static void Restore()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            restore();
        }
	}

    //上传最高分
    public static void ReportScore(int A)
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            reportScore(A);
        }
	}

    //上传成就
    public static void ReportAchievement(string identifier, float percentComplete)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            reportAchievement(identifier, percentComplete);
        }
    }

    //按照不同模式上传最高分
    public static void ReportScoreByGameMode(int modeIndex, int score)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            reportScoreByGameMode(modeIndex, score);
        }
    }

    //是否弹出带评论按钮的道具界面
    public static bool GetIfHaveACommentButton()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            return getIfHaveACommentButton();
        }

        //如果SDK功能没有打开
        else
        {
            return false;
        }
    }

	//玩家是否评论过
	public static bool GetIfRate()
	{
		return getIfRate();
	}

	//特殊评论
	public static void PopRate()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            popRate();
        }
	}

	//联系我们
	public static void SendEmail()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            sendEmail();
        }
	}

	//更多游戏
	public static void MoreGames()
	{
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            moreGames();
        }
	}

    //跟踪页面，开始
    public static void TrackPageBeginWithName(string Name)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            trackPageBeginWithName(Name);
        }
    }

    //跟踪页面，结束
    public static void TrackPageEndWithName(string Name)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            trackPageEndWithName(Name);
        }
    }

    //跟踪按钮使用
    public static void TrackButtonActionWithName(string Name)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            trackButtonActionWithName(Name);
        }
    }

    //微信分享
    public static void WeChatShare()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            MobShare();
        }
    }

    //微信分享
    public static void WeChatShareWithScore(int Score)
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            MobShareWithScore(Score);
        }
    }

    //FaceBook分享
    public static void FacebookShare()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            facebookShare();
        }
    }

    //FaceBook邀请好友
    public static void FacebookInviteFriends()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            facebookInvatefriends();
        }
    }

    //录制视频开始
    public static void RecordButtonStart()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            recordButtonStart();
        }
    }

    //录制视频结束
    public static void RecordButtonStop()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            recordButtonStop();
        }
    }

    //调用录制视屏界面
    public static void RecordButtonPlay()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            recordButtonPlay();
        }
    }

    //当前是否在录制
    public static bool IfIsRecording()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            return ifIsRecording();
        }

        //否则
        else
        {
            return false;
        }
    }

    //手机振动
    public static void MobileVibrate()
    {
        //如果SDK功能打开
        if (iosSDKEnable)
        {
            vibrate();
        }
    }
}
