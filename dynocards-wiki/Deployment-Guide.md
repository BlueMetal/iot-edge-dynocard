![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/oil%26gas-deployment.png)
**Table of Contents** 

- [1 Deployment Guide](#1-deployment-guide)
- [2 What are Paired Regions?](#2-what-are-paired-regions?)
- [3 ARM Template Input Parameters](#3-arm-template-input-parameters)
- [4 Getting Started](#4-getting-started)
    - [4.1 ARM Template Deployment Using Azure Portal](#41-arm-template-deployment-using-azure-portal)
        - [4.1.1 Inputs](#411-inputs)
        - [4.1.2 Outputs](#412-Outputs)
    - [4.2 ARM Template Deployment Using Azure CLI](#42-arm-template-deployment-using-azure-cli)
- [5 Post Deployment Steps](#5-post-deployment-steps)
    - [5.1 Verify Containers in Edge VM and Azure Portal](#51-verify-containers-in-edge-vm-and-azure-portal)
    - [5.2 Update IoT Hub Device Primary Key in Web API Application Settings](#52-update-iot-hub-device-primary-key-in-Web-api-application-settings)
    - [5.3 Perform Device Twin Operation on Edge VM [Optional]](#51-perform-device-twin-operation-on-edge-vm-[optional])
- [6 Machine Learning Configuration](#5-machine-learning-configuration)
    - [6.1 Add Current user to Docker-users group](#61-add-current-user-to-docker-users-group)
    - [6.2 Install ML Workbench](#61-install-ml-workbench)
    - [6.3 Login to the Azure portal](#63-login-to-the-azure-portal)
    - [6.4 List Environment Components](#64-list-environment-components)
    - [6.5 Create ML Project](#65-create-ml-project)
    - [6.6 Download Git Repository](#66-download-git-repository)
    - [6.7 Submit experiment train4dc.py as local target](#67-submit-experiment-train4dc.py-as-local-target)
    - [6.8 Install azureml.datacollector](#68-install-azureml.datacollector)
    - [6.9 Submit experiment score4dc.py with local target](#69-submit-experiment-score4dc.py-with-local-target)
    - [6.10 Set Model Management account](#610-set-model-management-account)
    - [6.11 Deployment Configuration](#611-deployment-configuration)
    - [6.12 Registering providers](#612-registering-providers)
    - [6.13 Run experiment as Docker target](#613-run-experiment-as-docker-target)
    - [6.14 Web Service Deployment](#614-web-service-deployment)
    - [6.15 Testing Web Service](#615-testing-web-service)
	
	

## 1. Deployment Guide
This Document explains about how to deploy Oil & Gas solution using ARM Template. In this Document explained about two ways of deploying solution.
##### -Using Azure portal
##### * Using Azure CLI
This document explains about input parameters, output parameters and points to be noted while deploying ARM Template.

## 2.  What are paired regions?
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



## 3 ARM Template Input Parameters

In the parameters section of the template, specify which values you can input when deploying the resources. These parameter values enable you to customize the deployment by providing values that are tailored for an environment (such as dev, test, and production). You are limited to 255 parameters in a template. You can reduce the number of parameters by using objects that contain multiple properties.

| **Parameter Name**  | **Description**     | **Allowed Values**    | **Default Values**   |                                                                                                            
| -------------       | -------------       | -----------------     | ------------         |
| **Solution Type**   | Choose the solution deployment type from the drop down, for more information about the solution deployment type navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki      | Basic, Standard, Premium | Basic|
| **Edge VM + Simulator VM**   | Choose Yes/No to add Modbus VM as part of Solution deployment, for more information about the Modbus VM navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki   | Yes, No    | No |
| **mlVM**   | Choose Yes/No to add ML VM as part of Solution deployment, for more information about the ML VM navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki    | Yes, No   | No |
| **Data Lake Location**   | Choose location for Data Lake Store, for more information about the Data Lake Store navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki   | Eastus2,Centralus, Northeurope, westeurope  | Eastus2 |
| **Machine learning Location**  | Choose location for machine learning account, for more information about the machine learning location navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki   | Eastus2, Australieneast, Southeastasia, Westcentralus, Westeurope | eastus2 |
| **Geo Paired Primary Region**  | Choose geo-paired region,if you have selected standard (or) premium option in the solution type inputparameter, for more information about the geo-paired-region navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki  | EastUS2,CentralUS,WestUS2,WestCentralUS,CanadaCentral,CanadaEast,         AustraliaEast,AustraliaSouthEast,CentralIndia,SouthIndia,EastAsia,               SouthEastAsia,JapanWest,JapanEast,KoreaCentral,KoreaSouth,UKSouth,               UKWest  | eastus2|
| **oms-region**    | Choose location for OMS Log Analytics, for more information about the oms-region navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki   | australiasoutheast, canadacentral, centralindia, eastus, japaneast, southeastasia, uksouth, westeurope    | EastUs |
| **appInsightsLocation**   | specify the region for application insights, for more information about the appInsightsLocation navigate to https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide in wiki    | eastus, northeurope,  southcentralus, southeastasia, westeurope, westus2 | westus2 |
| **sqlAdministratorLogin**     | Provide the user name for the SQL server, please make a note of Username this will be used further. | sqluser | sqluser |
| **sqlAdministratorLoginPassword**   | Provide the password for the SQL server, make a note of the Password this will be used further.| Password@1234  | Password@1234 |
| **azureLogin** |  Provide the AZURE portal login username. This will be helpful to authenticate data lake store account in stream analytics outputs section. | user@domain.com | user@domain.com |
| **tenantId** | Tenant Id for the subscription. This will be helpful to authenticate data lake store account in stream analytics outputs section     | xxxxxxxx-xxxx-xxxx-xxxx-c5e6exxxcd38          | xxxxxxxx-xxxx-xxxx-xxxx-c5e6exxxcd38 |
| **vmsUsername** | Username to Login into Modbus VM and Edge VM. please make a note of Username this will be used further | adminuser  | Adminuser      |
| **vmsPassword** | Please provide VM login password, please make a note of Password this will be used further | Password@1234    | Password@1234  |

### Note: 
Allowed Values are updated based on the availability of azure resources based on Region w.e.f DT 18th Aug 2018. Microsoft may introduce availability of azure resources in future. More information can be found in https://azure.microsoft.com/en-in/status/ 

## 4 Deployment Guide

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. The template consists of JSON and expressions that you can use to construct values for your deployment. You use the same template to repeatedly deploy your application during every stage of the application lifecycle.
Resource Manager provides a consistent management layer to perform tasks through Azure PowerShell, Azure CLI, Azure portal, REST API, and client SDKs.

*   Deploy, manage, and monitor all the resources for your solution as a group, rather than handling these resources individually.
*   Repeatedly deploy your solution throughout the development lifecycle and have confidence your resources are deployed in a consistent state.
*   Manage your infrastructure through declarative templates rather than scripts.
*   Define the dependencies between resources so they're deployed in the correct order.
*   Apply access control to all services in your resource group because Role-Based Access Control (RBAC) is natively integrated into the management platform.
*   Apply tags to resources to logically organize all the resources in your subscription.

### 4.1  ARM Template Deployment using Azure Portal

1.  Click the below **Git hub** repo URL.

**https://github.com/sysgain/iot-edge-dynocard/tree/master**

2. Select **main-template.json** from **master** branch as shown in the following figure.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/5.png)

3. Select **Raw** from the top right corner.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/6.png)

4. **Copy** the raw template and **paste** in your azure portal for template deployment.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/7.png)

To deploy a template for Azure Resource Manager, follow the below steps.

1.  Go to **Azure portal**.

2.  Navigate to **Create a resource (+)**, search for **Template deployment**.

3.  Click Create button and click **Build your own Template in the editor** as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/8.png)

4.  The **Edit template** page is displayed as shown in the following figure. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/9.png)

5.  **Replace / paste** the template and click **Save** button.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/10.png)

6.  The **Custom deployment** page is displayed as shown in the following.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/11.png)

### 4.1.1. Inputs
These parameter values enable you to customize the deployment by providing values. There parameters allow to choose the solution type, region and credentials to authenticate SQL Database and Virtual Machines.

7.  If you choose **yes** for **Edge VM + Simulator VM** then **then Edge VM and Simulator VMs** will be **deployed** with in the solution.

8.  If you choose **No** then the **Edge VM + Simulator VM** vm  will be **not deployed** with in the solution, If you choose No, then the Edge VM and Simulator VM will be not deployed with in the solution, we have to manually deploy IoT Edge modules in Edge VM for more information refer the section 5.3 Perform Device Twin Operation on Edge VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/12.png)

9.  If you choose **yes** for **ML VM** then Pre-installed **docker** will be **deployed** with in the solution.

10. If you choose **No** for **ML VM** then Pre-installed **docker** will **not be deployed** with in the solution.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/13.png)

**Parameters for Basic Solution**

11. Deploy the template by providing the parameters in custom deployment settings as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/14.png)

**Parameters for Standard Solution**

12. If you want to deploy the core with monitoring you must enter the below parameters

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/15.png)

**Parameters for Premium Solution**

13. If you want to deploy the core with Hardening and Monitoring you must enter the below parameters.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/16.png)

14. Once all the parameters are entered, select the **terms and conditions** check box and click **Purchase**.

15. After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

16. When you choose Costing model as **Standard** and the **Modbus VM** and **ML VM** as **Yes**, then the below components will be installed.

* 3-Virtual machines (2 windows & 1 Linux)
Windows VMS
**Dyno card VM** which install pre-installed software's for dyno card VM.
**ML VM** which install pre-installed docker.
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

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/17.png)
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/18.png)
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/19.png)
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/20.png)

### 4.1.2 Outputs
The Outputs section consists of the values that are returned from deployment. The output values can be used for further steps in Solution Configuration
1. Go to **Resource group** -> click **deployments** -> select **Microsoft Template**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/21.png)

2. Click **outputs**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/22.png)

### 4.2. ARM Template Deployment Using Azure CLI

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
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/23.png)

8. Paste your **main-template.json** in editor as shown below and save the file. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/24.png) 

9. Paste your **main-template.parameters.json** in editor. 
10. Update the following parameters in main-template.json file 
* Solution Type 
*   Edge VM + Simulator VM
*   MLVM
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
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/25.png)

11. Create Resource Group for oilandgas Solution 
12. Use the **az group create** command to create a **Resource Group** in your region.
 
**Description:** To create a resource group, use az group create command, 
It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l). 

**Syntax: az group create -n <resource group name> -l <location>**.

**Ex: az group create -n <****> -l <***>**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/26.png)
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/27.png)

Execute the Template Deployment 
Use the **az group deployment create** command to deploy the ARM template 

**Description:** To deploy the ARM Template, you require two files: 

**main-template.json** – contains the resource & its dependency resources to be provisioned from the ARM template 

**main-template.parameters.json** –contains the input values that are required to provision respective SKU & Others details. 

**Syntax:  az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in previous section> -n deploy >> <provide the outputs filename>**

**Ex: az group deployment create --template-file './main-template.json' --parameters '@./main-template.parameters.json' -g oilandgas-iot -n deploy >> outputs.txt**
 
 ![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/28.png)

15. Deployment may take between 15-20 minutes depending on deployment size. 
16. After successful deployment you can see the deployment outputs as follows.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/29.png)

## 5. Post Deployment steps

### 5.1. Verify Containers in EdgeVM and Azure Portal

1. Go to **Resource Group** ->click on **iotEdge** VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/30.png)

2. Copy the Public IP Address of the iotEdge VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/31.png)

3. Login to the VM through putty.

4. Paste the public ip at Host Name (or IP address) and click on open.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/32.png)

