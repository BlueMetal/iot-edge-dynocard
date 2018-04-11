# ******************************************************
# *
# * Name:         score.py
# *     
# * Design Phase:
# *     Author:   BlueMetal, Inc.
# *     Date:     04-04-2018
# *     Purpose:  
# *               Read in a mod bus json document.
# *               Execute simular classification model.
# *               Randomly classify data.  
# *     Notes:
# *               Run via AML Workbench.
# *               Download service_schema.json into local dir.
# *               Optionally use data collector.
# * 
# ******************************************************


# Common modules
from azureml.datacollector import ModelDataCollector
from azureml.api.schema.dataTypes import DataTypes
from azureml.api.schema.sampleDefinition import SampleDefinition
from azureml.api.realtime.services import generate_schema


#
#  Init routine - Web service
#

def init():  

    # Load module
    from sklearn.externals import joblib

    # Variables
    global model
    
    # Load model
    model = joblib.load('model4dc.pkl')



#
#  Run routine - Web service
#

def run(input_str):

    # Load module
    import json

    # What type of input
    # print("Input_str: type")
    # print (type(input_str))
    # print ("")
    # print(input_str)

    # Convert to dictionary
    if type(input_str) is str:
        input = json.loads(input_str)
    else:
        input = input_str

    # Fake a prediction
    prediction = write_msg(input['Id'], input['Timestamp']);

    # Return json
    return prediction


#
#  Read Input Message Rountine - Read in mod bus sample message
#

def read_msg():

    # Load module
    import json

    # Create some json input
    in1 = ""
    in1 += '{ '
    in1 += '"Id": 0, '
    in1 += '"Timestamp": "2018-04-04T22:42:59+00:00", '
    in1 += '"NumberOfPoints": 400, '
    in1 += '"MaxLoad": 19500, '
    in1 += '"MinLoad": 7500, '
    in1 += '"StrokeLength": 1200, '
    in1 += '"StrokePeriod": 150, '
    in1 += '"CardType": 0, '
    in1 += '"CardPoints": [{ '
    in1 += '  "Load": 11744, '
    in1 += '  "Position": 145 }]'
    in1 += '} '

    # Return sample message
    return json.loads(in1)


#
#  Number 2 Class Rountine - Classify the anomaly.
#

def number_to_class(argument):
    switcher = {
        1: "Full Pump",
        2: "Flowing Well, Rod Part, Inoperative Pump",
        3: "Bent Barrel, Sticking Pump",
        4: "Pump Hitting Up or Down",
        5: "Fluid Friction",
        6: "Gas Interference",
        7: "Drag Friction",
        8: "Tube Movement",
        9: "Worn or Split Barrel",
        10: "Fluid Pound",
        11: "Worn Standing Value",
        12: "Worn Plunger or Traveling Value"
    };
    return switcher.get(argument, "Undefined");


#
#  Write Output Message Rountine - Randomly classify the data.
#

def write_msg(id, stamp):

    # Load module
    import json
    import random;

    # Five percent left tail
    occurs = 95

    # Grab a number 1.0 to 100.0
    pct1 = random.uniform(1, 100);

    # Create some json output
    out1 = ""
    out1 += '{ "Id": "' + str(id) + '", '
    out1 += '"Timestamp": "'+ stamp + '", '

    # Report a anomaly?
    if (pct1 >= occurs):
        out1 += '"Anomaly": "True", '
    else:
        out1 += '"Anomaly": "False", "Class": "Full Pump" }'

    # Choose random issue
    if (pct1 >= occurs):
        pct2 = int(random.uniform(1, 12)) + 1;
        out1 += '"Class": "' + number_to_class(pct2) + '" }'

    # Return sample message
    return json.loads(out1)


#
#  Main routine - Test init() & run()
#

def main():

    # Turn on data collection debug mode to view output in stdout
    os.environ["AML_MODEL_DC_DEBUG"] = 'false';
    os.environ["AML_MODEL_DC_STORAGE_ENABLED"] = 'false';

    # create the outputs folder
    os.makedirs('./outputs', exist_ok=True);

    # Read in json, mod bus sample msg
    input_msg = read_msg();

    # Debugging - remove when deploying
    #print (" ");
    #print ("Input Json:")
    #print (input_msg);

    # Test init function
    init();

    # Write out json, sample response msg
    output_msg = run(input_msg);

    # Debugging - remove when deploying
    #print (" ");
    #print ("Output Json:")
    print(output_msg);

    # Sample input string
    input_str = {"input_str": SampleDefinition(DataTypes.STANDARD, input_msg)};

    # Generate swagger document for web service
    generate_schema(run_func=run, inputs=input_str, filepath='./outputs/service_schema.json');


# Call main
if __name__ == "__main__":
    main()
