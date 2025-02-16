using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float cloneDuration;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float colorLosingSpeed;
    [SerializeField] private bool canAttack;


    public void CreateClone(Transform _clonePosition,Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,colorLosingSpeed,canAttack,_offset,FindClosestEnemy(newClone.transform));
    }
}
