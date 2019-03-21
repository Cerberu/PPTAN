using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    //单例
    public static GameController Instance;

    /****************************************公有变量****************************************/

    //集合，存储当前回合的所有玩家小球
    public List<Player> playerArray = new List<Player>();

    //第一个玩家小球
    public Player firstPlayer;

    //数组，存储当前棋盘中的所有方块
    public ChessBlock[,] chessBlockArray;

    //数组，存储当前棋盘中所有方块的虚拟数组
    ChessBlock[,] dummyChessBlockArray;

    //数组，记录棋盘上所有棋盘方块的标准本地位置
    public Vector3[,] chessBlockStandardLocalPosition;

    //指示虚线的线渲染组件
    public LineRenderer indicateDashLine;

    //棋盘区域
    public Transform chessArea;

    //玩家区域
    public Transform playerPool;

    //方块区域
    public Transform chessBlockPool;

    //显示分数的文本
    public Text scoreText;

    //显示历史最高分的文本
    public Text bestScoreText;

    //显示道具点数的文本
    public Text propertyPointsText;

    //显示当前剩余小球数量的文本
    public Text leftBallNumberText;

    //显示当前回合新获得的小球数量的文本
    public Text addedBallNumberText;

    //撞击播放器
    public BallPlayer ballPlayer;

    //光带播放器
    public BandPlayer bandPlayer;

    //惊喜
    public GameObject surprise;

    //暂停界面
    public GameObject pauseInterface;

    //复活界面
    public GameObject resurgenceInterface;

    //结算界面
    public GameObject accountInterface;

    //游戏体，两个道具界面
    public GameObject propertyInterfaceWithRate;
    public GameObject propertyInterfaceWithoutRate;

    //游戏体，道具界面
    public GameObject propertyInterface;

    //游戏体，获得新成就界面
    public GameObject achieveInterface;

    //游戏体，新手引导界面
    public GameObject guideInterface;

    //bool值，指示是否所有方块下移均已完成
    public bool allChessBlockMoveDownEnd = false;

    //bool值，指示是否所有玩家小球落地完成
    public bool allBallFallenEnd = false;

    //bool值，指示游戏是否响应滑屏
    public bool screenDragEnable = false;

    //bool值，指示暂停界面是否响应点击
    public bool pauseInterfaceClickEnable = false;

    //bool值，指示复活界面是否响应点击
    public bool resurgenceInterfaceClickEnable = false;

    //bool值，指示结算界面是否响应点击
    public bool accountInterfaceClickEnable = false;

    //bool值，指示道具界面是否响应点击
    public bool propertyInterfaceClickEnable = false;

    //bool值，获得新成就界面是否响应点击
    public bool achieveInterfaceClickEnable = false;

    //bool值，道具引导界面是否允许点击
    public bool propertyGuideInterfaceClickEnable = false;

    //bool值，指示本局是否创造了新的最高分
    public bool newScoreEnable = false;

    //整数值，指示撞击播放器是否处于繁忙状态，0表示不繁忙，1表示繁忙
    public int ballPlayerBusyState = 0;

    //整数值，指示光带播放器是否处于繁忙状态，0表示不繁忙，1表示繁忙
    public int bandPlayerBusyState = 0;

    //整数值，当前棋盘上存在的棋盘方块的数目
    public int currentChessBlockNumber = 0;

    //整数值，当前回合，下移的方块的个数
    public int currentMovingChessBlockNumber = 0;

    //整数值，当前回合数
    public int currentRoundNumber = 1;

    //整数值，当前回合，玩家可以一次性发射小球的个数
    public int currentSendedBallNumber = 1;

    //整数值，当前回合，剩下的小球数目(尚未发射出去的)
    public int currentLeftBallNumber = 1;

    //整数值，当前回合，发射出去的小球落地的数目
    public int currentFallenBallNumber = 0;

    //整数值，当前回合，小球落地之后进行位置调整的个数
    public int fallenAdjustBallNumber = 0;

    //整数值，当前回合，获得的“+”号道具的数目
    public int currentRoundAddNumber = 0;

    //整数值，本局复活弹框弹出的次数
    public int currentResurgenceNumber = 0;

    //整数值，当前回合，小球落地的最终位置
    public Vector3 fallenEndPosition;

    /****************************************私有变量****************************************/

    //指示虚线是否激活
    bool indicateDashLineEnable = false;

    //当前回合小球发射的初始位置
    Vector3 sendUpStartPosition;

    //当前回合小球的发射方向
    Vector3 sendUpStartDirection;

    //vector3，开始滑屏时的鼠标位置
    Vector3 startDragMousePosition;

    //vector3，实时的鼠标位置
    Vector3 realTimeMousePosition;

    //音效播放器
    AudioSource soundPlayer;

    //在start之前执行
    void Awake()
    {
        //内部计数器
        int i = 0;

        //指定目标帧率
        Application.targetFrameRate = 100;

        //不支持多点触控
        Input.multiTouchEnabled = false;

        //获得单例自身
        Instance = this;

        //获得音效播放器
        soundPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();

        //遍历每个成就
        for (i = 0; i < MyClass.achievementCompletedState.Length; i++)
        {
            //从玩家偏好中读取该成就是否已达成
            MyClass.achievementCompletedState[i] = PlayerPrefs.GetInt("achievementCompletedState" + i, 0);
        }
    }

	// Use this for initialization
	void Start ()
    {
        //如果是IOS平台
        #if (IOS_SDK)

        //页面跟踪
        SdkToU3d.TrackPageBeginWithName("Game");

        #endif

        //游戏状态
        MyClass.gameState = GameState.Playing;

        //如果激活新手引导
        if(MyClass.tutorialEnable == 1)
        {
            //激活新手引导界面
            guideInterface.SetActive(true);
        }

        //刷新道具点数
        RefreshPropertyPoints();

        //初始时，指示虚线禁用
        indicateDashLineEnable = false;

        //初始时，所有小球落地默认完成
        allBallFallenEnd = true;

        //初始时，所有方块下移默认未完成
        allChessBlockMoveDownEnd = false;

        //初始时，屏幕不响应点击
        screenDragEnable = false;

        //初始时，撞击播放器不繁忙
        ballPlayerBusyState = 0;

        //初始时，光带播放器不繁忙
        bandPlayerBusyState = 0;

        //初始时，默认小球发射方向垂直向上
        sendUpStartDirection = Vector3.up;

        //如果全新开局
        if (MyClass.gameStartWay == 0)
        {
            //初始时，复活次数为0
            currentResurgenceNumber = 0;

            //初始时，游戏处于第1回合
            currentRoundNumber = 1;

            //初始时，玩家只能一次性发射1枚小球
            currentSendedBallNumber = 1;

            //初始时，玩家处于棋盘中央正下方
            sendUpStartPosition = chessArea.position +
                                  (MyClass.chessHeight / 2) * Vector3.down +
                                  MyClass.charactorRadius[MyClass.selectedPlayerIndex] * Vector3.up;
        }

        //如果加载上次进度
        else
        {
            //从玩家偏好中读取当前复活次数
            currentResurgenceNumber = PlayerPrefs.GetInt("currentResurgenceNumber", 0);

            //从玩家偏好中读取当前回合数
            currentRoundNumber = PlayerPrefs.GetInt("currentRoundNumber", 1);

            //从玩家偏好中读取玩家可以发射多少枚小球
            currentSendedBallNumber = PlayerPrefs.GetInt("currentSendedBallNumber", 1);

            //计算玩家小球初始位置
            sendUpStartPosition = chessArea.position +
                                  (MyClass.chessHeight / 2) * Vector3.down +
                                  MyClass.charactorRadius[MyClass.selectedPlayerIndex] * Vector3.up +
                                  PlayerPrefs.GetFloat("sendUpX", 0) * Vector3.right;
        }

        //初始时分数即为当前回合数
        MyClass.score = currentRoundNumber - 1;

        //刷新分数
        RefreshScore();

        //初始时，尚未发射出去的小球数量为1
        currentLeftBallNumber = currentSendedBallNumber;

        //显示当前剩余小球数量的文本激活
        leftBallNumberText.gameObject.SetActive(true);

        //显示当前剩余小球数量的文本的位置
        leftBallNumberText.transform.position = new Vector3(sendUpStartPosition.x, -6.1F, 0);

        //显示当前剩余小球数量
        RefreshLeftBallNumber();

        //在初始位置处刷新一枚玩家小球
        CreatSinglePlayer(sendUpStartPosition, sendUpStartDirection);

        //获得第1个玩家小球
        firstPlayer = playerArray[0];

        //初始化棋盘方块数组
        chessBlockArray = new ChessBlock[MyClass.chessRow, MyClass.chessColumn];

        //初始化棋盘方块标准本地位置数组
        chessBlockStandardLocalPosition = new Vector3[MyClass.chessRow, MyClass.chessColumn];

        //计算棋盘方块的标准本地位置
        CalculateChessBlockStandardLocalPositon();

        //如果全新开局
        if (MyClass.gameStartWay == 0)
        {
            //创建棋盘方块
            CreatChessBlock(MyClass.chessRow - 1);

            //计算当前棋盘上总共有多少个棋盘方块
            CalculateCurrentChessBlockNumber();

            //所有棋盘方块下移
            AllChessBlockMoveDown();
        }

        //如果加载上次进度
        else
        {
            //按照playerprefs中存储的棋盘方格的属性，创建棋盘方块
            CreatPlayerprefsChessBlock();

            //屏幕开始响应点击
            screenDragEnable = true;
        }
    }

    //当该场景禁用时调用
    void OnDisable()
    {
        //如果是IOS平台
        #if (IOS_SDK)

        //页面跟踪
        SdkToU3d.TrackPageEndWithName("Game");

        #endif
    }

    //方法，计算棋盘方块的标准本地位置
    void CalculateChessBlockStandardLocalPositon()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //遍历棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //计算对应索引的棋盘方块的标准本地位置
                chessBlockStandardLocalPosition[i, j] = MyClass.baseChessBlockLocalPosition + MyClass.chessBlockSpace * new Vector3(j, i, 0);
            }
        }
    }

    //方法，创建指定行的棋盘方块
    public void CreatChessBlock(int targetRowIndex)
    {
        //内部计数器
        int i = 0;

        //调整系数
        float adjustRatio;

        //刷新加号道具的列索引
        int addColumnIndex;

        //临时的随机整数
        int tempRandomInt;
        int innerRandomInt;

        //临时产生的棋盘方块的类型
        int tempTypeIndex = -1;

        //临时产生的棋盘方块
        GameObject tempChessBlock;

        //获得一个随机的加号道具索引
        addColumnIndex = Random.Range(0, MyClass.chessColumn * 100) % MyClass.chessColumn;

        //列遍历
        for (i = 0; i < MyClass.chessColumn; i++)
        {
            //如果该索引为加号道具索引
            if (i == addColumnIndex)
            {
                //棋盘方块类型为11
                tempTypeIndex = 11;
            }

            //否则
            else
            {
                //调整系数
                adjustRatio = (MyClass.score <= 134) ? (1.5F * MyClass.score) : 200;

                //产生[0,1000)随机整数
                tempRandomInt = Random.Range(0, 10000) % 1000;

                //血量块
                if(tempRandomInt < (600 + adjustRatio))
                {
                    //产生[0,100)随机整数
                    innerRandomInt = Random.Range(0, 10000) % 100;

                    //[0,40) 
                    if(innerRandomInt < 40)
                    {
                        //类型0             ↑正方形
                        tempTypeIndex = 0;
                    }

                    //[40,60)
                    else if (innerRandomInt < 60)
                    {
                        //类型1              ↑菱形
                        tempTypeIndex = 1;
                    }

                    //[60,70)
                    else if (innerRandomInt < 70)
                    {
                        //类型2             ↑圆形
                        tempTypeIndex = 2;
                    }

                    //[70,100)
                    else
                    {
                        //类型[3,6]随机       ↑三角形
                        tempTypeIndex = 3 + Random.Range(0, 400) % 4;
                    }
                }

                //道具块
                else
                {
                    //产生[0,100)随机整数
                    innerRandomInt = Random.Range(0, 10000) % 100;

                    //[0,40) 
                    if (innerRandomInt < 40)
                    {
                        //类型-1             ↑空
                        tempTypeIndex = -1;
                    }

                    //[40,55)
                    else if (innerRandomInt < 55)
                    {
                        //类型[12,14]随机    ↑光带
                        tempTypeIndex = 12 + Random.Range(0, 300) % 3;
                    }

                    //[55,70)
                    else if (innerRandomInt < 70)
                    {
                        //类型15             ↑向上弹
                        tempTypeIndex = 15;
                    }

                    //[70,100)
                    else
                    {
                        //类型16金币
                        tempTypeIndex = 16;
                    }
                }                
            }

            //如果需要在对应位置产生任何一种方块
            if (tempTypeIndex >= 0)
            {
                //产生对应的棋盘方块
                tempChessBlock = ChessBlockPool.GetChessBlock(tempTypeIndex);

                //指定该棋盘方块的父物体
                tempChessBlock.transform.SetParent(chessBlockPool, false);

                //设定该棋盘方块的本地位置
                tempChessBlock.GetComponent<RectTransform>().anchoredPosition3D = chessBlockStandardLocalPosition[targetRowIndex, i];

                //如果棋盘方块类型为12
                if (tempTypeIndex == 12)
                {
                    //指定水平光带的本地位置
                    tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[targetRowIndex, i].x, 0, 0);
                }

                //如果棋盘方块类型为13
                if (tempTypeIndex == 13)
                {
                    //指定竖直光带的本地位置
                    tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[targetRowIndex, i].y, 0);
                }

                //如果棋盘方块类型为14
                if (tempTypeIndex == 14)
                {
                    //指定水平光带的本地位置
                    tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[targetRowIndex, i].x, 0, 0);

                    //指定竖直光带的本地位置
                    tempChessBlock.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[targetRowIndex, i].y, 0);
                }

                //获得该棋盘方块的ChessBlock组件，存入棋盘方块数组中
                chessBlockArray[targetRowIndex, i] = tempChessBlock.GetComponent<ChessBlock>();

                //设置该棋盘方块的行列索引
                chessBlockArray[targetRowIndex, i].rowIndex = targetRowIndex;
                chessBlockArray[targetRowIndex, i].columnIndex = i;

                //设置该棋盘方块的类型
                chessBlockArray[targetRowIndex, i].typeIndex = tempTypeIndex;

                //设置该棋盘方块的生命值
                chessBlockArray[targetRowIndex, i].lifeValue = currentRoundNumber;

                //初始时，该棋盘方块未被触碰过
                chessBlockArray[targetRowIndex, i].isTouched = false;

                //刷新该棋盘方块的显示
                chessBlockArray[targetRowIndex, i].RefreshChessBlock();
            }
        }
    }

    //方法，按照playerprefs中存储的棋盘方格的属性，创建棋盘方块
    void CreatPlayerprefsChessBlock()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //临时产生的棋盘方块的类型
        int tempTypeIndex = -1;

        //临时产生的棋盘方块
        GameObject tempChessBlock;

        //遍历棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //从玩家偏好中读取该棋盘位置处的方块类型
                tempTypeIndex = PlayerPrefs.GetInt("blockTypeR" + i + "C" + j, Random.Range(-1, 7));

                //如果需要在对应位置产生任何一种方块
                if (tempTypeIndex >= 0)
                {
                    //产生对应的棋盘方块
                    tempChessBlock = ChessBlockPool.GetChessBlock(tempTypeIndex);

                    //指定该棋盘方块的父物体
                    tempChessBlock.transform.SetParent(chessBlockPool, false);

                    //设定该棋盘方块的本地位置
                    tempChessBlock.GetComponent<RectTransform>().anchoredPosition3D = chessBlockStandardLocalPosition[i, j];

                    //如果棋盘方块类型为12
                    if (tempTypeIndex == 12)
                    {
                        //指定水平光带的本地位置
                        tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[i, j].x, 0, 0);
                    }

                    //如果棋盘方块类型为13
                    if (tempTypeIndex == 13)
                    {
                        //指定竖直光带的本地位置
                        tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[i, j].y, 0);
                    }

                    //如果棋盘方块类型为14
                    if (tempTypeIndex == 14)
                    {
                        //指定水平光带的本地位置
                        tempChessBlock.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[i, j].x, 0, 0);

                        //指定竖直光带的本地位置
                        tempChessBlock.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[i, j].y, 0);
                    }

                    //获得该棋盘方块的ChessBlock组件，存入棋盘方块数组中
                    chessBlockArray[i, j] = tempChessBlock.GetComponent<ChessBlock>();

                    //设置该棋盘方块的行列索引
                    chessBlockArray[i, j].rowIndex = i;
                    chessBlockArray[i, j].columnIndex = j;

                    //设置该棋盘方块的类型
                    chessBlockArray[i, j].typeIndex = tempTypeIndex;

                    //设置该棋盘方块的生命值
                    chessBlockArray[i, j].lifeValue = PlayerPrefs.GetInt("blockLifeR" + i + "C" + j, currentRoundNumber);

                    //初始时，该棋盘方块未被触碰过
                    chessBlockArray[i, j].isTouched = false;

                    //刷新该棋盘方块的显示
                    chessBlockArray[i, j].RefreshChessBlock();
                }
            }
        }
    }

    //方法，计算当前棋盘上(除第0行以外)总共有多少个棋盘方块
    public void CalculateCurrentChessBlockNumber()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //临时计数器
        int tempCount = 0;

        //遍历棋盘(除第0行以外)
        for (i = 1; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //如果该棋盘位置处存在方块
                if (chessBlockArray[i, j] != null)
                {
                    //计数
                    tempCount++;
                }
            }
        }

        //获得最终结果
        currentChessBlockNumber = tempCount;
    }

    //方法，所有棋盘方块下移
    public void AllChessBlockMoveDown()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //下移的方块计数清0
        currentMovingChessBlockNumber = 0;

        //方块下移尚未完成
        allChessBlockMoveDownEnd = false;

        //遍历棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //如果存在被触碰过的AOE棋盘方块和随机方向棋盘方块
                if (chessBlockArray[i, j] != null &&
                    chessBlockArray[i, j].typeIndex >= 12 &&
                    chessBlockArray[i, j].typeIndex <= 15 &&
                    chessBlockArray[i, j].isTouched == true)
                {
                    //该棋盘方块消失
                    chessBlockArray[i, j].ChessBlockDisappear();
                }
            }
        }

        //计算当前棋盘上(除第0行以外)总共有多少个棋盘方块
        CalculateCurrentChessBlockNumber();

        //如果棋盘上存在方块
        if (currentChessBlockNumber > 0)
        {
            //遍历棋盘(除第0行以外)
            for (i = 1; i < MyClass.chessRow; i++)
            {
                for (j = 0; j < MyClass.chessColumn; j++)
                {
                    //如果该棋盘位置处存在方块
                    if (chessBlockArray[i, j] != null)
                    {
                        //该棋盘方块下移
                        chessBlockArray[i, j].ChessBlockMoveDown();
                    }
                }
            }
        }

        //列遍历
        for (i = 0; i < MyClass.chessColumn; i++)
        {
            //如果第0行第i列存在方块，并且为方块类型为[11,16]
            if (chessBlockArray[0, i] != null &&
                chessBlockArray[0, i].typeIndex >= 11 &&
                chessBlockArray[0, i].typeIndex <= 16)
            {
                //该方块消失
                chessBlockArray[0, i].ChessBlockDisappear();
            }
        }
    }

    //方法，所有棋盘方块下移完成之后的操作
    public void AllChessBlockMoveDownCompleted()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //遍历棋盘(除第0行以外)
        for (i = 1; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //如果该棋盘位置处存在方块
                if (chessBlockArray[i, j] != null)
                {
                    //该棋盘方块下移
                    chessBlockArray[i, j].rowIndex--;
                }
            }
        }

        //初始化棋盘方块虚拟数组
        dummyChessBlockArray = new ChessBlock[MyClass.chessRow, MyClass.chessColumn];

        //遍历棋盘(除最上面一行以外)
        for (i = 0; i < (MyClass.chessRow - 1); i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //虚拟棋盘数组赋值
                dummyChessBlockArray[i, j] = chessBlockArray[i + 1, j];
            }
        }

        //遍历棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //将虚拟棋盘数组赋值给棋盘数组
                chessBlockArray[i, j] = dummyChessBlockArray[i, j];

                //如果对应索引的棋盘方格不为空
                if (chessBlockArray[i, j] != null)
                {
                    //如果棋盘方块类型为12
                    if (chessBlockArray[i, j].typeIndex == 12)
                    {
                        //指定水平光带的本地位置
                        chessBlockArray[i, j].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[i, j].x, 0, 0);
                    }

                    //如果棋盘方块类型为13
                    if (chessBlockArray[i, j].typeIndex == 13)
                    {
                        //指定竖直光带的本地位置
                        chessBlockArray[i, j].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[i, j].y, 0);
                    }

                    //如果棋盘方块类型为14
                    if (chessBlockArray[i, j].typeIndex == 14)
                    {
                        //指定水平光带的本地位置
                        chessBlockArray[i, j].transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-chessBlockStandardLocalPosition[i, j].x, 0, 0);

                        //指定竖直光带的本地位置
                        chessBlockArray[i, j].transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -chessBlockStandardLocalPosition[i, j].y, 0);
                    }
                }
            }
        }

        //所有方块下移完成
        allChessBlockMoveDownEnd = true;

        //如果所有玩家小球落地完成
        if (allBallFallenEnd)
        {
            //如果游戏失败
            if (GameDefeatJudge())
            {
                //执行游戏失败之后的操作
                GameDefeat();
            }

            //如果游戏尚未失败
            else
            {
                //当前回合结算
                RoundAccount();
            }
        }
    }

    //方法，创建单个玩家
    void CreatSinglePlayer(Vector3 targetPosition, Vector3 targetDirection)
    {
        //创建的临时玩家
        GameObject tempPlayer;

        //临时玩家的Player组件
        Player targetPlayer;

        //从缓存池中获得一枚玩家小球
        tempPlayer = PlayerPool.GetPlayer(MyClass.selectedPlayerIndex);

        //指定玩家小球的父物体
        tempPlayer.transform.SetParent(playerPool, false);

        //玩家小球的初始位置
        tempPlayer.transform.localPosition = targetPosition;

        //获得玩家小球的Player组件
        targetPlayer = tempPlayer.GetComponent<Player>();

        //指定玩家的索引
        targetPlayer.playerIndex = MyClass.selectedPlayerIndex;

        //玩家的初始方向
        targetPlayer.playerSendUpDirection = targetDirection;

        //更新玩家图片
        targetPlayer.RefreshPlayerSprite();

        //将该玩家添加进玩家集合中
        playerArray.Add(targetPlayer);
    }

    //方法，根据起点、终点和点间距，在两点之间创建一条指示虚线
    void RefreshIndicateDashLine(Vector3 startPosition, Vector3 endPosition)
    {
        //内部计数器
        int i = 0;

        //临时位置
        Vector3 tempPosition;

        //临时角度
        float tempAngle;

        //计算终点和起点的归一化相对方向
        Vector3 relativeDirection = (endPosition - startPosition).normalized;

        //计算归一化相对方向的角度
        tempAngle = CalculateTargetAngle(relativeDirection.x, relativeDirection.y);

        //如果角度小于最小阈值
        if((tempAngle >= 0 && tempAngle < MyClass.playerMinSendUpAngle) ||
           (tempAngle >= 270 && tempAngle < 360))
        {
            //重新设置终点和起点的归一化相对方向
            relativeDirection = new Vector3(Mathf.Cos(MyClass.playerMinSendUpAngle * Mathf.Deg2Rad),
                                            Mathf.Sin(MyClass.playerMinSendUpAngle * Mathf.Deg2Rad),
                                            0);
        }

        //如果角度大于最大阈值
        if(tempAngle > MyClass.playerMaxSendUpAngle && tempAngle < 270)
        {
            //重新设置终点和起点的归一化相对方向
            relativeDirection = new Vector3(Mathf.Cos(MyClass.playerMaxSendUpAngle * Mathf.Deg2Rad),
                                            Mathf.Sin(MyClass.playerMaxSendUpAngle * Mathf.Deg2Rad),
                                            0);
        }

        //初始化线渲染顶点数
        indicateDashLine.SetVertexCount(MyClass.maxDashLineVertexCount);

        //逐个顶点遍历
        for (i = 0; i < MyClass.maxDashLineVertexCount; i++)
        {
            //计算第i个顶点的位置
            tempPosition = startPosition + i * (MyClass.indicateDashLineIncrement * relativeDirection);

            //如果该点位于棋盘内
            if (PointInsideChessJudge(tempPosition))
            {
                //将该点作为指示虚线的第i个顶点
                indicateDashLine.SetPosition(i, tempPosition);
            }

            //否则
            else
            {
                //修正指示虚线的顶点数
                indicateDashLine.SetVertexCount(i);

                //跳出循环
                break;
            }
        }
    }

    //方法，通过目标角度的正余弦值，计算目标角度的值，范围为[0, 360)
    float CalculateTargetAngle(float targetCos, float targetSin)
    {
        //最终的角度值
        float targetAngle = 0;

        //如果目标角度位于第1象限, [0, 90)
        if (targetCos > 0 && targetSin >= 0)
        {
            //通过反余弦来获得最终的角度值
            targetAngle = Mathf.Acos(targetCos) * Mathf.Rad2Deg;
        }

        //如果目标角度位于第2象限, [90, 180)
        if (targetCos <= 0 && targetSin > 0)
        {
            //通过反余弦来获得最终的角度值
            targetAngle = Mathf.Acos(targetCos) * Mathf.Rad2Deg;
        }

        //如果目标角度位于第3象限, [180, 270)
        if (targetCos < 0 && targetSin <= 0)
        {
            //通过反余弦来获得最终的角度值
            targetAngle = 360 - Mathf.Acos(targetCos) * Mathf.Rad2Deg;
        }

        //如果目标角度位于第4象限, [270, 360)
        if (targetCos >= 0 && targetSin < 0)
        {
            //通过反余弦来获得最终的角度值
            targetAngle = 360 - Mathf.Acos(targetCos) * Mathf.Rad2Deg;
        }

        //返回最终结果值
        return targetAngle;
    }

    //方法，判断一个位置是否处于棋盘内
    bool PointInsideChessJudge(Vector3 targetPosition)
    {
        //根据目标点的x、y坐标值来判断
        return (Mathf.Abs(targetPosition.x - chessArea.position.x) <= (MyClass.chessWidth / 2)) &&
               (Mathf.Abs(targetPosition.y - chessArea.position.y) <= (MyClass.chessHeight / 2));
    }

    //方法，执行当玩家点击屏幕鼠标点下时的操作
    public void ScreenClickDown()
    {
        //屏幕响应滑动
        if (screenDragEnable)
        {
            //记录鼠标起始位置
            startDragMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startDragMousePosition.z = 0;

            //指示虚线处于激活状态
            indicateDashLineEnable = true;

            //指示虚线激活
            indicateDashLine.gameObject.SetActive(true);

            //沿着玩家小球和鼠标位置，产生一条指示虚线
            RefreshIndicateDashLine(sendUpStartPosition, startDragMousePosition);
        }
    }

    //方法，执行当玩家在屏幕中执行拖动操作时的操作
    public void ScreenOnDrag()
    {
        //屏幕响应滑动，并且指示虚线激活
        if (screenDragEnable && indicateDashLineEnable)
        {
            //记录鼠标实时位置
            realTimeMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            realTimeMousePosition.z = 0;

            //沿着玩家小球和鼠标位置，产生一条指示虚线
            RefreshIndicateDashLine(sendUpStartPosition, realTimeMousePosition);
        }
    }

    //方法，执行当玩家点击屏幕鼠标松开时的操作
    public void ScreenClickUp()
    {
        //临时角度
        float tempAngle;

        //屏幕响应滑动，并且指示虚线激活
        if (screenDragEnable && indicateDashLineEnable)
        {
            //屏幕不再响应滑动
            screenDragEnable = false;

            //指示虚线禁用状态
            indicateDashLineEnable = false;

            //指示虚线禁用
            indicateDashLine.gameObject.SetActive(false);

            //记录鼠标实时位置
            realTimeMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            realTimeMousePosition.z = 0;

            //计算小球的发射方向
            sendUpStartDirection = (realTimeMousePosition - sendUpStartPosition).normalized;

            //计算发射方向的角度
            tempAngle = CalculateTargetAngle(sendUpStartDirection.x, sendUpStartDirection.y);

            //如果角度小于最小阈值
            if ((tempAngle >= 0 && tempAngle < MyClass.playerMinSendUpAngle) ||
                (tempAngle >= 270 && tempAngle < 360))
            {
                //重新设置小球的发射方向
                sendUpStartDirection = new Vector3(Mathf.Cos(MyClass.playerMinSendUpAngle * Mathf.Deg2Rad),
                                                   Mathf.Sin(MyClass.playerMinSendUpAngle * Mathf.Deg2Rad),
                                                   0);
            }

            //如果角度大于最大阈值
            if (tempAngle > MyClass.playerMaxSendUpAngle && tempAngle < 270)
            {
                //重新设置终点和起点的归一化相对方向
                sendUpStartDirection = new Vector3(Mathf.Cos(MyClass.playerMaxSendUpAngle * Mathf.Deg2Rad),
                                                   Mathf.Sin(MyClass.playerMaxSendUpAngle * Mathf.Deg2Rad),
                                                   0);
            }

            //小球集合清空
            playerArray.Clear();

            //第1个玩家小球禁用
            firstPlayer.gameObject.SetActive(false);         

            //多小球创建与发射
            StartCoroutine(MultiPlayerSendUp());
        }
    }

    //方法，多小球发射
    IEnumerator<WaitForSeconds> MultiPlayerSendUp()
    {
        //内部计数器
        int i = 0;

        //当前回合获得加号道具的个数为0
        currentRoundAddNumber = 0;

        //落地小球数目清0
        currentFallenBallNumber = 0;

        //落地之后进行位置调整的小球个数清0
        fallenAdjustBallNumber = 0;

        //小球落地尚未完成
        allBallFallenEnd = false;

        //创建并发射后续小球
        for (i = 0; i < currentSendedBallNumber; i++)
        {
            //延迟0.08秒
            yield return new WaitForSeconds(0.08F);

            //创建小球
            CreatSinglePlayer(sendUpStartPosition, sendUpStartDirection);

            //小球开始发射
            playerArray[i].PlayerSendUp();

            //剩余小球递减
            currentLeftBallNumber--;

            //显示当前剩余小球数量
            RefreshLeftBallNumber();
        }
    }

    //方法，执行当前回合发出的小球全部落地完成之后的操作
    public void AllBallFallenCompleted()
    {
        //小球下一个回合发射的初始位置，即为当前回合落地的位置
        sendUpStartPosition = fallenEndPosition;

        //清空小球清空
        playerArray.Clear();

        //第一个小球注册
        playerArray.Add(firstPlayer);

        //所有小球落地完成
        allBallFallenEnd = true;

        //如果所有方块下移已完成
        if (allChessBlockMoveDownEnd)
        {
            //如果游戏失败
            if (GameDefeatJudge())
            {
                //执行游戏失败之后的操作
                GameDefeat();
            }

            //如果游戏尚未失败
            else
            {
                //当前回合结算
                RoundAccount();
            }
        }
    }

    //方法，游戏失败判定
    bool GameDefeatJudge()
    {
        //内部计数器
        int i = 0;

        //最终的返回结果
        bool result = false;

        //列遍历
        for (i = 0; i < MyClass.chessColumn; i++)
        {
            //如果第0行第i列存在方块，并且为方块类型小于等于10
            if (chessBlockArray[0, i] != null &&
                chessBlockArray[0, i].typeIndex <= 10)
            {
                //游戏失败
                result = true;

                //跳出循环
                break;
            }
        }

        //返回最终结果
        return result;
    }

    //方法，清屏判断
    public bool ScreenClearJudge()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //最终的返回结果
        bool result = true;

        //遍历棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //如果该棋盘位置处存在方块
                if (chessBlockArray[i, j] != null && chessBlockArray[i,j].typeIndex <= 10)
                {
                    //最终结果为false
                    result = false;

                    //跳出循环
                    break;
                }
            }
        }

        return result;
    }

    //方法，当前回合结算
    public void RoundAccount()
    {
        //回合数+1
        currentRoundNumber++;

        //分数即为当前回合数
        MyClass.score = currentRoundNumber - 1;

        //刷新分数
        RefreshScore();

        //更新当前回合可以发射的玩家小球个数
        currentSendedBallNumber += currentRoundAddNumber;

        //尚未发射出去的小球数量
        currentLeftBallNumber = currentSendedBallNumber;

        //显示当前剩余小球数量的文本激活
        leftBallNumberText.gameObject.SetActive(true);

        //显示当前剩余小球数量的文本的位置
        leftBallNumberText.transform.position = new Vector3(sendUpStartPosition.x, -6.1F, 0);

        //显示当前剩余小球数量
        RefreshLeftBallNumber();

        //如果当前回合新增小球数量大于0
        if (currentRoundAddNumber > 0)
        {
            //显示当前回合新增小球数量的文本激活
            addedBallNumberText.gameObject.SetActive(true);

            //显示当前回合新增小球数量的文本的位置
            addedBallNumberText.transform.position = new Vector3(sendUpStartPosition.x, -5, 0);

            //刷新当前回合新增小球数量
            RefreshAddedBallNumber();
        }

        //开局方式改变
        MyClass.gameStartWay = 1;

        //将该方式存入玩家偏好中
        PlayerPrefs.SetInt("gameStartWay", MyClass.gameStartWay);

        //保存游戏进度
        GameSave();

        //屏幕重新响应点击
        screenDragEnable = true;
    }

    //方法，游戏失败
    public void GameDefeat()
    {
        //屏幕不响应滑屏
        screenDragEnable = false;

        //如果本局复活次数尚未达到允许的最大次数
        if (currentResurgenceNumber < MyClass.maxResurgenceNumber)
        {
            //本局复活次数+1
            currentResurgenceNumber++;

            //复活界面不允许点击
            resurgenceInterfaceClickEnable = false;

            //复活界面激活
            resurgenceInterface.SetActive(true);
        }

        //否则
        else
        {
            //游戏结算
            GameAccount();
        }
    }

    //方法，游戏结算
    public void GameAccount()
    {
        //如果创造了新纪录
        if (newScoreEnable)
        {
            //播放游戏失败音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/GameWin"), MyClass.soundEnable);
        }

        //如果没有创造新纪录
        else
        {
            //播放游戏失败音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/GameDefeat"), MyClass.soundEnable);
        }

        //游戏状态
        MyClass.gameState = GameState.Defeat;

        //游戏加载方式变为0
        MyClass.gameStartWay = 0;

        //将该方式存入玩家偏好中
        PlayerPrefs.SetInt("gameStartWay", MyClass.gameStartWay);

        //如果是IOS平台
        #if (IOS_SDK)

        //截屏
        SdkToU3d.ScreenShot();

        //上报分数
        SdkToU3d.ReportScore(MyClass.score);

        //如果手机振动开启
        if (MyClass.vibrateEnable == 1)
        {
            //手机振动
            SdkToU3d.MobileVibrate();
        }

        #endif

        //结算界面不响应点击
        accountInterfaceClickEnable = false;

        //结算界面激活
        accountInterface.SetActive(true);
    }

    //方法，游戏复活
    public void GameResurgence()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //遍历棋盘最下面三行
        for (i = 0; i < 3; i++)
        {
            for(j = 0; j < MyClass.chessColumn; j++)
            {
                //如果该棋盘方块不为空
                if (chessBlockArray[i, j] != null)
                {
                    //该棋盘方块消失
                    chessBlockArray[i, j].ChessBlockDisappear();
                }
            }
        }

        //当前回合结算
        RoundAccount();
    }

    //方法，保存游戏进度
    void GameSave()
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //遍历整个棋盘
        for (i = 0; i < MyClass.chessRow; i++)
        {
            for (j = 0; j < MyClass.chessColumn; j++)
            {
                //如果该棋盘位置处不存在方块
                if(chessBlockArray[i, j] == null)
                {
                    //将该位置的方块类型存为-1
                    PlayerPrefs.SetInt("blockTypeR" + i + "C" + j, -1);

                    //将该位置的方块血量存为0
                    PlayerPrefs.SetInt("blockLifeR" + i + "C" + j, 0);
                }

                //如果该棋盘位置处存在方块
                else
                {
                    //存储该位置的方块类型
                    PlayerPrefs.SetInt("blockTypeR" + i + "C" + j, chessBlockArray[i,j].typeIndex);

                    //存储该位置的方块血量
                    PlayerPrefs.SetInt("blockLifeR" + i + "C" + j, chessBlockArray[i, j].lifeValue);
                }
            }
        }

        //存储玩家小球的起始发射X坐标
        PlayerPrefs.SetFloat("sendUpX", sendUpStartPosition.x);

        //存储当前的回合数
        PlayerPrefs.SetInt("currentRoundNumber", currentRoundNumber);

        //存储当前回合能发射的小球数量
        PlayerPrefs.SetInt("currentSendedBallNumber", currentSendedBallNumber);

        //存储当前的复活次数
        PlayerPrefs.SetInt("currentResurgenceNumber", currentResurgenceNumber);
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

        //显示道具点数
        propertyPointsText.text = MyClass.propertyPoints.ToString();

        //将道具点数存入玩家偏好中
        PlayerPrefs.SetInt("propertyPoints", MyClass.propertyPoints);
    }

    //方法，刷新当前分数和最高分数
    public void RefreshScore()
    {
        //显示当前分数
        scoreText.text = MyClass.score.ToString();

        //如果当前分数超过历史最好分数
        if (MyClass.score > MyClass.bestScore)
        {
            //创造了新的最高分
            newScoreEnable = true;

            //更新最高分数
            MyClass.bestScore = MyClass.score;

            //将新的最高分存入玩家偏好中
            PlayerPrefs.SetInt("bestScore", MyClass.bestScore);
        }

        //如果当前分数未超过历史最高分
        else
        {
            //未创造最高分
            newScoreEnable = false;
        }

        //如果本地化语言索引为汉语
        if (MyClass.localizationLanguageIndex == 0)
        {
            //显示历史最高分
            bestScoreText.text = "最高分 " + MyClass.bestScore.ToString();
        }

        else
        {
            //显示历史最高分
            bestScoreText.text = "Best " + MyClass.bestScore.ToString();
        }
    }

    //方法，刷新当前剩余小球数量
    public void RefreshLeftBallNumber()
    {
        //如果剩余小球大于0
        if (currentLeftBallNumber > 0)
        {
            //显示
            leftBallNumberText.text = "x " + currentLeftBallNumber.ToString();
        }

        else
        {
            //不显示
            leftBallNumberText.gameObject.SetActive(false);
        }
    }

    //方法，刷新当前回合新增小球数量
    public void RefreshAddedBallNumber()
    {
        //如果游戏语言为汉语
        if(MyClass.localizationLanguageIndex == 0)
        {
            //显示当前回合新增小球数目
            addedBallNumberText.text = "弹珠 + " + currentRoundAddNumber;
        }

        //如果游戏语言非汉语
        else
        {
            //显示当前回合新增小球数目
            addedBallNumberText.text = "BALL + " + currentRoundAddNumber;
        }       
    }

    //方法，视频复活按钮点击之后的操作
    public void VideoResurgenceButtonClick()
    {
        //如果复活界面允许点击
        if (resurgenceInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //看视频送点数的奖励激活
            MyClass.videoRewardEnable = 1;

            //视频奖励类型为1
            MyClass.videoRewardType = 1;

            //展示视频广告
            SdkToU3d.ShowUnityAds();

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("VideoResurgenceButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);
        }
    }

    //方法，退出复活界面按钮点击之后的操作
    public void ExitResurgenceButtonClick()
    {
        //如果复活界面允许点击
        if (resurgenceInterfaceClickEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("ExitResurgenceButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //复活界面不再允许点击
            resurgenceInterfaceClickEnable = false;

            //复活界面离场
            resurgenceInterface.GetComponent<Animator>().SetTrigger("ResurgenceInterfaceEnd");
        }
    }

    //方法，道具按钮点击之后的操作
    public void PropertyButtonClick()
    {
        //如果响应滑屏
        if (screenDragEnable)
        {
            #if (IOS_SDK)

            //按钮追踪
            SdkToU3d.TrackButtonActionWithName("PropertyButton");

            #endif

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

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

    //方法，游戏重启按钮被点击之后的操作
    public void RestartButtonClick()
    {
        //如果结算界面响应玩家点击
        if (accountInterfaceClickEnable)
        {
            //结算界面不再允许点击
            accountInterfaceClickEnable = false;

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //加载游戏场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Game");
        }
    }

    //方法，facebook分享按钮被点击之后的操作
    public void FacebookShareButtonClick()
    {
        //如果结算界面响应玩家点击
        if (accountInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //获得游戏本地化语言为汉语
            if (MyClass.localizationLanguageIndex == 0)
            {
                #if (IOS_SDK)

                //微信分享
                SdkToU3d.WeChatShareWithScore(MyClass.score);

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("WeChatShareButton");

                #endif

                //微信分享送道具的奖励禁用
                MyClass.weChatShareRewardEnable = 0;
            }

            //非汉语
            else
            {
                #if (IOS_SDK)

                //Facebook分享
                SdkToU3d.FacebookShare();

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("FacebookShareButton");

                #endif

                //facebook分享送道具的奖励禁用
                MyClass.facebookShareRewardEnable = 0;
            }        
        }
    }

    //方法，返回首页按钮被点击之后的操作
    public void MenuButtonClick()
    {
        //如果结算界面响应玩家点击
        if (accountInterfaceClickEnable)
        {
            //结算界面不再允许点击
            accountInterfaceClickEnable = false;

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //加载首页场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Menu");
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

    ////方法，观看插页按钮被点击之后的操作
    //public void WatchChartboostButtonClick()
    //{
    //    //如果道具界面响应点击
    //    if (propertyInterfaceClickEnable)
    //    {
    //        #if (IOS_SDK)

    //        //看插页送点数的奖励激活
    //        MyClass.chartboostRewardEnble = 1;

    //        //展示插页广告
    //        SdkToU3d.ShowChartboostInterstitial();

    //        //按钮追踪
    //        SdkToU3d.TrackButtonActionWithName("WatchChartboostButton");

    //        #endif

    //        //播放按钮音效
    //        MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

    //        //观看插页按钮禁用
    //        propertyInterface.GetComponent<PropertyInterface>().watchChartboostButton.SetActive(false);

    //        //观看插页禁用图片激活
    //        propertyInterface.GetComponent<PropertyInterface>().watchChartboostDisableImage.SetActive(true);

    //        //初始冷却时间
    //        propertyInterface.GetComponent<PropertyInterface>().watchChartboostColdTime.startColdTime = MyClass.watchChartboostColdTime;

    //        //开始倒计时
    //        propertyInterface.GetComponent<PropertyInterface>().watchChartboostColdTime.TimeCountDown();

    //        //将此时的时间存入玩家偏好中
    //        PlayerPrefs.SetInt("lastWatchChartboostYear", System.DateTime.Now.Year);
    //        PlayerPrefs.SetInt("lastWatchChartboostMonth", System.DateTime.Now.Month);
    //        PlayerPrefs.SetInt("lastWatchChartboostDay", System.DateTime.Now.Day);
    //        PlayerPrefs.SetInt("lastWatchChartboostHour", System.DateTime.Now.Hour);
    //        PlayerPrefs.SetInt("lastWatchChartboostMinute", System.DateTime.Now.Minute);
    //        PlayerPrefs.SetInt("lastWatchChartboostSecond", System.DateTime.Now.Second);
    //    }
    //}

    //方法，评论按钮被点击之后的操作
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

                //玩家获得道具点
                MyClass.propertyPoints += 20;

                //刷新道具点数
                RefreshPropertyPoints();

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

    ////方法，使用1美元购买60个点数按钮点击之后的操作
    //public void IAPOneDollarButton()
    //{
    //    //如果道具界面响应点击
    //    if (propertyInterfaceClickEnable)
    //    {
    //        //播放按钮音效
    //        MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

    //        #if (IOS_SDK)

    //        //使用1美元购买60个点数
    //        SdkToU3d.Buy60PointsWith1dollar();

    //        //按钮追踪
    //        SdkToU3d.TrackButtonActionWithName("IAPOneDollarButton");

    //        #endif
    //    }
    //}

    ////方法，使用5美元购买360个点数按钮点击之后的操作
    //public void IAPFiveDollarButton()
    //{
    //    //如果道具界面响应点击
    //    if (propertyInterfaceClickEnable)
    //    {
    //        //播放按钮音效
    //        MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

    //        #if (IOS_SDK)

    //        //使用5美元购买360个点数
    //        SdkToU3d.Buy360PointsWith5dollar();

    //        //按钮追踪
    //        SdkToU3d.TrackButtonActionWithName("IAPFiveDollarButton");

    //        #endif
    //    }
    //}

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

    //方法，执行暂停按钮被点击之后的操作
    public void PauseButtonClick()
    {
        //临时的随机数
        int tempInt = 0;

        //如果屏幕响应滑动
        if (screenDragEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //游戏进入暂停状态
            MyClass.gameState = GameState.Pause;

            //暂停界面不允许点击
            pauseInterfaceClickEnable = false;

            //暂停界面激活
            pauseInterface.SetActive(true);

            //产生[0,100)的随机数
            tempInt = Random.Range(0, 10000) % 100;

            //[0,30)
            if(tempInt < 30)
            {
                //播放相关广告和引导评论
                ShowGameEndADsAndPopRate();
            }
        }
    }

    //方法，暂停界面的返回首页按钮被点击之后的操作
    public void PauseMenuButtonClick()
    {
        //如果暂停界面响应玩家点击
        if (pauseInterfaceClickEnable)
        {
            //暂停界面不再允许点击
            pauseInterfaceClickEnable = false;

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //加载首页场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Menu");
        }
    }

    //方法，暂停界面的游戏重启按钮被点击之后的操作
    public void PauseRestartButtonClick()
    {
        //如果暂停界面响应玩家点击
        if (pauseInterfaceClickEnable)
        {
            //暂停界面不再允许点击
            pauseInterfaceClickEnable = false;

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //游戏加载方式变为0
            MyClass.gameStartWay = 0;

            //将该方式存入玩家偏好中
            PlayerPrefs.SetInt("gameStartWay", MyClass.gameStartWay);

            //加载首页场景
            GameObject.Find("FadeCanvas").GetComponent<FadeInOut>().SwitchScene("Game");
        }
    }

    //方法，暂停界面的继续游戏按钮被点击之后的操作
    public void PauseResumeButtonClick()
    {
        //如果暂停界面响应玩家点击
        if (pauseInterfaceClickEnable)
        {
            //暂停界面不再允许点击
            pauseInterfaceClickEnable = false;

            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //播放暂停界面离场动画
            pauseInterface.GetComponent<Animator>().SetTrigger("PauseInterfaceOut");
        }
    }

    //方法，成就界面的分享按钮点击之后的操作
    public void AchieveShareButtonClick()
    {
        //如果成就界面允许点击
        if (achieveInterfaceClickEnable)
        {
            //播放按钮音效
            MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

            //获得游戏本地化语言为汉语
            if (MyClass.localizationLanguageIndex == 0)
            {
                #if (IOS_SDK)

                //微信分享
                SdkToU3d.WeChatShareWithScore(MyClass.score);

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("WeChatShareButton");

                #endif

                //微信分享送道具的奖励禁用
                MyClass.weChatShareRewardEnable = 0;
            }

            //非汉语
            else
            {
                #if (IOS_SDK)

                //Facebook分享
                SdkToU3d.FacebookShare();

                //按钮追踪
                SdkToU3d.TrackButtonActionWithName("FacebookShareButton");

                #endif

                //facebook分享送道具的奖励禁用
                MyClass.facebookShareRewardEnable = 0;
            }
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
            achieveInterface.GetComponent<Animator>().SetTrigger("AchieveInterfaceOut");
        }
    }

    //方法，退出新手引导界面按钮点击之后的操作
    public void ExitGuideButtonClick()
    {
        //播放按钮音效
        MyClass.AudioPlay(soundPlayer, Resources.Load<AudioClip>("Audio/Button"), MyClass.soundEnable);

        //引导界面禁用
        guideInterface.SetActive(false);

        //不再激活新手引导
        MyClass.tutorialEnable = 0;

        //将该状态存入玩家偏好中
        PlayerPrefs.SetInt("tutorialEnable", MyClass.tutorialEnable);
    }

    //方法，播放相关广告和引导评论
    public void ShowGameEndADsAndPopRate()
    {
        //如果是IOS平台
        #if (IOS_SDK)

        //如果玩家之前没有进行过强制评论，而且当天允许弹出强制评论界面
        if (SdkToU3d.GetIfRate() == false && MyClass.popRateEnableToday == 1)
        {
            //玩游戏次数加1
            MyClass.playedNumerToday++;

            //如果玩游戏的次数达到目标次数
            if (MyClass.playedNumerToday == MyClass.popRateTargetPlayedNumber)
            {
                //当天的次数清0
                MyClass.playedNumerToday = 0;

                //当天不再弹出强制评论界面
                MyClass.popRateEnableToday = 0;

                //弹出强制评论界面
                SdkToU3d.PopRate();
            }

            //次数未到达目标次数
            else
            {
                //看视频送点数的奖励关闭
                MyClass.videoRewardEnable = 0;

                //看插页送点数的奖励关闭
                MyClass.chartboostRewardEnble = 0;

                //播放游戏结束时广告
                SdkToU3d.ShowGameEndADs();
            }

            //将玩游戏的次数存入玩家偏好之中
            PlayerPrefs.SetInt("playedNumerToday", MyClass.playedNumerToday);

            //将当天是否允许弹出强制评论界面的状态标志位存入玩家偏好中
            PlayerPrefs.SetInt("popRateEnableToday", MyClass.popRateEnableToday);
        }

        //否则
        else
        {
            //看视频送点数的奖励关闭
            MyClass.videoRewardEnable = 0;

            //看插页送点数的奖励关闭
            MyClass.chartboostRewardEnble = 0;

            //播放游戏结束时广告
            SdkToU3d.ShowGameEndADs();
        }

        #endif
    }
}
