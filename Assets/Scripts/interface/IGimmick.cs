namespace Komugi.Gimmick
{
    interface IGimmick
    {
        bool ClearFlag { get; set; }

        int ClearItem { get; set; }

        bool CheckClearConditions(int itemId);

        // ギミック解除
        void RescissionGimmick();
    }
}
