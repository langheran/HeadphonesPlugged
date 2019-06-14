#SingleInstance, Force
#Persistent
#include CLR.ahk

; Compile the helper class. This could be pre-compiled.
GoSub, CreateMonitor
; Create our test object, which simply exposes a single event.
OnExit, ExitAppSub
return

CreateMonitor:
FileRead c#, EventHelper.cs
global helperAsm := CLR_CompileC#(c#)
global helper := helperAsm.CreateInstance("EventHelper")
global asm := CLR_LoadLibrary("HeadphonesPlugged.dll")
global obj := asm.CreateInstance("HeadphonesPlugged.Class1")
GoSub, AddEventHandlers
;obj.RaiseHeadphonesPluggedEvent()
;obj.RaiseHeadphonesUnpluggedEvent()
obj.MonitorDeviceChanges()
return

AddEventHandlers:
helper.AddHandler(obj, "OnHeadphonesPluggedEvent", "" RegisterCallback("HeadphonesPluggedEventHandler",,, 1))
helper.AddHandler(obj, "OnHeadphonesUnpluggedEvent", "" RegisterCallback("HeadphonesUnpluggedEventHandler",,, 1))
return

HeadphonesPlugged:
Tooltip, Headphones CONNECTED!
return

HeadphonesUnplugged:
Tooltip, Headphones DISCONNECTED!
return

HeadphonesPluggedEventHandler(pprm)
{
    prm := ComObject(0x200C, pprm)
    SetTimer, HeadphonesPlugged, -10
    return 1
}

HeadphonesUnpluggedEventHandler(pprm)
{
    prm := ComObject(0x200C, pprm)
    SetTimer, HeadphonesUnplugged, -10
    return 1
}

ExitAppSub:
ExitApp