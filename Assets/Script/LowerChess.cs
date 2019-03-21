using UnityEngine;
using System.Collections;

public class LowerChess : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //方法，执行别的物体进入下侧棋盘壁时的操作
    void OnCollisionEnter2D(Collision2D coll)
    {
        //目标刚体
        Rigidbody2D targetRigidbody;

        //目标的Player组件
        Player targetPlayer;

        //如果碰到的是玩家小球
        if(coll.gameObject.tag == "Player")
        {
            //获得目标的Player组件
            targetPlayer = coll.transform.GetComponent<Player>();

            //获得玩家小球的Rigidbody2D组件
            targetRigidbody = coll.gameObject.GetComponent<Rigidbody2D>();

            //如果玩家小球向下运动
            if (targetRigidbody.velocity.y < 0)
            {
                //玩家小球停止运动
                targetRigidbody.velocity = Vector2.zero;

                //玩家的碰撞体组件禁用
                targetRigidbody.GetComponent<CircleCollider2D>().enabled = false;

                //落地玩家小球计数
                GameController.Instance.currentFallenBallNumber++;

                //如果是最后一个小球落地
                if (GameController.Instance.currentFallenBallNumber == GameController.Instance.currentSendedBallNumber)
                {
                    //如果清屏
                    if (GameController.Instance.ScreenClearJudge())
                    {
                        //播放Amazing音效
                        MyClass.AudioPlay(GameObject.Find("SoundPlayer").GetComponent<AudioSource>(),
                                          Resources.Load<AudioClip>("Audio/Amazing"),
                                          MyClass.soundEnable);

                        //Surprise激活
                        GameController.Instance.surprise.SetActive(true);
                    }

                    //在最上面一行创建棋盘方块
                    GameController.Instance.CreatChessBlock(MyClass.chessRow - 1);

                    //计算当前棋盘上总共有多少个棋盘方块
                    GameController.Instance.CalculateCurrentChessBlockNumber();

                    //所有棋盘方块下移
                    GameController.Instance.AllChessBlockMoveDown();
                }

                //如果是第1个落地的小球
                if (GameController.Instance.currentFallenBallNumber == 1)
                {
                    //获得第1个小球
                    GameController.Instance.firstPlayer = targetPlayer;
                                                  
                    //该小球此时的位置，即为当前回合落地小球的最终位置
                    GameController.Instance.fallenEndPosition = coll.transform.position;

                    //如果当前回合，只发出了一枚小球
                    if(GameController.Instance.currentSendedBallNumber == 1)
                    {
                        //执行当前回合发出的小球全部落地完成之后的操作
                        GameController.Instance.AllBallFallenCompleted();
                    }
                }

                //如果是后续的落地小球
                else
                {
                    //目标玩家进行位置调整
                    targetPlayer.PlayerFallPositionAdjust();
                }
            }
        }
    }
}
