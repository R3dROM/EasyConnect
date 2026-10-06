using EasyConnect.Services;
using System.Diagnostics;

namespace EasyConnect.Controllers
{
    public class DeployController(
        DeploymentService _deploymentService)
    {
        private bool _inAction = false;
        public bool InAction => _inAction;
        public event Action<bool>? ActionStateChanged;
        private void SetInAction(bool value)
        {
            if (_inAction == value)
                return;
            
            _inAction = value;
            ActionStateChanged?.Invoke(value);
        }
        private async Task ExecuteAction(
            Func<Task> action)
        {
            if (InAction)
                throw new InvalidOperationException(
                    "Another action is already running");
            try
            {
                SetInAction(true);
                await action();
            }
            finally
            {
                SetInAction(false);
            }
        }
        public async Task StartUninstall(int maxDevices)
        {
            await ExecuteAction(() => _deploymentService.StartUninstall(maxDevices));
        }
        public async Task StartExperience(int maxDevices)
        {
            await ExecuteAction(() => _deploymentService.StartExperience(maxDevices));
        }
        public async Task StartDeployment(int maxDevices)
        {
            await ExecuteAction(async() =>
            {
                await _deploymentService.StartDeployment(maxDevices);
            });
        }
        public async Task StopDeployment()
        {
            try
            {
                await _deploymentService.StopDeployment();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EXCEPTION ON DEPLOYMENT APP: {ex.Message}");
                throw;
            }
        }
    }
}
