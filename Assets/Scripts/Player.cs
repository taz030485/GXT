using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    static Player manager = null;

    public Grid grid;

    static Vector3Int playerPosition = Vector3Int.zero;
    public static Vector3Int checkpoint = Vector3Int.zero;

    public TextMeshProUGUI text;
    static string baseText;   

    public Animator animator;

    static int damage = 1;
    public static int Damage
    {
        get
        {
            return damage;
        }
    }

    public static void DamageIncrease(int numDamage)
    {
        damage += numDamage;
        manager.text.text = string.Format(baseText, damage);
    }

    private void Awake()
    {
        manager = this;
        playerPosition = Vector3Int.zero;
        checkpoint = Vector3Int.zero;
        damage = 1;
        baseText = text.text;
        text.text = string.Format(baseText, damage);
    }

    public static void ResetTo(Vector3Int position)
    {
        manager.StopAllCoroutines();
        playerPosition = position;
        Vector3 target = manager.grid.CellToWorld(position);
        manager.transform.position = target;
        manager.transform.LookAt(target + Vector3.right);
    }

    public static void MoveTo(Vector3Int position)
    {
        playerPosition = position;
        Vector3 target = manager.grid.CellToWorld(position);
        manager.transform.LookAt(target);
        manager.StartCoroutine(manager.MoveOverSeconds(target, 1/ActionsPlayer.playspeed));
    }

    public IEnumerator MoveOverSeconds(Vector3 target, float seconds)
    {
	    float elapsedTime = 0;
	    Vector3 startingPos = transform.position;
        animator.SetTrigger("Run");
	    while (elapsedTime < seconds)
	    {
    		transform.position = Vector3.Lerp(startingPos, target, (elapsedTime / seconds));
		    elapsedTime += Time.deltaTime;
		    yield return new WaitForEndOfFrame();
	    }
	    transform.position = target;
        animator.SetTrigger("Idle");
    }

    public static bool PlayerUnderPointer(Vector3Int position)
    {
        if (position == playerPosition)
        {
            return true;
        }else{
            return false;
        }
    }
}
