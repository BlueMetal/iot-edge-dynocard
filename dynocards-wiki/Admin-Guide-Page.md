## 1. Admin Guide

This Document explains how to use the solution. In addition to the user document, this provides verifying data in resources, updating SKUs, Securing steps for the resources and performing DR activities for Standard and Premium solutions. 

## 2. Starting Stream Analytics job

1.Go to **Resource group** -> click on **StreamAnalytics26hs3**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/39.png)

2.After clicking on stream analytics job, you will be directed to below screen.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/40.png)

**Note**: Before **Starting** the Stream analytic job, first you need to **Renew the authorization**of the output Data Lake.

3.Click on **Datalake** in the output section it will navigate the output details page.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/41.png)

4.Click on **Renew authorization** button and click **save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/42.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/43.png)

5.Now, you can click the **start** button to start the Stream analytics job and you can see the **Starting Streaming job** message in notifications window.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/44.png)

6.Now you can see the Stream analytics job is **Running**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/45.png)

## 3. Check modbus logs

Login to **iotEdge** VM:

1.Go to -> **Resource group** -> Click on **iotEdge** VM

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/92.png)

2.After clicking on iotEdge VM, you will be directed to iotEdge overview page. Here Copy the **iotEdge VM Public IP address**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/93.png)

3.Now Open  **Putty** and Paste the  **Public IP addres** of  **iotEdge** VM at Host Name (or IP address) then click open.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/94.png)

4.Then click **Yes** to continue.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/95.png)

5.Enter the credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/96.png)

6.Once login successful. Run the below command for root privilege.

**sudo -i**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/97.png)

7.Run the below command to check the data logs that are coming from Modbus simulator.

**docker logs modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/98.png)

8.Modbus container logs show the data that is coming from modbus simulator.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/99.png)

## 4. Verify Messages in IoT Hub

1.Go to Resource group -> click on iothub26hs3.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/100.png)

2.Go to overview section and scroll down to check the IoT Hub messages.

Here you can confirm the data is coming to IoT Hub.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/101.png)

### 4.1. Update IoT Hub SKU [Administrative Task]

Here we can change the IoT Hub SKU tier free to standard, because the number of messages will send to logic app. 

1.Navigate to pricing and scale blade. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/Iothub1.png)

2.Here you can see the standard tier of the IoT Hub, it will receive up to 400000 messages daily. Logic app will receive up to 400000 daily messages. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/Iothub2.PNG)

3.Go to IoT Hub endpoints And verify that the endpoint status is healthy. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/Iothub3.PNG)

## 5. Verify Data in Azure SQL Database 

### 5.1. Login to the SQL Server to check the data. 

1.Go to **Resource group** -> Click **db4cards** SQL database and copy the server name.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/103.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/104.png)

2.Open SQL Server Management studio and paste the server URL.

3.Enter the Credentials:

Login_ID: **adminuser**

Password: **Password@1234**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/105.png)

**Note**: Before login to SQL database you need to add firewall settings.

4.For adding firewall settings, Go to **resource Group** ->click on **oilgasserver26hrs3**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/106.png)

5.Click on **Show Firewall Settings** from the **Firewalls and virtual networks**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/107.png)

6.Click **+ Add client IP** to add the client IP address.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/108.png)

7.As shown below add the client IP address form the **Firewall and virtual networks** and click on **save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/109.png)

8. Go back to SQL Server management studio and click on connect. 
9. After connecting to the server, expand the tables and views inside the **db4cards** database as shown below. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/110.png)

10. Check the data in each table. 

**select * From [ACTIVE].[CARD_DETAIL]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/111.png)

**select * From [ACTIVE].[CARD_HEADER]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/112.png)

**select * From [ACTIVE].[DYNO_CARD]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/113.png)

**select * From [ACTIVE].[EVENT]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/114.png)

**select * From [ACTIVE].[EVENT_DETAIL]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/115.png)

**select * From [ACTIVE].[PUMP]**

**select * From [STAGE].[PUMP_DATA]**

**select * From [STAGE].[SURFACE_DATA]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/116.png)

11.Select data from **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

12.Right click on [ACTIVE].[DYNO_CARD_ANOMALY_VIEW] --> script view as -->select to -->New Query Editor window.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/117.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/118.png)

### 5.2. Login to Edge sql container server to check the data

1.Login to edge sql Container using SQL Server management studio.

2.Copy the Edge VM IP Address:**[104.42.121.3]** and give port **1401**.

3.Enter the Credentials:

EX: **104.42.121.3,1401**

User_ID: **sa**

