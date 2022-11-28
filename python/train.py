import pandas
from sklearn import model_selection
from sklearn.neighbors import KNeighborsClassifier
import pickle
from sklearn.pipeline import make_pipeline
from sklearn.preprocessing import StandardScaler
import sys

dataframe = pandas.read_csv(sys.argv[1], sep=',', header=None)
array = dataframe.values
X = array[:,0:8]
Y = array[:,8]
X_train, X_test, Y_train, Y_test = model_selection.train_test_split(X, Y, test_size=0.2, random_state=2)

pipe = make_pipeline(StandardScaler(), KNeighborsClassifier())
pipe.fit(X_train, Y_train)

result = pipe.score(X_test, Y_test)
print(result)

pickle.dump(pipe, open(sys.argv[2], 'wb'))