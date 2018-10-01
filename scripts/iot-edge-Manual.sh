#!/bin/sh
UserName=""
IoThubName=""
DeviceUrl=""
DeviceConfig=""
DeviceName=""
primarykey=""
dnsname=""
semcol=';'
comma=','
RED='\033[0;31m'
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m'
connectionString='HostName='$IoThubName.azure-devices.net$semcol'SharedAccessKeyName=iothubowner'$semcol'SharedAccessKey='$primarykey
echo "${GREEN} IoThub Connection String is:" $connectionString ${NC}
modbusPip=`nslookup $dnsname| awk '/^Address: / { print $2 }'`
echo "${GREEN} Public ip of Simulator is:" $modbusPip ${NC}
#Run Updates
sudo apt-get update
#Change Directory to user home directory

cd /home/$UserName/
#Verify Python-pip is installed on not. if not installed, install python-pip.
if ! [ -f "/usr/bin/pip" ]; then
    echo "${RED} Error: python-pip is not installed. Installing Python-pip....${NC}" >&2
  	sudo apt-get update
  	sudo apt install -y python-pip
else
	  echo "${GREEN} python-pip is already installed${NC}"
fi

#Verify docker is installed on not. if not installed, install docker.
if ! [ -x "$(command -v docker)" ]; then
  	echo "${RED} Error: Docker is not installed.Installing Docker.... ${NC}" >&2
  	sudo apt-get update
	  sudo apt-get install -y apt-transport-https ca-certificates curl software-properties-common
	  curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
	  sudo apt-key fingerprint 0EBFCD88
	  sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
	  sudo apt-get update
	  sudo apt-get install -y docker-ce
else
	  echo "${GREEN} Docker is already installed ${NC}"
fi

#Verify az cli is installed on not. if not installed, install az cli.
if ! [ -x "$(command -v az)" ]; then
    echo "${RED} Error: az cli is not installed.Installing az cli.... ${NC}" >&2
  	sudo apt-get update
  	AZ_REPO=$(lsb_release -cs)
	  echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | sudo tee /etc/apt/sources.list.d/azure-cli.list
	  curl -L https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
	  sudo apt-get update
	  sudo apt-get install apt-transport-https -y
	  sudo apt-get update && sudo apt-get install azure-cli -y
	  az extension add --name azure-cli-iot-ext
	  wget https://packages.microsoft.com/repos/azure-cli/pool/main/a/azure-cli/azure-cli_2.0.32-1~wheezy_all.deb
	  sleep 1m
	  sudo dpkg -i azure-cli_2.0.32-1~wheezy_all.deb
	  sleep 2m
	  chmod 777 azure-cli_2.0.32-1~wheezy_all.deb
else
	  echo "${GREEN} az cli is already installed ${NC}"
fi

if ! [ -f /home/$UserName/edge_device_config.json ]; then
    echo "${RED} Error: edge device config file is not Found. Downloading edge device config file....${NC}" >&2
    wget $DeviceConfig
    mv edge_device_config.json* edge_device_config.json
    TAB='\t    '
	  comma=','
    sed -i -e "s/.*SlaveConnection.*/${TAB}\"SlaveConnection\": \"$modbusPip\"$comma/" edge_device_config.json
else
	  echo "${GREEN} edge_device_config.json file is already exists${NC}"
fi

#Verify Edge Device is created or not. If not created create one.
exstdev=`az iot hub device-identity list -ee -l $connectionString | grep '"deviceId":' | cut -d ":" -f2 | tr -d "\"," |tr -d " "`
echo ${BLUE} "Device name which needs to be created is " $DeviceName ${NC}
sleep 10
if [ "$exstdev" = "$DeviceName" ];
then
	  echo ${BLUE} $DeviceName "is already exists in" $IoThubName ${NC}
else
	  echo "${GREEN} Creating" $DeviceName "Edge Device" ${NC}
	  wget $DeviceUrl
    az iot hub apply-configuration --hub-name $IoThubName --device-id $DeviceName -l $connectionString -k edge_device_config.json
    sleep 20s
fi

# Get IoT hub Edge Device Connection String
connstr=`az iot hub device-identity show-connection-string --hub-name $IoThubName --device-id $DeviceName -l $connectionString | grep '"cs":'| cut -d ":" -f2 | tr -d "\"," |tr -d " "`
sleep 20s
#Install idna
sudo pip install idna\<2.6
sleep 10s
#Setup iotedgectl Runtime controller
export HOME=/root

#Verify iotedge run time controller is installed or not. if not installed, install iotedge run time controller.
if ! [ -d "/etc/azure-iot-edge" ]; then
  	echo "${RED} Error: azure-iot-edge-runtime-ctl is not installed. Installing azure-iot-edge-runtime-ctl....${NC}" >&2
  	sudo apt-get update
  	sudo pip install -U azure-iot-edge-runtime-ctl
	  sleep 10s
	  iotedgectl setup --connection-string "${connstr}" --auto-cert-gen-force-no-passwords
	  iotedgectl start
else
	  echo "${RED} Error: azure-iot-edge-runtime-ctl is already installed. UnInstalling azure-iot-edge-runtime-ctl.... ${NC}" 
	  iotedgectl uninstall
    rm -r /etc/azure-iot-edge
	  sleep 20
    echo "${GREEN} azure-iot-edge-runtime-ctl uninstalled successfully. Installing azure-iot-edge-runtime-ctl....${NC}"
	  sudo apt-get update
  	sudo pip install -U azure-iot-edge-runtime-ctl
	  sleep 10s
	  iotedgectl setup --connection-string "${connstr}" --auto-cert-gen-force-no-passwords
	  iotedgectl start
	  echo "${GREEN} iotedgectl Setup Completed Successfully, Wait for some time to comeup docker Contianers Status to running${NC}"
fi
