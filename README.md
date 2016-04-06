# GSMComm
GSMComm is a .NET library for sending and receiving SMS using any compatible GSM device. It connects via the COM ports on your host machine and supports USB COM ports as well.

## Components
The library consists of 3 components.

- **GSMCommunication** is the core library which handles communication between the GSM device and your application via a COM port.
- **GSMCommServer** is a fully web server which allows sending and receiving SMS messages.
- **PDUConverter** contains classes for converting between .NET objects and the PDU format. This is used primarily to send and decode SMS messages.


## Usage
### GSMCommServer
To use the **GSMCommServer**, it's as simple as creating a console application and adding a reference to the library. Then, you can do something like below.

```cs
using (SmsServer server = new SmsServer())
{
	// Set the COM port for communicating with the device. (By default this is COM1)
	server.PortName = "COM9";
	
	// Set the baud rate. (By default this is 19200)
	server.BaudRate = 115200;

	// Allow anonymous users to use the web service. (By default this is false)
	server.AllowAnonymous = true;

	// Set the port on which our web service will run. (By default this is 2000)
	server.NetworkPort = 8080;

	// Start the server.
	server.Start();
}
```

Once you've finished using it, you can stop the server.

```cs
server.Stop();
```


### GSMCommunication
If you'd like to write your own application which makes use of SMS capabilities, you can use the **GSMCommunication** library like below.

```cs
// Create a communications object on COM port 9 with the baud rate 115200.
GsmCommMain comm = new GsmCommMain(9, 115200);
```

Once you've instantiated the object, you can use it like below.

#### Sending an SMS
```cs
// Set a phone number and message.
string phoneNumber = "+11111111111";
string text = "Hello world.";

try
{
	// Attempt to send the message.
    comm.SendMessage(new SmsSubmitPdu(text, phoneNumber));

	// Message sending was successful!
}
catch (CommException e)
{
	// There was an issue sending the message.
}
```


#### Reading SMS from the SIM card
```cs
// Read the messages from the SIM card. (SM specifies the SIM storage.)
DecodedShortMessage[] messages = comm.ReadMessages(PhoneMessageStatus.ReceivedUnread, "SM");

foreach (DecodedShortMessage message in messages)
{
    SmsPdu pdu = message.Data;

	if (pdu is SmsDeliverPdu)
	{
		// Cast the message data.
		SmsDeliverPdu msgPdu = (SmsDeliverPdu)pdu;

		// Get the data from the message.
		string phoneNumber = pdu.OriginatingAddress;
		string text = pdu.UserDataText;
        DateTime timestamp = pdu.GetTimestamp().ToDateTime();

		// Do something with the data.
	}
	else
	{
		// There's an issue with this message.
	}
}
```