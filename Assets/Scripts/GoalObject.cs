using UnityEngine;
using System.Collections;

public class GoalObject : ClickableObject {

    public Game game;
    public Player player;
    public int[] goalCost;
    public bool currentTarget = false;
    public Material material;

    private bool alive = true;
    private GameObject manaHand, selectionMarker;
    private ManaPayment manaPayment;

    void Awake()
    {
        manaHand = GameObject.Find("Hand");
        manaPayment = manaHand.GetComponent<ManaPayment>();
        selectionMarker = GameObject.Find("SelectionMarker");

        GetComponent<MeshRenderer>().material = material;
    }

    public override void ClickAction()
    {

        if (alive)
        {
            switch (game.state)
            {
                case Game.State.IDLE:
                    // Check if adjacent to player
                    if ((player.transform.position - transform.parent.position).sqrMagnitude < player.moveDistance)
                    {
                        game.state = Game.State.PAYING;
                        selectionMarker.transform.position = transform.position;
                        selectionMarker.SetActive(true);

                        manaPayment.SetCost(goalCost, new int[3] { 0, 0, 0 }, gameObject);
                        currentTarget = true;
                    }
                    break;
                case Game.State.PAYING:
                    if (currentTarget)
                    {
                        game.state = Game.State.IDLE;
                        currentTarget = false;
                        manaPayment.Reset();
                        selectionMarker.SetActive(false);
                    }
                    break;
                case Game.State.GOAL:
                case Game.State.WON:
                case Game.State.LOST:
                    break;
            }
        }
    }
}
