## 1 Introduction
### 1.1 Overview of Oil & Gas Industry

Oil and Gas Companies wants to operate the sucker pumps in an Efficient, Safe, Eco – Friendly & responsible manner. Companies are using dynamometers (dyno) surveys to determine the condition of the pump operating under ground or downhole. Medium to large oil companies have thousands of these pumps scattered remotely across the world. Monitoring the dynamometers with the current setup is ideally very expensive, Time Consuming & ineffective solution.  To have Efficient & Effective solution to overcome the problem, we have come up with the Automated Solution which is Reliable, Cost Effective, Scalable & useful across the industry. 

### 1.2 Overview of IOT Solution
#### 1.2.1 Highlights:

The Rationale behind this IOT Solution for Oil & Gas industry is to:  

1. **Detect issues with the pump using data points captured from the Dyno Card**
2. **Capturing historical data**  
3. **Immediately Turn off the pump** 
4. **Trigger Alert field technicians**

#### 1.2.2 Brief about the Solution:

Oil and Gas companies can remotely monitor the condition of the sucker pumps.  

* **Azure IoT Edge** technologies are applied to detect the issues at the edge, to send both immediate and historical dyno card messages to the cloud.  These messages can be used to issue alerts. We can visualize both the surface and pump card data. 
* This solution is beneficial to inspect thousands of sucker pumps throughout remote areas of the world by issuing alerts. Based on the alerts that are received to the cloud, the field technicians can repair the appropriate sucker pumps. 

## 2 IoT Solutions
### 2.1 Existing Solution

Existing solution process flow is explained in following steps:  

* Azure IoT Edge modules to retrieve Dyno Card data using both Modbus and OPC Server interfaces. 
* Azure SQL Database to Save Dyno Card data. 
* Azure ML to detect issues with Sucker Pumps 
* Azure IoT Edge to send messages to IoT Hub 
* Stream Analytics Jobs to save data to an Azure Data Lake 
* Logic App to save data through a Web API into a database 
* Power BI to visualize the Dyno Card data 


#### 2.1.1 Core Architecture (Current) 

Below Diagram explains the Core architecture for oil & Gas companies.

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/core.png)

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

### 2.2 Automated  Solution

Automated IOT Solution is designed on the top of current core architecture. In addition, this solution also provides **monitoring**, **High availability** and **Security**. 

This solution is deployed through ARM template. This is a single click deployment which reduces manual effort when compared with the existing solution. 

In addition, this solution consists 

* Application Insights to provide monitoring for Web API. Application Insights store the logs of the Web API which will be helpful to trace the web API working. 
* Log analytics to provide monitoring for Logic App, IoT hub, Azure Data Lake Store, Service Bus Namespace, SQL database. Log analytics store the logs, which will be helpful to trace the working of these resources. 
* Geo-replication to provide high availability for SQL database. Geo-replication is used to set the recovery database as the primary database whenever primary database is failed. 
* Securing steps for Data Lake Store with encryption and Firewall options. We can restrict users to access Data Lake Store from unwanted IP ranges. 
* Securing steps for IoT Hub with Shared Access policies and IP Filter options. 
* Securing steps for Service Bus Namespace with Shared Access policies options. 
* Securing steps for SQL Database with Enable Firewall, SQL Database Authentication, Advanced Threat Protection, Auditing and Transparent Data Encryption options. We can restrict access from unwanted networks.  
* This solution also provides Disaster Recovery activities. IoT Hub manual fail over is helpful to make the IoT Hub available in another region, when IoT Hub of one region is failed. 

### 2.3 Architectures
#### 2.3.1 Basic Architecture:
Basic solution will have all core components, in addition this solution also consists monitoring components like Application Insights and OMS Log Analytics.  
* Application Insights provide monitoring for Web API. 
* OMS Log Analytics provide monitoring for Logic App, IoT hub, Azure Data Lake Store, Service Bus Namespace, SQL database.
 
