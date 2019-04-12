using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : AlgoVisual {
    List<Pos> list = new List<Pos>();
    AScore[,] astar_search;
    short[,] temp_map = new short[Height,Width];
    IEnumerator InitAStar()
    {
        astar_search = new AScore[mapInstance.map.GetLength(0),mapInstance.map.GetLength(1)];
        astar_search[mapInstance.startPos.y,mapInstance.startPos.x] = new AScore(0,0);
        list.Add(mapInstance.startPos);

        while(list.Count > 0)
        {
            list.Sort((Pos p1,Pos p2) =>
            {// 根据估价值对open表中的节点排序
                AScore a1 = astar_search[p1.y,p1.x];
                AScore a2 = astar_search[p2.y,p2.x];

                return a1.CompareTo(a2);
            });
            // 
            Pos cur = list[0];
            list.RemoveAt(0);
            astar_search[cur.y,cur.x].closed = true;

            // 上
            if (cur.y > 0)
            {
                if (checkPos(cur, 0, -1)) { break; }
            }
            // 下
            if (cur.y < Height - 1)
            {
                if (checkPos(cur, 0, 1)) { break; }
            }
            // 左
            if (cur.x > 0)
            {
                if (checkPos(cur, -1, 0)) { break; }
            }
            // 右
            if (cur.x < Width - 1)
            {
                if (checkPos(cur, 1, 0)) { break; }
            }


            for(int i = 0;i<Height;i++)
            {
                for(int j = 0;j<Width;j++)
                {
                    temp_map[i,j] = short.MaxValue;
                    if(astar_search[i,j] != null)
                    {
                        temp_map[i,j] = (short)astar_search[i,j].F;
                    }
                }
            }

            Refresh();
            yield return null;
        }

        StartCoroutine(visualPath());
    }

    bool checkPos(Pos cur,int dx,int dy)
    {
        var o_score = astar_search[cur.y+dy,cur.x+dx];
        if(o_score!=null && o_score.closed)
        {
            return false;
        }

        var cur_score = astar_search[cur.y,cur.x];
        Pos o_pos = new Pos(cur.x + dx,cur.y + dy);
        if(mapInstance.map[cur.y+dy,cur.x+dx] == 8)
        {
            var a = new AScore(cur_score.G + 1,0);
            a.parent = cur;
            astar_search[cur.y+dy,cur.x+dx] = a;
            Debug.Log("寻路完成。");
            return true;
        }
        if(mapInstance.map[cur.y+dy,cur.x+dx] == 0)
        {
            if(o_score == null)
            {// 在open表中没有该节点，计算该节点的h与g值，并把
                var a = new AScore(cur_score.G + 1,Pos.AStarDistance(o_pos,mapInstance.endPos));
                a.parent = cur;
                astar_search[cur.y + dy,cur.x +dx] = a;
                list.Add(o_pos);
            }
            else if(o_score.G > cur_score.G + 1)
            {// 在Open表中有该节点，比较两个x的G值，更新目标节点的G值。
                o_score.G = cur_score.G + 1;
                o_score.parent = cur;
                if(!list.Contains(o_pos))
                {// 如果检查列表中没有该节点，加入
                    list.Add(o_pos);
                }
            }
        }
        return false;
    }

    void Refresh()
	{
		GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
		foreach (var go in all_go)
		{
			Destroy(go);
		}
		for(int i = 0;i<Height;i++)
		{
			for(int j = 0;j<Width;j++)
			{
				if (mapInstance.map[i,j]==0 && temp_map[i, j] != short.MaxValue)
                {
                    var go = Instantiate(mapInstance.prefabs[3], new Vector3(j * 1, 0.1f, i * 1), Quaternion.identity,mapInstance.GetWalls().transform);
                    go.tag = "Path";
                }

			}
		}
	}
    public override IEnumerator visualPath()
	{
		Pos p = mapInstance.endPos;

		while(!p.Equals(mapInstance.startPos))
        {
            var go = Instantiate(mapInstance.prefabs[4],new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, mapInstance.GetWalls().transform);
            p = astar_search[p.y, p.x].parent;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
	}

    public override void startSearch()
	{
		StartCoroutine(InitAStar());
	}
}
