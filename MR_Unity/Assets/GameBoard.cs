using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBoard : MonoBehaviour
{
    private List<SnapPoint> snapPoints;
    public List<SnapPoint> SnapPoints { get { return snapPoints; } }
}