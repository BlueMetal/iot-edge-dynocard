# Microsoft

# Oil & Gas

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/oil%26gas-userguide.png)

**Table of Contents** 

- [1 User Guide](#1-user-guide)
- [2 Update the MLAlert Container Image in Deployment.json](#2-update-the-mlalert-container-image-in-deployment.json)
    - [2.1 Login to the IoT Edge Virtual Machine](#21-login-to-the-iot-edge-virtual-machine)
- [3 Running Modbus Simulator](#3-running-modbus-simulator)
    - [3.1 Check Modbus Logs](#31-check-modbus-logs)
- [4 Verify Data in Azure SQL Database](#4-verify-data-in-azure-sql-database)
    - [4.1 Login to the SQL server to check the data](#41-login-to-the-sql-server-to-check-the-data)
    - [4.2 Login to Edge SQL Container Server to check the data](#42-Login-to-edge-sql-container-server-to-check-the-data)
- [5 Configuring Power BI](#5-configuring-power-bi)
    - [5.1 Download Power Bi Desktop app](#51-download-power-bi-desktop-app)
    - [5.2 Download dynoCardVisuals.pbiviz](#52-download-dynocardvisuals.pbiviz)
    - [5.3 Import DynoCard custom from visuals](#53-import-dynoCard-custom-from-visuals)
    - [5.4 Generating and publishing Power BI reports](#54-generating-and-publishing-power-bi-reports)
    - [5.5 Power BI report](#55-power-bi-report)

## 1. User Guide

This Document explains how to use the Dyno Card solution, specifically it explains  generate data from the Simulator and how to verify the output of the project/solution.

## 2. Update the MLAlert Container Image in Deployment.json

1. Go to the deployed resource group, click on **Container registry**, In container registry overview click on Access Keys. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml1.png)

2. Copy and note the login server name, username, password in notepad will use those values later.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml2.png)

3. Now click on Machine learning service workspace resource, then click on **images** options in over view.In images click on anomaly-svc image name.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml3.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml4.png)

4. Copy and note the location value in notepad.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml5.png)

## 2.1 Login to IoT edge Virtual Machine

1. Go to the **resource group** and click on **IoT Edge Virtual Machine**.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml6.png)

2. Copy the **public IP** address.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml7.png)

3. Open **putty** and paste the public IP address, Click on **Open**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml8.png)

4. Click yes on security alert popup and Provide user name and password. User Name : **adminuser** Password : **Password@1234**

**Note**:Credentials might vary depends on deployment.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml9.png)

5. After login edit the deployment.json file by below command.

**vim deployment.json**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml10.png)

6. In **modulesContent** update the container registry values username ,password and address with acr login server name which you copied in notepad before. 

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml11.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml12.png)

7. In line number 67, **mlAlertModule** update the image value with copied location value from machine learning workspace before.

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml13.png)

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml14.png)

8. After that Save the file and run the below command to restart the iotedge service.

**sudo systemctl restart iotedge**

![alt text](https://github.com/nvtuluva/iot-edge-dynocard/blob/master/images/ml15.png)

## 3.Running Modbus Simulator

1. Go to **Resource Group** -> Click on **dynocardVM**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/71.png)

2. Now It will direct to **overview** page of the **dynocardVM**. 

3. **Copy** the **public IP address**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/72.png)

4. Open **Remote Desktop Connection** on your local computer and **paste** the copied **dynocardVM** Public IP address.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/73.png)

5. Use the below Credentials for login to the VM.

   User_ID: **adminuser**

   Password: **Password@1234**

6. Click **Ok**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/74.png)

7. Now Click **Yes** button to continue.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/75.png)

8. It should open the dynocardVM. Open a Windows Explorer window and check to make sure the downloaded software is present.

**Note**: Path to check the downloaded Softwares is “C:\”   (Click on **This PC** and click **C** drive) 

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/76.png)

**Note**:  Before Running the Modbus Simulator, you need to **stop** the **Windows Firewall**.

