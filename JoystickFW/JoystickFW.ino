const int XPin = A0;
const int YPin = A1;
const int ButtonPin = A2;

const int NeutralMinValue = 100;
const int NeutralMaxValue = 900;

const byte SpeedRates[] = {100, 170, 255};
const int SpeedLeds[] = {5, 2, 10};
const int MaxSpeed = 3;

void Drive(int x, int y);
bool IsButtonPressed();
void ChangeSpeed();

// Return value:
// 0 - equal
// 1 - lower
// 2 - greater
int NeutralCompare(int value);

int CurrentSpeed = 1;

void setup()
{
  pinMode(XPin, INPUT);
  pinMode(YPin, INPUT);
  pinMode(ButtonPin, INPUT_PULLUP);

  for (int i = 0; i < MaxSpeed; ++i)
  {
    pinMode(SpeedLeds[i], OUTPUT);
  }

  digitalWrite(SpeedLeds[0], HIGH);

  Serial.begin(9600);
}

void loop()
{
  if (IsButtonPressed())
  {
    ChangeSpeed();
  }

  int x = analogRead(XPin);
  int y = analogRead(YPin);
  Drive(x, y);

  delay(10);
}

void Drive(int x, int y)
{
  byte data[3] = { 0 };

  int xState = NeutralCompare(x);
  int yState = NeutralCompare(y);

  int value = xState + yState * 10;

  switch (value)
  {
    case 0:
      {
        break;
      }

    case 1:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000010;
        break;
      }

    case 2:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000001;
        break;
      }

    case 10:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000011;
        break;
      }

    case 20:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000000;
        break;
      }

    case 11:
      {
        data[0] = SpeedRates[CurrentSpeed - 1] / 2;
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000011;
        break;
      }

    case 12:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1] / 2;
        data[2] = 0b00000011;
        break;
      }

    case 21:
      {
        data[0] = SpeedRates[CurrentSpeed - 1] / 2;
        data[1] = SpeedRates[CurrentSpeed - 1];
        data[2] = 0b00000000;
        break;
      }

    case 22:
      {
        data[0] = SpeedRates[CurrentSpeed - 1];
        data[1] = SpeedRates[CurrentSpeed - 1] / 2;
        data[2] = 0b00000000;
        break;
      }
  }

  Serial.write(data, 3);
}

int NeutralCompare(int value)
{
  if (value < NeutralMinValue)
  {
    return 1;
  }

  if (value > NeutralMaxValue)
  {
    return 2;
  }

  return 0;
}

bool IsButtonPressed()
{
  if (!digitalRead(ButtonPin))
  {
    delay(10);
    if (digitalRead(ButtonPin))
    {
      return false;
    }

    while (!digitalRead(ButtonPin))
    {
    }

    return true;
  }

  return false;
}

void ChangeSpeed()
{
  ++CurrentSpeed;
  if (CurrentSpeed > MaxSpeed)
  {
    CurrentSpeed = 1;
  }

  for (int i = 0; i < MaxSpeed; ++i)
  {
    if (i < CurrentSpeed)
    {
      digitalWrite(SpeedLeds[i], HIGH);
    }
    else
    {
      digitalWrite(SpeedLeds[i], LOW);
    }
  }
}
