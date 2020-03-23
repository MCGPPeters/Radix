using System;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Blazor.Html
{

    public static class on
    {
        public static IAttribute @event<T>(Name name, Func<T, Task> callback) where T : EventArgs
        {
            


            return new ExplicitAttribute(
                name,
                (builder, sequence, receiver) =>
                { 
                    builder.AddAttribute(sequence, "on" + name, EventCallback.Factory.Create(receiver, callback));
                    return sequence + 1;
                });
        }

        public static IAttribute focus(Func<FocusEventArgs, Task> callback)
        {
            return @event(nameof(focus), callback);
        }

        public static IAttribute blur(Func<FocusEventArgs, Task> callback)
        {
            return @event(nameof(blur), callback);
        }

        public static IAttribute focusin(Func<FocusEventArgs, Task> callback)
        {
            return @event(nameof(focusin), callback);
        }

        public static IAttribute focusout(Func<FocusEventArgs, Task> callback)
        {
            return @event(nameof(focusout), callback);
        }

        public static IAttribute mouseover(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(mouseover), callback);
        }

        public static IAttribute mouseout(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(mouseout), callback);
        }

        public static IAttribute mousemove(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(mousemove), callback);
        }

        public static IAttribute mousedown(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(mousedown), callback);
        }

        public static IAttribute click(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(click), callback);
        }

        public static IAttribute dblclick(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(dblclick), callback);
        }

        public static IAttribute wheel(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(wheel), callback);
        }

        public static IAttribute mousewheel(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(mousewheel), callback);
        }

        public static IAttribute contextmenu(Func<MouseEventArgs, Task> callback)
        {
            return @event(nameof(contextmenu), callback);
        }

        public static IAttribute drag(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(drag), callback);
        }

        public static IAttribute dragenter(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(dragenter), callback);
        }

        public static IAttribute dragleave(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(dragleave), callback);
        }

        public static IAttribute dragover(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(dragover), callback);
        }

        public static IAttribute dragstart(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(dragstart), callback);
        }

        public static IAttribute drop(Func<DragEventArgs, Task> callback)
        {
            return @event(nameof(drop), callback);
        }

        public static IAttribute keydown(Func<KeyboardEventArgs, Task> callback)
        {
            return @event(nameof(keydown), callback);
        }

        public static IAttribute keyup(Func<KeyboardEventArgs, Task> callback)
        {
            return @event(nameof(keyup), callback);
        }

        public static IAttribute keypress(Func<KeyboardEventArgs, Task> callback)
        {
            return @event(nameof(keypress), callback);
        }

        public static IAttribute change(Func<ChangeEventArgs, Task> callback)
        {
            return @event(nameof(change), callback);
        }

        public static IAttribute input(Func<ChangeEventArgs, Task> callback)
        {
            return @event(nameof(input), callback);
        }

        public static IAttribute reset(Func<EventArgs, Task> callback)
        {
            return @event(nameof(reset), callback);
        }

        public static IAttribute select(Func<EventArgs, Task> callback)
        {
            return @event(nameof(select), callback);
        }

        public static IAttribute selectstart(Func<EventArgs, Task> callback)
        {
            return @event(nameof(selectstart), callback);
        }

        public static IAttribute selectionchange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(selectionchange), callback);
        }

        public static IAttribute submit(Func<EventArgs, Task> callback)
        {
            return @event(nameof(submit), callback);
        }

        public static IAttribute beforecopy(Func<EventArgs, Task> callback)
        {
            return @event(nameof(beforecopy), callback);
        }

        public static IAttribute beforecut(Func<EventArgs, Task> callback)
        {
            return @event(nameof(beforecut), callback);
        }

        public static IAttribute beforepaste(Func<EventArgs, Task> callback)
        {
            return @event(nameof(beforepaste), callback);
        }

        public static IAttribute copy(Func<EventArgs, Task> callback)
        {
            return @event(nameof(copy), callback);
        }

        public static IAttribute paste(Func<EventArgs, Task> callback)
        {
            return @event(nameof(paste), callback);
        }

        public static IAttribute touchcancel(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchcancel), callback);
        }

        public static IAttribute touchend(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchend), callback);
        }

        public static IAttribute touchmove(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchmove), callback);
        }

        public static IAttribute touchstart(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchstart), callback);
        }

        public static IAttribute touchenter(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchenter), callback);
        }

        public static IAttribute touchleave(Func<TouchEventArgs, Task> callback)
        {
            return @event(nameof(touchleave), callback);
        }

        public static IAttribute pointercapture(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointercapture), callback);
        }

        public static IAttribute lostpointercapture(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(lostpointercapture), callback);
        }

        public static IAttribute pointercancel(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointercancel), callback);
        }

        public static IAttribute pointerdown(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerdown), callback);
        }

        public static IAttribute pointerenter(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerenter), callback);
        }

        public static IAttribute pointerleave(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerleave), callback);
        }

        public static IAttribute pointermove(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointermove), callback);
        }

        public static IAttribute pointerout(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerout), callback);
        }

        public static IAttribute pointerover(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerover), callback);
        }

        public static IAttribute pointerup(Func<PointerEventArgs, Task> callback)
        {
            return @event(nameof(pointerup), callback);
        }

        public static IAttribute canplay(Func<EventArgs, Task> callback)
        {
            return @event(nameof(canplay), callback);
        }

        public static IAttribute canplaythrough(Func<EventArgs, Task> callback)
        {
            return @event(nameof(canplaythrough), callback);
        }

        public static IAttribute cuechange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(cuechange), callback);
        }

        public static IAttribute durationchange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(durationchange), callback);
        }

        public static IAttribute emptied(Func<EventArgs, Task> callback)
        {
            return @event(nameof(emptied), callback);
        }

        public static IAttribute pause(Func<EventArgs, Task> callback)
        {
            return @event(nameof(pause), callback);
        }

        public static IAttribute play(Func<EventArgs, Task> callback)
        {
            return @event(nameof(play), callback);
        }

        public static IAttribute playing(Func<EventArgs, Task> callback)
        {
            return @event(nameof(playing), callback);
        }

        public static IAttribute ratechange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(ratechange), callback);
        }

        public static IAttribute seeked(Func<EventArgs, Task> callback)
        {
            return @event(nameof(seeked), callback);
        }

        public static IAttribute seeking(Func<EventArgs, Task> callback)
        {
            return @event(nameof(seeking), callback);
        }

        public static IAttribute stalled(Func<EventArgs, Task> callback)
        {
            return @event(nameof(stalled), callback);
        }

        public static IAttribute stop(Func<EventArgs, Task> callback)
        {
            return @event(nameof(stop), callback);
        }

        public static IAttribute suspend(Func<EventArgs, Task> callback)
        {
            return @event(nameof(suspend), callback);
        }

        public static IAttribute timeupdate(Func<EventArgs, Task> callback)
        {
            return @event(nameof(timeupdate), callback);
        }

        public static IAttribute volumechange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(volumechange), callback);
        }

        public static IAttribute waiting(Func<EventArgs, Task> callback)
        {
            return @event(nameof(waiting), callback);
        }

        public static IAttribute loadstart(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(loadstart), callback);
        }

        public static IAttribute timeout(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(timeout), callback);
        }

        public static IAttribute abort(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(abort), callback);
        }

        public static IAttribute load(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(load), callback);
        }

        public static IAttribute loadend(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(loadend), callback);
        }

        public static IAttribute progress(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(progress), callback);
        }

        public static IAttribute error(Func<ProgressEventArgs, Task> callback)
        {
            return @event(nameof(error), callback);
        }

        public static IAttribute activate(Func<EventArgs, Task> callback)
        {
            return @event(nameof(activate), callback);
        }

        public static IAttribute beforeactivate(Func<EventArgs, Task> callback)
        {
            return @event(nameof(beforeactivate), callback);
        }

        public static IAttribute beforedeactivate(Func<EventArgs, Task> callback)
        {
            return @event(nameof(beforedeactivate), callback);
        }

        public static IAttribute deactivate(Func<EventArgs, Task> callback)
        {
            return @event(nameof(deactivate), callback);
        }

        public static IAttribute ended(Func<EventArgs, Task> callback)
        {
            return @event(nameof(ended), callback);
        }

        public static IAttribute fullscreenchange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(fullscreenchange), callback);
        }

        public static IAttribute fullscreenerror(Func<EventArgs, Task> callback)
        {
            return @event(nameof(fullscreenerror), callback);
        }

        public static IAttribute loadeddata(Func<EventArgs, Task> callback)
        {
            return @event(nameof(loadeddata), callback);
        }

        public static IAttribute loadedmetadata(Func<EventArgs, Task> callback)
        {
            return @event(nameof(loadedmetadata), callback);
        }

        public static IAttribute pointerlockchange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(pointerlockchange), callback);
        }

        public static IAttribute pointerlockerror(Func<EventArgs, Task> callback)
        {
            return @event(nameof(pointerlockerror), callback);
        }

        public static IAttribute readystatechange(Func<EventArgs, Task> callback)
        {
            return @event(nameof(readystatechange), callback);
        }

        public static IAttribute scroll(Func<EventArgs, Task> callback)
        {
            return @event(nameof(scroll), callback);
        }
    }
}
