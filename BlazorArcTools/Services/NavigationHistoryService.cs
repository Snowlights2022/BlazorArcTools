using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.Generic;

namespace BlazorArcTools.Services
{
    public class NavigationHistoryService : IDisposable
    {
        private readonly NavigationManager _nav;
        private readonly List<string> _stack = new();

        public NavigationHistoryService(NavigationManager nav)
        {
            _nav = nav;
            _stack.Add(_nav.Uri);
            _nav.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            var uri = e.Location;
            if (_stack.Count == 0 || _stack[^1] != uri)
            {
                _stack.Add(uri);
            }
        }

        public bool GoBack()
        {
            if (_stack.Count <= 1)
                return false;

            _stack.RemoveAt(_stack.Count - 1);
            var prev = _stack[^1];
            _nav.NavigateTo(prev, forceLoad: false);
            return true;
        }

        public void Dispose()
        {
            _nav.LocationChanged -= OnLocationChanged;
        }
    }
}
