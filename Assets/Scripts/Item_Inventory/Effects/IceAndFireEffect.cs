using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Data/Item Effect/Ice and Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform _spondPosition)
    {
        Transform player = PlayerManager.instance.player.transform;
        bool thirdAttack=player.GetComponent<Player>().primaryAttack.comboCounter == 2;
        
        if (thirdAttack)
        {

            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _spondPosition.position, player.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity* player.GetComponent<Player>().facingDir,0); 
            Destroy(newIceAndFire, 10);
        }
    }
}