The below diagram depicts the data flow between azure components:

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/Basic.png)

Basic Architecture comprises of following components: 

* 3-Virtual machines  
* 1-Web App 
* 1-Function App 
* 1-Application Insights 
* 1-Data Lake Storage 
* 1-IoT HUB 
* 1-Log analytics 
* 1-Logic app 
* 1-service bus namespace 
* 1-SQL database 
* 1-Storage account 
* 1-Stream Analytics job 
* 1-Machine Learning Experiment Account 
* 1-Machine Learning Management Account 
* 1 – Container registry  

#### 2.3.2 Standard Architecture:
Standard Architecture diagram will have two regions. 

1. Primary Region (Deployment) 
2. Secondary Region (Re – Deployment) 

We have IoT Hub manual failover, SQL DB geo replication and redeployment components. The effect of these components will occur when primary Region goes down. 

The main use of this solution is whenever disaster recovery occurs the redeployment components will deploy in another region and user need to configure the VM’s with redeployment components. 

The below diagram depicts the dataflow between azure components in standard solution: 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/Standard.png)

Standard Architecture comprises of following components: 

* 3-Virtual machines (2 windows & 1 Linux) Windows VMS **Dyno card VM** which install pre-installed software's for dyno card VM. **ML VM** which install pre-installed Docker. Linux VM **Edge VM** is used to create IoT Edge device and installs modules in IoT Edge device. **VMs are optional Components.** 
* 1-Web App 
* 1-Function App 
* 1-Application Insights 
* 1-Data Lake Storage 
* 1-IoT HUB 
* 1-Log analytics 
* 1-Logic app 
* 1-service bus namespace 
* 2-SQL database 
* 1-Storage account 
* 1-Stream Analytics job 
* 1-Machine Learning Experiment Account 
* 1-Machine Learning Management Account 
* 1 – Container registry 

When there is a Region failover, user needs to redeploy ARM Template provided in GIT Repo. When redeployment completed successfully, below azure resources will be deployed.  

**Note:  Deployment process will take some time around 30 mins to complete deployment successfully.** 

* 1-Web App 
* 1-Data Lake Storage 
* 1-Logic app 
* 1-service bus namespace 
* 1-Stream Analytics job 

#### 2.3.3 Premium  Architecture:
Premium Architecture diagram also have two regions. 

1. Primary Region 
2. Secondary Region 

Both the regions have same set of components.  

In primary region the configuration and set up of VM’s was default but in secondary region we need to configure and setup the VM’s. 

The below diagram depicts the dataflow between azure components in premium solution: 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/Premium.png)

Premium Architecture comprises of following components: 

* 3-Virtual machines (2 windows & 1 Linux) Windows VMS **Dyno card VM** which install pre-installed software's for dyno card VM. **ML VM** which install pre-installed Docker. Linux VM **Edge VM** is used to create IoT Edge device and installs modules in IoT Edge device. **VMs are optional Components.** 
* 2-Web App 
* 1-Function App 
* 1-Application Insights 
* 2-Data Lake Storage 
* 1-IoT HUB 
* 1-Log analytics 
* 2-Logic app 
* 2-service bus namespace 
* 2-SQL database 
* 1-Storage account 
* 2-Stream Analytics job 
* 1-Machine Learning Experiment Account 
* 1-Machine Learning Management Account 
* 1 – container registry 

In this type of solution all components will be deployed at initial deployment itself. 

This type of solution reduces downtime of solution when region is down. In this solution there is redeployment approach which reduces downtime of the solution. 

### 2.4 Conventional Data Work Flow 
The data flow is similar across all the solutions we have explained in section 2.3 

The below diagram explains about the data flow between azure components with in the solution. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/raw/master/images/4.png)

