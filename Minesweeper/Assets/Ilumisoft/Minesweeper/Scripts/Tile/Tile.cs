using UnityEngine;
using UnityEngine.Events;

namespace Ilumisoft.Minesweeper
{
    public class Tile : MonoBehaviour
    {
        public UnityAction<TileState> OnStateChanged { get; set; } = null;

        public TileState State { get; private set; }

        public virtual void Reveal()
        {
            FindObjectOfType<AudioManager>().Play("Square Open One");
            State = TileState.Revealed;
            OnStateChanged?.Invoke(State);
        }
        public void Flag()
        {
            State = TileState.Flagged;
            OnStateChanged?.Invoke(State);
        }

        public void Unflag()
        {
            State = TileState.Hidden;
            OnStateChanged?.Invoke(State);
        }

        public void Lock()
        {
            State = TileState.Locked;
            OnStateChanged?.Invoke(State);
        }
        public void Unlock()
        {
            State = TileState.Hidden;
            OnStateChanged?.Invoke(State);
        }

        public void SwitchLock()
        {
            if (State != TileState.Revealed)
            {
                if (State == TileState.Locked)
                {
                    Unlock();
                }
                else
                {
                    Lock();
                }
            }
        }
        public void SwitchFlag()
        {
            FindObjectOfType<AudioManager>().Play("Square Open Two");
            if (State != TileState.Revealed)
            {
                if (State == TileState.Flagged)
                {
                    Unflag();
                }
                else
                {
                    Flag();
                }
            }
        }
    }
}