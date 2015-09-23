// arguments:
// byte 1 - segment (0 - SegmentsCount-1)
// byte 2 - value (0 - 180)

#include <Servo.h>

byte RecieveSerialByte();

class ArmSegment
{
  public:
    ArmSegment(int pin, int minPos, int maxPos, int stdPos)
    {
      _servo.attach(pin);

      _minPos = minPos;
      _maxPos = maxPos;
      _stdPos = stdPos;

      SetStdPos();
    }

    bool SetPos(int value)
    {
      if (value < 0 || value > 180)
      {
        return false;
      }

      int pos = map(value, 0, 180, _minPos, _maxPos);
      _servo.write(pos);

      return true;
    }

    void SetStdPos()
    {
      _servo.write(_stdPos);
    }

  private:
    int _minPos;
    int _maxPos;
    int _stdPos;
    Servo _servo;
};

const int SegmentsCount = 3;
ArmSegment* Segments[SegmentsCount];

void setup(void)
{
  pinMode(13, OUTPUT);
  digitalWrite(13, HIGH);

  Segments[0] = new ArmSegment(3, 0, 180, 90);
  Segments[1] = new ArmSegment(4, 0, 180, 90);
  Segments[2] = new ArmSegment(5, 0, 180, 90);

  Serial.begin(9600);
}

void loop(void)
{
  if (Serial.available() > 0)
  {
    byte segment = Serial.read();
    if (segment < SegmentsCount)
    {
      byte value = RecieveSerialByte();
      Segments[segment]->SetPos(value);
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

