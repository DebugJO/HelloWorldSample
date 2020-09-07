import paho.mqtt.client as mqtt

mqtt = mqtt.Client("sss")
mqtt.username_pw_set("username", "password")
mqtt.connect("xxx.xxx.xxx.xxx", 1883)

mqtt.publish("mainTopic/subTopic", "Payload메시지")
print("Published!")

mqtt.loop()
print("Exit")
