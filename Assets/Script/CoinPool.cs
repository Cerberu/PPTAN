using UnityEngine;
using System.Collections.Generic;

//缓存池
public class CoinPool
{
    //集合，存储所有块
    public static List<GameObject> blockArray = new List<GameObject>();

    //方法，从缓存池中取出一个块
    public static GameObject GetCoin()
    {
        //最终获得的块
        GameObject resultBlock = null;

        //遍历集合
        foreach (GameObject block in blockArray)
        {
            //如果缓存池中存在可用的块
            if ((block != null) && (!block.activeSelf))
            {
                //该块即为最终获得的块
                resultBlock = block;

                //该块激活
                resultBlock.SetActive(true);

                //跳出循环
                break;
            }
        }

        //如果集合中找不到可用的块
        if (resultBlock == null)
        {
            //手动实例化一个块
            resultBlock = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefab/FlyingCoin")) as GameObject;

            //将该块添加到对应的集合中
            blockArray.Add(resultBlock);
        }

        //返回最终获得的块
        return resultBlock;
    }
}
