using System.Collections.Generic;
using UnityEngine;

public class TeleportPointManager : MonoBehaviour
{
    public static TeleportPointManager Instance;

    [HideInInspector]
    public List<Transform> teleportPoints = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        FindTeleportPoints();
    }

    private void FindTeleportPoints()
    {
        teleportPoints.Clear();
        GameObject[] points = GameObject.FindGameObjectsWithTag("TeleportPoint");

        foreach (GameObject point in points)
        {
            teleportPoints.Add(point.transform);
        }

        teleportPoints.Sort((a, b) => a.name.CompareTo(b.name)); // İsim sırasına göre sıralarsak kontrol kolay olur
    }
}