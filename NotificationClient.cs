using System;
using NAudio;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Windows.Forms;

namespace HeadphonesPlugged
{
    public class NotificationClient : NAudio.CoreAudioApi.Interfaces.IMMNotificationClient
    {
        Class1 client;
        public NotificationClient(Class1 cl)
        {
            this.client=cl;
        }
        void IMMNotificationClient.OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            Console.WriteLine("OnDeviceStateChanged\n Device Id -->{0} : Device State {1}", deviceId, newState);
        }

        void IMMNotificationClient.OnDeviceAdded(string pwstrDeviceId)
        {

        }
        void IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {

        }
        void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {

        }
        void IMMNotificationClient.OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key_in)
        {
            PropertyKey key = PropertyKeys.PKEY_AudioEndpoint_FormFactor;
            if (key_in.propertyId == key.propertyId)
            {
                var endp = new NAudio.CoreAudioApi.MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                var store = endp.Properties;
                bool isHeadPhone = false;
                for (var index = 0; index < store.Count; index++)
                {
                    if (store.Get(index).Equals(key_in))
                    {
                        var value = (uint)store.GetValue(index).Value;
                        const uint formHeadphones = 3;
                        const uint formHeadset = 5;
                        const uint formSpeaker = 1;
                        if (value == formHeadphones || value == formHeadset)
                        {
                            isHeadPhone = true;
                            break;
                        }
                    }
                }
                // if (isHeadPhone)
                //     Console.WriteLine("Headphones CONNECTED.");
                // else
                //     Console.WriteLine("Headphones DISCONNECTED.");
                // this.client.RaiseEvent(new DeviceEventArgs(isHeadPhone));
                if (isHeadPhone)
                    this.client.RaiseHeadphonesPluggedEvent();
                else
                    this.client.RaiseHeadphonesUnpluggedEvent();
            }
        }
    }
}