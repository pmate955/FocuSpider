
const int LED = 13;
const int M1 = 4;
const int M2 = 5;
const int M3 = 6;
const int M4 = 7;

void setup() {
  pinMode(LED, OUTPUT);
  pinMode(M1, OUTPUT);
  pinMode(M2, OUTPUT);
  pinMode(M3, OUTPUT);
  pinMode(M4, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  readData();
}