Password: **Strong!Passw0rd**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/119.png)

4. After connecting to the server, expand the tables and views inside the db4cards database as shown below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/120.png)

5.Check the data in tables.

**select * From [ACTIVE].[CARD_DETAIL]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/121.png)

**select * from [ACTIVE].[CARD_HEADER]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/122.png)

**select * from [ACTIVE].[DYNO_CARD]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/123.png)

**select * from [ACTIVE].[EVENT]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/124.png)

**select * from [ACTIVE].[EVENT_DETAIL]**

**select * from [ACTIVE].[PUMP]**

**select * from [STAGE].[PUMP_DATA]**

**select * from [STAGE].[SURFACE_DATA]**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/125.png)

6.Select data **from [ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

7.Right click on [ACTIVE].[DYNO_CARD_ANOMALY_VIEW] --> script view as -->select to -->New Query Editor window.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/126.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/127.png)

### 5.3. Update Azure SQL SKU [Administrative Task]

1. Go to Resource Group -> click db4cards it will be navigated to the overview page. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sqlsku1.PNG)

2. Navigate to configure blade. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sqlsku2.PNG)

3. Here we can change the pricing tier. 

**Note:** Service tiers are differentiated by a range of performance levels with a fixed amount of included storage, fixed retention period for backups, and fixed price. All service tiers provide flexibility of changing performance levels without downtime. Single databases and elastic pools are billed hourly based on service tier and performance level. 

DTUs are most useful for understanding the relative amount of resources between Azure SQL Databases at different performance levels and service tiers. For example, doubling the DTUs by increasing the performance level of a database equates to doubling the set of resources available to that database. For example, a Premium P11 database with 1750 DTUs provides 350x more DTU compute power than a Basic database with 5 DTUs. 

In this solution we configured **Standard tier**, we are selected 10 DTUs and 10GB of the data size. 

4. Click **Standard** and select **10-DTUs** and **10GB** of data size and click **Apply** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sqlsku3.PNG)

## 6. Verify run history of Logic App

1.Go to **Resource group** -> click on **logicapp26hs3** logic app.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/128.png)

2.Go to overview page of Logic app to see the Runs history. You can see status as Succeeded.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/129.png)

3.Click on any one of the Succeeded run.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/130.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/131.png)

4.Click on duration of the run **41s**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/132.png)

5.Click on show raw inputs to view the Uri, method and body.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/133.png)

## 7.  Verify data in Data Lake Store

1.Go to **Resource Group** -> click on **datalakestore26hs3** data lake store.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/134.png)

2.Navigate to **Data  explorer** blade.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/135.png)

3.Click on **dynocards**  folder to see the data.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/136.png)

4.Click on the JSON file to open it.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/137.png)

5. As per above diagram, we can confirm that data is coming to data lake store. 

### 8. Verify messages in Service Bus Namespace

1.Go to **Resource group** --> click on **snamespace26hs3** service bus namespace

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/138.png)

2. Go to overview page of service bus to see the successful requests, Incoming and outgoing messages graph.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/139.png)

### 8.1 Update SKU for Service Bus Name Space [Administrative Task] 

1. Go to Resource Group -> click on service bus namespace 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sbussku1.PNG)

2. Navigate to scale blade. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sbussku2.PNG)

3. Here we can change the **messaging tier**, in this solution we are using **standard tier.** 

Note: **Standard Pricing tier** provides better CPU performance and IOPS than basic tier. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sbussku3.PNG)


## 9. Verify events in Stream analytics job

1.Go to Resource group--> click on StreamAnalytics26hs3 stream analytics.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/140.png)

2.Now you can check input events and output events graph under overview section

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/141.png)


## 10. Web App Auto Scaling

1. Go to Resource Group -> click on **App Service Plan** 

2. Go to **Overview** page of the app service plan and see the **App Service Plan is Free.** In this solution we are using free tier. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku1.PNG) 

3. In free tier app service plan **auto scale** is not available. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku2.PNG) 

### 10.1 Update Web API SKU [Administrative Task] 

We can change the app service plan from free to basic/standard. 

1. Navigate to Scale up (App service Plan) blade, select **standard S1 tier** and click on **Apply.** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku3.PNG)

2. After updating app service plan from free to standard.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku4.PNG)

3. In **Overview page** you can see the updated app service plan tier **Standard: 1 small.**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku5.PNG)

**Note:** Auto scale allows you to have the right amount of resources running to handle the load on Application. 

### 10.2. Enable Auto Scale 

1. Click on scale out in the left side menu and click **Enable auto scale** button. This activates the editor for scaling rules. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku6.PNG)