Steps involved in data work flow between the components: 
1. Simulator device sends data to IoT Hub via IoT edge using Edge modules. 
2. Stream analytics pulls data from IoT Hub & ingests data into Data Lake Store. 
3. Using IoT Hub Endpoints, data will be pushed to Service Bus Queue. 
4. Logic app polls service bus queue for every 10s, whenever data reached to service bus queue, logic app will be triggered. 
5. Logic app processes the telemetry data from IoT Hub. 
6. Using POST operation, data will be sent to IoT Hub. 
7. From Web API, data will be inserted into SQL Database. 
8. Power BI applied on SQL Database. 


### 2.5 Azure Components Functionality 

Microsoft Azure is a cloud computing service created by Microsoft for building, testing, deploying, and managing applications and services through a global network of Microsoft-managed data centers. It provides software as a service (SaaS), platform as a service (PaaS) and infrastructure as a service (IaaS) and supports many different programming languages, tools and frameworks, including both Microsoft-specific and third-party software and systems. 

Microsoft lists over 600 Azure services, of which some are as below: 

* Compute 
* Storage services 
* Data management 
* Management 
* Machine learning 
* IoT 

#### 2.5.1 Simulator 

**Introduction:**

PLC simulator is an open source and free, it is programmed in a language. It has a diagnostics screen that shows the traffic for the Modbus commands and responses. Modrssim provided for handling all the Modbus device ID's from 0 to 255. 
Modbus simulator also support scripting. 

**Implementation:** 

Modbus simulator generates the data as per the VB script provided in Modbus simulator. Simulator sends the data to IoT Edge Modules. 

Simulator is an Optional Component, which lets user to choose whether user wants to deploy Simulator device in Cloud Based environment or user wants to deploy on premises network. When user wants to deploy Simulator in Virtual Machine, ARM Template will install all required software. If user wants to use simulator as on premises System, then user might require install required Software manually. 

#### 2.5.2 IoT Edge 

**Introduction:** 

Azure IoT Edge moves cloud analytics and custom business logic to devices so that your organization can focus on business insights instead of data management. Enable your solution to truly scale by configuring your IoT software, deploying it to devices via standard containers, and monitoring it all from the cloud. 

Analytics drives business value in IoT solutions, but not all analytics needs to be in the cloud. If you want a device to respond to emergencies as quickly as possible, you can perform anomaly detection on the device itself. Similarly, if you want to reduce bandwidth costs and avoid transferring terabytes of raw data, you can perform data cleaning and aggregation locally. Then send the insights to the cloud. 

Azure IoT Edge is made up of three components: 

* IoT Edge modules are containers that run Azure services, 3rd party services, or your own code. They are deployed to IoT Edge devices and execute locally on those devices. 
* The IoT Edge runtime runs on each IoT Edge device and manages the modules deployed to each device. 
* A cloud-based interface enables you to remotely monitor and manage IoT Edge devices. 

**Implementation:** 

IoT Edge device gathers data from Simulator and processes the data to IoTHub. Below module will be installed in IoT Edge System. 

* sql 
* modbus 
* dynocardalertModule 
* Mlalertmodule 
* edgeAgent 
* edgeHub 

IoT Edge is an Optional Component, which lets user to choose whether user wants to deploy IoTEdge device in Cloud Based environment or user wants to deploy on premises network. When user wants to deploy IoTEdge in Virtual Machine, ARM Template will configure all required parameters. If user wants to deploy IoT Edge as on premises System, then user might require running some manual Configuration. 

#### 2.5.3 IoT Hub 

**Introduction:** 

Azure IoT Hub is a fully managed service that enables reliable and secure bi-directional communications between millions of IoT devices and an application back end.  

Azure IoT Hub offers reliable device-to-cloud and cloud-to-device hyper-scale messaging, enables secure communications using per-device security credentials and access control, and includes device libraries for the most popular languages and platforms. Before you can communicate with IoT Hub from a gateway you must create an IoT Hub instance in your Azure subscription and then provision your device in your IoT hub. Some samples in this repository require that you have a usable IoT Hub instance. 

