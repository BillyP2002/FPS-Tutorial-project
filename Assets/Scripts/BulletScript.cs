using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + "!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit a wall!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Bottle"))
        {
            print("hit " + collision.gameObject.name + "!");
            collision.gameObject.GetComponent<Bottle>().Shatter();
            
            //We will not destroy the bullet on impact, it will be destroyed through lifetime or solid object 
        }
    }

    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        
        GameObject hole = Instantiate(
            GlobalReference.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
    }
}
