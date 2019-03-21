using UnityEngine;

//游戏状态枚举,加载、玩、失败、胜利
public enum GameState { Load, Playing, Defeat, Win, Pause};

public class MyClass{

    //游戏本地化语言索引,0表示汉语，1表示英语(非汉语)，默认为英文版本
    public static int localizationLanguageIndex = 1;

    //浮点值，指示虚线点间距
    public static float indicateDashLineIncrement = 0.1F;

    //整数值，最大的指示虚线顶点数
    public static int maxDashLineVertexCount = 200;

    //整数值，棋盘的行数和列数
    public static int chessRow = 9;
    public static int chessColumn = 7;

    //浮点值，棋盘区域的宽和高
    public static float chessWidth = 7.45F;
    public static float chessHeight = 9.55F;

    //vector3向量，棋盘最左下角方块的本地位置
    public static Vector3 baseChessBlockLocalPosition = new Vector3(-315, -420, 0);

    //浮点值，相邻棋盘方块的间隔
    public static float chessBlockSpace = 105;

    //浮点值，玩家小球的发射速度幅度
    public static float playerVelocityAmplitude = 10;

    //浮点值，玩家小球的发射角度最小阈值
    public static float playerMinSendUpAngle = 15;

    //浮点值，玩家小球的发射角度最大阈值
    public static float playerMaxSendUpAngle = 165;

    //整数值，一局中允许复活的最大次数
    public static int maxResurgenceNumber = 3;

    //浮点值，记录每个角色的半径
    public static float[] charactorRadius = new float[10] {0.13F, 0.13F, 0.13F, 0.13F, 0.16F, 0.16F, 0.18F, 0.2F, 0.24F, 0.05F };

    //整数值，记录每个角色的价格
    public static int[] charactorCost = new int[10] { 200, 200, 200, 200, 500, 500, 1000, 1000, 1000, 1000};

    //字符串数组，记录每个角色的中文名字
    public static string[] charactorNameChinese = new string[10] { "火星", "海王星", "金星", "泡泡糖", "阴阳鱼", "天王星", "泡泡", "电光球", "龙珠", "超级小不点" };

    //字符串数组，记录每个角色的英文名字
    public static string[] charactorNameEnglish = new string[10] { "Mars", "Neptune", "Venus", "Bubble Gum", "Yin Yang Fish", "Uranus", "Bubble", "Lightning", "Dragon Ball", "Little Dot" };

    //整数值，记录每个角色的解锁状态
    public static int[] charactorUnlockState = new int[10];

    //整数值，记录当前选中的角色的索引
    public static int selectedPlayerIndex = 0;

    //浮点值，撞击播放器的冷却时间(以秒为单位)
    public static float ballPlayerCD = 0.05F;

    //浮点值，光带播放器的冷却时间(以秒为单位)
    public static float bandPlayerCD = 0.1F;

    //整数值，记录当前游戏的得分
    public static int score = 0;

    //整数值，记录历史最高得分
    public static int bestScore = 0;

    //玩家当前拥有的道具点数
    public static int propertyPoints = 0;

    //玩家最多能够拥有的道具点数
    public static int maxPropertyPoints = 9999;

    //游戏状态，初始时为load状态
    public static GameState gameState = GameState.Load;

    //整数数组，记录每个成就是否达成，0表示未达成，1表示已经达成
    public static int[] achievementCompletedState = new int[8];

    //整数值，指示看视频送点数的奖励是否激活，0表示未激活，1表示激活
    public static int videoRewardEnable = 0;

    //整数值，看视频获得奖励的类型，0表示道具点数奖励，1表示复活奖励
    public static int videoRewardType = 0;

    //整数值，指示观看插页送点数的奖励是否激活，0表示未激活，1表示激活
    public static int chartboostRewardEnble = 0;

    //整数值，指示Facebook分享送点数的奖励是否激活，0表示未激活，1表示激活
    public static int facebookShareRewardEnable = 0;