5. Enter credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

6. Once login successful the below welcome message is displayed.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/33.png)

7. Here you can check the device and device modules in IoT Edge VM.

**docker ps** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/34.png)

8. Go to resource group -> click on **iothub26hs3**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/35.png)

9. Navigate to **IoT Edge** blade under **Automatic Device Management** section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/36.png)

10. Here we created and configured device from the IoT Edge VM. Click on **iot-dynocard-demo-device_1** device you can see the modules.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/37.png)

11. We can see the created modules in device details 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/38.png)

### 5.2. Update IoT Hub Device Primary key in Web API Application Settings

1. Go to **Resource Group** -> Click on **iothub26hs3**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/46.png)

2. Navigate to **IoT Edge** blade under **Automatic Device Management** section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/47.png)

3. Click on iot-dynocard-demo-device_1 device as shown below and copy the connection string-primary key.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/48.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/49.png)

4. Go to **resource group** -> open the primary web app **webapi26hs3** in the app service.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/50.png)

5. Navigate to **Application settings** blade.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/51.png)

6. Click **+ Add new setting** in the Application settings, enter name and value in the new setting.

Name: **DeviceConnectionString**

Value: **[Device connection string-primary key]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/52.png)

7. Then click **Save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/53.png)

### 5.3. Perform Device Twin operation on Edge VM [Optional]

