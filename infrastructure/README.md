# Dynocard Infrastructure deployment/provisioning scripts

## IoT Dynocard Edge Demo Deployment
### Step 1: Install Pre-Requisites
The automation scripts require the following software to be installed:
 - Powershell
 - [Azure Powershell Modules](https://docs.microsoft.com/en-us/powershell/azure/install-azurerm-ps?view=azurermps-5.6.0)
 - [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
 - SQL Server command line tools (`sqlcmd`)
    - [Linux/OSX/Docker installation instructions](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-setup-tools)
    - [Windows installation instructions](https://www.microsoft.com/en-us/download/details.aspx?id=53591)

### Step 2: Configure automation scripts
In order to deploy the infrastructure the automation scripts need to know about your Azure account. The following configuration file keys control how the scripts log into Azure:
- subscription_id (required for all methods)
- tenant_id (optional, required for automatic authentication)
- client_id (optional, required for automatic authentication)
- client_secret (optional, required for automatic authentication)

Example:
```
{
    "subscription_id" : "429583a4-a2d5-5c2b-afeb-2350zd056e12",
    ...
}
```

The user may also change the resource_group and names of resources if desired. The configuration file should be straightforward in this right.

### Step 3: Deploy base infrastructure
To deploy the base infrastructure issue the following command from the `/infrastructure` directory.
```
./iot-dynocard-demo.ps1 deploy base_infrastructure
```
The scripts will automatically provision all necessary resources for the solution.

### Step 4: Deploy database schema
In order for the WebAPI to operate the database must be initialized with the proper schema. To initalize the database, issue the following command:
```
./iot-dynocard-demo.ps1 deploy db_schema
```

**Note: Be sure to configure the database connection string within the App Service.

### Step 5: Deploy and configure WebAPI
The base infrastructure deployment created an Azure App Service into which the `/code/webapi/DynoCardWebAPI` application must be deployed. If you have Visual Studio Team Services (VSTS) you can easily build and deploy the application via [App Services Continuous Delivery](https://blogs.msdn.microsoft.com/devops/2016/11/17/azure-app-services-continuous-delivery/).

<MS, is the API app able to pull its database connection string from the configuration section of the App Service blade of the azure portal?>

### Step ?: Deploy Stream Analytics Job
[Placeholder for Mike>]

### Step 6: Deploy ModBus Controller
[Placeholder for David J]

### Step 7: Deploy PowerBI visualization
[Placeholder for Mike]

### Step 8: Deploy Edge Device VMs
For this demo, the edge devices are represented by Ubutntu VMs running within Azure. This solution already has 3 VMs configured, but more can be added by simply duplicating their configuration blocks within `/infrastructure/config.json`.

First deploy a virtual network to hold the machines:
```
./iot-dynocard-demo.ps1 deploy vnet
```

Then, to deploy the first edge device, issue the following command:
```
./iot-dynocard-demo.ps1 deploy edge_device_1
```

Deploy the remaining edge devices (edge_device_2, edge_device_3) as you'd expect.

### Destroying the resources
To destroy/remove all the resources you've created, first destroy all of your edge devices and then destroy the base_infrastructure. The PowerBI configuration will need to be removed manually.
```
./iot-dynocard-demo.ps1 destroy edge_device_3
./iot-dynocard-demo.ps1 destroy edge_device_2
./iot-dynocard-demo.ps1 destroy edge_device_1
./iot-dynocard-demo.ps1 destroy base_infrastructure
```

## Operational Walkthrough
### Modbus Controller, Edge Device, and IoT Hub 
The Edge Devices are configured to read data from the Modbus Controller via the ?? protocol. The devices are configured via the IoT Edge to read data registers of the ModBus controller. The initial configuration of IoT Edge (and thus the devices) is controlled by the content in `/infrastructure/iot_edge_device.json`.

Among other things, the edge device configuration controls the docker containers (location, tag/version, environment vars, etc..) that the edge device runs. If you create custom containers, you will need to modify the container image names in this file to point to the new images.


<DJ, do you want to add more detail about communication within the edge device itself?>

### IoT Hub and Service Bus
When a message is sent by the IoT Edge device to the IoT Hub in Azure, that message is read by the Azure Service bus and duplicated into two queues, `IoTHubLoopback` and `MessageProcessing`.

**Note: This is configured within the `routes` section of of the `Microsoft.Devices/IotHubs` resource definition in `base_infrastructure.json`.
```
"routes": [
    {
        "name": "IoTHubLoopback",
        "source": "DeviceMessages",
        "condition": "true",
        "endpointNames": ["events"],
        "isEnabled": true
    },
    {
        "name": "MessageProcessing",
        "source": "DeviceMessages",
        "condition": "true",
        "endpointNames": ["dynocard-output"],
        "isEnabled": true
    }
]
```

### Service Bus, Logic App, WebAPI, and SQL Database
The Azure Logic App is triggered when a message is received by the MessageProcessing queue. The Logic App takes the message and pushes it into the SQL database via the Web API. This is configured

**Note: This is configured within the `Microsoft.Logic/workflows` resource definition in `base_infrastructure.json`.

### Service Bus, Stream Analytics, and Data Lake
The Stream Analytics job reads from the IoT Hub and saves the messages in the Data Lake
