using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{


    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> KeyCodeList;


    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public List<Transform> targets=new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale=Vector2.Lerp(transform.localScale,new Vector3(maxSize,maxSize), growSpeed*Time.deltaTime);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
            // targets.Add(collision.transform);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {


        if (KeyCodeList.Count <= 0)
        {
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKey = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        Debug.Log(chosenKey.ToString());
        KeyCodeList.Remove(chosenKey);

        BlackHoleHotKey_Controller newHotKeyScript = newHotKey.GetComponent<BlackHoleHotKey_Controller>();
        newHotKeyScript.SetupHotKey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);

}
