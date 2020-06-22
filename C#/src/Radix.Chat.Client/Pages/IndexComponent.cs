using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Elements;

namespace Radix.Chat.Client.Pages
{
    /// <inheritdoc />
    [Route("/")]
    public class IndexComponent : Component<IndexViewModel, ChatCommand, ChatEvent, Json>
    {
        protected override Update<IndexViewModel, ChatEvent> Update => throw new NotImplementedException();

        protected override Node View(IndexViewModel currentViewModel)
        {
            const string startLocalCameraScript = "var constraints = { audio: true, video: true };getMedia(constraints);";
            return concat(
                div(new[] {id("active-users")}, h3(new[] {id()}, text("Active users:"))),
                div(
                    new[] {id("video-chat")},
                    h2(new[] {id("")}, text("Select an active user")),
                    div(new[] {id("videos")}, video(new[] {id("remote-video")}), video(new[] {id("local-video")}))),
                script(Enumerable.Empty<IAttribute>(), text(startLocalCameraScript)));
        }
    }

    public class IndexViewModel : ViewModel
    {
        public IEnumerable<Error> Errors { get; set; }
    }

    public interface ChatCommand : IComparable, IComparable<ChatCommand>, IEquatable<ChatCommand>
    {
    }

    public interface ChatEvent : Event
    {
    }
}