2. Provide a name for the scale setting, and the click on **"Add a rule".** Notice the scale rule options that opens as a context pane in the right-hand side.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku7.PNG)

3. Click on **+ Add a rule** it will open the **Scale rule** popup window and change the metric source at the top to **Current resource**, time aggregation: **Average**, metric name: **CPU Percentage**, time grain statistic: **Average**, operator: **Greater then**, Threshold: **70** and duration: **10 min** under **Criteria** section. 

Enter operation: **Increase count by**, instance count: **1** and cool down: **5 min** under **Action** section and click **Add**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku8.PNG)

4. Add a **Scale rule** that will **scale in** and enter operator: **Less then**, threshold: **30 **and Duration (in minutes) as **10 **under **Criteria **section. Add operation: **Decrease count by**, instance count: **1**, cool down: **5 min** under **Action **section and click **Add**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku9.PNG)

5. Set the instance limits. if you want to scale between 1-2 instances depending on the custom metric fluctuations, set 'minimum' to '1', 'maximum' to '2' and 'default' to '2'. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku10.PNG)

**Note**: In case there is a problem reading the resource metrics and the current capacity is below the default capacity, then to ensure the availability of the resource, Auto scale will scale out to the default value. If the current capacity is already higher than default capacity, Auto scale will not scale in. 

6. Click on **'Save'** 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/apisku11.PNG)

## 11.Configuring Power BI

Before configuring Power BI, you need to download and install the power BI Desktop application.
You can perform the below steps in Power BI Desktop:

### 11.1. Download dynoCardVisuals.pbiviz

1.Download dynoCardVisuals.pbiviz file from the below link
https://github.com/BlueMetal/iot-edge-dynocard/tree/master/dynoCardVisuals/dist 

### 11.2. Import DynoCard custom from visuals 

2.Open Power BI Desktop and select the ellipses from the bottom of the Visualizations pane.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/142.png)

3.From the drop-down, select Import from file.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/143.png)

**Caution: Import Custom visual** --> dialog box will appear. Click Import to confirm.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/144.png)

4.From the Open file menu navigate to **dynoCardVisuals.pbiviz** file and then select Open. The icon for Dyno Card custom visual will be added to the bottom of Visualizations pane and it's now available for use.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/145.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/146.png)

#### 11.3. Generating and publishing Power BI reports

5.In the top left-hand side of Power BI Desktop, select 'Get Data' -> 'SQL Server database' and click on **connect**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/147.png)

6.Fill in the database server information and click **'OK'**. 

•   Server: xxxx.database.windows.net

•   Database: db4cards

•   Database Connectivity Mode: Import


![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/148.png)

7.Select 'Database' on the left-hand side of the connection window, fill in the database connection information and click 'Connect'.

    •   Username: <<user name>>
    •   Password: <<password>>

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/149.png)

8.Select 'ACTIVE.DYNO_CARD_ANOMALY_VIEW' from the navigation list and click load.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/150.png)

9.Add the Dynocard Visualization to the report by clicking on the icon and then map the imported fields accordingly. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/151.png)

10.Drag and drop all the fields respectively.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/152.png)

### 11.4.  Power BI report 

11.Now reports can be observed like below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/153.png)

## 12. Monitoring Components

### 12.1 Application Insights

1. Go to **Azure portal**, select your **Resource Group** and select **Application Insights** as shown below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/212.png)

2. On **Overview** page, Summary details are displayed as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/213.png)
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/214.png)    

3. Here you can see the **Live Requests** of the Web App as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/215.png)

4. Go back to **Application Insights**, in the Overview page **click Analytics icon** in the Health section as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/216.png)

5. The Application Insights page is displayed and **click Home Page** tab.  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/217.png)

You can **run** the following common queries **to see** the **specific logs** of application as below.

For example, **Run** the Users query to see the logs.

6. Click **RUN** in Users block as highlighted in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/218.png)

7. You can see the below **graph** of application users. If you modify the query please click RUN from top left menu to see the updated graph.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/219.png)

8. Similarly, you can run the other common Pre-defined queries by navigating back to the **Home Page**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/220.png)

9. Go back to the **Application Insights** overview page in Azure Portal to view **metrics** of the application.

10. Click **Metrics Explorer** on the left side of the page as shown below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/221.png)

11. **Click Edit** as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/222.png)

12. You can select any of the **listed Metrics** to view application logs.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/223.png)

13. If you want to add new chart **click Add new chart** as shown in the following figure and **click Edit** to add the specific metrics.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/224.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/225.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/226.png)

