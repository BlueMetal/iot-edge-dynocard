![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/oil%26gas-deployment.png)

**Table of Contents** 

- [1.0 Deployment Guide](#10-deployment-guide)
- [2.0 What are Paired Regions](#20-what-are-paired-regions)
- [3.0 ARM Template Input Parameters](#30-arm-template-input-parameters)
- [4.0 Deployment Guide](#40-deployment-guide)
    - [4.1 ARM Template Deployment Using Azure Portal](#41-arm-template-deployment-using-azure-portal)
        - [4.1.1 Inputs](#411-inputs)
        - [4.1.2 Outputs](#412-Outputs)
    - [4.2 ARM Template Deployment Using Azure CLI](#42-arm-template-deployment-using-azure-cli)
- [5.0 Post Deployment Steps](#50-post-deployment-steps)
    - [5.1 Verify Containers in Edge VM and Azure Portal](#51-verify-containers-in-edge-vm-and-azure-portal)
    - [5.2 Update IoT Hub Device Primary key in Web API Application Settings](#52-update-iot-hub-device-primary-key-in-web-api-application-settings)
    - [5.3 Perform Device Twin Operation on Edge VM [Optional]](#53-perform-device-twin-operation-on-edge-vm-[optional])
- [6.0 Machine Learning Configuration](#60-machine-learning-configuration)
    - [6.1 Login to Data Science Virtual Machine](#61-login-to-data-science-virtual-machine)
    - [6.2 Clone iot-edge-dynocard git repository](#62-clone-iot-edge-dynocard-git-repository)
    - [6.3 Update the config.json file](#63-update-the-config.json-file)
    - [6.4 Install the Python Packages](#64-install-the-python-packages)
    - [6.5 Launch Jupyter server](#65-launch-jupyter-server)
    - [6.6 Create train4dc Notebook](#66-create-train4dc-notebook)
    - [6.7 Run train4dc experiment](#67-run-train4dc-experiment)
    	- [6.7.1 Check the model.pkl file](#671-check-the-model.pkl-file)
	- [6.7.2 Check train4dc experiment in ML service workspace](#672-check-train4dc-experiment-in-ml-service-workspace)
	- [6.7.3 Check the model in ML service workspace](#673-check-the-model-in-ml-service-workspace)
    - [6.8 Create Score4dc notebook](#68-create-score4dc-notebook)
    - [6.9 Run score4dc experiment](#69-run-score4dc-experiment)
    	- [6.9.1 Check the service_schema.json file](#691-check-the-service_schema.json-file)
    	- [6.9.2 Check score4dc experiment in ML service workspace](#692-check-score4dc-experiment-in-ml-service-workspace)
    - [6.10 Update myenv.yaml file](#610-update-myenv.yaml-file)
    - [6.11 Create deployment Notebook](#611-create-deployment-notebook)
    - [6.12 Run Deployment Notebook](#612-run-deployment-notebook)
    	- [6.12.1 Check the image](#6121-check-the-image)
    	- [6.12.2 Check the Deployment](#6122-check-the-deployment)
	
	

## 1.0 Deployment Guide

This Document explains about how to deploy Oil & Gas solution using ARM Template. In this Document explained about two ways of deploying solution.

##### - Using Azure portal

##### - Using Azure CLI

This document explains about input parameters, output parameters and points to be noted while deploying ARM Template.

## 2.0 What are paired regions

Azure operates in multiple geographies around the world. An Azure geography is a defined area of the world that contains at least one Azure Region. An Azure region is an area within a geography, containing one or more datacenters.
Each Azure region is paired with another region within the same geography, together making a regional pair. The exception is Brazil South, which is paired with a region outside its geography.

IoT Hub Manual Failover Support Geo-Paired Regions:

| **S.NO**  | **Geography**         | **Paired Regions**
| ------    | -------------         | ---------------------------------------| 
|1          | North America         | East US 2         -     Central US     | 
|2          | North America         | Central US         -   East US 2       | 
|3          | North America         | West US 2          -   West Central US |  
|4          | North America         | West Central US    -   West US 2       | 
|5          | Canada                | Canada Central     -   Canada East     |
|6          | Canada                | Canada East        -   Canada Central  | 
|7          | Australia             | Australia East     -   Australia South East|
|8          | Australia             | Australia South East -   Australia East|
|9          | India                 | Central India     -     South India    | 
|10         | India                 | South India       -    Central India   |
|11         | Asia                  | East Asia         -     South East Asia|
|12         | Asia                  | South East Asia   -    East Asia       |
|13         | Japan                 | Japan West        -     Japan East     |
|14         | Japan                 | Japan East        -     Japan West      |
|15         | Korea                 | Korea Central      -     Korea South    |
|16         | Korea                 | Korea South         -   Korea Central   | 
|17         | UK                    | UK South             -   UK West        |
|18         | UK                    | UK West               - UK South        |



## 3.0 ARM Template Input Parameters

In the parameters section of the template, specify which values you can input when deploying the resources. These parameter values enable you to customize the deployment by providing values that are tailored for an environment (such as dev, test, and production). You are limited to 255 parameters in a template. You can reduce the number of parameters by using objects that contain multiple properties.

| **Parameter Name**  | **Description**     | **Allowed Values**    | **Default Values**   |                                                                                                            
| -------------       | -------------       | -----------------     | ------------         |
| **Solution Type**   | Choose the solution deployment type from the drop down, for more information about the solution deployment type navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki      | Basic, Standard, Premium | Basic|
| **Edge VM + Simulator VM**   | Choose Yes/No to add Modbus VM as part of Solution deployment, for more information about the Modbus VM navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki   | Yes, No    | No |
| **Data Science VM**   | Choose Yes/No to add Data Science VM as part of Solution deployment    | Yes, No   | No |
| **Data Science VM size**   | Choose size for Data Science VM   | Standard_B2s,Standard_B2ms,Standard_B4ms,Standard_B8ms,Standard_DS1,Standard_DS2,Standard_DS3,Standard_DS4,Standard_DS11,Standard_DS12,Standard_DS13,Standard_DS14,Standard_DS1_v2,Standard_DS2_v2,Standard_DS3_v2  | Standard_B2s |
| **Data Lake Location**   | Choose location for Data Lake Store, for more information about the Data Lake Store navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki   | Eastus2,Centralus, Northeurope, westeurope  | Eastus2 |
| **Geo Paired Primary Region**  | Choose geo-paired region,if you have selected standard (or) premium option in the solution type inputparameter, for more information about the geo-paired-region navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki  | EastUS2,CentralUS,WestUS2,WestCentralUS,CanadaCentral,CanadaEast,         AustraliaEast,AustraliaSouthEast,CentralIndia,SouthIndia,EastAsia,               SouthEastAsia,JapanWest,JapanEast,KoreaCentral,KoreaSouth,UKSouth,               UKWest  | eastus2|
| **oms-region**    | Choose location for OMS Log Analytics, for more information about the oms-region navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki   | australiasoutheast, canadacentral, centralindia, eastus, japaneast, southeastasia, uksouth, westeurope    | EastUs |
| **appInsightsLocation**   | specify the region for application insights, for more information about the appInsightsLocation navigate to https://github.com/nvtuluva/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md in wiki    | eastus, northeurope,  southcentralus, southeastasia, westeurope, westus2 | westus2 |
| **sqlAdministratorLogin**     | Provide the user name for the SQL server, please make a note of Username this will be used further. | sqluser | sqluser |
| **sqlAdministratorLoginPassword**   | Provide the password for the SQL server, make a note of the Password this will be used further.| Password@1234  | Password@1234 |
| **Azure Login** |  Provide the AZURE portal login username. This will be helpful to authenticate data lake store account in stream analytics outputs section. | user@domain.com | user@domain.com |
| **Azure Password** |  Provide the AZURE portal login password. This will be helpful to set the modules on iotedge device through iot-edge.sh script. | Password | Password |
| **tenantId** | Tenant Id for the subscription. This will be helpful to authenticate data lake store account in stream analytics outputs section     | xxxxxxxx-xxxx-xxxx-xxxx-c5e6exxxcd38          | xxxxxxxx-xxxx-xxxx-xxxx-c5e6exxxcd38 |
| **vmsUsername** | Username to Login into Modbus VM and Edge VM. please make a note of Username this will be used further | adminuser  | Adminuser      |
| **vmsPassword** | Please provide VM login password, please make a note of Password this will be used further | Password@1234    | Password@1234  |
| **Web App Buildfile Url** | Please use web app build file which is under /builds folder  by storing it in a public storage | https://github.com/nvtuluva/iot-edge-dynocard/raw/master/builds/DynoCardAPI_with_appinsights.zip    | https://github.com/nvtuluva/iot-edge-dynocard/raw/master/builds/DynoCardAPI_with_appinsights.zip     |
| **Storage Uri** |  Please use the sql bacpac file which is under /builds folder  by storing it in a public storage | https://projectiot.blob.core.windows.net/oilgas-iot/db4cards.bacpac | https://projectiot.blob.core.windows.net/oilgas-iot/db4cards.bacpac |
| **Plcsimulatorv1** | Please provide Plcsimulatorv1 exe file which is under /builds folder  by storing it in a public storage     | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/SimSetup.msi          | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/SimSetup.msi |
| **vcredist** | Please provide vcredist exe file which is under /builds folder  by storing it in a public storage | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/vcredist_x86.exe  | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/vcredist_x86.exe      |
| **Plcsimulatorv2** | Please provide Plcsimulatorv2 exe file which is under /builds folder  by storing it in a public storage | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/ModRSsim2.exe    | https://projectiot.blob.core.windows.net/oilgas-iot/ModbusSimulator/ModRSsim2.exe  |
| **Device Name** | Please provide the IoT Edge device name | your device name    | your device name  |

### Note:  

Allowed Values are updated based on the availability of azure resources based on Region w.e.f DT 18th Aug 2018. Microsoft may introduce availability of azure resources in future. More information can be found in https://azure.microsoft.com/en-in/status/ 

## 4.0 Deployment Guide

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. The template consists of JSON and expressions that you can use to construct values for your deployment. You use the same template to repeatedly deploy your application during every stage of the application lifecycle.
Resource Manager provides a consistent management layer to perform tasks through Azure PowerShell, Azure CLI, Azure portal, REST API, and client SDKs.

*   Deploy, manage, and monitor all the resources for your solution as a group, rather than handling these resources individually.
*   Repeatedly deploy your solution throughout the development lifecycle and have confidence your resources are deployed in a consistent state.
*   Manage your infrastructure through declarative templates rather than scripts.
*   Define the dependencies between resources so they're deployed in the correct order.
*   Apply access control to all services in your resource group because Role-Based Access Control (RBAC) is natively integrated into the management platform.
*   Apply tags to resources to logically organize all the resources in your subscription.

### 4.1 ARM Template Deployment using Azure Portal

1.  Click the below **Git hub** repo URL.

**https://github.com/nvtuluva/iot-edge-dynocard/tree/master**

2. Select **main-template.json** from **master** branch as shown in the following figure.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/5.PNG)

3. Select **Raw** from the top right corner.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/6.PNG)

4. **Copy** the raw template and **paste** in your azure portal for template deployment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/7.PNG)

To deploy a template for Azure Resource Manager, follow the below steps.

1.  Go to **Azure portal**.

2.  Navigate to **Create a resource (+)**, search for **Template deployment**.

3.  Click Create button and click **Build your own Template in the editor** as shown in the following figure.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/8.png)

4.  The **Edit template** page is displayed as shown in the following figure. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/9.png)

5.  **Replace / paste** the template and click **Save** button.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/10.png)

6.  The **Custom deployment** page is displayed as shown in the following.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg01.PNG)

### 4.1.1 Inputs
These parameter values enable you to customize the deployment by providing values. There parameters allow to choose the solution type, region and credentials to authenticate SQL Database and Virtual Machines.

7.  If you choose **yes** for **Edge VM + Simulator VM** then **then Edge VM and Simulator VMs** will be **deployed** with in the solution.

8.  If you choose **No** then the **Edge VM + Simulator VM** vm  will be **not deployed** with in the solution, If you choose No, then the Edge VM and Simulator VM will be not deployed with in the solution, we have to manually deploy IoT Edge modules in Edge VM for more information refer the section 5.3 Perform Device Twin Operation on Edge VM.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg02.PNG)

9.  If you choose **yes** for **Data Science VM** then Pre-installed **docker** will be **deployed** with in the solution.

10. If you choose **No** for **Data Science VM** then Pre-installed **docker** will **not be deployed** with in the solution.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg03.PNG)

**Parameters for Basic Solution**

11. Deploy the template by providing the parameters in custom deployment settings as shown in the following figure.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg04.PNG)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/parameters12.png)

**Parameters for Standard Solution**

12. If you want to deploy the core with monitoring you must enter the below parameters

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg05.PNG)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/parameters12.png)

**Parameters for Premium Solution**

13. If you want to deploy the core with Hardening and Monitoring you must enter the below parameters.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/dg06.PNG)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/parameters12.png)

14. Once all the parameters are entered, select the **terms and conditions** check box and click **Purchase**.

15. After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

16. When you choose Costing model as **Standard** and the **Modbus VM** and **Data Science VM** as **Yes**, then the below components will be installed.

* 3-Virtual machines (2 windows & 1 Linux)

Windows VMS

**Dyno card VM** which install pre-installed software's for dyno card VM.

**Data Science VM** which install pre-installed docker.

Linux VM

**Edge VM** is used to create IoT Edge device and installs modules in IoT Edge device

* 2-Web App

* 1-Application Insights

* 1-Data Lake Storage

* 1-IoT HUB

* 1-Log analytics

* 1-Logic app

* 1-service bus namespace

* 2-SQL database

* 1-Storage account

* 1-Stream Analytics job

* 1-Traffic Manager

17. Once the solution is deployed successfully navigate to the resource group, select the created **resource group** to view the list of resources that are created in the Resource Group as shown in the following figure.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/17.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/18.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/19.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/20.png)

### 4.1.2 Outputs

The Outputs section consists of the values that are returned from deployment. The output values can be used for further steps in Solution Configuration

1. Go to **Resource group** -> click **deployments** -> select **Microsoft Template**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/21.png)

2. Click **outputs**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/22.png)

### 4.2 ARM Template Deployment Using Azure CLI

Azure CLI is used to deploy your resources to Azure. The Resource Manager template you deploy, can either be a local file on your machine, or an external file that is in a repository like GitHub.  

Azure Cloud Shell is an interactive, browser-accessible shell for managing Azure resources. Cloud Shell enables access to a browser-based command-line experience built with Azure management tasks in mind. 

Deployment can proceed within the Azure Portal via Azure Cloud Shell. 

Customize main-template.parameters.json file 

1. Log in to **Azure portal.** 

2. Open the prompt. 

3. Select **Bash (Linux)** environment as shown in the following figure. 

4. Select your preferred **Subscription** from the dropdown list.  

5. Click **Create Storage** button as shown in the following figure. 

6. Copy **main-template.json** and **main-template.parameters.json** to your Cloud Shell before updating the parameters. 

7. Create **main-template.json** using the following command. 

**vim main-template.json**
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/23.png)

8. Paste your **main-template.json** in editor as shown below and save the file. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/24.PNG) 

9. Paste your **main-template.parameters.json** in editor. 

10. Update the following parameters in main-template.json file 

* Solution Type 

*   Edge VM + Simulator VM

*   Data Science VM

*   Data Science VM Size

*   dataLakelocation

*   machineLearningLocation

*   geo-Paired-Primary-region

*   oms-region

*   appInsightsLocation

*   sqlAdministratorLogin

*   sqlAdministratorLoginPassword

*   azureLogin

*   tenantId

*   vmsUsername

*   vmsPassword
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/25.PNG)

11. Create Resource Group for oilandgas Solution 

12. Use the **az group create** command to create a **Resource Group** in your region.
 
**Description:** To create a resource group, use az group create command, 

It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l). 

**Syntax: az group create -n <resource group name> -l <location>**.

**Ex: az group create -n <****> -l <***>**
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/26.png)
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/27.png)

Execute the Template Deployment 

Use the **az group deployment create** command to deploy the ARM template 

**Description:** To deploy the ARM Template, you require two files: 

**main-template.json** – contains the resource & its dependency resources to be provisioned from the ARM template 

**main-template.parameters.json** –contains the input values that are required to provision respective SKU & Others details. 

**Syntax:  az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in previous section> -n deploy >> <provide the outputs filename>**

**Ex: az group deployment create --template-file './main-template.json' --parameters '@./main-template.parameters.json' -g oilandgas-iot -n deploy >> outputs.txt**
 
 ![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/28.png)

15. Deployment may take between 15-20 minutes depending on deployment size. 

16. After successful deployment you can see the deployment outputs as follows.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/29.png)

## 5.0 Post Deployment steps

### 5.1 Verify Containers in EdgeVM and Azure Portal

1. Go to **Resource Group** ->click on **iotEdge** VM.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/30.png)

2. Copy the Public IP Address of the iotEdge VM.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/31.png)

3. Login to the VM through putty.

4. Paste the public ip at Host Name (or IP address) and click on open.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/32.png)

5. Enter credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

6. Once login successful the below welcome message is displayed.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/33.png)

7. Here you can check the device and device modules in IoT Edge VM.

**docker ps** 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/moduleslist.png)

8. Go to resource group -> click on **iothub26hs3**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/35.png)

9. Navigate to **IoT Edge** blade under **Automatic Device Management** section.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/iotedge.png)

