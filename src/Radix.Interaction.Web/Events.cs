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

    public static Data.Event focusin => Create<focusin>();

    public static Data.Event focusout => Create<focusout>();

    public static Data.Event mouseover => Create<mouseover>();

    public static Data.Event mouseout => Create<mouseout>();

    public static Data.Event mousemove => Create<mousemove>();

    public static Data.Event mousedown => Create<mousedown>();

    public static Data.Event click => Create<click>();

    public static Data.Event dblclick => Create<dblclick>();

    public static Data.Event wheel => Create<wheel>();

    public static Data.Event mousewheel => Create<mousewheel>();

    public static Data.Event contextmenu => Create<contextmenu>();

    public static Data.Event drag => Create<drag>();

    public static Data.Event dragenter => Create<dragenter>();

    public static Data.Event dragleave => Create<dragleave>();

    public static Data.Event dragover => Create<dragover>();

    public static Data.Event dragstart => Create<dragstart>();

    public static Data.Event drop => Create<drop>();

    public static Data.Event keydown => Create<keydown>();

    public static Data.Event keyup => Create<keyup>();
    public static Data.Event keypress => Create<keypress>();

    public static Data.Event change => Create<change>();
    public static Data.Event input => Create<input>();
    public static Data.Event reset => Create<reset>();
    public static Data.Event select => Create<select>();
    public static Data.Event selectstart => Create<selectstart>();

    public static Data.Event selectionchange => Create<selectionchange>();

    public static Data.Event submit => Create<submit>();
    public static Data.Event beforecopy => Create<beforecopy>();

    public static Data.Event beforecut => Create<beforecut>();
    public static Data.Event beforepaste => Create<beforepaste>();

    public static Data.Event copy => Create<copy>();
    public static Data.Event paste => Create<paste>();
    public static Data.Event touchcancel => Create<touchcancel>();

    public static Data.Event touchend => Create<touchend>();

    public static Data.Event touchmove => Create<touchmove>();

    public static Data.Event touchstart => Create<touchstart>();

    public static Data.Event touchenter => Create<touchenter>();

    public static Data.Event touchleave => Create<touchleave>();

    public static Data.Event pointercapture => Create<pointercapture>();

    public static Data.Event lostpointercapture => Create<lostpointercapture>();

    public static Data.Event pointercancel => Create<pointercancel>();

    public static Data.Event pointerdown => Create<pointerdown>();

    public static Data.Event pointerenter => Create<pointerenter>();

    public static Data.Event pointerleave => Create<pointerleave>();

    public static Data.Event pointermove => Create<pointermove>();

    public static Data.Event pointerout => Create<pointerout>();

    public static Data.Event pointerover => Create<pointerover>();

    public static Data.Event pointerup => Create<pointerup>();

    public static Data.Event canplay => Create<canplay>();
    public static Data.Event canplaythrough => Create<canplaythrough>();

    public static Data.Event cuechange => Create<cuechange>();
    public static Data.Event durationchange => Create<durationchange>();

    public static Data.Event emptied => Create<emptied>();
    public static Data.Event pause => Create<pause>();
    public static Data.Event play => Create<play>();
    public static Data.Event playing => Create<playing>();
    public static Data.Event ratechange => Create<ratechange>();

    public static Data.Event seeked => Create<seeked>();
    public static Data.Event seeking => Create<seeking>();
    public static Data.Event stalled => Create<stalled>();
    public static Data.Event stop => Create<stop>();
    public static Data.Event suspend => Create<suspend>();
    public static Data.Event timeupdate => Create<timeupdate>();

    public static Data.Event volumechange => Create<volumechange>();

    public static Data.Event waiting => Create<waiting>();
    public static Data.Event loadstart => Create<loadstart>();

    public static Data.Event timeout => Create<timeout>();

    public static Data.Event abort => Create<abort>();
    public static Data.Event load => Create<load>();
    public static Data.Event loadend => Create<loadend>();

    public static Data.Event progress => Create<progress>();

    public static Data.Event error => Create<error>();
    public static Data.Event activate => Create<activate>();
    public static Data.Event beforeactivate => Create<beforeactivate>();

    public static Data.Event beforedeactivate => Create<beforedeactivate>();

    public static Data.Event deactivate => Create<deactivate>();

    public static Data.Event ended => Create<ended>();
    public static Data.Event fullscreenchange => Create<fullscreenchange>();

    public static Data.Event fullscreenerror => Create<fullscreenerror>();

    public static Data.Event loadeddata => Create<loadeddata>();

    public static Data.Event loadedmetadata => Create<loadedmetadata>();

    public static Data.Event pointerlockchange => Create<pointerlockchange>();

    public static Data.Event pointerlockerror => Create<pointerlockerror>();

    public static Data.Event readystatechange => Create<readystatechange>();

    public static Data.Event scroll => Create<scroll>();
}
