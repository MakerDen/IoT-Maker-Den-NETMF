*Documentation in progress*

# Getting Started

The getting started lab code and the complete [Maker Den Lab Guide ](https://github.com/MakerDen/IoT-Maker-Den-NETMF/blob/master/MakerDen/Lab%20Code/IoT%20Maker%20Den%20v2.0.pdf) (click Raw to open in Adobe Reader)
can be found in the Lab Code folder in the Maker Den Project.

# First time deploying code to your Netduino
After either downloading or cloning the source code you will need to reset the .NET Micro Framework Deployment Transport.  

1. Ensure your [Netduino Plus 2](http://www.netduino.com/netduinoplus2/specs.htm) is connected to your PC via the USB cable.  
2. Right mouse click the MakerDen project, and select Properties.  
3. From the properties page select the .NET Micro Framework tab
4. Select Emulator from the Transport dropdown then reselect USB and your Netduino.  

The project should now deploy correctly to your Netduino.



##Software Requirements (as at March 2015)

1. Windows 7 or above
2. [Visual Studio 2013](https://www.visualstudio.com/en-au/products/visual-studio-community-vs) (Community Edition (free), Professional or above)
3. [.NET Micro Framework SDK v4.3 (QFE2-RTM)](http://netmf.codeplex.com/releases/view/611040)
	1. Download the “FULL ZIP File of all SDK Files”
	2. From the unzipped SDK download install
		1. MicroFrameworkSDK
		2. netmfvs2013 (Visual Studio 2013 add-on)
4. [Netduino SDK v4.3.1](http://www.netduino.com/downloads/netduinosdk_NETMF43.exe)

## Updating the Netduino Firmware to 4.3.1
The first time you use the Netduino you will almost certainly need to update the device firmware to the latest version to match the .NET Micro Framework you installed on your PC.

1. Watch the ["How to Update Firmware and set the MAC Address"](https://t.co/m45wF2RMXY) video guide
2. [Download](http://forums.netduino.com/index.php?/topic/10479-netduino-plus-2-firmware-v431/) the Netduino Plus 2 Firmware (ignore the prerequists listed on that page as they are out of date)

## Initial Hardware Setup
Be sure to read the Maker Den Lab Guide appendix section to understand:-

1. Trouble Shooting
2. Initial Hardware Setup
3. Lab Parts


# What is the Internet of Things Solution Accelerator?

The Internet of Things Solution Accelerator for the .NET Micro Framework provides a pluggable foundation to support sensors, actuators, data serialisation, communications, and command and control. 


![Alt text](https://github.com/MakerDen/IoT-Maker-Den-NETMF/blob/master/MakerDen/Lab%20Code/Maker%20Den%20IoT%20Framework.jpg)



## Extensible/pluggable framework supporting

1. Sensors
 * Physical: Light, Sound, Temperature (onewire), Servo
 * Virtual: Memory Usage, Diagnostics
 * Sensor data serialised to a JSON schema

2. Actuators
 * RGB, RGB PWM, Piezo, Relay
 * NeoPixel - Supports Strips, Rings, and Grids
    - Low and high level pixel frame transformation primitives 
    - Extensive colour library support along with some predefined palettes
    - NeoPixel NeoMatrix Grid library adds alphanumeric character drawing and scrolling capability 

3. Command and Control
 * Control relays, start NeoPixels etc via the communications layer

4. Communications
 * Pluggable – currently implemented on MQTT ([Mosquitto](http://mosquitto.org) MQTT Server running on Azure)

5. Supported and Tested
 * Netduino 2 Plus and Gadgeteer
 * Supports Visual Studio 2012 and 2013, 2013 preferred.


## IoT Dashboard
The IoT Dashboard allows you to visualise the data streamed to Azure. 

![IoT Dashboard](https://github.com/MakerDen/IoT-Maker-Den-NETMF/blob/master/MakerDen/Lab%20Code/IoTDashboard.JPG)

You can install the IoT Dashboard from [here](http://iotmakerdendashboard.azurewebsites.net/install/publish.htm).  Note, you will need to allow to run from unknown publisher.

## Programming Models

### Declarative Event Driven Model

    using Glovebox.MicroFramework.Sensors;
    using Glovebox.Netduino.Actuators;
    using Glovebox.Netduino.Sensors;
    using Microsoft.SPOT;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;
    
    namespace MakerDen {
        public class Program : MakerBaseIoT  {
            public static void Main() {
                // main code marker
                
                //Replace the "emul" which is the name of the device with a unique 3 to 5 character name
                //use your initials or something similar.  This code will be visible on the IoT Dashboard
                StartNetworkServices("emul", true);
                
                
                // sensor timer value 10000 measure every 10000 milliseconds (10 seconds)
                using (SensorTemp temp = new SensorTemp(Pins.GPIO_PIN_D8, 10000, "temp01"))
                using (SensorLight light = new SensorLight(AnalogChannels.ANALOG_PIN_A0, 1000, "light01"))
                using (rgb = new RgbLed(Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D6, "rgb01")) {
                
                    temp.OnBeforeMeasurement += OnBeforeMeasure;
                    temp.OnAfterMeasurement += OnMeasureCompleted;
                    light.OnBeforeMeasurement += OnBeforeMeasure;
                    light.OnAfterMeasurement += OnMeasureCompleted;
                    
                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }
    }

### Imperative Model

    using Glovebox.MicroFramework.Sensors;
    using Glovebox.Netduino.Actuators;
    using Glovebox.Netduino.Sensors;
    using Microsoft.SPOT;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;
    
    namespace MakerDen {
        public class Program : MakerBaseIoT  {
            public static void Main() {
                // main code marker
    
                // sensor timer value -1 disables auto measure
                using (SensorLight light = new SensorLight(AnalogChannels.ANALOG_PIN_A0, -1, "light01"))
                using (rgb = new RgbLed(Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D6, "rgb01")) {
    
                    while (true) {
                        if (light.Current < 60) {
                            rgb.On(RgbLed.Led.Red);
                            rgb.Off(RgbLed.Led.Green);
                        }
                        else {
                            rgb.Off(RgbLed.Led.Red);
                            rgb.On(RgbLed.Led.Green);
                        }
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }

# Creating Sensors and Actuators

Be sure to check out the sensors and actuators that have been implemented in the Glovebox.Netduino project.  See the Sensors and Actuators folders, all the code is provided to help you understand how things work.

## Creating a Sensor


### Inheriting from SensorBase

Your sensor class must inherit from SensorBase.  


    using Glovebox.MicroFramework.Base;

    namespace Glovebox.Netduino.Sensors {
        public class SensorLight : SensorBase {
        }
    }


### Implement the Abstract Class

Next right mouse click on SensorBase to Implement the Abstract Class.


    using Glovebox.MicroFramework.Base;

    namespace Glovebox.Netduino.Sensors {
        public class SensorLight : SensorBase {

            protected override void Measure(double[] value) {
                throw new NotImplementedException();
            }

            protected override string GeoLocation() {
                throw new NotImplementedException();
            }

            public override double Current {
                get { throw new NotImplementedException(); }
            }

            protected override void SensorCleanup() {
                throw new NotImplementedException();
            }
        }
    }

### Initialise the Sensor Base Constructor

Add a Sensor Constructor and initialise the SensorBase Base constructor.  

The SensorBase base constructor requires

1. **Sensor Type** - arbitrary/sensible type for the sensor.  The value is published alongside the sensor reading to provide some type information.
2. **Sensor Unit** - arbitrary/sensible measurement unit for the sensor.  Example p for percentage, n for numeric etc.  The unit is published alongside the sensor reading to provide some unit information.
3. **Values Per Sample** - Defines how many values will be collected per reading.  Most sensors generate only one value per sample.  But there are sensors that generate more data.  For example a sensor that sensors both temperature and humidity.
4. **Sample Rate in Milliseconds** - how often to take a sensor reading.
5. **Name** - This is a unique name that you can use to identify a sensor from the command and control service.


        public SensorLight(Cpu.AnalogChannel pin, int SampleRateMilliseconds, string name)
            : base("light", "p", ValuesPerSample.One, SampleRateMilliseconds, name) {

            ldrAnalogPin = new AnalogInput(pin, -1);

            // Call StartMeasuring() after sensor initialisation
            StartMeasuring();
        }


### Implement the sensor logic

Implement the abstract methods and properties for the Sensor class.


    using Glovebox.MicroFramework.Base;
    using Microsoft.SPOT.Hardware;
    using System;

    namespace Glovebox.Netduino.Sensors {
        public class SensorLdr : SensorBase {

            private AnalogInput ldrAnalogPin;

            /// <summary>
            /// Light Dependent Resistor Sensor Class
            /// </summary>
            /// <param name="pin">Analog Pin</param>
            /// <param name="SampleRateMilliseconds">How often to sample the sensor on milliseconds</param>
            /// <param name="name">Sensor target name for command and control</param>
            public SensorLdr(Cpu.AnalogChannel pin, int SampleRateMilliseconds, string name)
                : base("light", "p", ValuesPerSample.One, SampleRateMilliseconds, name) {

                ldrAnalogPin = new AnalogInput(pin, -1);

                // Call StartMeasuring() after sensor initialisation
                StartMeasuring();
            }


            protected override void Measure(double[] value) {
                value[0] = (int)(ldrAnalogPin.Read() * 100);
            }

            protected override string GeoLocation() {
                return string.Empty;
            }

            public override double Current {
                get { return (int)(ldrAnalogPin.Read() * 100); }
            }

            protected override void SensorCleanup() {
                ldrAnalogPin.Dispose();
            }
        }
    }


### Using your newly created sensor

When you reference a pin on the Netduino in your code it is important to use the **SecretLabs.NETMF.Hardware.NetduinoPlus** namespace not the Microsoft.SPOT.Hardware.Cpu.Pin which (unfortunately) is the Visual Studio default.

	// program.cs
    using Glovebox.Netduino.Sensors;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;

    namespace MakerDen {
        public class Program : MakerBaseIoT {

            public static void Main() {

                using (SensorLdr ldr = new SensorLdr(AnalogChannels.ANALOG_PIN_A0, 1000, "ldr01")) {

                    // the event handlers are implemented in the MakerBaseIoT subclass
                    ldr.OnBeforeMeasurement += OnBeforeMeasure;
                    ldr.OnAfterMeasurement += OnMeasureCompleted;

                    // Thread sleep the main thread forever.  
                    // Your newly created sensor runs on its own thread and in this case wakes up 1000 milliseconds
                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }
    }

Or

	// program.cs
    using Glovebox.Netduino.Sensors;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;

    namespace MakerDen {
        public class Program : MakerBaseIoT {

            public static void Main() {

                using (SensorLdr ldr = new SensorLdr(AnalogChannels.ANALOG_PIN_A0, -1, "ldr01")) {

                    while (true) {
                        if (ldr.Current < 60) {
                            // do something...
                        }
                        else {
                            // do something...
                        }
                        // good practice not to put your netduino in to a hard loop, so add a thread sleep
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }

## Creating an Actuator

The following example creates a relay switch with on/off command and control support.

Your actuator class must inherit from ActuatorBase.

    using Glovebox.MicroFramework.Base;

    namespace Glovebox.Netduino.Actuators {
        public class Relay : ActuatorBase {
        }
    }

### Implement the Abstract Class

Next right mouse click on ActuatorBase to Implement the Abstract Class.

    using Glovebox.MicroFramework.Base;

    namespace Glovebox.Netduino.Actuators {
        public class Relay : ActuatorBase {

            protected override void ActuatorCleanup() {
                throw new System.NotImplementedException();
            }

            public override void Action(MicroFramework.IoT.IotAction action) {
                throw new System.NotImplementedException();
            }
        }
    }


### Implement the class constructor

The ActuatorBase base constructor requires

1. **Name** - This is a unique name that you can use to identify a actuator from the command and control service.
2. **Actuator Type** - arbitrary/sensible type for the actuator.  For now not used.


        public Relay(Cpu.Pin pin, string name)
            : base(name, "relay") {
            relay = new OutputPort(pin, false);
        }

### Implement the Actuator Cleanup

        protected override void ActuatorCleanup() {
            relay.Dispose();
        }

### Implement Command and Control

See the Lab Guide Appendix for information on sending a command via MQTT.

        public override void Action(Glovebox.MicroFramework.IoT.IotAction action) {
            switch (action.cmd) {
                case "on":
                    relay.Write(true);
                    break;
                case "off":
                    relay.Write(false);
                    break;
            }
        }

### The Completed Relay Class

    using Glovebox.MicroFramework.Base;
    using Microsoft.SPOT.Hardware;

    namespace Glovebox.Netduino.Actuators {
        public class Relay : ActuatorBase {

            private OutputPort relay;

	        /// <summary>
	        /// Create a relay control
	        /// </summary>
	        /// <param name="pin">From the SecretLabs.NETMF.Hardware.NetduinoPlus.Pins namespace</param>
	        /// <param name="name">Unique identifying name for command and control</param>
            public Relay(Cpu.Pin pin, string name)
                : base(name, "relay") {
                relay = new OutputPort(pin, false);
            }

            protected override void ActuatorCleanup() {
                relay.Dispose();
            }

            public override void Action(Glovebox.MicroFramework.IoT.IotAction action) {
                switch (action.cmd) {
                    case "on":
                        relay.Write(true);
                        break;
                    case "off":
                        relay.Write(false);
                        break;
                }
            }
        }
    }


### Coder Friendly Relay Class

This version adds an Actions enumeration to make it more developer friendly.

    using Glovebox.MicroFramework.Base;
    using Microsoft.SPOT.Hardware;

    namespace Glovebox.Netduino.Actuators {
        public class Relay : ActuatorBase {

            public enum Actions { On, Off }

            private OutputPort relay;

            /// <summary>
	        /// Create a relay control
	        /// </summary>
	        /// <param name="pin">From the SecretLabs.NETMF.Hardware.NetduinoPlus.Pins namespace</param>
	        /// <param name="name">Unique identifying name for command and control</param>
			public Relay(Cpu.Pin pin, string name)
                : base(name, "relay") {
                relay = new OutputPort(pin, false);
            }

            protected override void ActuatorCleanup() {
                relay.Dispose();
            }

            public override void Action(Glovebox.MicroFramework.IoT.IotAction action) {
                switch (action.cmd) {
                    case "on":
                        TurnOn();
                        break;
                    case "off":
                        TurnOff();
                        break;
                }
            }

            public void Action(Actions action) {
                switch (action) {
                    case Actions.On:
                        TurnOn();
                        break;
                    case Actions.Off:
                        TurnOff();
                        break;
                }
            }

            public void TurnOn() {
                relay.Write(true);
            }

            public void TurnOff() {
                relay.Write(false);
            }
        }
    }


### Using your newly created actuator

This example uses the Light Dependent Resistor Sensor to determine the light levels.  Depending on the light level, the Relay will be turned on or off.  The relay could be controlling a light.

When you reference a pin on the Netduino in your code it is important to use the **SecretLabs.NETMF.Hardware.NetduinoPlus** namespace not the Microsoft.SPOT.Hardware.Cpu.Pin which (unfortunately) is the Visual Studio default.

	// program.cs    
	using Glovebox.Netduino.Actuators;
    using Glovebox.Netduino.Sensors;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;

    namespace MakerDen {
        public class Program : MakerBaseIoT {

            public static void Main() {

                using (Sensorldr ldr = new Sensorldr(AnalogChannels.ANALOG_PIN_A0, -1, "ldr01")) 
                using (Relay relay = new Relay(Pins.GPIO_PIN_D0, "relay01")) {

                    while (true) {
                        if (ldr.Current < 60) {
                            relay.Action(Relay.Actions.On);
                        }
                        else {
                            relay.Action(Relay.Actions.Off);
                        }
                        // good practice not to put your netduino in to a hard loop, so add a thread sleep
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }

## Bringing it all together

The following example brings it all together and uses all the sensors and actuators defined in the IoT Solution Accelerator.  

	// program.cs
    using Glovebox.MicroFramework.Sensors;
    using Glovebox.Netduino.Actuators;
    using Glovebox.Netduino.Sensors;
    using SecretLabs.NETMF.Hardware.NetduinoPlus;
    using System.Threading;

    namespace MakerDen {
        public class Program : MakerBaseIoT {

            public static void Main() {
                Piezo speaker = new Piezo(PWMChannels.PWM_PIN_D9, "speaker01");
                speaker.BeebStartup();

                // main code marker

                //Replace the "emul" which is the name of the device with a unique 3 to 5 character name
                //use your initials or something similar.  This code will be visible on the IoT Dashboard
                StartNetworkServices("test", true);

                using (SensorTemp temp = new SensorTemp(Pins.GPIO_PIN_D8, 10000, "temp01"))
                using (SensorLight light = new SensorLight(AnalogChannels.ANALOG_PIN_A0, 1000, "light01"))
                using (SensorSound sound = new SensorSound(AnalogChannels.ANALOG_PIN_A4, 1000, "sound01"))
                using (SensorMemory mem = new SensorMemory(5000, "mem01"))
                using (rgb = new RgbLed(Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D6, "rgb01"))
                using (Relay relay = new Relay(Pins.GPIO_PIN_D7, "relay01")) {

                    speaker.BeepOK();

                    temp.OnBeforeMeasurement += OnBeforeMeasure;
                    temp.OnAfterMeasurement += OnMeasureCompleted;

                    light.OnBeforeMeasurement += OnBeforeMeasure;
                    light.OnAfterMeasurement += OnMeasureCompleted;

                    sound.OnBeforeMeasurement += OnBeforeMeasure;
                    sound.OnAfterMeasurement += OnMeasureCompleted;

                    mem.OnBeforeMeasurement += OnBeforeMeasure;
                    mem.OnAfterMeasurement += OnMeasureCompleted;

                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }
    }

This solution is publishing sensor data to the Mosquitto MQTT Service running on Microsoft Azure.  The data is displayed using the [IoT Dashboard](https://github.com/MakerDen/IoT-Dashboard).

![IoT Dashboard](https://github.com/MakerDen/IoT-Maker-Den-NETMF/blob/master/MakerDen/Lab%20Code/IoTDashboard.JPG)