The Azure IoT Hub offers several services for connecting IoT devices with Azure services, processing incoming messages or sending messages to the devices. From a device perspective, the functionalities of the Azure IoT Hub enable simple and safe connection of IoT devices with Azure services by facilitating bidirectional communication between the devices and the Azure IoT Hub. 

**Implementation:** 

IoT Hub is the core component of IoT Hub Solution. IoT Edge VM will send the data to IoT Hub using Edge Modules. There are below modules present in the IoT Hub. 

* sql 
* modbus 
* dynocardalertModule 
* Mlalertmodule 
* edgeAgent 
* edgeHub 

#### 2.5.4 Stream Analytics Job 

**Introduction:** 

Stream Analytics is an event processing engine that can ingest events in real-time, whether from one data stream or multiple streams. Events can come from sensors, applications, devices, operational systems, websites, and a variety of other sources. Just about anything that can generate event data is fair game. 

Stream Analytics provides high-throughput, low-latency processing, while supporting real-time stream computation operations. With a Stream Analytics solution, organizations can gain immediate insights into real-time data as well as detect anomalies in the data, set up alerts to be triggered under specific conditions, and make the data available to other applications and services for presentation or further analysis. Stream Analytics can also incorporate historical or reference data into the real-time streams to further enrich the information and derive better analytics. 

To implement a streaming pipeline, developers create one or more jobs that define a stream’s inputs and outputs. The jobs also incorporate SQL-like queries that determine how the data should be transformed. In addition, developers can adjust a number of a job’s settings. For example, they can control when the job should start producing result output, how to handle events that do not arrive sequentially, and what to do when a partition lags other or does not contain data. Once a job is implemented, administrators can view the job’s status via the Azure portal. 

**Implementation:** 

Stream Analytics gets data input from IoT Hub and output data to Data lake store. Stream analytics processes all the data from IoT Hub to Data Lake Store. Stream analytics should always in running state. Data lake store authorization must be renewed if stream analytics failed.  

#### 2.5.5 Azure Data Lake Store 

**Introduction:** 

The Azure Data Lake Store is an enterprise-wide hyper-scale repository for managing big data analytic workloads. Azure Data Lake can capture data of any size, type, and ingestion speed in one single place for operations and analytics. Azure Data Lake Store can be accessed from Hadoop Distributed File System (HDFS) and Microsoft’s HDInsight cluster using the Compatible REST APIs. It is specifically designed to enable analytics on the stored data and is tuned for performance for data analytics scenarios. It has all the enterprise-grade capabilities like scalability, security, reliability, manageability, and availability—essential for real-world enterprise use cases. 

Azure Data Lake store can handle any data in their native format, as is, without requiring prior transformations. Data Lake store does not require a schema to be defined before the data is uploaded, leaving it up to the individual analytic framework to interpret the data and define a schema at the time of the analysis. Being able to store files of arbitrary size and formats makes it possible for Data Lake store to handle structured, semi-structured, and even unstructured data. 

**Implementation:** 

Azure Data lake store resources stores the data in JSON file. Stream analytics collects the data from IoT Hub and sends output to Azure data lake store. Stream analytics process the data from IoT Hub and send it to Data lake store. 

#### 2.5.6 Service Bus Name Space Queue 

**Introduction:** 

Azure Service Bus is a multi-tenant Cloud Service, which means that the Service is shared by the multiple users. Each user creates a namespace and defines the communication mechanism requirements within that namespace. 

The purpose of this Service is to make the communication easier. When two or more business partners want to exchange the information, they need a communication mechanism. Service Bus is a brokered or third-party communication mechanism. Azure Service Bus is like a mailing Service in the physical world and make it very easy to send diverse types of letters and packages with a variety of delivery guarantees, anyplace in the world. 

If geo recovery needs to be enabled, then following points needs to Consider. 
* In your fail over planning, you should also consider the time factor. For example, if you lose connectivity for longer than 15 to 20 minutes, you might decide to initiate the fail over. 
* The fact that no data is replicated means that currently active sessions are not replicated. Additionally, duplicate detection and scheduled messages may not work. New sessions, new scheduled messages and new duplicates will work. 
* Synchronizing entities can take some time, approximately 50-100 entities per minute.  

