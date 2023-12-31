using Hakoniwa.PluggableAsset;
using Hakoniwa.PluggableAsset.Assets.Robot.Parts;
using Hakoniwa.PluggableAsset.Communication.Connector;
using Hakoniwa.PluggableAsset.Communication.Pdu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public enum TB3RobotState {
        Idle=0, Crashed=1
    }
    public class StatePublisher : MonoBehaviour, IRobotPartsSensor
    {
        private GameObject root;
        private string root_name;
        private PduIoConnector pdu_io;
        private IPduWriter pdu_writer;
        private TB3RobotState robot_state = TB3RobotState.Idle;
        private TB3RobotState previous_robot_state = TB3RobotState.Idle;

        public void Initialize(System.Object obj)
        {
            if(obj is not GameObject){
                Debug.LogError("Initialize error[StatePublisher]: Initialize argument is not GameObject.");
                return;
            }

            if (this.root == null)
            {
                Debug.Log("StatePublisher Init");
                this.root = obj as GameObject;
                this.root_name = string.Copy(this.root.transform.name);
                this.pdu_io = PduIoConnector.Get(this.root_name);
                if (this.pdu_io == null)
                {
                    throw new ArgumentException("can not found pdu_io:" + root_name);
                }
                var pdu_writer_name = root_name + "_" + this.topic_name + "Pdu";
                this.pdu_writer = this.pdu_io.GetWriter(pdu_writer_name);
                if (this.pdu_writer == null)
                {
                    throw new ArgumentException("can not found Robot State pdu:" + pdu_writer_name);
                }
                UpdateSensorValues();
            }
        }
        public void UpdateSensorValues()
        {
            // TODO ここで状態更新を行って下さい。変更した値がPublishされます。
            if(false){
                robot_state = TB3RobotState.Crashed;
            }

            if(robot_state!=previous_robot_state){
                pdu_writer.GetWriteOps().SetData("data", (int)robot_state);
                previous_robot_state = robot_state;
            }
        }

        public string topic_type = "std_msgs/UInt32";        
        public string topic_name = "robot_state";
        public int update_cycle = 10;

        public RosTopicMessageConfig[] getRosConfig()
        {
            RosTopicMessageConfig[] cfg = new RosTopicMessageConfig[1];
            cfg[0] = new RosTopicMessageConfig();
            cfg[0].topic_message_name = this.topic_name;
            cfg[0].topic_type_name = this.topic_type;
            cfg[0].sub = false;
            cfg[0].pub_option = new RostopicPublisherOption();
            cfg[0].pub_option.cycle_scale = this.update_cycle;
            cfg[0].pub_option.latch = false;
            cfg[0].pub_option.queue_size = 1;
            return cfg;
        }
        public bool isAttachedSpecificController()
        {
            return false;
        }
    }
}