**Note**: This step is required only when user wants to run simulator on their own system. If Modbus VM is Deployed as part of the solution, then skip this step and continue with next step.

Need to perform device twin operation on Modbus IoT Edge Module when solution deployed with an option Modbus VM as **No**. when Modbus VM deployment option is chosen as **no** then there is no Simulator VM is deployed.

Install IoTedge Modules by executing **iot-edge-Manual.sh** Under scripts from GitHub repo script by providing Required input parameters.
https://github.com/sysgain/iot-edge-dynocard/blob/master/scripts/iot-edge-Manual.sh

Needs to update slave connection IP address in Modbus module configuration in IoT Edge Modules with actual Simulator IP address.


1. Go to **Resource Group** ->click on **iotEdge VM**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/54.png)

2. **Copy** the **Public IP Address** of the **IoTEdge** VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/55.png)

3. Login to the VM through putty.

4. Paste the public IP at Host Name (or IP address) and click on open.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/56.png)

5. Enter credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

6. Once login successful the below welcome message is displayed.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/57.png)

7. Here you can check the device and device modules in IoT Edge VM.

**docker ps**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/58.png)

8. Check the logs of Modbus container by executing below command.

**docker logs modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/59.png)

As per above diagram slave connection is 52.186.11.164 and it’s trying to connect with 52.186.11.164 which is not available. We need to update slave connection IP address with correct IP Address using twin operation.

