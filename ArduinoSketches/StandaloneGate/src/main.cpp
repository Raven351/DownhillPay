#include <Arduino.h>
#include <CytronWiFiShield.h>
#include <CytronWiFiClient.h>
#include <ArduinoJson.h>
#include <ArduinoHttpClient.h>
#include <SoftwareSerial.h>

const char* ssid = "";
const char* pass = "";

const char* host = "192.168.1.13";
const int httpPort = 3000;
// const char* host = "api.github.com";
// const int httpPort = 443;

void setUpWiFiShield(int rx, int tx)
{
  if (!wifi.begin(rx,tx)) Serial.println(F("Error talking to shield"));
  Serial.println(F("Serial wifi connection"));
}
void connectToWifi(char* ssid, char* pass){
  if(!wifi.connectAP(ssid, pass)) Serial.println(F("Error connecting to WiFi"));
  Serial.print(F("Connected to: ")); Serial.println(wifi.SSID());
  Serial.print(F("IP address: ")); Serial.println(wifi.localIP());
}
ESP8266Client connectToHost(char* host, int port){
  ESP8266Client client;
  Serial.print("Connecting to ");
  Serial.println(host);
  if (!client.connect(host, httpPort))
  {
    Serial.println(F("Failed to connect to server"));
    client.stop();
    return client;
  }
  Serial.println("Connection successful");
  return client;
}

String getJsonFromResponse(String response){
  int locStart = response.indexOf("{");
  if (locStart ==-1) return "";
  int locFinish = response.indexOf("}", locStart);
  if (locFinish == -1) return "";
  return response.substring(locStart,locFinish);
}

String getRequest(ESP8266Client client, String url){
  Serial.print("Requesting URL: "); Serial.println(url);
  String request = String("GET ") + url + " HTTP/1.1\r\n" + 
    "User-Agent: BuildFailureDetectorESP8266\r\n" +
    "Accept: application/vnd.pgrst.object+json\r\n" + 
    "Cache-Control: no-cache\r\n" +
    "Host: " + host + "\r\n" + 
    //"Accept-Encoding: gzip, deflate\r\n" +
    "Connection: close\r\n\r\n";
  Serial.print(request); Serial.println();
  client.print(request);
  Serial.println("Request sent");
  int i = 5000; //timeout = 5s
  while (client.available() <=0&&i--)
  {
    delay(1);
    if (i==1)
    {
      Serial.println(F("Timeout"));
      client.stop();
      return "";
    }
  }

  if (client.available())
  {
    if (client.find("\r\n\r\n")) Serial.println(F("Headers received"));

    String response = client.readStringUntil('}');
    
    Serial.println(F("Reply was:"));
    Serial.println(F("========"));
    Serial.println(response);
    while (client.available()>0) client.flush();
    DynamicJsonDocument doc(1024);
    String stringJson = getJsonFromResponse(response);
    deserializeJson(doc, stringJson);
    return doc["uuid"];
  }
}

String requestCard(ESP8266Client client, String uuid){
  String url = "/rfid_card?uid=eq." + uuid;
   return getRequest(client, url);
}

void setup() {
  Serial.begin(9600);
  setUpWiFiShield(2,3);
  connectToWifi(ssid, pass);
  ESP8266Client client = connectToHost(host, httpPort);
  Serial.println(requestCard(client, "8F%200A%2011%2029"));
  //String url = "/rfid_card?uid=eq.8F%200A%2011%2029";


}

void loop() {
  // put your main code here, to run repeatedly:
}

