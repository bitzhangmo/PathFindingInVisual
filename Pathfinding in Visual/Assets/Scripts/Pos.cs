using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pos {
// 数据类，用来存放vector2数据
	public int x = 0;
	public int y = 0;

	public Pos() {}
	public Pos(Pos p)
	{
		x = p.x;
		y = p.y;
	}
	public Pos(int x,int y )
	{
		this.x = x;
		this.y = y;
	}
	public static float AStarDistance(Pos p1, Pos p2)
    {
        float d1 = Mathf.Abs(p1.x - p2.x);
        float d2 = Mathf.Abs(p1.y - p2.y);
        return d1 + d2;
    }

	public bool Equals(Pos p)
	{
		return p.x == x && p.y == y;
	}
}

public class AScore
{
	    // G是从起点出发的步数
    public float G = 0;
    // H是估算的离终点距离
    public float H = 0;

    public bool closed = false;

    public Pos parent = null;

    public AScore(float g, float h)
    {
        G = g;		
        H = h;		
        closed = false;		// 开闭
    }

    public float F
    {// 估值函数
        get { return G + H; }
    }

    public int CompareTo(AScore a2)
    {
        if (F == a2.F)
        {
            return 0;
        }
        if (F > a2.F)
        {
            return 1;
        }
        return -1;
    }

    public bool Equals(AScore a)
    {
        if (a.F == F)
        {
            return true;
        }
        return false;
    }
}