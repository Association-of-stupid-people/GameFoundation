namespace Mech.Network.HttpRequest
{
    using GameFoundation.Scripts.BlueprintFlow.BlueprintControlFlow;
    using GameFoundation.Scripts.Network.WebService;
    using GameFoundation.Scripts.Utilities.LogService;
    using MechSharingCode.WebService.Blueprint;

    /// <summary>
    /// Get blueprint download link from server
    /// </summary>
    public class BlueprintDownloadRequest : BaseHttpRequest<GetBlueprintResponseData>
    {
        #region zenject

        private readonly BlueprintReaderManager blueprintReaderManager;

        #endregion
        
        public BlueprintDownloadRequest(ILogService logger, BlueprintReaderManager blueprintReaderManager) : base(logger)
        {
            this.blueprintReaderManager = blueprintReaderManager;
        }

        public override void Process(GetBlueprintResponseData responseDataData)
        {
            this.Logger.Log($"Blueprint download link: {responseDataData.Url}");
            this.blueprintReaderManager.LoadBlueprint(responseDataData.Url, responseDataData.Hash);
        }
    }
}
