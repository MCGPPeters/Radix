using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        CancellationTokenSource disposalTokenSource = new CancellationTokenSource();
        ClientWebSocket webSocket = new ClientWebSocket();
        string message = "Hello, websocket!";
        string log = "";

        protected override async Task OnInitializedAsync()
        {
            await webSocket.ConnectAsync(new Uri("wss://localhost:5001/ws"), disposalTokenSource.Token);
            await ReceiveLoop();

            await SendMessageAsync();
        }

        async Task SendMessageAsync()
        {
            log += $"Sending: {message}\n";
            var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes("Hello World!"));
            await webSocket.SendAsync(dataToSend, WebSocketMessageType.Binary, true, disposalTokenSource.Token);
        }

        async Task ReceiveLoop()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            while (!disposalTokenSource.IsCancellationRequested)
            {
                // Note that the received block might only be part of a larger message. If this applies in your scenario,
                // check the received.EndOfMessage and consider buffering the blocks until that property is true.
                // Or use a higher-level library such as SignalR.
                var received = await webSocket.ReceiveAsync(buffer, disposalTokenSource.Token);
                var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
                log += $"Received: {receivedAsText}\n";
                StateHasChanged();
            }
        }

        public async void Dispose()
        {
            disposalTokenSource.Cancel();
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
        }
    }



    public class IndexViewModel : ViewModel
    {
        public IEnumerable<Error> Errors { get; set; }
    }

    public record ChatCommand
    {
    }

    public record ChatEvent
    {
    }
}
