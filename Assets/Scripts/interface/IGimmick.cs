namespace Komugi.Gimmick
{
    interface IGimmick
    {
        GimmickData Data { get; set; }

        bool ClearFlag { get; set; }

        bool CheckClearConditions(int itemId);

        // ギミック解除
        void RescissionGimmick();
    }
}
