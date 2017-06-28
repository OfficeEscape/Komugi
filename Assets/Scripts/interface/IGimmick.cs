namespace Komugi.Gimmick
{
    interface IGimmick
    {
        GimmickData Data { get; set; }

        bool ClearFlag { get; set; }

        System.Action OpenAction { get; set; }

        // ギミック解除
        void RescissionGimmick();
    }
}