9. Go to **Control panel** -> **Network and Internet** -> **Network and Sharing Center**  window and then click on **Windows Firewall** which is located in the left bottom of the pane.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/77.png)

10. Click **Turn Windows Firewall on or off**, which is located in the left side Menu bar.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/78.png)

11. When you click  **Turn Windows Firewall on or off**, you will be directed to below page.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/79.png)

12. Now select **Turn off windows firewall** radio button for both public and private network settings then click **ok**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/80.png)

13. Now you can see, the Windows Firewalls of dynocardVM are stopped.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/81.png)

14. Run the 2nd version of **ModRSsim2** simulator by double clicking it.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/82.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/83.png)

15. Click on **Animation setting** icon, which is in the right-side menu bar.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/84.png)

16. Select the **"Training PLC Simulation"** option and browse the **DynoCard.vbs** script in downloaded section of the VM.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/85.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/86.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/87.png)

17. Select **Analog Inputs 300000**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/88.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/89.png)

18. Now Close the Simulator window and then open the version 1 of the Simulator, which should be installed in the  **C:\Program Files (x86)\EmbeddedIntelligence\Mod_RSsim** directory.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/90.png)

19. Click on the same **Animation setting** icon, which is located in right side menu bar to verify the **DynoCard.vbs** script is selected or not.

20. if it’s not selected, then Select the **"Training PLC Simulation"**  option and browse the **DynoCard.vbs** script in downloaded section of the VM.

21. Now Select **Analog Inputs 300000**  then you can observe the received/sent data count which is highlighted in below screen. Data from dynoCardVM will be sent to iotedge device's modbus module.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/91.png)

## 3.1. Check modbus logs

**Login to iotEdge VM**:

1. Go to -> **Resource group** -> Click on **iotEdge** VM

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/92.png)

2. After clicking on **iotEdge VM**, you will be directed to **iotEdge overview** page. Here Copy the **iotEdge VM Public IP address**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/93.png)

3. Now Open  **Putty** and **Paste** the  **Public IP addres** of  **iotEdge** VM into the **Host Name (or IP address)** box and then click **Open**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/94.png)

4. Then click **Yes** to continue.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/95.png)

5. Enter the Login Credentials:

   Username as: **adminuser**

   Password as: **Password@1234**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/96.png)

6. Once login is successful. Run the below command for root privilege.

**sudo -i**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/97.png)

7. Run the below command to check whether the data is coming from Modbus simulator or not.

**docker logs modbus**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/98.png)

8. Modbus container logs show the data that is coming from modbus simulator and minimize the screen.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/99.png)

9. To verify whether all modules on the edge device are running or not, go to **Resource group** -> Click on **iothub26hs3**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/01.png)

10. Navigate to **IoT Edge** blade under the **Automatic Device Management** section.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/iotedge.png)

11. Here we created and configured device from the **IoT Edge** VM. Click on **iot-dynocard-demo-device_1** device to see the modules.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/iotedge.png)

12. We can see the created **modules** in **device details**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/modules-running.png)

## 4.  Verify Data in Azure SQL Database

## 4.1.   Login to the SQL server to check the data.

1. Go to **Resource group** -> Click on **db4cards** SQL database and copy the server name.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/103.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/104.png)

2. Download and Install the SQL Server Management Studio on your local machine. Open SQL Server Management studio and past the server URL.

**Credentials:**

  Login_ID: **adminuser**

  Password: **Password@1234**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/oil-a2.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/105.png)

**Note**: Before login to SQL database you need to add firewall settings.

3. Go to **Resource Group** ->click on **oilgasserver26hrs3**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/106.png)

4. Click **Show Firewall Settings** from the **Firewalls and virtual networks**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/107.png)

5. Click **+ Add client IP** to add the client IP address.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/108.png)

6. As shown below added the **client ip address** form the **Firewall and virtual networks** and click **Save**.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/109.png)

7. Go back to **SQL Server Management Studio** and click on connect.

8. After connecting to the server, expand the tables and views inside the **db4cards** database as shown below.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/110.png)

9. Check the data in each table by running the following SQL Commands.

**select * From [ACTIVE].[CARD_DETAIL]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/111.png)

