using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Radix.Blazor
{
    public class RouterComponent : IComponent, IDisposable
    {
        private bool _navigationInterceptionEnabled;
        private RenderHandle _renderHandle;
        [Inject]private NavigationManager NavigationManager { get; set; }
        [Inject]private INavigationInterception NavigationInterception { get; set; }
        [Inject]private Dictionary<string, View> UriComponentMap { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;

            NavigationManager.LocationChanged += OnLocationChanged;

        }

        /// <summary>
        ///     Performs no operation. This is used by blazor to pass down parameters in razor / markup. We don't use that here
        ///     We can provide parameters to child components by setting basic properties.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task SetParametersAsync(ParameterView parameters)
        {
            Console.Out.WriteLine("SetParametersAsync");
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask; ;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(e.Location);

            Console.Out.WriteLine("OnLocationChanged");

            _renderHandle.Render(builder =>
            {
                View currentComponent = UriComponentMap[relativeUri];
                Rendering.RenderNode(currentComponent, builder, 0, currentComponent.Render());
            });
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
                return;

            if (disposing)
                NavigationManager.LocationChanged -= OnLocationChanged;

            disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public Task OnAfterRenderAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