**Implementation:** 

The data generated from IoT Device (Simulator) will be send to IoTHub. Using IoT hub endpoint Configuration, Data received to IoT Hub will be redirected to Service bus name space. All the messages received to IoT Hub will be routed to service bus name space queue. 

#### 2.5.7 Logic App 

**Introduction:** 

Logic Apps are a relatively new feature of Microsoft Azure that makes it simple to build complex workflows using one or more of the over 200 plus different connectors. Since the Logic Apps are server less, you do not need to worry about server sizing. The platform will scale to meet your demand, and better yet. 

* Logic Apps brings speed and scalability into the enterprise integration space. The ease of use of the designer, variety of available triggers and actions, and powerful management tools make centralizing your APIs simpler than ever. As businesses move towards digitalization, Logic Apps allow you to connect legacy and cutting-edge systems together. 
* Logic Apps provide a way to simplify and implement scalable integrations and workflows in the cloud. It provides a visual designer to model and automates your process as a series of steps known as a workflow. 
* Logic Apps is a fully managed PaaS (integration Platform as a Service) allowing developers not to have to worry about building hosting, scalability, availability, and management. Logic Apps will scale up automatically to meet demand. 

Every logic app workflow starts with a trigger, which fires when a specific event happens, or when newly available data meets specific criteria. Many triggers include basic scheduling capabilities so that you can specify how regularly your workloads run. For more custom scheduling scenarios, start your workflows with the Schedule trigger. 

**Implementation:** 

Logic app will be triggered whenever a message received to service bus name space queue. When a message received to service bus name space queue logic app will get trigger and performs http post action on Web API to send data to web API. 

#### 2.5.8 Web App 

**Introduction:** 

Azure Web Apps enables you to build and host web applications in the programming language of your choice without managing infrastructure. It offers auto-scaling and high availability, supports both Windows and Linux, and enables automated deployments from GitHub, Visual Studio Team Services, or any Git repo. 

Web Apps not only adds the power of Microsoft Azure to your application, such as security, load balancing, auto scaling, and automated management. You can also take advantage of its DevOps capabilities, such as continuous deployment from VSTS, GitHub, Docker Hub, and other sources, package management, staging environments, custom domain, and SSL certificates 

**Implementation:** 

Web API receives data from Logic app and process the generated data to Azure SQL Database. And stores the data in respective tables. 

#### 2.5.9 SQL Database 

**Introduction:** 

SQL Database is a general-purpose relational database managed service in Microsoft Azure that supports structures such as relational data, JSON, spatial, and XML. SQL Database offers logical servers that can contain single SQL databases and elastic pools, and Managed Instances (in public preview) that contain system and user databases. 

**Implementation:** 

SQL Database will store the Data received from Web API. Power BI reports can be generated using Azure SQL Database. Web API processes the data and stores in database with in the tables accordingly.  

#### 2.5.10 Power BI 

**Introduction:**

Power BI is a suite of business analytics tools that deliver insights throughout your organization. Connect to hundreds of data sources, simplify data prep, and drive ad hoc analysis. Produce beautiful reports, then publish them for your organization to consume on the web and across mobile devices. Everyone can create personalized dashboards with a unique, 360-degree view of their business. And scale across the enterprise, with governance and security built-in. 

**Implementation:**

Power BI desktop is used to visualize the output of the solution. Power BI gathers data from SQL Database and visualizes in pictorial representation of output. 

#### 2.5.11 Azure Container Registry 

**Introduction:**

Azure Container Registry allows you to store images for all types of container deployments including DC/OS, Docker Swarm, Kubernetes and Azure services such as App Service, Batch, Service Fabric and others. Azure Container Registry is a managed Docker registry service based on the open-source Docker Registry 2.0. Create and maintain Azure container registries to store and manage your private Docker container images. 

