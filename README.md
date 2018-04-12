# iot-edge-dynocard

## Business Problem
Sucker pumps are a a mainstay of many oil companies.  They run continually, extracting oil from wells in remote areas of the world.

[Image of Sucker Pump - TBD]

Oil and Gas companies want to operate the sucker pumps in an efficient, safe and environmentally responsible manner.  Companies use dynamometers (dyno) surveys determine the condition of the pump operating beneath the ground or downhole.  Medium to large oil companies can have thousands of these pumps scattered throughout remote areas of the world.  It is very costly to inspect the Dyno Cards individually for thousands of these pumps.

If the dyno card output can be evaluated on the sucker pump to determine if the pump in not operating correctly then the pump can be stopped or potentially slowed down.

## Business Solution Demo
The goal of this Dyno Card demo is to show how Azure technologies can be applied to 

```
1. Detect issues at the edge
2. Send both immediate and historical dyno card messagse to the cloud
3. Visualize both the surface and pump card
```

## Technical Solution
This github repo demostrates how to utilize:

```
1. Azure IoT Edge modules to retrieve Dyno Card data using both Modbus and OPC Server interfaces
2. Save Dyno Card data to a local SQL Server Data Store
3. Azure ML to detect issues with Sucker Pumps
4. Azure IoT Edge to send messages to IoT Hub
5. Use a Stream Analytics Jobs to save data to an Azure Data Lake
6. Use a Logic App to save data through a Web API into a database
7. User Power BI to visualize the Dyno Card data
```

![cover](./images/iot-edge-dynocard-arch.JPG)

The [[Wiki|WikiLink]] contains more information about each of the individual portion of this demo.