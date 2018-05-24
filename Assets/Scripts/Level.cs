using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is the base architecture for future level creation
public class Level : MonoBehaviour {

    public int ID { get; set; }
    public string LevelName { get; set;}
    public bool Completed { get; set; }
    public bool Locked { get; set; }

    public Level(int id, string levelName, bool completed, bool locked)
    {
        this.ID = id;
        this.LevelName = levelName;
        this.Completed = completed;
        this.Locked = locked;
    }

    public void Complete()
    {
        this.Completed = true;
    }

    public void Lock()
    {
        this.Locked = true;
    }

    public void Unlock()
    {
        this.Locked = false;
    }
}