    //整数值，指示微信分享送点数的奖励是否激活，0表示未激活，1表示激活
    public static int weChatShareRewardEnable = 0;

    //整数值，指示是否打开背景音乐，0表示关闭，1表示打开
    public static int musicEnable = 1;

    //整数值，指示是否打开音效，0表示关闭，1表示打开
    public static int soundEnable = 1;

    //整数值，指示是否允许手机振动,0表示关闭，1表示打开
    public static int vibrateEnable = 1;

    //整数值，指示游戏的启动方式，0表示全新开局，1表示加载上次进度，默认全新开局
    public static int gameStartWay = 0;

    //整数值，指示是否激活新手引导，0表示不激活，1表示激活，默认激活
    public static int tutorialEnable = 1;

    //整数值，指示玩家是不是第一次从道具界面评论这个游戏,1表示是第一次评论，0表示不是，初始时为1
    public static int firstRateFromPropertyInterface = 1;

    //整数值，记录玩家当天时间内一共玩了多少次游戏
    public static int playedNumerToday = 0;

    //整数值，记录玩家当天时间内通过道具界面一共对游戏做了几次分享
    public static int shareNumerToday = 0;

    //整数值，记录玩家当天时间内玩过几次之后开始谈强制评论界面,暂定为3次
    public static int popRateTargetPlayedNumber = 3;

    //整数值，记录当天是否允许弹出强制评论界面，初始值为允许
    public static int popRateEnableToday = 0;

    //整数值，记录玩家上次启动游戏的时间，包括年，月，日
    public static int lastPlayYear = 0;
    public static int lastPlayMonth = 0;
    public static int lastPlayDay = 0;

    //整数值，记录玩家上次通过观看视频来获得道具点的具体时间（年、月、日、时、分、秒）
    public static int lastWatchVideoYear = 2016;
    public static int lastWatchVideoMonth = 1;
    public static int lastWatchVideoDay = 1;
    public static int lastWatchVideoHour = 0;
    public static int lastWatchVideoMinute = 0;
    public static int lastWatchVideoSecond = 0;

    //玩家通过观看视频来获得道具点的冷却时间，以秒为单位
    public static float watchVideoColdTime = 120;

    //整数值，记录玩家上次通过观看插页来获得道具点的具体时间（年、月、日、时、分、秒）
    public static int lastWatchChartboostYear = 2016;
    public static int lastWatchChartboostMonth = 1;
    public static int lastWatchChartboostDay = 1;
    public static int lastWatchChartboostHour = 0;
    public static int lastWatchChartboostMinute = 0;
    public static int lastWatchChartboostSecond = 0;

    //玩家通过观看插页来获得道具点的冷却时间，以秒为单位
    public static float watchChartboostColdTime = 60;

    //方法，通用的播放音频的方法
    public static void AudioPlay(AudioSource audio_source, int enable)
    {
        //如果音频激活，并且声音开关也激活
        if (audio_source != null && enable == 1)
        {
            //播放对应的音频
            audio_source.Play();
        }
    }

    //重载方法
    public static void AudioPlay(AudioSource audio_source, AudioClip audio_clip, int enable)
    {
        //如果音频激活
        if (audio_source != null && enable == 1)
        {
            //播放对应的音频
            audio_source.PlayOneShot(audio_clip);
        }
    }

    //通用的音频停止方法
    public static void AudioStop(AudioSource audio_source)
    {
        //如果音频非空
        if (audio_source != null)
        {
            audio_source.Stop();
        }
    }

    //方法，产生一个[0,m)的随机数组
	public static int[] CreatRandomArray(int m){

		int[] tag = new int[m];
		int[] result = new int[m];
		int temp;

		for (int i = 0; i < m; i++) {
			
			temp = Random.Range(0, m - i);
			
			int k = - 1;
			
			for(int j = 0; j < m; j++){
				
				if(tag[j] == 0){

					k ++;
				}

				if(k == temp)
				{
					result[i] = j;

					tag[j] = 1;

					break;
				}
			}
		}

		return result;
	}
}

