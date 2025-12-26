using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleSocket[] sockets;
    public UnityEvent onSolved;

    bool solved;

    public void CheckSolved()
    {
        if (solved) return;

        foreach (var s in sockets)
            if (s == null || !s.IsCorrectlyFilled())
                return;

        solved = true;
        Debug.Log("CAPTCHA SOLVED!");
        onSolved?.Invoke();
    }
}
