using System;
using NAudio.CoreAudioApi;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RGiesecke.DllExport;

namespace HeadphonesPlugged
{
    public class DeviceEventArgs : EventArgs
    {
        private bool HeadphonesPlugged;
        public bool headphonesPlugged
        {
            get
            {
                return headphonesPlugged;
            }

            set
            {
                headphonesPlugged = value;
            }
        }
        public DeviceEventArgs(bool headphonesPlugged)
        {
            this.headphonesPlugged = headphonesPlugged;
        }
    }
    public class Class1
    {
        public NAudio.CoreAudioApi.MMDeviceEnumerator enumerator;
        public NotificationClient nt;
        public event System.EventHandler OnDeviceEvent;
        public event System.EventHandler OnHeadphonesPluggedEvent;
        public event System.EventHandler OnHeadphonesUnpluggedEvent;
        public Class1()
        {

        }
        [DllExport("MonitorDeviceChanges", CallingConvention = CallingConvention.Cdecl)]
        public void MonitorDeviceChanges()
        {
            var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            // Allows you to enumerate rendering devices in certain states
            var endpoints = enumerator.EnumerateAudioEndPoints(
                DataFlow.Render,
                DeviceState.Unplugged | DeviceState.Active);
            // Aswell as hook to the actual event
            this.nt = new NotificationClient(this);
            enumerator.RegisterEndpointNotificationCallback(nt);
            // MessageBox.Show("Monitoring Devices");
        }
        public void RaiseEvent(DeviceEventArgs e)
        {
            if (OnDeviceEvent != null)
                OnDeviceEvent(this, e);
        }
        public void RaiseHeadphonesPluggedEvent()
        {
            // MessageBox.Show("RaiseHeadphonesPluggedEvent Start");
            if (OnHeadphonesPluggedEvent != null)
                OnHeadphonesPluggedEvent(this, System.EventArgs.Empty);
            // MessageBox.Show("RaiseHeadphonesPluggedEvent End");
        }
        public void RaiseHeadphonesUnpluggedEvent()
        {
            // MessageBox.Show("RaiseHeadphonesUnpluggedEvent Start");
            if (OnHeadphonesUnpluggedEvent != null)
                OnHeadphonesUnpluggedEvent(this, System.EventArgs.Empty);
            // MessageBox.Show("RaiseHeadphonesUnpluggedEvent End");
        }
    }
}
