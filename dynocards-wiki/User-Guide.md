## 1. User Guide
This Document explains how to use the solution. The document explains about how to generate data from Simulator and how to verify output of the project.

## 2.Running Modbus Simulator

1. Go to **Resource Group** -> Click on **dynocardVM**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/71.png)

2. Now It will direct to **overview** page of the **dynocardVM**. 

3. **Copy** the **public IP address**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/72.png)

4. Open **Remote Desktop Connection** in your local computer and **paste** the copied **dynocardVM** Public IP address.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/73.png)

5. Use the below Credentials for login to the VM.

   User_ID: **adminuser**

   Password: **Password@1234**

6. Then Click **Ok**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/74.png)

7. Now Click **Yes** button to continue.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/75.png)

8. It should open the dynocardVM and check the downloaded software.

**Note**: Path to check the downloaded Softwares is “C:\”   (Click on **This PC** and click **C** drive) 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/76.png)

**Note**:  Before Running the Modbus Simulator, you need to **stop** the **Windows Firewall**.

9. Go to **Control panel** -> **Network and Internet** -> **Network and Sharing Center**  window then click **Windows Firewall** which is located at the left bottom of the pane

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/77.png)

10. Then click **Turn Windows Firewall on or off**, which is located at the left side Menu bar.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/78.png)

11. When you click  **Turn Windows Firewall on or off**, you will be directed to below page.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/79.png)

12. Now select **Turn off windows firewall** radio button for both public and private network settings then click **ok**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/80.png)

13. Now you can see, the Windows Firewalls of dynocardVM are stopped.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/81.png)

14. Now Run the v2 version of **ModRSsim2** simulator by double clicking it.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/82.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/83.png)

15. Click on **Animation setting** icon, which is located at right top menu bar.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/84.png)

16. Select the **"Training PLC Simulation"** option and browse the **DynoCard.vbs** script in downloaded section of the VM.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/85.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/86.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/87.png)

17. Select **Analog Inputs 300000**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/88.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/89.png)

18. Now Close the v2 simulator window and then open the v1 simulator, which should be installed in the **C:\Program Files (x86)\EmbeddedIntelligence\Mod_RSsim** directory.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/90.png)

19. Click on the same **Animation setting** icon, which is located at right top menu bar to verify the DynoCard.vbs script is selected or not.

20. if it’s not selected then Select the **"Training PLC Simulation"**  option and browse the DynoCard.vbs script in downloaded section of the VM.

21. Now Select **Analog Inputs 300000**  then you can observe the received/sent messages count. Which is highlighted in below screen.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/91.png)

## 2.1. Check modbus logs

**Login to iotEdge VM**:

1. Go to -> **Resource group** -> Click on **iotEdge** VM

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/92.png)

2. After clicking on **iotEdge VM**, you will be directed to **iotEdge overview** page. Here Copy the **iotEdge VM Public IP address**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/93.png)

3. Now Open  **Putty** and **Paste** the  **Public IP addres** of  **iotEdge** VM at **Host Name (or IP address)** then click **Open**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/94.png)

4. Then click **Yes** to continue.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/95.png)

5. Enter the Login Credentials:

   Username as: **adminuser**

   Password as: **Password@1234**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/96.png)

6. Once login successful. Run the below command for root privilege.

**sudo -i**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/97.png)

7. Run the below command to check whether the data is coming from modbus simulator or not.

**docker logs modbus**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/98.png)

8. Modbus container logs show the data that is coming from modbus simulator.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/99.png)

9. Go to **Resource group** -> Click on **iothub26hs3**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/01.png)

10. Navigate to **IoT Edge** blade under the **Automatic Device Management** section.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/02.png)

11. Here we created and configured device from the **IoT Edge** VM. Click on **iot-dynocard-demo-device_1** device to see the modules.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/03.png)

12. We can see the created modules in device details 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/04.png)

## 3.  Verify Data in Azure SQL Database