10. Go to the **resource group** and choose **IoT Hub**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/60.png)

11. Click on **IoT Hub** and navigate to **IoT Edge** blade under **Automatic device management** section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/61.png)

12. Click on IoT Device **iot-dynocard-demo-device_1** device to check modules.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/62.png)

13. We can see the created **modules** in **device details**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/63.png)

14. Click on **modbus** module.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/64.png)

15. Click on **Module twin**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/65.png)

16. Change IP Address of slave Connection and click on **Save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/66.png)

17. In this scenario IP address changed to 104.42.153.165

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/67.png)

18. Now go back to Edge VM to verify slave connection IP Address.

19. Stop the Modbus container by passing below command.

**docker stop modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/68.png)

20. Start the Modbus container by passing below command.

**docker start modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/69.png)

21. Now verify slave connection string by checking logs of Modbus.

**docker logs modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/70.png)

Now slave connection IP address is updated with New IP Address.

**6.  Machine Learning Configuration**

Azure Machine Learning is an integrated, end-to-end data science and advanced analytics solution. It enables data scientists to prepare data, develop experiments and deploy models at cloud scale. The document goes over the following topics.

    1 - AML Experimentation Account
    2 - AML Model Management Account
    3 - AML Workbench
    4 - Configuring the various ML objects.
    5 - Training and deploying a sample model.