10. Here we created and configured device from the IoT Edge VM. Click on **iot-dynocard-demo-device_1** device you can see the modules.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/iotedge.png)

11. We can see the created modules in device details 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/modules-running.png)

### 5.2 Update IoT Hub Device Primary key in Web API Application Settings

1. Go to **Resource Group** -> Click on **iothub26hs3**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/46.png)

2. Navigate to **IoT Edge** blade under **Automatic Device Management** section.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/iotedge.png)

3. Click on iot-dynocard-demo-device_1 device as shown below and copy the connection string-primary key.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/48.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/49.png)

4. Go to **resource group** -> open the primary web app **webapi26hs3** in the app service.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/50.png)

5. Navigate to **Application settings** blade.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/51.png)

6. Click **+ Add new setting** in the Application settings, enter name and value in the new setting.

Name: **DeviceConnectionString**

Value: **[Device connection string-primary key]**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/52.png)

7. Then click **Save**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/53.png)

### 5.3 Perform Device Twin operation on Edge VM [Optional]

**Note**: This step is required only when user wants to run simulator on their own system. If Modbus VM is Deployed as part of the solution, then skip this step and continue with next step.

Need to perform device twin operation on Modbus IoT Edge Module when solution deployed with an option Modbus VM as **No**. when Modbus VM deployment option is chosen as **no** then there is no Simulator VM is deployed.