## 12.2 OMS Log Analytics

1. Open **Azure portal** -> **Resource group** -> Click the **OMS Workspace** in Resource Group to view OMS Overview Section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/227.png)

2. Click **Azure Resources** on left side menu to view available Azure Resources.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/228.png)

3. Select your **Resource Group** name from the dropdown list.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/229.png)

4. Access **OMS portal** from OMS Workspace from the left side menu. Click on **OMS Workspace** -> **OMS Portal**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/230.png)

5. Once you click OMS Workspace, OMS Home Page is displayed, where you can see the **Logs of Azure SQL and Azure Web Apps** by clicking on each tab.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/231.png)

6. Click **Azure SQL Analytics** to view the SQL Server logs.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/232.png)

7. Click **sqldb** under DATABASE column, as shown in the following figure to view the detailed information.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/233.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/234.png)

8. Click **Home icon** on the left side of the page for **Web Apps Analytics**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/235.png)

9. Click **Logic App Management (Preview)** to view each web application logs as highlighted in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/236.png)

10. Click **Succeeded** in the Logic App Management (Preview).

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/237.png)

11. The **Succeeded logs** are displayed as shown in the following figure.              

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/238.png)

12. **Click Failed** in the Logic App Management (Preview) page as highlighted in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/239.png)

13. The **Failed logs are displayed** as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/240.png)

14. **Click Running** in the Logic App Management (Preview) page as highlighted in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/241.png)

15. **The Running logs are displayed** as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/242.png)

16. **Click All Status** in the Logic App Management (Preview) page as highlighted in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/243.png)

17. The **All Status logs** are displayed as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/244.png)

18. **Click Search tab** to search the IoT hub, Azure Data Lake Store, Service Bus Namespace, SQL database logs.

19. **Click Show legacy** language converter.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/245.png)

20. **Copy IoT Hub resource name**, paste it in the Converter box and **click RUN**.

The IoT Hub information is displayed below the page as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/246.png)

21. **Copy Data Lake Store** resource name, paste it in the Converter box and click RUN.

The **data lake store** information is displayed as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/247.png)

22. **Copy Service Bus Namespace** resource name, paste it in the Converter box and click RUN.

The Service bus namespace information is displayed in the below page as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/248.png)

23. **Copy SQL database** resource name, **paste it in the Converter box** and click RUN.

The **SQL database information is displayed** below the page as shown in the following figure.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/249.png)

## 13 Hardening Components

### 13.1 SQL Geo Replication Verification

1.Login to **Azure Portal**, go to your **Resources Group** and Open the **SQL Database**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/250.png)

2.Select **Geo-Replication** under the **SETTINGS** section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/251.png)

In the initial stage, the Global Distribution map will highlight the **Primary region**, which is the origin of the SQL database.

3.Now choose a **new region** from global distribution map which will act as secondary region.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/252.png)

Now you can add **secondary region** by simply clicking **Add New Region** button which is available under the **Secondary region section**.

But here already one region is added, so you can do Failover to that region.

4.Now you can add secondary region by simply clicking Add New Region button which is available under the Secondary region section.  

5.Navigate to **Failover Group** blade under the Settings section in the **SQL server** resource, you created failover group.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/sqlgeo1.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/253.png)

6.Click on **Failover**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/254.png)

7.Here click **yes** then you can switch all the secondary database to the primary role. All TDS sessions will be disconnected. All new TDS sessions will be automatically re-routed to the secondary server, which now becomes primary server.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/255.png)

8.Once the Geo-Replication is done, you can go back and check all your regions on the Map, further you can add or remove any other regions as per your need.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/256.png)

### 14. Securing Solution Azure Components
### 14.1.   Data Lake Store Account
Data Lake Store can be secured with below following options.
* Encryption
* Firewall

1. Go to **Data Lake Store account** from the Resource Group. An overview page will be displayed.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/257.png) 

### 14.1.1. Encryption
1. Go to **Encryption** option from left menu bar. As a security best practice, we have enabled encryption of storage account from ARM Template deployment.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/258.png) 
 
### 14.1.2. Firewall
Firewall can be configured to allow access from different IP ranges. We can restrict users to access data lake store account from unwanted IP ranges.
1. Go to **Firewall** option from left menu.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/259.png) 
 
2. Click on **ON** to **Enable Firewall** and make sure to select **Allow access to Azure services** to **ON**. click on **Save**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/260.png) 

3. Once firewall configuration completed successfully, a popup will be opened as below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/261.png) 

