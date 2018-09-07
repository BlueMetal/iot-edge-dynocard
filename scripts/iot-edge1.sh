#!/bin/bash
UserName=$1 
IoThubName=$2 
DeviceUrl=$3
DeviceConfig=$4
DeviceName=$5
primarykey=$6
# dnsname=$7
semcol=';'
comma=','
connectionString='HostName='$IoThubName.azure-devices.net$semcol'SharedAccessKeyName=iothubowner'$semcol'SharedAccessKey='$primarykey
# modbusPip=`nslookup $dnsname| awk '/^Address: / { print $2 }'`
#Install Python and Docker
echo $connectionString
sudo apt-get update
sudo apt install -y python-pip
sudo apt-get update
sudo apt install -y docker.io
sudo apt-get update
#Install Azure-CLI
AZ_REPO=$(lsb_release -cs)
echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | sudo tee /etc/apt/sources.list.d/azure-cli.list
curl -L https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
sudo apt-get update
sudo apt-get install apt-transport-https -y
sudo apt-get update && sudo apt-get install azure-cli -y
az extension add --name azure-cli-iot-ext
cd /home/$UserName/
wget https://packages.microsoft.com/repos/azure-cli/pool/main/a/azure-cli/azure-cli_2.0.32-1~wheezy_all.deb
sleep 1m
sudo dpkg -i azure-cli_2.0.32-1~wheezy_all.deb
sleep 2m
chmod 777 azure-cli_2.0.32-1~wheezy_all.deb
# Create IoT hub Edge Device
wget $DeviceUrl
wget $DeviceConfig
mv edge_device_config.json* edge_device_config.json
# TAB='\t    '
# sed -i -e "s/.*SlaveConnection.*/${TAB}\"SlaveConnection\": \"$modbusPip\"$comma/" edge_device_config.json
az iot hub apply-configuration --hub-name $IoThubName --device-id $DeviceName -l $connectionString -k edge_device_config.json
sleep 20s
connstr=`az iot hub device-identity show-connection-string --hub-name $IoThubName --device-id $DeviceName -l $connectionString | grep '"cs":'| cut -d ":" -f2 | tr -d "\"," |tr -d " "`
sleep 20s
sudo pip install idna\<2.6
sleep 10s
export HOME=/root
sudo apt-get update
sudo pip install -U azure-iot-edge-runtime-ctl
sleep 10s
iotedgectl setup --connection-string "${connstr}" --auto-cert-gen-force-no-passwords
iotedgectl start
