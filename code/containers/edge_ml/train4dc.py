# ******************************************************
# *
# * Name:         train4dc.py
# *     
# * Design Phase:
# *     Author:   BlueMetal, Inc.
# *     Date:     04-04-2018
# *     Purpose:  
# *               Save model that solves known data 
# *               science problem.  
# *     Notes:
# *               Run via AML Workbench.
# *               Download model4dc.pkl into local dir.
# * 
# ******************************************************


# Support Vector Machines for supervised learning
# http://scikit-learn.org/stable/modules/svm.html


# import libaries
from sklearn.svm import SVC
import pickle

# features - height, width, and shoe size
X = [[181, 80, 44], [177, 70, 43], [160, 60, 38], [154, 54, 37], [166, 65, 40], [190, 90, 47], [175, 64, 39],
     [177, 70, 40], [159, 55, 37], [171, 75, 42], [181, 85, 43]]

# category - male | female
Y = ['male', 'male', 'female', 'female', 'male', 'male', 'female', 'female', 'female', 'male', 'male']

# classify the data
clf = SVC()
clf = clf.fit(X, Y)

# predict a value & show accuracy
X_old = [[190, 70, 43]]
print('Old Sample:', X_old)
print('Predicted value:', clf.predict(X_old))
print('Accuracy', clf.score(X,Y))

# create the outputs folder
os.makedirs('./outputs', exist_ok=True)

# export model
print('Export the model to model4dc.pkl')
f = open('./outputs/model4dc.pkl', 'wb')
pickle.dump(clf, f)
f.close()

# import model
print('')
print('Import the model from model4dc.pkl')
f2 = open('./outputs/model4dc.pkl', 'rb')
clf2 = pickle.load(f2)

# predict new value
X_new = [[154, 54, 35]]
print('New Sample:', X_new)
print('Predicted class:', clf2.predict(X_new))