using BowlingPainelOnBlazor.Data;

namespace Microsoft.AspNetCore.Components.Web
{
    public class ToastBase : ComponentBase
    {
        [Inject] ToastService ToastService { get; set; }

        protected string Heading { get; set; }
        protected string Message { get; set; }
        protected bool IsVisible { get; set; }
        protected string BackgroundCssClass { get; set; }
        protected string IconCssClass { get; set; }

        protected override void OnInitialized()
        {
            ToastService.OnShow += ShowToast;
            ToastService.OnHide += HideToast;
        }

        private void ShowToast(string message, ToastLevel level)
        {
            BuildToastSettings(level, message);
            IsVisible = true;
            InvokeAsync(StateHasChanged);
        }

        private void HideToast()
        {
            IsVisible = false;
            InvokeAsync(StateHasChanged);
        }

        private void BuildToastSettings(ToastLevel level, string message)
        {
            switch (level)
            {
                default: //ToastLevel.Info:
                    BackgroundCssClass = "bg-info";
                    IconCssClass = "info";
                    Heading = "Info";
                    break;
                case ToastLevel.Success:
                    BackgroundCssClass = "bg-success";
                    IconCssClass = "check";
                    Heading = "Success";
                    break;
                case ToastLevel.Warning:
                    BackgroundCssClass = "bg-warning";
                    IconCssClass = "warning";
                    Heading = "Warning";
                    break;
                case ToastLevel.Error:
                    BackgroundCssClass = "bg-danger";
                    IconCssClass = "bug";
                    Heading = "Error";
                    break;
            }

            Message = message;
        }

        ~ToastBase()
        {
            ToastService.OnShow -= ShowToast;
        }
    }
}