#include <Arduino.h>
#include <CytronWiFiShield.h>
#include <CytronWiFiClient.h>
#include <SoftwareSerial.h>

const char *ssid = "";
const char *pass = "";

const char* host = "192.168.1.13";
const int httpPort = 3000;
// const char* host = "api.github.com";
// const int httpPort = 443;

void setup() {
  Serial.begin(9600);
  if (!wifi.begin(2,3)) Serial.println(F("Error talking to shield"));
  Serial.println(F("Serial wifi connection"));
  if(!wifi.connectAP(ssid, pass)) Serial.println(F("Error connecting to WiFi"));
  Serial.print(F("Connected to: ")); Serial.println(wifi.SSID());
  Serial.print(F("IP address: ")); Serial.println(wifi.localIP());

  ESP8266Client client;
  Serial.print("Connecting to ");
  Serial.println(host);
  if (!client.connect(host, httpPort))
  {
    Serial.println(F("Failed to connect to server"));
    client.stop();
    return;
  }

  Serial.println("Connection successful");

  String url = "/rfid_card?uid=eq.8F%200A%2011%2029";
  Serial.print("Requesting URL: "); Serial.println(url);


  String request = String("GET ") + url + " HTTP/1.1\r\n" + 
    "User-Agent: BuildFailureDetectorESP8266\r\n" +
    "Accept: */*\r\n" + 
    "Cache-Control: no-cache\r\n" +
    "Host: " + host + "\r\n" + 
    //"Accept-Encoding: gzip, deflate\r\n" +
    "Connection: close\r\n\r\n";
  Serial.print(request); Serial.println();
  client.print(request);
  Serial.println("request sent");
  int i = 5000; //timeout = 5s
  while (client.available() <=0&&i--)
  {
    delay(1);
    if (i==1)
    {
      Serial.println(F("Timeout"));
      client.stop();
      return;
    }
  }

  if (client.available())
  {
    if (client.find("\r\n\r\n")) Serial.println(F("Headers received"));

    String line = client.readString();
    if (line.startsWith("{\"state\":\"success\"")) Serial.println(F("Request successful!"));
    else Serial.println(F("Request failed!"));
    
    Serial.println(F("Reply was:"));
    Serial.println(F("========"));
    Serial.println(line);
  }

}

void loop() {
  // put your main code here, to run repeatedly:
}