We can also add specific IP ranges to allow access to Data Lake Store account. Provide below parameters and click on **Save**.   
* RULE NAME
* START IP
* END IP
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/262.png) 

### 14.2.   IoT Hub
IoT Hub can be secured with below settings.
* Shared Access Policies
* IP Filter

### 14.2.1. Shared Access Policies
IoT Hub authorization can be achieved using shared access policies. Shared access polices will be helpful to authenticate IoTHub using endpoints.
1. Go to **Resource Group** and click on **IoTHub**. An overview page will be displayed. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/263.png) 

2. Click on **Shared access policies** and below shared access policies will be displayed.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/264.png) 

Each shared access policy has its own policy. Each role has its own permissions to IoT hub.
3. Clicking on each policy provides connection strings. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/265.png)


### 14.2.2. IP Filter
If **IP Filter** table is empty or no rule matches, the connection is accepted. Rules are applied in order: the first matching rule decides the action. To change the order of the IP filter rules, hover over the row to drag and drop to the desired location in the grid.
1. Go to **IP Filter** option from left menu.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/266.png) 

2. Click on **+Add**
3. Provide below required parameters and click on **Create**. 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/267.png) 

    Rule Name       : A unique string
    Action          : Accept or reject
    IP Address Range    : IP address range that must accept or reject.      

4. Click on **Save**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/268.png) 

### 14.3.   Service Bus Name Space

### 14.3.1. Shared Access Policies
Shared access policies will be helpful to authenticate service bus name space queue. To access service bus queue from service bus explorer, these shared access key will be helpful. To get shared access policies go to resource group and click on **service bus name space**. An overview page will be displayed.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/269.png) 
 
1. Click on **shared access policies**.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/270.png) 

2. Clicking on **RootmanageSharedAccessKey** provides connection string that can used to authenticate service bus name space.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/271.png) 


### 14.4.   SQL Database

SQL Database can be secured using below sections.

### 14.4.1. Enable Firewall
To restrict access from unwanted network azure SQL supports firewall settings.
1. To configure azure firewall settings, click on **Resource Group** and click on **SQL Database**. An Overview page will be displayed. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/272.png) 

2. Click on **Set Server Firewall**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/273.png) 

3. Click on **+Add Client IP** and click on **Save**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/274.png) 

We can also add specific range of IP address by providing rule name, start IP and end IP. Only IPs white listed in Firewall will be able to access database.

### 14.4.2. SQL Database Authentication
SQL Database is accessible using the SQL credentials provided during deployment. In addition to those SQL is also accessible using AD credentials.
1. To configure AD credentials, go to Overview page of **SQL Server**. Click on resource group and choose **SQL Server**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/275.png) 

2. Navigate **Active directory admin** blade.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/276.png) 

3. Click on **set admin**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/277.png) 

4. Choose user and click on **Select**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/278.png) 

5. Click on **Save**.
Now Database is accessible from active directory credentials.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/279.png) 


6. Now Database is accessible with AD Credentials.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/280.png) 

### 14.4.3. Advanced Threat Protection
Advanced Threat Protection costs --/server/month. It includes Data Discovery & Classification, Vulnerability Assessment and Threat Detection.
1. Click on **Resource Group** and select **SQL Server**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/281.png) 

2. Click on **Advanced Threat Protection** under **Security** in left side menu.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/282.png) 


3. Enable **Advanced Threat Protection** and provide email Address of users whom alert should trigger. Choose Storage account details and threat detection types and Click on **Save**. 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/283.png) 

4. SQL Supports below detection types.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/284.png) 

### 14.4.4. Auditing
You can use SQL database auditing to:
**Retain** an audit trail of selected events. You can define categories of database actions to be audited.
**Report** on database activity. You can use preconfigured reports and a dashboard to get started quickly with activity and event reporting.
**Analyze** reports. You can find suspicious events, unusual activity, and trends.
1. Go to **auditing** option from left hand menu bar. Click on **ON** and Choose storage account details and click on **Save**.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/285.png) 
 

### 14.4.5. Transparent Data Encryption
Transparent data encryption (TDE) helps protect Azure SQL Database and Azure Data Warehouse against the threat of malicious activity. It performs real-time encryption and decryption of the database, associated backups, and transaction log files at rest without requiring changes to the application. By default, TDE is enabled for all newly deployed Azure SQL Databases. TDE cannot be used to encrypt the logical master database in SQL Database. The master database contains objects that are needed to perform the TDE operations on the user databases.
Transparent data encryption uses a service-managed key. Azure will automatically generate a key to encrypt your databases, and manage key rotations

