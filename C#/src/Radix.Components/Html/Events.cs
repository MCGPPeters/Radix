using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Components.Html;

public static class on
{
    public static IAttribute @event<T>(string name, Action<T> callback) where T : EventArgs =>
        new ExplicitAttribute
        (
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            }
        );

    public static IAttribute focus(Action<FocusEventArgs> callback) => @event(nameof(focus), callback);

    public static IAttribute blur(Action<FocusEventArgs> callback) => @event(nameof(blur), callback);

    public static IAttribute focusin(Action<FocusEventArgs> callback) => @event(nameof(focusin), callback);

    public static IAttribute focusout(Action<FocusEventArgs> callback) => @event(nameof(focusout), callback);

    public static IAttribute mouseover(Action<MouseEventArgs> callback) => @event(nameof(mouseover), callback);

    public static IAttribute mouseout(Action<MouseEventArgs> callback) => @event(nameof(mouseout), callback);

    public static IAttribute mousemove(Action<MouseEventArgs> callback) => @event(nameof(mousemove), callback);

    public static IAttribute mousedown(Action<MouseEventArgs> callback) => @event(nameof(mousedown), callback);

    public static IAttribute click(Action<MouseEventArgs> callback) => @event(nameof(click), callback);

    public static IAttribute dblclick(Action<MouseEventArgs> callback) => @event(nameof(dblclick), callback);

    public static IAttribute wheel(Action<MouseEventArgs> callback) => @event(nameof(wheel), callback);

    public static IAttribute mousewheel(Action<MouseEventArgs> callback) => @event(nameof(mousewheel), callback);

    public static IAttribute contextmenu(Action<MouseEventArgs> callback) => @event(nameof(contextmenu), callback);

    public static IAttribute drag(Action<DragEventArgs> callback) => @event(nameof(drag), callback);

    public static IAttribute dragenter(Action<DragEventArgs> callback) => @event(nameof(dragenter), callback);

    public static IAttribute dragleave(Action<DragEventArgs> callback) => @event(nameof(dragleave), callback);

    public static IAttribute dragover(Action<DragEventArgs> callback) => @event(nameof(dragover), callback);

    public static IAttribute dragstart(Action<DragEventArgs> callback) => @event(nameof(dragstart), callback);

    public static IAttribute drop(Action<DragEventArgs> callback) => @event(nameof(drop), callback);

    public static IAttribute keydown(Action<KeyboardEventArgs> callback) => @event(nameof(keydown), callback);

    public static IAttribute keyup(Action<KeyboardEventArgs> callback) => @event(nameof(keyup), callback);

    public static IAttribute keypress(Action<KeyboardEventArgs> callback) => @event(nameof(keypress), callback);

    public static IAttribute change(Action<ChangeEventArgs> callback) => @event(nameof(change), callback);

    public static IAttribute input(Action<ChangeEventArgs> callback) => @event(nameof(input), callback);

    public static IAttribute reset(Action<EventArgs> callback) => @event(nameof(reset), callback);

    public static IAttribute select(Action<EventArgs> callback) => @event(nameof(select), callback);

    public static IAttribute selectstart(Action<EventArgs> callback) => @event(nameof(selectstart), callback);

    public static IAttribute selectionchange(Action<EventArgs> callback) => @event(nameof(selectionchange), callback);

    public static IAttribute submit(Action<EventArgs> callback) => @event(nameof(submit), callback);

    public static IAttribute beforecopy(Action<EventArgs> callback) => @event(nameof(beforecopy), callback);

    public static IAttribute beforecut(Action<EventArgs> callback) => @event(nameof(beforecut), callback);

    public static IAttribute beforepaste(Action<EventArgs> callback) => @event(nameof(beforepaste), callback);

    public static IAttribute copy(Action<EventArgs> callback) => @event(nameof(copy), callback);

    public static IAttribute paste(Action<EventArgs> callback) => @event(nameof(paste), callback);

    public static IAttribute touchcancel(Action<TouchEventArgs> callback) => @event(nameof(touchcancel), callback);

    public static IAttribute touchend(Action<TouchEventArgs> callback) => @event(nameof(touchend), callback);

    public static IAttribute touchmove(Action<TouchEventArgs> callback) => @event(nameof(touchmove), callback);

    public static IAttribute touchstart(Action<TouchEventArgs> callback) => @event(nameof(touchstart), callback);

    public static IAttribute touchenter(Action<TouchEventArgs> callback) => @event(nameof(touchenter), callback);

    public static IAttribute touchleave(Action<TouchEventArgs> callback) => @event(nameof(touchleave), callback);

    public static IAttribute pointercapture(Action<PointerEventArgs> callback) => @event(nameof(pointercapture), callback);

    public static IAttribute lostpointercapture(Action<PointerEventArgs> callback) => @event(nameof(lostpointercapture), callback);

    public static IAttribute pointercancel(Action<PointerEventArgs> callback) => @event(nameof(pointercancel), callback);

    public static IAttribute pointerdown(Action<PointerEventArgs> callback) => @event(nameof(pointerdown), callback);

    public static IAttribute pointerenter(Action<PointerEventArgs> callback) => @event(nameof(pointerenter), callback);

    public static IAttribute pointerleave(Action<PointerEventArgs> callback) => @event(nameof(pointerleave), callback);

    public static IAttribute pointermove(Action<PointerEventArgs> callback) => @event(nameof(pointermove), callback);

    public static IAttribute pointerout(Action<PointerEventArgs> callback) => @event(nameof(pointerout), callback);

    public static IAttribute pointerover(Action<PointerEventArgs> callback) => @event(nameof(pointerover), callback);

    public static IAttribute pointerup(Action<PointerEventArgs> callback) => @event(nameof(pointerup), callback);

    public static IAttribute canplay(Action<EventArgs> callback) => @event(nameof(canplay), callback);

    public static IAttribute canplaythrough(Action<EventArgs> callback) => @event(nameof(canplaythrough), callback);

    public static IAttribute cuechange(Action<EventArgs> callback) => @event(nameof(cuechange), callback);

    public static IAttribute durationchange(Action<EventArgs> callback) => @event(nameof(durationchange), callback);

    public static IAttribute emptied(Action<EventArgs> callback) => @event(nameof(emptied), callback);

    public static IAttribute pause(Action<EventArgs> callback) => @event(nameof(pause), callback);

    public static IAttribute play(Action<EventArgs> callback) => @event(nameof(play), callback);

    public static IAttribute playing(Action<EventArgs> callback) => @event(nameof(playing), callback);

    public static IAttribute ratechange(Action<EventArgs> callback) => @event(nameof(ratechange), callback);

    public static IAttribute seeked(Action<EventArgs> callback) => @event(nameof(seeked), callback);

    public static IAttribute seeking(Action<EventArgs> callback) => @event(nameof(seeking), callback);

    public static IAttribute stalled(Action<EventArgs> callback) => @event(nameof(stalled), callback);

    public static IAttribute stop(Action<EventArgs> callback) => @event(nameof(stop), callback);

    public static IAttribute suspend(Action<EventArgs> callback) => @event(nameof(suspend), callback);

    public static IAttribute timeupdate(Action<EventArgs> callback) => @event(nameof(timeupdate), callback);

    public static IAttribute volumechange(Action<EventArgs> callback) => @event(nameof(volumechange), callback);

    public static IAttribute waiting(Action<EventArgs> callback) => @event(nameof(waiting), callback);

    public static IAttribute loadstart(Action<ProgressEventArgs> callback) => @event(nameof(loadstart), callback);

    public static IAttribute timeout(Action<ProgressEventArgs> callback) => @event(nameof(timeout), callback);

    public static IAttribute abort(Action<ProgressEventArgs> callback) => @event(nameof(abort), callback);

    public static IAttribute load(Action<ProgressEventArgs> callback) => @event(nameof(load), callback);

    public static IAttribute loadend(Action<ProgressEventArgs> callback) => @event(nameof(loadend), callback);

    public static IAttribute progress(Action<ProgressEventArgs> callback) => @event(nameof(progress), callback);

    public static IAttribute error(Action<ProgressEventArgs> callback) => @event(nameof(error), callback);

    public static IAttribute activate(Action<EventArgs> callback) => @event(nameof(activate), callback);

    public static IAttribute beforeactivate(Action<EventArgs> callback) => @event(nameof(beforeactivate), callback);

    public static IAttribute beforedeactivate(Action<EventArgs> callback) => @event(nameof(beforedeactivate), callback);

    public static IAttribute deactivate(Action<EventArgs> callback) => @event(nameof(deactivate), callback);

    public static IAttribute ended(Action<EventArgs> callback) => @event(nameof(ended), callback);

    public static IAttribute fullscreenchange(Action<EventArgs> callback) => @event(nameof(fullscreenchange), callback);

    public static IAttribute fullscreenerror(Action<EventArgs> callback) => @event(nameof(fullscreenerror), callback);

    public static IAttribute loadeddata(Action<EventArgs> callback) => @event(nameof(loadeddata), callback);

    public static IAttribute loadedmetadata(Action<EventArgs> callback) => @event(nameof(loadedmetadata), callback);

    public static IAttribute pointerlockchange(Action<EventArgs> callback) => @event(nameof(pointerlockchange), callback);

    public static IAttribute pointerlockerror(Action<EventArgs> callback) => @event(nameof(pointerlockerror), callback);

    public static IAttribute readystatechange(Action<EventArgs> callback) => @event(nameof(readystatechange), callback);

    public static IAttribute scroll(Action<EventArgs> callback) => @event(nameof(scroll), callback);

    public static class task
    {

        public static IAttribute @event<T>(string name, Func<T, Task> callback) where T : EventArgs => new ExplicitAttribute(
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            });

        public static IAttribute focus(Func<FocusEventArgs, Task> callback) => @event(nameof(focus), callback);

        public static IAttribute blur(Func<FocusEventArgs, Task> callback) => @event(nameof(blur), callback);

        public static IAttribute focusin(Func<FocusEventArgs, Task> callback) => @event(nameof(focusin), callback);

        public static IAttribute focusout(Func<FocusEventArgs, Task> callback) => @event(nameof(focusout), callback);

        public static IAttribute mouseover(Func<MouseEventArgs, Task> callback) => @event(nameof(mouseover), callback);

        public static IAttribute mouseout(Func<MouseEventArgs, Task> callback) => @event(nameof(mouseout), callback);

        public static IAttribute mousemove(Func<MouseEventArgs, Task> callback) => @event(nameof(mousemove), callback);

        public static IAttribute mousedown(Func<MouseEventArgs, Task> callback) => @event(nameof(mousedown), callback);

        public static IAttribute click(Func<MouseEventArgs, Task> callback) => @event(nameof(click), callback);

        public static IAttribute dblclick(Func<MouseEventArgs, Task> callback) => @event(nameof(dblclick), callback);

        public static IAttribute wheel(Func<MouseEventArgs, Task> callback) => @event(nameof(wheel), callback);

        public static IAttribute mousewheel(Func<MouseEventArgs, Task> callback) => @event(nameof(mousewheel), callback);

        public static IAttribute contextmenu(Func<MouseEventArgs, Task> callback) => @event(nameof(contextmenu), callback);

        public static IAttribute drag(Func<DragEventArgs, Task> callback) => @event(nameof(drag), callback);

        public static IAttribute dragenter(Func<DragEventArgs, Task> callback) => @event(nameof(dragenter), callback);

        public static IAttribute dragleave(Func<DragEventArgs, Task> callback) => @event(nameof(dragleave), callback);

        public static IAttribute dragover(Func<DragEventArgs, Task> callback) => @event(nameof(dragover), callback);

        public static IAttribute dragstart(Func<DragEventArgs, Task> callback) => @event(nameof(dragstart), callback);

        public static IAttribute drop(Func<DragEventArgs, Task> callback) => @event(nameof(drop), callback);

        public static IAttribute keydown(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keydown), callback);

        public static IAttribute keyup(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keyup), callback);

        public static IAttribute keypress(Func<KeyboardEventArgs, Task> callback) => @event(nameof(keypress), callback);

        public static IAttribute change(Func<ChangeEventArgs, Task> callback) => @event(nameof(change), callback);

        public static IAttribute input(Func<ChangeEventArgs, Task> callback) => @event(nameof(input), callback);

        public static IAttribute reset(Func<EventArgs, Task> callback) => @event(nameof(reset), callback);

        public static IAttribute select(Func<EventArgs, Task> callback) => @event(nameof(select), callback);

        public static IAttribute selectstart(Func<EventArgs, Task> callback) => @event(nameof(selectstart), callback);

        public static IAttribute selectionchange(Func<EventArgs, Task> callback) => @event(nameof(selectionchange), callback);

        public static IAttribute submit(Func<EventArgs, Task> callback) => @event(nameof(submit), callback);

        public static IAttribute beforecopy(Func<EventArgs, Task> callback) => @event(nameof(beforecopy), callback);

        public static IAttribute beforecut(Func<EventArgs, Task> callback) => @event(nameof(beforecut), callback);

        public static IAttribute beforepaste(Func<EventArgs, Task> callback) => @event(nameof(beforepaste), callback);

        public static IAttribute copy(Func<EventArgs, Task> callback) => @event(nameof(copy), callback);

        public static IAttribute paste(Func<EventArgs, Task> callback) => @event(nameof(paste), callback);

        public static IAttribute touchcancel(Func<TouchEventArgs, Task> callback) => @event(nameof(touchcancel), callback);

        public static IAttribute touchend(Func<TouchEventArgs, Task> callback) => @event(nameof(touchend), callback);

        public static IAttribute touchmove(Func<TouchEventArgs, Task> callback) => @event(nameof(touchmove), callback);

        public static IAttribute touchstart(Func<TouchEventArgs, Task> callback) => @event(nameof(touchstart), callback);

        public static IAttribute touchenter(Func<TouchEventArgs, Task> callback) => @event(nameof(touchenter), callback);

        public static IAttribute touchleave(Func<TouchEventArgs, Task> callback) => @event(nameof(touchleave), callback);

        public static IAttribute pointercapture(Func<PointerEventArgs, Task> callback) => @event(nameof(pointercapture), callback);

        public static IAttribute lostpointercapture(Func<PointerEventArgs, Task> callback) => @event(nameof(lostpointercapture), callback);

        public static IAttribute pointercancel(Func<PointerEventArgs, Task> callback) => @event(nameof(pointercancel), callback);

        public static IAttribute pointerdown(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerdown), callback);

        public static IAttribute pointerenter(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerenter), callback);

        public static IAttribute pointerleave(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerleave), callback);

        public static IAttribute pointermove(Func<PointerEventArgs, Task> callback) => @event(nameof(pointermove), callback);

        public static IAttribute pointerout(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerout), callback);

        public static IAttribute pointerover(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerover), callback);

        public static IAttribute pointerup(Func<PointerEventArgs, Task> callback) => @event(nameof(pointerup), callback);

        public static IAttribute canplay(Func<EventArgs, Task> callback) => @event(nameof(canplay), callback);

        public static IAttribute canplaythrough(Func<EventArgs, Task> callback) => @event(nameof(canplaythrough), callback);

        public static IAttribute cuechange(Func<EventArgs, Task> callback) => @event(nameof(cuechange), callback);

        public static IAttribute durationchange(Func<EventArgs, Task> callback) => @event(nameof(durationchange), callback);

        public static IAttribute emptied(Func<EventArgs, Task> callback) => @event(nameof(emptied), callback);

        public static IAttribute pause(Func<EventArgs, Task> callback) => @event(nameof(pause), callback);

        public static IAttribute play(Func<EventArgs, Task> callback) => @event(nameof(play), callback);

        public static IAttribute playing(Func<EventArgs, Task> callback) => @event(nameof(playing), callback);

        public static IAttribute ratechange(Func<EventArgs, Task> callback) => @event(nameof(ratechange), callback);

        public static IAttribute seeked(Func<EventArgs, Task> callback) => @event(nameof(seeked), callback);

        public static IAttribute seeking(Func<EventArgs, Task> callback) => @event(nameof(seeking), callback);

        public static IAttribute stalled(Func<EventArgs, Task> callback) => @event(nameof(stalled), callback);

        public static IAttribute stop(Func<EventArgs, Task> callback) => @event(nameof(stop), callback);

        public static IAttribute suspend(Func<EventArgs, Task> callback) => @event(nameof(suspend), callback);

        public static IAttribute timeupdate(Func<EventArgs, Task> callback) => @event(nameof(timeupdate), callback);

        public static IAttribute volumechange(Func<EventArgs, Task> callback) => @event(nameof(volumechange), callback);

        public static IAttribute waiting(Func<EventArgs, Task> callback) => @event(nameof(waiting), callback);

        public static IAttribute loadstart(Func<ProgressEventArgs, Task> callback) => @event(nameof(loadstart), callback);

        public static IAttribute timeout(Func<ProgressEventArgs, Task> callback) => @event(nameof(timeout), callback);

        public static IAttribute abort(Func<ProgressEventArgs, Task> callback) => @event(nameof(abort), callback);

        public static IAttribute load(Func<ProgressEventArgs, Task> callback) => @event(nameof(load), callback);

        public static IAttribute loadend(Func<ProgressEventArgs, Task> callback) => @event(nameof(loadend), callback);

        public static IAttribute progress(Func<ProgressEventArgs, Task> callback) => @event(nameof(progress), callback);

        public static IAttribute error(Func<ProgressEventArgs, Task> callback) => @event(nameof(error), callback);

        public static IAttribute activate(Func<EventArgs, Task> callback) => @event(nameof(activate), callback);

        public static IAttribute beforeactivate(Func<EventArgs, Task> callback) => @event(nameof(beforeactivate), callback);

        public static IAttribute beforedeactivate(Func<EventArgs, Task> callback) => @event(nameof(beforedeactivate), callback);

        public static IAttribute deactivate(Func<EventArgs, Task> callback) => @event(nameof(deactivate), callback);

        public static IAttribute ended(Func<EventArgs, Task> callback) => @event(nameof(ended), callback);

        public static IAttribute fullscreenchange(Func<EventArgs, Task> callback) => @event(nameof(fullscreenchange), callback);

        public static IAttribute fullscreenerror(Func<EventArgs, Task> callback) => @event(nameof(fullscreenerror), callback);

        public static IAttribute loadeddata(Func<EventArgs, Task> callback) => @event(nameof(loadeddata), callback);

        public static IAttribute loadedmetadata(Func<EventArgs, Task> callback) => @event(nameof(loadedmetadata), callback);

        public static IAttribute pointerlockchange(Func<EventArgs, Task> callback) => @event(nameof(pointerlockchange), callback);

        public static IAttribute pointerlockerror(Func<EventArgs, Task> callback) => @event(nameof(pointerlockerror), callback);

        public static IAttribute readystatechange(Func<EventArgs, Task> callback) => @event(nameof(readystatechange), callback);

        public static IAttribute scroll(Func<EventArgs, Task> callback) => @event(nameof(scroll), callback);
    }
}
