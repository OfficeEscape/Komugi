using UnityEngine;

namespace Komugi.Gimmick
{
    public class GimmickBase : MonoBehaviour
    {
        [SerializeField]
        protected GameObject closeObject = null;

        [SerializeField]
        protected GameObject openObject = null;

        protected GimmickData data;

        protected bool clearflag;
    }
}