1. Navigate to **Transparent data encryption** blade of the SQL server.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/286.png) 

2. To use own key, click on **Yes**. Choose from key vault or enter key identifier manually. And click on **Save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/287.png) 

### 15. Performing DR Activities

### 15.1.   Standard Solution type
In this scenario, there is again a primary and a secondary Azure region. All the traffic goes to the active deployment on the primary region. The secondary region is better prepared for disaster recovery because the database is running on both regions. 

Only the primary region has a deployed cloud service application. Both regions are synchronized with the contents of the database. When a disaster occurs, there are fewer activation requirements. You redeploy azure resources in the secondary region.

Redeployment approach, you should have already stored the service packages. However, you don’t incur most of the overhead that database restore operation requires, because the database is ready and running.  This saves a significant amount of time, making this an affordable DR pattern.

Standard Solution requires redeployment of azure resources in secondary region when there is primary region is down.

When user chooses Standard Solution type below azure resources will be deployed in primary region and SQL in both Regions. 

* 3-Virtual machines (2 windows & 1 Linux)
        Windows VMS: Dyno card VM which install pre-installed software's          for dyno card VM. 
        ML VM which install pre-installed Docker.
        Linux VM: Edge VM is used to create IoT Edge device and installs modules in IoT Edge device
* 1-Web App
* 1-Function App
* 1-Application Insights
* 1-Data Lake Storage
* 1-IoT HUB
* 1-Log analytics
* 1-Logic app
* 1-service bus namespace
* 2-SQL database
* 2-Storage account
* 1-Stream Analytics job
* 1-Machine Learning Experiment Account
* 1-Machine Learning Management Account
* 1-Traffic Manager

When there is primary region is down, and user needs to redeploy azure resources. When redeployment is completed below resources will be deployed.
* 1-Web App
* 1-Data Lake Storage
* 1-Logic app
* 1-service bus namespace
* 1-Stream Analytics job

### 15.1.1. Stop Simulator
1. Go to Resource Group > Click on **dynocardVM** and in overview page copy the public ip of the VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/288.png) 

2. Login to the **Dynocard** VM with the credentials provided at ARM template deployment. Close running simulator.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/289.png) 


### 15.1.2. IoTHub Manual Failover
1. Go to **Resource Group** ->  Click on **IoTHub**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/290.png) 

2. Go to **Manual Failover (Preview)** from left side menu.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/291.png) 

3. Click on **Initiate failover** to initiate manual failover of IoTHub.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/292.png) 

When failover process started, a pop up will be displayed on right top corner. Once Manual Failover process completed, Primary Location and secondary location will interchange.
## Note:
This process will take around 15 mins. To initial failover again, user needs wait for 1 hour to run failover again.

### 15.1.3. SQL Database Manual Failover

1. Go to **Resource Group** and choose Primary **SQL Server** from list.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/293.png) 

2. Click on **Failover groups** on left hand menu.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/294.png) 

3. Click on **Failover Group**. 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/295.png) 

A page will be displayed as below. Click on **Failover** and click on **Yes** for confirmation.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/296.png) 

A pop up will be displayed which shows progress of failover. Once failover is completed Primary Role and secondary role will interchange.

### 15.1.4. Stop Stream Analytics Job in Primary Region
1. Go to **Resource Group** and click on primary **Stream Analytics job**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/297.png) 

2. Stop the Stream analytics job by click on **Stop** and click on **Yes** for confirmation.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/298.png) 

### 15.1.5. Redeploy Azure Resources
1. Click on **Add** in existing resource group and re-deploy the ARM template. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/299.png) 

### 15.1.6. Update IoTHub Device Connection String in Web API
1. Click on **IoT Hub Edge Device** and click on device and copy the primary key and past to the second region of the web application settings.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/300.png) 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/301.png)  

### 15.1.7. Start Stream analytics Job
1. Go to **Resource Group** and click on Secondary **Stream analytics job**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/302.png)  

2. After click on stream analytics job, you will have directed to screen like below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/303.png)  

**Note:** Before **Starting** the Stream analytic job, first you need to **Renew the authorization** of the **output DataLake**.
3. Click on **Datalake** in the output section it will navigate the output details page.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/304.png)  

4. Click on **Renew authorization** button and click *Save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/305.png)  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/306.png)  

5. Start the Stream analytics job by click on **Start** and click on **Start** for confirmation.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/307.png)  


### 15.1.8. Update Service Bus Endpoint in Logic App in Secondary Region
1. Go to **Resource Group** and click on secondary region **logic app**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/308.png)  

2. On **Overview** page click on **Edit**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/309.png)  

