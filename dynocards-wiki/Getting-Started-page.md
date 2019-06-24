# Microsoft

# Oil & Gas

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/oil%26gas-gettingstarted.png)

**Table of Contents** 

- [1.0 Introduction](#10-introduction)
    - [1.1 Overview](#11-overview)
    - [1.2 IoT Solution Summary](#12-iot-solution-summary)
        - [1.2.1 Highlights](#121-highlights)
        - [1.2.2 About the Solution](#122-about-the-solution)
- [2.0 IoT Solution Automation and Hardening](#20-iot-solution-automation-and-hardening)
    - [2.1 Existing Solution](#21-existing-solution)
        - [2.1.1 Core Architecture (Current)](#211-core-architecture-(current))
    - [2.2 Automated Solution](#22-automated-solution)
    - [2.3 Architectures](#23-architectures)
        - [2.3.1 Basic Architecture](#231-basic-architecture)
        - [2.3.2 Standard Architecture](#232-standard-architecture)
        - [2.3.3 Premium Architecture](#233-premium-architecture)
    - [2.4 Conventional Data WorkFlow](#24-conventional-data-workflow)
    - [2.5 Azure Components and their Functionality](#25-azure-components-and-their-functionality)
        - [2.5.1 Simulator](#251-simulator)
        - [2.5.2 IoT Edge](#252-iot-edge)
        - [2.5.3 IoT Hub](#253-iot-hub)
        - [2.5.4 Stream Analytics Job](#254-stream-analytics-job)
        - [2.5.5 Azure Data Lake Store](#255-azure-data-lake-store)
        - [2.5.6 Service Bus Name Space Queue](#256-service-bus-name-space-queue)
        - [2.5.7 Logic App](#257-logic-app)
        - [2.5.8 Web App](#258-web-app)
        - [2.5.9 Azure SQL Database](#257-azure-sql-database)
        - [2.5.10 Power BI](#2510-power-bi)
        - [2.5.11 Azure Container Registry](#2511-azure-container-registry)
        - [2.5.12 Machine Learning Service workspace](#2512-machine-learning-studio)
        - [2.5.13 Application Insights](#2512-application-insights)
        - [2.5.14 OMS Log Analytics](#2514-oms-log-analytics)
        - [2.5.15 Function App](#2515-function-app)
- [3.0 Solution Types and Deployment Costs](#30-solution-types-and-deployment-costs)
    - [3.1 Solutions and Associated Costs](#31-solutions-and-associated-costs)
        - [3.1.1 Basic](#311-basic)
        - [3.1.2 Standard](#312-standard)
        - [3.1.3 Premium](#313-premium)
    - [3.2 Solution Features and Cost Comparison](#32-solution-features-and-cost-comparison)
        - [3.2.1 In terms of features](#321-in-terms-of-features)
        - [3.2.2 Solution Cost Impact](#322-solution-cost-impact)
        - [3.2.3 Estimated Monthly Cost for each Solution](#323-estimated-monthly-cost-for-each-solution)
- [4.0 Further References](#40-further-references)
    - [4.1 Deployment Guide](#41-deployment-guide)
    - [4.2 Admin Guide](#42-admin-guide)
    - [4.3 User Guide](#43-user-guide)


## 1.0 Introduction

### 1.1 Overview

Oil and Gas Companies wants to operate the sucker pumps in an Efficient, Safe, Eco – Friendly & responsible manner. Companies are using dynamometers (dyno) surveys to determine the condition of the pump operating under ground or downhole. Medium to large oil companies have thousands of these pumps scattered remotely across the world. Monitoring the dynamometers with the current setup is ideally very expensive, Time Consuming & ineffective solution.  To have Efficient & Effective solution to overcome the problem, we have come up with the Automated Solution which is Reliable, Cost Effective, Scalable & useful across the industry.

### 1.2 IoT Solution Summary

#### 1.2.1 Highlights:

The Rationale behind this IoT Solution specifically designed for the Oil & Gas industry is to: 

1. **Detect issues with the pump using data points captured from the Dyno Card**

2. **Capturing historical data**  

3. **Immediately Turn off the pump** 

4. **Trigger Alert field technicians**

#### 1.2.2 About the Solution

Oil and Gas companies can remotely monitor the condition of the sucker pumps.  
 
*	**Azure IoT Edge** technologies are applied to detect the issues at the edge, to send both immediate and historical dyno card messages to the cloud.  These messages can be used to issue alerts. We can visualize both the surface and pump card data.

*	This Solution is beneficial to inspect thousands of sucker pumps throughout remote areas of the world by issuing alerts. Based on the alerts that are received to the cloud, the field technicians can repair the appropriate sucker pumps.

## 2.0 IoT Solution Automation and Hardening

### 2.1 Existing Solution

Existing solution process flow is explained in following steps:  

* Azure IoT Edge to retrieve Dynocard data using both Modbus and OPC server interfaces. 

* Azure SQL Database to save Dynocard data.

* Azure ML to detect issues with Sucker Pumps.

* Azure IoT Edge to send messages to IoT Hub.

* Stream Analytics Jobs to save data to an Azure Data Lake.

* Logic App to save data through a Web API into a database.

* Power BI to visualize the Dynocard data.


#### 2.1.1 Core Architecture (Current) 

The diagram below explains the Core Architecture for Oil & Gas Companies. 

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/core.png)

Core Architecture components: 

* Virtual Machine 

* Function App 

* IoT Hub 

* Logic App 

* Service Bus 

* Stream Analytics Job 

* Azure data storage 

* Web API 

* SQL Database 

* Power BI 

* Machine learning 

* Container registry 

**Note**: Please refer **section 2.5** for more details about the components.

### 2.2 Automated Solution

The Automated IoT Solution is designed on the top of existing core solution architecture. In addition, this solution also provides Monitoring, High availability and Security.

This solution is deployed through an ARM template. This is a single click deployment which reduces manual effort when compared with the exiting solution.. 

In additionally, this solution adds the following features:

In addition, this solution consists 

*	Application Insights to provide monitoring for Web APIs. Application Insights store the logs of the Web API which will be helpful to trace the Web API working.

*	Log analytics to provide monitoring for the Logic App, IoT Hub, Azure Data Lake store, Service Bus Namespace, SQL Database. Log analytics stores the logs, which will be helpful to trace the working of resources.

*	Geo-replication to provide high availability for mission critical resources like SQL Server database. Geo-replication is used to set the recovery database as the primary database whenever primary database failed.

*	Security steps for the Data Lake store with encryption and Firewall options. We can restrict users to access Data Lake Store from unwanted IP ranges.

*	Securing steps  for  IoT Hub with Shared Access policies and IP Filter options.

*	Securing steps for Service Bus Namespace with Shared Access policies options.

*	Securing steps for SQL Database with Enable Firewall, SQL Database Authentication, Advanced Threat Protection, Auditing and Transparent Data Encryption options. We can restrict access from unwanted networks.

*	This solution also provides Disaster Recovery activities  IoT Hub manual failover is helpful to make the IoT Hub available in another region, when IoT Hub of one region is failed. 

### 2.3 Architectures

#### 2.3.1 Basic Architecture:

The Basic solution will have all the core components, in addition, this solution also consists of some of the monitoring components like Application Insights and OMS Log Analytics. 

*	Application Insights provides monitoring for Web APIs.

*	OMS Log Analytics provide monitoring for the Logic App, IoT Hub, Azure Data Lake Store, Service Bus Namespace and SQL database.

The below is the Basic Architecture Diagram:

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/Basic.png)

Basic Architecture comprises of following components: 

* 3-Virtual machines  

* 1-Web App 

* 1-Function App 

* 2-Application Insights 

* 1-Data Lake Storage 

* 1-IoT HUB 

* 1-Log analytics 

* 1-Logic app 

* 1-service bus namespace

* 1-SQL database 

* 2-Storage accounts 

* 1-Stream Analytics job 

* 1-Machine Learning Service workspace

* 1 – Container registry  

* 1 – key vault  

**Note**: Please refer **section 2.5** for more details about the components.

#### 2.3.2 Standard Architecture:

The Standard solution will have all of the components from the Base solution and in addition, the Standard Architecture will provide High Availability capability by deploying the solution to two regions. The failover is a manual process (not a fully automated solution) and the two deployments are:

1.	Primary Region (Deployment)

2.	Secondary Region (via Re – deployment)

The main use of this solution is for disaster recovery scenarios. When a region goes down,  the re-deployment components will deploy in another region and the redeployed solution needs to be configured to ensure proper functioning. (the VM’s have to be updated with redeployment components).

The diagram below depicts the dataflow between Azure Components in Standard Solution architecture:

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/Standard.png)

Standard Architecture comprises of following components: 

*	3-Virtual machines (2 windows & 1 Linux). One of the Windows VM is the **Dynocard VM** which has pre-installed PLC Simulator v1 and Modbus Simulator software’s. The second Windows VM is the **Data Science Machine Learning VM** which is used  to run the notebooks to create experiments, register model, create image and to deploy webservices in ML service workspace. The Linux VM is the **Edge VM** and is used to create and install modules in IoT Edge device. **VMs are optional Components**.

*	1-Web App

*	1-Function App

*	2-Application Insights

*	1-Data Lake Storage

*	1-IoT HUB

*	1-Log analytics

*	1-Logic app

*	1-service bus namespace

*	2-SQL database

*	2-Storage accounts

*	1-Stream Analytics job

*	1-Machine Learning service workspace

*	1 – Container registry

*	1 – key vault

**Note**: Please refer **section 2.5** for more details about the components.

When there is an Azure Region failover, user needs to redeploy the ARM Template provided in GIT Repo to a Secondary Region. When redeployment is completed successfully, below azure resources will be deployed. 

**Note**:  Re-deployment process will take around 30mins to complete deploying successfully.

* 1-Web App 

* 1-Data Lake Storage 

* 1-Logic app 

* 1-service bus namespace 

* 1-Stream Analytics job 

**Note**: Please refer **section 2.5** for more details about the components.

#### 2.3.3 Premium Architecture

The Premium solution will have all of the components from the Standard solution and in addition, the Premium Architecture will be deploying the solution to two regions right when the solution is first deployed. The failover is an automatic process (a partially automated solution) and the two deployments are:

1.	Primary Region (fully deployed)

2.	Secondary Region (fully deployed when the Primary Region is deployed)

Both the regions have the same set of components and are running at the same time. 

In Primary Region the configuration and set up of the VM’s was done automatically by default but in the Secondary Region, they need to be configured  manually.

The below is the Architecture diagram of the Premium solution:

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/Premium.png)

Premium Architecture comprises of following components: 

* 3-Virtual machines (2 windows & 1 Linux). One of the Windows VM is the **Dynocard VM** which has  pre-installed PLC Simulator and Modbus Simulator software’s. The second Windows VM is the **Data Science Machine Learning VM** which is used  to run the notebooks to create experiments, register model, create image and to deploy webservices in ML service workspace. The Linux VM is the **Edge** VM and is used to create and  install modules for the IoT Edge devices. **VMs are optional Components**.

* 2-Web App 

* 1-Function App 

* 2-Application Insights 

* 2-Data Lake Storage 

* 1-IoT HUB 

* 1-Log analytics 

* 2-Logic app 

* 2-service bus namespace 

* 2-SQL database 

* 2-Storage accounts 

* 2-Stream Analytics job 

* 1-Machine Learning service workspace 

* 1 – container registry 

* 1 – key vault 

**Note**: Please refer **section 2.5** for more details about the components.

In this solution, all required components will be deployed at the initial deployment stage itself. This type of solution reduces the overall downtime of if and when a region goes down. In this solution there is NO redeployment approach which reduces the downtime of the solution.

### 2.4 Conventional Data WorkFlow 

The data flow is similar across all of the solutions offered. 

The below diagram explains the data flow between Azure components within the solution.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/4.png)

### 2.5 Azure Components and their Functionality 

The sections below describes each of the solution components in more details.

#### 2.5.1 Simulator 

**Introduction**:

PLC simulator is an open source and free software package. It has a diagnostics screen that shows the traffic for the Modbus commands and responses. Modrssim provided for handling all the Modbus device ID's from 0 to 255.

Modbus simulator also support scripting.

**Implementation**:

Modbus simulator generates the data as per the VB script provided in Modbus simulator. The simulator then sends the data to IoT Edge Modules.

Simulator is an Optional Component, which lets users choose whether they  want to deploy Simulator device in a Cloud Based environment or deploy them on-premises. When deployed in the cloud, as a Simulator in a Virtual Machine, the ARM Template will install all required software. If the Simulator is on premise, then the user might have to install the required software manually.

#### 2.5.2 IoT Edge 

**Introduction:** 

Azure IoT Edge moves cloud analytics and custom business logic to devices so that your organization can focus on business insights instead of data management. Enable your solution to truly scale by configuring your IoT software, deploying it to devices via standard containers, and monitoring it all from the cloud.

Analytics drives business value in IoT solutions, but not all analytics needs to be in the cloud. If you want a device to respond to emergencies as quickly as possible, you can perform anomaly detection on the device itself. Similarly, if you want to reduce bandwidth costs and avoid transferring terabytes of raw data, you can perform data cleaning and aggregation locally. Then send just the insights to the cloud.

Azure IoT Edge is made up of three components:

*	IoT Edge modules: These are containers that run Azure services, 3rd party services, or your own code. They are deployed to IoT Edge devices and execute locally on those devices.

*	The IoT Edge runtime: This runs on each IoT Edge device and manages the modules deployed to each device.

*	The Interface: A cloud-based interface enables you to remotely monitor and manage IoT Edge devices.

**Implementation**:

In the solution implementation, the IoT Edge device gathers data from the Simulator and processes the data into the IoTHub service. Below modules will be installed on the IoT Edge System.

* sql 

* modbus 

* dynocardalertModule 

* Mlalertmodule

* edgeAgent 

* edgeHub 

* dynoCardWebApi

* dynocard_telemetry

* dynocard_web

In this solution, the IoT Edge VM is an Optional Component, which lets users  choose whether they want to deploy the IoT Edge device in a Cloud Based environment or  deploy it on premise. When deployed  in a Virtual Machine, ARM Template will configure all the required parameters. If deployed on premise, it might require running some additional manual configurations.

Here is a description of the 9 modules

* modbus                - the modbus protocol adapter

* dynoCardAlertModule   - the custom logic in C# to handle parsing all the messages and sending the alerts to the IoT Hub

* mlAlertModule         - the machine learning module that detects the anomalies

* dynocard_telemetry    - custom C# code that generates the device telemetry without the need for an actual device

* sql                   - the SQL Server DB

* dynoCardWebAPI        - a web app running on the edge that accesses the data for the local web UI

* dynocard_web          - the locally running web application that displays the dynocard graph

* edgeAgent & edgeHub   - system provided modules that manage the edge runtime.


#### 2.5.3 IoT Hub 

**Introduction:** 

Azure IoT Hub is a fully managed service that enables reliable and secure bi-directional communications between millions of IoT devices and an application back end. 

Azure IoT Hub offers reliable device-to-cloud and cloud-to-device hyper-scale messaging, enables secure communications using per-device security credentials and access control, and includes device libraries for the most popular languages and platforms. Before you can communicate with IoT Hub from a gateway you must create an IoT Hub instance in your Azure subscription and then provision your device in your IoT Hub. 

The Azure IoT Hub offers several services for connecting IoT devices with Azure services, processing incoming messages or sending messages to the devices. From a device perspective, the functionalities of the Azure IoT Hub enable simple and safe connection of IoT devices with Azure services by facilitating bidirectional communication between the devices and the Azure IoT Hub.

**Implementation:** 

IoT Hub is the core component of the Dynocard Solution. IoT Edge VM will send the data to the IoT Hub using the Edge Modules. These are the modules present in the IoT Edge device.

* sql 

* modbus 

* dynocardalertModule 

* Mlalertmodule 

* edgeAgent 

* edgeHub 

* dynoCardWebApi 

* dynocard_telemetry

* dynocard_web 

#### 2.5.4 Stream Analytics Job 

**Introduction:** 

Stream Analytics is an event processing engine that can ingest events in real-time, whether from one data stream or multiple streams. Events can come from sensors, applications, devices, operational systems, websites, and a variety of other sources. Just about anything that can generate event data is fair game.

Stream Analytics provides high-throughput, low-latency processing, while supporting real-time stream computation operations. With a Stream Analytics solution, organizations can gain immediate insights into real-time data as well as detect anomalies in the data, set up alerts to be triggered under specific conditions, and make the data available to other applications and services for presentation or further analysis. Stream Analytics can also incorporate historical or reference data into the real-time streams to further enrich the information and derive better analytics.

To implement a streaming pipeline, developers create one or more jobs that define a stream’s inputs and outputs. The jobs also incorporate SQL-like queries that determine how the data should be transformed. In addition, developers can adjust a number of a job’s settings. For example, they can control when the job should start producing result output, how to handle events that do not arrive sequentially, and what to do when a partition lags other or does not contain data. Once a job is implemented, administrators can view the job’s status via the Azure portal.

**Implementation:** 

In this solution, the Stream Analytics engine gets data inputted from IoT Hub and outputs data to an Azure Data Lake store. Stream analytics processes all incoming data into the Data Lake Store and hence should always be running. Data Lake Store authorization must be renewed if Stream Analytics service failed for whatever reason.

#### 2.5.5 Azure Data Lake Store 

**Introduction:** 

The Azure Data Lake Store is an enterprise-wide hyper-scale repository for managing big data analytic workloads. Azure Data Lake can capture data of any size, type, and ingestion speed in one single place for operations and analytics. Azure Data Lake Store can be accessed from Hadoop Distributed File System (HDFS) and Microsoft’s HDInsight cluster using the Compatible REST APIs. It is specifically designed to enable analytics on the stored data and is tuned for performance for data analytics scenarios. It has all the enterprise-grade capabilities like scalability, security, reliability, manageability, and availability—essential for real-world enterprise use cases.

Azure Data Lake store can handle any data in their native format, as is, without requiring prior transformations. Data Lake store does not require a schema to be defined before the data is uploaded, leaving it up to the individual analytic framework to interpret the data and define a schema at the time of the analysis. Being able to store files of arbitrary size and formats makes it possible for Data Lake store to handle structured, semi-structured, and even unstructured data.

**Implementation:** 

Azure Data Lake Store stores the data in JSON files format. Stream analytics collects the data from IoT Hub and sends output to Azure Data Lake Store. 

#### 2.5.6 Service Bus Name Space Queue 

**Introduction:** 

Azure Service Bus is a multi-tenant Cloud Service, which means that the Service is shared by the multiple users. Each user creates a namespace and defines the communication mechanism requirements within that namespace.

The purpose of this Service is to make the communication easier. When two or more business partners want to exchange the information, they need a communication mechanism. Service Bus is a brokered or third-party communication mechanism. 

If geo recovery needs to be enabled, then following points needs to Consider.

1.	In your failover planning, you should also consider the time factor. For example, if you lose connectivity for longer than 15 to 20 minutes, you might decide to initiate the failover.

2.	The fact that no data is replicated means that currently active sessions are not replicated. Additionally, duplicate detection and scheduled messages may not work. New sessions, new scheduled messages and new duplicates will work.

3.	Synchronizing entities can take some time, approximately 50-100 entities per minute. 

**Implementation:** 

The data generated from IoT Device (Simulator) will be sent to IoT Hub. Using IoT Hub endpoint configuration, data received by the IoT Hub will be redirected to the Service bus name space. All the messages received by the IoT Hub will be routed to the Service Bus name space queue.

#### 2.5.7 Logic App 

**Introduction:** 

Logic Apps are a relatively new feature of Microsoft Azure that makes it simple to build complex workflows using one or more of the over 200 plus different connectors. Since the Logic Apps are server less, you do not need to worry about server sizing. The platform will scale to meet your demand, and better yet.

*	Logic Apps brings speed and scalability into the enterprise integration space. The ease of use of the designer, variety of available triggers and actions, and powerful management tools make centralizing your APIs simpler than ever. As businesses move towards digitalization, Logic Apps allow you to connect legacy and cutting-edge systems together.

*	Logic Apps provide a way to simplify and implement scalable integrations and workflows in the cloud. It provides a visual designer to model and automates your process as a series of steps known as a workflow.

*	Logic Apps is a fully managed PaaS (integration Platform as a Service) allowing developers not to have to worry about building hosting, scalability, availability, and management. Logic Apps will scale up automatically to meet demand.

Every logic app workflow starts with a trigger, which fires when a specific event happens, or when newly available data meets specific criteria. Many triggers include basic scheduling capabilities so that you can specify how regularly your workloads run. For more custom scheduling scenarios, start your workflows with the Schedule trigger.

**Implementation:** 

In this implementation, the Logic app will be triggered whenever a message is received by the Service Bus name space queue. The trigger will perform and http POST action to send data to Web API Service.

#### 2.5.8 Web App 

**Introduction:** 

Azure Web Apps enables you to build and host web applications in the programming language of your choice without managing infrastructure. It offers auto-scaling and high availability, supports both Windows and Linux, and enables automated deployments from GitHub, Visual Studio Team Services, or any Git repo.

Web Apps not only adds the power of Microsoft Azure to your application, such as security, load balancing, auto scaling, and automated management. You can also take advantage of its DevOps capabilities, such as continuous deployment from VSTS, GitHub, Docker Hub, and other sources, package management, staging environments, custom domain, and SSL certificates

**Implementation:** 

Web API receives data from the Logic pp and processes the generated data and moves it to the Azure SQL Database and stores them in tables.

#### 2.5.9 Azure SQL Database 

**Introduction:** 

Azure SQL Database is a general-purpose relational database managed service in Microsoft Azure that supports structures such as relational data, JSON, spatial, and XML. SQL Database offers logical servers that can contain single SQL databases and elastic pools, and Managed Instances (in public preview) that contain system and user databases.

**Implementation:** 

In this solution, Azure SQL Database is used to store the data received from Web API. Web API processes the data and stores it in the Azure SQL Database as tables. Power BI reports can be generated using the data stored in the Azure SQL Database.

#### 2.5.10 Power BI 

**Introduction:**

Power BI is a suite of business analytics tools that deliver insights throughout your organization. Connect to hundreds of data sources, simplify data prep, and drive ad hoc analysis. Produce beautiful reports, then publish them for your organization to consume on the web and across mobile devices. Everyone can create personalized dashboards with a unique, 360-degree view of their business. And scale across the enterprise, with governance and security built-in.

**Implementation:**

Power BI Desktop is used to visualize the output of the solution. Power BI gathers data from the Azure SQL Database and visualizes it in pictorial representation of the desired outputs.

#### 2.5.11 Azure Container Registry 

**Introduction:**

Azure Container Registry allows you to store images for all types of container deployments including DC/OS, Docker Swarm, Kubernetes and Azure services such as App Service, Batch, Service Fabric and others. Azure Container Registry is a managed Docker registry service based on the open-source Docker Registry 2.0. Create and maintain Azure container registries to store and manage your private Docker container images.

**Implementation:**

In this implementation, Azure Container registry is used to store the images used as part of the IoT Edge modules and other aspects. Images like the websvc4dc image is stored in a private repository that will be pulled down to create containers. 

#### 2.5.12 Machine Learning Service workspace

**Introduction:**

Azure Machine Learning service is a cloud service that you use to train, deploy, automate, and manage machine learning models.
The workspace is the top-level resource for Azure Machine Learning service, providing a centralized place to work with all the artifacts you create when you use Azure Machine Learning service.The workspace keeps a history of all training runs, metrics and a snapshot of your scripts.

**Implementation:**

Machine learning service workspace is used to create experiments, register model, deploy image and web service.

#### 2.5.13 Application Insights 

**Introduction:**

Application Insights is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor your live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help you diagnose issues and to understand what users do with your app. It's designed to help you continuously improve performance and usability.

**Implementation:**

Application insights provides the ability to monitor the Web API request status. It will be used to understand the failure of Web requests and also log successful requests.

#### 2.5.14 OMS Log Analytics 

**Introduction:**

The Microsoft Operations Management Suite (OMS), previously known as Azure Operational Insights, is a software as a service platform that allows an administrator to manage on-premises and cloud IT assets from one console.

Microsoft OMS handles log analytics, IT automation, backup and recovery, and security and compliance tasks. Log analytics will collect and store your data from various log sources and allow you to query over them using a custom query language.

**Implementation:**

OMS log analytics is used in this solution to monitor Azure resources. OMS Log analytics provides detailed insights into the usage of the Dyno Card solution. 

To enable OMS Log Analytics for Azure SQL Database and Logic App, we have following solutions.

1.	Logic App Management Solution.

2.	Azure SQL Database Management Solution.

The Above Solutions will provide more insights of the Azure Resources Logic app and Azure SQL Database respectively. 

By using log search option we can get Meta data information like IoT Hub, stream analytics, Service bus name space resources.

#### 2.5.15 Function App 

**Introduction:**

Azure Functions is the server less computing service hosted on the Microsoft Azure public cloud. Azure 	Functions, and server less computing, in general, is designed to accelerate and simplify application development. 

Implementation: 

Azure function App HTTP trigger url is accessed through IoT edge script inside the edge VM, which will create an edge device in IoT Hub.

## 3.0 Solution Types and Deployment Costs

### 3.1 Solutions and Associated Costs 

The Automated solutions provided by us covers in Section. Will have the following Cost associated. The solutions are created considering users requirements & have Cost effective measures. User have control on what Type of azure resources need to be deploy with respect to SKU and Cost. These options will let user to choose whether user wants to deploy azure resources with minimal SKU and Production ready SKU. The Cost Models per solutions are explained in future sections:

#### 3.1.1 Basic 

This solution is mostly recommended for a PoC or Piloting environment. Deploy the Basic solution when you want only to need the monitoring for the Azure components. Also, when the Basic solution is deployed, no high availability is provided. 

The Basic solution requires minimum Azure components with minimal available SKU’s. This Solution provides (Core + Monitoring) features such as Application Insights & OMS Log Analytics. The details on components used in this solution is listed in section below: 

* The Estimated Monthly Azure cost is: **$105.67**
  
* The Estimated Monthly Azure cost (Including Optional Component) is: **$300.73**

**Note**: Refer below table for the optional component list & Features

**Pricing Model for Basic Solution:** 

Prices are calculated by using the US East Location and Pricing Model is set to **"PAYG"**. This may vary depending on your region and licensing model. 



| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | F1: Shared Cores(s),  1 GB RAM, 1 GB Storage, US$0.00, Location: East US, Windows VM     | PAYG         | $0.00
| **Consumption Plan**       | Free, 1 million requests and 400,000 gigabyte seconds (GB-s).      | PAYG         | $0.00
|**Data Lake Store**           |Storage used 5GB, read transactions 1*US$0.004, write transaction 1*US$0.05            | PAYG          |$0.25
| **IoT HUB**        | Free Tier,F1-Free, Unlimited devices, 8,000 messages/day.                     | PAYG                          | $0.00   
| **Logic App**      | Data Retention 1GB * US$0.12 per GB/month, Location: East US                  | PAYG                          | $0.12 
| **Service Bus Namespace**    | Standard, Messaging Operations 1 millions/month * US$0.05 per millions operations.    | PAYG        | $9.81
| **SQL Database**        | B1 (Basic tier), 5DTUs, 2GB included storage per DB, US$ 0.0067/hour      | PAYG                       | $4.90  
| **Stream Analytics**       | Standard Streaming Unit, 1 Units * US$ 80.30                          | PAYG                          | $80.30
| **Azure Container Registry**       | Basic Tier, 1 units * 30 day * US$ 0.167 per unit per day = US$ 5.00, total WebHooks = 2, AS= 10GiB      | PAYG                             | $5.00
| **Machine learning Service workspace**   | Free    | PAYG   | $0.00
| **Application Insights**     | 6 GB, 5 GB of data is included for free. 1 GB * $2.99   | PAYG                             | $2.99
| **Log Analytics**     | 6 GB, 5 GB of data is included for free. 1 GB * $2.30   | PAYG                             | $2.30
| **Virtual machine (edgevm)**(Optional - Depends on Customer Choice)   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60
| **Virtual network(Optional)**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10
| **Virtual machine (Data Science VM)(Optional)**     | Data Science VM windows -2016 Standard, B2s: 2 vCPU(s), 4GB RAM, 8GB Temporary storage   | PAYG                             | $41.66 
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70   
|      |       | **Estimated monthly cost**       | **$105.67**
|      |       | **Optional Cost**       | **$195.06**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$300.73**               

#### 3.1.2 Standard

Deploy the Standard solution when you want monitoring, high availability and security/hardening for the deployed components. High availability is achieved by deploying the components in two regions. When the primary region fails , the secondary region needs to be redeployed (manual effort required).

This Solution provides (Core + Monitoring + Hardening) features such as Application Insights, OMS Log Analytics, High Availability, Security & Disaster Recovery. The details of the components used in this solution is listed in section below: 

* The Estimated Monthly Azure cost is: **301.21**  

* The Estimated monthly Azure cost (Including Optional Component) is: **496.27**  

**Note**: Refer below table for the optional component list & Features

**Pricing Model for Standard Solution:** 

Prices are calculated by Location as **East US** and Pricing Model as **“PAYG”.** 

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | 2 * (Standard Tier; 1 S1 (1 Core(s), 1.75 GB RAM, 50 GB Storage) x 730 Hours; Windows OS; Location: East US)      | PAYG         | $146.00  
| **Consumption Plan**       | Free, 1 million requests and 400,000 gigabyte seconds (GB-s).       | PAYG         | $0.00
|**Data Lake Store**           |Storage used 5GB, read transactions 1*US$0.004, write transaction 1*US$0.05            | PAYG          |$0.25
| **IoT HUB**        | Standard Tier, S1-Free, Unlimited devices, 8,000 messages/day.                     | PAYG                          | $25.00     
| **Logic App**      | Data Retention 1GB * US$0.12 per GB/month, Location: East US                  | PAYG                          | $0.12 
| **Service Bus Namespace**    | Standard, Messaging Operations 1 millions/month * US$0.05 per millions operations.    | PAYG        | $9.81
| **SQL Database**        | 2 * (Single Database, DTU Purchase Model, Standard Tier, S0: 10 DTUs, 250 GB included storage per DB, 1 Database(s) x 730 Hours, 5 GB Retention)       | PAYG                       | $29.44  
| **Stream Analytics**       | Standard Streaming Unit, 1 Units * US$ 80.30                          | PAYG                          | $80.30
| **Azure Container Registry**       | Basic Tier, 1 units * 30 day * US$ 0.167 per unit per day = US$ 5.00, total WebHooks = 2, AS= 10GiB      | PAYG                             | $5.00
| **Machine learning service workspace**   | Free    | PAYG   | $0.00
| **Application Insights**  | 6 GB, 5 GB of data is included for free. 1 GB * $2.99  | PAYG  | $2.99 
| **Log Analytics**    | 6 GB, 5 GB of data is included for free. 1 GB * $2.30  | PAYG  | $2.30
| **Virtual machine (edgevm)(Optional - Depends on Customer Choice)**   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60
| **Virtual network(Optional)**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10 
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70 
| **Virtual machine (Data Science VM)(Optional)**     | Data Science VM windows -2016 Standard, B2s: 2 vCPU(s), 4GB RAM, 8GB Temporary storage   | PAYG                             | $41.66  
|      |       | **Estimated monthly cost**       | **$301.21**
|      |       | **Optional Cost**       | **$195.06**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$496.27**             

**Note**: When we redeploy the solution, there will be no extra cost, since primary region is already paid for. 

#### 3.1.3 Premium

Deploy the Premium solution when you require Monitoring, High Availability and Hardening/Security for the deployed components. High availability is achieved by deploying the same components in two regions at the same time. No redeployment is required in this solution.

This solution also provides (Core + Monitoring + Hardening), the difference between Standard & Premium solution is under Premium - both the regions will be deployed at same time. The details on components used in this solution is listed in section below: 

* The Estimated Monthly Azure cost is: **$391.69**  

* The Estimated Monthly Azure cost (Including Optional Component) is: **$586.75**  

**Note**: Refer below table for the optional component list & Features 

**Pricing Model for Premium Solution:** 

Prices are calculated by Considering Location as **East US** and Pricing Model as **“PAYG”**.

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan** | 2 * (Standard Tier; 1 S1 (1 Core(s), 1.75 GB RAM, 50 GB Storage) x 730 Hours; Windows OS; Location: East US)US) | PAYG         | $146.00 
| **Consumption Plan**       | Free, 1 million requests and 400,000 gigabyte seconds (GB-s).      | PAYG         | $0.00
|**Data Lake Store**           |2 * (Storage used 5GB, read transactions 1*US$0.004, write transaction 1*US$0.05) (2 * 0.25)             | PAYG          |$0.50  
| **IoT HUB**        | Standard Tier, S1-Free, Unlimited devices, 8,000 messages/day.             | PAYG                          | $25.00  
| **Logic App**      | 2 * (Data Retention 1GB * US$0.12 per GB/month, Location: East US) (2 * 0.12)                   | PAYG                          | $0.24 
| **Service Bus Namespace**    | 2 * (Standard, Messaging Operations 1 million/month * US$0.05 per million operations) (2*9.81).    | PAYG        | $19.62  
| **SQL Database**        | 2 * (Single Database, DTU Purchase Model, Standard Tier, S0: 10 DTUs, 10 GB included storage per DB, 1 Database(s) x 730 Hours, 5 GB Retention)      | PAYG                       | $29.44 
| **Stream Analytics**       | 2 * (Standard Streaming Unit, 1 Units * US$ 80.30) (2 * 80.30)                          | PAYG                          | $160.60 
| **Azure Container Registry**       | Basic Tier, 1 units * 30 day * US$ 0.167 per unit per day = US$ 5.00, total WebHooks = 2, AS= 10GiB      | PAYG                             | $5.00
| **Machine learning service workspace**   | Free    | PAYG   | $0.00
| **Application Insights**  | 6 GB, 5 GB of data is included for free. 1 GB * $2.99  | PAYG  | $2.99 
| **Log Analytics**    | 6 GB, 5 GB of data is included for free. 1 GB * $2.30  | PAYG  | $2.30
| **Virtual machine (edgevm)(optional)**   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60 
| **Virtual network**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70 
| **Virtual machine (Data Science VM)(Optional)**     | Data Science VM windows -2016 Standard, B2s: 2 vCPU(s), 4GB RAM, 8GB Temporary storage   | PAYG                             | $41.66  
|      |       | **Estimated monthly cost**       | **$391.69**
|      |       | **Optional Cost**       | **$195.06**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$586.75**               


## 3.2 Solution Features and Cost Comparison: 

In this section we will be comparing the features of each of the solutions and the cost for all the solution:

### 3.2.1 In terms of features:

The below table explain the distinctive features available across solution types.

| **Resource Name**                  | **Parameter**           | **Basic**           | **Standard**             | **Premium**
| -------------              | -------------     | -------------     | -------------         | -------------
| App Service Plan	      | SKU                     | F1               | S1	              | S1
|              | Cores      | Shared Cores	     | 1 core	        | 1 core
|              | RAM	    | 1GB	     | 1.75GB	      | 1.75GB
|              | Storage	  | 1GB       | 50GB	     | 50GB
|              | OS           |	Windows        | Windows	     | Windows
| Consumption Plan     | SKU	     | Free	        | Free	          | Free
|                  | Executions	    | 1 million requests and 400,000 gigabyte seconds (GB-s).     | 1 million requests and 400,000 gigabyte seconds (GB-s).      | 1 million requests and 400,000 gigabyte seconds (GB-s).
|Data Lake Store    | Storage Used	     | 5GB	     | 5GB      | 2*5GB
|              | Read Transactions	    | 1 Transaction units (10,000 transactions)	     | 1 Transaction units (10,000 transactions)	   | 2*1 Transaction units (10,000 transactions)
|              | Write Transactions	     | 1 Transaction units (10,000 transactions)	   | 1 Transaction units (10,000 transactions)	     | 2*1 Transaction units (10,000 transactions)
| IoT-Hub	     | SKU       | F1	    | S1	      | S1
|	     | Devices     | 500 Devices	  | Unlimited Devices 	   | Unlimited Devices 
|         |	Messages       | 8000 msgs/day	    | 4,00,000 msgs/day	     | 4,00,000 msgs/day
| Logic App	     | Data Retention	    | 1GB	    | 1GB	      | 2 * 1GB
| Service Bus Namespace       |	SKU	Standard    	| Standard    	| 2 * Standard
|              | Messaging operations	    | 1 Million/month	       | 1 Million/month	       | 1 Million/month
|            	| Brokered connections	    | 1000 connections	      | 1000 connections	       | 1000 connections
| SQL Database     | SKU	   | B1	      | S1	       | S1
|          | Database        |1	          | 1	             | 1
|           | Storage	    | 2 GB	       | 250 GB	        | 250 GB
|            | Purchase model	    | 5 DTUs	     | 10 DTUs	        | 10 DTUs
| Stream Analytics	    | SKU	      | Standard	   | Standard	     | Standard
|                  | Streaming Unit       | 1 Units	        | 1 Units         | 1 Units
| Azure Container Registry         | SKU	        | Basic         | Basic	          | Basic
|            | Units              |	1	         | 1	             | 1
| Machine learning service workspace       | SKU	       | Free	       | Free	       | Free
| Application Insights        |	Logs collected	       | 6 GB, 5 GB of data is included for free.	    | 6 GB, 5 GB of data is included for free.	     | 6 GB, 5 GB of data is included for free.
| Log Analytics	       | Logs ingested	       | 5 GB of data is included for free. An average Azure VM ingests 1 GB to 3 GB	      | 5 GB of data is included for free. An average Azure VM ingests 1 GB to 3 GB	       | 5 GB of data is included for free. An average Azure VM ingests 1 GB to 3 GB
| Virtual machine (edgevm) (Optional - Depends on Customer Choice)	       | SKU	           | Standard	         | Standard	        | Standard
|                     |	Cores	            | 2 core	         | 2 core	                | 2 core
|	                  | RAM	                | 3.5 GB	                | 3.5 GB	                    | 3.5 GB
|	                  | Storage	                 | 135 GB	          | 135 GB	            | 135 GB
|	                   | OS	                   | Linux	               | Linux	                 | Linux
| Virtual network (Optional)	        | Data Transfer	        | 5 GB	           | 5 GB	             | 5 GB
|                   | Outbound data transfer           | 5       	          | 5	                   | 5
|                 |	Inbound data transfer	             | 5	            | 5	                 | 5
| Virtual machine (Data Science VM) (Optional)	        | SKU	              | Standard, B2s	       | Standard, B2s	        | Standard, B2s
|                  | Cores	           | 2 vCPU(s)	        | 2 vCPU(s)	          | 2 vCPU(s)
|                 |	RAM           | 4 GB	       | 4 GB	      | 4 GB
|                | Storage	          | 8 GB	          | 8 GB	            | 8 GB
|	            |OS           |	Windows VM	              | Windows VM	                  | Windows VM
| Virtual machine (modbusvm) (Optional)        | SKU	           | Standard	            |Standard, D2 v3	        | Standard, D2 v3
|                      | Cores	            | 1 Core	          | 2 vCPU(s)	               | 2 vCPU(s)
|                    | RAM	               | 1.75 GB	            | 8 GB	               | 8 GB
|	                   | Storage	            | 70 GB	            | 50 GB	                 | 50 GB
|	                    | OS	           | Windows VM	             | Windows VM	              | Windows VM


### 3.2.2 Solution Cost  Impact:  

The below Table explains the $ impact for the solutions by resources.

| **Resource Name**	                 | **Basic**           | **Standard**              | **Premium**
| -------------                 | -------------          | -------------            | -------------
| **App Service Plan**     |	$0.00   |	$146.00   |	$146.00 
| **Consumption Plan**     |	$0.00   |	$0.00    |	$0.00 
| **Data Lake Store**      |	$0.25 	|   $0.25 	 |  $0.50 
| **IoT-Hub**	  |  $0.00 	|  $25.00    |	$25.00 
| **Logic App**   |	$0.12   |	$0.12    |	$0.24 
| **Service Bus Namespace**    |	$9.81     |	$9.81   |	$19.62 
| **SQL Database** | $4.90      |	$29.44     | 	$29.44 
| **Stream Analytics** | $80.30 	 | $80.30   |  $160.60 
| **Azure Container Registry**    | $5.00 	    | $5.00     |	$5.00  
| **Machine learning service workspace**    | $0.00      | $0.00     | $0.00 
| **Application Insights**    | $2.99 	  | $2.99 	   | $2.99 
| **Log Analytics**   | $2.30      | $2.30 	    | $2.30 
| **Virtual machine (edgevm) (Optional)**    | $87.60 	    | $87.60     | $87.60 
| **Virtual network (Optional)**    | $0.10 	  | $0.10 	     | $0.10 
| **Virtual machine (Data Science VM) (Optional)**    | $41.66 	  | $41.66 	    | $41.66 
| **Virtual machine (modbusvm) (Optional)**    | $65.70 	  | $65.70    | $65.70 


### 3.2.3 stimated Monthly Cost for each Solution:

| **Resource Name**                 | **Basic**           | **Standard**            | **Premium**
| -------------                  | -------------          | -------------            | ------------- 
| **Estimated monthly cost**     | **$105.67**            | **$301.21** 	              | **$391.69** 
| **Optional Cost**      | **$195.06** 	       | **$195.06** 	              | **$195.06** 
| **Estimated monthly cost (Including Optional)**       | **$300.73** 	       | **$496.27** 	              | **$586.75** 


## 4.0 Further Reference

### 4.1 Deployment Guide

To Deploy the Oil & Gas solution please refer Deployment guide in the Provided Documents.

https://github.com/BlueMetal/iot-edge-dynocard/blob/master/dynocards-wiki/Deployment-Guide.md

### 4.2 Admin Guide

Refer Admin Guide to perform Administrator tasks of the Solution in Azure Portal.

https://github.com/BlueMetal/iot-edge-dynocard/blob/master/dynocards-wiki/Admin-Guide-Page.md

### 4.3 User Guide

Refer User Guide to find how to use Oil & Gas solution once deployed successfully in Azure portal. 

https://github.com/BlueMetal/iot-edge-dynocard/blob/master/dynocards-wiki/User-Guide.md


************************************* END OF DOCUMENT *************************************
