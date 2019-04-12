using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class DFS : AlgoVisual {

	// dfs表
	int[,] dfs = new int[Height,Width];
	public int curdepth;
	List<Pos> quene = new List<Pos>();
	string allbfs; 
	IEnumerator initDFS()
	{
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				dfs[i,j] = short.MaxValue;
			}
		}
		dfs[mapInstance.startPos.y,mapInstance.startPos.x] = 0;
		quene.Add(mapInstance.startPos);
		while(quene.Count>0)
		{
			int min_i = 0;
			Pos cur = quene[min_i];
			float min_dist = Pos.AStarDistance(cur,mapInstance.endPos);
			for(int i = 0;i<quene.Count;i++)
			{
				float d = Pos.AStarDistance(quene[i],mapInstance.endPos);
				if(d < min_dist)
				{
					min_i = i;
					cur = quene[i];
					min_dist = d;
				}
			}

			quene.RemoveAt(min_i);

			int last_Count = quene.Count;
			if(cur.y > 0)
			{
				if(checkPos(cur,0,-1,0)) { break; }
			}
			if(cur.y < Height-1)
			{
				if(checkPos(cur,0,1,1)) { break; }
			}
			if(cur.x > 0)
			{
				if(checkPos(cur,-1,0,2)) { break; }
			}
			if(cur.x < Width - 1)
			{
				if(checkPos(cur,1,0,3)) { break; }
			}
			Refresh();
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
 		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				allbfs+=dfs[i,j].ToString()+' ';
			}
			allbfs+='\n';
		} 
		// Refresh();
		System.IO.File.WriteAllText(Application.dataPath + "//" + "dfsMap.txt", allbfs, Encoding.UTF8);
		StartCoroutine(visualPath());
	}

	public bool checkPos(Pos cur,int dx,int dy,int dir)
	{
		if(mapInstance.map[cur.y + dy,cur.x + dx] == 8 )
		{
			dfs[cur.y + dy,cur.x + dx] = (short)(dfs[cur.y,cur.x] + 1);
			Debug.Log("寻路完成。");
			return true;
		}
		if(mapInstance.map[cur.y + dy,cur.x + dx] == 0)
		{
			if(dfs[cur.y + dy,cur.x + dx]>(short)(dfs[cur.y,cur.x] + 1))
			{
				dfs[cur.y + dy,cur.x + dx] = (short)(dfs[cur.y,cur.x] + 1);
				quene.Add(new Pos(cur.x + dx,cur.y + dy));
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
				if (mapInstance.map[i,j]==0 && dfs[i, j] != short.MaxValue)
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

		while(true)
		{
			if(mapInstance.endPos == null)
			{
				Debug.Log("无对应路径存在！");
				break;
			}
			int cur_step = dfs[p.y,p.x];
			if(cur_step == 0)
				break;
			if (p.y>0 && dfs[p.y-1, p.x] == cur_step - 1)
            {
                p.y -= 1;
            }
            else if (p.y<dfs.GetLength(0) && dfs[p.y+1, p.x] == cur_step - 1)
            {
                p.y += 1;
            }
            else if (p.x>0 && dfs[p.y, p.x-1] == cur_step - 1)
            {
                p.x -= 1;
            }
            else if (p.x>0 && dfs[p.y, p.x+1] == cur_step - 1)
            {
                p.x += 1;
            }
			if (!p.Equals(mapInstance.startPos))
            {
                var go = Instantiate(mapInstance.prefabs[4], new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, mapInstance.GetWalls().transform);
                yield return new WaitForSeconds(0.1f);
            }
		}
		yield return null;
	}
	public override void startSearch()
	{
		StartCoroutine(initDFS());
	}
}