Install IoTedge Modules by executing **iot-edge-Manual.sh** Under scripts from GitHub repo script by providing Required input parameters.

https://github.com/nvtuluva/iot-edge-dynocard/blob/master/scripts/iot-edge-Manual.sh

Needs to update slave connection IP address in Modbus module configuration in IoT Edge Modules with actual Simulator IP address.


1. Go to **Resource Group** ->click on **iotEdge VM**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/54.png)

2. **Copy** the **Public IP Address** of the **IoTEdge** VM.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/55.png)

3. Login to the VM through putty.

4. Paste the public IP at Host Name (or IP address) and click on open.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/56.png)

5. Enter credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

6. Once login successful the below welcome message is displayed.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/57.png)

7. Here you can check the device and device modules in IoT Edge VM.

**docker ps**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/moduleslist.png)

8. Check the logs of Modbus container by executing below command.

**docker logs modbus**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/59.png)

As per above diagram slave connection is 52.186.11.164 and it’s trying to connect with 52.186.11.164 which is not available. We need to update slave connection IP address with correct IP Address using twin operation.

10. Go to the **resource group** and choose **IoT Hub**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/60.png)

11. Click on **IoT Hub** and navigate to **IoT Edge** blade under **Automatic device management** section.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/61.png)

12. Click on IoT Device **iot-dynocard-demo-device_1** device to check modules.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/iotedge.png)

13. We can see the created **modules** in **device details**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/modules-running.png)

14. Click on **modbus** module.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/64.png)

15. Click on **Module twin**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/65.png)

16. Change IP Address of slave Connection and click on **Save**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/66.png)

17. In this scenario IP address changed to 104.42.153.165

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/67.png)

18. Now go back to Edge VM to verify slave connection IP Address.

19. Stop the Modbus container by passing below command.

**docker stop modbus**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/68.png)

20. Start the Modbus container by passing below command.

**docker start modbus**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/69.png)

