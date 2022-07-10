using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Interaction.Components;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Web.Components;

public static class on
{
    public static Attribute @event<T>(NodeId nodeId, string name, Action<T> callback) where T : EventArgs =>
        new ExplicitAttribute
        (
            nodeId,
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            }
        );

    public static Attribute focus(NodeId nodeId, Action<FocusEventArgs> callback) => @event(nodeId, nameof(focus), callback);

    public static Attribute blur(NodeId nodeId, Action<FocusEventArgs> callback) => @event(nodeId, nameof(blur), callback);

    public static Attribute focusin(NodeId nodeId, Action<FocusEventArgs> callback) => @event(nodeId, nameof(focusin), callback);

    public static Attribute focusout(NodeId nodeId, Action<FocusEventArgs> callback) => @event(nodeId, nameof(focusout), callback);

    public static Attribute mouseover(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(mouseover), callback);

    public static Attribute mouseout(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(mouseout), callback);

    public static Attribute mousemove(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(mousemove), callback);

    public static Attribute mousedown(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(mousedown), callback);

    public static Attribute click(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(click), callback);

    public static Attribute dblclick(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(dblclick), callback);

    public static Attribute wheel(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(wheel), callback);

    public static Attribute mousewheel(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(mousewheel), callback);

    public static Attribute contextmenu(NodeId nodeId, Action<MouseEventArgs> callback) => @event(nodeId, nameof(contextmenu), callback);

    public static Attribute drag(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(drag), callback);

    public static Attribute dragenter(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(dragenter), callback);

    public static Attribute dragleave(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(dragleave), callback);

    public static Attribute dragover(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(dragover), callback);

    public static Attribute dragstart(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(dragstart), callback);

    public static Attribute drop(NodeId nodeId, Action<DragEventArgs> callback) => @event(nodeId, nameof(drop), callback);

    public static Attribute keydown(NodeId nodeId, Action<KeyboardEventArgs> callback) => @event(nodeId, nameof(keydown), callback);

    public static Attribute keyup(NodeId nodeId, Action<KeyboardEventArgs> callback) => @event(nodeId, nameof(keyup), callback);

    public static Attribute keypress(NodeId nodeId, Action<KeyboardEventArgs> callback) => @event(nodeId, nameof(keypress), callback);

    public static Attribute change(NodeId nodeId, Action<ChangeEventArgs> callback) => @event(nodeId, nameof(change), callback);

    public static Attribute input(NodeId nodeId, Action<ChangeEventArgs> callback) => @event(nodeId, nameof(input), callback);

    public static Attribute reset(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(reset), callback);

    public static Attribute select(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(select), callback);

    public static Attribute selectstart(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(selectstart), callback);

    public static Attribute selectionchange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(selectionchange), callback);

    public static Attribute submit(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(submit), callback);

    public static Attribute beforecopy(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(beforecopy), callback);

    public static Attribute beforecut(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(beforecut), callback);

    public static Attribute beforepaste(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(beforepaste), callback);

    public static Attribute copy(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(copy), callback);

    public static Attribute paste(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(paste), callback);

    public static Attribute touchcancel(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchcancel), callback);

    public static Attribute touchend(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchend), callback);

    public static Attribute touchmove(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchmove), callback);

    public static Attribute touchstart(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchstart), callback);

    public static Attribute touchenter(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchenter), callback);

    public static Attribute touchleave(NodeId nodeId, Action<TouchEventArgs> callback) => @event(nodeId, nameof(touchleave), callback);

    public static Attribute pointercapture(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointercapture), callback);

    public static Attribute lostpointercapture(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(lostpointercapture), callback);

    public static Attribute pointercancel(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointercancel), callback);

    public static Attribute pointerdown(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerdown), callback);

    public static Attribute pointerenter(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerenter), callback);

    public static Attribute pointerleave(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerleave), callback);

    public static Attribute pointermove(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointermove), callback);

    public static Attribute pointerout(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerout), callback);

    public static Attribute pointerover(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerover), callback);

    public static Attribute pointerup(NodeId nodeId, Action<PointerEventArgs> callback) => @event(nodeId, nameof(pointerup), callback);

    public static Attribute canplay(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(canplay), callback);

    public static Attribute canplaythrough(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(canplaythrough), callback);

    public static Attribute cuechange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(cuechange), callback);

    public static Attribute durationchange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(durationchange), callback);

    public static Attribute emptied(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(emptied), callback);

    public static Attribute pause(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(pause), callback);

    public static Attribute play(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(play), callback);

    public static Attribute playing(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(playing), callback);

    public static Attribute ratechange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(ratechange), callback);

    public static Attribute seeked(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(seeked), callback);

    public static Attribute seeking(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(seeking), callback);

    public static Attribute stalled(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(stalled), callback);

    public static Attribute stop(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(stop), callback);

    public static Attribute suspend(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(suspend), callback);

    public static Attribute timeupdate(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(timeupdate), callback);

    public static Attribute volumechange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(volumechange), callback);

    public static Attribute waiting(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(waiting), callback);

    public static Attribute loadstart(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(loadstart), callback);

    public static Attribute timeout(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(timeout), callback);

    public static Attribute abort(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(abort), callback);

    public static Attribute load(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(load), callback);

    public static Attribute loadend(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(loadend), callback);

    public static Attribute progress(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(progress), callback);

    public static Attribute error(NodeId nodeId, Action<ProgressEventArgs> callback) => @event(nodeId, nameof(error), callback);

    public static Attribute activate(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(activate), callback);

    public static Attribute beforeactivate(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(beforeactivate), callback);

    public static Attribute beforedeactivate(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(beforedeactivate), callback);

    public static Attribute deactivate(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(deactivate), callback);

    public static Attribute ended(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(ended), callback);

    public static Attribute fullscreenchange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(fullscreenchange), callback);

    public static Attribute fullscreenerror(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(fullscreenerror), callback);

    public static Attribute loadeddata(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(loadeddata), callback);

    public static Attribute loadedmetadata(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(loadedmetadata), callback);

    public static Attribute pointerlockchange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(pointerlockchange), callback);

    public static Attribute pointerlockerror(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(pointerlockerror), callback);

    public static Attribute readystatechange(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(readystatechange), callback);

    public static Attribute scroll(NodeId nodeId, Action<EventArgs> callback) => @event(nodeId, nameof(scroll), callback);

    public static class task
    {

        public static Attribute @event<T>(NodeId nodeId, string name, Func<T, Task> callback) where T : EventArgs => new ExplicitAttribute(
            nodeId,
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            });

        public static Attribute focus(NodeId nodeId, Func<FocusEventArgs, Task> callback) => @event(nodeId, nameof(focus), callback);

        public static Attribute blur(NodeId nodeId, Func<FocusEventArgs, Task> callback) => @event(nodeId, nameof(blur), callback);

        public static Attribute focusin(NodeId nodeId, Func<FocusEventArgs, Task> callback) => @event(nodeId, nameof(focusin), callback);

        public static Attribute focusout(NodeId nodeId, Func<FocusEventArgs, Task> callback) => @event(nodeId, nameof(focusout), callback);

        public static Attribute mouseover(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(mouseover), callback);

        public static Attribute mouseout(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(mouseout), callback);

        public static Attribute mousemove(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(mousemove), callback);

        public static Attribute mousedown(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(mousedown), callback);

        public static Attribute click(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(click), callback);

        public static Attribute dblclick(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(dblclick), callback);

        public static Attribute wheel(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(wheel), callback);

        public static Attribute mousewheel(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(mousewheel), callback);

        public static Attribute contextmenu(NodeId nodeId, Func<MouseEventArgs, Task> callback) => @event(nodeId, nameof(contextmenu), callback);

        public static Attribute drag(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(drag), callback);

        public static Attribute dragenter(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(dragenter), callback);

        public static Attribute dragleave(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(dragleave), callback);

        public static Attribute dragover(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(dragover), callback);

        public static Attribute dragstart(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(dragstart), callback);

        public static Attribute drop(NodeId nodeId, Func<DragEventArgs, Task> callback) => @event(nodeId, nameof(drop), callback);

        public static Attribute keydown(NodeId nodeId, Func<KeyboardEventArgs, Task> callback) => @event(nodeId, nameof(keydown), callback);

        public static Attribute keyup(NodeId nodeId, Func<KeyboardEventArgs, Task> callback) => @event(nodeId, nameof(keyup), callback);

        public static Attribute keypress(NodeId nodeId, Func<KeyboardEventArgs, Task> callback) => @event(nodeId, nameof(keypress), callback);

        public static Attribute change(NodeId nodeId, Func<ChangeEventArgs, Task> callback) => @event(nodeId, nameof(change), callback);

        public static Attribute input(NodeId nodeId, Func<ChangeEventArgs, Task> callback) => @event(nodeId, nameof(input), callback);

        public static Attribute reset(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(reset), callback);

        public static Attribute select(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(select), callback);

        public static Attribute selectstart(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(selectstart), callback);

        public static Attribute selectionchange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(selectionchange), callback);

        public static Attribute submit(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(submit), callback);

        public static Attribute beforecopy(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(beforecopy), callback);

        public static Attribute beforecut(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(beforecut), callback);

        public static Attribute beforepaste(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(beforepaste), callback);

        public static Attribute copy(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(copy), callback);

        public static Attribute paste(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(paste), callback);

        public static Attribute touchcancel(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchcancel), callback);

        public static Attribute touchend(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchend), callback);

        public static Attribute touchmove(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchmove), callback);

        public static Attribute touchstart(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchstart), callback);

        public static Attribute touchenter(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchenter), callback);

        public static Attribute touchleave(NodeId nodeId, Func<TouchEventArgs, Task> callback) => @event(nodeId, nameof(touchleave), callback);

        public static Attribute pointercapture(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointercapture), callback);

        public static Attribute lostpointercapture(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(lostpointercapture), callback);

        public static Attribute pointercancel(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointercancel), callback);

        public static Attribute pointerdown(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerdown), callback);

        public static Attribute pointerenter(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerenter), callback);

        public static Attribute pointerleave(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerleave), callback);

        public static Attribute pointermove(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointermove), callback);

        public static Attribute pointerout(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerout), callback);

        public static Attribute pointerover(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerover), callback);

        public static Attribute pointerup(NodeId nodeId, Func<PointerEventArgs, Task> callback) => @event(nodeId, nameof(pointerup), callback);

        public static Attribute canplay(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(canplay), callback);

        public static Attribute canplaythrough(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(canplaythrough), callback);

        public static Attribute cuechange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(cuechange), callback);

        public static Attribute durationchange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(durationchange), callback);

        public static Attribute emptied(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(emptied), callback);

        public static Attribute pause(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(pause), callback);

        public static Attribute play(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(play), callback);

        public static Attribute playing(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(playing), callback);

        public static Attribute ratechange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(ratechange), callback);

        public static Attribute seeked(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(seeked), callback);

        public static Attribute seeking(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(seeking), callback);

        public static Attribute stalled(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(stalled), callback);

        public static Attribute stop(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(stop), callback);

        public static Attribute suspend(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(suspend), callback);

        public static Attribute timeupdate(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(timeupdate), callback);

        public static Attribute volumechange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(volumechange), callback);

        public static Attribute waiting(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(waiting), callback);

        public static Attribute loadstart(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(loadstart), callback);

        public static Attribute timeout(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(timeout), callback);

        public static Attribute abort(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(abort), callback);

        public static Attribute load(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(load), callback);

        public static Attribute loadend(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(loadend), callback);

        public static Attribute progress(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(progress), callback);

        public static Attribute error(NodeId nodeId, Func<ProgressEventArgs, Task> callback) => @event(nodeId, nameof(error), callback);

        public static Attribute activate(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(activate), callback);

        public static Attribute beforeactivate(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(beforeactivate), callback);

        public static Attribute beforedeactivate(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(beforedeactivate), callback);

        public static Attribute deactivate(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(deactivate), callback);

        public static Attribute ended(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(ended), callback);

        public static Attribute fullscreenchange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(fullscreenchange), callback);

        public static Attribute fullscreenerror(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(fullscreenerror), callback);

        public static Attribute loadeddata(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(loadeddata), callback);

        public static Attribute loadedmetadata(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(loadedmetadata), callback);

        public static Attribute pointerlockchange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(pointerlockchange), callback);

        public static Attribute pointerlockerror(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(pointerlockerror), callback);

        public static Attribute readystatechange(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(readystatechange), callback);

        public static Attribute scroll(NodeId nodeId, Func<EventArgs, Task> callback) => @event(nodeId, nameof(scroll), callback);
    }
}
