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
    public enum TB3SimulatitonState {
        Idle=0, Restart=1
    }
    public class StateSubscriber : MonoBehaviour, IRobotPartsController
    {
        private GameObject root;
        private string root_name;
        private PduIoConnector pdu_io;
        private IPduReader pdu_reader;

        private TB3SimulatitonState robot_state;

        public void Initialize(System.Object obj)
        {
            if(obj is not GameObject){
                Debug.LogError("Initialize error[StateReceiver]: Initialize argument is not GameObject.");
                return;
            }

            if (this.root == null)
            {
                Debug.Log("StateReceiver Init");
                this.root = obj as GameObject;
                this.root_name = string.Copy(this.root.transform.name);
                this.pdu_io = PduIoConnector.Get(this.root_name);

                var pdu_reader_name = root_name + "_" + this.topic_name + "Pdu";
                this.pdu_reader = this.pdu_io.GetReader(pdu_reader_name);
                if (this.pdu_reader == null)
                {
                    throw new ArgumentException("can not found State pdu:" + pdu_reader_name);
                }
            }
        }

        public string topic_type = "std_msgs/UInt32";
        public string topic_name = "sim_state";
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
        public void DoControl()
        {
            // Updateの間隔で本関数が呼ばれます。状態に合わせた処理は以下で行ってください。
            var state = this.pdu_reader.GetReadOps().GetDataUInt32("data");
            if(state!=0){
                robot_state = TB3SimulatitonState.Restart;
            }
            switch(robot_state){
                case TB3SimulatitonState.Restart:
                    var body = root.GetComponentsInChildren<Rigidbody>()[0];
                    body.isKinematic = true;
                    body.transform.position = new Vector3(0.0f, 1.5f, -3.2f); 
                    body.transform.rotation = UnityEngine.Quaternion.identity; 
                    body.isKinematic = false;
                    robot_state = TB3SimulatitonState.Idle;
                    break;
            }
        }
    }
}