3. Click on **“When a message is received in a queue (auto-complete)”.** And Click on **Change Connection**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/310.png)  

4. Click on **“+ Add new connection”**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/311.png)  


5. Enter **Connection Name** and choose Service bus name space which is in primary region. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/312.png)  

6. Choose **RootMangedSharedAccessKey** and click on **Create**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/313.png)  

7. Once creation is completed. Click on **Save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/314.png)  

### 15.1.9. Start Simulator in dynocardVM
1. Go to **Resource Group** -> Click **dynocardVM**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/315.png)  

Now it will direct to **overview** page of the **dynocardVM**. 
2. **Copy** the **public IP address**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/316.png)  

3. Open **Remote Desktop Connection** in your local computer and **past** the copied **dynocardVM** Public IP address.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/317.png)

4. Use the below Credentials for login to the VM.
    User ID: **adminuser**
    Password: **Password@1234**
Then Click **Ok**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/318.png)  

5. Now Click **Yes** button to continue.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/319.png)  

6. It should open the **dynocardVM** and check the downloaded software.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/320.png)  

7. Run the v2 version of **ModRSsim2 simulator** by double clicking it.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/321.png)  
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/322.png)  

8. Click on **Animation setting** icon, which is located at right top menu bar.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/323.png)  

9. Select the **"Training PLC Simulation"** option and browse the **DynoCard.vb** script in downloaded section of the VM.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/324.png)  
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/325.png)  
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/326.png)  

10. Select **Analog Inputs 300000**.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/327.png)   
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/328.png)  

11. Now Close the v2 simulator window and then open the v1 simulator, which should be installed in the C:\Program Files (x86)\EmbeddedIntelligence\Mod_RSsim directory.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/329.png)  

12. Click on the same **Animation** setting icon, which is located at right top menu bar to verify the DynoCard.vbs script is selected or not.
If it’s not selected, then select the "Training PLC Simulation" option and browse the DynoCard.vbs script in downloaded section of the VM.
13. Now Select **Analog Inputs 300000** then you can observe the received/sent messages count. Which is highlighted in below screen.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/330.png)  

### 15.1.10.    Verify Triggers in Logic App in Secondary Region
1. Go to **Resource Group** and click on Secondary **logic app**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/331.png)  

2. Scroll down to bottom of the page on **Overview** section to check logic app is running successfully or not. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/332.png)  

### 15.1.11.    Verify Data in SQL Database
1. Login to **SQL Database** using **SQL Management studio** and verify whether data is getting stored in database.  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/333.png)  

### 15.1.12.    Verify Data in Power BI
1. Open **Power BI Desktop** and click on Refresh to see updated graphs.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/334.png)  


### 15.2.   Premium Solution type
Both the primary region and the secondary region have a full deployment. This deployment includes the cloud services and a synchronized database. However, only the primary region is actively handling network requests. The secondary region becomes active only when the primary region experiences a service disruption. In that case, all new network requests route to the secondary region. Azure Traffic Manager can manage this failover automatically.
Failover occurs faster than the database-only variation because the services are already deployed. This topology provides a very low RTO. The secondary failover region must be ready to go immediately after failure of the primary region.
For the fastest response time with this model, you must have similar scale (number of role instances) in the primary and secondary regions. 
When user chooses premium as solution type below azure resource will be deployed in both regions.
* 3-Virtual machines (2 windows & 1 Linux)
        Windows VMS: Dyno card VM which install pre-installed software's          for dyno card VM. 
        ML VM which install pre-installed Docker.
        Linux VM: Edge VM is used to create IoT Edge device and installs modules in IoT Edge device
* 2-Web App
* 1-Azure Function
* 1-Application Insights
* 2-Data Lake Storage
* 1-IoT HUB
* 1-Log analytics
* 2-Logic app
* 2-service bus namespace
* 2-SQL database
* 2-Storage account
* 2-Stream Analytics job
* 1-Machine Learning Experiment Account
*  1-Machine Learning Management Account
* 1-Traffic Manager

### 15.2.1. Stop Simulator
1. Go to **Resource Group** > Click on **dynocardVM** and in Overview page copy the public ip of the VM.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/335.png)  

2. Login to the **Dynocard** VM with the credentials provided at ARM template deployment. Close running simulator.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/336.png)  

### 15.2.2. IoTHub Manual Failover
1. Go to **Resource Group** and Click on **IoTHub**.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/337.png)  

2. Go to **Manual Failover (Preview)** from left side menu.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/338.png)  