**Experimentation service** allows data scientists to execute their experiments locally, in Docker containers, or in Spark clusters through simple configuration. It manages run history, provides version control, and enables sharing and collaboration.

**Azure Machine Learning Workbench** is a front-end for a variety of tools and services, including the Azure Machine Learning Experimentation and Model Management services.

Workbench is a relatively open toolkit. First, you can use almost any Python-based machine learning framework, such as Tensor flow or scikit-learn. Second, you can train and deploy your models either on-premises or on Azure. Workbench also includes a great data-preparation module. It has a drag-and-drop interface that makes it easy to use, but its features are surprisingly sophisticated.

## 6.1. Add Current user to Docker-users group

1. Go to **AzuremlVM** from the **Resource Group** and copy **Public IP Address** of the VM.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/154.png)

2. Open **Remote desktop connection** and place IP Address and click on **Connect.**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/155.png)

3. Provide user name and password and click on OK.
User Name   : adminuser
Password    : Password@1234
**Note:** Credentials might vary depends on deployment.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/156.png)

4. Open **PowerShell as Administrator** and execute below command to add current user to docker-users group.
**Add-LocalGroupMember -Member $env:username -Name docker-users**
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/157.png) 

5. Sign out from windows and sign in again.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/158.png)

6. Search for docker for windows in search menu and click on **“Docker for Windows.”**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/159.png)

7. When it prompted to enable **Hyper-V and Containers feature**, click **ok**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/160.png)

8. This will restart Windows System. Login again to VM using the same credentials as above.

9. When Docker is running, a pop will be opened in task bar which states docker is running.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/161.png)

**6.2. Install ML Workbench**
1. Install ML work bench using the setup file from below path.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/162.png) 

2. Double click on **amlWorkbenchSetup.msi** file.
3. Click on **Continue**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/163.png)

4. Click on **Install**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/164.png)

5. It will take around 30mins to 45mins to install ML Workbench.
6. Once installation completed successfully, click on Launch **Azure Machine Learning Workbench**.
7. Click on **Sign in with Microsoft.** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/165.png)

8. Provide azure portal login credentials. Once login is completed successfully. The below page will be displayed. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/166.png)

9. Click on File > Open Command Prompt.

## 6.3. Login to the Azure portal

10. Login to the portal using below command.
**az login -t <tenant ID>**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/167.png)

11. Open edge browser and go to the page **https://microsoft.com/devicelogin** and provide code and click on Continue.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/168.png)

12. Provide azure portal login credentials, once authentication is successful, go back to command prompt.
13. List available azure subscriptions using below command.

**az account list -o table**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/169.png)

14. Set current subscription as default using below command.

**az account set -s <enter your sub id here>**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/170.png)

15. Verify default subscription using below command

**az account show**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/171.png)

## 6.4. List Environment Components

16. Location of anaconda env

**conda env list**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/172.png)
 
17. List python packages

**pip freeze**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/173.png)

18. Versions of cli components

**az -v**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/174.png) 

19. All cli cmd for machine learning

**az ml -h**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/175.png) 

## 6.5. Create ML Project

1. Create ML Project using below command.

**az ml project create --name mlproject --workspace mlworkspacerk23l --account oilgasexpaccrk23l --resource-group oilandgas-coresolution-267 --path c:\mlproject**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/176.png)

2. Once project is created, refresh home page in GUI, where we can see project name under workspace. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/177.png)

## 6.6. Download Git Repository

1. Download repository from below URL and extract to specific path.

**https://github.com/BlueMetal/iot-edge-dynocard**

2. Click on **“Download Zip”**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/178.png)

3. Extract the zip downloaded project file into ml project sub directory. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/179.png) 

4. Change directory

**cd C:\mlproject\iot-edge-dynocard-master\code\containers\edge_ml** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/180.png)

## 6.7. Submit experiment train4dc.py as local target

5. Run experiment locally using below command

**az ml experiment submit -c local train4dc.py**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/181.png)

