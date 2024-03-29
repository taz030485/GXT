using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Echo : MonoBehaviour
{
    public Material solidMaterial;
    public Material transparentMaterial;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator animator;
    public AudioSource audioSource;
    static bool playingAudio = false;

    List<ActionsManager.Action> actions;

    Grid grid;

    Vector3Int lastPosition = Vector3Int.zero;
    Vector3Int startPosition = Vector3Int.zero;

    private void OnEnable()
    {
        ActionsPlayer.OnPlayAction += PlayAction;
        ActionsPlayer.OnResetActions += ResetActions;
    }

    private void OnDisable()
    {
        ActionsPlayer.OnPlayAction -= PlayAction;
        ActionsPlayer.OnResetActions -= ResetActions;
    }

    void ResetActions()
    {
        ReturnToStart();
    }

    void PlayAction(int index, float speed)
    {        
        if (index >= actions.Count) return;

        ActionsManager.Action action = actions[index];
        Vector3 target = CellToWorld(action.position);
        transform.LookAt(target);

        switch (action.type)
        {
            case ActionsManager.ActionType.Move:
                StartCoroutine(MoveOverSeconds(target, speed));
                break;
            case ActionsManager.ActionType.Attack:
                //Attack
                StartCoroutine(Attack(action.position, speed));
                break; 
            default:
                break;
        }
    }

    public IEnumerator Attack(Vector3Int target, float seconds)
    {
        Enemy enemy = GridManager.GetEnemy(target);
        if (enemy != null && enemy.IsAlive)
        {
            animator.SetTrigger("Attack");
            if (!playingAudio)
            {
                StartCoroutine(PlayAudio());
            }
            yield return new WaitForSeconds(seconds / 2);
            enemy.TakeDamage(Player.Damage);
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(1.0f);
            animator.SetTrigger("Idle");
        }
    }

    public IEnumerator PlayAudio()
    {
        playingAudio = true;
        audioSource.Play();
        yield return new WaitForSeconds(0.75f);
        playingAudio = false;
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
        if (target == CellToWorld(lastPosition))
        {
            BecomeSolid();
        }
        animator.SetTrigger("Idle");
    }

    public Vector3Int SetActions(List<ActionsManager.Action> actions)
    {
        this.actions = new(actions);
        foreach (var action in actions)
        {
            if (action.type == ActionsManager.ActionType.Start)
            {
                startPosition = action.position;
            }

            if (action.type != ActionsManager.ActionType.Attack)
            {
                lastPosition = action.position;
            }
        }

        ReturnToStart();

        return lastPosition;
    }

    public void SetGrid(Grid grid)
    {
        this.grid = grid;
    }

    void ReturnToStart()
    {
        transform.position = CellToWorld(startPosition);
        transform.LookAt(CellToWorld(actions[1].position));
        BecomeTransparent();
    }

    void BecomeSolid()
    {
        skinnedMeshRenderer.material = solidMaterial;
    }

    void BecomeTransparent()
    {
        skinnedMeshRenderer.material = transparentMaterial;
    }

    Vector3 CellToWorld(Vector3Int node)
    {
        return grid.CellToWorld(node);
    }
}
