using UnityEngine;
using UnityEngine.UI;

public class AccountInterface : MonoBehaviour {

    //显示分数的文本
    public Text scoreText;

    //显示最高分数的文本
    public Text bestScoreText;

    //显示本局分数创纪录的标记
    public GameObject newTag;

	// Use this for initialization
    void Start()
    {
        //显示本局分数和历史最高分数
        scoreText.text = MyClass.score.ToString();

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

        //根据是否创造了新的高分记录，来显示响应的标记
        newTag.SetActive(GameController.Instance.newScoreEnable);
    }

    //方法，执行结算界面入场动画结束之后的操作
    public void AccountInterfaceStartCompleted()
    {
        //结算界面响应点击
        GameController.Instance.accountInterfaceClickEnable = true;

        //游戏结束之后播放相关广告和引导评论
        GameController.Instance.ShowGameEndADsAndPopRate();
    }
}