**select * From [ACTIVE].[CARD_HEADER]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/112.png)

**select * From [ACTIVE].[DYNO_CARD]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/113.png)

**select * From [ACTIVE].[EVENT]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/114.png)

**select * From [ACTIVE].[EVENT_DETAIL]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/115.png)

**select * From [ACTIVE].[PUMP]**

**select * From [STAGE].[PUMP_DATA]**

**select * From [STAGE].[SURFACE_DATA]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/116.png)

10. Select data from **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

To check the data in ACTIVE.DYNO_CARD_ANOMALY View follow the steps below.

Right click on **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW] -->** script view as -->select to -->New Query Editor window.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/117.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/118.png)

## 4.2. Login to Edge sql container server to check the data

1. Login to **edge sql Container** using **SQL Server Management Studio.**

2. Copy the **Edge VM IP Address:[104.42.121.3]** and give port **1401**.

**Credentials:**

  EX: **104.42.121.3,1401**

  User_ID: **sa**

  Password: **Strong!Passw0rd**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/119.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/120.png)

3. Check the data in tables.

**select * From [ACTIVE].[CARD_DETAIL]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/121.png)

**select * from [ACTIVE].[CARD_HEADER]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/122.png)

**select * from [ACTIVE].[DYNO_CARD]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/123.png)

**select * from [ACTIVE].[EVENT]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/124.png)

**select * from [ACTIVE].[EVENT_DETAIL]**

**select * from [ACTIVE].[PUMP]**

**select * from [STAGE].[PUMP_DATA]**

**select * from [STAGE].[SURFACE_DATA]**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/125.png)

4. Select data from **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

Right click on **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]** --> script view as -->select to -->New Query Editor window.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/126.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/127.png)

## 5.Configuring Power BI

Before configuring Power BI, you need to download and install the power BI Desktop application.

You can perform the below steps in Power BI Desktop:

### 5.1. Download Power Bi Desktop App 

Download Power Bi desktop from below link.

https://www.microsoft.com/en-us/download/details.aspx?id=45331

### 5.2. Download dynoCardVisuals.pbiviz

Download dynoCardVisuals.pbiviz file from the below link

https://github.com/BlueMetal/iot-edge-dynocard/tree/master/dynoCardVisuals/dist 

### 5.3. Import DynoCard custom from visuals 

1. Open Power BI Desktop and select the ellipses from the bottom of the Visualizations pane.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/142.png)

2. From the drop-down, select Import from file.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/143.png)

3. **Caution: Import Custom visual** --> dialog box will appear. Click Import to confirm.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/144.png)

4. From the Open file menu navigate to **dynoCardVisuals.pbiviz** file and then select Open. The icon for Dyno Card custom visual will be added to the bottom of Visualizations pane and it's now available for use.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/145.png)

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/146.png)

### 5.4. Generating and publishing Power BI reports

1. In the top left-hand side of **Power BI Desktop**, select **'Get Data'** -> **'SQL Server database'** and click on **Connect.**

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/147.png)

2. Fill in the **database server** information and click 'OK'. 

To get the database information, go to **Resource group**. 

Click on **db4cards** database

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/oil-a4.png)

Copy the **server** name and **database** name

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/oil-a5.png)

•   Server: xxxx.database.windows.net

•   Database: db4cards

•   Database Connectivity Mode: Import

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/148.png)

3. Select **'Database'** on the left-hand side of the connection window, fill in the database connection information and click **'Connect'**.

    •   Username: <<user name>>
    •   Password: <<password>>

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/149.png)

4. Select **'ACTIVE.DYNO_CARD_ANOMALY_VIEW'** from the navigation list and **Click** load.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/150.png)

5. Add the **Dynocard Visualization** to the report by clicking on the icon and then map the imported fields accordingly. 

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/151.png)

6. Drag and drop all the fields respectively.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/152.png)

## 5.5.  Power BI report 

Now reports can be observed like below.

![alt text](https://github.com/BlueMetal/iot-edge-dynocard/blob/master/images/153.png)


********************************* END OF DOCUMENT ***************************************
