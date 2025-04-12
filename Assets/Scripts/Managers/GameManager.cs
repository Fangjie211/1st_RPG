using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour,ISavemanager
{
    public static GameManager instance;

    [SerializeField] private CheckPoints[] checkPoints;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        checkPoints = FindObjectsOfType<CheckPoints>();
    }

    private void Start()
    {
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,bool> pair in _data.checkpoints)
        {
            foreach (CheckPoints checkPoint in checkPoints)
            {
                if (checkPoint.id == pair.Key)
                {
                    checkPoint.activated = pair.Value;
                    checkPoint.GetComponent<Animator>().SetBool("active", pair.Value);
                }
            }
        }
        foreach (CheckPoints checkpoint in checkPoints)
        {
            if (_data.closestCheckPointId==checkpoint.id)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {

        _data.closestCheckPointId = FindClosestCheckPoint().id;
        _data.checkpoints.Clear();
        foreach (CheckPoints checkPoint in checkPoints)
        {
            _data.checkpoints.Add(checkPoint.id, checkPoint.activated);
        }

    }


    private CheckPoints FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoints closestCheckPoint = null;

        foreach (var checkPoint in checkPoints)
        {
            float distance = Vector3.Distance(checkPoint.transform.position, PlayerManager.instance.player.transform.position);
            if (distance < closestDistance&&checkPoint.activated)
            {
                closestDistance = distance;
                closestCheckPoint = checkPoint;
            }
        }
        return closestCheckPoint;
    }
}