21. Now verify slave connection string by checking logs of Modbus.

**docker logs modbus**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/70.png)

Now slave connection IP address is updated with New IP Address.

## 6.0 Machine Learning Configuration

### 6.1 Login to Data Science Virtual Machine

Go to the resource group and click on Data Science Virtual Machine. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d01.png)
 
Copy the public IP address.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d02.png)
 
Open Remote Desktop Connection and paste the public IP address

Click **on Connect**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d03.png)

Provide user name and password and click on OK. User Name : **adminuser** Password : **Password@1234**

**Note**:Credentials might vary depends on deployment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d04.png)

Click on **OK**.

You will be log in to Data Science Virtual Machine.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d05.png)
 
### 6.2 Clone iot-edge-dynocard git repository

Open command prompt in Data Science Virtual Machine

Create a directory with a unique name.

**mkdir OilNGas**

navigate to the directory that you created

**cd OilNGas**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d06.png)
 
Clone the iot-edge-dynocard git code using the below command

**git clone https://github.com/nvtuluva/iot-edge-dynocard.git**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d07.png)
 
Open File Explorer and Navigate to C:\OilNGas directory to see the downloaded git code.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d08.png) 
 
Navigate to the 

C:\OilNGas\iot-edge-dynocard\code\dynocard_alert\modules\edge_ml

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d09.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d010.png)

### 6.3 Update the config.json file

**subscription_id :** subscription id in which Machine Learning Service Work space is deployed

**resource_group :** Resource group name where Machine Learning Service Work space is deployed

**work space_name :** Name of the Machine Learning service Work space

To get the name of the ML Service Work space, Go to Resource group and click on Machine Learning Service workspace. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d011.png)
 
Copy the name of ML Service workspace name, Resource group name and subscription ID.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d012.png)

Go back to Data science VM and update the config.json file.

{
    **"subscription_id": "xxxxxxxxxxxxxxxxxxxxx",**
    
    
   **"resource_group": " AS-OilNGas0623",**
    
   **"workspace_name": " mlworkspace43ysm"**
}

Save the config.json file

Open command prompt and change the directory to edge_ml

Cd C:\OilNGas\iot-edge-dynocard\code\dynocard_alert\modules\edge_ml

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d013.png)
 
### 6.4 Install the Python Packages 

Activate the conda 

**conda activate AzureML**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d014.png)
 
Install Azure ML SDK.

**pip install azureml-sdk**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d015.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d016.png)

Update azureml-sdk

**pip install --upgrade azureml-sdk**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d017.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d018.png)
 
Install azureml.datacollector module

**pip install azureml.datacollector**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d019.png)
 
Install azureml-webservice-schema

**pip install azureml-webservice-schema**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d020.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d021.png)
 
Install inference_schema

**pip install inference_schema**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d022.png)

### 6.5 Launch Jupyter server

Launch Jupyter Notebook server with the following command.

jupyter notebook

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d023.png)
 
Once you execute the above command, a prompt will be opened for selecting browser.

Select a browser and click on OK.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d024.png)
 
All the files in the current directory will be opened in Jupyter server like below.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d025.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d026.png)
 
### 6.6 Create train4dc Notebook

Click on **New**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d027.png)

Select **Python3** from the drop-down list

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d028.png)
 
A new tab will be opened with the name **Untitled**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d029.png)

To change the name of the file, click on **File** and select **Rename**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d030.png)
 
Enter **train4dc** in the text box and click on **Rename**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d031.png)
 
Now the Notebook name is changed to train4dc.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d032.png)
 
First, let's import the azureml.core package. This contains core packages, modules and classes for Azure Machine Learning. 

Copy the below code and paste it in the first cell of the Notebook.

import azureml.core

from azureml.core import Workspace, Experiment, Run

#check core SDK version number

print("Azure ML SDK Version: ", azureml.core.VERSION)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d033.png)
 
To insert a new cell, click on Insert and select Insert Cell Below 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d034.png)

A new cell will be created.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d035.png)
 
Paste the below code to load the workspace.

ws = Workspace.from_config()

print(ws.name, ws.location, ws.resource_group, ws.location, sep = '\t')

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d036.png)

Insert a new cell and paste the below code to create the experiment

**experiment = Experiment(workspace = ws, name = "train4dc")**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d037.png)
 
Create a new cell and paste the below code.

run = experiment.start_logging()

#import libaries
from sklearn.svm import SVC
import pickle

#features - height, width, and shoe size
X = [[181, 80, 44], [177, 70, 43], [160, 60, 38], [154, 54, 37], [166, 65, 40], [190, 90, 47], [175, 64, 39],
     [177, 70, 40], [159, 55, 37], [171, 75, 42], [181, 85, 43]]

#category - male | female
Y = ['male', 'male', 'female', 'female', 'male', 'male', 'female', 'female', 'female', 'male', 'male']

#classify the data

clf = SVC()

clf = clf.fit(X, Y)

#predict a value & show accuracy

X_old = [[190, 70, 43]]

print('Old Sample:', X_old)

print('Predicted value:', clf.predict(X_old))

print('Accuracy', clf.score(X,Y))

#create the outputs folder

#os.makedirs('./outputs', exist_ok=True)

#export model

print('Export the model to model.pkl')

f = open('./model.pkl', 'wb')

pickle.dump(clf, f)

f.close()

#import model

print('')

print('Import the model from model.pkl')

f2 = open('./model.pkl', 'rb')

clf2 = pickle.load(f2)

#predict new value

X_new = [[154, 54, 35]]

print('New Sample:', X_new)

print('Predicted class:', clf2.predict(X_new))

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d038.png) 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d039.png)
 
Insert three more new cells and paste **run, experiment** and **run.complete()** as shown below

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d040.png)
 
Click on new cell and paste the below code to register the model.

**from azureml.core.model import Model
model = Model.register(workspace = ws,
                       model_path ="model.pkl",
                       model_name = "anomaly_detect",
                       description = "Dynocard anomaly detection")**
		 
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d041.png)

Save the file.

### 6.7 Run train4dc experiment

To run a cell, select it and click on **Run**

Select the first cell and click on run to import the azureml.core packages and to print azureml sdk version.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d042.png)
 
You will get the below output under the cell.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d043.png)

Select the next cell and click on run

When you run **ws = Workspace.from_config** below, you will be prompted to log in to your Azure subscription that is configured in the config.json file. Once you are connected to your workspace in Azure cloud, you can start experimenting.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d044.png)

You will get the below output and a prompt will be opened in the browser.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d045.png)
 
Login with your Azure Credentials.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d046.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d047.png)
 
Go back to the notebook.

Once your authentication is successful, **Interactive authentication successfully completed** message will be displayed.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d048.png)
 
Run the next cell to create the experiment. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d049.png)
 
Run the next cell to run the train4dc experiment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d050.png)

You will get the below output

*Old Sample: [[190, 70, 43]]
Predicted value: ['male']
Accuracy 1.0
Export the model to model.pkl*

