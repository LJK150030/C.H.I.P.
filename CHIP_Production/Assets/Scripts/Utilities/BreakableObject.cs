using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BreakableObject : MonoBehaviour {
    public GameObject breakEffectPrefab;
    public GameObject WallCollider;
    public AudioClip[] SFX_breaking;
    private BoxCollider2D trigger;

	void Start () {
        trigger = GetComponent<BoxCollider2D>();
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Animator anim = collision.GetComponentInParent<Animator>();
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if(anim.GetBool("IsDucking"))
            {
                Vector2 dir = trigger.bounds.ClosestPoint(collision.transform.position) - collision.transform.position;
                float angle = Vector2.Angle(dir, rb.velocity);
                if (rb.velocity.magnitude > 0 && Mathf.Abs(angle - 90) < 20f)
                {
                    WallCollider.SetActive(true);
                    
                }
                else
                {
                    int randomNumber = Random.Range(0, SFX_breaking.Length);
                    breakEffectPrefab.GetComponent<AudioSource>().clip = SFX_breaking[randomNumber];
                    Instantiate(breakEffectPrefab, transform.position, transform.rotation);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                WallCollider.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            WallCollider.SetActive(false);
        }
    }
}
