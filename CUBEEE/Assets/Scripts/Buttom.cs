using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttom : MonoBehaviour
{
    [SerializeField] protected LayerMask capasPermitidas;
    [SerializeField] protected List<string> tagsPermitidos;

    protected int objetosDentro = 0;

    public virtual bool IsPressed => objetosDentro > 0;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (EsObjetoPermitido(other))
        {
            objetosDentro++;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (EsObjetoPermitido(other))
        {
            objetosDentro--;
        }
    }

    protected bool EsObjetoPermitido(Collider other)
    {
        bool layerOk = ((1 << other.gameObject.layer) & capasPermitidas) != 0;
        bool tagOk = tagsPermitidos.Contains(other.tag);
        return layerOk || tagOk;
    }
}
