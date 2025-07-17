using UnityEngine;

public class Transicion : MonoBehaviour
{
    [SerializeField] private Animator _transicion;

    private void Start()
    {
        _transicion = GetComponent<Animator>();
    }
    public void Transition(bool dead)
    {
        _transicion.SetBool("Start", dead);
    }
}