3. Click on **Initiate failover** to initiate manual failover of IoTHub.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/339.png)  

When failover process started, a pop up will be displayed on right top corner. Once Manual Failover process completed, Primary Location and secondary location will interchange.
## Note: 
This process will take around 15 mins. To initial failover again, user needs wait for 1 hour to run failover again.

### 15.2.3. SQL Database Manual Failover
1. Go to **Resource Group** and choose Primary SQL Server from list.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/340.png)  

2. Click on **Failover groups** on left hand menu.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/341.png)  


3. Click on **Failover Group**. 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/342.png)  

4. A page will be displayed as below. Click on **Failover** and click on **Yes** for Confirmation.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/343.png)  

A pop up will be displayed which shows progress of failover. Once failover is completed Primary Role and secondary role will interchange.

### 15.2.4. Stop Stream Analytics Job in Primary Region
1. Go to **Resource Group** and click on primary **Stream Analytics job**.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/344.png)  

2. Stop the Stream analytics job by click on **Stop** and click on **Yes** for confirmation.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/345.png)  

### 15.2.5. Start Stream analytics Job
1. Go to **Resource Group** and click on Secondary **Stream Analytics job**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/346.png)  

2. After click on stream analytics job, you will have directed to screen like below.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/347.png)  

## Note: 
Before **Starting** the Stream analytic job, first you need to **Renew the authorization** of the output DataLake.
3. Click on **Data lake** in the output section it will navigate the output details page.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/348.png)  

4. Click on **Renew authorization** button and click **save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/349.png)  
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/350.png)  
 
5. Start the Stream analytics job by click on **Start** and click on **Start** for confirmation.
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/351.png)  

### 15.2.6. Update Service Bus Endpoint in Logic App in Secondary Region
1. Go to **Resource Group** and click on secondary region **logic app**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/352.png)   

2. On **Overview** page click on **Edit**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/353.png)  

3. Click on **“When a message is received in a queue (auto-complete)”.** And Click on **Change Connection**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/354.png)   

4. Click on **“+ Add new connection”**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/355.png)   

5. Enter **Connection Name** and choose **Service Bus Namespace** which is in **Primary region**. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/356.png)  

6. Choose **RootMangedSharedAccessKey** and click on **Create.**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/357.png)  

7. Once creation is completed. Click on **Save.**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/358.png)  

### 15.2.7. Start Simulator in dynocardVM.
1. Go to **Resource Group** -> Click **dynocardVM.**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/359.png)  

Now it will direct to **overview** page of the **dynocardVM**. 
2. **Copy** the **public IP address**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/360.png)  

3. Open **Remote Desktop Connection** in your local computer and **past** the copied **dynocardVM** Public IP address.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/361.png)  
 
4. Use the below Credentials for login to the VM.
User ID: **adminuser**
Password: **Password@1234**
5. Then Click **Ok**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/362.png)  

6. Now Click **Yes** button to continue.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/363.png)  

7, It should open the **dynocardVM** and check the downloaded software.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/364.png)  

8. Run the v2 version of **ModRSsim2 simulator** by double clicking it.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/365.png)  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/366.png)  

9. Click on **Animation setting** icon, which is located at right top menu bar.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/367.png)  

10. Select the **"Training PLC Simulation"** option and browse the **DynoCard.vbs** script in downloaded section of the VM.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/368.png)  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/369.png)  

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/370.png)  
 
11. Select **Analog Inputs 300000.**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/371.png)  
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/372.png)

12. Now Close the v2 simulator window and then open the v1 simulator, which should be installed in the C:\Program Files (x86)\EmbeddedIntelligence\Mod_RSsim directory.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/373.png)  

13. Click on the same **Animation setting** icon, which is located at right top menu bar to verify the DynoCard.vbs script is selected or not.
If it’s not selected, then select the "Training PLC Simulation" option and browse the DynoCard.vbs script in downloaded section of the VM.
Now Select **Analog Inputs 300000** then you can observe the received/sent messages count. Which is highlighted in below screen.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/374.png)  
 
### 15.2.8. Verify Triggers in Logic App in Secondary Region
1. Go to **Resource Group** and click on **Secondary logic app.**
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/375.png)  

2. Scroll down to bottom of the page on **Overview** section to check logic app is running successfully or not. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/376.png)  

### 15.2.9. Verify Data in SQL Database

1. Login to **SQL Database** using **SQL Management studio** and verify whether data is getting stored in database. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/377.png)  

### 15.2.10.    Verify Data in Power BI
1. Open **Power BI Desktop** and click on Refresh to see updated graphs.
 
![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/378.png)  
