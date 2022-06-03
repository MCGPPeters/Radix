using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Interaction.Components;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Web.Components;

public static class on
{
    public static Attribute @event<T>(AttributeId attributeId, string name, Action<T> callback) where T : EventArgs =>
        new ExplicitAttribute
        (
            attributeId,
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            }
        );

    public static Attribute focus(AttributeId attributeId, Action<FocusEventArgs> callback) => @event(attributeId, nameof(focus), callback);

    public static Attribute blur(AttributeId attributeId, Action<FocusEventArgs> callback) => @event(attributeId, nameof(blur), callback);

    public static Attribute focusin(AttributeId attributeId, Action<FocusEventArgs> callback) => @event(attributeId, nameof(focusin), callback);

    public static Attribute focusout(AttributeId attributeId, Action<FocusEventArgs> callback) => @event(attributeId, nameof(focusout), callback);

    public static Attribute mouseover(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(mouseover), callback);

    public static Attribute mouseout(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(mouseout), callback);

    public static Attribute mousemove(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(mousemove), callback);

    public static Attribute mousedown(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(mousedown), callback);

    public static Attribute click(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(click), callback);

    public static Attribute dblclick(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(dblclick), callback);

    public static Attribute wheel(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(wheel), callback);

    public static Attribute mousewheel(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(mousewheel), callback);

    public static Attribute contextmenu(AttributeId attributeId, Action<MouseEventArgs> callback) => @event(attributeId, nameof(contextmenu), callback);

    public static Attribute drag(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(drag), callback);

    public static Attribute dragenter(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(dragenter), callback);

    public static Attribute dragleave(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(dragleave), callback);

    public static Attribute dragover(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(dragover), callback);

    public static Attribute dragstart(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(dragstart), callback);

    public static Attribute drop(AttributeId attributeId, Action<DragEventArgs> callback) => @event(attributeId, nameof(drop), callback);

    public static Attribute keydown(AttributeId attributeId, Action<KeyboardEventArgs> callback) => @event(attributeId, nameof(keydown), callback);

    public static Attribute keyup(AttributeId attributeId, Action<KeyboardEventArgs> callback) => @event(attributeId, nameof(keyup), callback);

    public static Attribute keypress(AttributeId attributeId, Action<KeyboardEventArgs> callback) => @event(attributeId, nameof(keypress), callback);

    public static Attribute change(AttributeId attributeId, Action<ChangeEventArgs> callback) => @event(attributeId, nameof(change), callback);

    public static Attribute input(AttributeId attributeId, Action<ChangeEventArgs> callback) => @event(attributeId, nameof(input), callback);

    public static Attribute reset(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(reset), callback);

    public static Attribute select(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(select), callback);

    public static Attribute selectstart(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(selectstart), callback);

    public static Attribute selectionchange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(selectionchange), callback);

    public static Attribute submit(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(submit), callback);

    public static Attribute beforecopy(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(beforecopy), callback);

    public static Attribute beforecut(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(beforecut), callback);

    public static Attribute beforepaste(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(beforepaste), callback);

    public static Attribute copy(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(copy), callback);

    public static Attribute paste(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(paste), callback);

    public static Attribute touchcancel(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchcancel), callback);

    public static Attribute touchend(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchend), callback);

    public static Attribute touchmove(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchmove), callback);

    public static Attribute touchstart(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchstart), callback);

    public static Attribute touchenter(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchenter), callback);

    public static Attribute touchleave(AttributeId attributeId, Action<TouchEventArgs> callback) => @event(attributeId, nameof(touchleave), callback);

    public static Attribute pointercapture(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointercapture), callback);

    public static Attribute lostpointercapture(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(lostpointercapture), callback);

    public static Attribute pointercancel(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointercancel), callback);

    public static Attribute pointerdown(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerdown), callback);

    public static Attribute pointerenter(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerenter), callback);

    public static Attribute pointerleave(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerleave), callback);

    public static Attribute pointermove(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointermove), callback);

    public static Attribute pointerout(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerout), callback);

    public static Attribute pointerover(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerover), callback);

    public static Attribute pointerup(AttributeId attributeId, Action<PointerEventArgs> callback) => @event(attributeId, nameof(pointerup), callback);

    public static Attribute canplay(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(canplay), callback);

    public static Attribute canplaythrough(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(canplaythrough), callback);

    public static Attribute cuechange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(cuechange), callback);

    public static Attribute durationchange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(durationchange), callback);

    public static Attribute emptied(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(emptied), callback);

    public static Attribute pause(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(pause), callback);

    public static Attribute play(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(play), callback);

    public static Attribute playing(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(playing), callback);

    public static Attribute ratechange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(ratechange), callback);

    public static Attribute seeked(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(seeked), callback);

    public static Attribute seeking(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(seeking), callback);

    public static Attribute stalled(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(stalled), callback);

    public static Attribute stop(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(stop), callback);

    public static Attribute suspend(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(suspend), callback);

    public static Attribute timeupdate(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(timeupdate), callback);

    public static Attribute volumechange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(volumechange), callback);

    public static Attribute waiting(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(waiting), callback);

    public static Attribute loadstart(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(loadstart), callback);

    public static Attribute timeout(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(timeout), callback);

    public static Attribute abort(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(abort), callback);

    public static Attribute load(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(load), callback);

    public static Attribute loadend(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(loadend), callback);

    public static Attribute progress(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(progress), callback);

    public static Attribute error(AttributeId attributeId, Action<ProgressEventArgs> callback) => @event(attributeId, nameof(error), callback);

    public static Attribute activate(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(activate), callback);

    public static Attribute beforeactivate(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(beforeactivate), callback);

    public static Attribute beforedeactivate(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(beforedeactivate), callback);

    public static Attribute deactivate(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(deactivate), callback);

    public static Attribute ended(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(ended), callback);

    public static Attribute fullscreenchange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(fullscreenchange), callback);

    public static Attribute fullscreenerror(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(fullscreenerror), callback);

    public static Attribute loadeddata(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(loadeddata), callback);

    public static Attribute loadedmetadata(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(loadedmetadata), callback);

    public static Attribute pointerlockchange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(pointerlockchange), callback);

    public static Attribute pointerlockerror(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(pointerlockerror), callback);

    public static Attribute readystatechange(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(readystatechange), callback);

    public static Attribute scroll(AttributeId attributeId, Action<EventArgs> callback) => @event(attributeId, nameof(scroll), callback);

    public static class task
    {

        public static Attribute @event<T>(AttributeId attributeId, string name, Func<T, Task> callback) where T : EventArgs => new ExplicitAttribute(
            attributeId,
            name,
            (builder, sequence, receiver) =>
            {
                builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                return sequence + 1;
            });

        public static Attribute focus(AttributeId attributeId, Func<FocusEventArgs, Task> callback) => @event(attributeId, nameof(focus), callback);

        public static Attribute blur(AttributeId attributeId, Func<FocusEventArgs, Task> callback) => @event(attributeId, nameof(blur), callback);

        public static Attribute focusin(AttributeId attributeId, Func<FocusEventArgs, Task> callback) => @event(attributeId, nameof(focusin), callback);

        public static Attribute focusout(AttributeId attributeId, Func<FocusEventArgs, Task> callback) => @event(attributeId, nameof(focusout), callback);

        public static Attribute mouseover(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(mouseover), callback);

        public static Attribute mouseout(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(mouseout), callback);

        public static Attribute mousemove(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(mousemove), callback);

        public static Attribute mousedown(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(mousedown), callback);

        public static Attribute click(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(click), callback);

        public static Attribute dblclick(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(dblclick), callback);

        public static Attribute wheel(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(wheel), callback);

        public static Attribute mousewheel(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(mousewheel), callback);

        public static Attribute contextmenu(AttributeId attributeId, Func<MouseEventArgs, Task> callback) => @event(attributeId, nameof(contextmenu), callback);

        public static Attribute drag(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(drag), callback);

        public static Attribute dragenter(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(dragenter), callback);

        public static Attribute dragleave(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(dragleave), callback);

        public static Attribute dragover(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(dragover), callback);

        public static Attribute dragstart(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(dragstart), callback);

        public static Attribute drop(AttributeId attributeId, Func<DragEventArgs, Task> callback) => @event(attributeId, nameof(drop), callback);

        public static Attribute keydown(AttributeId attributeId, Func<KeyboardEventArgs, Task> callback) => @event(attributeId, nameof(keydown), callback);

        public static Attribute keyup(AttributeId attributeId, Func<KeyboardEventArgs, Task> callback) => @event(attributeId, nameof(keyup), callback);

        public static Attribute keypress(AttributeId attributeId, Func<KeyboardEventArgs, Task> callback) => @event(attributeId, nameof(keypress), callback);

        public static Attribute change(AttributeId attributeId, Func<ChangeEventArgs, Task> callback) => @event(attributeId, nameof(change), callback);

        public static Attribute input(AttributeId attributeId, Func<ChangeEventArgs, Task> callback) => @event(attributeId, nameof(input), callback);

        public static Attribute reset(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(reset), callback);

        public static Attribute select(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(select), callback);

        public static Attribute selectstart(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(selectstart), callback);

        public static Attribute selectionchange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(selectionchange), callback);

        public static Attribute submit(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(submit), callback);

        public static Attribute beforecopy(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(beforecopy), callback);

        public static Attribute beforecut(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(beforecut), callback);

        public static Attribute beforepaste(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(beforepaste), callback);

        public static Attribute copy(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(copy), callback);

        public static Attribute paste(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(paste), callback);

        public static Attribute touchcancel(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchcancel), callback);

        public static Attribute touchend(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchend), callback);

        public static Attribute touchmove(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchmove), callback);

        public static Attribute touchstart(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchstart), callback);

        public static Attribute touchenter(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchenter), callback);

        public static Attribute touchleave(AttributeId attributeId, Func<TouchEventArgs, Task> callback) => @event(attributeId, nameof(touchleave), callback);

        public static Attribute pointercapture(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointercapture), callback);

        public static Attribute lostpointercapture(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(lostpointercapture), callback);

        public static Attribute pointercancel(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointercancel), callback);

        public static Attribute pointerdown(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerdown), callback);

        public static Attribute pointerenter(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerenter), callback);

        public static Attribute pointerleave(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerleave), callback);

        public static Attribute pointermove(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointermove), callback);

        public static Attribute pointerout(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerout), callback);

        public static Attribute pointerover(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerover), callback);

        public static Attribute pointerup(AttributeId attributeId, Func<PointerEventArgs, Task> callback) => @event(attributeId, nameof(pointerup), callback);

        public static Attribute canplay(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(canplay), callback);

        public static Attribute canplaythrough(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(canplaythrough), callback);

        public static Attribute cuechange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(cuechange), callback);

        public static Attribute durationchange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(durationchange), callback);

        public static Attribute emptied(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(emptied), callback);

        public static Attribute pause(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(pause), callback);

        public static Attribute play(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(play), callback);

        public static Attribute playing(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(playing), callback);

        public static Attribute ratechange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(ratechange), callback);

        public static Attribute seeked(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(seeked), callback);

        public static Attribute seeking(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(seeking), callback);

        public static Attribute stalled(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(stalled), callback);

        public static Attribute stop(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(stop), callback);

        public static Attribute suspend(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(suspend), callback);

        public static Attribute timeupdate(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(timeupdate), callback);

        public static Attribute volumechange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(volumechange), callback);

        public static Attribute waiting(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(waiting), callback);

        public static Attribute loadstart(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(loadstart), callback);

        public static Attribute timeout(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(timeout), callback);

        public static Attribute abort(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(abort), callback);

        public static Attribute load(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(load), callback);

        public static Attribute loadend(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(loadend), callback);

        public static Attribute progress(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(progress), callback);

        public static Attribute error(AttributeId attributeId, Func<ProgressEventArgs, Task> callback) => @event(attributeId, nameof(error), callback);

        public static Attribute activate(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(activate), callback);

        public static Attribute beforeactivate(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(beforeactivate), callback);

        public static Attribute beforedeactivate(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(beforedeactivate), callback);

        public static Attribute deactivate(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(deactivate), callback);

        public static Attribute ended(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(ended), callback);

        public static Attribute fullscreenchange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(fullscreenchange), callback);

        public static Attribute fullscreenerror(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(fullscreenerror), callback);

        public static Attribute loadeddata(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(loadeddata), callback);

        public static Attribute loadedmetadata(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(loadedmetadata), callback);

        public static Attribute pointerlockchange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(pointerlockchange), callback);

        public static Attribute pointerlockerror(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(pointerlockerror), callback);

        public static Attribute readystatechange(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(readystatechange), callback);

        public static Attribute scroll(AttributeId attributeId, Func<EventArgs, Task> callback) => @event(attributeId, nameof(scroll), callback);
    }
}
