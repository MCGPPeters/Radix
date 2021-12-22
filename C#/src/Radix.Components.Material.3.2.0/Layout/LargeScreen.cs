using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Components.Html;

namespace Radix.Components.Material._3._2._0.Layout
{
    public class LargeScreen : Component<LargeScreenViewModel>
    {
        protected override Node View(LargeScreenViewModel currentViewModel) => throw new NotImplementedException();
    }

    public record LargeScreenViewModel : ViewModel
    {

    }
}
