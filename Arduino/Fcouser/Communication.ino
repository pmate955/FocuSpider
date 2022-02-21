const int MAX_MESSAGE_LENGTH = 8;
const byte STX = 0x00;
const byte ETX = 0x01;
static char message[MAX_MESSAGE_LENGTH];
static bool isForward;
static int message_pos = 0;

//STX SIGN N1 N2 ... ETX
void readData() {
  while (Serial.available() > 0) {
    byte inByte = Serial.read();

    if (inByte == STX) {
      message_pos = -1;
    } else if (inByte != ETX && (message_pos < MAX_MESSAGE_LENGTH - 1) ) {
      if (message_pos == -1) {
        if (inByte == '+') {
          isForward = true;
          message_pos = 0;
        } else if (inByte == '-') {
          isForward = false;
          message_pos = 0;
        } else {
          Serial.println((String)"E1");
          message_pos = -1;
          break;
        }
      } else {
        message[message_pos++] = inByte;
      }
    } else if (inByte == ETX) {
      message[message_pos] = '\0';
      int steps = atoi(message);
      doSteps(isForward, steps);
      Serial.println((String)"OK " + steps);
    } else {
      Serial.println((String)"E3 " + message_pos);
    }
  }
}
