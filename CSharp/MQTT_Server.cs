// https://developpaper.com/%E2%98%85-unit-actual-combat-advanced-c-mqtt-message-queue-telemetry-transmission-protocol-detailed-explanation-8/

using MQTTnet;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MQTT
{
    public class MqttServer
    {
        private IMqttServer m_MqttServer = null;

        private List<MqttClientObject> m_MqttClientObject = new List<MqttClientObject>();

        public Action<string, string> MessageCallback = null;

        public bool IsConnect
        {
            get
            {
                if (m_MqttServer == null)
                    return false;
                return m_MqttServer.IsStarted;
            }
        }

        public MqttServer()
        {
            m_MqttClientObject.Clear();
        }

        public void OpenMqttServer(int port = 3883)
        {
            var server_logger = new MqttNetEventLogger("server log");
            server_logger.LogMessagePublished += (sender, e) =>
            {
                //Debug.Log(e.LogMessage.ToString());
            };

            m_MqttServer = new MqttFactory().CreateMqttServer(server_logger);
            var optionbuilder = new MqttServerOptionsBuilder();
            //The default port is 1883, which can be set here
            optionbuilder.WithDefaultEndpointPort(port);
            optionbuilder.WithConnectionValidator(WithConnectionValidator);

            m_MqttServer.StartedHandler = new MqttServerStartedHandlerDelegate(StartedHandler);
            m_MqttServer.StoppedHandler = new MqttServerStoppedHandlerDelegate(StoppedHandler);
            m_MqttServer.UseClientConnectedHandler(UseClientConnectedHandler);
            m_MqttServer.UseClientDisconnectedHandler(UseClientDisconnectedHandler);
            m_MqttServer.UseApplicationMessageReceivedHandler(UseApplicationMessageReceivedHandler);
            m_MqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(ClientSubscribedTopicHandler);
            m_MqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(ClientUnsubscribedTopicHandler);

            m_MqttServer.StartAsync(optionbuilder.Build());
        }

        public void Disconnect()
        {
            m_MqttServer.StopAsync();
        }

        public async Task<int> ConnectClientCount()
        {
            if (m_MqttServer == null) return 0;
            var msgs = await m_MqttServer.GetClientStatusAsync();
            return msgs.Count;
        }

        private void WithConnectionValidator(MqttConnectionValidatorContext context)
        {
            //Here, you can verify the username and password of the connected client, or you can choose not to verify
            if (string.IsNullOrWhiteSpace(context.Username) || string.IsNullOrWhiteSpace(context.Password))
            {
                Debug. Log ($"user: {context. ClientID} login failed, user information is empty");

                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                context. Reasonstring = "login failed, user name or password is empty";
                return;
            }
            if (context.Username.Equals("admin") && context.Password.Equals("123456"))
            {
                m_MqttClientObject.Add(new MqttClientObject() { ClientID = context.ClientId, UserName = context.Username, PassWord = context.Password });
                context.ReasonCode = MqttConnectReasonCode.Success;
                context. Reasonstring = "login succeeded";
            }
        }

        private void StartedHandler(EventArgs eventArgs)
        {
            Debug. Log ("service: started!");

            Loom.RunAsync(() =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    MessageCallback?. Invoke ("start server", "service: started!");
                });
            });
        }

        private void StoppedHandler(EventArgs eventArgs)
        {
            Debug. Log ("service: stopped!");
        }

        /// <summary>
        ///Client connection message
        /// </summary>
        /// <param name="eventArgs"></param>
        private void UseClientConnectedHandler(MqttServerClientConnectedEventArgs eventArgs)
        {
            Debug. Log ($"service: client connected ClientID: {EventArgs. ClientID}");

            Loom. Runasync (() = > {room. Queueonmainthread (() = > {messagecallback?. invoke (EventArgs. ClientID, "client connected");});});
        }

        /// <summary>
        ///Client disconnect message
        /// </summary>
        /// <param name="eventArgs"></param>
        private void UseClientDisconnectedHandler(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            Debug. Log ($"service: client disconnected ClientID: {EventArgs. ClientID} type: {EventArgs. Disconnecttype}");
            for (int i = 0; i < m_MqttClientObject.Count; i++)
            {
                if (m_MqttClientObject[i].ClientID.Equals(eventArgs.ClientId))
                {
                    m_MqttClientObject.RemoveAt(i);
                    break;
                }
            }

            Loom. Runasync (() = > {room. Queueonmainthread (() = > {messagecallback?. invoke (EventArgs. ClientID, "client disconnected");});});
        }

        /// <summary>
        ///Received application message processing
        /// </summary>
        /// <param name="eventArgs"></param>
        private void UseApplicationMessageReceivedHandler(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(eventArgs.ClientId)) return;

            var msg = eventArgs.ApplicationMessage;
            var topic = msg.Topic;
            var payload = msg.ConvertPayloadToString();
            Debug. Log ($"service: ClientID: '{EventArgs. ClientID}' topic: '{topic}' payload: '{payload}'");

            Loom.RunAsync(() =>
            {
                Loom.QueueOnMainThread(() =>
                {
                    MessageCallback?.Invoke(eventArgs.ClientId, payload);
                });
            });
        }

        /// <summary>
        ///Topics for client subscriptions
        /// </summary>
        /// <param name="eventArgs"></param>
        private void ClientSubscribedTopicHandler(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            Debug. Log ($"service: topic subscribed ClientID: {EventArgs. ClientID} topic: {EventArgs. Topicfilter}");
        }

        /// <summary>
        ///Topics unsubscribed by the client
        /// </summary>
        /// <param name="eventArgs"></param>
        private void ClientUnsubscribedTopicHandler(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            Debug. Log ($"service: topic subscribed ClientID: {EventArgs. ClientID} topic: {EventArgs. Topicfilter}");
        }

        /// <summary>
        ///Push all clients whose topic is clinetid
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            for (int i = 0; i < m_MqttClientObject.Count; i++)
            {
                m_MqttServer.PublishAsync(new MqttApplicationMessage
                {
                    Topic = m_MqttClientObject[i].ClientID,
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce,
                    Retain = false,
                    Payload = Encoding.UTF8.GetBytes(message),
                }, CancellationToken.None);
            }
        }

        /// <summary>
        ///Push a client that listens to a topic
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public void SendMessage(string topic, string message)
        {
            m_MqttServer.PublishAsync(new MqttApplicationMessage
            {
                Topic = topic,
                QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce,
                Retain = false,
                Payload = Encoding.UTF8.GetBytes(message),
            }, CancellationToken.None);
        }
    }

}
