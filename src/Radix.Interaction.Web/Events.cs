using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radix.Web.Html.Data;
using Radix.Web.Html.Data.Names;
using Radix.Web.Html.Data.Names.Events;
using static Radix.Interaction.Event;
// ReSharper disable InconsistentNaming

namespace Radix.Interaction.Web;

public static class on
{
    public static Data.Attribute focus(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focus>(callback, nodeId);

    public static Data.Attribute blur(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<blur>(callback, nodeId);

    public static Data.Attribute focusin(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focusin>(callback, nodeId);

    public static Data.Attribute focusout(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<focusout>(callback, nodeId);

    public static Data.Attribute mouseover(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseover, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute mouseout(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mouseout>(callback, nodeId);

    public static Data.Attribute mousemove(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousemove>(callback, nodeId);

    public static Data.Attribute mousedown(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousedown>(callback, nodeId);

    /// <summary>
    /// 
    /// </summary>
    public static Data.Attribute click(Action<MouseEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<click, MouseEventArgs>(callback, nodeId);

    public static Data.Attribute dblclick(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dblclick>(callback, nodeId);

    public static Data.Attribute wheel(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<wheel>(callback, nodeId);

    public static Data.Attribute mousewheel(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<mousewheel>(callback, nodeId);

    public static Data.Attribute contextmenu(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<contextmenu>(callback, nodeId);

    public static Data.Attribute drag(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<drag>(callback, nodeId);

    public static Data.Attribute dragenter(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragenter>(callback, nodeId);

    public static Data.Attribute dragleave(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragleave>(callback, nodeId);

    public static Data.Attribute dragover(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragover>(callback, nodeId);

    public static Data.Attribute dragstart(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<dragstart>(callback, nodeId);

    public static Data.Attribute drop(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<drop>(callback, nodeId);

    public static Data.Attribute keydown(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keydown>(callback, nodeId);

    public static Data.Attribute keyup(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keyup>(callback, nodeId);
    public static Data.Attribute keypress(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<keypress>(callback, nodeId);

    public static Data.Attribute change(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<change>(callback, nodeId);
    public static Data.Attribute input(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<input>(callback, nodeId);
    public static Data.Attribute reset(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<reset>(callback, nodeId);
    public static Data.Attribute select(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<select>(callback, nodeId);
    public static Data.Attribute selectstart(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<selectstart>(callback, nodeId);

    public static Data.Attribute selectionchange(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<selectionchange>(callback, nodeId);

    public static Data.Attribute submit(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<submit>(callback, nodeId);
    public static Data.Attribute beforecopy(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforecopy>(callback, nodeId);

    public static Data.Attribute beforecut(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforecut>(callback, nodeId);
    public static Data.Attribute beforepaste(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<beforepaste>(callback, nodeId);

    public static Data.Attribute copy(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<copy>(callback, nodeId);
    public static Data.Attribute paste(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<paste>(callback, nodeId);
    public static Data.Attribute touchcancel(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchcancel>(callback, nodeId);

    public static Data.Attribute touchend(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchend>(callback, nodeId);

    public static Data.Attribute touchmove(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchmove>(callback, nodeId);

    public static Data.Attribute touchstart(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchstart>(callback, nodeId);

    public static Data.Attribute touchenter(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchenter>(callback, nodeId);

    public static Data.Attribute touchleave(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<touchleave>(callback, nodeId);

    public static Data.Attribute pointercapture(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointercapture>(callback, nodeId);

    public static Data.Attribute lostpointercapture(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<lostpointercapture>(callback, nodeId);

    public static Data.Attribute pointercancel(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointercancel>(callback, nodeId);

    public static Data.Attribute pointerdown(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerdown>(callback, nodeId);

    public static Data.Attribute pointerenter(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerenter>(callback, nodeId);

    public static Data.Attribute pointerleave(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerleave>(callback, nodeId);

    public static Data.Attribute pointermove(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointermove>(callback, nodeId);

    public static Data.Attribute pointerout(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerout>(callback, nodeId);

    public static Data.Attribute pointerover(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerover>(callback, nodeId);

    public static Data.Attribute pointerup(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<pointerup>(callback, nodeId);

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
    public static Data.Attribute loadstart(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadstart>(callback, nodeId);

    public static Data.Attribute timeout(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<timeout>(callback, nodeId);

    public static Data.Attribute abort(Action<ProgressEventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<abort, ProgressEventArgs>(callback, nodeId);
    public static Data.Attribute load(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<load>(callback, nodeId);
    public static Data.Attribute loadend(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<loadend>(callback, nodeId);

    public static Data.Attribute progress(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<progress>(callback, nodeId);

    public static Data.Attribute error(Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) => Create<error>(callback, nodeId);
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
