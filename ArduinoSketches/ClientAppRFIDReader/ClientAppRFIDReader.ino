#include <deprecated.h>
#include <require_cpp11.h>
#include <MFRC522.h>
#include <MFRC522Extended.h>
#include <SPI.h>
#include <SoftwareSerial.h>

#define BUZZ 8 //Buzzer PIN Definition
#define SS_PIN 10 //MRFRC522 PIN Definition
#define RST_PIN 9 //MRFRC522 PIN Definition

MFRC522 rfid(SS_PIN, RST_PIN); //Instance of MFRC522 class
MFRC522::MIFARE_Key key;

const byte inputSize = 32;
char receivedCommand[inputSize]; //stores received command
char tempCommand[inputSize]; //temporary for parsing 
char * strtokIndex;
byte cardUID[4]; //stores UID of cards after it's read

int commandNumber = 0;
boolean newCommand = false; 

void setup(){
  Serial.begin(9600);
  SPI.begin(); // Init SPI bus
  rfid.PCD_Init(); // Init MRFC522
}

void loop(){
    receiveCommand();
    receiveCommandNumber();
//  if (newCommand == true){
//    strcpy(tempCommand, receivedCommand);
//    receiveCommandNumber();
//    newCommand = false;
//  }
  switch(commandNumber)
  {
    case 1:
    {
      bool isCardReaded = readUID();;
      if (isCardReaded == true){
        printHexUID(cardUID, sizeof(cardUID));
      }
      else Serial.println("<0>");
      commandNumber = 0;
      break;
    }
    
    case 2: //example how to get parameters from array
    {
      strcpy(tempCommand, receivedCommand);
      strtokIndex = strtok(tempCommand, ","); //command number - skip
      strtokIndex = strtok(NULL, ","); //first parameter
      int a = atoi(strtokIndex); //convert first parameter to int
      strtokIndex = strtok(NULL, ",");
      int b = atoi(strtokIndex);
      Serial.print("Wynik: ");
      Serial.print(AddUp(a,b));
      Serial.println("");
      commandNumber = 0;
      break; 
    }
    
    case 3:
    {
      Serial.print("3rd Case");
      break;
    }
      
    default:
      break;
  }
  delay(1000);
}

void receiveCommand(){
  static boolean receiveInProgress = false;
  static byte index = 0;
  char startMarker = '<';
  char endMarker = '>';
  char recievedChar;
  while (Serial.available() > 0 && newCommand == false){
    recievedChar = Serial.read(); //read character from Serial and store it in variable
    if (receiveInProgress == true){
      if (recievedChar != endMarker) {
        receivedCommand[index] = recievedChar;
        index++;
        if (index >= inputSize){
          index = inputSize - 1;
        }
      }
      else{
        receivedCommand[index] = '\0';
        receiveInProgress = false;
        index = 0;
        newCommand = true;
      }
    }
    else if (recievedChar == startMarker){
      receiveInProgress = true;
    } 
  }
}

void receiveCommandNumber(){
    if (newCommand == true){
    strcpy(tempCommand, receivedCommand);
    char * strtokIndex;
    strtokIndex = strtok(tempCommand, ",");
    commandNumber = atoi(strtokIndex);
    newCommand = false;
  }
}

bool readUID(){
  unsigned long operationStartTime = millis(); //sets timer
  while(!rfid.PICC_IsNewCardPresent()){
    unsigned long currentTime = millis();
    receiveCommand();
    receiveCommandNumber();
    if(currentTime - operationStartTime > 30000 || commandNumber == -1) return false;
  }
  while(!rfid.PICC_ReadCardSerial());
  tone(BUZZ, 2300, 500);
  MFRC522::PICC_Type piccType = rfid.PICC_GetType(rfid.uid.sak);
  //Serial.println(rfid.PICC_GetTypeName(piccType));
  if(piccType != MFRC522::PICC_TYPE_MIFARE_MINI &&  piccType != MFRC522::PICC_TYPE_MIFARE_1K && piccType != MFRC522::PICC_TYPE_MIFARE_4K && piccType != MFRC522::PICC_TYPE_MIFARE_UL) {
    rfid.PICC_HaltA(); //halt PICC
    rfid.PCD_StopCrypto1(); //stop encryption on PCD
    return false;
  }
  else{
    for(byte i=0;i<4;i++){
      cardUID[i] = rfid.uid.uidByte[i];
    }
    rfid.PICC_HaltA(); //halt PICC
    rfid.PCD_StopCrypto1(); //stop encryption on PCD
    return true;
  }
}

void printHexUID(byte *buffer, byte bufferSize) { //prints UID to serial in hexadecimal format || *buffer = arrray of UID values in byte, bufferSize cardUID.size
  Serial.print("<");
  for (byte i = 0; i < bufferSize; i++) {

    Serial.print(buffer[i] < 0x10 ? "0" : "");
    Serial.print(buffer[i], HEX);
    Serial.print(i == bufferSize-1 ? "" : " ");
  }
  Serial.print(">");  
  Serial.println("");
}

void printDecUID(byte *buffer, byte bufferSize) { //prints UID to serial in decimal format || *buffer = arrray of UID values in byte, bufferSize cardUID.size
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? " 0" : " ");
    Serial.print(buffer[i], DEC);
  }
}

int AddUp(int a, int b){
  return a+b;
}