Use container registries in Azure with your existing container development and deployment pipelines. Use Azure Container Registry Build (ACR Build) to build container images in Azure. Build on demand, or fully automate builds with source code commit and base image update build triggers. 

**Implementation:**

 Azure Container registry is used to store the websvc4dc image in private repository. When we are deploying websvc4dc web Service, it will create wbesvc4dc image in ACR. 

#### 2.5.12 Machine Learning Studio 

**Introduction:**

Microsoft Azure Machine Learning Studio is a collaborative, drag-and-drop tool you can use to build, test, and deploy predictive analytics solutions on your data. Machine Learning Studio publishes models as web services that can easily be consumed by custom apps or BI tools such as Excel. Machine Learning Studio is where data science, predictive analytics, cloud resources, and your data meet. 

**Implementation:**

Machine learning studio will be helpful to create Machine learning experiment account, model management account and workspace. 

#### 2.5.13 Application Insights 

**Introduction:**

Application Insights is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor your live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help you diagnose issues and to understand what users do with your app. It's designed to help you continuously improve performance and usability. 

**Implementation:**

Application insights provides the ability to monitor the web API requests status. It will be helpful to understand the failure web requests and successful requests. 

#### 2.5.14 OMS Log Analytics 

**Introduction:**

The Microsoft Operations Management Suite (OMS), previously known as Azure Operational Insights, is a software as a service platform that allows an administrator to manage on-premises and cloud IT assets from one console. 

Microsoft OMS handles log analytics, IT automation, backup and recovery, and security and compliance tasks. 

Log analytics will collect and store your data from various log sources and allow you to query over them using a custom query language. 

**Implementation:**

OMS log analytics is helpful to monitor azure resources. OMS log analytics provides in detailed insights using solutions. Logic app and SQL management solution provides more insights of the azure resources logic app and SQL Database respectively. Also, able to get Meta data information like resources IoT Hub, stream analytics, Service bus name space. 


#### 2.5.15 Function App 

**Introduction:**

Azure Functions is the server less computing service hosted on the Microsoft Azure public cloud. Azure Functions, and server less computing, in general, is designed to accelerate and simplify application development.  

Implementation: 

Function app is useful to create IoT Edge device in IoT Hub. Invoking HTTP call to azure function creates IoT Edge device in IoT Hub. 

## 3 Solution Types & Cost Mechanism

### 3.1 Solutions and Associated Costs 

The Automated solutions provided by us covers in Section …. Will have the following Cost associated. The solutions are created considering users requirements & have Cost effective measures. User have control on what Type of azure resources need to be deploy with respect to SKU and Cost. These options will let user to choose whether user wants to deploy azure resources with minimal SKU and Production ready SKU. The Cost Models per solutions are explained in future sections.

#### 3.1.1. Basic 

Deploy the Basic solution when you want only monitoring for the components. When Basic solution is deployed, no high availability is provided.  

The Basic solution requires minimum azure components with minimal available SKU’s. This Solution provides (Core + Monitoring) features such as application Insights & OMS Log Analytics. The details on components used in this solution is listed in Section:  

* The Estimated Monthly Azure cost is: **$105.67**  
* The Estimated Monthly Azure cost (Including Optional Component) is: **$396.31**

_Note: Refer below table for the optional component list & Features_

**Pricing Model for Basic Solution:** 

Prices are calculated by Considering Location as **East US** and Pricing Model as **“PAYG”.** 



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
| **ML Services**   | Model Management (Preview), Dev/Test, Location East US2(Managed models=20,managed deployments=2, Available Cores=4)| PAYG  | $0.00
| **Machine learning studio**   | Free    | PAYG   | $0.00
| **Application Insights**     | 6 GB, 5 GB of data is included for free. 1 GB * $2.99   | PAYG                             | $2.99
| **Log Analytics**     | 6 GB, 5 GB of data is included for free. 1 GB * $2.30   | PAYG                             | $2.30
| **Virtual machine (edgevm)**(Optional - Depends on Customer Choice)   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60
| **Virtual network(Optional)**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10
| **Virtual machine (MLVM)(Optional)**     | Standard, D2 v3:2 vCPU(s), 8GB RAM, 50GB Temporary storage, US $ 0.117/hour, East US, windows VM  | PAYG                             | $137.24 
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70   
|      |       | **Estimated monthly cost**       | **$105.67**
|      |       | **Optional Cost**       | **$290.64**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$396.31**               

