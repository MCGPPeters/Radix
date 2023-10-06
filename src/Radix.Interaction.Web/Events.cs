using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Web.Html.Data;
using Radix.Web.Html.Data.Names;
using Radix.Web.Html.Data.Names.Events;
using static Radix.Interaction.Event;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;
// ReSharper disable InconsistentNaming

namespace Radix.Interaction.Web;

public static class on
{
    public static Data.Attribute focus(Action<FocusEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focus, FocusEventArgs>(callback, nodeId);

    public static Data.Attribute blur(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<blur>(callback, nodeId);

    public static Data.Attribute focusin(Action<FocusEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focusin, FocusEventArgs>(callback, nodeId);

    public static Data.Attribute focusout(Action<FocusEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focusout, FocusEventArgs>(callback, nodeId);

    public static Data.Attribute mouseover(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseover, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute mouseout(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseout, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute mousemove(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousemove, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute mousedown(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousedown, MouseEventArgs>(callback, nodeId);
    public static Data.Attribute mouseup(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseup, MouseEventArgs>(callback, nodeId);
    public static Data.Attribute mousewheel(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousewheel, MouseEventArgs>(callback, nodeId);
    public static Data.Attribute mouseleave(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseleave, MouseEventArgs>(callback, nodeId);
    public static Data.Attribute mouseenter(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseenter, MouseEventArgs>(callback, nodeId);

    /// <summary>
    /// 
    /// </summary>
    public static Data.Attribute click(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<click, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute dblclick(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dblclick, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute wheel(Action<WheelEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<wheel, WheelEventArgs>(callback, nodeId);

    public static Data.Attribute contextmenu(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<contextmenu, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute drag(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<drag, DragEventArgs>(callback, nodeId);

    public static Data.Attribute dragenter(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragenter, DragEventArgs>(callback, nodeId);

    public static Data.Attribute dragleave(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragleave, DragEventArgs>(callback, nodeId);

    public static Data.Attribute dragover(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragover, DragEventArgs>(callback, nodeId);

    public static Data.Attribute dragstart(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragstart, DragEventArgs>(callback, nodeId);
    public static Data.Attribute dragend(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragend, DragEventArgs>(callback, nodeId);

    public static Data.Attribute drop(Action<DragEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<drop, DragEventArgs>(callback, nodeId);

    public static Data.Attribute keydown(Action<KeyboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keydown, KeyboardEventArgs>(callback, nodeId);

    public static Data.Attribute keyup(Action<KeyboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keyup, KeyboardEventArgs>(callback, nodeId);
    public static Data.Attribute keypress(Action<KeyboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keypress, KeyboardEventArgs>(callback, nodeId);

    public static Data.Attribute change(Action<ChangeEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<change, ChangeEventArgs>(callback, nodeId);
    public static Data.Attribute input(Action<ChangeEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<input, ChangeEventArgs>(callback, nodeId);
    public static Data.Attribute reset(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<reset>(callback, nodeId);
    public static Data.Attribute select(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<select>(callback, nodeId);
    public static Data.Attribute selectstart(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<selectstart>(callback, nodeId);

    public static Data.Attribute selectionchange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<selectionchange>(callback, nodeId);

    public static Data.Attribute submit(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<submit>(callback, nodeId);
    public static Data.Attribute beforecopy(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforecopy>(callback, nodeId);

    public static Data.Attribute beforecut(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforecut>(callback, nodeId);
    public static Data.Attribute beforepaste(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforepaste>(callback, nodeId);

    public static Data.Attribute copy(Action<ClipboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<copy, ClipboardEventArgs>(callback, nodeId);
    public static Data.Attribute cut(Action<ClipboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<cut, ClipboardEventArgs>(callback, nodeId);
    public static Data.Attribute paste(Action<ClipboardEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<paste, ClipboardEventArgs>(callback, nodeId);
    public static Data.Attribute touchcancel(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchcancel>(callback, nodeId);

    public static Data.Attribute touchend(Action<TouchEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchend, TouchEventArgs>(callback, nodeId);

    public static Data.Attribute touchmove(Action<TouchEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchmove, TouchEventArgs>(callback, nodeId);

    public static Data.Attribute touchstart(Action<TouchEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchstart, TouchEventArgs>(callback, nodeId);

    public static Data.Attribute touchenter(Action<TouchEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchenter, TouchEventArgs>(callback, nodeId);

    public static Data.Attribute touchleave(Action<TouchEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchleave, TouchEventArgs>(callback, nodeId);

    public static Data.Attribute pointercapture(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointercapture, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute lostpointercapture(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<lostpointercapture, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointercancel(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointercancel, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerdown(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerdown, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerenter(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerenter, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerleave(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerleave, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointermove(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointermove, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerout(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerout, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerover(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerover, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute pointerup(Action<PointerEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerup, PointerEventArgs>(callback, nodeId);

    public static Data.Attribute canplay(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<canplay>(callback, nodeId);
    public static Data.Attribute canplaythrough(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<canplaythrough>(callback, nodeId);

    public static Data.Attribute cuechange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<cuechange>(callback, nodeId);
    public static Data.Attribute durationchange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<durationchange>(callback, nodeId);

    public static Data.Attribute emptied(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<emptied>(callback, nodeId);
    public static Data.Attribute pause(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pause>(callback, nodeId);
    public static Data.Attribute play(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<play>(callback, nodeId);
    public static Data.Attribute playing(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<playing>(callback, nodeId);
    public static Data.Attribute ratechange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<ratechange>(callback, nodeId);

    public static Data.Attribute seeked(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<seeked>(callback, nodeId);
    public static Data.Attribute seeking(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<seeking>(callback, nodeId);
    public static Data.Attribute stalled(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<stalled>(callback, nodeId);
    public static Data.Attribute stop(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<stop>(callback, nodeId);
    public static Data.Attribute suspend(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<suspend>(callback, nodeId);
    public static Data.Attribute timeupdate(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<timeupdate>(callback, nodeId);

    public static Data.Attribute volumechange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<volumechange>(callback, nodeId);

    public static Data.Attribute waiting(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<waiting>(callback, nodeId);
    public static Data.Attribute loadstart(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadstart, ProgressEventArgs>(callback, nodeId);

    public static Data.Attribute timeout(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<timeout, ProgressEventArgs>(callback, nodeId);

    public static Data.Attribute abort(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<abort, ProgressEventArgs>(callback, nodeId);
    public static Data.Attribute load(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<load, ProgressEventArgs>(callback, nodeId);
    public static Data.Attribute loadend(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadend, ProgressEventArgs>(callback, nodeId);

    public static Data.Attribute progress(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<progress>(callback, nodeId);

    public static Data.Attribute error(Action<ErrorEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<error, ErrorEventArgs>(callback, nodeId);
    public static Data.Attribute activate(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<activate, EventArgs>(callback, nodeId);
    public static Data.Attribute beforeactivate(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforeactivate>(callback, nodeId);

    public static Data.Attribute beforedeactivate(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforedeactivate>(callback, nodeId);

    public static Data.Attribute deactivate(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<deactivate>(callback, nodeId);

    public static Data.Attribute ended(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<ended>(callback, nodeId);
    public static Data.Attribute fullscreenchange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<fullscreenchange>(callback, nodeId);

    public static Data.Attribute fullscreenerror(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<fullscreenerror>(callback, nodeId);

    public static Data.Attribute loadeddata(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadeddata>(callback, nodeId);

    public static Data.Attribute loadedmetadata(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadedmetadata>(callback, nodeId);

    public static Data.Attribute pointerlockchange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerlockchange>(callback, nodeId);

    public static Data.Attribute pointerlockerror(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerlockerror>(callback, nodeId);

    public static Data.Attribute readystatechange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<readystatechange>(callback, nodeId);

    public static Data.Attribute scroll(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<scroll>(callback, nodeId);
}