*Import the model from model.pkl
New Sample: [[154, 54, 35]]
Predicted class: ['female']*

Run the next cell **run** to view a detail report of the run history on Azure Portal.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d051.png)
 
Run the next cell **experiment** to view all runs under the experiment section.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d052.png)

Run the next cell **run.complete()** to complete the experiment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d053.png)
 
Run the next cell to register the model.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d054.png)
 
You will get the below output

Registering model anomaly_detect

#### 6.7.1 Check the model.pkl file

Go to **Home** tab

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d055.png)
 
Here you can find the generated model.pkl file.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d056.png)

##### 6.7.2 Check train4dc experiment in ML service workspace

To see the created experiment go to Resource group and click on ML service Workspace

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d057.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d058.png)
 
Navigate to the Experiments blade to see the created experiment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d059.png)
 
Click on train4dc experiment to see the all runs

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d060.png)
 
Here you can see the run status as completed.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d061.png)

#### 6.7.3 Check the model in ML service workspace

Navigate to the Models blade to see the registered model.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d062.png)

### 6.8 Create Score4dc notebook

Go back to the Jupyter sever in Data Science VM 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d063.png)
 
create a new Notebook score4dc

click on **New**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d064.png)
 
Select the python3

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d065.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d066.png) 

Rename the Notebook as **score4dc**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d067.png)
 
Let's import the azureml.core package. This contains core packages, modules and classes for Azure Machine Learning. 

**import azureml.core**

**from azureml.core import Workspace, Experiment, Run**

**# check core SDK version number
print("Azure ML SDK Version: ", azureml.core.VERSION)**

Paste the below code in the next cell to configure the workspace

**ws = Workspace.from_config()
print(ws.name, ws.location, ws.resource_group, ws.location, sep = '\t')**

paste the below code in next cell to create experiment

**experiment = Experiment(workspace = ws, name = "score4dc")**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d068.png)
 
Insert a new cell and paste the below code to start the experiment logging.

**runscore4dc = experiment.start_logging()**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d069.png)
 
Insert a new cell to paste the below content. 

%%writefile score4dc.py

#Common modules

import os

from azureml.datacollector import ModelDataCollector

from azureml.webservice_schema.data_types import DataTypes

from azureml.webservice_schema.schema_generation import generate_schema

from azureml.webservice_schema.sample_definition import SampleDefinition

#from azureml.api.schema.dataTypes import DataTypes

#from azureml.api.schema.sampleDefinition import SampleDefinition

#from azureml.api.realtime.services import generate_schema

from inference_schema.schema_decorators import input_schema, output_schema

from inference_schema.parameter_types.numpy_parameter_type import NumpyParameterType

#Init routine - Web service

def init():  

   #Load module
   
   from sklearn.externals import joblib

   #Variables
   
   global model
    
   #Load model
   
   model = joblib.load('model.pkl')

#Run routine - Web service

def run(input_str):

   #Load module
   
   import json

   #What type of input
   
   #print("Input_str: type")
   
   #print (type(input_str))
   
   #print ("")
   
   #print(input_str)

   #Convert to dictionary
   
   if type(input_str) is str:
   
   input = json.loads(input_str)
   
  else:
    
   input = input_str

   #Fake a prediction
   
   prediction = write_msg(input['Id'], input['Timestamp']);

   #Return json
   
   return prediction

#Read Input Message Rountine - Read in mod bus sample message

def read_msg():

   #Load module
   
   import json

   #Create some json input
   
   in1 = ""
   
   in1 += '{ '
   
   in1 += '"Id": 0, '
   
   in1 += '"Timestamp": "2018-04-04T22:42:59+00:00", '
   
   in1 += '"NumberOfPoints": 400, '
   
   in1 += '"MaxLoad": 19500, '
   
   in1 += '"MinLoad": 7500, '
   
   in1 += '"StrokeLength": 1200, '
   
   in1 += '"StrokePeriod": 150, '
   
   in1 += '"CardType": 0, '
   
   in1 += '"CardPoints": [{ '
   
   in1 += '  "Load": 11744, '
   
   in1 += '  "Position": 145 }]'
   
   in1 += '} '

   #Return sample message
   
   return json.loads(in1)

#Number 2 Class Rountine - Classify the anomaly.

def number_to_class(argument):
   
   switcher = {
   
   1: "Full Pump",
   
   2: "Flowing Well, Rod Part, Inoperative Pump",
   
   3: "Bent Barrel, Sticking Pump",
   
   4: "Pump Hitting Up or Down",
   
   5: "Fluid Friction",
   
   6: "Gas Interference",
   
   7: "Drag Friction",
   
   8: "Tube Movement",
   
   9: "Worn or Split Barrel",
   
   10: "Fluid Pound",
   
   11: "Worn Standing Value",
   
   12: "Worn Plunger or Traveling Value"
   
   };
   
   return switcher.get(argument, "Undefined");


#Write Output Message Rountine - Randomly classify the data.

def write_msg(id, stamp):

   #Load module
   
   import json
   
   import random;

   #Five percent left tail
   
   occurs = 95

   #Grab a number 1.0 to 100.0
   
   pct1 = random.uniform(1, 100);

   #Create some json output
   
   out1 = ""
   
   out1 += '{ "Id": "' + str(id) + '", '
   
   out1 += '"Timestamp": "'+ stamp + '", '

   #Report a anomaly?
   
   if (pct1 >= occurs):
   
   out1 += '"Anomaly": "True", '
   
   else:
   
   out1 += '"Anomaly": "False", "Class": "Full Pump" }'

   #Choose random issue

if (pct1 >= occurs):

pct2 = int(random.uniform(1, 12)) + 1;

out1 += '"Class": "' + number_to_class(pct2) + '" }'

   #Return sample message

return json.loads(out1)

#Main routine - Test init() & run()

def main():

   #Turn on data collection debug mode to view output in stdout
   
   os.environ["AML_MODEL_DC_DEBUG"] = 'true';
   
   os.environ["AML_MODEL_DC_STORAGE_ENABLED"] = 'true';

   #create the outputs folder
   
   os.makedirs('./outputs', exist_ok=True);

   #Read in json, mod bus sample msg
   
   input_msg = read_msg();

   #Debugging - remove when deploying
   
   #print (" ");
   
   #print ("Input Json:")
   
   #print (input_msg);

   #Test init function
   
   init();

   #Write out json, sample response msg
   
   output_msg = run(input_msg);

   #Debugging - remove when deploying
   
   #print (" ");
   
   #print ("Output Json:")
   
   print(output_msg);

   #Sample input string
   
   input_str = {"input_str": SampleDefinition(DataTypes.STANDARD, input_msg)};

   #Generate swagger document for web service
   
   generate_schema(run_func=run, inputs=input_str, filepath='./outputs/service_schema.json');
   
#Call main

if __name__ == "__main__":

main()
    
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d070.png)    

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d071.png)

Insert three more new cells and paste **runscore4dc, experiment and run.complete()** as shown below

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d072.png)
 
Save the file.

### 6.9 Run score4dc experiment

