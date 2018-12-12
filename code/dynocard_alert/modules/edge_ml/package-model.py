from azureml.core.image import Image, ContainerImage
from azureml.core.model import Model
from azureml.core  import Workspace

#Load existing workspace from the config file info.
ws  = Workspace.from_config()

model = Model.register(model_path="model4dc.pkl",    # this path points to the local file
                       model_name="dynocard-model",  # the model gets registered as this name
                       tags={'classification': "myclassification"},
                       description="Dynocard anomaly detection model",
                       workspace = ws)


model_name = "dynocard-model"
model = Model(ws, model_name)


#Image configuration
image_config = ContainerImage.image_configuration(runtime="python",
                                                  execution_script="score4dc.py",
                                                  conda_file="myenv.yml",
                                                  description="dynocard-anomaly-model-image"
                                                  )

image = ContainerImage.create(name="dynocard-image",
                              models=[model],  # this is the model object
                              image_config=image_config,
                              workspace=ws
                              )
