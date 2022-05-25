// https://developpaper.com/%E2%98%85-unit-actual-combat-advanced-c-mqtt-message-queue-telemetry-transmission-protocol-detailed-explanation-8/

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace MQTT
{
    public class MqttClient
    {
        private IMqttClient m_MqttClient;
        private string m_ClientID;
        public Action<string, string> MessageCallback = null;
        public Action<MqttClient> ConnectCallback = null;
        public Action<MqttClient> DisconnectCallback = null;

        /// <summary>
        ///Message pushed down
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        public bool IsConnect
        {
            get
            {
                if (m_MqttClient == null) return false;
                return m_MqttClient.IsConnected;
            }
        }

        public string ClientId
        {
            get
            {
                return m_ClientID;
            }
        }

        public MqttClient(string clientID, string ip = "127.0.0.1", int port = 3883)
        {
            m_ClientID = clientID;
            var options = new MqttClientOptionsBuilder()
            .WithClientId(m_ClientID)
            .WithTcpServer(ip, port)
            .WithCredentials("admin", "123456")
            .Build();

            m_MqttClient = new MqttFactory().CreateMqttClient();
            m_MqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(MqttClient_Connected);
            m_MqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(MqttClient_Disconnected);
            //m_MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MqttClient_Recevied);
            m_MqttClient.UseApplicationMessageReceivedHandler(MqttClient_Recevied);
            m_MqttClient.ConnectAsync(options);
        }

        public void Disconnect()
        {
            if (m_MqttClient != null)
                m_MqttClient.DisconnectAsync();
        }

        private void MqttClient_Connected(MqttClientConnectedEventArgs eventArgs)
        {
            Debug.Log($"mqtt client '{m_ClientID}' started!");
            //The associated server subscription is used to accept the push information from the server
            if (m_MqttClient != null)
                m_MqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(m_ClientID).Build());

            Loom.RunAsync(() => { Loom.QueueOnMainThread(() => { ConnectCallback?.Invoke(this); }); });
        }

        private void MqttClient_Disconnected(MqttClientDisconnectedEventArgs eventArgs)
        {
            Debug.Log($"mqtt client '{m_ClientID}' stopped!");
            Loom.RunAsync(() => { Loom.QueueOnMainThread(() => { DisconnectCallback?.Invoke(this); }); });
        }

        private void MqttClient_Recevied(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            if (eventArgs.ApplicationMessage.Topic.Equals("Client")) return;

            Message = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            Debug.Log($"mqtt client '{eventArgs.ClientId}'  Topic:{eventArgs.ApplicationMessage.Topic}    Recevied: '{Message}'");
            Loom.RunAsync(() => { Loom.QueueOnMainThread(() => { MessageCallback?.Invoke(m_ClientID, Message); }); });
        }

        /// <summary>
        ///Client push information 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (m_MqttClient == null) return;

            m_MqttClient.PublishAsync(new MqttApplicationMessage
            {
                Topic = "Client",
                QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce,
                Retain = false,
                Payload = Encoding.UTF8.GetBytes(message),
            }, CancellationToken.None);

        }
    }
}