Run the first 4 cells one after another to import azureml-sdk, connect to workspace and create score4dc experiment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d073.png)
 
Select the next cell and run the score4dc.py script code.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d074.png)
 
Once you run the cell you will get the following output “**overwriting score4dc.py**”

The first line in the cell **“%%writefile score4dc.py”** writes whole content of the cell into a file named score4dc.py.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d075.png)
 
**Now, comment the first line of the script cell and run it again.**

#writefile score4dc.py

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d075.png)
 
Now you will get the following output.

{'Id': '0', 'Timestamp': '2018-04-04T22:42:59+00:00', 'Anomaly': 'False', 'Class': 'Full Pump'}

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d076.png)
 

Run the next cells “**runscore4dc**”, “**experiment**” and “**runscore4dc.complete()**”

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d077.png)
 
#### 6.9.1 Check the service_schema.json file

Go to **Home** tab

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d078.png)

Click on **Outputs** folder

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d079.png)
 
Here you can see the generated service_schema.json file.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d080.png)
 
Click on the file to see the content.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d081.png)

The file contains below content.

**{"input": {"input_str": {"internal": "gANjYXp1cmVtbC53ZWJzZXJ2aWNlX3NjaGVtYS5fcHl0aG9uX3V0aWwKUHl0aG9uU2NoZW1hCnEAKYFxAX1xAlgJAAAAZGF0YV90eXBlcQNjYnVpbHRpbnMKZGljdApxBHNiLg==", "swagger": {"type": "object", "additionalProperties": {"type": "object"}, "example": {"Id": 0, "Timestamp": "2018-04-04T22:42:59+00:00", "NumberOfPoints": 400, "MaxLoad": 19500, "MinLoad": 7500, "StrokeLength": 1200, "StrokePeriod": 150, "CardType": 0, "CardPoints": [{"Load": 11744, "Position": 145}]}}, "type": 0, "version": "1.0.33"}}}**

#### 6.9.2 Check score4dc experiment in ML service workspace

To see the created experiment go to ML service Workspace

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d082.png)
 
Navigate to the Experiments blade to see the created experiment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d083.png)
 
Click on score4dc experiment to see the graph.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d084.png)
 
### 6.10 Update myenv.yaml file

Go back to Data science VM and click on Home tab of Jupyter server

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d085.png)
 
Click on myenv.yaml and replace the below content in the file.

 #Conda environment specification. The dependencies defined in this file will

 #be automatically provisioned for runs with userManagedDependencies=False.

#Details about the Conda environment file format:

#https://conda.io/docs/user-guide/tasks/manage-environments.html#create-env-file-manually


name: project_environment

dependencies:

#The python interpreter version.

#Currently Azure ML only supports 3.5.2 and later.

- python=3.6.2

- pip:

#Required packages for AzureML execution, history, and data preparation.

  - azureml-defaults
  
  - scikit-learn==0.20.0
  
  - azureml.webservice-schema
  
  - inference-schema[numpy-support]
  
  - azureml.datacollector==0.1.0a13

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d086.png)
 
Save the file.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d087.png)
 
### 6.11 Create deployment Notebook

Create a notebook named deployment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d089.png)
 
Let's import the azureml.core package. This contains core packages, modules and classes for Azure Machine Learning. 

**import azureml.core
from azureml.core import Workspace, Experiment, Run**

**#check core SDK version number
print("Azure ML SDK Version: ", azureml.core.VERSION)**

Paste the below code in the next cell to configure the workspace

**ws = Workspace.from_config()
print(ws.name, ws.location, ws.resource_group, ws.location, sep = '\t')**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d090.png)
 
Paste the below code in the next cell.

**from azureml.core.model import Model
model_name = "anomaly_detect"
model = Model(ws, model_name)**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d091.png)
 
Paste the below code in the next cell.

**from azureml.core.image import ContainerImage
image_config = ContainerImage.image_configuration(execution_script="score4dc.py",
                                                    runtime="python",
                                                    conda_file="myenv.yml",
                                                    dependencies=["inputs.json", "model.pkl", "service_schema.json", "train4dc.py"],
                                                    description="image for model",
                                                    enable_gpu=True
                                                    )**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d092.png)
 
Paste the below code in the next cell.


**from azureml.core.webservice import AciWebservice
aciconfig = AciWebservice.deploy_configuration(cpu_cores=1, 
                                               memory_gb=1, 
                                               description='Predict anomaly')**
					       
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d093.png)

 
Paste the below code in the next cell.

**%%time
from azureml.core.webservice import Webservice
service = Webservice.deploy_from_model(workspace=ws,
                                       name='anomaly-svc',
                                       deployment_config=aciconfig,
                                       models=[model],
                                       image_config=image_config)
service.wait_for_deployment(show_output=True)**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d094.png)
 
Paste the below code in the next cell.

**print(service.scoring_uri)**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d095.png)

 
Paste the below code in the next cell.

**print(service.get_logs())**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d096.png)
 
Paste the below code in the next cell.

**import requests
input_data = "{ \"Id\": 0, \"Timestamp\": \"2018-04-04T22:42:59+00:00\", \"NumberOfPoints\": 400, \"MaxLoad\": 19500, \"MinLoad\": 7500, \"StrokeLength\": 1200, \"StrokePeriod\": 150, \"CardType\": 0,\"CardPoints\": [{\"Load\": 11744,\"Position\": 145 }] }"
headers = {'Content-Type':'application/json'}
resp = requests.post(service.scoring_uri, input_data, headers=headers)
print("POST to url", service.scoring_uri)
print("prediction:", resp.text)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d097.png)

Paste the below code in the next cell. 

**print(service.get_logs())**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d098.png)

### 6.12 Run Deployment Notebook

Run the first two cells to import azureml sdk and to load workspace.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d099.png)
 
Run next 3 cells. For these 3 cells you won’t get any output.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0100.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0101.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0102.png) 
 
Run the next cell, to create image and to deploy web service.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0103.png)
 
Run the next cell to print the scoring uri.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0104.png)
 
http://a157fdbc-21dd-486b-9eb8-3390238b1ac6.westeurope.azurecontainer.io/score

Run the next cell to get the logs.
 
![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0105.png)

You will get the below logs.

2019-06-19T07:27:25,897775737+00:00 - iot-server/run 

2019-06-19T07:27:25,913429346+00:00 - gunicorn/run 

2019-06-19T07:27:25,914979327+00:00 - rsyslog/run 

2019-06-19T07:27:25,915555020+00:00 - nginx/run 

EdgeHubConnectionString and IOTEDGE_IOTHUBHOSTNAME are not set. Exiting...

2019-06-19T07:27:26,113551005+00:00 - iot-server/finish 1 0

2019-06-19T07:27:26,114691791+00:00 - Exit code 1 is normal. Not restarting iot-server.

Starting gunicorn 19.6.0

Listening at: http://127.0.0.1:9090 (14)

Using worker: sync

worker timeout is set to 300

Booting worker with pid: 47

Initializing logger

Starting up app insights client

Starting up request id generator

