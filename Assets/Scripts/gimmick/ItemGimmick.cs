using System;
using UnityEngine;

namespace Komugi.Gimmick
{
    public class ItemGimmick : GimmickBase, IGimmick
    {
        public GimmickData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public bool ClearFlag
        {
            get
            {
                return clearflag;
            }

            set
            {
                clearflag = value;
                if (clearflag) { RescissionGimmick(); }
            }
        }

        public bool CheckClearConditions(int itemId)
        {
            return data.gimmickAnswer == itemId;
        }

        public void RescissionGimmick()
        {
            closeObject.SetActive(false);
            openObject.SetActive(true);
        }
    }
}