6. Now go back to GUI. Click on **run history** on left hand pane and it will page as below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/182.png)

7. Click on **Run** and a page will be opened as below. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/183.png)

8. Select **model4dc.pkl** and click on **Download.** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/184.png)

9. Choose path to save the file. [This path will be working directory of ML Workspace, Ex: C:\iot-edge-dynocard-master\code\containers\edge_ml]. click on **save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/185.png) 

10. Click on **Yes** when it’s prompted for Confirm Save As warning.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/186.png)

## 6.8. Install azureml.datacollector

11. Install azureml.datacollector using below command.

**pip install azureml.datacollector**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/187.png)

## 6.9. Submit experiment score4dc.py with local target

12. Submit experiment with docker local as target using below command.

**az ml experiment submit -c local score4dc.py**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/188.png)
 
13. Go to GUI and check **Run History** page.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/189.png)

14. Click on **recently completed run job**. A page will be opened as below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/190.png)

15. Select **Service_schema.json** file and click on **Download**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/191.png) 

16. Download the file to current working directory as earlier step.

## 6.10.set Model Management account

Azure machine learning uses two accounts. The first account keeps track of the experiments and the second account keeps track of the models. The Azure CLI commands below can be used to validate your accounts and set your model management account to the project directory.

**Syntax:  az ml account modelmanagement set -n <modelmanagementaccountname> -g <resourcegroupname>**

**Ex: az ml account modelmanagement set -n mmact4pumps -g rg4pumps**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/192.png)
 
## 6.11.   Deployment Configuration

17. Create dev environment using below command.

**Syntax: az ml env setup -l eastus2 -n <Uniquename> -g <resourcegroup name> --debug --verbose**

**az ml env setup -l eastus2 -n mldevcofigex -g mldevconfigrg --debug --verbose**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/193.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/194.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/195.png)
 
 
18. We can verify the deployment progress using below command. Wait until provisioning state shows as succeeded.

**az ml env show -g mldevconfigrg -n mldevcofigex**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/196.png)

19. Set as default using below command.

**az ml env set -g mldevconfigrg -n mldevcofigex**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/197.png)
 
20. Show default environment using below command.

**az ml env show**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/198.png)

21. The above deployment deploys below resources in azure. We can verify deployed resources in azure portal.
* 1- Azure Container Registry
* 2 – Storage Accounts
* 1- Application Insights

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/199.png)

## 6.12.Registering providers

22. Register providers in azure portal using below commands.

**az provider register -n Microsoft.MachineLearningCompute**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/200.png)

**az provider register -n Microsoft.ContainerRegistry**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/201.png)

**az provider register -n Microsoft.ContainerService**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/202.png)

## 6.13.   Run experiment as Docker target

23. Run below command to submit experiment with docker target

**az ml experiment submit -c docker train4dc.py**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/203.png)

When the process is running open new command prompt from ML GUI > File > Open Command Prompt. This process will take around 15mins of time.

24. **Run docker ps –a** to verify running docker containers.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/204.png)

25. Once process completed, the output will be as below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/205.png)

26. Go to GUI run history and check recently create Run Number.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/206.png)

## 6.14. Web Service Deployment

27. Deploy web service using below command.

**az ml service create realtime -m model4dc.pkl -f score4dc.py -r python –n websvc4dc**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/207.png)

28. Verify docker container using below command.

**docker ps -a**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/208.png)

## 6.15.   Testing Web Service

29. Run below command to test web service using sample data.

**az ml service run realtime -i websvc4dc -d "{ \"Id\": 0, \"Timestamp\": \"2018-04-04T22:42:59+00:00\", \"NumberOfPoints\": 400, \"MaxLoad\": 19500, \"MinLoad\": 7500, \"StrokeLength\": 1200, \"StrokePeriod\": 150, \"CardType\": 0,\"CardPoints\": [{\"Load\": 11744,\"Position\": 145 }] }"**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/209.png)

30. Check the logs by running the below command

**Syntax:  az ml service logs realtime –i websvc4dc**

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/210.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/211.png)