Starting up app insight hooks

Invoking user's init function

Users's init has completed successfully

/opt/miniconda/lib/python3.6/site-packages/sklearn/base.py:251: UserWarning: Trying to unpickle estimator SVC from version 0.19.1 when 

using version 0.20.0. This might lead to breaking code or invalid results. Use at your own risk.
  
  UserWarning)

Scoring timeout setting is not found. Use default timeout: 3600000 ms

Received input: {}

Headers passed in (total 11):
	
Host: localhost:5001
	
X-Real-Ip: 127.0.0.1
	
X-Forwarded-For: 127.0.0.1
	
X-Forwarded-Proto: http
	
Connection: close
	
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36
	
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
	
Accept-Encoding: gzip, deflate
	
Accept-Language: en-US,en;q=0.9
	
Upgrade-Insecure-Requests: 1
	
X-Ms-Request-Id: 27ae05de-f543-46eb-9795-d5c0185ee9c0

Scoring Timer is set to 3600.0 seconds

Encountered Exception: Traceback (most recent call last):
  
  File "/var/azureml-app/app.py", line 207, in run_scoring
    
   response = invoke_user_with_timer(service_input, request_headers)
  
  File "/var/azureml-app/app.py", line 275, in invoke_user_with_timer
    
   result = user_main.run(**params)
 
 File "/var/azureml-app/main.py", line 47, in run
   
   return_obj = driver_module.run(**arguments)
 
 File "score4dc.py", line 54, in run
   
   prediction = write_msg(input['Id'], input['Timestamp']);

KeyError: 'Id'

During handling of the above exception, another exception occurred:

Traceback (most recent call last):
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1612, in full_dispatch_request
   
   rv = self.dispatch_request()
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1598, in dispatch_request
    
   return self.view_functions[rule.endpoint](**req.view_args)
  
  File "/var/azureml-app/app.py", line 101, in get_prediction_realtime
   
   return run_scoring(service_input, request.headers)
  
  File "/var/azureml-app/app.py", line 219, in run_scoring
    
   raise RunFunctionException(str(exc))

run_function_exception.RunFunctionException

500

127.0.0.1 - - [19/Jun/2019:07:41:44 +0000] "GET /score HTTP/1.0" 500 4 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) 
AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"

127.0.0.1 - - [19/Jun/2019:07:41:44 +0000] "GET /favicon.ico HTTP/1.0" 404 232 "http://a157fdbc-21dd-486b-9eb8-3390238b1ac6.westeurope.azurecontainer.io/score" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /robots.txt HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /nmaplowercheck1560930239 HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "POST / HTTP/1.0" 405 178 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "POST /sdk HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /.git/HEAD HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:00 +0000] "GET /HNAP1 HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:00 +0000] "GET /favicon.ico HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /administrator HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "POST / HTTP/1.0" 405 178 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /admin/ HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /random404page/ HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:02 +0000] "GET /rails/info/properties HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

Run the next cell to test the web service.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0106.png)
 
You will get the below output.

POST to url http://a157fdbc-21dd-486b-9eb8-3390238b1ac6.westeurope.azurecontainer.io/score

prediction: {"Id": "0", "Timestamp": "2018-04-04T22:42:59+00:00", "Anomaly": "False", "Class": "Full Pump"}

Run the next cell to print the logs.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0107.png)
 
You will get below logs.

2019-06-19T07:27:25,897775737+00:00 - iot-server/run 

2019-06-19T07:27:25,913429346+00:00 - gunicorn/run 

2019-06-19T07:27:25,914979327+00:00 - rsyslog/run 

2019-06-19T07:27:25,915555020+00:00 - nginx/run 

EdgeHubConnectionString and IOTEDGE_IOTHUBHOSTNAME are not set. Exiting...

2019-06-19T07:27:26,113551005+00:00 - iot-server/finish 1 0

2019-06-19T07:27:26,114691791+00:00 - Exit code 1 is normal. Not restarting iot-server.

Starting gunicorn 19.6.0

Listening at: http://127.0.0.1:9090 (14)

Using worker: sync

worker timeout is set to 300

Booting worker with pid: 47

Initializing logger

Starting up app insights client

Starting up request id generator

Starting up app insight hooks

Invoking user's init function

Users's init has completed successfully

/opt/miniconda/lib/python3.6/site-packages/sklearn/base.py:251: UserWarning: Trying to unpickle estimator SVC from version 0.19.1 when 
using version 0.20.0. This might lead to breaking code or invalid results. Use at your own risk.
  UserWarning)

Scoring timeout setting is not found. Use default timeout: 3600000 ms

Received input: {}

Headers passed in (total 11):
 
 Host: localhost:5001
 
 X-Real-Ip: 127.0.0.1
 
 X-Forwarded-For: 127.0.0.1
 
 X-Forwarded-Proto: http
 
 Connection: close
 
 User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36
 
 Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
 
 Accept-Encoding: gzip, deflate
 
 Accept-Language: en-US,en;q=0.9
 
 Upgrade-Insecure-Requests: 1
 
 X-Ms-Request-Id: 27ae05de-f543-46eb-9795-d5c0185ee9c0

Scoring Timer is set to 3600.0 seconds

Encountered Exception: Traceback (most recent call last):
  
  File "/var/azureml-app/app.py", line 207, in run_scoring
    
   response = invoke_user_with_timer(service_input, request_headers)
  
  File "/var/azureml-app/app.py", line 275, in invoke_user_with_timer
  
   result = user_main.run(**params)
  
  File "/var/azureml-app/main.py", line 47, in run
   
   return_obj = driver_module.run(**arguments)
  
  File "score4dc.py", line 54, in run
    
   prediction = write_msg(input['Id'], input['Timestamp']);

KeyError: 'Id'

During handling of the above exception, another exception occurred:

Traceback (most recent call last):
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1612, in full_dispatch_request
   
   rv = self.dispatch_request()
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1598, in dispatch_request
   
   return self.view_functions[rule.endpoint](**req.view_args)
  
  File "/var/azureml-app/app.py", line 101, in get_prediction_realtime
  
   return run_scoring(service_input, request.headers)
  
  File "/var/azureml-app/app.py", line 219, in run_scoring
   
   raise RunFunctionException(str(exc))

run_function_exception.RunFunctionException

500

127.0.0.1 - - [19/Jun/2019:07:41:44 +0000] "GET /score HTTP/1.0" 500 4 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"

127.0.0.1 - - [19/Jun/2019:07:41:44 +0000] "GET /favicon.ico HTTP/1.0" 404 232 "http://a157fdbc-21dd-486b-9eb8-3390238b1ac6.westeurope.azurecontainer.io/score" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /robots.txt HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /nmaplowercheck1560930239 HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "POST / HTTP/1.0" 405 178 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "POST /sdk HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:43:59 +0000] "GET /.git/HEAD HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:00 +0000] "GET /HNAP1 HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:00 +0000] "GET /favicon.ico HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /administrator HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "POST / HTTP/1.0" 405 178 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /admin/ HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:01 +0000] "GET /random404page/ HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

