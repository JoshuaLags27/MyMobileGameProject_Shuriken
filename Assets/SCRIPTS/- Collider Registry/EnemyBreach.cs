using UnityEngine;

public class EnemyBreach: MonoBehaviour
{
    public bool activePowerup;
  
    private Score score;
    private ChainHit chainhit;

    private void OnCollisionEnter(Collision other)
    {  
        NormalShurikenDetect(other);

        if(activePowerup == true)
        {
            ChainHit(other);
        }
    }
  
    // (ALWAYS ACTIVE) The Default Phase of the Shuriken to Level up
    void NormalShurikenDetect(Collision other)
    {
        score = GameObject.FindObjectOfType<Score>();

        if (activePowerup == false)
        {
            if (other.gameObject.GetComponent<Enemy_Movement>() != null)
            {
                score.LifeAmount -= 1;
                Destroy(other.gameObject);
            }
        }
    }

    // (IF ACTIVE METHOD) Checks if the Chain hit powerup is activated
    void ChainHit(Collision other)
    {
        chainhit = GameObject.FindObjectOfType<ChainHit>();

        if (chainhit.activate == true)
        {
            score.LifeAmount -= 1;
            
            Destroy(other.gameObject);
            chainhit.EnemiesTagged.Remove(other.gameObject);
          
            Destroy(chainhit.activeCrossMarkers[0].gameObject);
            chainhit.activeCrossMarkers.Remove(chainhit.activeCrossMarkers[0].gameObject);
        }
    }
}