#### 3.1.2. Standard

Deploy the Standard solution when you want monitoring, high availability and security for the components. High availability is achieved by deploying the components in two regions. When the primary region is failed, the secondary region needs to be redeployed. 

This Solution provides (Core + Monitoring +Hardening) features such as application Insights, OMS Log Analytics, High Availability, Security & Disaster recovery. The details on components used in this solution is listed in Section:  

* The Estimated Monthly Azure cost is: **01.21**  
* The Estimated monthly Azure cost (Including Optional Component) is: **91.85**  

_Note: Refer below table for the optional component list & Features_ 

 

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
| **ML Services**  | Model Management (Preview), Dev/Test, Location East US2(Managed models=20,managed deployments=2, Available Cores=4) | PAYG | $0.00
| **Machine learning studio**   | Free    | PAYG   | $0.00
| **Application Insights**  | 6 GB, 5 GB of data is included for free. 1 GB * $2.99  | PAYG  | $2.99 
| **Log Analytics**    | 6 GB, 5 GB of data is included for free. 1 GB * $2.30  | PAYG  | $2.30
| **Virtual machine (edgevm)(Optional - Depends on Customer Choice)**   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60
| **Virtual network(Optional)**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10 
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70 
| **Virtual machine (MLVM)(Optional)**     | Standard, D2 v3:2 vCPU(s), 8GB RAM, 50GB Temporary storage, US $ 0.117/hour, East US, windows VM  | PAYG                             | $137.24   
|      |       | **Estimated monthly cost**       | **$301.21**
|      |       | **Optional Cost**       | **$290.64**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$591.85**             

#### 3.1.3. Premium

Deploy the Premium solution when you want monitoring, high availability and security for the components. High availability is achieved by deploying the components in two regions at a time. No redeployment is required in Premium solution. 

This solution also provides (Core + Monitoring +Hardening), the difference between Standard & Premium solution is under Premium - Both the regions can be deployed at same time, and however this is not possible under standard solution. The details on components used in this solution is listed in Section:  

* The Estimated Monthly Azure cost is: **$391.69**  
* The Estimated Monthly Azure cost (Including Optional Component) is: **$682.33**  

_Note: Refer below table for the optional component list & Features_ 

**Pricing Model for Premium Solution:** 

Prices are calculated by Considering Location as **East US** and Pricing Model as **“PAYG”.** 

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
| **ML Services**  | Model Management (Preview), Dev/Test, Location East US2(Managed models=20, managed deployments=2, Available Cores=4) | PAYG     | $0.00
| **Machine learning studio**   | Free    | PAYG   | $0.00
| **Application Insights**  | 6 GB, 5 GB of data is included for free. 1 GB * $2.99  | PAYG  | $2.99 
| **Log Analytics**    | 6 GB, 5 GB of data is included for free. 1 GB * $2.30  | PAYG  | $2.30
| **Virtual machine (edgevm)(optional)**   | Standard, A2:2 core(s), 3.5 GB memory, 135GB Temporary storage, East US, Linux VM   | PAYG   | $87.60 
| **Virtual network**     | 5GB Data transfer, Outbound data transfer 1*US$ 0.0100 per GB, Inbound data transfer 1*US$0.0100 per GB, East US | PAYG                             | $0.10
| **Virtual machine(modbusvm)(Optional)**   | 1 A1 (1 vCPU(s), 1.75 GB RAM) x 730 Hours; Windows – (OS Only); Pay as you go; 0 managed OS disks – S4    | PAYG    | $65.70 
| **Virtual machine (MLVM)(Optional)**     | Standard, D2 v3:2 vCPU(s), 8GB RAM, 50GB Temporary storage, US $ 0.117/hour, East US, windows VM  | PAYG                             | $137.24  
|      |       | **Estimated monthly cost**       | **$391.69**
|      |       | **Optional Cost**       | **$290.64**
|              |                               | **Estimated Monthly Cost (Including Optional)**                 | **$682.33**               


