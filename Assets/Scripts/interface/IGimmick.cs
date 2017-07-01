namespace Komugi.Gimmick
{
    interface IGimmick
    {
        GimmickData Data { get; set; }

        bool ClearFlag { get; set; }

        System.Action OpenAction { get; set; }

        /// <summary>
        /// ギミック解除
        /// </summary>
        void RescissionGimmick();
    }
}
