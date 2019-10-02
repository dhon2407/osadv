using UnityEngine;
using System.Collections;

namespace UI
{
    public abstract class Entity : MonoBehaviour
    {
        protected Manager itsOwner;
        public Manager Owner { set => itsOwner = value; }
    }
}