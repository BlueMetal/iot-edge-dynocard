from azureml.core  import Workspace

#Load existing workspace from the config file info.
ws  = Workspace.from_config()

from azureml.core.model import Model
model = Model.register(model_path="model4dc.pkl",  # this path points to the local file
                       model_name="anomaly_detect",  # the model gets registered as this name
                       description="Dynocard anomaly detection",
                       workspace=ws)

from azureml.core.model import Model

model_name = "anomaly_detect"
model = Model(ws, model_name)

from azureml.core.image import Image, ContainerImage

#Image configuration
image_config = ContainerImage.image_configuration(runtime="python",
                                                  execution_script="score4dc.py",
                                                  description="Dynocard anomaly detection",
                                                  dependencies=["inputs.json", "model4dc.pkl", "myenv.yml", "service_schema.json", "train4dc.py"])

image = ContainerImage.create(name="iot_dynocard_demo_edgeml",
                              models=[model],  # this is the model object
                              image_config=image_config,
                              workspace=ws)
