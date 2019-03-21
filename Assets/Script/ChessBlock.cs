using UnityEngine;
using UnityEngine.UI;

public class ChessBlock : MonoBehaviour {

    //整数值，块的行列索引
    public int rowIndex;
    public int columnIndex;

    //整数值，该方块的类型索引,0表示正方形方块
    public int typeIndex;

    //整数值，该块的生命值
    public int lifeValue;

    //显示方块生命值的Text组件
    public Text lifeText;

    //bool值，指示该棋盘方块在当前回合是否被触碰过
    public bool isTouched;

    //方法，刷新棋盘方块的显示
    public void RefreshChessBlock()
    {
        //如果棋盘块类型小于等于10
        if (typeIndex <= 10)
        {
            //显示方块的生命值
            lifeText.text = lifeValue.ToString();

            //加载对应颜色的块
            GetComponent<Image>().sprite = Resources.Load<Sprite>("BlockSprites/Block" + typeIndex + "Life" + Random.Range(0, 5));             
        }
    }

    //方法，棋盘方块下移
    public void ChessBlockMoveDown()
    {
        //数值变化
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.5F, "easetype", iTween.EaseType.easeInOutSine, "onupdate", "ChessBlockMovingDown", "oncomplete", "ChessBlockMoveDownEnd"));
    }

    //方法，棋盘方块下移进行时
    void ChessBlockMovingDown(float value)
    {
        //初始时的Y值
        float startLocalY = GameController.Instance.chessBlockStandardLocalPosition[rowIndex, columnIndex].y;

        //终止时的Y值
        float endLocalY = GameController.Instance.chessBlockStandardLocalPosition[rowIndex - 1, columnIndex].y;

        //该棋盘方块的实时本地位置
        GetComponent<RectTransform>().anchoredPosition3D = new Vector3(GameController.Instance.chessBlockStandardLocalPosition[rowIndex, columnIndex].x,
                                                                       startLocalY * (1 - value) + endLocalY * value,
                                                                       0);
    }

    //方法，棋盘方块下移完成之后的操作
    void ChessBlockMoveDownEnd()
    {
        //计数
        GameController.Instance.currentMovingChessBlockNumber++;

        //如果所有的棋盘方块下移都已完成
        if (GameController.Instance.currentMovingChessBlockNumber == GameController.Instance.currentChessBlockNumber)
        {
            //执行所有棋盘方块下移完成之后的操作
            GameController.Instance.AllChessBlockMoveDownCompleted();
        }
    }

    //方法，执行别的物体进入棋盘方块时的操作
    void OnCollisionExit2D(Collision2D coll)
    {
        //如果碰到的是玩家小球
        if (coll.gameObject.tag == "Player")
        {
            //如果该棋盘方块类型小于等于10
            if(typeIndex <= 10)
            {
                //该方块的生命值-1
                lifeValue--;

                //如果剩余生命值大于0
                if(lifeValue > 0)
                {
                    //刷新棋盘方块的显示
                    RefreshChessBlock();
                }

                else
                {
                    //棋盘方块消失
                    ChessBlockDisappear();
                }

                //如果撞击播放器不处于繁忙状态
                if (GameController.Instance.ballPlayerBusyState == 0)
                {
                    //播放小球音效
                    MyClass.AudioPlay(GameController.Instance.ballPlayer.GetComponent<AudioSource>(),
                                      Resources.Load<AudioClip>("Audio/Ball" + MyClass.selectedPlayerIndex),
                                      MyClass.soundEnable);

                    //撞击播放器进入繁忙状态
                    GameController.Instance.ballPlayer.BallPlayerEnterBusyState();
                }
            }
        }
    }

    //方法，执行别的物体进入棋盘方块时的操作
    void OnTriggerEnter2D(Collider2D other)
    {
        //内部计数器
        int i = 0;
        int j = 0;

        //临时的随机角度
        float tempRandomAngle;

        //该棋盘方块碰到的玩家小球的刚体组件
        Rigidbody2D targetPlayerRigidbody;

        //如果碰到的是玩家小球
        if(other.gameObject.tag == "Player")
        {
            //如果棋盘方块的类型为11
            if (typeIndex == 11)
            {
                //播放加号音效
                MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                                  Resources.Load<AudioClip>("Audio/Add"),
                                  MyClass.soundEnable);

                //当前回合获得加号道具的数量+1
                GameController.Instance.currentRoundAddNumber++;

                //该棋盘方块消失
                ChessBlockDisappear();
            }

            //如果棋盘方块的类型为12
            if (typeIndex == 12)
            {
                //该棋盘方块被触碰过
                isTouched = true;

                //该棋盘方块变为最后一个子物体
                transform.SetAsLastSibling();

                //水平光带闪烁
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("LightBandBlink");

                //如果光带播放器不处于繁忙状态
                if (GameController.Instance.bandPlayerBusyState == 0)
                {
                    //播放光带音效
                    MyClass.AudioPlay(GameController.Instance.bandPlayer.GetComponent<AudioSource>(), MyClass.soundEnable);

                    //光带播放器进入繁忙状态
                    GameController.Instance.bandPlayer.BandPlayerEnterBusyState();
                }

                //列遍历
                for (j = 0; j < MyClass.chessColumn; j++)
                {
                    //如果对应的棋盘方格处存在方块，并且方块的类型为[0,10]
                    if (GameController.Instance.chessBlockArray[rowIndex, j] != null &&
                        GameController.Instance.chessBlockArray[rowIndex, j].typeIndex <= 10)
                    {
                        //方块的生命值-1
                        GameController.Instance.chessBlockArray[rowIndex, j].lifeValue--;

                        //如果剩余生命值大于0
                        if (GameController.Instance.chessBlockArray[rowIndex, j].lifeValue > 0)
                        {
                            //刷新棋盘方块的显示
                            GameController.Instance.chessBlockArray[rowIndex, j].RefreshChessBlock();
                        }

                        //如果生命值为0
                        if (GameController.Instance.chessBlockArray[rowIndex, j].lifeValue == 0)
                        {
                            //对应的棋盘方块消失
                            GameController.Instance.chessBlockArray[rowIndex, j].ChessBlockDisappear();
                        }
                    }
                }
            }

            //如果棋盘方块的类型为13
            if (typeIndex == 13)
            {
                //该棋盘方块被触碰过
                isTouched = true;

                //该棋盘方块变为最后一个子物体
                transform.SetAsLastSibling();

                //竖直光带闪烁
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("LightBandBlink");

                //如果光带播放器不处于繁忙状态
                if (GameController.Instance.bandPlayerBusyState == 0)
                {
                    //播放光带音效
                    MyClass.AudioPlay(GameController.Instance.bandPlayer.GetComponent<AudioSource>(), MyClass.soundEnable);

                    //光带播放器进入繁忙状态
                    GameController.Instance.bandPlayer.BandPlayerEnterBusyState();
                }

                //行遍历
                for (i = 0; i < MyClass.chessRow; i++)
                {
                    //如果对应的棋盘方格处存在方块，并且方块的类型为[0,10]
                    if (GameController.Instance.chessBlockArray[i, columnIndex] != null &&
                        GameController.Instance.chessBlockArray[i, columnIndex].typeIndex <= 10)
                    {
                        //方块的生命值-1
                        GameController.Instance.chessBlockArray[i, columnIndex].lifeValue--;

                        //如果剩余生命值大于0
                        if (GameController.Instance.chessBlockArray[i, columnIndex].lifeValue > 0)
                        {
                            //刷新棋盘方块的显示
                            GameController.Instance.chessBlockArray[i, columnIndex].RefreshChessBlock();
                        }

                        //如果生命值为0
                        if (GameController.Instance.chessBlockArray[i, columnIndex].lifeValue == 0)
                        {
                            //对应的棋盘方块消失
                            GameController.Instance.chessBlockArray[i, columnIndex].ChessBlockDisappear();
                        }
                    }
                }
            }

            //如果棋盘方块的类型为14
            if (typeIndex == 14)
            {
                //该棋盘方块被触碰过
                isTouched = true;

                //该棋盘方块变为最后一个子物体
                transform.SetAsLastSibling();

                //水平和数值光带闪烁
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("LightBandBlink");
                transform.GetChild(1).GetComponent<Animator>().SetTrigger("LightBandBlink");

                //如果光带播放器不处于繁忙状态
                if (GameController.Instance.bandPlayerBusyState == 0)
                {
                    //播放光带音效
                    MyClass.AudioPlay(GameController.Instance.bandPlayer.GetComponent<AudioSource>(), MyClass.soundEnable);

                    //光带播放器进入繁忙状态
                    GameController.Instance.bandPlayer.BandPlayerEnterBusyState();
                }

                //行列遍历
                for (i = 0; i < MyClass.chessRow; i++)
                {
                    for (j = 0; j < MyClass.chessColumn; j++)
                    {
                        //如果对应的棋盘方格处存在方块，并且方块的类型为[0,10]
                        if ((i == rowIndex || j == columnIndex) &&
                            GameController.Instance.chessBlockArray[i, j] != null &&
                            GameController.Instance.chessBlockArray[i, j].typeIndex <= 10)
                        {
                            //方块的生命值-1
                            GameController.Instance.chessBlockArray[i, j].lifeValue--;

                            //如果剩余生命值大于0
                            if (GameController.Instance.chessBlockArray[i, j].lifeValue > 0)
                            {
                                //刷新棋盘方块的显示
                                GameController.Instance.chessBlockArray[i, j].RefreshChessBlock();
                            }

                            //如果生命值为0
                            if (GameController.Instance.chessBlockArray[i, j].lifeValue == 0)
                            {
                                //对应的棋盘方块消失
                                GameController.Instance.chessBlockArray[i, j].ChessBlockDisappear();
                            }
                        }
                    }
                }
            }

            //如果棋盘方块的类型为15
            if (typeIndex == 15)
            {
                //该棋盘方块被触碰过
                isTouched = true;

                //产生[30,150]度之间的随机角度
                tempRandomAngle = Random.Range(30.0F, 150.0F);

                //获得碰到的玩家的刚体组件
                targetPlayerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();

                //重新设定目标玩家的速度
                targetPlayerRigidbody.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempRandomAngle * Mathf.Deg2Rad), Mathf.Sin(tempRandomAngle * Mathf.Deg2Rad));
            }

            //如果棋盘方块的类型为16
            if (typeIndex == 16)
            {
                //播放金币音效
                MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                                  Resources.Load<AudioClip>("Audio/Dollar"),
                                  MyClass.soundEnable);

                //玩家获得的道具点数+1
                MyClass.propertyPoints++;

                //更新道具点数
                GameController.Instance.RefreshPropertyPoints();

                //该棋盘方块消失
                ChessBlockDisappear();
            }
        }
    }

    //方法，棋盘方块消失
    public void ChessBlockDisappear()
    {
        //如果方块类型[0,10]
        if (typeIndex <= 10)
        {
            //在该棋盘的位置处产生一枚爆炸粒子效果
            BlockDisappearPool.GetBlockDisappearParticle(transform.position);

            //播放方块爆炸音效
            MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                              Resources.Load<AudioClip>("Audio/BlockBroken"),
                              MyClass.soundEnable);

            //对应的棋盘数组为空
            GameController.Instance.chessBlockArray[rowIndex, columnIndex] = null;

            //该棋盘方块禁用
            gameObject.SetActive(false);
        }

        //如果方块类型11或16
        if (typeIndex == 11 || typeIndex == 16)
        {
            //对应的棋盘数组为空
            GameController.Instance.chessBlockArray[rowIndex, columnIndex] = null;

            //该棋盘方块禁用
            gameObject.SetActive(false);
        }

        //如果方块类型[12,15]
        if(typeIndex >= 12 && typeIndex <= 15)
        {
            //对应的棋盘数组为空
            GameController.Instance.chessBlockArray[rowIndex, columnIndex] = null;

            //播放离场动画
            GetComponent<Animator>().SetTrigger("ChessBlockDisappear");
        }
    }

    //方法，棋盘方块离场动画结束之后的操作
    void ChessBlockDisappearCompleted()
    {
        //该棋盘方块禁用
        gameObject.SetActive(false);
    }
}
