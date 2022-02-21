void doSteps(bool isForward, int steps) {
  int state = 0;
  for (int i = 0; i < steps; i++) {
    oneStep(state);
    state = getNextState(isForward, state);
    delay(5);
  }

  oneStep(8);
}

int getNextState(bool isForward, int state) {
  if (isForward) {

    if (state == 8) {
      state = -1;
    }
    state++;

    if (state > 7) {
      state = 0;
    }
  } else {
    state--;
    if (state < 0) {
      state = 7;
    }
  }
  return state;
}

void oneStep(int state) {
  switch (state) {
    case 0:
      digitalWrite(M1, LOW);
      digitalWrite(M2, LOW);
      digitalWrite(M3, LOW);
      digitalWrite(M4, HIGH);
      digitalWrite(LED, HIGH);
      break;
    case 1:
      digitalWrite(M1, LOW);
      digitalWrite(M2, LOW);
      digitalWrite(M3, HIGH);
      digitalWrite(M4, HIGH);
      break;
    case 2:
      digitalWrite(M1, LOW);
      digitalWrite(M2, LOW);
      digitalWrite(M3, HIGH);
      digitalWrite(M4, LOW);

      break;
    case 3:
      digitalWrite(M1, LOW);
      digitalWrite(M2, HIGH);
      digitalWrite(M3, HIGH);
      digitalWrite(M4, LOW);
      break;
    case 4:
      digitalWrite(M1, LOW);
      digitalWrite(M2, HIGH);
      digitalWrite(M3, LOW);
      digitalWrite(M4, LOW);
      digitalWrite(LED, LOW);
      break;
    case 5:
      digitalWrite(M1, HIGH);
      digitalWrite(M2, HIGH);
      digitalWrite(M3, LOW);
      digitalWrite(M4, LOW);
      break;
    case 6:
      digitalWrite(M1, HIGH);
      digitalWrite(M2, LOW);
      digitalWrite(M3, LOW);
      digitalWrite(M4, LOW);
      break;
    case 7:
      digitalWrite(M1, HIGH);
      digitalWrite(M2, LOW);
      digitalWrite(M3, LOW);
      digitalWrite(M4, HIGH);
      break;
    default:
      digitalWrite(M1, LOW);
      digitalWrite(M2, LOW);
      digitalWrite(M3, LOW);
      digitalWrite(M4, LOW);
      digitalWrite(LED, LOW);
      break;
  }
}
