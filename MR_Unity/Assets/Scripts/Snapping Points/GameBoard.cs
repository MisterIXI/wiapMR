using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBoard : MonoBehaviour
{   
    private List<SnapPoint> snapPoints;

    public List<SnapPoint> GetSnapPoints() {
        return snapPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
