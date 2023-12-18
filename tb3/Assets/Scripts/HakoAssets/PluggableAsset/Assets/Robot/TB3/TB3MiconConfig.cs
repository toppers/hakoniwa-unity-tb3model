using Hakoniwa.PluggableAsset.Assets.Robot.Parts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class TB3MiconConfigShmPduReader
    {
        public string type = "";
        public string org_name = "";
        public string name = null;
        public string class_name = "Hakoniwa.PluggableAsset.Communication.Pdu.Raw.RawPduReader";
        public string class_path = null;
        public string conv_class_name = "Hakoniwa.PluggableAsset.Communication.Pdu.Raw.RawPduReaderConverter";
        public string conv_class_path = null;
        public int channel_id = 0;
        public int pdu_size = 196;
        public string method_type = "SHM";
        public TB3MiconConfigShmPduReader(string type, string org_name, int channel_id, int pdu_size)
        {
            this.type = type;
            this.org_name = org_name;
            this.channel_id = channel_id;
            this.pdu_size = pdu_size;
        }
    }
    
    [System.Serializable]
    public class TB3MiconConfigShmPduWriter
    {
        public string type = "ev3_msgs/Ev3PduSensor";
        public string org_name = "ev3_sensor";
        public string name = null;
        public string class_name = "Hakoniwa.PluggableAsset.Communication.Pdu.Raw.RawPduWriter";
        public string class_path = null;
        public string conv_class_name = "Hakoniwa.PluggableAsset.Communication.Pdu.Raw.RawPduWriterConverter";
        public string conv_class_path = null;
        public int channel_id = 1;
        public int pdu_size = 248;
        public string method_type = "SHM";
        public TB3MiconConfigShmPduWriter(string type, string org_name, int channel_id, int pdu_size)
        {
            this.type = type;
            this.org_name = org_name;
            this.channel_id = channel_id;
            this.pdu_size = pdu_size;
        }
    }
    [System.Serializable]
    public class TB3MiconConfigSettingsContainer
    {
        public string name = "micon_setting";
        public TB3MiconConfigShmPduReader[] shm_pdu_readers =
        {
            new TB3MiconConfigShmPduReader(
                "geometry_msgs/Twist",
                "cmd_vel",
                0,
                48),
        };
        public TB3MiconConfigShmPduWriter[] shm_pdu_writers =
        {
            new TB3MiconConfigShmPduWriter(
                "sensor_msgs/JointState",
                "joint_states",
                1,
                440),
            new TB3MiconConfigShmPduWriter(
                "sensor_msgs/Imu",
                "imu",
                2,
                432),
            new TB3MiconConfigShmPduWriter(
                "nav_msgs/Odometry",
                "odom",
                3,
                944),
            new TB3MiconConfigShmPduWriter(
                "tf2_msgs/TFMessage",
                "tf",
                4,
                320),
            // new TB3MiconConfigShmPduWriter(
            //     "sensor_msgs/Image",
            //     "image",
            //     5,
            //     1229080),
            // new TB3MiconConfigShmPduWriter(
            //     "sensor_msgs/CompressedImage",
            //     "image/compressed",
            //     6,
            //     1229064),
            // new TB3MiconConfigShmPduWriter(
            //     "sensor_msgs/CameraInfo",
            //     "camera_info",
            //     7,
            //     580),
            new TB3MiconConfigShmPduWriter(
                "sensor_msgs/LaserScan",
                "scan",
                5,
                1800),
        };
    }

    public class TB3MiconConfig : MonoBehaviour, IMiconSettings
    {
        public bool on = false;
        public TB3MiconConfigSettingsContainer settings;

        public string GetSettings(string name)
        {
            this.settings.name = name;
            foreach (var e in this.settings.shm_pdu_readers)
            {
                e.name = name + "_" + e.org_name;
            }
            foreach (var e in this.settings.shm_pdu_writers)
            {
                e.name = name + "_" + e.org_name;
            }
            return JsonConvert.SerializeObject(this.settings, Formatting.Indented);
        }

        public bool isEnabled()
        {
            return this.on;
        }
    }
}
