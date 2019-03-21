using UnityEngine;

public class Player : MonoBehaviour {

    //玩家索引
    public int playerIndex;

    //玩家的初始方向
    public Vector2 playerSendUpDirection;

    //bool值，指示玩家小球是否处于落地位置调整状态
    bool fallPositionAdjustState = false;

    //浮点值，小球自身旋转的角速度
    float playerRotateSpeed = 540;

    //整数值，记录玩家小球以接近水平或垂直的速度离开外壁的次数
    int leaveUpperChessCount = 0;
    int leaveLeftChessCount = 0;
    int leaveRightChessCount = 0;

    //浮点值，上次进行速度修正时的游戏时间
    float lastAdjustTime = 0;

    void Update()
    {
        if (MyClass.selectedPlayerIndex == 4 || MyClass.selectedPlayerIndex == 7)
        {
            //小球旋转
            transform.Rotate(Vector3.back * playerRotateSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //临时距离
        float tempDistance = 0;

        //如果处于落地位置调整状态
        if (fallPositionAdjustState)
        {
            //计算玩家小球当前位置与当前回合落地最终位置之间的距离
            tempDistance = Vector3.Distance(transform.position, GameController.Instance.fallenEndPosition);

            //如果两者之间的距离小于0.05
            if(tempDistance < 0.05F)
            {
                //退出落地位置调整状态
                fallPositionAdjustState = false;

                //玩家小球停止移动
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                //该玩家小球禁用
                gameObject.SetActive(false);

                //落地位置调整个数计数
                GameController.Instance.fallenAdjustBallNumber++;

                //如果所有小球位置调整结束
                if (GameController.Instance.fallenAdjustBallNumber == (GameController.Instance.currentSendedBallNumber - 1))
                {
                    //执行所有小球落地完成之后的操纵
                    GameController.Instance.AllBallFallenCompleted();
                }
            }
        }
    }

    //方法，更新玩家图片
    public void RefreshPlayerSprite()
    {
        //根据玩家索引，使用不同的玩家图片
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PlayerSprites/" + playerIndex);

        //计算角色的碰撞范围
        GetComponent<CircleCollider2D>().radius = MyClass.charactorRadius[playerIndex];
    }

    //方法，玩家发射
    public void PlayerSendUp()
    {
        //玩家的碰撞体组件激活
        GetComponent<CircleCollider2D>().enabled = true;

        //设定玩家的初始速度
        GetComponent<Rigidbody2D>().velocity = MyClass.playerVelocityAmplitude * playerSendUpDirection;

        //玩家小球以接近水平或垂直的速度离开外壁的次数清0
        leaveUpperChessCount = 0;
        leaveLeftChessCount = 0;
        leaveRightChessCount = 0;
    }

    //方法，玩家小球落地位置调整
    public void PlayerFallPositionAdjust()
    {
        //重新设定玩家小球的速度
        GetComponent<Rigidbody2D>().velocity = 1.5F * MyClass.playerVelocityAmplitude * (GameController.Instance.fallenEndPosition - transform.position).normalized;

        //进入落地位置调整状态
        fallPositionAdjustState = true;
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

    //方法，执行别的物体离开时的操作
    void OnCollisionExit2D(Collision2D coll)
    {
        //自身的刚体组件
        Rigidbody2D selfRigidbody2D;

        //归一化速度
        Vector3 normalizedVeloctiy;

        //临时角度
        float tempAngle;

        //如果离开的物体对象不是下方棋盘壁
        if (coll.gameObject.tag != "LowerChess")
        {
            //如果碰撞时的游戏时间距离上次速度修正时间超过2秒
            if ((Time.time - lastAdjustTime) >= 2)
            {
                //获得自身的刚体组件
                selfRigidbody2D = GetComponent<Rigidbody2D>();

                //计算归一化速度
                normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

                //重新设定玩家小球的速度
                selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * normalizedVeloctiy;

                //将本次修正的时间记录为上次修正时间
                lastAdjustTime = Time.time;
            }

            //如果碰到的是上、左、右三面墙壁
            if (coll.gameObject.tag == "UpperChess" ||
               coll.gameObject.tag == "LeftChess" ||
               coll.gameObject.tag == "RightChess")
            {
                //获得自身的刚体组件
                selfRigidbody2D = GetComponent<Rigidbody2D>();

                //计算归一化速度
                normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

                //计算速度的角度
                tempAngle = CalculateTargetAngle(normalizedVeloctiy.x, normalizedVeloctiy.y);

                //如果离开棋盘上边界
                if (coll.gameObject.tag == "UpperChess")
                {
                    //如果速度角度接近270度
                    if (tempAngle > 265 && tempAngle < 275)
                    {
                        //计数
                        leaveUpperChessCount++;

                        //如果累计次数达到3次
                        if (leaveUpperChessCount == 3)
                        {
                            //如果角度小于等于270
                            if (tempAngle <= 270)
                            {
                                //角度为260
                                tempAngle = 260;
                            }

                            //如果角度大于270
                            if (tempAngle > 270)
                            {
                                //角度为280
                                tempAngle = 280;
                            }

                            //计数清0
                            leaveUpperChessCount = 0;

                            //重新设定玩家小球的速度
                            selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                        }
                    }

                    //否则
                    else
                    {
                        //计数清0
                        leaveUpperChessCount = 0;
                    }
                }

                //如果离开棋盘左边界
                if (coll.gameObject.tag == "LeftChess")
                {
                    //如果速度角度接近0度
                    if ((tempAngle >= 0 && tempAngle < 5) ||
                        (tempAngle > 355 && tempAngle < 360))
                    {
                        //计数
                        leaveLeftChessCount++;

                        //如果累计次数达到5次
                        if (leaveLeftChessCount == 3)
                        {
                            //如果角度[0,5)
                            if (tempAngle >= 0 && tempAngle < 5)
                            {
                                //角度为10
                                tempAngle = 10;
                            }

                            //如果角度[355,360)
                            if (tempAngle > 355 && tempAngle < 360)
                            {
                                //角度为350
                                tempAngle = 350;
                            }

                            //计数清0
                            leaveLeftChessCount = 0;

                            //重新设定玩家小球的速度
                            selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                        }
                    }

                    //否则
                    else
                    {
                        //计数清0
                        leaveLeftChessCount = 0;
                    }
                }

                //如果离开棋盘右边界
                if (coll.gameObject.tag == "RightChess")
                {
                    //如果速度角度接近180度
                    if (tempAngle > 175 && tempAngle < 185)
                    {
                        //计数
                        leaveRightChessCount++;

                        //如果累计次数达到3次
                        if (leaveRightChessCount == 3)
                        {
                            //如果角度小于等于180
                            if (tempAngle <= 180)
                            {
                                //角度为170
                                tempAngle = 170;
                            }

                            //如果角度大于180
                            if (tempAngle > 180)
                            {
                                //角度为190
                                tempAngle = 190;
                            }

                            //计数清0
                            leaveRightChessCount = 0;

                            //重新设定玩家小球的速度
                            selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                        }
                    }

                    //否则
                    else
                    {
                        //计数清0
                        leaveRightChessCount = 0;
                    }
                }
            }
        }
    }
}
