namespace Komugi.Gimmick
{
    interface IGimmick
    {
        GimmickData Data { get; set; }

        int ClearFlag { get; set; }

        System.Action<int> OpenAction { get; set; }

        /// <summary>
        /// ギミック解除
        /// </summary>
        void ReleaseGimmick();
    }
}
