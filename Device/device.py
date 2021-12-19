import os
import asyncio
import json
import time

from azure.iot.device import ProvisioningDeviceClient
from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import Message

from datetime import datetime
import random

provisioning_host = os.getenv("PROVISIONING_HOST")
id_scope = os.getenv("PROVISIONING_IDSCOPE")

device_id = os.getenv("DEVICE_ID")
derivied_device_key = os.getenv("DEVICE_KEY")

def register_device(registration_id):

    provisioning_device_client = ProvisioningDeviceClient.create_from_symmetric_key(
        provisioning_host=provisioning_host,
        registration_id=registration_id,
        id_scope=id_scope,
        symmetric_key=derivied_device_key,
    )

    return provisioning_device_client.register()

def generateNewData(lastData):
    gain = random.randrange(-100,100)/100

    print(f'gain : {gain}')

    newData = lastData + gain

    if newData < 0 or newData > 100:
        return 1.0
    else:
        lastData = newData
        return lastData

async def main():

    registration_result = register_device(device_id)

    if registration_result.status == "assigned":
        print("Will send telemetry from the provisioned device with id {id}".format(id=device_id))
        device_client = IoTHubDeviceClient.create_from_symmetric_key(
            symmetric_key=derivied_device_key,
            hostname=registration_result.registration_state.assigned_hub,
            device_id=registration_result.registration_state.device_id,
        )

        # Connect the client.
        await device_client.connect()

        last_measurement = 10.0

        while True:
            
            data = dict()

                
            data['measurement'] = generateNewData(last_measurement)
            data['adress'] = device_id 
            data['timestamp'] = datetime.now().strftime('%m/%d/%Y, %H:%M:%S')

            data_json = json.dumps(data)

            print(f'Sending message : {data_json}')
            msg = Message(data_json)
            msg.content_type = "application/json"

            await device_client.send_message(msg)

            time.sleep(5)

    else:
        print(
            "Can not send telemetry from the provisioned device with id {id}".format(id=device_id)
        )


if __name__ == "__main__":
    asyncio.run(main())