127.0.0.1 - - [19/Jun/2019:07:44:02 +0000] "GET /rails/info/properties HTTP/1.0" 404 232 "-" "Mozilla/5.0 (compatible; Nmap Scripting Engine; https://nmap.org/book/nse.html)"

Received input: {}

Headers passed in (total 11):
	
 Host: localhost:5001
	
 X-Real-Ip: 127.0.0.1
	
 X-Forwarded-For: 127.0.0.1
	
 X-Forwarded-Proto: http
	
 Connection: close
	
 User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36
	
 Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
	
 Accept-Encoding: gzip, deflate

 Accept-Language: en-US,en;q=0.9
	
 Upgrade-Insecure-Requests: 1
	
 X-Ms-Request-Id: 70fd2dd4-7700-4fa9-884d-30df36bbfbb4

Scoring Timer is set to 3600.0 seconds

Encountered Exception: Traceback (most recent call last):
  
  File "/var/azureml-app/app.py", line 207, in run_scoring
    
   response = invoke_user_with_timer(service_input, request_headers)
  
  File "/var/azureml-app/app.py", line 275, in invoke_user_with_timer
    
   result = user_main.run(**params)
  
  File "/var/azureml-app/main.py", line 47, in run
    
   return_obj = driver_module.run(**arguments)
  
  File "score4dc.py", line 54, in run
    
   prediction = write_msg(input['Id'], input['Timestamp']);

KeyError: 'Id'

During handling of the above exception, another exception occurred:

Traceback (most recent call last):
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1612, in full_dispatch_request
    
   rv = self.dispatch_request()
  
  File "/opt/miniconda/lib/python3.6/site-packages/flask/app.py", line 1598, in dispatch_request
    
   return self.view_functions[rule.endpoint](**req.view_args)
  
  File "/var/azureml-app/app.py", line 101, in get_prediction_realtime
   
   return run_scoring(service_input, request.headers)
  
  File "/var/azureml-app/app.py", line 219, in run_scoring
    
   raise RunFunctionException(str(exc))

run_function_exception.RunFunctionException

500

127.0.0.1 - - [19/Jun/2019:08:20:26 +0000] "GET /score HTTP/1.0" 500 4 "-" "Mozilla/5.0 (Windows NT 10.0; Win64; x64) 

AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36"

Validation Request Content-Type

Received input: { "Id": 0, "Timestamp": "2018-04-04T22:42:59+00:00", "NumberOfPoints": 400, "MaxLoad": 19500, "MinLoad": 7500, "StrokeLength": 1200, "StrokePeriod": 150, "CardType": 0,"CardPoints": [{"Load": 11744,"Position": 145 }] }

Headers passed in (total 11):
 
 Host: localhost:5001
 
 X-Real-Ip: 127.0.0.1
 
 X-Forwarded-For: 127.0.0.1
 
 X-Forwarded-Proto: http
 
 Connection: close
 
 Content-Length: 218
 
 User-Agent: python-requests/2.21.0
 
 Accept: */*
 
 Accept-Encoding: gzip, deflate
 
 Content-Type: application/json
 
 X-Ms-Request-Id: c82bd0ef-e058-41fb-8386-c852008f8810

Scoring Timer is set to 3600.0 seconds

200

127.0.0.1 - - [19/Jun/2019:08:26:52 +0000] "POST /score HTTP/1.0" 200 95 "-" "python-requests/2.21.0"
Validation Request Content-Type

Received input: { "Id": 0, "Timestamp": "2018-04-04T22:42:59+00:00", "NumberOfPoints": 400, "MaxLoad": 19500, "MinLoad": 7500, "StrokeLength": 1200, "StrokePeriod": 150, "CardType": 0,"CardPoints": [{"Load": 11744,"Position": 145 }] }

Headers passed in (total 11):
 
 Host: localhost:5001
 
 X-Real-Ip: 127.0.0.1
 
 X-Forwarded-For: 127.0.0.1
 
 X-Forwarded-Proto: http
 
 Connection: close
 
 Content-Length: 218
 
 User-Agent: python-requests/2.21.0
 
 Accept: */*
 
 Accept-Encoding: gzip, deflate
 
 Content-Type: application/json
 
 X-Ms-Request-Id: 436df8ad-e509-4529-867b-69677ee381b6

Scoring Timer is set to 3600.0 seconds

200

127.0.0.1 - - [19/Jun/2019:08:27:10 +0000] "POST /score HTTP/1.0" 200 95 "-" "python-requests/2.21.0"
Validation Request Content-Type

Received input: { "Id": 0, "Timestamp": "2018-04-04T22:42:59+00:00", "NumberOfPoints": 400, "MaxLoad": 19500, "MinLoad": 7500, "StrokeLength": 1200, "StrokePeriod": 150, "CardType": 0,"CardPoints": [{"Load": 11744,"Position": 145 }] }

Headers passed in (total 11):
 Host: localhost:5001
 X-Real-Ip: 127.0.0.1
	
 X-Forwarded-For: 127.0.0.1
 
 X-Forwarded-Proto: http
 
 Connection: close
 
 Content-Length: 218
 
 User-Agent: python-requests/2.21.0
 
 Accept: */*
 
 Accept-Encoding: gzip, deflate
 
 Content-Type: application/json
 
 X-Ms-Request-Id: b04d747f-b825-4fe5-8041-1104dd940767

Scoring Timer is set to 3600.0 seconds

200

127.0.0.1 - - [19/Jun/2019:08:29:01 +0000] "POST /score HTTP/1.0" 200 95 "-" "python-requests/2.21.0"
Validation Request Content-Type

Received input: { "Id": 0, "Timestamp": "2018-04-04T22:42:59+00:00", "NumberOfPoints": 400, "MaxLoad": 19500, "MinLoad": 7500, "StrokeLength": 1200, "StrokePeriod": 150, "CardType": 0,"CardPoints": [{"Load": 11744,"Position": 145 }] }

Headers passed in (total 11):
 
 Host: localhost:5001
 
 X-Real-Ip: 127.0.0.1
 
 X-Forwarded-For: 127.0.0.1
 
 X-Forwarded-Proto: http
 
 Connection: close
 
 Content-Length: 218
 
 User-Agent: python-requests/2.21.0
 
 Accept: */*
 
 Accept-Encoding: gzip, deflate
 
 Content-Type: application/json
 
 X-Ms-Request-Id: a48a1f8d-81ef-40d4-9465-fab39d8fbe39

Scoring Timer is set to 3600.0 seconds

200

127.0.0.1 - - [19/Jun/2019:08:29:06 +0000] "POST /score HTTP/1.0" 200 95 "-" "python-requests/2.21.0"

#### 6.12.1 Check the image

To check the image, go to Workspace and Navigate to Image blade.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0108.png)
 
Click on the image to see the details.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0109.png)
 
#### 6.12.2 Check the Deployment

To check the deployment, go to Workspace and Navigate to deployment blade. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0110.png)
 
Click on the deployment to see the details.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/d0111.png)
 
