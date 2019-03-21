using UnityEngine;
using System.Collections;

public class MenuPlayer : MonoBehaviour {

    //浮点值，小球自身旋转的角速度
    float playerRotateSpeed = 540;

    //整数值，记录玩家小球以接近水平或垂直的速度离开外壁的次数
    int leaveUpperWallCount = 0;
    int leaveLowerWallCount = 0;
    int leaveLeftWallCount = 0;
    int leaveRightWallCount = 0;

    void Update()
    {
        if (MyClass.selectedPlayerIndex == 4 || MyClass.selectedPlayerIndex == 7)
        {
            //小球旋转
            transform.Rotate(Vector3.back * playerRotateSpeed * Time.deltaTime);
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

    //方法，执行别的物体离开时的操作
    void OnCollisionExit2D(Collision2D coll)
    {
        //自身的刚体组件
        Rigidbody2D selfRigidbody2D;

        //归一化速度
        Vector3 normalizedVeloctiy;

        //临时角度
        float tempAngle;

        //如果离开棋盘上边界
        if (coll.gameObject.tag == "UpperWall")
        {
            //获得自身的刚体组件
            selfRigidbody2D = GetComponent<Rigidbody2D>();

            //计算归一化速度
            normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

            //计算速度的角度
            tempAngle = CalculateTargetAngle(normalizedVeloctiy.x, normalizedVeloctiy.y);

            //如果速度角度接近270度
            if (tempAngle > 265 && tempAngle < 275)
            {
                //计数
                leaveUpperWallCount++;

                //如果累计次数达到3次
                if (leaveUpperWallCount == 3)
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
                    leaveUpperWallCount = 0;

                    //重新设定玩家小球的速度
                    selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                }
            }

            //否则
            else
            {
                //计数清0
                leaveUpperWallCount = 0;
            }
        }

        //如果离开棋盘下边界
        if (coll.gameObject.tag == "LowerWall")
        {
            //获得自身的刚体组件
            selfRigidbody2D = GetComponent<Rigidbody2D>();

            //计算归一化速度
            normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

            //计算速度的角度
            tempAngle = CalculateTargetAngle(normalizedVeloctiy.x, normalizedVeloctiy.y);

            //如果速度角度接近90度
            if (tempAngle > 85 && tempAngle < 95)
            {
                //计数
                leaveLowerWallCount++;

                //如果累计次数达到3次
                if (leaveLowerWallCount == 3)
                {
                    //如果角度小于等于90
                    if (tempAngle <= 90)
                    {
                        //角度为80
                        tempAngle = 80;
                    }

                    //如果角度大于90
                    if (tempAngle > 90)
                    {
                        //角度为100
                        tempAngle = 100;
                    }

                    //计数清0
                    leaveLowerWallCount = 0;

                    //重新设定玩家小球的速度
                    selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                }
            }

            //否则
            else
            {
                //计数清0
                leaveLowerWallCount = 0;
            }
        }

        //如果离开棋盘左边界
        if (coll.gameObject.tag == "LeftWall")
        {
            //获得自身的刚体组件
            selfRigidbody2D = GetComponent<Rigidbody2D>();

            //计算归一化速度
            normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

            //计算速度的角度
            tempAngle = CalculateTargetAngle(normalizedVeloctiy.x, normalizedVeloctiy.y);

            //如果速度角度接近0度
            if ((tempAngle >= 0 && tempAngle < 5) ||
                (tempAngle > 355 && tempAngle < 360))
            {
                //计数
                leaveLeftWallCount++;

                //如果累计次数达到3次
                if (leaveLeftWallCount == 3)
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
                    leaveLeftWallCount = 0;

                    //重新设定玩家小球的速度
                    selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                }
            }

            //否则
            else
            {
                //计数清0
                leaveLeftWallCount = 0;
            }
        }

        //如果离开棋盘右边界
        if (coll.gameObject.tag == "RightWall")
        {
            //获得自身的刚体组件
            selfRigidbody2D = GetComponent<Rigidbody2D>();

            //计算归一化速度
            normalizedVeloctiy = selfRigidbody2D.velocity.normalized;

            //计算速度的角度
            tempAngle = CalculateTargetAngle(normalizedVeloctiy.x, normalizedVeloctiy.y);

            //如果速度角度接近180度
            if (tempAngle > 175 && tempAngle < 185)
            {
                //计数
                leaveRightWallCount++;

                //如果累计次数达到3次
                if (leaveRightWallCount == 3)
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
                    leaveRightWallCount = 0;

                    //重新设定玩家小球的速度
                    selfRigidbody2D.velocity = MyClass.playerVelocityAmplitude * new Vector2(Mathf.Cos(tempAngle * Mathf.Deg2Rad), Mathf.Sin(tempAngle * Mathf.Deg2Rad));
                }
            }

            //否则
            else
            {
                //计数清0
                leaveRightWallCount = 0;
            }
        }
    }
}