## 3.2 Cost Comparison: 

In this section we will be comparing the cost for all the solution provided in terms of Features & $ Impact:

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
|ML Services	    | Feature	          | Model Management (Preview)        | Model Management (Preview)	       | Model Management (Preview)
|                  | Tier	        | Dev/Test	        | Dev/Test	       | Dev/Test
| Machine learning studio       | SKU	       | Free	       | Free	       | Free
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
| Virtual machine (MLVM) (Optional)	        | SKU	              | Standard, D2 v3	       | Standard, D2 v3	        | Standard, D2 v3
|                  | Cores	           | 2 vCPU(s)	        | 2 vCPU(s)	          | 2 vCPU(s)
|                 |	RAM           | 8 GB	       | 8 GB	      | 8 GB
|                | Storage	          | 50 GB	          | 50 GB	            | 50 GB
|	            |OS           |	Windows VM	              | Windows VM	                  | Windows VM
| Virtual machine (modbusvm) (Optional)        | SKU	           | Standard	            |Standard, D2 v3	        | Standard, D2 v3
|                      | Cores	            | 1 Core	          | 2 vCPU(s)	               | 2 vCPU(s)
|                    | RAM	               | 1.75 GB	            | 8 GB	               | 8 GB
|	                   | Storage	            | 70 GB	            | 50 GB	                 | 50 GB
|	                    | OS	           | Windows VM	             | Windows VM	              | Windows VM


### 3.2.2 Dollar Impact: 

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
| **ML Services**	| $0.00 	 | $0.00     | $0.00 
| **Machine learning studio**    | $0.00      | $0.00     | $0.00 
| **Application Insights**    | $2.99 	  | $2.99 	   | $2.99 
| **Log Analytics**   | $2.30      | $2.30 	    | $2.30 
| **Virtual machine (edgevm) (Optional)**    | $87.60 	    | $87.60     | $87.60 
| **Virtual network (Optional)**    | $0.10 	  | $0.10 	     | $0.10 
| **Virtual machine (MLVM) (Optional)**    | $137.24 	  | $65.70 	    | $65.70 
| **Virtual machine (modbusvm) (Optional)**    | $65.70 	  | $137.24    | $137.24 


### 3.2.3. Estimated Monthly Cost for each Solution:

| **Resource Name**                 | **Basic**           | **Standard**            | **Premium**
| -------------                  | -------------          | -------------            | ------------- 
| **Estimated monthly cost**     | **$105.67**            | **$301.21** 	              | **$391.69** 
| **Optional Cost**      | **$290.64** 	       | **$290.64** 	              | **$290.64** 
| **Estimated monthly cost (Including Optional)**       | **$396.31** 	       | **$591.85** 	              | **$682.33** 


## 4. Further Reference Documents

### 4.1. Deployment Guide

To Deploy the Oil & Gas solution please refer Deployment guide in the Provided Documents.

https://github.com/sysgain/iot-edge-dynocard/wiki/Deployment-Guide

### 4.2. Admin Guide

Refer Admin Guide to perform Administrator tasks of the Solution in Azure Portal.

https://github.com/sysgain/iot-edge-dynocard/wiki/Admin-Guide-Page

### 4.3. User Guide

Refer User Guide to find how to use Oil & Gas solution once deployed successfully in Azure portal. 

https://github.com/sysgain/iot-edge-dynocard/wiki/User-Guide	