using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Radix.Web.Html.Data;
using Radix.Web.Html.Data.Names;
using Radix.Web.Html.Data.Names.Events;
using static Radix.Interaction.Event;


namespace Radix.Interaction.Web;

public static class on
{
    public static Data.Event focus => Create<focus>();

    public static Data.Event blur => Create<blur>();

    public static Radix.Interaction.Data.Event focusin => Create<focusin>();

    public static Radix.Interaction.Data.Event focusout => Create<focusout>();

    public static Radix.Interaction.Data.Event mouseover => Create<mouseover>();

    public static Radix.Interaction.Data.Event mouseout => Create<mouseout>();

    public static Radix.Interaction.Data.Event mousemove => Create<mousemove>();

    public static Radix.Interaction.Data.Event mousedown => Create<mousedown>();

    public static Data.Event click => Create<click>();

    public static Radix.Interaction.Data.Event dblclick => Create<dblclick>();

    public static Radix.Interaction.Data.Event wheel => Create<wheel>();

    public static Radix.Interaction.Data.Event mousewheel => Create<mousewheel>();

    public static Radix.Interaction.Data.Event contextmenu => Create<contextmenu>();

    public static Radix.Interaction.Data.Event drag => Create<drag>();

    public static Radix.Interaction.Data.Event dragenter => Create<dragenter>();

    public static Radix.Interaction.Data.Event dragleave => Create<dragleave>();

    public static Radix.Interaction.Data.Event dragover => Create<dragover>();

    public static Radix.Interaction.Data.Event dragstart => Create<dragstart>();

    public static Radix.Interaction.Data.Event drop => Create<drop>();

    public static Radix.Interaction.Data.Event keydown => Create<keydown>();

    public static Radix.Interaction.Data.Event keyup => Create<keyup>();
    public static Radix.Interaction.Data.Event keypress => Create<keypress>();

    public static Radix.Interaction.Data.Event change => Create<change>();
    public static Radix.Interaction.Data.Event input => Create<input>();
    public static Radix.Interaction.Data.Event reset => Create<reset>();
    public static Radix.Interaction.Data.Event select => Create<select>();
    public static Radix.Interaction.Data.Event selectstart => Create<selectstart>();

    public static Radix.Interaction.Data.Event selectionchange => Create<selectionchange>();

    public static Radix.Interaction.Data.Event submit => Create<submit>();
    public static Radix.Interaction.Data.Event beforecopy => Create<beforecopy>();

    public static Radix.Interaction.Data.Event beforecut => Create<beforecut>();
    public static Radix.Interaction.Data.Event beforepaste => Create<beforepaste>();

    public static Radix.Interaction.Data.Event copy => Create<copy>();
    public static Radix.Interaction.Data.Event paste => Create<paste>();
    public static Radix.Interaction.Data.Event touchcancel => Create<touchcancel>();

    public static Radix.Interaction.Data.Event touchend => Create<touchend>();

    public static Radix.Interaction.Data.Event touchmove => Create<touchmove>();

    public static Radix.Interaction.Data.Event touchstart => Create<touchstart>();

    public static Radix.Interaction.Data.Event touchenter => Create<touchenter>();

    public static Radix.Interaction.Data.Event touchleave => Create<touchleave>();

    public static Radix.Interaction.Data.Event pointercapture => Create<pointercapture>();

    public static Radix.Interaction.Data.Event lostpointercapture => Create<lostpointercapture>();

    public static Radix.Interaction.Data.Event pointercancel => Create<pointercancel>();

    public static Radix.Interaction.Data.Event pointerdown => Create<pointerdown>();

    public static Radix.Interaction.Data.Event pointerenter => Create<pointerenter>();

    public static Radix.Interaction.Data.Event pointerleave => Create<pointerleave>();

    public static Radix.Interaction.Data.Event pointermove => Create<pointermove>();

    public static Radix.Interaction.Data.Event pointerout => Create<pointerout>();

    public static Radix.Interaction.Data.Event pointerover => Create<pointerover>();

    public static Radix.Interaction.Data.Event pointerup => Create<pointerup>();

    public static Radix.Interaction.Data.Event canplay => Create<canplay>();
    public static Radix.Interaction.Data.Event canplaythrough => Create<canplaythrough>();

    public static Radix.Interaction.Data.Event cuechange => Create<cuechange>();
    public static Radix.Interaction.Data.Event durationchange => Create<durationchange>();

    public static Radix.Interaction.Data.Event emptied => Create<emptied>();
    public static Radix.Interaction.Data.Event pause => Create<pause>();
    public static Radix.Interaction.Data.Event play => Create<play>();
    public static Radix.Interaction.Data.Event playing => Create<playing>();
    public static Radix.Interaction.Data.Event ratechange => Create<ratechange>();

    public static Radix.Interaction.Data.Event seeked => Create<seeked>();
    public static Radix.Interaction.Data.Event seeking => Create<seeking>();
    public static Radix.Interaction.Data.Event stalled => Create<stalled>();
    public static Radix.Interaction.Data.Event stop => Create<stop>();
    public static Radix.Interaction.Data.Event suspend => Create<suspend>();
    public static Radix.Interaction.Data.Event timeupdate => Create<timeupdate>();

    public static Radix.Interaction.Data.Event volumechange => Create<volumechange>();

    public static Radix.Interaction.Data.Event waiting => Create<waiting>();
    public static Radix.Interaction.Data.Event loadstart => Create<loadstart>();

    public static Radix.Interaction.Data.Event timeout => Create<timeout>();

    public static Radix.Interaction.Data.Event abort => Create<abort>();
    public static Radix.Interaction.Data.Event load => Create<load>();
    public static Radix.Interaction.Data.Event loadend => Create<loadend>();

    public static Radix.Interaction.Data.Event progress => Create<progress>();

    public static Radix.Interaction.Data.Event error => Create<error>();
    public static Radix.Interaction.Data.Event activate => Create<activate>();
    public static Radix.Interaction.Data.Event beforeactivate => Create<beforeactivate>();

    public static Radix.Interaction.Data.Event beforedeactivate => Create<beforedeactivate>();

    public static Radix.Interaction.Data.Event deactivate => Create<deactivate>();

    public static Radix.Interaction.Data.Event ended => Create<ended>();
    public static Radix.Interaction.Data.Event fullscreenchange => Create<fullscreenchange>();

    public static Radix.Interaction.Data.Event fullscreenerror => Create<fullscreenerror>();

    public static Radix.Interaction.Data.Event loadeddata => Create<loadeddata>();

    public static Radix.Interaction.Data.Event loadedmetadata => Create<loadedmetadata>();

    public static Radix.Interaction.Data.Event pointerlockchange => Create<pointerlockchange>();

    public static Radix.Interaction.Data.Event pointerlockerror => Create<pointerlockerror>();

    public static Radix.Interaction.Data.Event readystatechange => Create<readystatechange>();

    public static Radix.Interaction.Data.Event scroll => Create<scroll>();
}
