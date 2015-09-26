// Drive Command - 1
// arguments:
// byte 1 - M1 speed (0 - 255)
// byte 2 - M2 speed (0 - 255)
// byte 3, bit 1 - M1 direction (0 - 1)
// byte 3, bit 0 - M2 direction (0 - 1)

#include "DualVNH5019MotorShield.h"

byte RecieveSerialByte();
int GetMotorValue(byte speed, bool direction);

DualVNH5019MotorShield MotorsDriver;

void setup()
{
  pinMode(13, OUTPUT);
  digitalWrite(13, HIGH);

  MotorsDriver.init();
  Serial.begin(9600);
}

void loop()
{
  if (Serial.available() > 0)
  {
    byte command = RecieveSerialByte();
    if (command == 1)
    {
      byte m1Speed = RecieveSerialByte();
      byte m2Speed = RecieveSerialByte();
      byte dirction = RecieveSerialByte();

      bool m1Direction = dirction & 0b00000010;
      bool m2Direction = dirction & 0b00000001;

      int m1Value = GetMotorValue(m1Speed, m1Direction);
      int m2Value = GetMotorValue(m2Speed, m2Direction);

      MotorsDriver.setM1Speed(m1Value);
      MotorsDriver.setM2Speed(m2Value);
    }

    if (command == 2)
    {
      byte arg1 = RecieveSerialByte();
      byte arg2 = RecieveSerialByte();
      byte data[3] = { 2, arg1, arg2 };
      Serial.write(data, 3);
    }

    delay(10);
  }
}

byte RecieveSerialByte()
{
  while (1)
  {
    if (Serial.available() > 0)
    {
      return Serial.read();
    }
  }
}

int GetMotorValue(byte speed, bool direction)
{
  int value = speed;
  if (direction)
  {
    value = -value;
  }

  return map(value, -255, 255, -400, 400);
}

