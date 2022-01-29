using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Components;

namespace Radix.Web.Html.Data;

public static class on
{
    public static Attribute @event<T>(string name, Action<T> callback) where T : EventArgs =>
        new ExplicitAttribute
        (
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            }
        );

    public static Attribute focus(Action<FocusEventArgs> callback) => @event(nameof(focus), callback);

    public static Attribute blur(Action<FocusEventArgs> callback) => @event(nameof(blur), callback);

    public static Attribute focusin(Action<FocusEventArgs> callback) => @event(nameof(focusin), callback);

    public static Attribute focusout(Action<FocusEventArgs> callback) => @event(nameof(focusout), callback);

    public static Attribute mouseover(Action<MouseEventArgs> callback) => @event(nameof(mouseover), callback);

    public static Attribute mouseout(Action<MouseEventArgs> callback) => @event(nameof(mouseout), callback);

    public static Attribute mousemove(Action<MouseEventArgs> callback) => @event(nameof(mousemove), callback);

    public static Attribute mousedown(Action<MouseEventArgs> callback) => @event(nameof(mousedown), callback);

    public static Attribute click(Action<MouseEventArgs> callback) => @event(nameof(click), callback);

    public static Attribute dblclick(Action<MouseEventArgs> callback) => @event(nameof(dblclick), callback);

    public static Attribute wheel(Action<MouseEventArgs> callback) => @event(nameof(wheel), callback);

    public static Attribute mousewheel(Action<MouseEventArgs> callback) => @event(nameof(mousewheel), callback);

    public static Attribute contextmenu(Action<MouseEventArgs> callback) => @event(nameof(contextmenu), callback);

    public static Attribute drag(Action<DragEventArgs> callback) => @event(nameof(drag), callback);

    public static Attribute dragenter(Action<DragEventArgs> callback) => @event(nameof(dragenter), callback);

    public static Attribute dragleave(Action<DragEventArgs> callback) => @event(nameof(dragleave), callback);

    public static Attribute dragover(Action<DragEventArgs> callback) => @event(nameof(dragover), callback);

    public static Attribute dragstart(Action<DragEventArgs> callback) => @event(nameof(dragstart), callback);

    public static Attribute drop(Action<DragEventArgs> callback) => @event(nameof(drop), callback);

    public static Attribute keydown(Action<KeyboardEventArgs> callback) => @event(nameof(keydown), callback);

    public static Attribute keyup(Action<KeyboardEventArgs> callback) => @event(nameof(keyup), callback);

    public static Attribute keypress(Action<KeyboardEventArgs> callback) => @event(nameof(keypress), callback);

    public static Attribute change(Action<ChangeEventArgs> callback) => @event(nameof(change), callback);

    public static Attribute input(Action<ChangeEventArgs> callback) => @event(nameof(input), callback);

    public static Attribute reset(Action<EventArgs> callback) => @event(nameof(reset), callback);

    public static Attribute select(Action<EventArgs> callback) => @event(nameof(select), callback);

    public static Attribute selectstart(Action<EventArgs> callback) => @event(nameof(selectstart), callback);

    public static Attribute selectionchange(Action<EventArgs> callback) => @event(nameof(selectionchange), callback);

    public static Attribute submit(Action<EventArgs> callback) => @event(nameof(submit), callback);

    public static Attribute beforecopy(Action<EventArgs> callback) => @event(nameof(beforecopy), callback);

    public static Attribute beforecut(Action<EventArgs> callback) => @event(nameof(beforecut), callback);

    public static Attribute beforepaste(Action<EventArgs> callback) => @event(nameof(beforepaste), callback);

    public static Attribute copy(Action<EventArgs> callback) => @event(nameof(copy), callback);

    public static Attribute paste(Action<EventArgs> callback) => @event(nameof(paste), callback);

    public static Attribute touchcancel(Action<TouchEventArgs> callback) => @event(nameof(touchcancel), callback);

    public static Attribute touchend(Action<TouchEventArgs> callback) => @event(nameof(touchend), callback);

    public static Attribute touchmove(Action<TouchEventArgs> callback) => @event(nameof(touchmove), callback);

    public static Attribute touchstart(Action<TouchEventArgs> callback) => @event(nameof(touchstart), callback);

    public static Attribute touchenter(Action<TouchEventArgs> callback) => @event(nameof(touchenter), callback);

    public static Attribute touchleave(Action<TouchEventArgs> callback) => @event(nameof(touchleave), callback);

    public static Attribute pointercapture(Action<PointerEventArgs> callback) => @event(nameof(pointercapture), callback);

    public static Attribute lostpointercapture(Action<PointerEventArgs> callback) => @event(nameof(lostpointercapture), callback);

    public static Attribute pointercancel(Action<PointerEventArgs> callback) => @event(nameof(pointercancel), callback);

    public static Attribute pointerdown(Action<PointerEventArgs> callback) => @event(nameof(pointerdown), callback);

    public static Attribute pointerenter(Action<PointerEventArgs> callback) => @event(nameof(pointerenter), callback);

    public static Attribute pointerleave(Action<PointerEventArgs> callback) => @event(nameof(pointerleave), callback);

    public static Attribute pointermove(Action<PointerEventArgs> callback) => @event(nameof(pointermove), callback);

    public static Attribute pointerout(Action<PointerEventArgs> callback) => @event(nameof(pointerout), callback);

    public static Attribute pointerover(Action<PointerEventArgs> callback) => @event(nameof(pointerover), callback);

    public static Attribute pointerup(Action<PointerEventArgs> callback) => @event(nameof(pointerup), callback);

    public static Attribute canplay(Action<EventArgs> callback) => @event(nameof(canplay), callback);

    public static Attribute canplaythrough(Action<EventArgs> callback) => @event(nameof(canplaythrough), callback);

    public static Attribute cuechange(Action<EventArgs> callback) => @event(nameof(cuechange), callback);

    public static Attribute durationchange(Action<EventArgs> callback) => @event(nameof(durationchange), callback);

    public static Attribute emptied(Action<EventArgs> callback) => @event(nameof(emptied), callback);

    public static Attribute pause(Action<EventArgs> callback) => @event(nameof(pause), callback);

    public static Attribute play(Action<EventArgs> callback) => @event(nameof(play), callback);

    public static Attribute playing(Action<EventArgs> callback) => @event(nameof(playing), callback);

    public static Attribute ratechange(Action<EventArgs> callback) => @event(nameof(ratechange), callback);

    public static Attribute seeked(Action<EventArgs> callback) => @event(nameof(seeked), callback);

    public static Attribute seeking(Action<EventArgs> callback) => @event(nameof(seeking), callback);

    public static Attribute stalled(Action<EventArgs> callback) => @event(nameof(stalled), callback);

    public static Attribute stop(Action<EventArgs> callback) => @event(nameof(stop), callback);

    public static Attribute suspend(Action<EventArgs> callback) => @event(nameof(suspend), callback);

    public static Attribute timeupdate(Action<EventArgs> callback) => @event(nameof(timeupdate), callback);

    public static Attribute volumechange(Action<EventArgs> callback) => @event(nameof(volumechange), callback);

    public static Attribute waiting(Action<EventArgs> callback) => @event(nameof(waiting), callback);

    public static Attribute loadstart(Action<ProgressEventArgs> callback) => @event(nameof(loadstart), callback);

    public static Attribute timeout(Action<ProgressEventArgs> callback) => @event(nameof(timeout), callback);

    public static Attribute abort(Action<ProgressEventArgs> callback) => @event(nameof(abort), callback);

    public static Attribute load(Action<ProgressEventArgs> callback) => @event(nameof(load), callback);

    public static Attribute loadend(Action<ProgressEventArgs> callback) => @event(nameof(loadend), callback);

    public static Attribute progress(Action<ProgressEventArgs> callback) => @event(nameof(progress), callback);

    public static Attribute error(Action<ProgressEventArgs> callback) => @event(nameof(error), callback);

    public static Attribute activate(Action<EventArgs> callback) => @event(nameof(activate), callback);

    public static Attribute beforeactivate(Action<EventArgs> callback) => @event(nameof(beforeactivate), callback);

    public static Attribute beforedeactivate(Action<EventArgs> callback) => @event(nameof(beforedeactivate), callback);

    public static Attribute deactivate(Action<EventArgs> callback) => @event(nameof(deactivate), callback);

    public static Attribute ended(Action<EventArgs> callback) => @event(nameof(ended), callback);

    public static Attribute fullscreenchange(Action<EventArgs> callback) => @event(nameof(fullscreenchange), callback);

    public static Attribute fullscreenerror(Action<EventArgs> callback) => @event(nameof(fullscreenerror), callback);

    public static Attribute loadeddata(Action<EventArgs> callback) => @event(nameof(loadeddata), callback);

    public static Attribute loadedmetadata(Action<EventArgs> callback) => @event(nameof(loadedmetadata), callback);

    public static Attribute pointerlockchange(Action<EventArgs> callback) => @event(nameof(pointerlockchange), callback);

    public static Attribute pointerlockerror(Action<EventArgs> callback) => @event(nameof(pointerlockerror), callback);

    public static Attribute readystatechange(Action<EventArgs> callback) => @event(nameof(readystatechange), callback);

    public static Attribute scroll(Action<EventArgs> callback) => @event(nameof(scroll), callback);

    public static class task
    {

        public static Components.Attribute @event<T>(string name, Func<T, Task> callback) where T : EventArgs => new ExplicitAttribute(
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            });

        public static Components.Attribute focus(Func<FocusEventArgs, Task> callback) => @event(nameof(focus), callback);

        public static Components.Attribute blur(Func<FocusEventArgs, Task> callback) => @event(nameof(blur), callback);

        public static Components.Attribute focusin(Func<FocusEventArgs, Task> callback) => @event(nameof(focusin), callback);

        public static Components.Attribute focusout(Func<FocusEventArgs, Task> callback) => @event(nameof(focusout), callback);

        public static Components.Attribute mouseover(Func<MouseEventArgs, Task> callback) => @event(nameof(mouseover), callback);

        public static Components.Attribute mouseout(Func<MouseEventArgs, Task> callback) => @event(nameof(mouseout), callback);

        public static Components.Attribute mousemove(Func<MouseEventArgs, Task> callback) => @event(nameof(mousemove), callback);

        public static Components.Attribute mousedown(Func<MouseEventArgs, Task> callback) => @event(nameof(mousedown), callback);

        public static Components.Attribute click(Func<MouseEventArgs, Task> callback) => @event(nameof(click), callback);

        public static Components.Attribute dblclick(Func<MouseEventArgs, Task> callback) => @event(nameof(dblclick), callback);

        public static Components.Attribute wheel(Func<MouseEventArgs, Task> callback) => @event(nameof(wheel), callback);

        public static Components.Attribute mousewheel(Func<MouseEventArgs, Task> callback) => @event(nameof(mousewheel), callback);

        public static Components.Attribute contextmenu(Func<MouseEventArgs, Task> callback) => @event(nameof(contextmenu), callback);

        public static Components.Attribute drag(Func<DragEventArgs, Task> callback) => @event(nameof(drag), callback);

        public static Components.Attribute dragenter(Func<DragEventArgs, Task> callback) => @event(nameof(dragenter), callback);

        public static Components.Attribute dragleave(Func<DragEventArgs, Task> callback) => @event(nameof(dragleave), callback);

        public static Components.Attribute dragover(Func<DragEventArgs, Task> callback) => @event(nameof(dragover), callback);

        public static Components.Attribute dragstart(Func<DragEventArgs, Task> callback) => @event(nameof(dragstart), callback);

        public static Components.Attribute drop(Func<DragEventArgs, Task> callback) => @event(nameof(drop), callback);

        public static Components.Attribute keydown(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keydown), callback);

        public static Components.Attribute keyup(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keyup), callback);

        public static Components.Attribute keypress(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keypress), callback);

        public static Components.Attribute change(Func<ChangeEventArgs, Task> callback) => @event(nameof(change), callback);

        public static Components.Attribute input(Func<ChangeEventArgs, Task> callback) => @event(nameof(input), callback);

        public static Components.Attribute reset(Func<EventArgs, Task> callback) => @event(nameof(reset), callback);

        public static Components.Attribute select(Func<EventArgs, Task> callback) => @event(nameof(select), callback);

        public static Components.Attribute selectstart(Func<EventArgs, Task> callback) => @event(nameof(selectstart), callback);

        public static Components.Attribute selectionchange(Func<EventArgs, Task> callback) => @event(nameof(selectionchange), callback);

        public static Components.Attribute submit(Func<EventArgs, Task> callback) => @event(nameof(submit), callback);

        public static Components.Attribute beforecopy(Func<EventArgs, Task> callback) => @event(nameof(beforecopy), callback);

        public static Components.Attribute beforecut(Func<EventArgs, Task> callback) => @event(nameof(beforecut), callback);

        public static Components.Attribute beforepaste(Func<EventArgs, Task> callback) => @event(nameof(beforepaste), callback);

        public static Components.Attribute copy(Func<EventArgs, Task> callback) => @event(nameof(copy), callback);

        public static Components.Attribute paste(Func<EventArgs, Task> callback) => @event(nameof(paste), callback);

        public static Components.Attribute touchcancel(Func<TouchEventArgs, Task> callback) => @event(nameof(touchcancel), callback);

        public static Components.Attribute touchend(Func<TouchEventArgs, Task> callback) => @event(nameof(touchend), callback);

        public static Components.Attribute touchmove(Func<TouchEventArgs, Task> callback) => @event(nameof(touchmove), callback);

        public static Components.Attribute touchstart(Func<TouchEventArgs, Task> callback) => @event(nameof(touchstart), callback);

        public static Components.Attribute touchenter(Func<TouchEventArgs, Task> callback) => @event(nameof(touchenter), callback);

        public static Components.Attribute touchleave(Func<TouchEventArgs, Task> callback) => @event(nameof(touchleave), callback);

        public static Components.Attribute pointercapture(Func<PointerEventArgs, Task> callback) => @event(nameof(pointercapture), callback);

        public static Components.Attribute lostpointercapture(Func<PointerEventArgs, Task> callback) => @event(nameof(lostpointercapture), callback);

        public static Components.Attribute pointercancel(Func<PointerEventArgs, Task> callback) => @event(nameof(pointercancel), callback);

        public static Components.Attribute pointerdown(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerdown), callback);

        public static Components.Attribute pointerenter(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerenter), callback);

        public static Components.Attribute pointerleave(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerleave), callback);

        public static Components.Attribute pointermove(Func<PointerEventArgs, Task> callback) => @event(nameof(pointermove), callback);

        public static Components.Attribute pointerout(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerout), callback);

        public static Components.Attribute pointerover(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerover), callback);

        public static Components.Attribute pointerup(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerup), callback);

        public static Components.Attribute canplay(Func<EventArgs, Task> callback) => @event(nameof(canplay), callback);

        public static Components.Attribute canplaythrough(Func<EventArgs, Task> callback) => @event(nameof(canplaythrough), callback);

        public static Components.Attribute cuechange(Func<EventArgs, Task> callback) => @event(nameof(cuechange), callback);

        public static Components.Attribute durationchange(Func<EventArgs, Task> callback) => @event(nameof(durationchange), callback);

        public static Components.Attribute emptied(Func<EventArgs, Task> callback) => @event(nameof(emptied), callback);

        public static Components.Attribute pause(Func<EventArgs, Task> callback) => @event(nameof(pause), callback);

        public static Components.Attribute play(Func<EventArgs, Task> callback) => @event(nameof(play), callback);

        public static Components.Attribute playing(Func<EventArgs, Task> callback) => @event(nameof(playing), callback);

        public static Components.Attribute ratechange(Func<EventArgs, Task> callback) => @event(nameof(ratechange), callback);

        public static Components.Attribute seeked(Func<EventArgs, Task> callback) => @event(nameof(seeked), callback);

        public static Components.Attribute seeking(Func<EventArgs, Task> callback) => @event(nameof(seeking), callback);

        public static Components.Attribute stalled(Func<EventArgs, Task> callback) => @event(nameof(stalled), callback);

        public static Components.Attribute stop(Func<EventArgs, Task> callback) => @event(nameof(stop), callback);

        public static Components.Attribute suspend(Func<EventArgs, Task> callback) => @event(nameof(suspend), callback);

        public static Components.Attribute timeupdate(Func<EventArgs, Task> callback) => @event(nameof(timeupdate), callback);

        public static Components.Attribute volumechange(Func<EventArgs, Task> callback) => @event(nameof(volumechange), callback);

        public static Components.Attribute waiting(Func<EventArgs, Task> callback) => @event(nameof(waiting), callback);

        public static Components.Attribute loadstart(Func<ProgressEventArgs, Task> callback) => @event(nameof(loadstart), callback);

        public static Components.Attribute timeout(Func<ProgressEventArgs, Task> callback) => @event(nameof(timeout), callback);

        public static Components.Attribute abort(Func<ProgressEventArgs, Task> callback) => @event(nameof(abort), callback);

        public static Components.Attribute load(Func<ProgressEventArgs, Task> callback) => @event(nameof(load), callback);

        public static Components.Attribute loadend(Func<ProgressEventArgs, Task> callback) => @event(nameof(loadend), callback);

        public static Components.Attribute progress(Func<ProgressEventArgs, Task> callback) => @event(nameof(progress), callback);

        public static Components.Attribute error(Func<ProgressEventArgs, Task> callback) => @event(nameof(error), callback);

        public static Components.Attribute activate(Func<EventArgs, Task> callback) => @event(nameof(activate), callback);

        public static Components.Attribute beforeactivate(Func<EventArgs, Task> callback) => @event(nameof(beforeactivate), callback);

        public static Components.Attribute beforedeactivate(Func<EventArgs, Task> callback) => @event(nameof(beforedeactivate), callback);

        public static Components.Attribute deactivate(Func<EventArgs, Task> callback) => @event(nameof(deactivate), callback);

        public static Components.Attribute ended(Func<EventArgs, Task> callback) => @event(nameof(ended), callback);

        public static Components.Attribute fullscreenchange(Func<EventArgs, Task> callback) => @event(nameof(fullscreenchange), callback);

        public static Components.Attribute fullscreenerror(Func<EventArgs, Task> callback) => @event(nameof(fullscreenerror), callback);

        public static Components.Attribute loadeddata(Func<EventArgs, Task> callback) => @event(nameof(loadeddata), callback);

        public static Components.Attribute loadedmetadata(Func<EventArgs, Task> callback) => @event(nameof(loadedmetadata), callback);

        public static Components.Attribute pointerlockchange(Func<EventArgs, Task> callback) => @event(nameof(pointerlockchange), callback);

        public static Components.Attribute pointerlockerror(Func<EventArgs, Task> callback) => @event(nameof(pointerlockerror), callback);

        public static Components.Attribute readystatechange(Func<EventArgs, Task> callback) => @event(nameof(readystatechange), callback);

        public static Components.Attribute scroll(Func<EventArgs, Task> callback) => @event(nameof(scroll), callback);
    }
}
