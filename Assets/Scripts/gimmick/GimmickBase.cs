using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Komugi.Gimmick
{
    public class GimmickBase : MonoBehaviour
    {
        [SerializeField]
        protected GameObject closeObject;

        [SerializeField]
        protected GameObject openObject;

        protected GimmickData data;

        protected bool clearflag;
    }
}