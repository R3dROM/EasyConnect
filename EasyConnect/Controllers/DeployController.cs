using EasyConnect.Services;
using System.Diagnostics;

namespace EasyConnect.Controllers
{
    public class DeployController(
        NetworkService _networkService,
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

            SetInAction(true);

            try
            {
                await action();
            }
            finally
            {
                SetInAction(false);
            }
        }
        public Task StartUninstall(int maxDevices)
        {
            return ExecuteAction(() => _deploymentService.StartUninstall(maxDevices));
        }
        public Task StartExperience(int maxDevices)
        {
            return ExecuteAction(() => _deploymentService.StartExperience(maxDevices));
        }
        public Task StartDeployment(int maxDevices)
        {
            return ExecuteAction(async() =>
            {
                await _networkService.GenerateNetworkingConfigurationJson();
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
