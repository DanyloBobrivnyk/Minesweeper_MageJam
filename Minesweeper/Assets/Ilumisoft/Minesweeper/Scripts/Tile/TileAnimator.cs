using UnityEngine;

namespace Ilumisoft.Minesweeper
{
    [RequireComponent(typeof(Tile))]
    [RequireComponent(typeof(Animator))]
    public class TileAnimator : MonoBehaviour
    {
        Tile tile;

        Animator animator;

        private void Awake()
        {
            tile = GetComponent<Tile>();
            animator = GetComponent<Animator>();

            tile.OnStateChanged += OnTileStateChanged;
        }

        private void OnTileStateChanged(TileState state)
        {
            switch (state)
            {
                case TileState.Hidden:
                    animator.SetTrigger("Unlock");
                    animator.SetTrigger("Unflag");
                    break;
                case TileState.Flagged:
                    animator.SetTrigger("Flag");
                    break;
                case TileState.Locked:
                    animator.SetTrigger("Lock");
                    break;
                case TileState.Revealed:
                    animator.SetTrigger("Reveal");
                    break;
            }
        }
    }
}