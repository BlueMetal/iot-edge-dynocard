#!/bin/bash
UserName=$1 
IoThubName=$2 
DeviceUrl=$3
DeviceConfig=$4
DeviceName=$5
primarykey=$6
dnsname=$7
azureLogin=$8
azurePassword=$9
tenantId=$10
subscriptionID=$11
semcol=';'
comma=','
HubConnStr='HostName='$IoThubName.azure-devices.net$semcol'SharedAccessKeyName=iothubowner'$semcol'SharedAccessKey='$primarykey
#modbusPip=`nslookup $dnsname| awk '/^Address: / { print $r2 }'`
modbusPip=`getent hosts $dnsname | awk '{ print $1 }'`
#Create IoT hub Edge Device
wget $DeviceUrl
sleep 20s
#Install azure CLI
AZ_REPO=$(lsb_release -cs)
echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | sudo tee /etc/apt/sources.list.d/azure-cli.list
curl -L https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo apt-get update
sleep 30s
sudo apt-get install apt-transport-https -y
sudo apt-get update && sudo apt-get install azure-cli -y
sleep 1m
az extension add --name azure-cli-iot-ext
EdgeConnStr=`az iot hub device-identity show-connection-string --hub-name $IoThubName --device-id $DeviceName -l $HubConnStr | grep '"connectionString":'| cut -d ":" -f2 | tr -d "\"," |tr -d " "`
echo $EdgeConnStr > /tmp/EdgeConnstr.txt
#install IoT Edge runtime with GA bits
curl https://packages.microsoft.com/config/ubuntu/16.04/prod.list > ./microsoft-prod.list
sudo cp ./microsoft-prod.list /etc/apt/sources.list.d/
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo cp ./microsoft.gpg /etc/apt/trusted.gpg.d/
sudo apt-get update
sleep 30s
sudo apt-get install moby-engine -y
sudo apt-get install moby-cli
sudo apt-get update
sleep 30s
sudo apt-get install iotedge -y
sleep 1m
sudo chmod 777 /etc/iotedge/config.yaml
sudo sed -i "s|<ADD DEVICE CONNECTION STRING HERE>|${EdgeConnStr}|g" /etc/iotedge/config.yaml
sudo chmod 400 /etc/iotedge/config.yaml
sudo systemctl restart iotedge
sleep 1m
sudo systemctl status iotedge > /tmp/iotedgestatus.txt
#Update deployment.json file with modbus vm IP address
cd /home/$UserName/
wget $DeviceConfig
sleep 10s
mv deployment.json* deployment.json
TAB='\t    '
sed -i -e "s/.*SlaveConnection.*/${TAB}\"SlaveConnection\": \"$modbusPip\"$comma/" deployment.json
#Deploy IOT Edge Modules using Azure CLI
az login -t $tenantId -u $azureLogin -p $azurePassword
sleep 1m
az account set -s $subscriptionID
az account show>>log.txt
echo "------------------------------------------">>log.txt
az iot edge set-modules -d $DeviceName -n $IoThubName -k deployment.json>>log.txt
sleep 1m
#Disable Process Identification
docker0IP=`ifconfig docker0 | awk '{ print $2}' | grep -E -o "([0-9]{1,3}[\.]){3}[0-9]{1,3}"`
managementUri="http://$docker0IP:15580"
workloadUri="http://$docker0IP:15581"
export IOTEDGE_HOST="http://$docker0IP:15580"
sudo sed -i "s|unix:///var/run/iotedge/mgmt.sock|${managementUri}|g" /etc/iotedge/config.yaml
sudo sed -i "s|unix:///var/run/iotedge/workload.sock|${workloadUri}|g" /etc/iotedge/config.yaml
sudo sed -i "s|fd://iotedge.mgmt.socket|${managementUri}|g" /etc/iotedge/config.yaml
sudo sed -i "s|fd://iotedge.socket|${workloadUri}|g" /etc/iotedge/config.yaml
sudo systemctl restart iotedge
sleep 1m