## 3.1.   Login to the SQL server to check the data.

1. Go to **Resource group** -> Click on **db4cards** SQL database and copy the server name.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/103.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/104.png)

2. Open SQL Server Management studio and paste the server URL.

**Credentials:**

  Login_ID: **adminuser**

  Password: **Password@1234**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/105.png)

**Note**: Before login to SQL database you need to add firewall settings.

3. Go to **resource Group** ->click on **oilgasserver26hrs3**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/106.png)

4. Click **Show Firewall Settings** from the **Firewalls and virtual networks**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/107.png)

5. Click **+ Add client IP** to add the client ip address.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/108.png)

6. As shown below added the **client ip address** form the **Firewall and virtual networks** and click **save**.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/109.png)

7. Go back to **SQL Server Management Studio** and click on connect.

8. After connecting to the server, expand the tables and views inside the **db4cards** database as shown below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/110.png)

9. Check the data in each table.

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

10. Select data from **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

Right click on [ACTIVE].[DYNO_CARD_ANOMALY_VIEW] --> script view as -->select to -->New Query Editor window.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/117.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/118.png)

## 3.2. Login to Edge sql container server to check the data

1. Login to **edge sql Container** using **SQL Server management studio.**

2. Copy the Edge VM IP Address:**[104.42.121.3]** and give port **1401**.

**Credentials:**

  EX: **104.42.121.3,1401**

  User_ID: **sa**

  Password: **Strong!Passw0rd**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/119.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/120.png)

3. Check the data in tables.

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

4. Select data **from [ACTIVE].[DYNO_CARD_ANOMALY_VIEW]**

Right click on **[ACTIVE].[DYNO_CARD_ANOMALY_VIEW]** --> script view as -->select to -->New Query Editor window.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/126.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/127.png)

## 4.Configuring Power BI

Before configuring Power BI, you need to download and install the power BI Desktop application.
You can perform the below steps in Power BI Desktop:

## 4.1. Download Power Bi Desktop app 

Download Power Bi desktop from below link.

https://www.microsoft.com/en-us/download/details.aspx?id=45331

## 4.2. Download dynoCardVisuals.pbiviz

Download dynoCardVisuals.pbiviz file from the below link

https://github.com/BlueMetal/iot-edge-dynocard/tree/master/dynoCardVisuals/dist 

## 4.3. Import DynoCard custom from visuals 

1. Open Power BI Desktop and select the ellipses from the bottom of the Visualizations pane.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/142.png)

2. From the drop-down, select Import from file.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/143.png)

3. **Caution: Import Custom visual** --> dialog box will appear. Click Import to confirm.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/144.png)

4. From the Open file menu navigate to **dynoCardVisuals.pbiviz** file and then select Open. The icon for Dyno Card custom visual will be added to the bottom of Visualizations pane and it's now available for use.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/145.png)

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/146.png)

## 4.4. Generating and publishing Power BI reports

1. In the top left-hand side of **Power BI Desktop**, select **'Get Data'** -> **'SQL Server database'** and click on **Connect.**

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/147.png)

2. Fill in the **database server** information and click 'OK'. 

•   Server: xxxx.database.windows.net

•   Database: db4cards

•   Database Connectivity Mode: Import


![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/148.png)

3. Select **'Database'** on the left-hand side of the connection window, fill in the database connection information and click **'Connect'.**

    •   Username: <<user name>>
    •   Password: <<password>>

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/149.png)

4. Select **'ACTIVE.DYNO_CARD_ANOMALY_VIEW'** from the navigation list and **click** load.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/150.png)

5. Add the **Dynocard Visualization** to the report by clicking on the icon and then map the imported fields accordingly. 

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/151.png)

6. Drag and drop all the fields respectively.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/152.png)

## 4.5.  Power BI report 

Now reports can be observed like below.

![alt text](https://github.com/sysgain/iot-edge-dynocard/blob/master/images/153.png)

