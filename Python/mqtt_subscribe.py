import paho.mqtt.client as mqtt

def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))
    client.subscribe("mainTopic/#")

def on_message(client, userdata, msg):
    print(msg.topic + " " + msg.payload.decode())

client = mqtt.Client()
client.username_pw_set("username", "password")
client.on_connect = on_connect
client.on_message = on_message
client.connect("xxx.xxx.xxx.xxx", 1883, 60)
client.loop_forever()
