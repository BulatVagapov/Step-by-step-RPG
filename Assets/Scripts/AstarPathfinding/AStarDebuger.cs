using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarPathfinding))]
public class AStarDebuger : MonoBehaviour
{
    private AStarPathfinding _pathfinder;

    public Transform startTransform;
    public Transform endTransform;

    private Vector3 _startPostion;
    private Vector3 _endPosition;
    private List<MapLocation> _path = new List<MapLocation>();

    [SerializeField] private GameObject startObjPrefab;
    [SerializeField] private GameObject endObjPrefab;
    [SerializeField] private GameObject pathPObjPrefab;

    [SerializeField] private Material openMaterial;
    [SerializeField] private Material closedMAterial;
    [SerializeField] private Material pathPMaterial;


    private void SetStartAndEndPositions()
    {
        _startPostion = new Vector3(MapManager.map[0].x, 1, MapManager.map[0].z);

        int endPosition = Random.Range(1, MapManager.map.Count);

        _endPosition = new Vector3(MapManager.map[endPosition].x, 1, MapManager.map[endPosition].z);
    }

    public void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }

    void Awake()
    {
        _pathfinder = GetComponent<AStarPathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _startPostion = startTransform.position;
            _endPosition = endTransform.position;
            
            //SetStartAndEndPositions();
            RemoveAllMarkers();
            _pathfinder.BeginSearch(_startPostion, _endPosition);
            Instantiate(startObjPrefab, new Vector3(_pathfinder.StartNode.Position.x, 1, _pathfinder.StartNode.Position.z), Quaternion.identity);
            Instantiate(endObjPrefab, new Vector3(_pathfinder.EndNode.Position.x, 1, _pathfinder.EndNode.Position.z), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _pathfinder.SearchAllNodes();

            for (int i = 0; i < _pathfinder.Open.Count; i++)
            {
                GameObject marker = Instantiate(pathPObjPrefab, new Vector3(_pathfinder.Open[i].Position.x, 1, _pathfinder.Open[i].Position.z), Quaternion.identity);
                marker.GetComponent<Renderer>().material = openMaterial;
                TextMesh[] values = marker.GetComponentsInChildren<TextMesh>();
                values[0].text = "G:" + _pathfinder.Open[i].G.ToString("0.00");
                values[1].text = "H:" + _pathfinder.Open[i].H.ToString("0.00");
                values[2].text = "F:" + _pathfinder.Open[i].F.ToString("0.00");
            }

            for (int i = 0; i < _pathfinder.Closed.Count; i++)
            {
                GameObject marker = Instantiate(pathPObjPrefab, new Vector3(_pathfinder.Closed[i].Position.x, 1, _pathfinder.Closed[i].Position.z), Quaternion.identity);
                marker.GetComponent<Renderer>().material = closedMAterial;
                TextMesh[] values = marker.GetComponentsInChildren<TextMesh>();
                if (values.Length > 0)
                {
                    values[0].text = "G:" + _pathfinder.Closed[i].G.ToString("0.00");
                    values[1].text = "H:" + _pathfinder.Closed[i].H.ToString("0.00");
                    values[2].text = "F:" + _pathfinder.Closed[i].F.ToString("0.00");
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            _path.Clear();
            RemoveAllMarkers();
            _path = _pathfinder.GetPath();
            Debug.Log($"PathCount {_path.Count}");

            for (int i = 0; i < _path.Count; i++)
            {
                GameObject marker = Instantiate(pathPObjPrefab, new Vector3(_path[i].x, 1, _path[i].z), Quaternion.identity);
                marker.GetComponent<Renderer>().material = pathPMaterial;
                TextMesh[] values = marker.GetComponentsInChildren<TextMesh>();
                values[0].text = "G:" + _pathfinder.Closed[i].G.ToString("0.00");
                values[1].text = "H:" + _pathfinder.Closed[i].H.ToString("0.00");
                values[2].text = "F:" + _pathfinder.Closed[i].F.ToString("0.00");
            }
        }
    }
}
